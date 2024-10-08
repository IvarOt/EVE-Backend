using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.Models
{
    public class DynamicModel : DynamicObject
    {
        private Dictionary<string, object> properties = new Dictionary<string, object>();

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            properties[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return properties.TryGetValue(binder.Name, out result);
        }

        public void ListProperties()
        {
            foreach (var prop in properties)
            {
                Console.WriteLine($"{prop.Key}: {prop.Value}");
            }
        }
    }
}
