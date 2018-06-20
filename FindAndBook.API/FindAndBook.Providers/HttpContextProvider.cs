using FindAndBook.Providers.Contracts;
using System.Web;

namespace FindAndBook.Providers
{
    public class HttpContextProvider : IHttpContextProvider
    {
        public HttpContext CurrentHttpContext => HttpContext.Current;
    }
}
