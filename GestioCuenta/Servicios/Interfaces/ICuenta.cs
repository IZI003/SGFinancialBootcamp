using Comunes.Respuesta;
using Cuenta.Modelos;

namespace Cuenta.Servicios.Interfaces
{
    public interface ICuenta
    {
        Task<SalidaSaldo> ObtenerSaldoIdCuenta(int idCuenta);
        Task<SalidaSaldoTotal> ObtenerSaldoIdUsuario(int idUsuario);
        Task<RespuestaBD> CrearCuenta(int idUsuario);
    }
}