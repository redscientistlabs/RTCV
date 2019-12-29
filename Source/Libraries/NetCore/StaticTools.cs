using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTCV.NetCore.StaticTools
{

	// Implementing this interface causes auto-coloration.
	public interface IAutoColorize { }

    class LazyConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<TKey, Lazy<TValue>>
    {
        public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
        {
            var result = base.AddOrUpdate(key,
                k => new Lazy<TValue>(() => addValueFactory(k), LazyThreadSafetyMode.ExecutionAndPublication),
                (k2, old) => new Lazy<TValue>(() => updateValueFactory(k2, old.Value), LazyThreadSafetyMode.ExecutionAndPublication));

            return result.Value;
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            var lazyResult = base.GetOrAdd(key, k => new Lazy<TValue>(() => valueFactory(k), LazyThreadSafetyMode.ExecutionAndPublication));

            return lazyResult.Value;
        }
    }

    //Static singleton manager
    //Call or create a singleton using class type
    public static class S
	{
		private static readonly LazyConcurrentDictionary<Type, object> instances = new LazyConcurrentDictionary<Type, object>();
        public static FormRegister formRegister = new FormRegister();


		public static bool ISNULL<T>()
		{
            Type typ = typeof(T);
            return instances.ContainsKey(typ);
        }

        //returns all singletons that implements a certain type
        public static T[] GETINTERFACES<T>()
        {
            return instances.Values.OfType<T>().ToArray();
        }

        public static T GET<T>() where T : class, new()
        {
            Type typ = typeof(T);
            var r = instances.GetOrAdd(typ, key => new Lazy<object>(valueFactory: () =>
            {
                var t = new T();
                if (typ.IsSubclassOf(typeof(System.Windows.Forms.Form)))
                    formRegister.OnFormRegistered(new NetCoreEventArgs("FORMREGISTER", t));
                return t; 
            }, LazyThreadSafetyMode.PublicationOnly));

            return (T)((Lazy<object>) r).Value;
        }

        public static object GET(Type typ)
        {
            var constructor = typ.GetConstructor(new Type[0]);
            if (constructor == null)
                throw new Exception("GET requires a parameterless constructor");

            var r = instances.GetOrAdd(typ, key => new Lazy<object>(() =>
            {
                var t= constructor.Invoke(new object[0]);
                if (typ.IsSubclassOf(typeof(System.Windows.Forms.Form)))
                    formRegister.OnFormRegistered(new NetCoreEventArgs("FORMREGISTER", t));
                return t;
            }, LazyThreadSafetyMode.PublicationOnly));
            var info = r.GetType().GetProperty("Value");

            return info.GetValue(r);
        }

        
        public static void SET<T>(T newTyp)
        {
            Func<Type, Object> addFunc = (key) => new Lazy<object>(() => newTyp, LazyThreadSafetyMode.PublicationOnly);
            Func<Type, Object, Object> updateFunc = (key, key2) =>
            {
                if (newTyp == null)
                    return null;
                return new Lazy<object>(() =>
                {
                    if (newTyp.GetType().IsSubclassOf(typeof(System.Windows.Forms.Form)))
                        formRegister.OnFormRegistered(new NetCoreEventArgs("FORMREGISTER", newTyp));
                    return newTyp;
                }, LazyThreadSafetyMode.PublicationOnly);
            };
            instances.AddOrUpdate(newTyp.GetType(), addFunc, updateFunc);
        }
    }

	public class FormRegister
	{
		public event EventHandler<NetCoreEventArgs> FormRegistered;
		public virtual void OnFormRegistered(NetCoreEventArgs e) => FormRegistered?.Invoke(this, e);
	}

}
