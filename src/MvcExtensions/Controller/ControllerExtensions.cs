using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcExtensions.Model;

namespace MvcExtensions.Controller
{

    public static class IControllerExtensions
    {
        public static VMActionLink ActionLink<TController>(this TController controller, string description
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

    }
}
