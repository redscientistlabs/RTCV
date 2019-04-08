//Incomplete. Needs to be re-done as the library in use took exclusive control of the keys which was a big no-no

/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using RTC;
using RTC.Shortcuts;

namespace RTCV.CorruptCore
{
	public static class RTC_Hotkeys
	{
		private static HotkeyBinder _hotkeyBinder = new HotkeyBinder();
		public static Dictionary<string, Hotkey> name2HotkeyDico = new Dictionary<string, Hotkey>();

		#region HOTKEYDEFINITIONS

		public static Hotkey REMOTE_HOTKEY_MANUALBLAST			= null;
		public static Hotkey REMOTE_HOTKEY_AUTOCORRUPTTOGGLE	= null;
		public static Hotkey REMOTE_HOTKEY_ERRORDELAYDECREASE	= null;
		public static Hotkey REMOTE_HOTKEY_ERRORDELAYINCREASE	= null;
		public static Hotkey REMOTE_HOTKEY_INTENSITYDECREASE	= null;
		public static Hotkey REMOTE_HOTKEY_INTENSITYINCREASE	= null;
		public static Hotkey REMOTE_HOTKEY_GHLOADCORRUPT		= null;
		public static Hotkey REMOTE_HOTKEY_GHCORRUPT			= null;
		public static Hotkey REMOTE_HOTKEY_GHLOAD				= null;
		public static Hotkey REMOTE_HOTKEY_GHSAVE				= null;
		public static Hotkey REMOTE_HOTKEY_GHSTASHTOSTOCKPILE	= null;
		public static Hotkey REMOTE_HOTKEY_BLASTRAWSTASH		= null;
		public static Hotkey REMOTE_HOTKEY_SENDRAWSTASH			= null;
		public static Hotkey REMOTE_HOTKEY_BLASTLAYERTOGGLE		= null;
		public static Hotkey REMOTE_HOTKEY_BLASTLAYERREBLAST	= null;

		#endregion HOTKEYDEFINITIONS

		public static void InitializeHotkeySystem()
		{
			//AddPreDefinedHotkeysToList();
		}

		public static void AddPreDefinedHotkeysToList()
		{
		}

		/*
		public static void AssignAllHotkeyBindings()
		{
			_hotkeyBinder.Bind(REMOTE_HOTKEY_MANUALBLAST)?.To(()		=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_MANUALBLAST)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_AUTOCORRUPTTOGGLE)?.To(()	=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_AUTOCORRUPTTOGGLE)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_ERRORDELAYDECREASE)?.To(()	=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_ERRORDELAYDECREASE)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_INTENSITYDECREASE)?.To(()	=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_INTENSITYDECREASE)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_INTENSITYINCREASE)?.To(()	=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_INTENSITYINCREASE)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_GHLOADCORRUPT)?.To(()		=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHLOADCORRUPT)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_GHCORRUPT)?.To(()			=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHCORRUPT)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_GHLOAD)?.To(()				=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHLOAD)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_GHSAVE)?.To(()				=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHSAVE)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_GHSTASHTOSTOCKPILE)?.To(()	=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHSTASHTOSTOCKPILE)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_BLASTRAWSTASH)?.To(()		=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTRAWSTASH)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_SENDRAWSTASH)?.To(()		=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_SENDRAWSTASH)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_BLASTRAWSTASH)?.To(()		=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTRAWSTASH)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_BLASTLAYERTOGGLE)?.To(()	=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTLAYERTOGGLE)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_BLASTLAYERREBLAST)?.To(()	=> RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTLAYERREBLAST)));
		}	*/
/*
public static void AssignStaticHotkeyBinding(Hotkey key, string command)
{
	_hotkeyBinder.Bind(key).To(() => RTC_Core.SendCommandToRTC(new RTC_Command((CommandType)Enum.Parse(typeof(CommandType),command))));
	name2HotkeyDico[command] = key;

	RTCV.NetCore.Params.SaveHotkeys();
}

public static string SaveHotkeys()
{
	string data = "";
	foreach (var hotkey in name2HotkeyDico)
	{
		data = data + $"{hotkey.Value},";
	}
	return data;
}

//The save works like this. Modifier,Key,Name, . There's always a trailing , so you have to subtract by 1 when iterating through. Increment by 3 every time
public static string LoadHotkeys(string data)
{
	string[] _data = data.Split(',');
	for (int i = 0; i < _data.Length-1; i = i + 3)
	{
		Modifiers modifier = (Modifiers)Enum.Parse(typeof(Modifiers), _data[i].ToString());
		Keys keys = (Keys)Enum.Parse(typeof(Keys), _data[i + 1].ToString());
		string command = _data[i + 2].ToString();

		Hotkey key = new Hotkey(modifier,keys,command);

		AssignStaticHotkeyBinding(key, _data[i + 2].ToString());
	}
	return data;
}
}
}
/*
switch (trigger)
	{
		default:
			return false;

		case "Manual Blast":

			break;

		case "Auto-Corrupt":
			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_AUTOCORRUPTTOGGLE));
			break;

		case "Error Delay--":
			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_ERRORDELAYDECREASE));
			break;

		case "Error Delay++":
			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_ERRORDELAYINCREASE));
			break;

		case "Intensity--":
			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_INTENSITYDECREASE));
			break;

		case "Intensity++":
			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_INTENSITYINCREASE));
			break;

		case "GH Load and Corrupt":
			watch = System.Diagnostics.Stopwatch.StartNew();
			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHLOADCORRUPT));
			break;

		case "GH Just Corrupt":
			watch = System.Diagnostics.Stopwatch.StartNew();
			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHCORRUPT));
			break;

		case "GH Load":
			watch = System.Diagnostics.Stopwatch.StartNew();
			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHLOAD));
			break;

		case "GH Save":
			watch = System.Diagnostics.Stopwatch.StartNew();
			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHSAVE));
			break;

		case "Stash->Stockpile":
			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHSTASHTOSTOCKPILE));
			break;

		case "Induce KS Crash":
			RTC_RPC.Stop();
			break;

		case "Blast+RawStash":
			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTRAWSTASH));
			break;

		case "Send Raw to Stash":
			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_SENDRAWSTASH));
			break;

		case "BlastLayer Toggle":
			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTLAYERTOGGLE));
			break;

		case "BlastLayer Re-Blast":
			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTLAYERREBLAST));
			break;
	}
*/
