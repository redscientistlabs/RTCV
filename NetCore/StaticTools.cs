using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.NetCore.StaticTools
{


	// Implementing this interface causes auto-coloration.
	public interface IAutoColorize { }

	//Static singleton manager
	//Call or create a singleton using class type
	public static class S
	{
		static Dictionary<Type, object> instances = new Dictionary<Type, object>();

		public static FormRegister formRegister = new FormRegister();

		public static bool ISNULL<T>()
		{
			Type typ = typeof(T);
			return instances.ContainsKey(typ);
		}

		public static T GET<T>()
		{
			Type typ = typeof(T);

			if (!instances.ContainsKey(typ))
			{
				instances[typ] = Activator.CreateInstance(typ);

				if (typ.IsSubclassOf(typeof(System.Windows.Forms.Form)))
					formRegister.OnFormRegistered(new NetCoreEventArgs("FORMREGISTER", instances[typ]));
			}

			return (T)instances[typ];
		}

		public static object GET(Type typ)
		{
			//Type typ = typeof(T);

			if (!instances.ContainsKey(typ))
				instances[typ] = Activator.CreateInstance(typ);

			return instances[typ];
		}

		public static void SET<T>(T newTyp)
		{
			Type typ = typeof(T);

			if (newTyp is Nullable && newTyp == null)
				instances.Remove(typ);
			else
				instances[typ] = newTyp;

			if (typ.IsSubclassOf(typeof(System.Windows.Forms.Form)))
				formRegister.OnFormRegistered(new NetCoreEventArgs("FORMREGISTER", instances[typ]));
		}


	}

	public class FormRegister
	{
		public event EventHandler<NetCoreEventArgs> FormRegistered;
		public virtual void OnFormRegistered(NetCoreEventArgs e) => FormRegistered.Invoke(this, e);
	}

}
