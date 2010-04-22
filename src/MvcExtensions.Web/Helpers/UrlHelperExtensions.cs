using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcExtensions.Model;

namespace MvcExtensions.Web.Helpers
{
    public static class UrlHelperExtensions
    {
        public static string ActionLink(this UrlHelper u, VMActionLink va)
        {
            if (va == null) return "";
            if (va.Disabled)
                return null;
            else
            {
                var rvd = new RouteValueDictionary();
                foreach (var q in va.Params.Keys)
                    rvd.Add(q, va.Params[q]);
                return u.RouteUrl(rvd);
            }
        }
    }
}
