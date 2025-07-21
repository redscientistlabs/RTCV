namespace RTCV.UI
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    #pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class MemoryDomainsForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);
        private System.Timers.Timer updateTimer;

        public MemoryDomainsForm()
        {
            InitializeComponent();
            updateTimer = new System.Timers.Timer
            {
                AutoReset = false,
                Interval = 300,
            };
            updateTimer.Elapsed += UpdateSelectedMemoryDomains;


            //Registers the drag and drop with MyVMDsForm
            AllowDrop = true;
            this.DragEnter += S.GET<VmdPoolForm>().HandleDragEnter;
            this.DragDrop += S.GET<VmdPoolForm>().HandleDragDrop;
        }

        private void UpdateSelectedMemoryDomains(object sender, EventArgs args)
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                StringBuilder sb = new StringBuilder();
                foreach (var s in lbMemoryDomains.SelectedItems.Cast<string>().ToArray())
                {
                    sb.Append($"{s},");
                }

                logger.Trace("UpdateSelectedMemoryDomains Setting SELECTEDDOMAINS domains to {domains}", sb);
                string[] output = lbMemoryDomains.SelectedItems.Cast<string>().Distinct().ToArray();
                AllSpec.UISpec.Update(UISPEC.SELECTEDDOMAINS, output);

                SyncObjectSingleton.FormExecute(() =>
                {
                    UISideHooks.OnSelectedDomainsChanged(output);
                });
            });
        }

        public void SetMemoryDomainsSelectedDomains(string[] domains)
        {
            var oldState = this.Visible;

            for (int i = 0; i < lbMemoryDomains.Items.Count; i++)
            {
                if (domains.Contains(lbMemoryDomains.Items[i].ToString()))
                {
                    lbMemoryDomains.SetSelected(i, true);
                }
                else
                {
                    lbMemoryDomains.SetSelected(i, false);
                }
            }

            UpdateSelectedMemoryDomains(null, null);
            this.Visible = oldState;
        }

        public void SetMemoryDomainsAllButSelectedDomains(string[] blacklistedDomains)
        {
            var oldState = this.Visible;

            for (int i = 0; i < lbMemoryDomains.Items.Count; i++)
            {
                if (blacklistedDomains?.Contains(lbMemoryDomains.Items[i].ToString()) ?? false)
                {
                    lbMemoryDomains.SetSelected(i, false);
                }
                else
                {
                    lbMemoryDomains.SetSelected(i, true);
                }
            }

            UpdateSelectedMemoryDomains(null, null);
            this.Visible = oldState;
        }

        private void SelectAllDomains(object sender, EventArgs e)
        {
            RefreshDomains();

            for (int i = 0; i < lbMemoryDomains.Items.Count; i++)
            {
                lbMemoryDomains.SetSelected(i, true);
            }

            UpdateSelectedMemoryDomains(null, null);
        }

        private void AutoSelectDomains(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetCore.Endpoints.Vanguard, NetCore.Commands.Remote.DomainRefreshDomains, true);
            RefreshDomains();
            SetMemoryDomainsAllButSelectedDomains((string[])AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS] ?? new string[] { });
        }

        public void RefreshDomains()
        {
            var oldState = this.Visible;
            lbMemoryDomains.Items.Clear();
            if (MemoryDomains.MemoryInterfaces != null)
            {
                lbMemoryDomains.Items.AddRange(MemoryDomains.MemoryInterfaces?.Keys.ToArray());
            }

            if (MemoryDomains.VmdPool.Count > 0)
            {
                lbMemoryDomains.Items.AddRange(MemoryDomains.VmdPool.Values.Select(it => it.ToString()).ToArray());
            }

            this.Visible = oldState;
        }

        public void RefreshDomainsAndKeepSelected(string[] overrideDomains = null)
        {
            var temp = (string[])AllSpec.UISpec[UISPEC.SELECTEDDOMAINS];

            if (temp != null)
                temp = temp.Distinct().ToArray(); //remove dupes

            var oldDomain = lbMemoryDomains.Items;

            RefreshDomains(); //refresh and reload domains

            if (overrideDomains != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var s in overrideDomains)
                {
                    sb.Append($"{s},");
                }

                logger.Trace("RefreshDomainsAndKeepSelected override SELECTEDDOMAINS domains to {domains}", sb);
                AllSpec.UISpec.Update(UISPEC.SELECTEDDOMAINS, overrideDomains);
                SetMemoryDomainsSelectedDomains(overrideDomains);
            }
            //If we had old domains selected don't do anything
            else if (temp?.Length != 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var s in temp)
                {
                    sb.Append($"{s},");
                }

                logger.Trace("RefreshDomainsAndKeepSelected temp Setting SELECTEDDOMAINS domains to {domains}", sb);

                AllSpec.UISpec.Update(UISPEC.SELECTEDDOMAINS, temp);
                SetMemoryDomainsSelectedDomains(temp);
            }
            else
            {
                SetMemoryDomainsAllButSelectedDomains((string[])AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS] ?? new string[0]);
            }
        }

        private void HandleMemoryDomainSelectionChange(object sender, EventArgs e)
        {
            updateTimer.Stop();
            updateTimer.Start();

            //var selectedDomains = lbMemoryDomains.SelectedItems.Cast<string>();
            //foreach (var key in domains.Keys.ToArray())
            //    domains[key] = selectedDomains.Contains(key);

            //UpdateSelectedMemoryDomains(null, null);
        }

        private void HandleRefreshDomainsClick(object sender, EventArgs e)
        {
            RefreshDomains();
            AllSpec.UISpec.Update(UISPEC.SELECTEDDOMAINS, lbMemoryDomains.SelectedItems.Cast<string>().Distinct().ToArray());
        }

        private void HandleMemoryDomainsMouseDown(object sender, MouseEventArgs e)
        {
            //Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);
            Point locate = new Point(e.Location.X, e.Location.Y);

            if (e.Button != MouseButtons.Right)
                return;
            
            string vectorLimiter = S.GET<CorruptionEngineForm>().CurrentVectorLimiterListName;

            if (vectorLimiter == null)
                return;
            var autoLimitedDomains = MemoryDomains.AllMemoryInterfaces.Where(it => it.Value is VirtualMemoryDomain && it.Key.Contains("--")).ToList();
            var vlpForm = S.GET<VmdLimiterProfilerForm>();
            var cmb = new ContextMenuBuilder()
                //.AddItem("Generate VMD using Vector Limiter", (ob, ev) => {}, false)
                .AddText("Limiter Profiler", FontStyle.Italic)
                .AddSeparator()
                .AddItem("Regenerate All Profiled VMDs", (ob, ev) =>
                {
                    foreach (var mi in MemoryDomains.AllMemoryInterfaces.Where(it =>
                                 it.Value is VirtualMemoryDomain && it.Key.Contains("--")))
                    {
                        var vmd = (mi.Value as VirtualMemoryDomain);

                        string realDomain = vmd.GetRealDomain(0);
                        var realDomainInterface =
                            MemoryDomains.AllMemoryInterfaces.Count(it => it.Key == realDomain);
                        if (realDomainInterface == 0)
                        {
                            //this is not very good, it only checks for the first domain referenced in the VMDs.
                            //like, if you were to do "Regenerate all VMDs" and had a cross-domain VMD loaded and
                            //you changed games and one of the domains isn't loaded but the first domain referenced in the VMD
                            //is loaded, this will go through and shit itself when it tries to read from the domain that is unloaded

                            //in order to fix this, we would have to store with each VMD a list of the domains it references so that
                            //we don't have to check every single pointer address or range.

                            MessageBox.Show(
                                $"The Memory Domain named {realDomain} does not appear to be loaded. {vmd} cannot be regenerated.");
                            continue;
                        }

                        string domain;
                        if (vmd.CompactPointerDomains.Length > 0)
                        {
                            domain = vmd.CompactPointerDomains.FirstOrDefault();
                        }
                        else
                        {
                            domain = vmd.PointerDomains.FirstOrDefault();
                        }

                        if (domain != null)
                        {
                            string limiter = vmd.Name.Substring(vmd.Name.IndexOf("--") + 3);
                            S.GET<VmdLimiterProfilerForm>()
                                .AutoProfile(MemoryDomains.AllMemoryInterfaces[domain], limiter);
                        }
                    }
                }, autoLimitedDomains.Count > 0)
                .AddItem("Load GH State on Generate", (ob, ev) =>
                {
                    vlpForm.cbLoadBeforeGenerate.Checked = !vlpForm.cbLoadBeforeGenerate.Checked;
                }, isChecked: vlpForm.cbLoadBeforeGenerate.Checked)
                .AddSeparator();

            foreach (var mi in MemoryDomains.AllMemoryInterfaces.Where(it =>
                         !(it.Value is VirtualMemoryDomain)))
            {
                string extraVector = "";
                if (MemoryDomains.VmdPool.ContainsKey($"[V]{mi.Value} -- {vectorLimiter}"))
                {
                    extraVector = " (Regenerate)";
                }

                var currentListMenuItem = new ToolStripMenuItem(mi.Key);
                var vectorMenuItem =
                    new ToolStripMenuItem($"Use Vector Engine Limiter: -> {vectorLimiter}" + extraVector);
                vectorMenuItem.Click += (ob, ev) =>
                {
                    S.GET<VmdLimiterProfilerForm>().AutoProfile(mi.Value, vectorLimiter);
                };

                currentListMenuItem.DropDownItems.Add(vectorMenuItem);
                currentListMenuItem.DropDownItems.Add(new ToolStripSeparator());

                foreach (ComboBoxItem<string> listItem in S.GET<CorruptionEngineForm>().VectorEngineControl
                             .cbVectorLimiterList.Items)
                {
                    var listName = listItem.Name;

                    string extra = "";
                    if (MemoryDomains.VmdPool.ContainsKey($"[V]{mi.Value} -- {listName}"))
                    {
                        extra = " (Regenerate)";
                    }

                    var subMenuItem = new ToolStripMenuItem("-> " + listName + extra);
                    subMenuItem.Click += (ob, ev) =>
                    {
                        S.GET<VmdLimiterProfilerForm>().AutoProfile(mi.Value, listName);
                    };

                    currentListMenuItem.DropDownItems.Add(subMenuItem);
                }

                cmb.AddItem(currentListMenuItem);
            }

            cmb.Build().Show((Control)sender, locate);
        }

        private void lbMemoryDomains_DoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.lbMemoryDomains.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                for (int i = 0; i < lbMemoryDomains.Items.Count; i++)
                {
                    bool state = i == index;
                    lbMemoryDomains.SetSelected(i, state);
                }
            }
        }
    }
}
