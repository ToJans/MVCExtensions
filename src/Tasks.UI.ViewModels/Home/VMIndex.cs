using System.Collections.Generic;
using MvcExtensions.UI.ViewModel;

namespace Tasks.UI.ViewModels.Home
{
    public class VMIndex : VMApplication 
    {
        public class Task
        {
            public VMActionLink Name;
            public string Description;
            public VMActionLink AL_Status;
            public VMActionLink AL_Delete;
        }

        public IEnumerable<Task> AllTasks;
        public VMActionLink AL_AddTask;
        public bool HasNoTasks;
    }
}
