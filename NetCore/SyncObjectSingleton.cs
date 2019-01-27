using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.NetCore
{
	public static class SyncObjectSingleton
	{
		public static Form SyncObject;

		public static void FormExecute(Action<object, EventArgs> a, object[] args = null)
		{
			if (SyncObject.InvokeRequired)
				SyncObject.Invoke(new MethodInvoker(() => { a.Invoke(null, null); }));
			else
				a.Invoke(null, null);
		}

		public static void SyncObjectExecute(Form sync, Action<object, EventArgs> a, object[] args = null)
		{
			if (sync.InvokeRequired)
				sync.Invoke(new MethodInvoker(() => { a.Invoke(null, null); }));
			else
				a.Invoke(null, null);
		}
	}
}
