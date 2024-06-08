using Microsoft.AspNetCore.Mvc.ModelBinding;
using Comunes.Config;

namespace Comunes.Respuesta;

public class ResultadoApi<E>
{
    public const string Estado_OK = "OK";
    public const string Estado_Error = "ERROR";

    public ResultadoApi()
    {
        Estado = Estado_OK;
    }

    public ResultadoApi(E datos) : this()
    {
        Datos = datos;
    }

    public E Datos { get; set; }
    public ApiError Error { get; set; }

    public string Estado { get; set; }

    public void AgregarError(string descripcion, int codigoHttp,
       string codigoInterno = null, string mensaje = null, string estado = Estado_Error)
    {
        Datos = default(E);
        Estado = estado;

        Error = new ApiError
        {
            CodigoHttp = codigoHttp,
            CodigoInterno = codigoInterno,
            Mensaje = mensaje,
            Descripcion = descripcion
        };
    }

    public void AgregarError(RespuestaBD respuestaBD, int codigoHttp = 400, string mensaje = null, string estado = Estado_Error, string codigoError = null)
    {
        if (!respuestaBD.Error)
            return;

        Datos = default(E);
        Estado = estado;

        Error = new ApiError
        {
            CodigoHttp = codigoHttp,
            Descripcion = respuestaBD.Mensaje,
            CodigoInterno = codigoError == null ? respuestaBD.CodigoError : codigoError,
            Mensaje = mensaje
        };
    }

    public void AgregarError(Exception ex, string descripcion = GestionErrores.Men_9999, int codigoHttp = 500)
    {
        Datos = default(E);
        Estado = Estado_Error;

        Error = new ApiErrorConException(ex, descripcion)
        {
            CodigoHttp = codigoHttp,
            Descripcion = descripcion,
            CodigoInterno = GestionErrores.Cod_9999
        };
    }

    public void AgregarError(ModelStateDictionary modelState)
    {
        var key = modelState.First().Key;
        string mensaje = key + " no válido";
        AgregarError(GestionErrores.Men_9999, 400,
            codigoInterno: GestionErrores.Cod_9999, mensaje: mensaje);
    }
}

public class ResultadoApi : ResultadoApi<object>
{
    public ResultadoApi() : base()
    { }

    public ResultadoApi(object datos) : base(datos)
    { }
}