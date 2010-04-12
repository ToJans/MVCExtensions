using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;

namespace MvcExtensions.UI.Web.Helpers
{
    public class CultureHelper : IDisposable 
    {
        CultureInfo oldculture = Thread.CurrentThread.CurrentCulture;
        CultureInfo olduiculture = Thread.CurrentThread.CurrentUICulture;

        public CultureHelper(string newculture)
        {
            if (newculture == oldculture.Name) return;
            var cult = new CultureInfo(newculture);
            Thread.CurrentThread.CurrentCulture = cult;
            Thread.CurrentThread.CurrentUICulture = cult;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = oldculture;
            Thread.CurrentThread.CurrentUICulture = olduiculture;
        }

        #endregion
    }
}
