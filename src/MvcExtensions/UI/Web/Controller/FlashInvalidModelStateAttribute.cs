using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcExtensions.UI.Web.Controller
{
    // loosely based on MvcContrib's ModelStateToTempDataAttribute 
    public class FlashInvalidModelStateAttribute : ActionFilterAttribute
    {
        string flashkey = "__MVCExtensions_flash_ModelState______";

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var modelState = filterContext.Controller.ViewData.ModelState;
            if ((filterContext.Result is RedirectToRouteResult || filterContext.Result is RedirectResult) && !modelState.IsValid)
                filterContext.Controller.TempData.Add(flashkey, modelState);
            base.OnActionExecuted(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var x = filterContext.Controller.TempData[flashkey] as ModelStateDictionary;
            if (x != null)
                filterContext.Controller.ViewData.ModelState.Merge(x);
            base.OnActionExecuting(filterContext);
        }
    }
}
