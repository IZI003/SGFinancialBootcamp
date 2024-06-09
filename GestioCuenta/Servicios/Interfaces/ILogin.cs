using Comunes.Respuesta;
using Cuenta.Modelos;

namespace Cuenta.Servicios.Interfaces;

public interface ILogin
{
    Task<SalidaLogin> login(EntradaLogin entradaLogin);
    Task<RespuestaBD> CrearUsuario(EntradaUsuario entradaUsuario);
}