using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Ceras;

namespace RTCV.NetCore
{
    internal class Example
    {
        private void TEST()
        {

            PartialSpec vanguardSpecTemplate = new PartialSpec("VanguardSpec");
            vanguardSpecTemplate["KeyName1"] = "Value1";
            vanguardSpecTemplate["KeyName2"] = "Value2";
            vanguardSpecTemplate["Keyname3"] = "Value3";



            FullSpec vanguardSpec = new FullSpec(vanguardSpecTemplate); //You have to feed a partial spec as a template


            vanguardSpec.RegisterUpdateAction((ob, ea) =>
            {

                PartialSpec partial = ea.partialSpec;
                //send partial update via netcore or whatever

            });


            PartialSpec update = new PartialSpec("VanguardSpec");
            update["KeyName4"] = "Value4";
            update["KeyName3"] = null; //this removes the entry if it already exists

            vanguardSpec.Update(update);
            //Update will trigger the Update Action and it will propagate through netcore if needed


            //vanguardSpec["test"] = "something"; //this will give a readonly error
            //you cannot edit the fullspec without an update
        }
    }

    public class FullSpec : BaseSpec
    {
        public event EventHandler<SpecUpdateEventArgs> SpecUpdated;
        public virtual void OnSpecUpdated(SpecUpdateEventArgs e) => SpecUpdated?.Invoke(this, e);

        private PartialSpec template = null;
        public string name = "UnnamedSpec";

        public new object this[string key] //FullSpec is readonly, must update with partials
        {
            get
            {
                if (specDico.ContainsKey(key))
                    return specDico[key];
                return null;
            }
        }

        public FullSpec(PartialSpec partialSpec)
        {
            //Creating a FullSpec requires a template
            template = partialSpec;
            name = partialSpec.Name;
            Update(template);
        }

        public void RegisterUpdateAction(Action<object, SpecUpdateEventArgs> registrant)
        {
            UnregisterUpdateAction();
            SpecUpdated += registrant.Invoke; //We trick the eventhandler in executing the registrant instead
        }

        public void UnregisterUpdateAction()
        {
            //finds any delegate referencing SpecUpdated and dereferences it

            FieldInfo eventFieldInfo =
                typeof(FullSpec).GetField("SpecUpdated", BindingFlags.NonPublic | BindingFlags.Instance);
            MulticastDelegate eventInstance = (MulticastDelegate)eventFieldInfo.GetValue(this);
            Delegate[] invocationList = eventInstance?.GetInvocationList() ?? new Delegate[] { };
            MethodInfo eventRemoveMethodInfo = typeof(FullSpec).GetEvent("SpecUpdated").GetRemoveMethod(true);
            foreach (Delegate eventHandler in invocationList)
                eventRemoveMethodInfo.Invoke(this, new object[] {eventHandler});
        }

        public new void Reset()
        {
            base.Reset();

            if (template != null)
                Update(template);
        }

        public void Update(PartialSpec _partialSpec, bool propagate = true)
        {
            if (name != _partialSpec.Name)
                throw new Exception("Name mismatch between PartialSpec and FullSpec");

            //For initial
            foreach (var key in _partialSpec.specDico.Keys)
                base[key] = _partialSpec.specDico[key];

            if (propagate)
                OnSpecUpdated(new SpecUpdateEventArgs() {partialSpec = _partialSpec});
        }

        public void Update(String key, Object value, bool propagate = true)
        {PartialSpec spec = new PartialSpec(name);
            spec[key] = value;
            Update(spec, propagate);
        }

        public PartialSpec GetPartialSpec()
        {
            PartialSpec p = new PartialSpec(name);
            foreach (var key in specDico.Keys)
            {
                p[key] = base[key];
            }

            return p;
        }

        public void FullUpdate()
        {
            Update(GetPartialSpec());
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

        protected PartialSpec(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name");
        }
    }

    [Serializable]
    [Ceras.MemberConfig(TargetMember.All)]
    public abstract class BaseSpec
    {
        internal Dictionary<string, object> specDico { get; set; } = new Dictionary<string, object>();

        public object this[string key]
        {
            get
            {
                if (specDico.ContainsKey(key))
                    return specDico[key];
                return null;
            }
            set
            {
                if (value == null && !(this is PartialSpec)) // Partials can have null values
                {
                    // A null value means a key removal in the Full Spec
                    if (specDico.ContainsKey(key))
                        specDico.Remove(key);
                }
                else
                    specDico[key] = value;
            }
        }

        public void Reset() => specDico.Clear();
    }

    public class SpecUpdateEventArgs : EventArgs
    {
        public PartialSpec partialSpec = null;
    }
}