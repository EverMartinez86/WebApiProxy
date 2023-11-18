
using Newtonsoft.Json;
using WebApiProxy.Data.Common;
using System.Net.Http.Headers;
using System.Text;

namespace WebApiProxy.Business.Integration
{
    /// <summary>
    /// Clase abstracta para la integración de servicios Api 
    /// </summary>
    public abstract class IntegrationApi
    {
        /// <summary>
        /// Método para ejecutar la integracióm con una URI especifica 
        /// </summary>
        /// <param name="input">Objeto cuerpo de la solicitud</param>
        /// <param name="endpoint">URI de la solicitud</param>
        /// <param name="token">Token Bearer</param>
        /// <param name="verb">Verbo HttpRequest</param>
        /// <returns>HttpResponseMessage, objeto que se obtiene en la integración</returns>
        public static async Task<HttpResponseMessage> Execute(object input, string endpoint, string token, HttpRequestType verb, int timeSpan)
        {
            HttpResponseMessage output = new ();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (timeSpan > 1)
            {
                client.Timeout = TimeSpan.FromMinutes(timeSpan);
            }

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }

            dynamic jsonBody = System.Text.Json.JsonSerializer.Serialize(input);
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            switch (verb)
            {
                case HttpRequestType.POST:
                    output = await client.PostAsync(endpoint, content);
                    break;
                case HttpRequestType.GET:
                    output = await client.GetAsync(endpoint);
                    break;
            }


            return output;
        }


    }
}
