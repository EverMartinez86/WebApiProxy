
using System.Net;

namespace WebApiProxy.Business.Utils
{
    /// <summary>
    /// Clase para crear los métodos estatitcos que necesitan los servicios. integraciones
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Obtener la IP del host actual
        /// </summary>
        /// <returns>dirección ip</returns>
        public static string GetHostIP()
        {
            IPAddress[] ip = Dns.GetHostAddresses(Dns.GetHostName());
            return ip[1].ToString();
        }

    }
}
