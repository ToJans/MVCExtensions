using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcExtensions.FNHModules
{
    public class DefaultMvcExtensionsModules : ModuleGroup
    {
        public DefaultMvcExtensionsModules()
        {
            Items.Add(new ConventionModule<Conventions.CascadeSaveOrUpdateConvention>());
            Items.Add(new MyText.MyTextModule());
            Items.Add(new CustomUserTypesModule<CustomUserTypes.BitmapUserType>());
            Items.Add(new CustomUserTypesModule<CustomUserTypes.ColorUserType>());
        }
    }
}
