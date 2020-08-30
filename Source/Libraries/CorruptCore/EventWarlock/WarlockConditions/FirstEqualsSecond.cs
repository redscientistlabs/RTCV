namespace RTCV.CorruptCore.EventWarlock.WarlockConditions
{
    using System;
    using EventWarlock;
    using Editor;

    /// <summary>
    /// Example conditional
    /// </summary>
    [Serializable]
    [WarlockEditable]
    public class FirstEqualsSecond : EWConditional
    {
        [WarlockEditorField("First string")]  string a;
        [WarlockEditorField("Second string")] string b;

        public FirstEqualsSecond(string a, string b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool Evaluate(Grimoire grimoire)
        {
            return a == b;
        }
    }
}
