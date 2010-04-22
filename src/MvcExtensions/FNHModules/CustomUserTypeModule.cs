using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.UserTypes;
using FluentNHibernate.Conventions;
using MvcExtensions.Model;
using FluentNHibernate.Automapping;

namespace MvcExtensions.FNHModules
{
    public class CustomUserTypesModule<T> : ConventionModule<MyUserTypeConvention<T>>
        where T:IUserType,new()
    { }

    public class MyUserTypeConvention<T> : UserTypeConvention<T>
        where T : IUserType, new()
    {
        public MyUserTypeConvention()
        { }
    }


}
