using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

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
    public partial class RTC_MemoryDomains_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);
        private readonly Dictionary<string, bool> domains = new Dictionary<string, bool>();
        private bool ignoreListboxChanged;

        //private BindingSource _bs;

        public RTC_MemoryDomains_Form()
        {
            InitializeComponent();

            //Registers the drag and drop with RTC_MyVMDs_Form
            AllowDrop = true;
            this.DragEnter += S.GET<RTC_VmdPool_Form>().RTC_VmdPool_Form_DragEnter;
            this.DragDrop += S.GET<RTC_VmdPool_Form>().RTC_VmdPool_Form_DragDrop;
        }


        private void UpdateSelectedMemoryDomains(object sender, EventArgs args)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var s in domains.Where(x => x.Value).ToList())
            {
                sb.Append($"{s},");
            }

            logger.Trace("UpdateSelectedMemoryDomains Setting SELECTEDDOMAINS domains to {domains}", sb);
            AllSpec.UISpec.Update("SELECTEDDOMAINS", domains.Where(x => x.Value).Select(x => x.Key).ToArray());
            PostDomainsToUI();
        }

        public void SetMemoryDomainsSelectedDomains(string[] _domains)
        {
            foreach (var domain in _domains)
            {
                if (_domains.Contains(domain))
                {
                    this.domains[domain] = true;
                }
                else
                {
                    this.domains[domain] = false;
                }
            }
            UpdateSelectedMemoryDomains(null, null);
        }

        private void SendDomainsToUI()
        {
            ignoreListboxChanged = true;
            lbMemoryDomains.Items.Clear();
            lbMemoryDomains.Items.AddRange(domains.Keys.ToArray());
            ignoreListboxChanged = false;
        }

        private void PostDomainsToUI()
        {
            SyncObjectSingleton.PostToUI(
                _ =>
                {
                    SendDomainsToUI();
                }, null);
            PostSelectedDomainsToUI();
        }

        private void SendSelectedDomainsToUI()
        {
            ignoreListboxChanged = true;
            foreach (var domain in domains.ToList())
            {
                lbMemoryDomains.SetSelected(lbMemoryDomains.FindStringExact(domain.Key), domain.Value);
            }
            ignoreListboxChanged = false;
        }

        private void PostSelectedDomainsToUI()
        {
            SyncObjectSingleton.PostToUI(
                _ =>
                {
                    SendSelectedDomainsToUI();
                }, null);
        }

        public void SetMemoryDomainsAllButSelectedDomains(string[] _blacklistedDomains)
        {
            foreach (var domain in domains.ToList())
            {
                if (_blacklistedDomains?.Contains(domain.Key) ?? false)
                {
                    domains[domain.Key] = false;
                }
                else
                {
                    domains[domain.Key] = true;
                }
            }
            UpdateSelectedMemoryDomains(null, null);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            RefreshDomains();
            foreach (var domain in domains.ToList())
            {
                domains[domain.Key] = true;
            }
            UpdateSelectedMemoryDomains(null, null);
        }

        private void btnAutoSelectDomains_Click(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_DOMAIN_REFRESHDOMAINS, true);
            RefreshDomains();
            SetMemoryDomainsAllButSelectedDomains((string[])AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS] ?? new string[] { });
        }

        public void RefreshDomains()
        {
            domains.Clear();
            if (MemoryDomains.MemoryInterfaces != null)
            {
                foreach (var domain in MemoryDomains.MemoryInterfaces?.Keys.ToArray())
                {
                    domains[domain] = false;
                }
            }

            if (MemoryDomains.VmdPool.Count > 0)
            {
                foreach (var domain in MemoryDomains.VmdPool.Values.Select(it => it.ToString()).ToArray())
                {
                    domains[domain] = false;
                }
            }
        }

        public void RefreshDomainsAndKeepSelected(string[] overrideDomains = null)
        {
            var temp = (string[])AllSpec.UISpec["SELECTEDDOMAINS"];

            RefreshDomains(); //refresh and reload domains

            if (overrideDomains != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var s in overrideDomains)
                {
                    sb.Append($"{s},");
                }

                logger.Trace("RefreshDomainsAndKeepSelected override SELECTEDDOMAINS domains to {domains}", sb);
                AllSpec.UISpec.Update("SELECTEDDOMAINS", overrideDomains);
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

                AllSpec.UISpec.Update("SELECTEDDOMAINS", temp);
                SetMemoryDomainsSelectedDomains(temp);
            }
            else
            {
                SetMemoryDomainsAllButSelectedDomains((string[])AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS] ?? new string[0]);
            }

            PostDomainsToUI();
        }

        private void lbMemoryDomains_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoreListboxChanged)
            {
                return;
            }

            foreach (string item in lbMemoryDomains.SelectedItems)
            {
                domains[item] = true;
            }

            UpdateSelectedMemoryDomains(null, null);
            //RTC_Restore.SaveRestore();
        }

        private void btnUnselectAll_Click(object sender, EventArgs e)
        {
            foreach (var domain in domains.ToList())
            {
                domains[domain.Key] = false;
            }
            AllSpec.UISpec.Update("SELECTEDDOMAINS", domains.Where(x => x.Value).ToArray());
            PostSelectedDomainsToUI();
        }

        private void lbMemoryDomains_MouseDown(object sender, MouseEventArgs e)
        {
            //Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);
            Point locate = new Point(e.Location.X, e.Location.Y);

            if (e.Button == MouseButtons.Right)
            {
                string vectorLimiter = S.GET<CorruptionEngineForm>().CurrentVectorLimiterListName;
                var AutoLimitedDomains = MemoryDomains.AllMemoryInterfaces.Where(it => it.Value is VirtualMemoryDomain vmd && vmd.Name.Contains("->")).ToList();

                if (vectorLimiter != null)
                {
                    ContextMenuStrip cms = new ContextMenuStrip();
                    //cms.Items.Add($"Generate VMD using Vector Limiter", null, (ob, ev) => {}).Enabled = false;
                    var lbGen = new ToolStripLabel($"Limiter Profiler");
                    lbGen.Font = new Font(lbGen.Font, FontStyle.Italic);

                    cms.Items.Add(lbGen);
                    cms.Items.Add(new ToolStripSeparator());
                    cms.Items.Add($"Regenerate all Profiled VMDs", null, (ob, ev) =>
                    {
                        foreach (var mi in MemoryDomains.AllMemoryInterfaces.Where(it => it.Value is VirtualMemoryDomain && it.Key.Contains("--")))
                        {
                            var vmd = (mi.Value as VirtualMemoryDomain);

                            string realDomain = vmd.GetRealDomain(0);
                            var realDomainInterface = MemoryDomains.AllMemoryInterfaces.Where(it => it.Key == realDomain).Count();
                            if (realDomainInterface == 0)
                            {
                                //this is not very good, it only checks for the first domain referenced in the VMDs.
                                //like, if you were to do "Regenerate all VMDs" and had a cross-domain VMD loaded and
                                //you changed games and one of the domains isn't loaded but the first domain referenced in the VMD
                                //is loaded, this will go through and shit itself when it tries to read from the domain that is unloaded

                                //in order to fix this, we would have to store with each VMD a list of the domains it references so that
                                //we don't have to check every single pointer address or range.

                                MessageBox.Show($"The Memory Domain named {realDomain} does not appear to be loaded. {vmd} cannot be regenerated.");
                                continue;
                            }

                            string domain;
                            if (vmd.CompactPointerDomains.Length > 0)
                                domain = vmd.CompactPointerDomains.FirstOrDefault();
                            else
                                domain = vmd.PointerDomains.FirstOrDefault();


                            if (domain != null)
                            {
                                string limiter = vmd.Name.Substring(vmd.Name.LastIndexOf('>') + 2);
                                S.GET<RTC_VmdLimiterProfiler_Form>().AutoProfile(MemoryDomains.AllMemoryInterfaces[domain], limiter);
                            }
                        }
                    }).Enabled = (AutoLimitedDomains.Count > 0);


                    var cbLoadState = new ToolStripMenuItem();
                    cbLoadState.Text = "Load GH State on Generate";
                    var vlpForm = S.GET<RTC_VmdLimiterProfiler_Form>();
                    cbLoadState.Checked = vlpForm.cbLoadBeforeGenerate.Checked;
                    cbLoadState.Click += (ob, ev) => {
                        vlpForm.cbLoadBeforeGenerate.Checked = !vlpForm.cbLoadBeforeGenerate.Checked;
                    };
                    cms.Items.Add(cbLoadState);

                    cms.Items.Add(new ToolStripSeparator());

                    foreach (var mi in MemoryDomains.AllMemoryInterfaces.Where(it => !(it.Value is VirtualMemoryDomain)))
                    {
                        var menu = new ToolStripMenuItem();

                        string extraVector = "";
                        if (MemoryDomains.VmdPool.ContainsKey($"[V]{mi.Value} -- {vectorLimiter}"))
                            extraVector = " (Regenerate)";

                        var currentListMenuItem = new ToolStripMenuItem();
                        currentListMenuItem.Text = mi.Key.ToString();

                        var vectorMenuItem = new ToolStripMenuItem();
                        vectorMenuItem.Text = $"Use Vector Engine Limiter: -> {vectorLimiter}" + extraVector;

                        vectorMenuItem.Click += (ob, ev) => {
                            S.GET<RTC_VmdLimiterProfiler_Form>().AutoProfile(mi.Value, vectorLimiter);
                        };

                        currentListMenuItem.DropDownItems.Add(vectorMenuItem);
                        currentListMenuItem.DropDownItems.Add(new ToolStripSeparator());

                        foreach (ComboBoxItem<string> listItem in S.GET<CorruptionEngineForm>().cbVectorLimiterList.Items)
                        {
                            var listName = listItem.Name;
                            var subMenuItem = new ToolStripMenuItem();

                            string extra = "";
                            if (MemoryDomains.VmdPool.ContainsKey($"[V]{mi.Value} -- {listName}"))
                                extra = " (Regenerate)";

                            subMenuItem.Text = "-> " + listName + extra;

                            subMenuItem.Click += (ob, ev) => {
                                S.GET<RTC_VmdLimiterProfiler_Form>().AutoProfile(mi.Value, listName);
                            };

                            currentListMenuItem.DropDownItems.Add(subMenuItem);
                        }

                        cms.Items.Add(currentListMenuItem);
                    }

                    cms.Show((Control)sender, locate);
                }
            }
        }
    }
}
