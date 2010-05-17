using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcExtensions.MeepMeep
{
    public abstract class ConventionViewModelBinder : IViewFinder,IViewViewmodelBinder
    {

        #region IViewFinder Members

        public object GetView(object viewmodel)
        {
            return null;
        }

        #endregion

        #region IViewViewmodelBinder Members

        public abstract void Set(object view, object viewmodel);

        public abstract object Get(object view);

        #endregion
    }
}
