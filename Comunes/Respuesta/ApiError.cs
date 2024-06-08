
namespace Comunes.Respuesta
{
    public class ApiError
    {
        /// <summary>
        /// Código Http
        /// </summary>
        [Newtonsoft.Json.JsonProperty("codigo")]
        public int CodigoHttp { get; set; }
        /// <summary>
        /// Código interno de la aplicación
        /// </summary>
        public string CodigoInterno { get; set; }
        /// <summary>
        /// Mensaje de error amigable
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Detalle del error
        /// </summary>
        public string Mensaje { get; set; }
    }

    public class ApiErrorConException : ApiError
    {
        public ApiErrorConException(Exception exception, string descripcion) : base()
        {
            Exception = exception;
            Descripcion = descripcion;
            Mensaje = exception.Message;
        }

        [Newtonsoft.Json.JsonIgnore]
        public Exception Exception { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string StackTrace => Exception.StackTrace;
    }
}
