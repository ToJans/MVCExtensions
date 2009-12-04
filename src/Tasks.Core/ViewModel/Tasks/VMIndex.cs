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
        public IEnumerable<Task> AllTasks {get;set;}
        public Task SelectedTask { get; set; }
        public VMActionLink AL_AddTask { get; set; }
    }
}
