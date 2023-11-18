using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApiProxy.Data.Dto;
using System.Net.Http.Headers;
using System.Text;
using WebApiProxy.Business.Utils;


namespace WebApiProxy.Business.Integration
{
    public class LogManagerApi
    {
        private readonly ILogger<LogManagerApi> _logger;
        private readonly string _applicationLogManagerID;
        private readonly string _urlLogManagerAPI;
        private readonly bool _enable;

        public LogManagerApi(IConfiguration configuration, ILogger<LogManagerApi> logger)
        {
            IConfiguration _configuration = configuration;
            _logger = logger;
            _applicationLogManagerID = _configuration["LogAPI:ApplicationLogManagerID"];
            _urlLogManagerAPI = _configuration["LogAPI:URLLogManagerAPI"];
            _enable = Convert.ToBoolean(_configuration["LogAPI:Enable"]);
        }

        public async Task CreateLogApplication(CreateLogApplicationIn input)
        {
            input.aud_IdRequest = 0;
            input.aud_idAplication = _applicationLogManagerID;
            input.aud_ip = Tools.GetHostIP();
            var jsonContent = JsonConvert.SerializeObject(input);

            if (_enable)
            {
                try
                {                   
                    using var client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    var content = new StringContent(jsonContent.ToString(), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(_urlLogManagerAPI, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("input: {jsonContent} ", jsonContent);
                        var errorMsj = response.Content.ToString();
                        _logger.LogError("Error API LogManager: {errorMsj}", errorMsj);
                    }

                }
                catch (Exception ex)
                {
                    var errorMsj = ex.Message;
                    _logger.LogError("input: {jsonContent}", jsonContent);
                    _logger.LogError("Error API LogManager: {errorMsj}", errorMsj);
                }

            }
            else {

                if (input.aud_status == "Error")
                {
                    _logger.LogError("input: {jsonContent}", jsonContent);
                }
                else if (input.aud_status == "Succes")
                {
                    _logger.LogInformation("input: {jsonContent}", jsonContent);
                }
            }
            
        }


    }
}
