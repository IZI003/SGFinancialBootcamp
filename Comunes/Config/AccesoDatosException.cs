using Comunes.Respuesta;

namespace Comunes.Config
{
    public class AccesoDatosException : Exception
    {
        public ApiError Error { get; private set; }

        public AccesoDatosException(string descripcion,
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
    }
}