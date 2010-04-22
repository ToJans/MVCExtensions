using MvcExtensions.Model;
using MvcExtensions.FNHModules.AuditInfo;
using System;

namespace Tasks.Core.Model
{
    public class Task : IModelId, IAuditInfo
    {
        public virtual int Id { get; set; }
        public virtual NonEmptyNormalText Name { get; set; }
        public virtual MemoText Description { get; set; }
        public virtual bool Done { get; set; }
        public virtual EmailText Contact { get; set; }

        #region IAuditInfo Members

        public virtual NormalText CreatedBy {get;set;}
        public virtual DateTime? CreatedOn { get; set; }
        public virtual NormalText UpdatedBy { get; set; }
        public virtual DateTime? UpdatedOn { get; set; }

        #endregion
    }
}
