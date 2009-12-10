using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tasks.Core.Services;
using MvcExtensions.Model;

namespace Tasks.Core.Model
{
    public class Task : IModelId
    {
        public int Id { get; set; }
        public string Name {get;set;}
        public string Description { get; set; }
        public bool Done { get; set; }
    }
}
