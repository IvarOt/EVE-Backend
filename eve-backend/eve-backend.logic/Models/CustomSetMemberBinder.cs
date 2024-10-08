using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.Models
{
    public class CustomSetMemberBinder : SetMemberBinder
    {
        public CustomSetMemberBinder(string name, bool ignoreCase) : base(name, ignoreCase) { }

        public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject? errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }
}
