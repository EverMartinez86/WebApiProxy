
using Microsoft.AspNetCore.Http;

namespace WebApiProxy.Business.Interfaces
{
    public interface IIntegrationService
    {
        Task<dynamic> ExecuteEndPoint(HttpRequest input, string endPoint, dynamic requestBody);


    }
}
