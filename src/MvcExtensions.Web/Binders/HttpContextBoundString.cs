using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using MvcExtensions.Services.Impl.Converters;

namespace MvcExtensions.Web.Binders
{

    public class HttpContextBoundString : HttpContextBoundObject<String>
    {
        public HttpContextBoundString(string name,RouteCollection routes)
            : base(routes,new PassThroughConverter<String>())
        {
            this.Name += name;
        }
    }
}
