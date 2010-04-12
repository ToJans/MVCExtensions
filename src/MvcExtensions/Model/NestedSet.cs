using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcExtensions.Model
{
    public enum AddWhere
    {
        Before,
        After,
        FirstChild,
        LastChild
    }

    public abstract class NestedSet<T> : IModelId
        where T : INestedSetNode
    {
        public virtual int Id { get; set; }
        public virtual IList<T> Nodes { get; set; }

        public NestedSet()
        {
            Nodes = new List<T>();
        }

        public virtual void Create(T instance)
        {
            Nodes.Clear();
            Nodes.Add(instance);
            instance.Left = 0;
            instance.Right = 1;
            instance.Depth = 0;
        }

        public virtual void Add(T instance, AddWhere where,T relative)
        {
            switch (where)
            {
                case AddWhere.Before:
                    instance.Left = relative.Left;
                    instance.Depth = relative.Depth;
                    break;
                case AddWhere.After:
                    instance.Left = relative.Right + 1;
                    instance.Depth = relative.Depth;
                    break;
                case AddWhere.FirstChild:
                    instance.Left = instance.Left + 1;
                    instance.Depth = relative.Depth+1;
                    break;
                case AddWhere.LastChild:
                    instance.Left = instance.Right + 1;
                    instance.Depth = relative.Depth + 1;
                    break;
            }
            instance.Right = instance.Left + 1;
            foreach (var x in Nodes.Where(x => x.Left >= instance.Left))
            {
                x.Left += 2;
                if (x.Right >= instance.Right)
                    x.Right += 2;
            }
            Nodes.Add(instance);
        }

        public virtual IEnumerable<T> Remove(T instance)
        {
            var l =new List<T>();
            foreach (var n in AllDescendants(instance))
            {
                yield return n;
                l.Add(n);
            }
            foreach (var n in l)
                Nodes.Remove(n);
            Nodes.Remove(instance);
            yield return instance;
            yield break ;
        }

        public virtual IEnumerable<T> AllDescendants(T instance)
        {
            return Nodes.Where(x => x.Left > instance.Left && x.Right < instance.Right);
        }

        public virtual IEnumerable<T> AllChildren(T instance)
        {
            return Nodes.Where(x => x.Depth == instance.Depth + 1 && x.Left > instance.Left && x.Right < instance.Right);
        }

        public virtual IEnumerable<T> AllLeafs(T instance)
        {
            return Nodes.Where(x => x.Right - x.Left == 1);
        }

        public virtual IEnumerable<T> AllBranches(T instance)
        {
            return Nodes.Where(x => x.Right - x.Left != 1);
        }

        public virtual IEnumerable<T> Ancestors(T instance)
        {
            return Nodes.Where(x => x.Left < instance.Left && x.Right > instance.Right).OrderByDescending(x => x.Depth);
        }
    }

    public interface INestedSetNode 
    {
        int Left { get; set; }
        int Right { get; set; }
        int Depth { get; set; }
    }
}
