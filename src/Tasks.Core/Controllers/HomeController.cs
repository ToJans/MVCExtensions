using System.Linq;
using System.Web.Mvc;
using MvcContrib;
using MvcContrib.Filters;
using MvcExtensions.Model;
using MvcExtensions.Services;
using Tasks.Core.Model;
using Tasks.Core.Model.Component;
using Tasks.UI.ViewModels.Home;
using MvcExtensions.UI.Web.Controller;


namespace Tasks.Core.Controllers
{

    public class HomeController : Controller
    {
        IMapper sMap;
        IRepository sRepo;
        IUnitOfWork sUnitOfWork;

        public string FlashMessage
        {
            get { return (string)TempData["msg"]; }
            set { TempData["msg"] = value; }
        }

        public string FlashError
        {
            get { return (string)TempData["err"]; }
            set { TempData["err"] = value; }
        }

        public HomeController(IMapper sMap, IRepository sRepo, IUnitOfWork sUnitOfWork)
        {
            this.sMap = sMap;
            this.sRepo = sRepo;
            this.sUnitOfWork = sUnitOfWork;
        }

        public class IMTask
        {
            public NonEmptyNormalText Name { get; set; }
            public MemoText Description { get; set; }
            public EmailText Contact { get; set; }
        }

        [HttpGet]
        public ActionResult Index()
        {
            var tasks = sRepo.Find<Task>().ToArray();
            return View(sMap.To<VMIndex>().From(tasks, this));
        }

        [HttpPost]
        public ActionResult Index(IMTask im)
        {
            if (!ModelState.IsValid)
            {
                FlashError = "Unable to save task";
                var tasks = sRepo.Find<Task>().ToArray();
                return View(sMap.To<VMIndex>().From(tasks,ModelState));
            }
            else
            {
                var task = sMap.To<Task>().From(im);
                sRepo.SaveOrUpdate(task);
                FlashMessage = "Task \"" + task.Name + "\" saved";
                sUnitOfWork.Commit();
                return this.RedirectToAction(x => x.Index());
            }
        }

        public ActionResult SwitchStatus(int id)
        {
            var task = sRepo.GetById<Task>(id);
            task.Done = !task.Done;
            sRepo.SaveOrUpdate(task);
            sUnitOfWork.Commit();
            FlashMessage = "Task \"" + task.Name + "\" updated";
            return this.RedirectToAction(c => c.Index(null));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var task = sRepo.GetById<Task>(id);
            return View(sMap.To<VMEdit>().From(task, this));
        }

        [HttpPost]
        public ActionResult Edit(int id,IMTask imTask)
        {
            var task = sRepo.GetById<Task>(id);
            if (ModelState.IsValid)
            {
                sMap.Map(imTask, task);
                sRepo.SaveOrUpdate(task);
                FlashMessage = "Task \"" + task.Name + "\" saved";
                sUnitOfWork.Commit();
                return this.RedirectToAction(c => c.Index());
            }
            else
            {
                FlashError = "Unable to save task";
            }
            return View(sMap.To<VMEdit>().From(task,this));
        }

        public ActionResult Delete(int id)
        {
            var task = sRepo.GetById<Task>(id);
            sRepo.Delete(task);
            FlashMessage = "Task \"" + task.Name + "\" deleted";
            sUnitOfWork.Commit();
            return this.RedirectToAction(c => c.Index());
        }
    }
}
