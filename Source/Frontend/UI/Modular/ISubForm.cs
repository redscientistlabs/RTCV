namespace RTCV.UI.Modular
{
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
}
