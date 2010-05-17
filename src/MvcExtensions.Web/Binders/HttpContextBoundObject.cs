using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Services;
using System.Web;
using System.Web.Routing;

namespace MvcExtensions.Web.Binders
{
    public class HttpContextBoundObject<T> : IContextBound<T> 
    {
        RouteCollection Routes;
        IConverter<string, T> Converter;
        public double CookieLifetimeInDays = 365;
        string localvalue = null;


        public string Name {get;set;}


        public HttpContextBoundObject(RouteCollection routes,IConverter<string,T> Converter)
        {
            this.Routes = routes;
            this.Name = "___reg___" + typeof(T).FullName;
            this.Converter = Converter;
        }

        #region IContextResolver Members

        public T ContextValue
        {
            get
            {
                if (localvalue != null)
                    return Converter.Convert(localvalue);
                string n = null;
                var rd = Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));
                if (rd.Values.ContainsKey(Name))
                    n=rd.Values[Name].ToString();
                else if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form[Name]))
                {
                    n=HttpContext.Current.Request.Form[Name];
                }
                else if (HttpContext.Current.Request.Cookies[Name]!=null)
                    n = HttpContext.Current.Request.Cookies[Name].Value;

                var x = default(T);
                if (n != null)
                    x = Converter.Convert(n);
                if (x == null)
                    x = default(T);
                 this.ContextValue = x;
                 return x;
            }
            set
            {
                var key = Converter.ConvertBack(value);
                if (key != null)
                {
                    var c = new HttpCookie(Name, key);
                    c.Expires = DateTime.Now.AddDays(CookieLifetimeInDays);
                    HttpContext.Current.Response.Cookies.Set(c);
                    localvalue = key;
                }
                else
                {

                }


            }
        }

        #endregion
    }
}
