using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebApiProxy.Business.Integration;
using WebApiProxy.Business.Interfaces;
using WebApiProxy.Data.Common;
using WebApiProxy.Data.Dto;
using System.Diagnostics;
using System.Dynamic;
using System.Web;

namespace WebApiProxy.Business.Services
{
    public class IntegrationService : IntegrationApi, IIntegrationService
    {
        private readonly Stopwatch _time;
        private readonly LogManagerApi _logmanager;
        private readonly string _urlBase;
        private readonly int _timeSpan;
        

        public IntegrationService(IConfiguration configuration, ILogger<LogManagerApi> logger)
        {
            _time = new Stopwatch();
            IConfiguration _configuration = configuration;
            ILogger<LogManagerApi> _logger = logger;
            _logmanager = new LogManagerApi(_configuration, _logger);
            _urlBase = _configuration["Backend:WebApiUrlBase"];
            _timeSpan = int.Parse(configuration["Backend:timeSpan"]);
        }

        public async Task<ResponseOut> ExecuteEndPoint(HttpRequest input, string endPoint, dynamic requestBody)
        {
            ResponseOut output = new ResponseOut();
            string requestUrl, token = string.Empty, newPath = string.Empty;
            var verb = HttpRequestType.GET;
            object responseContent;

            try
            {
                _time.Start();
                var uriBuilder = new UriBuilder(input.GetDisplayUrl());
                var varQuery = HttpUtility.ParseQueryString(uriBuilder.Query);
                
                if (string.IsNullOrEmpty(varQuery.ToString()))
                {
                    requestUrl = _urlBase + endPoint.Replace("%2F", "/");
                }
                else 
                {
                    if (endPoint.Contains('?'))
                    {
                        endPoint = endPoint.Replace("%2F", "/");
                    }
                    else
                    {
                        endPoint = string.Concat(endPoint.Replace("%2F", "/"), "?", varQuery.ToString());
                    }

                    requestUrl = _urlBase + endPoint;
                }

                if (!string.IsNullOrWhiteSpace(input.Headers["Authorization"]))
                {
                    token = input.Headers["Authorization"].ToString();
                }

                string requestMethod = input.Method.ToString();


                switch (requestMethod)
                {
                    case "GET":
                        {
                            verb = HttpRequestType.GET;
                            break;
                        }
                    case "POST":
                        {
                            verb = HttpRequestType.POST;
                            break;
                        }
                    case "DELETE":
                        {
                            verb = HttpRequestType.DELETE;
                            break;
                        }
                }

                var responde = await Execute(requestBody, requestUrl, token, verb, _timeSpan);
                var responseBody = responde.Content.ReadAsStringAsync().Result;
                var expandoObjectConverter = new ExpandoObjectConverter();

                output.statusCode = (int)responde.StatusCode;
                if (responseBody.StartsWith("["))
                {
                    if (responseBody.StartsWith("[\""))
                    {
                        responseContent = JsonConvert.DeserializeObject<List<string>>(responseBody, expandoObjectConverter);
                    }
                    else
                    {
                        responseContent = JsonConvert.DeserializeObject<List<ExpandoObject>>(responseBody, expandoObjectConverter);
                    }

                }
                else
                {
                    responseContent = JsonConvert.DeserializeObject<ExpandoObject>(responseBody, expandoObjectConverter);
                }

                switch (responde.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        output.content = responseContent;
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        output.content = JsonConvert.SerializeObject(new { message = "Not Found - Url/Method", url = requestUrl });
                        break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        output.content = responseContent;
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        output.content = responseContent;
                        break;
                    default:
                        output.content = JsonConvert.SerializeObject(new { message = "Error " + responde.ReasonPhrase, url = requestUrl });
                        break;
                }

                _time.Stop();
                
                #region auditoria
                await _logmanager.CreateLogApplication(new CreateLogApplicationIn()
                {
                    aud_class = "IntegrationService",
                    aud_method = "ExecuteEndPoint",
                    aud_status = "Succes",
                    aud_data_before = JsonConvert.SerializeObject(new { verbo = verb, metodo = newPath, json = Convert.ToString(requestBody) }),
                    aud_data_after = JsonConvert.SerializeObject(output),
                    aud_message = " totalTimeOnRequest: " + _time.Elapsed.TotalSeconds.ToString()
                });
                #endregion

            }
            catch (Exception ex)
            {
                #region auditoria
                await _logmanager.CreateLogApplication(new CreateLogApplicationIn()
                {
                    aud_class = "IntegrationService",
                    aud_method = "ExecuteEndPoint",
                    aud_status = "Error",
                    aud_data_before = JsonConvert.SerializeObject(new { verbo = verb, metodo = newPath, json = Convert.ToString(requestBody) }),
                    aud_data_after = string.Empty,
                    aud_message = ex.Message.Replace("'", ""),
                    aud_exception = ex.ToString().Replace("'", ""),
                });
                #endregion
                throw;
            }

            return output;
        }
    }
}
