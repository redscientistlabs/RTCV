namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Xml.Serialization;
    using Ceras;
    using Newtonsoft.Json;
    using RTCV.NetCore;
    using Exception = System.Exception;

    [Ceras.MemberConfig(TargetMember.All)]
    [Serializable]
    [XmlInclude(typeof(BlastUnit))]
    public class BlastLayer : ICloneable, INote
    {
        public List<BlastUnit> Layer;

        public BlastLayer()
        {
            Layer = new List<BlastUnit>();
        }

        public BlastLayer(List<BlastUnit> _layer)
        {
            Layer = _layer;
        }

        public object Clone()
        {
            return ObjectCopierCeras.Clone(this);
        }

        public void Apply(bool storeUncorruptBackup, bool followMaximums = false, bool mergeWithPrevious = false)
        {
            if (storeUncorruptBackup && this != StockpileManager_EmuSide.UnCorruptBL)
            {
                BlastLayer UnCorruptBL_Backup = null;
                BlastLayer CorruptBL_Backup = null;

                if (mergeWithPrevious)
                {
                    //UnCorruptBL_Backup = (BlastLayer)StockpileManager_EmuSide.UnCorruptBL?.Clone();
                    //CorruptBL_Backup = (BlastLayer)StockpileManager_EmuSide.CorruptBL?.Clone();

                    UnCorruptBL_Backup = StockpileManager_EmuSide.UnCorruptBL;
                    CorruptBL_Backup = StockpileManager_EmuSide.CorruptBL;
                }

                StockpileManager_EmuSide.UnCorruptBL = GetBackup();
                StockpileManager_EmuSide.CorruptBL = this;

                if (mergeWithPrevious)
                {
                    if (UnCorruptBL_Backup.Layer != null)
                    {
                        if (StockpileManager_EmuSide.UnCorruptBL.Layer == null)
                            StockpileManager_EmuSide.UnCorruptBL.Layer = new List<BlastUnit>();

                        StockpileManager_EmuSide.UnCorruptBL.Layer.AddRange(UnCorruptBL_Backup.Layer);
                    }

                    if (CorruptBL_Backup.Layer != null)
                    {
                        if (StockpileManager_EmuSide.CorruptBL.Layer == null)
                            StockpileManager_EmuSide.CorruptBL.Layer = new List<BlastUnit>();

                        StockpileManager_EmuSide.CorruptBL.Layer.AddRange(CorruptBL_Backup.Layer);
                    }
                }
            }

            bool success;
            bool UseRealtime = (bool)AllSpec.VanguardSpec[VSPEC.SUPPORTS_REALTIME];

            try
            {
                foreach (BlastUnit bb in Layer)
                {
                    if (bb == null) //BlastCheat getBackup() always returns null so they can happen and they are valid
                    {
                        success = true;
                    }
                    else
                    {
                        success = bb.Apply(true);
                    }

                    if (!success)
                    {
                        throw new Exception(
                        "One of the BlastUnits in the BlastLayer failed to Apply().\n\n" +
                        "The operation was cancelled");
                    }
                }

                //Only filter if there are actually enabled units
                if (Layer.Any(x => x.IsEnabled))
                {
                    StepActions.FilterBuListCollection();

                    //If we're not using realtime, we execute right away.
                    if (!UseRealtime)
                    {
                        StepActions.Execute();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                            "An error occurred in RTC while applying a BlastLayer to the game.\n\n" +
                            "The operation was cancelled\n\n" + ex.Message
                            );
            }
            finally
            {
                if (followMaximums)
                {
                    StepActions.RemoveExcessInfiniteStepUnits();
                }
            }
        }

        public BlastLayer GetBakedLayer()
        {
            List<BlastUnit> BackupLayer = new List<BlastUnit>();

            BackupLayer.AddRange(Layer.Select(it => it.GetBakedUnit()));

            return new BlastLayer(BackupLayer);
        }

        public BlastLayer GetBackup()
        {
            List<BlastUnit> BackupLayer = new List<BlastUnit>();

            BackupLayer.AddRange(Layer.Select(it => it.GetBackup()).Where(it => it != null));

            return new BlastLayer(BackupLayer);
        }

        public void Reroll()
        {
            foreach (BlastUnit bu in Layer.Where(x => x.IsLocked == false))
            {
                bu.Reroll();
            }
        }

        public void RasterizeVMDs(string vmdToRasterize = null)
        {
            List<BlastUnit> l = new List<BlastUnit>();
            //Inserting turned out to be WAY too cpu intensive, so just sacrifice the ram and rebuild the layer
            foreach (var bu in Layer)
            {
                var u = bu.GetRasterizedUnits(vmdToRasterize);
                l.AddRange(u);
            }
            this.Layer = l;
        }

        private string shared = "[DIFFERENT]";

        [JsonIgnore]
        public string Note
        {
            get
            {
                if (Layer.All(x => x.Note == Layer.First().Note))
                {
                    return Layer.FirstOrDefault()?.Note;
                }
                return shared;
            }
            set
            {
                if (value == shared)
                {
                    return;
                }
                foreach (BlastUnit bu in Layer)
                {
                    bu.Note = value;
                }
            }
        }

        public void SanitizeDuplicates()
        {
            /*
            Layer = Layer.GroupBy(x => new { x.Address, x.Domain })
              .Where(g => g.Count() > 1)
              .Select(y => y.First())
              .ToList();
              */

            List<BlastUnit> bul = new List<BlastUnit>(Layer.ToArray().Reverse());
            List<ValueTuple<string, long>> usedAddresses = new List<ValueTuple<string, long>>();

            foreach (BlastUnit bu in bul)
            {
                if (!usedAddresses.Contains(new ValueTuple<string, long>(bu.Domain, bu.Address)) && !bu.IsLocked)
                {
                    usedAddresses.Add(new ValueTuple<string, long>(bu.Domain, bu.Address));
                }
                else if (!bu.IsLocked)
                {
                    Layer.Remove(bu);
                }
            }
        }
    }
}
