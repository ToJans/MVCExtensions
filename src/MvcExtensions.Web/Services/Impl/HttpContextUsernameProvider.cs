using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.FNHModules.AuditInfo;

namespace MvcExtensions.Web.Services.Impl
{
    public class HttpContextUsernameProvider : IUsernameProvider
    {

        #region IUsernameProvider Members

        public string Username
        {
            get {
                var ctx = System.Web.HttpContext.Current;
                if (ctx == null)
                    return null;
                if (ctx.User == null || ctx.User.Identity == null || ctx.User.Identity.Name == null)
                    return "Anonymous [" + ctx.Request.UserHostAddress + "]";
                else
                    return ctx.User.Identity.Name;
            }
        }

        #endregion
    }
}
