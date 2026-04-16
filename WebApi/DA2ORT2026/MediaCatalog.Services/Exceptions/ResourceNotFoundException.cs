
using MediaCatalog.Common;

namespace MediaCatalog.Services.Exceptions
{
    public class ResourceNotFoundException : MediaCatalogException
    {
        public ResourceNotFoundException(string message) : base(message,404) { }
    }
}
