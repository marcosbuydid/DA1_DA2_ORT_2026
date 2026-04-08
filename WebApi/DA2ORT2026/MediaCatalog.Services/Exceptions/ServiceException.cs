
using MediaCatalog.Common;

namespace MediaCatalog.Services.Exceptions
{
    public class ServiceException : MediaCatalogException
    {
        public ServiceException(string message) : base(message,422) { }
    }
}
