// The quest for perfect asp.net MVC code - v0.3
//
// For a while I have been looking for the perfect ASP.Net MVC code.
// This is the cleanest code I have been able to write.
// I would like to challenge everyone to do better !!!
// 
// By this I mean creating a better controller/views if possible codewise.
// The focus is not on the layout stuff, but having it might be a plus.
//
// The scope : a very rudimentary Task list (KISS)
//
// You can download the full source here using (msys)git:
//     http://github.com/ToJans/MVCExtensions
//
// You will see it is very easy to alter, just fetch it with git and press F5
//
// Please do let me know what you think about my approach as well,
// and whether you could do better: ToJans@twitter
// Send this link to as much fellow coders as possible, so we can see lots of alternatives
//
// PS: you can also leave a comment @ my website (look at my twitter account for the url)
//
// Edit: this is my third version, and I am still looking for improvements
//
// Some noteworthy facts :
// - In the MVCapp, there only DLL directly referenced is the ViewModel DLL, 
//   so the views do NOT reference the controllers anywhere
// - The controller contains only logic & domain model objects => VERY CLEAN Controller
// - The resulting controller action model is mapped to the ViewModel using 
//   IMapper.Map<source,ViewModel>(s,vm)
// - The viewmodel should include everything that should be visible on the screen, so not only
//   data but also the actionlinks one can use
// - The actionlinks for the viewpages are defined in the IMapper, and automaticly passed on to
//   the view => you can see/alter the program flow in the mapping definitions
// - Stubbing the controller should be a piece of cake using this code, so you could use this
//   design to easily develop application mockups that are ready to be implemented once the client 
//   approves, so first build your viewmodels and views, show it to the client, and upon agreement
//   start development on the controller.... In fact I am going to test this method on my next project
//
// Kind regards,
// Tom Janssens
//
using System.Linq;
using System.Web.Mvc;
using MvcContrib;
using Tasks.Core.Model;
using Tasks.Core.Services;
using Tasks.ViewModel.Home;
using Tasks.Core.Services.Impl;

namespace Tasks.Core.Controllers
{
    public class HomeController : Controller
    {
        // for the sake of the demo we do not use DI but static instances
        static IRepository<Task> rTask = rTask ?? new FakeRepository<Task>(null);
        static IMapper sMapper = sMapper ?? new Mapper();

        public ActionResult Index()
        {
            return View(sMapper.Map<Task[], VMIndex>(rTask.Find.ToArray()));
        }

        public ActionResult AddNewTask(Task task)
        {
            rTask.SaveOrUpdate(task);
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
            return View(sMapper.Map<Task,VMEdit>(rTask.GetById(id)));
        }

        public ActionResult PostEdit(int id)
        {
            var task = rTask.GetById(id);
            UpdateModel(task);
            rTask.SaveOrUpdate(task);
            return this.RedirectToAction(c => c.Index());
        }

        public ActionResult Delete(int id)
        {
            rTask.Delete(rTask.GetById(id));
            return this.RedirectToAction(c => c.Index());
        }
    }
}
