namespace RTCV.UI
{
    using System.Windows.Forms;

    public interface ISubForm
    {
        //Interface used for added contrals in SubForms

        bool SubForm_HasLeftButton { get; }
        bool SubForm_HasRightButton { get; }
        string SubForm_LeftButtonText { get; }
        string SubForm_RightButtonText { get; }
        void SubForm_LeftButton_Click();
        void SubForm_RightButton_Click();
        void OnShown(); //The included OnShown sucks
        void OnHidden(); //There's no OnHidden and VisibleChanged sucks
    }

    public interface ITileForm
    {
        //Interface used for added contrals in TileForms

        int TilesX { get; set; }
        int TilesY { get; set; }
        bool CanPopout { get; set; }
    }

    public interface IBlockable
    {
        //Interface used for querying blockable fornms/panels and do actions of them
        Panel blockPanel { get; set; }
        //void BlockView();
        //void UnblockView();
    }
}
