using System.Collections.Generic;
using MvcExtensions.UI.ViewModel;

namespace Tasks.UI.ViewModels.Home
{
    public class VMIndex : VMApplication 
    {
        public class Task
        {
            public VMActionLink Name;
            public string DescriptionValue;
            public string ContactValue;
            public VMActionLink AL_Status;
            public VMActionLink AL_Delete;
        }

        public IEnumerable<Task> AllTasks;
        public VMActionLink AL_AddTask;
        public bool HasNoTasks;
        // IMTask
        public string Name;
        public string Description;
        public string Contact;
    }
}
