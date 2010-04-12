using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using MvcExtensions.Model;
using FluentNHibernate.Conventions.Instances;

namespace MvcExtensions.Services.Impl.FluentNHibernate
{
    class MyTextTypeConvention : IPropertyConvention
    {
        #region IConvention<IPropertyInspector,IPropertyInstance> Members

        static void SetLength<MyText> (IPropertyInstance instance,int len)
        {
            if (instance.EntityType==typeof(MyText) && instance.Property.PropertyType == typeof(string))
            {
                instance.Length(len);
            }

        }

        public void Apply(IPropertyInstance instance)
        {
            if (typeof(MyText).IsAssignableFrom(instance.EntityType))
            {
                SetLength<ShortText>(instance, 16);
                SetLength<XShortText>(instance, 16);
                SetLength<NonEmptyShortText>(instance, 16);
                SetLength<XNonEmptyShortText>(instance, 16);

                SetLength<NormalText>(instance, 256);
                SetLength<XNormalText>(instance, 256);
                SetLength<NonEmptyNormalText>(instance, 256);
                SetLength<XNonEmptyNormalText>(instance, 256);

                SetLength<LongText>(instance, 1024);
                SetLength<XLongText>(instance, 1024);
                SetLength<NonEmptyLongText>(instance, 1024);
                SetLength<XNonEmptyLongText>(instance, 1024);

                SetLength<MemoText>(instance, 16384);
                SetLength<XMemoText>(instance, 16384);
                SetLength<NonEmptyMemoText>(instance, 16384);
                SetLength<XNonEmptyMemoText>(instance, 16384);
            }

        }

        #endregion
    }
}
