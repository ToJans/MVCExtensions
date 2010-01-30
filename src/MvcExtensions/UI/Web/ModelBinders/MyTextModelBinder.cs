using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MvcExtensions.Model;

namespace MvcExtensions.UI.Web.ModelBinders
{
    public class MyTextModelBinder<T> : IModelBinder where T : MyText,new()
    {

        #region IModelBinder Members

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var m = (T)bindingContext.Model;
            if (m==null) m = new T();
            ValueProviderResult val;
            bindingContext.ValueProvider.TryGetValue(bindingContext.ModelName, out val);
            if (val == null)
                return m;
            string exmsg = null;
            try
            {
                m.Value = val.AttemptedValue;
            }
            catch (ArgumentNullException ex)
            {
                exmsg = ex.ParamName;
            }
            catch (ArgumentOutOfRangeException ex2)
            {
                exmsg = ex2.ParamName;
            }
            if (exmsg != null)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, exmsg);
            }
            return m;
        }

        #endregion
    }
}
