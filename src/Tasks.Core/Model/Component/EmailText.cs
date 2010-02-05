using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Model;
using System.Text.RegularExpressions;

namespace Tasks.Core.Model.Component
{
    public class EmailText: NonEmptyNormalText
    {
        Regex _regex = new Regex(@"[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}");

        protected override Regex Regex
        {
            get
            {
                return _regex;
            }
        }
    }
}
