using Comunes.Respuesta;
using Operaciones.Modelos;

namespace Operaciones.Servicios;

public interface IOperacion
{
    Task<RespuestaBD> CrearOperacion(EntradaCrearOperacion entradaOperacion);
    List<TipoOperacion> OptenerTipoOperacion();
    Task<SalidaConcepto> OptenerConceptos(int tipoOperacion);
    Task<SalidaOperacionesCuenta> ObtenerOperacionesCuenta(int idCuenta);
}