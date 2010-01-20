using Castle.Core;
using Tasks.Core.Model;
using Tasks.Core.Controllers;
using Tasks.UI.ViewModels.Home;
using MvcExtensions.Services;
using MvcExtensions.Services.Impl;
using MvcExtensions.UI.Web.Controller;

namespace Tasks.Core.Services.Impl
{
    public class HomeContainer : MvcContainer, IMvcCustomContainer<HomeController>
    {
        public HomeContainer() 
        {
            AddComponentLifeStyle<IMapper,MyMapper>(LifestyleType.Singleton);
        }

        class MyMapper : MvcMapper
        {
            HomeController cHome=null;

            public override void Initialize()
            {
                // all mappings are explicit to avoid bugs in case of refactoring
                // if you do not require this you can delete the name & description mappings
                // because they are using the conventions

                // map viewmodels
                MapperCfg.CreateMap<Task, VMIndex.Task>()
                    .ForMember(v => v.Name, m => m.MapFrom(t => cHome.AL(t.Name, a => a.Edit(t.Id))))
                    .ForMember(v => v.Description, m => m.MapFrom(t => t.Description))
                    .ForMember(v => v.AL_Status, m => m.MapFrom(t => cHome.AL(t.Done ? "Done" : "Todo", a => a.Done(t.Id))))
                    .ForMember(v => v.AL_Delete, m => m.MapFrom(t => cHome.AL("Delete", a => a.Delete(t.Id))));

                MapperCfg.CreateMap<Task[], VMIndex>()
                    .ForMember(v => v.AllTasks, m => m.MapFrom(t => t))
                    .ForMember(v => v.HasNoTasks, m => m.MapFrom(t => t.Length == 0))
                    .ForMember(v => v.AL_AddTask, m => m.MapFrom(t => cHome.AL("Add new task", c => c.AddNewTask(null))));

                MapperCfg.CreateMap<HomeController, VMIndex>()
                    .ForMember(v => v.FlashMessage, m => m.MapFrom(c => c.FlashMessage))
                    .ForMember(v => v.FlashError, m => m.MapFrom(c => c.FlashError));


                MapperCfg.CreateMap<HomeController, VMEdit>()
                    .ForMember(v => v.FlashMessage, m => m.MapFrom(c => c.FlashMessage))
                    .ForMember(v => v.FlashError, m => m.MapFrom(c => c.FlashError));

                MapperCfg.CreateMap<Task, VMEdit>()
                    .ForMember(v => v.Name, m => m.MapFrom(t => t.Name))
                    .ForMember(v => v.Description, m => m.MapFrom(t => t.Description))
                    .ForMember(v => v.AL_CancelEdit, m => m.MapFrom(t => cHome.AL("Cancel changes", c => c.Index())))
                    .ForMember(v => v.AL_PostEdit, m => m.MapFrom(t => cHome.AL("Save changes", c => c.PostEdit(t.Id,null))));

                MapperCfg.CreateMap<IMTask, Task>()
                    .ForMember(v => v.Name, m => m.MapFrom(t => t.Name))
                    .ForMember(v => v.Description, m => m.MapFrom(t => t.Description));
            }
        }
    }
}
