using Comunes.Respuesta;
using Cuenta.Modelos;

namespace Cuenta.Servicios.Interfaces;

public interface IOperacion
{
    Task<RespuestaBD> CrearOperacion(EntradaCrearOperacion entradaOperacion);
    List<TipoOperacion> OptenerTipoOperacion();
    Task<SalidaConcepto> OptenerConceptos(int tipoOperacion);
    Task<SalidaOperacionesCuenta> ObtenerOperacionesCuenta(int idCuenta);
}