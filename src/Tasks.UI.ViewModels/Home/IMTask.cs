using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Model;

namespace Tasks.UI.ViewModels.Home
{
    public class IMTask
    {
        public NonEmptyNormalText Name { get; set; }
        public MemoText Description { get; set; }
    }
}
