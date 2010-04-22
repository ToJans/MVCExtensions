using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcExtensions.Model;

namespace MvcExtensions.Web.Helpers
{
    public static class HTMLHelperExtensions
    {

        public static string ActionLink(this HtmlHelper h, VMActionLink va)
        {
            if (va == null) return "";
            if (va.Disabled)
                return h.Encode(va.Description);
            else
            {
                var rvd = new RouteValueDictionary();
                foreach (var q in va.Params.Keys)
                    rvd.Add(q, va.Params[q]);
                return h.ActionLink(
                    va.Description??"<< ? >>"
                    , va.Params["action"].ToString()
                    , rvd
                    ).ToString();
            }
        }

        public static MvcForm BeginForm(this HtmlHelper h, VMActionLink va)
        {
            if (va == null || va.Disabled)
                return null;
            var rvd = new RouteValueDictionary();
            foreach (var q in va.Params.Keys)
                rvd.Add(q, va.Params[q]);
            return h.BeginForm(rvd);
        }

        public static string SubmitButton(this HtmlHelper h, VMActionLink va)
        {
            return @"<button type=""submit"" >" + h.Encode(va.Description) + "</button>";
        }

        public static HtmlElement TagWrapper(this HtmlHelper h, string tag)
        {
            return h.TagWrapper(tag, null);
        }

        public static HtmlElement TagWrapper(this HtmlHelper h, string tag, object htmlattributes)
        {
            var tb = new TagBuilder(tag);
            tb.MergeAttributes(new RouteValueDictionary(htmlattributes));
            h.ViewContext.HttpContext.Response.Write(tb.ToString(TagRenderMode.StartTag));
            return new HtmlElement() { CloseTag = tb.ToString(TagRenderMode.EndTag), h = h };
        }
    }

    public class HtmlElement : IDisposable
    {
        public string CloseTag;
        public HtmlHelper h;


        #region IDisposable Members

        public void Dispose()
        {
            h.ViewContext.HttpContext.Response.Write(CloseTag);
        }

        #endregion
    }
}
