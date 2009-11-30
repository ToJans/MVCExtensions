using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcExtensions.Model
{
    public class VMActionLink
    {
        internal VMActionLink(string description, Dictionary<string, object> parameters)
        {
            Description = description;
            Params = parameters;
            Disabled = false;
        }

        public VMActionLink Disable()
        {
            return this.Disable(true);
        }

        public VMActionLink Disable(bool disable)
        {
            Disabled = disable;
            return this;
        }

        public String Description { get; private set; }
        public Dictionary<string, object> Params { get; private set; }
        public bool Disabled { get; set; }
    }

}
