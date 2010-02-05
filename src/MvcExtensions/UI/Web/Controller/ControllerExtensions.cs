using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcExtensions.UI.ViewModel;
using System.IO;
using System.Web;

namespace MvcExtensions.UI.Web.Controller
{

    public static class ControllerExtensions
    {
        public static VMActionLink AL<TController>(this TController controller, string description
        , Expression<Func<TController, ActionResult>> ActionExpression)
        where TController : IController
        {
            var methodCall = (MethodCallExpression)ActionExpression.Body;

            var pars = new Dictionary<string, object>();

            var ctrl = typeof(TController).Name;
            if (ctrl.EndsWith("controller", StringComparison.InvariantCultureIgnoreCase))
                ctrl = ctrl.Substring(0, ctrl.Length - "controller".Length);
            pars.Add("controller", ctrl);
            pars.Add("action", methodCall.Method.Name);

            //check parameters
            for (int i = 0; i < methodCall.Arguments.Count; i++)
            {
                string name = methodCall.Method.GetParameters()[i].Name;
                object value = null;

                switch (methodCall.Arguments[i].NodeType)
                {
                    case ExpressionType.Constant:
                        value = ((ConstantExpression)methodCall.Arguments[i]).Value;
                        break;
                    case ExpressionType.MemberAccess:
                    case ExpressionType.Convert:
                        value = Expression.Lambda(methodCall.Arguments[i]).Compile().DynamicInvoke();
                        break;
                }
                if (value != null)
                    pars.Add(name, value);
            }
            return new VMActionLink(description, pars);
        }

        /// <summary>
        /// Captures the HTML output by a controller action that returns a ViewResult
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <param name="controller"></param>
        /// <param name="viewName">The name of the view to execute</param>
        /// <returns>The HTML output from the view</returns>
        public static string CaptureView<TController>(this TController controller, string viewName)
        where TController : System.Web.Mvc.Controller
        {
            return CaptureView(controller, viewName, string.Empty, null);
        }

        /// <summary>
        /// Captures the HTML output by a controller action that returns a ViewResult
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <param name="controller"></param>
        /// <param name="viewName">The name of the view to execute</param>
        /// <param name="model">The model to pass to the view</param>
        /// <returns>The HTML output from the view</returns>
        public static string CaptureView<TController>(this TController controller, string viewName, object model)
        where TController : System.Web.Mvc.Controller
        {
            return CaptureView(controller, viewName, string.Empty, model);
        }

        /// <summary>
        /// Captures the HTML output by a controller action that returns a ViewResult
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <param name="controller"></param>
        /// <param name="viewName">The name of the view to execute</param>
        /// <param name="masterName">The master template to use for the view</param>
        /// <param name="model">The model to pass to the view</param>
        /// <returns>The HTML output from the view</returns>
        public static string CaptureView<TController>(this TController controller, string viewName, string masterName, object model)
        where TController : System.Web.Mvc.Controller
        {
            // pass the current controller context to orderController
            var controllerContext = controller.ControllerContext;

            // replace the current context with a new context that writes to a string writer
            var existingContext = System.Web.HttpContext.Current;
            var writer = new StringWriter();
            var response = new HttpResponse(writer);
            var context = new HttpContext(existingContext.Request, response) { User = existingContext.User };
            System.Web.HttpContext.Current = context;

            // execute the action

            if (model != null)
            {
                controller.ViewData.Model = model;
            }

            var result = new ViewResult
            {
                ViewName = viewName,
                MasterName = masterName,
                ViewData = controller.ViewData,
                TempData = controller.TempData
            };

            // execute the result
            result.ExecuteResult(controllerContext);

            // restore the old context
            System.Web.HttpContext.Current = existingContext;

            return writer.ToString();
        }
    }
}
