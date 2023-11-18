using Microsoft.AspNetCore.Mvc;
using WebApiProxy.Business.Interfaces;

namespace WebApiProxy.Controllers
{
    /// <summary>
    /// Controlador pronósticos del clima  
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProxyController : ControllerBase
    {
        private readonly IIntegrationService _service;

        /// <summary>
        /// Constructor del controlador 
        /// </summary>
        public ProxyController(IIntegrationService serviceIntegration)
        {
            _service = serviceIntegration;
        }

        /// <summary>
        /// Método proxy para acciones GET 
        /// </summary>
        /// <param name="endPoint">Valor URI del recurso a ejecutar</param>
        /// <response code="200">Objeto dinamico que se resuelve en tiempo de ejecuciónr</response>
        /// <response code="500">Oops! Error</response>       
        [Produces("application/json")]
        [HttpGet, Route("{*endPoint}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 500)]
        public async Task<IActionResult> ExecuteGetAsync([FromRoute]string endPoint)
        {
            try
            {
                var output = await _service.ExecuteEndPoint(Request, endPoint, null);
                return Ok(output);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Método proxy para acciones POST 
        /// </summary>
        /// <param name="endPoint">Valor URI del recurso a ejecutar</param>
        /// <param name="requestBody">Petición json</param>
        /// <response code="200">Objeto dinamico que se resuelve en tiempo de ejecución</response>
        /// <response code="500">Oops! Error</response>
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost, Route("{*endPoint}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 500)]
        public async Task<IActionResult> ExecutePostAsync([FromRoute] string endPoint, [FromBody] dynamic requestBody)
        {
            try
            {
                var output = await _service.ExecuteEndPoint(Request, endPoint, requestBody);
                return Ok(output);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

    }
}