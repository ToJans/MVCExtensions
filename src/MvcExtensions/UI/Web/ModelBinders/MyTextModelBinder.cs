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
            if (!m.CanBeEmpty && string.IsNullOrEmpty(val.AttemptedValue))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, 
                    "Text can not be empty");
            }
            else if (val.AttemptedValue.Length > m.Length)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, 
                    "Max lenght is " + m.Length + " characters");
            }
            else if (m.Regex!=null && !m.Regex.IsMatch(val.AttemptedValue))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, 
                    "Text does not match the expected format: "+m.Regex.ToString());
            } else 
            {
                m.Value = val.AttemptedValue;
            }
            return m;
        }

        #endregion
    }
}
