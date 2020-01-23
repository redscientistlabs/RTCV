using RTCV.CorruptCore.EventWarlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock
{
    [System.Serializable]
    class Grimoire
    {
        public List<Spell> LoadSpells = new List<Spell>();
        public List<Spell> PreExecuteSpells = new List<Spell>();
        public List<Spell> PostExecuteSpells = new List<Spell>();
    }
}
