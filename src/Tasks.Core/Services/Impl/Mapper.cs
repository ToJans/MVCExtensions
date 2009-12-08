using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using am = AutoMapper;
using Tasks.Core.Model;
using Tasks.ViewModel.Home;
using Tasks.Core.Controllers;
using MvcExtensions.Controller;


namespace Tasks.Core.Services.Impl
{
    // lousy wrapper class around the static AutoMapper instance
    public class Mapper : IMapper
    {
        public Mapper()
        {
            Register();
        }

        #region IMap Members

        public T Map<S, T>(S source)
        {
            return am.Mapper.Map<S, T>(source);
        }

        public T Map<S1, S2, T>(S1 source1, S2 source2)
        {
            return Map(source2, am.Mapper.Map<S1, T>(source1));
        }

        public T Map<S1, S2, S3, T>(S1 source1, S2 source2, S3 source3)
        {
            return Map(source2, source3, am.Mapper.Map<S1, T>(source1));
        }

        public T Map<S, T>(S source, T target)
        {
            return am.Mapper.Map<S, T>(source, target);
        }

        public T Map<S1, S2, T>(S1 source1, S2 source2, T Target)
        {
            return am.Mapper.Map(source1, am.Mapper.Map(source2, Target));
        }

        public T Map<S1, S2, S3, T>(S1 source1, S2 source2, S3 source3, T Target)
        {
            return Map(source1, source2, am.Mapper.Map(source3, Target));
        }

        #endregion


        protected static bool isRegistered = false;

        protected static HomeController cHome;

        // mappings registration
        public void Register()
        {
            if (isRegistered)
                return;

            isRegistered = true;

            // all mappings are explicit to avoid bugs in case of refactoring
            // if you do not requite this you can delete the name & description mappings
            // because they are using the conventions

            am.Mapper.CreateMap<Task, VMIndex.Task>()
                .ForMember(v => v.Name, m => m.MapFrom(t => t.Name))
                .ForMember(v => v.Description, m => m.MapFrom(t => t.Description))
                .ForMember(v => v.AL_Status, m => m.MapFrom(t => cHome.AL(t.Done ? "Done" : "Todo", a => a.Done(t.Id))))
                .ForMember(v => v.AL_Edit, m => m.MapFrom(t => cHome.AL("Edit", a => a.Edit(t.Id))))
                .ForMember(v => v.AL_Delete, m => m.MapFrom(t => cHome.AL("Delete", a => a.Delete(t.Id))));

            am.Mapper.CreateMap<Task[], VMIndex>()
                .ForMember(v => v.AllTasks, m => m.MapFrom(t => t))
                .ForMember(v => v.HasNoTasks, m => m.MapFrom(t=>t.Length==0))
                .ForMember(v => v.AL_AddTask, m => m.MapFrom(t => cHome.AL("Add new task", c => c.AddNewTask(null))));

            am.Mapper.CreateMap<Task, VMEdit>()
                .ForMember(v => v.Name, m => m.MapFrom(t => t.Name))
                .ForMember(v => v.Description, m => m.MapFrom(t => t.Description))
                .ForMember(v => v.AL_CancelEdit, m => m.MapFrom(t => cHome.AL("Cancel changes", c => c.Index())))
                .ForMember(v => v.AL_PostEdit, m => m.MapFrom(t => cHome.AL("Save changes", c => c.PostEdit(t.Id))));
        }
    }
}
