using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.UI.Input
{
    public static class Bindings
    {

        private static readonly WorkingDictionary<string, List<string>> _bindings = new WorkingDictionary<string, List<string>>();
        private static readonly WorkingDictionary<string, bool> _buttons = new WorkingDictionary<string, bool>();
        private static readonly WorkingDictionary<string, int> _buttonStarts = new WorkingDictionary<string, int>();

        // Looks for bindings which are activated by the supplied physical button.
        public static List<string> SearchBindings(string button)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, List<string>> kvp in _bindings)
            foreach (var boundButton in kvp.Value)
            {
                if (boundButton == button) list.Add(kvp.Key);
            }

            return list;
        }

        public static void BindButton(string action, string input)
        {
            _bindings[action].Add(input);
        }


    }
}
