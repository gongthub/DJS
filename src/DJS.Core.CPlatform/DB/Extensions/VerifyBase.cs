using DJS.Core.CPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Core.CPlatform.DB.Extensions
{
    [AttributeUsage(AttributeTargets.All)]

    public class VerifyAttribute : Attribute
    {
        private VerifyType[] _attrs;

        public VerifyAttribute(params VerifyType[] attrs)
        {
            _attrs = attrs;
        }
        public VerifyType[] Verify { get { return _attrs; } }
    }
    [AttributeUsage(AttributeTargets.All)]

    public class VerifyHasMsgAttribute : Attribute
    {
        private Dictionary<VerifyType, string> _attrs;

        public VerifyHasMsgAttribute(Dictionary<VerifyType, string> attrs)
        {
            _attrs = attrs;
        }
        public Dictionary<VerifyType, string> VerifyHasMsg { get { return _attrs; } }
    }
}
