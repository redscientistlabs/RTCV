namespace RTCV.CorruptCore.EventWarlock.Editor
{
    using System;

    /// <summary>
    /// Used to link parameters to constructor arguments for the editor
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class WarlockEditableAttribute : Attribute
    {
        readonly string customEditorID;
        public WarlockEditableAttribute(string customEditorName = null)
        {
            this.customEditorID = customEditorName;
        }

        public string CustomEditorID
        {
            get { return customEditorID; }
        }
    }
}
