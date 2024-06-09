using Comunes.Respuesta;
using Login.Modelos;

namespace Login.Servicios
{
    public interface ICuenta
    {
        Task<SalidaSaldo> ObtenerSaldoIdCuenta(int idCuenta);
        Task<SalidaSaldoTotal> ObtenerSaldoIdUsuario(int idUsuario);
        Task<RespuestaBD> CrearCuenta(int idUsuario);
    }
}