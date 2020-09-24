namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    using System;
    using System.Windows.Forms;
    using RTCV.CorruptCore;

    internal partial class ClusterEngineControl : EngineConfigControl
    {
        internal ClusterEngineControl()
        {
            InitializeComponent();

            cbClusterLimiterList.DataSource = null;
            cbClusterLimiterList.DisplayMember = "Name";
            cbClusterLimiterList.ValueMember = "Value";
        }

        private void UpdateClusterLimiterList(object sender, EventArgs e)
        {
            ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)sender).SelectedItem;
            if (item != null)
            {
                CorruptCore.ClusterEngine.LimiterListHash = item.Value;
            }
        }

        private void UpdateClusterChunkSize(object sender, EventArgs e)
        {
            CorruptCore.ClusterEngine.ChunkSize = (int)clusterChunkSize.Value;
        }

        private void UpdateClusterModifier(object sender, EventArgs e)
        {
            ClusterEngine.Modifier = (int)clusterChunkModifier.Value;
        }

        private void UpdateClusterMethod(object sender, EventArgs e)
        {
            ClusterEngine.ShuffleType = cbClusterMethod.SelectedItem.ToString();

            if (cbClusterMethod.SelectedItem.ToString().ToLower().Contains("rotate"))
            {
                clusterChunkModifier.Enabled = true;
            }
            else
            {
                clusterChunkModifier.Enabled = false;
            }
        }

        private void UpdateClusterSplitUnits(object sender, EventArgs e)
        {
            ClusterEngine.OutputMultipleUnits = clusterSplitUnits.Checked;
        }

        private void UpdateClusterDirection(object sender, EventArgs e)
        {
            ClusterEngine.Direction = clusterDirection.SelectedItem.ToString();
        }

        private void UpdateClusterFilterAll(object sender, EventArgs e)
        {
            ClusterEngine.FilterAll = clusterFilterAll.Checked;
        }
    }
}
