using System;
using MvcExtensions.Model;

namespace MvcExtensions.FNHModules.AuditInfo
{
    public interface IAuditInfo
    {
         NormalText CreatedBy { get; set; }
         DateTime? CreatedOn { get; set; }
         NormalText UpdatedBy { get; set; }
         DateTime? UpdatedOn { get; set; }
    }
}
