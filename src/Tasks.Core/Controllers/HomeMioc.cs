using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Services.Impl;
using MvcExtensions.Controller;
using Tasks.Core.Model;
using Tasks.ViewModel.Home;
using Tasks.Core.Controllers;
using MvcExtensions.Services;

namespace Tasks.Core.Services.Impl
{
    public class HomeMioc : Mioc, IMiocService<HomeController>
    {
        IRepository<Task> rTask;

        public HomeMioc(IIOC ioc,IRepository<Task> rTask) : base(ioc)
        {
            this.rTask = rTask;
        }
        
        HomeController cHome;

        public override void Initialize()
        {

            // all mappings are explicit to avoid bugs in case of refactoring
            // if you do not requite this you can delete the name & description mappings
            // because they are using the conventions


            // map viewmodels
            MapperCfg.CreateMap<Task, VMIndex.Task>()
                .ForMember(v => v.Name, m => m.MapFrom(t => t.Name))
                .ForMember(v => v.Description, m => m.MapFrom(t => t.Description))
                .ForMember(v => v.AL_Status, m => m.MapFrom(t => cHome.AL(t.Done ? "Done" : "Todo", a => a.Done(t.Id))))
                .ForMember(v => v.AL_Edit, m => m.MapFrom(t => cHome.AL("Edit", a => a.Edit(t.Id))))
                .ForMember(v => v.AL_Delete, m => m.MapFrom(t => cHome.AL("Delete", a => a.Delete(t.Id))));

            MapperCfg.CreateMap<Task[], VMIndex>()
                .ForMember(v => v.AllTasks, m => m.MapFrom(t => t))
                .ForMember(v => v.HasNoTasks, m => m.MapFrom(t => t.Length == 0))
                .ForMember(v => v.AL_AddTask, m => m.MapFrom(t => cHome.AL("Add new task", c => c.AddNewTask(null))));

            MapperCfg.CreateMap<Task, VMEdit>()
                .ForMember(v => v.Name, m => m.MapFrom(t => t.Name))
                .ForMember(v => v.Description, m => m.MapFrom(t => t.Description))
                .ForMember(v => v.AL_CancelEdit, m => m.MapFrom(t => cHome.AL("Cancel changes", c => c.Index())))
                .ForMember(v => v.AL_PostEdit, m => m.MapFrom(t => cHome.AL("Save changes", c => c.PostEdit(t.Id))));

        }
    }
}
