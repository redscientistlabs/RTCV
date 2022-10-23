namespace RTCV.CorruptCore
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Xml.Serialization;
    using Ceras;
    using Newtonsoft.Json;
    using RTCV.NetCore;
    using Exception = System.Exception;

    [MemberConfig(TargetMember.All)]
    [Serializable]
    [XmlInclude(typeof(BlastUnit))]
    public class BlastLayer : ICloneable, INote
    {
        [SuppressMessage("Microsoft.Design", "CA1051", Justification = "Unknown serialization impact of making this property instead of a field")]
        public List<BlastUnit> Layer;

        public BlastLayer()
        {
            Layer = new List<BlastUnit>();
        }

        public BlastLayer(BlastUnit bu)
        {
            Layer = new List<BlastUnit>();
            Layer.Add(bu);
        }

        public BlastLayer(List<BlastUnit> layer)
        {
            Layer = layer;
        }

        public object Clone()
        {
            return ObjectCopierCeras.Clone(this);
        }

        public void Apply(bool storeUncorruptBackup, bool followMaximums = false, bool mergeWithPrevious = false)
        {
            //RPC stuff -------
            //When using RPC, memory must be cached locally. Every time a blast is done
            //A copy of the domain (usually chunked) is cached locally so that list-based
            //engines don't continously peek repeatedly over RPC.
            MemoryDomainProxy[] mdps = (AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_INTERFACES] as MemoryDomainProxy[]);
            string[] domains_forRpcUse = null;
            if (mdps[0].UsingRPC)
            {
                domains_forRpcUse = (string[])AllSpec.UISpec[UISPEC.SELECTEDDOMAINS];
                for (int i = 0; i < mdps.Length; i++)
                {
                    for (int j = 0; j < domains_forRpcUse.Length; j++)
                    {
                        if (mdps[i].Name == domains_forRpcUse[j])
                        {
                            (mdps[i].MD as IRPCMemoryDomain).DumpMemory();
                        }
                    }
                }
            }
            //-----------


            if (storeUncorruptBackup && this != StockpileManagerEmuSide.UnCorruptBL)
            {
                BlastLayer UnCorruptBL_Backup = null;
                BlastLayer CorruptBL_Backup = null;

                if (mergeWithPrevious)
                {
                    UnCorruptBL_Backup = StockpileManagerEmuSide.UnCorruptBL;
                    CorruptBL_Backup = StockpileManagerEmuSide.CorruptBL;
                }

                StockpileManagerEmuSide.UnCorruptBL = GetBackup();
                StockpileManagerEmuSide.CorruptBL = this;

                if (mergeWithPrevious)
                {
                    if (UnCorruptBL_Backup?.Layer != null)
                    {
                        if (StockpileManagerEmuSide.UnCorruptBL.Layer == null)
                        {
                            StockpileManagerEmuSide.UnCorruptBL.Layer = new List<BlastUnit>();
                        }

                        StockpileManagerEmuSide.UnCorruptBL.Layer.AddRange(UnCorruptBL_Backup.Layer);
                    }

                    if (CorruptBL_Backup?.Layer != null)
                    {
                        if (StockpileManagerEmuSide.CorruptBL.Layer == null)
                        {
                            StockpileManagerEmuSide.CorruptBL.Layer = new List<BlastUnit>();
                        }

                        StockpileManagerEmuSide.CorruptBL.Layer.AddRange(CorruptBL_Backup.Layer);
                    }
                }
            }
            else
            {
                //StockpileManagerEmuSide.UnCorruptBL = null;
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
                    bool IsUsingRPC = (mdps != null && mdps.Length > 0 && mdps[0].UsingRPC == true);
                    if (!UseRealtime || IsUsingRPC)
                    {
                        StepActions.Execute();
                    }
                }


                //More RPC Stuff ------------
                //Commits the memory updates via RPC
                if (mdps[0].UsingRPC)
                {
                    for (int i = 0; i < mdps.Length; i++)
                    {
                        for (int j = 0; j < domains_forRpcUse.Length; j++)
                        {
                            if (mdps[i].Name == domains_forRpcUse[j])
                            {
                                (mdps[i].MD as IRPCMemoryDomain).UpdateMemory();
                            }
                        }
                    }
                }
                //------------------
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
