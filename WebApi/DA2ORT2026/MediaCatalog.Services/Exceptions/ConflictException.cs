
using MediaCatalog.Common;

namespace MediaCatalog.Services.Exceptions
{
    public class ConflictException : MediaCatalogException
    {
        public ConflictException(string message) : base(message, 409) { }
    }
}
