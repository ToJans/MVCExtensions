using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using MvcExtensions.FNHModules.AuditInfo;
using MvcExtensions.Web.Binders;

namespace Tasks.Core.Services
{
    public class HttpUsernameProvider : HttpContextBoundString, IUsernameProvider 
    {
        public HttpUsernameProvider(RouteCollection routes) : base("MyUserName",routes) {}

        #region IUsernameProvider Members

        public string Username
        {
            get {
                var s =base.ContextValue;
                if (string.IsNullOrEmpty(s))
                    s="Anonymous["+System.Web.HttpContext.Current.Request.UserHostAddress+"]";
                return s;
            }
        }

        #endregion
    }
}
