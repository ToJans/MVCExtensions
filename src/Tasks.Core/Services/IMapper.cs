using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tasks.Core.Services
{
    public interface IMapper
    {
        T Map<S, T>(S source);
        T Map<S1, S2, T>(S1 source1, S2 source2);
        T Map<S1, S2, S3, T>(S1 source1, S2 source2, S3 source3);

        T Map<S, T>(S source, T Target);
        T Map<S1, S2, T>(S1 source1, S2 source2, T Target);
        T Map<S1, S2, S3, T>(S1 source1, S2 source2, S3 source3, T Target);
    }
}
