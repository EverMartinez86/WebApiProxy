
using Microsoft.AspNetCore.Http;
using WebApiProxy.Data.Dto;

namespace WebApiProxy.Business.Interfaces
{
    public interface IIntegrationService
    {
        Task<ResponseOut> ExecuteEndPoint(HttpRequest input, string endPoint, dynamic requestBody);


    }
}
