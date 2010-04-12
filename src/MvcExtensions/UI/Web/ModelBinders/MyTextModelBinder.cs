using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MvcExtensions.Model;

namespace MvcExtensions.UI.Web.ModelBinders
{
    public class MyTextModelBinder<T> : IModelBinder where T : MyText, new()
    {

        #region IModelBinder Members

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var m = (T)bindingContext.Model;
            if (m == null) m = new T();
            var val = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (val == null)
                return m;
            string exmsg = null;
            if (typeof(MyValidatedText).IsAssignableFrom(m.GetType()))
            {
                var vt = m as MyValidatedText;
                exmsg = vt.Validate(val.AttemptedValue, false);
                if (string.IsNullOrEmpty(exmsg))
                {
                    m.Value = val.AttemptedValue;
                }
            }
            else
            {
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
            }
            if (exmsg != null)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, exmsg);
            }
            if (val != null)
                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, new ValueProviderResult(val.AttemptedValue, val.AttemptedValue, null));
            return m;
        }

        #endregion
    }
}
