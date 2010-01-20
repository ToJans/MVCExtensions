using MvcExtensions.Model;

namespace Tasks.Core.Model
{
    public class Task : IModelId
    {
        public virtual int Id { get; set; }
        public virtual NonEmptyNormalText Name { get; set; }
        public virtual MemoText Description { get; set; }
        public virtual bool Done { get; set; }
    }
}
