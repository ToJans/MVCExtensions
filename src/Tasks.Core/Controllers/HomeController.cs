using System.Linq;
using System.Web.Mvc;
using MvcContrib;
using MvcExtensions.Services;
using Tasks.Core.Model;
using Tasks.UI.ViewModels.Home;

namespace Tasks.Core.Controllers
{

    public class HomeController : Controller
    {
        IMapper sMap;
        IRepository sRepo;
        IUnitOfWork sUnitOfWork;

        ModelStateDictionary FlashModelState
        {
            get { return (ModelStateDictionary)TempData["mstate"]; }
            set { TempData["mstate"] = value; }
        }

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

        public ActionResult Index()
        {
            ModelState.Merge(FlashModelState);
            var tasks = sRepo.Find<Task>().ToArray();
            return View(sMap.To<VMIndex>().From(tasks,this)); 
        }

        public ActionResult AddNewTask(IMTask imtask)
        {
            if (!ModelState.IsValid)
            {
                FlashModelState = ModelState;
                FlashError = "Unable to save task";
                return this.RedirectToAction(c => c.Index());
            }
            var task = sMap.To<Task>().From(imtask);

            sRepo.SaveOrUpdate(task);
            FlashMessage = "Task \"" + task.Name + "\" saved";
            sUnitOfWork.Commit();
            return this.RedirectToAction(c => c.Index());
        }

        public ActionResult SwitchStatus(int id)
        {
            var task = sRepo.GetById<Task>(id);
            task.Done = !task.Done;
            sRepo.SaveOrUpdate(task);
            sUnitOfWork.Commit();
            FlashMessage = "Task \"" + task.Name + "\" updated";
            return this.RedirectToAction(c => c.Index());
        }

        public ActionResult Edit(int id)
        {
            ModelState.Merge(FlashModelState);
            var task = sRepo.GetById<Task>(id);
            return View(sMap.To<VMEdit>().From(task,this));
        }

        public ActionResult PostEdit(int id,IMTask imTask)
        {
            if (ModelState.IsValid)
            {
                var task = sRepo.GetById<Task>(id);
                sMap.Map(imTask, task);
                sRepo.SaveOrUpdate(task);
                FlashMessage = "Task \"" + task.Name + "\" saved";
                sUnitOfWork.Commit();
                return this.RedirectToAction(c => c.Index());
            }
            else
            {
                FlashModelState = ModelState;
                FlashError = "Unable to save task";
                return this.RedirectToAction(c => c.Edit(id));
            }
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
