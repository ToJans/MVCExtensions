using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tasks.Core.Controllers;
using Tasks.Core.Model;
using MvcExtensions.Model;
using MvcExtensions.Controller;

namespace Tasks.Core.ModelExtensions
{
    public static class TaskExtensions
    {
        static TaskController ctrl;

        public static VMActionLink AL_Status(this Task task)
        {
            return ctrl.ActionLink(task.Done?"Done":"Todo", a=>a.Done(task.Id));
        }

        public static VMActionLink AL_Edit(this Task task)
        {
            return ctrl.ActionLink("Edit", a => a.Edit(task.Id));
        }

        public static VMActionLink AL_Delete(this Task task)
        {
            return ctrl.ActionLink("Delete", a => a.Delete(task.Id));
        }
    }
}
