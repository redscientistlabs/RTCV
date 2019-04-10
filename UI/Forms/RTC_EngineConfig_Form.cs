using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public partial class RTC_EngineConfig_Form : Form, IAutoColorize
	{

		public RTC_SelectBox_Form mtForm;
		
		public RTC_EngineConfig_Form()
		{
			InitializeComponent();
			LoadLists();

			mtForm = new RTC_SelectBox_Form(new ComponentForm[] {
				S.GET<RTC_VmdNoTool_Form>(),
				S.GET<RTC_VmdPool_Form>(),
				S.GET<RTC_VmdGen_Form>(),
				S.GET<RTC_VmdAct_Form>(),
				S.GET<RTC_ListGen_Form>(),
			})
			{
				popoutAllowed = false
			};

			S.GET<RTC_GeneralParameters_Form>().AnchorToPanel(pnGeneralParameters);
			S.GET<RTC_MemoryDomains_Form>().AnchorToPanel(pnMemoryDomains);
			S.GET<RTC_CorruptionEngine_Form>().AnchorToPanel(pnCorruptionEngine);
			mtForm.AnchorToPanel(pnAdvancedMemoryTools);
		}

		private void toggleLimiterBoxSource(bool setToBindingSource)
		{
			if (setToBindingSource)
			{
				S.GET<RTC_CustomEngineConfig_Form>().cbLimiterList.DisplayMember = "Name";
				S.GET<RTC_CustomEngineConfig_Form>().cbLimiterList.ValueMember = "Value";
				S.GET<RTC_CustomEngineConfig_Form>().cbLimiterList.DataSource = CorruptCore.CorruptCore.LimiterListBindingSource;


				S.GET<RTC_CustomEngineConfig_Form>().cbValueList.DisplayMember = "Name";
				S.GET<RTC_CustomEngineConfig_Form>().cbValueList.ValueMember = "Value";
				S.GET<RTC_CustomEngineConfig_Form>().cbValueList.DataSource = CorruptCore.CorruptCore.ValueListBindingSource;



				S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.DisplayMember = "Name";
				S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.ValueMember = "Value";
				S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.DataSource = CorruptCore.CorruptCore.LimiterListBindingSource;

				S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.DisplayMember = "Name";
				S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.ValueMember = "Value";
				S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.DataSource = CorruptCore.CorruptCore.ValueListBindingSource;
			}
			else
			{
				S.GET<RTC_CustomEngineConfig_Form>().cbLimiterList.DataSource = null;
				S.GET<RTC_CustomEngineConfig_Form>().cbValueList.DataSource = null;

				S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.DataSource = null;
				S.GET<RTC_CorruptionEngine_Form>().cbVectorValueList.DataSource = null;
			}
		}

		public void LoadLists()
		{
			toggleLimiterBoxSource(false);

			string[] paths = System.IO.Directory.GetFiles(CorruptCore.CorruptCore.listsDir);

			paths = paths.OrderBy(x => x).ToArray();

			List<string> hashes = Filtering.LoadListsFromPaths(paths);
			for (int i = 0; i < hashes.Count; i++)
			{
				string[] _paths = paths[i].Split('\\' , '.');
				CorruptCore.Filtering.RegisterListInUI(_paths[_paths.Length - 2], hashes[i]);
			}
			toggleLimiterBoxSource(true);
		}


	}
}
