namespace RTCV.NetCore
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;
    using Ceras;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using RTCV.NetCore.NetCore_Extensions;

    public class FullSpec : BaseSpec
    {
        public event EventHandler<SpecUpdateEventArgs> SpecUpdated;
        public virtual void OnSpecUpdated(SpecUpdateEventArgs e) => SpecUpdated?.Invoke(this, e);

        private PartialSpec template = null;
        public string name = "UnnamedSpec";
        public bool propagationIsEnabled;

        public new object this[string key]  //FullSpec is readonly, must update with partials
        {
            get
            {
                specDico.TryGetValue(key, out object value);
                return value; //returns null if doesn't exist
            }
        }

        public FullSpec(PartialSpec partialSpec, bool _propagationEnabled)
        {
            propagationIsEnabled = _propagationEnabled;

            //Creating a FullSpec requires a template
            template = partialSpec;
            base.version = 1;
            name = partialSpec.Name;
            Update(template);

            //Set the version after the update
            if (partialSpec.version != 1)
            {
                base.version = partialSpec.version;
            }
        }

        public void RegisterUpdateAction(Action<object, SpecUpdateEventArgs> registrant)
        {
            UnregisterUpdateAction();
            SpecUpdated += registrant.Invoke; //We trick the eventhandler in executing the registrant instead
        }

        public void UnregisterUpdateAction()
        {
            //finds any delegate referencing SpecUpdated and dereferences it

            FieldInfo eventFieldInfo = typeof(FullSpec).GetField("SpecUpdated", BindingFlags.NonPublic | BindingFlags.Instance);
            MulticastDelegate eventInstance = (MulticastDelegate)eventFieldInfo.GetValue(this);
            Delegate[] invocationList = eventInstance?.GetInvocationList() ?? new Delegate[] { };
            MethodInfo eventRemoveMethodInfo = typeof(FullSpec).GetEvent("SpecUpdated").GetRemoveMethod(true);
            foreach (Delegate eventHandler in invocationList)
            {
                eventRemoveMethodInfo.Invoke(this, new object[] { eventHandler });
            }
        }

        public new void Reset()
        {
            base.Reset();

            if (template != null)
            {
                Update(template);
            }
        }

        public void Update(PartialSpec _partialSpec, bool propagate = true, bool synced = true)
        {
            if (name != _partialSpec.Name)
            {
                throw new Exception("Name mismatch between PartialSpec and FullSpec");
            }

            //For initial
            foreach (var key in _partialSpec.specDico.Keys)
            {
                base[key] = _partialSpec.specDico[key];
            }

            //Increment the version
            base.version++;

            if (propagationIsEnabled && propagate)
            {
                OnSpecUpdated(new SpecUpdateEventArgs()
                {
                    partialSpec = _partialSpec,
                    syncedUpdate = synced
                });
            }
        }

        public void Update(string key, object value, bool propagate = true, bool synced = true)
        {
            /*
            //Make a partial spec and pass it into Update(PartialSpec)
            if (RTC_NetcoreImplementation.isStandaloneEmu && name == "RTCSpec" && key != RTCSPEC.CORE_AUTOCORRUPT.ToString())
                throw new Exception("Tried updating the RTCSpec from Emuhawk");

            if (RTC_NetcoreImplementation.isStandaloneUI && name == "EmuSpec")
                throw new Exception("Tried updating the EmuSpec from StandaloneRTC");
                */
            /*
            if(value is bool)
            {
                bool boolValue = (bool)value;
                if (boolValue == false)
                    value = null;
            }*/

            PartialSpec spec = new PartialSpec(name);
            spec[key] = value;
            Update(spec, propagate, synced);
        }

        public PartialSpec GetPartialSpec()
        {
            PartialSpec p = new PartialSpec(name);
            foreach (var key in specDico.Keys)
            {
                p[key] = base[key];
            }

            p.version = base.version;
            return p;
        }

        public void FullUpdate() => Update(GetPartialSpec());

        public List<string> GetDump()
        {
            var dump = new List<string>
            {
                this.name + " v" + version
            };
            foreach (string key in base.specDico.Keys)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(key + ": ");
                if (base[key] is IEnumerable em && !(base[key] is string))
                {
                    sb.AppendLine(RecursiveEnumerate(em));
                }
                else
                {
                    sb.Append(base[key].ToString());
                }

                dump.Add(sb.ToString());
            }
            return dump;
        }

        private string RecursiveEnumerate(IEnumerable em, StringBuilder sb = null, int tab = 1)
        {
            if (sb == null)
            {
                sb = new StringBuilder();
            }

            StringBuilder tabBuilder = new StringBuilder();
            for (int i = 0; i < tab; i++)
            {
                tabBuilder.Append("\t");
            }

            string t = tabBuilder.ToString();

            sb.AppendLine(t + em.ToString());
            foreach (var x in em)
            {
                if (x is IEnumerable _em && !(x is string))
                {
                    sb.AppendLine(RecursiveEnumerate(_em, sb, tab + 1));
                }
                /*
                if (x is KeyValuePair<string, List<Byte[]>> b)
                {
                    b.Value.ForEach(y =>
                    {
                        foreach (var z in y)
                            sb.Append(z.ToString());
                        sb.Append("\n" + t + "\t");
                    });
                }
                */
                sb.AppendLine(t + x.ToString());
            }
            return sb.ToString();
        }
    }

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public class PartialSpec : BaseSpec
    {
        public string Name;

        public PartialSpec(string _name)
        {
            Name = _name;
        }

        public PartialSpec()
        {
        }

        public void Insert(PartialSpec partialSpec)
        {
            if (partialSpec == null)
            {
                return;
            }

            foreach (var key in partialSpec.specDico.Keys)
            {
                this[key] = partialSpec.specDico[key];
            }
        }

        protected PartialSpec(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name");
        }
    }

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public abstract class BaseSpec
    {
        internal int version { get; set; }
        public int Version => version;
        internal ConcurrentDictionary<string, object> specDico { get; set; } = new ConcurrentDictionary<string, object>();

        public object this[string key]
        {
            get
            {
                specDico.TryGetValue(key, out object value);
                return value;   //returns null if doesn't exist
            }
            set
            {
                if (value == null && !(this is PartialSpec))    // Partials can have null values
                {                                               // A null value means a key removal in the Full Spec
                    specDico.TryRemove(key, out _);
                }
                else
                {
                    specDico[key] = value;
                }
            }
        }

        public void Set(string key, object value) => this[key] = value;

        public T Get<T>(string key)
        {
            if (specDico.TryGetValue(key, out object value))
            {
                return (T)value;
            }
            return default(T);
        }

        public List<string> GetKeys() => specDico.Keys.ToList();

        public string GetSerializedDico()
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                Converters = new JsonConverter[] { new StringEnumConverter() }
            };
            return JsonConvert.SerializeObject(specDico, jsonSerializerSettings);
        }

        public void Reset() => specDico.Clear();
        public object Clone() => ObjectCopier.Clone(this);
    }

    public class SpecUpdateEventArgs : EventArgs
    {
        public PartialSpec partialSpec = null;
        public bool syncedUpdate = true;
    }
}
