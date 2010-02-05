using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace MvcExtensions.Services.Impl.FluentNHibernate
{
    public class CascadeSaveUpdateConvention : IHasOneConvention, IHasManyConvention, IReferenceConvention
    {
        public void Apply(IOneToOneInstance instance)
        {
            instance.Cascade.SaveUpdate();
        }

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Cascade.SaveUpdate();
        }

        public void Apply(IManyToOneInstance instance)
        {
            instance.Cascade.SaveUpdate();
        }
    }
}
