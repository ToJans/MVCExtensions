using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Model;

namespace MvcExtensions.FNHModules.MyText
{
    public class MyTextModule : ModuleGroup
    {
        public MyTextModule()
        {
            Items.Add(new InterfaceMapModule<MyValidatedXlatText,MyValidatedXlatTextInterfaceMap>());
            Items.Add(new ConventionModule<MyTextTypeConvention>());
        }
    }
}
