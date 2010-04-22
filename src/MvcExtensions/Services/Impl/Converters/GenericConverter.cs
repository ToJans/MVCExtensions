using System;

namespace MvcExtensions.Services.Impl.Converters
{
    public class GenericConverter<TFrom,TTo> : IConverter<TFrom,TTo>
    {
        public Func<TFrom, TTo> ConvertFunc { get; set; }
        public Func<TTo, TFrom> ConvertBackFunc { get; set; }

        #region IConvertor<TFrom,TTo> Members

        public TTo Convert(TFrom source)
        {
            if (source==null)
                return default(TTo);
            else
                return ConvertFunc(source);
        }

        public TFrom ConvertBack(TTo destination)
        {
            if (destination == null)
                return default(TFrom);
            else
                return ConvertBackFunc(destination);
        }

        #endregion
    }
}
