using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Model;
using System.Text.RegularExpressions;

namespace Tasks.Core.Model.Component
{
    public class EmailText : NonEmptyNormalText
    {

        protected Regex MyRegex = new Regex(@"[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}");


        public EmailText() { }
        public EmailText(string email) : base(email) { }

        protected override System.Text.RegularExpressions.Regex Regex
        {
            get
            {
                return MyRegex; ;
            }
        }

        protected override string CustomMessage
        {
            get
            {
                return "This is not a valid emailadres";
            }
        }

        public static implicit operator EmailText(string email)
        {
            return new EmailText(email);
        }
    }
}
