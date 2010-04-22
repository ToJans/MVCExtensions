using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace MvcExtensions.FNHModules.Conventions
{
    public class CascadeSaveOrUpdateConvention : IHasOneConvention, IHasManyConvention, IReferenceConvention
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
