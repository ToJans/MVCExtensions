using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tasks.Core.Model;
using MvcExtensions.Model;

namespace Tasks.Core.ViewModel.Tasks
{
    public class VMIndex
    {
        public IEnumerable<Task> AllTasks;
        public Task SelectedTask;
        public VMActionLink AL_AddTask;
    }
}
