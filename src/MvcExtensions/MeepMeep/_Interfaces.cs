using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcExtensions.MeepMeep
{
    public interface IViewFinder
    {
        object GetView(object viewmodel);
    }

    public interface IViewViewmodelBinder
    {
        void Set(object view, object viewmodel);
        object Get(object view);
    }

    public interface ICommand
    {
        bool CanExecute { get; }
        void Execute();
    }
}
