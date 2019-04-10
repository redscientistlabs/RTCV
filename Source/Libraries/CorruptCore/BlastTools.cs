using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using RTCV.CorruptCore;
using Newtonsoft.Json;
using RTCV.NetCore;


namespace RTCV.CorruptCore
{
	public static class BlastTools
	{
		public static string LastBlastLayerSavePath { get; set; }

		public static bool SaveBlastLayerToFile(BlastLayer bl, string path = null)
		{
			string filename = path;

			if (bl.Layer.Count == 0)
			{
				MessageBox.Show("Can't save because the provided blastlayer is empty is empty");
				return false;
			}

			if (filename == null)
			{
				SaveFileDialog saveFileDialog1 = new SaveFileDialog
				{
					DefaultExt = "bl",
					Title = "Save BlastLayer File",
					Filter = "bl files|*.bl",
					RestoreDirectory = true
				};

				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					filename = saveFileDialog1.FileName;
				}
				else
					return false;
			}
		

			using (FileStream fs = new FileStream(filename, FileMode.Create))
			{
				JsonHelper.Serialize(bl, fs, Formatting.Indented);
			}

			LastBlastLayerSavePath = filename;
			return true;
		}

		public static BlastLayer LoadBlastLayerFromFile(string filename = null)
		{
			BlastLayer bl = null;

			if (filename == null)
			{
				OpenFileDialog ofd = new OpenFileDialog
				{
					DefaultExt = "bl",
					Title = "Open BlastLayer File",
					Filter = "bl files|*.bl",
					RestoreDirectory = true
				};
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					filename = ofd.FileName.ToString();
				}
				else
					return null;
			}

			if (!File.Exists(filename))
			{
				MessageBox.Show("The BlastLayer file wasn't found");
				return null;
			}


			try
			{
				using (FileStream fs = new FileStream(filename, FileMode.Open))
				{
					bl = JsonHelper.Deserialize<BlastLayer>(fs);
					return bl;
				}
			}
			catch
			{
				MessageBox.Show("The BlastLayer file could not be loaded");
				return null;
			}
		}

		private static byte[] CapBlastUnit(byte[] input)
		{
			switch (input.Length)
			{
				case 1:
					if (CorruptCore_Extensions.GetDecimalValue(input, false) > Byte.MaxValue)
						return getByteArray(1, 0xFF);
					break;
				case 2:
					if (CorruptCore_Extensions.GetDecimalValue(input, false) > UInt16.MaxValue)
						return getByteArray(2, 0xFF);
					break;
				case 4:
					if (CorruptCore_Extensions.GetDecimalValue(input, false) > UInt32.MaxValue)
						return getByteArray(2, 0xFF);
					break;
			}
			return input;
		}

		private static byte[] getByteArray(int size, byte value)
		{
			var temp = new byte[size];
			for (int i = 0; i < temp.Length; i++)
			{
				temp[i] = value;
			}
			return temp;
		}

		public static BlastLayer GetAppliedBackupLayer(BlastLayer bl, StashKey sk)
		{
			//So basically due to how netcore handles synced commands, we can't actually call sk.Run()
			//from within emuhawk or else it'll apply the blastlayer AFTER this code completes
			//So we manually apply the blastlayer
			sk.RunOriginal();
			sk.BlastLayer.Apply(false);

			//Fake advance a frame here to get it processed and the values set
			StepActions.Execute();
			BlastLayer newBlastLayer = bl.GetBackup();
			//Clear it all out
			StepActions.ClearStepBlastUnits();
			return newBlastLayer;
		}

		public static BlastLayer GetBlastLayerFromDiff(byte[] Original, byte[] Corrupt)
		{
			BlastLayer bl = new BlastLayer();

			string thisSystem = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.SYSTEM.ToString()];
			string romFilename = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME.ToString()];

			var rp = MemoryDomains.GetRomParts(thisSystem, romFilename);

			if (rp.Error != null)
			{
				MessageBox.Show(rp.Error);
				return null;
			}

			if (Original.Length != Corrupt.Length)
			{
				MessageBox.Show("ERROR, ROM SIZE MISMATCH");
				return null;
			}

			MemoryInterface mi = MemoryDomains.GetInterface(rp.PrimaryDomain);
			long maxaddress = mi.Size;

			for (int i = 0; i < Original.Length; i++)
			{
				if (Original[i] != Corrupt[i] && i >= rp.SkipBytes)
				{
					if (i - rp.SkipBytes >= maxaddress)
						bl.Layer.Add(new BlastUnit(new byte[] { Corrupt[i] }, rp.SecondDomain, (i - rp.SkipBytes) - maxaddress, 1, mi.BigEndian));
					else
						bl.Layer.Add(new BlastUnit(new byte[] { Corrupt[i] }, rp.PrimaryDomain, (i - rp.SkipBytes), 1, mi.BigEndian));
				}
			}

			if (bl.Layer.Count == 0)
				return null;
			return bl;
		}

		public static List<BlastGeneratorProto> GenerateBlastLayersFromBlastGeneratorProtos(List<BlastGeneratorProto> blastLayers, StashKey sk, bool loadBeforeCorrupt)
		{
			//Load the game first for stuff like REPLACE_X_WITH_Y
			if (loadBeforeCorrupt)
				sk?.RunOriginal();
			foreach (BlastGeneratorProto bgp in blastLayers)
			{
				//Only generate if there's no BlastLayer.
				//A new proto is always generated if the cell is dirty which means no BlastLayer will exist
				//Otherwise, we just return the existing BlastLayer
				if (bgp?.bl == null)
				{
					Console.Write("BGP was dirty. Generating BlastLayer\n");
					bgp.bl = bgp.GenerateBlastLayer();
				}

			}

			return blastLayers;
		}
	}
}
