using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Model;

namespace MvcExtensions.Web.Helpers
{
    public static class IEnumerableHelper
    {
        public static IEnumerable<T> Page<T>(this IEnumerable<T> src, int? page, int? pagesize)
        {
            var p = page ?? 1;
            var s = pagesize?? 10;
            s = s<0?10:s;
            return src.Skip((p-1) * s).Take(s);
        }

        public static IEnumerable<VMActionLink> Pager<T>(this IEnumerable<T> src,VMActionLink baselink, int? page, int? pagesize)
        {
            var basedict = new Dictionary<string,object>(baselink.Params);
            if (basedict.ContainsKey("page"))
                basedict.Remove("page");
            if (basedict.ContainsKey("pagesize"))
                basedict.Remove("pagesize");

            var p = page ?? 1;
            var ps = pagesize ?? 10;
            var pages = (src.Count()) / ps + 1;
            if ((pages-1) * ps >= src.Count())
                pages -= 1;

            yield return src.GetPageLink(baselink, "<<", 1, ps,p);
            yield return src.GetPageLink(baselink, "<", p>1?p-1:p, ps, p);
            yield return src.GetPageLink(baselink, String.Format("{0} / {1}",p,pages), p, ps, p);
            yield return src.GetPageLink(baselink, ">", p<pages?p+1:p, pagesize, p);
            yield return src.GetPageLink(baselink, ">>", pages , pagesize, p);
        }

        public static VMActionLink GetPageLink<T>(this IEnumerable<T> src, VMActionLink baselink, string description,int? page, int? pagesize,int? currentpage)
        {
            var p = page ?? 1;
            var s = pagesize ?? 10;
            s = s < 0 ? 10 : s;
            var basedict = new Dictionary<string,object>(baselink.Params);
            if (basedict.ContainsKey("page"))
                basedict.Remove("page");
            if (basedict.ContainsKey("pagesize"))
                basedict.Remove("pagesize");
            basedict.Add("page",p);
            basedict.Add("pagesize",s);
            return new VMActionLink(description, basedict).Disable(p == currentpage);
        }

    }
}
