using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.UI.ViewModel;

namespace Tasks.UI.ViewModels.Home
{
    public class VMEdit:VMApplication
    {
        public string Name;
        public string Description;
        public VMActionLink AL_PostEdit;
        public VMActionLink AL_CancelEdit;
    }
}
