// For a while I have been looking for the perfect ASP.Net MVC code.
// This is the cleanest code I have been able to write.
// I would like to challenge everyone to do better !!!
//
// By this I mean creating a better controller/views if possible codewise.
// The focus is not on the layout stuff, but having it might be a plus.
//
// The scope : a very rudimentary Task list (KISS)
//
// Some more info about my approach :
// It uses view models which contain both data and ActionLink references
// (like ICommand in WPF M-V-VM )
//
// You can download the full source here :
//     http://github.com/ToJans/MVCExtensions/tree/master/src/
//
// You will see it is very easy to alter, just download it and press F5
//
// Please do let me know what you think about my approach as well,
// and whether you could do better: ToJans@twitter
// Send this link to as much fellow coders as possible, so we can see lots of alternatives
//
// PS: you can also leave a comment @ my website (look at my twitter account)


using System.Linq;
using System.Web.Mvc;

using MvcContrib;
using MvcExtensions.Controller;
using MvcExtensions.Model;

using Tasks.Core.Model;
using Tasks.Core.Services;
using Tasks.ViewModel.Home;
using Tasks.Core.Interfaces;

namespace Tasks.Core.Controllers
{
    public class HomeController : Controller
    {
        // for the sake of the demo we do not use DI 
        // but just a static class instance here with a fake repo
        static IRepository<Task> rTask = rTask ?? new FakeRepository<Task>(null);

        public ActionResult Index()
        {
            return View(new VMIndex()
            {
                AllTasks = rTask.Find.OrderBy(o => o.Name).Select(t=> new VMIndex.Task() {
                     Name = t.Name,
                     Description = t.Description,
                     AL_Status = this.AL(t.Done?"Done":"Todo", a=>a.Done(t.Id)),
                     AL_Edit = this.AL("Edit", a => a.Edit(t.Id)),
                     AL_Delete = this.AL("Delete", a=>a.Delete(t.Id))
                }),
                AL_AddTask = this.AL("Add new task",c=>c.AddNewTask(null,null))
            });
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
            return View(new VMEdit()
            {
                 Name = t.Name,
                 Description = t.Description,
                 AL_PostEdit = this.AL("Save changes",c=>c.PostEdit(t.Id,null,null)),
                 AL_CancelEdit = this.AL("Cancel changes",c=>c.Index())
            });
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
