using Microsoft.AspNetCore.Mvc;

namespace Comunes.Respuesta
{
    public class RespuestaApi<E>
    {
        public RespuestaApi()
        {
            Resultado = new ResultadoApi<E>();
        }

        public RespuestaApi(E resultado)
        {
            Resultado = new ResultadoApi<E>(resultado);
        }

        public ResultadoApi<E> Resultado { get; set; }

        public ObjectResult ObtenerResult()
        {
            if (Resultado.Error == null)
            {
                return new OkObjectResult(this);
            }

            var result = new ObjectResult(this)
            {
                StatusCode = Resultado.Error.CodigoHttp
            };

            return result;
        }
    }

    public class RespuestaApi : RespuestaApi<object>
    {
        public RespuestaApi() : base() { }

        public RespuestaApi(object resultado) : base(resultado) { }
    }
}