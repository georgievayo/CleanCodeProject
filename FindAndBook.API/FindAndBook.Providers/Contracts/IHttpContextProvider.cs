using System.Web;

namespace FindAndBook.Providers.Contracts
{
    public interface IHttpContextProvider
    {
        HttpContext CurrentHttpContext { get; }
    }
}
