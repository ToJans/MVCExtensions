
namespace MvcExtensions.Services.Impl.Converters
{
    public class PassThroughConverter<T> : IConverter<T, T>
    {

        #region IConverter<T,T> Members

        public T Convert(T source)
        {
            return source;
        }

        public T ConvertBack(T destination)
        {
            return destination;
        }

        #endregion
    }

}
