using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

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


            vanguardSpec.RegisterUpdateAction((ob, ea) => {

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

        public new object this[string key]  //FullSpec is readonly, must update with partials
        {
            get
            {
                if (specDico.ContainsKey(key))
                    return specDico[key];
                else
                    return null;
            }
        }

        public FullSpec(PartialSpec partialSpec)
        {
            //Creating a FullSpec requires a template
            template = partialSpec;
            name = partialSpec.name;
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

            FieldInfo eventFieldInfo = typeof(FullSpec).GetField("SpecUpdated", BindingFlags.NonPublic | BindingFlags.Instance);
            MulticastDelegate eventInstance = (MulticastDelegate)eventFieldInfo.GetValue(this);
            Delegate[] invocationList = eventInstance?.GetInvocationList() ?? new Delegate[] { };
            MethodInfo eventRemoveMethodInfo = typeof(FullSpec).GetEvent("SpecUpdated").GetRemoveMethod(true);
            foreach (Delegate eventHandler in invocationList)
                eventRemoveMethodInfo.Invoke(this, new object[] { eventHandler });
        }

        public new void Reset()
        {
            base.Reset();

            if (template != null)
                Update(template);
        }

        public void Update(PartialSpec _partialSpec, bool propagate = true)
        {
            if (name != _partialSpec.name)
                throw new Exception("Name mismatch between PartialSpec and FullSpec");

            for (int i = 0; i < _partialSpec.keys.Count; i++)
                base[_partialSpec.keys[i]] = _partialSpec.values[i];

            if (propagate)
                OnSpecUpdated(new SpecUpdateEventArgs() { partialSpec = _partialSpec });
        }

    }

    [Serializable]
    public class PartialSpec : BaseSpec
    {
        public string name;
        public PartialSpec(string _name)
        {
            name = _name;
        }
    }

    [Serializable]
    public abstract class BaseSpec : ISerializable
    {

        [IgnoreDataMember]
        internal Dictionary<string, object> specDico { get; set; } = new Dictionary<string, object>();

        public List<string> keys = new List<string>();
        public List<object> values = new List<object>();

        public object this[string key]
        {
            get
            {

                if (specDico.ContainsKey(key))
                    return specDico[key];
                else
                    return null;
            }

            set
            {

                if (value == null && !(this is PartialSpec))    // Partials can have null values
                {                                               // A null value means a key removal in the Full Spec
                    if (specDico.ContainsKey(key))
                        specDico.Remove(key);
                }
                else
                    specDico[key] = value;

            }
        }

        public void Reset() => specDico.Clear();

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Keys", keys);
            info.AddValue("Values", values);
        }

        [OnSerializing]
        private void OnSerializing()
        {
            keys.Clear();
            keys.AddRange(specDico.Keys);

            values.Clear();
            values.AddRange(specDico.Values);

            //If for some ungodly reason the indexes were not to match, this slower code below should fix that.
            /*
            keys.Clear();
            values.Clear();
            foreach (var key in specDico.Keys)
            {
                keys.Add(key);
                values.Add(specDico[key]);
            }
            */
        }

        [OnDeserialized]
        private void OnDeserialized()
        {
            for (int i = 0; i < keys.Count; i++)
                specDico[keys[i]] = values[i];
        }

    }

    public class SpecUpdateEventArgs : EventArgs
    {
        public PartialSpec partialSpec = null;
    }

}
