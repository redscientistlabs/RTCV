//Based on code from https://github.com/TASVideos/BizHawk/
namespace RTCV.UI.Input
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public static class Bindings
    {
        private static readonly WorkingDictionary<string, List<string>> _bindings = new WorkingDictionary<string, List<string>>();

        // Looks for bindings which are activated by the supplied physical button.
        public static List<string> SearchBindings(string button)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, List<string>> kvp in _bindings)
            {
                foreach (var boundButton in kvp.Value)
                {
                    if (boundButton == button)
                    {
                        list.Add(kvp.Key);
                    }
                }
            }

            return list;
        }

        public static void BindButton(string action, string input)
        {
            _bindings[action].Add(input);
        }

        public static void BindMulti(string action, string controlString)
        {
            if (string.IsNullOrEmpty(controlString))
            {
                return;
            }

            var controlbindings = controlString.Split(',');
            foreach (var control in controlbindings)
            {
                _bindings[action].Add(control.Trim());
            }
        }

        public static void ClearBindings()
        {
            _bindings.Clear();
        }
    }

    public class Binding
    {
        public string DisplayName { get; set; }
        public string Bindings { get; set; }
        public string DefaultBinding { get; set; }
        public string TabGroup { get; set; }
        public string ToolTip { get; set; }
        public int Ordinal { get; set; }
    }

    [Newtonsoft.Json.JsonObject]
    public class BindingCollection : IEnumerable<Binding>
    {
        public List<Binding> Bindings { get; }

        [Newtonsoft.Json.JsonConstructor]
        public BindingCollection(List<Binding> bindings)
        {
            Bindings = bindings;
        }

        public BindingCollection()
        {
            Bindings = new List<Binding>();
            Bindings.AddRange(DefaultValues);
        }

        public void Add(Binding b)
        {
            Bindings.Add(b);
        }

        public IEnumerator<Binding> GetEnumerator()
        {
            return Bindings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Binding this[string index]
        {
            get
            {
                return Bindings.FirstOrDefault(b => b.DisplayName == index) ?? new Binding();
            }
        }

        private static Binding Bind(string tabGroup, string displayName, string bindings = "", string defaultBinding = "", string toolTip = "")
        {
            if (string.IsNullOrEmpty(defaultBinding))
            {
                defaultBinding = bindings;
            }

            return new Binding { DisplayName = displayName, Bindings = bindings, TabGroup = tabGroup, DefaultBinding = defaultBinding, ToolTip = toolTip };
        }

        public void ResolveWithDefaults()
        {
            // TODO - this method is potentially disastrously O(N^2) slow due to linear search nested in loop

            // Add missing entries
            foreach (Binding defaultBinding in DefaultValues)
            {
                var binding = Bindings.FirstOrDefault(b => b.DisplayName == defaultBinding.DisplayName);
                if (binding == null)
                {
                    Bindings.Add(defaultBinding);
                }
                else
                {
                    // patch entries with updated settings (necessary because of TODO LARP
                    binding.Ordinal = defaultBinding.Ordinal;
                    binding.DefaultBinding = defaultBinding.DefaultBinding;
                    binding.TabGroup = defaultBinding.TabGroup;
                    binding.ToolTip = defaultBinding.ToolTip;
                    binding.Ordinal = defaultBinding.Ordinal;
                }
            }

            List<Binding> entriesToRemove = (from entry in Bindings let binding = DefaultValues.FirstOrDefault(b => b.DisplayName == entry.DisplayName) where binding == null select entry).ToList();

            // Remove entries that no longer exist in defaults
            foreach (Binding entry in entriesToRemove)
            {
                Bindings.Remove(entry);
            }
        }

        private static List<Binding> _defaultValues;

        public static List<Binding> DefaultValues
        {
            get
            {
                if (_defaultValues == null)
                {
                    //RTC_Hijack - Remove some of the more annoying default binds that don't line up with bizhawk usage
                    _defaultValues = new List<Binding>
                    {
                        Bind("RTC", "Manual Blast", toolTip: "Triggers a manual blast" ),
                        Bind("RTC", "Auto-Corrupt", toolTip: "Toggles autocorrupt" ),
                        Bind("RTC", "Error Delay--", toolTip: "Reduces error delay by 1" ),
                        Bind("RTC", "Error Delay++", toolTip: "Increases error delay by 1" ),
                        Bind("RTC", "Intensity--", toolTip: "Reduces intensity by 1" ),
                        Bind("RTC", "Intensity++", toolTip: "Increases intensity by 1" ),
                        Bind("RTC", "Induce KS Crash", toolTip: "Induces a Killswitch Kill+Restart" ),
                        Bind("Glitch Harvester", "Load and Corrupt", toolTip: "Triggers a Glitch Harvest Blast/Send" ),
                        Bind("Glitch Harvester", "Just Corrupt", toolTip: "Triggers a Glitch Harvester Blast without loading a state" ),
                        Bind("Glitch Harvester", "Load Prev State", toolTip: "Loads the savestate from the previously selected slot without changing slots" ),
                        Bind("Glitch Harvester", "New Savestate", toolTip: "Creates a glitch harvester in the next available slot" ),
                        Bind("Glitch Harvester", "Reroll", toolTip: "Rerolls the current stashkey" ),
                        Bind("Glitch Harvester", "Load", toolTip: "Loads the current savestate" ),
                        Bind("Glitch Harvester", "Save", toolTip: "Saves to the current savestate slot" ),
                        Bind("Glitch Harvester", "Stash->Stockpile", toolTip: "Sends an entry from the stash to the stockpile" ),
                        Bind("Glitch Harvester", "Blast+RawStash", toolTip: "Manual blasts then sends raw to stash" ),
                        Bind("Glitch Harvester", "Send Raw to Stash", toolTip: "Sends raw to stash" ),
                        Bind("Glitch Harvester", "Reload Corruption", toolTip: "Reloads the selected item in the stockpile or stockpile" ),
                        Bind("RTC", "BlastLayer Toggle", toolTip: "Toggles the current blastlayer" ),
                        Bind("RTC", "BlastLayer Re-Blast", toolTip: "Reblasts the current blastlayer" ),
                        Bind("RTC", "Game Protect Back", toolTip: "Jumps back in game protection" ),
                        Bind("RTC", "Game Protect Now", toolTip: "Jumps to the \"now\" state" ),
                        Bind("Blast Editor", "Disable 50", toolTip: "For RTC" ),
                        Bind("Blast Editor", "Remove Disabled", toolTip: "For RTC" ),
                        Bind("Blast Editor", "Invert Disabled", toolTip: "For RTC" ),
                        Bind("Blast Editor", "Shift Up", toolTip: "For RTC" ),
                        Bind("Blast Editor", "Shift Down", toolTip: "For RTC" ),
                        Bind("Blast Editor", "Load Corrupt", toolTip: "For RTC" ),
                        Bind("Blast Editor", "Apply", toolTip: "For RTC" ),
                        Bind("Blast Editor", "Send Stash", toolTip: "For RTC" ),
                    };

                    // set ordinals based on order in list
                    for (int i = 0; i < _defaultValues.Count; i++)
                    {
                        _defaultValues[i].Ordinal = i;
                    }
                } // if (s_DefaultValues == null)

                return _defaultValues;
            }
        }
    }
}
