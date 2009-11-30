using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tasks.Core.Model;
using MvcExtensions.Model;

namespace Tasks.Core.ViewModel.Tasks
{
    public class VMEdit
    {
        public string Name;
        public string Description;
        public VMActionLink AL_PostEdit;
        public VMActionLink AL_CancelEdit;
    }
}
