namespace RTCV.UI.Modular
{
    public interface ISubForm
    {
        //Interface used for added contrals in SubForms

        bool HasLeftButton { get; }
        bool HasRightButton { get; }
        string LeftButtonText { get; }
        string RightButtonText { get; }
        void LeftButtonClick();
        void RightButtonClick();
        void OnShown(); //The included OnShown sucks
        void OnHidden(); //There's no OnHidden and VisibleChanged sucks
    }
}
