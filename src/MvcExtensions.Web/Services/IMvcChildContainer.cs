﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcExtensions.Web.Services
{
    public interface IMvcCustomContainer<T> where T:IController
    {
    }
}
