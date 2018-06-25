using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace FindAndBook.API.App_Start
{
    public class GlobalExceptionFilter : System.Web.Http.Filters.FilterAttribute, System.Web.Http.Filters.IExceptionFilter
    {
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => HandleException(actionExecutedContext.Exception), cancellationToken);
        }

        public void HandleException(Exception exception)
        {
            using (StreamWriter writer = new StreamWriter(Constants.ERRORS_FILE))
            {
                writer.WriteLine(exception.Message);
            }

            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(exception.Message)
            };

            throw new HttpResponseException(response);
        }
    }
}