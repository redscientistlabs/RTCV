namespace RTCV.CorruptCore.EventWarlock.WarlockActions
{
    using System;
    using RTCV.CorruptCore.EventWarlock.Editor;

    /// <summary>
    /// Example action
    /// </summary>
    [Serializable]
    [WarlockEditable]
    public class WarlockActionEcho : WarlockAction
    {
        [WarlockEditorField("Data")] string data;

        public WarlockActionEcho(string data)
        {
            this.data = data;
        }

        public override void DoAction(Grimoire grimoire)
        {
            Console.WriteLine("Repeating: " + data);
        }
    }
}
