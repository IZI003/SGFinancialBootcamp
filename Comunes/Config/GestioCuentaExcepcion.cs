using Comunes.Respuesta;

namespace Comunes.Config
{
    public class GestioCuentaExcepcion : Exception
    {
        private const string MensajeDefault = "Excepción no controlada";

        public GestioCuentaExcepcion(string descripcion,
            int codigoHttp = 500,
            string codigoInterno = null,
            string mensaje = null)
            : base(descripcion)
        {
            Error = new ApiError
            {
                CodigoHttp = codigoHttp,
                CodigoInterno = codigoInterno,
                Descripcion = descripcion,
                Mensaje = mensaje
            };
        }

        public GestioCuentaExcepcion(Exception exception,
            string descripcion = MensajeDefault,
            int codigoHttp = 500,
            string codigoInterno = null,
            string mensaje = null)
            : base(descripcion, exception)
        {
            Error = new ApiErrorConException(exception, descripcion)
            {
                CodigoHttp = codigoHttp,
                CodigoInterno = codigoInterno,
                Descripcion = descripcion
            };

            if (!string.IsNullOrEmpty(mensaje))
            {
                Error.Mensaje = mensaje;
            }
        }

        public ApiError Error { get; private set; }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"NotFound exception: \"{name}\" ({key}) was not found.")
        {
        }
        public NotFoundException(string message)
            : base($"" + message + "")
        {
        }
    }
}
