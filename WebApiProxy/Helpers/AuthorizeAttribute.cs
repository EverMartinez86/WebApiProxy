using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiProxy.Helpers
{
    /// <summary>
    /// Atributo para autorizar la ejecución de una clase o un método
    /// Si el contexto contiene el Item["UserId"], se permite la ejecución
    /// de lo contrario se retorna un erro 401 Unauthorized
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var UserId = context.HttpContext.Items["UserId"];
            if (UserId == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
