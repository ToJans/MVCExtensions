using System;
using System.Collections.Generic;

namespace MvcExtensions.Model
{
    public class VMActionLink
    {
        public VMActionLink(string description, Dictionary<string, object> parameters)
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
