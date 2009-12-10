using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcExtensions.Services
{
    public class CNothing
    {
        public List<object> Pars = new List<object>();
    }

    public class Wrapper<VType>
    {
        public IMapper Mapper;

        public VType From<S>(S s)
        {
            var v = Mapper.Map<S, VType>(s);
            var n = v as CNothing;
            if (n!=null) n.Pars.Add(s);
            return v;
        }

        public VType From<S1, S2>(S1 s1, S2 s2)
        {
            var v = Mapper.Map<S1, S2, VType>(s1, s2);
            var n = v as CNothing;
            if (n != null)
            {
                n.Pars.Add(s1);
                n.Pars.Add(s2);
            }
            return v;
        }

        public VType From<S1, S2, S3>(S1 s1, S2 s2, S3 s3)
        {
            var v = Mapper.Map<S1, S2, S3, VType>(s1, s2, s3);
            var n = v as CNothing;
            if (n != null)
            {
                n.Pars.Add(s1);
                n.Pars.Add(s2);
                n.Pars.Add(s3);
            }
            return v;
        }
    }


    public interface IMapper 
    {
        T Map<S, T>(S source);
        T Map<S1, S2, T>(S1 source1, S2 source2);
        T Map<S1, S2, S3, T>(S1 source1, S2 source2, S3 source3);

        T Map<S, T>(S source, T Target);
        T Map<S1, S2, T>(S1 source1, S2 source2, T Target);
        T Map<S1, S2, S3, T>(S1 source1, S2 source2, S3 source3, T Target);

        Wrapper<VType> To<VType>();

        CNothing Nothing {get;}
    }
}
