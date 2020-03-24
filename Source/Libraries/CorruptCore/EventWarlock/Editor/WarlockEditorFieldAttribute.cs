using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.CorruptCore.EventWarlock.Editor
{
    /// <summary>
    /// Used to link parameters to constructor arguments for the editor
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class WarlockEditorFieldAttribute : Attribute
    {
        readonly string fieldName;
        public WarlockEditorFieldAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }

        public string FieldName
        {
            get { return fieldName; }
        }
    }
}
