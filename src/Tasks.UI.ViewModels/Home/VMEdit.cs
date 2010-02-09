using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.UI.ViewModel;

namespace Tasks.UI.ViewModels.Home
{
    public class VMEdit:VMApplication
    {
        public string NameValue;
        public string DescriptionValue;
        public string ContactValue;
        public VMActionLink AL_PostEdit;
        public VMActionLink AL_CancelEdit;
    }
}
