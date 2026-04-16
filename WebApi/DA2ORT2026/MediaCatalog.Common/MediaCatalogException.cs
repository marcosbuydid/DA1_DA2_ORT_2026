
namespace MediaCatalog.Common
{
    public abstract class MediaCatalogException : Exception
    {
        public int StatusCode { get;}

        protected MediaCatalogException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

    }
}
