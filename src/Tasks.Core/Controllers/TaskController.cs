using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using MvcContrib;
using MvcExtensions.Controller;

using Tasks.Core.Model;
using Tasks.Core.Services;
using Tasks.Core.ViewModel.Tasks;
using Tasks.Core.Interfaces;

namespace Tasks.Core.Controllers
{
    public class TaskController : Controller
    {
        // for the sake of the demo we do not use DI 
        // but just a static class instance here with a fake repo
        static IRepository<Task> rTask = rTask ?? new FakeRepository<Task>(null);

        public ActionResult Index()
        {
            ViewData.Model = new VMIndex()
            {
                AllTasks = rTask.Find.OrderBy(o => o.Name),
                AL_AddTask = this.ActionLink("Add new task",c=>c.AddNewTask(null,null))
            };
            return View();
        }

        public ActionResult AddNewTask(string name,string description)
        {
            var t = new Task() { Description = description, Name = name };
            rTask.SaveOrUpdate(t);
            return this.RedirectToAction(c => c.Index());
        }

        public ActionResult Done(int id)
        {
            var t = rTask.GetById(id);
            t.Done = !t.Done;
            rTask.SaveOrUpdate(t);
            return this.RedirectToAction(c=>c.Index());
        }

        public ActionResult Edit(int id)
        {
            var t = rTask.GetById(id);
            ViewData.Model = new VMEdit()
            {
                 Name = t.Name,
                 Description = t.Description,
                 AL_PostEdit = this.ActionLink("Save changes",c=>c.PostEdit(t.Id,null,null)),
                 AL_CancelEdit = this.ActionLink("Cancel changes",c=>c.Index())
            };
            return View();
        }

        public ActionResult PostEdit(int id,string name,string description)
        {
            var t = rTask.GetById(id);
            t.Name = name;
            t.Description = description;
            rTask.SaveOrUpdate(t);
            return this.RedirectToAction(c => c.Index());
        }

        public ActionResult Delete(int id)
        {
            rTask.Delete(rTask.GetById(id));
            return this.RedirectToAction(c => c.Index());
        }
    
    }
}
