
using MediaCatalog.Common;

namespace MediaCatalog.Domain.Exceptions
{
    public class DomainException : MediaCatalogException
    {
        public DomainException(string message) : base(message, 400) { }
    }
}
