namespace RTCV.UI.Input
{
    public enum InputEventType
    {
        Press, Release
    }

    public class InputEvent
    {
        public LogicalButton LogicalButton;
        public InputEventType EventType;

        public override string ToString()
        {
            return string.Format("{0}:{1}", EventType.ToString(), LogicalButton.ToString());
        }
    }
}
