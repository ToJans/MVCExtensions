using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcExtensions.Model;

namespace Tasks.ViewModel.Home
{
    public class VMIndex
    {
        public class Task
        {
            public string Name;
            public string Description;
            public VMActionLink AL_Status;
            public VMActionLink AL_Edit;
            public VMActionLink AL_Delete;
        }

        public IEnumerable<Task> AllTasks;
        public VMActionLink AL_AddTask;
        public bool HasNoTasks;
    }
}
