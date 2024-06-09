using Cuenta.Modelos;

namespace Cuenta.Servicios.Interfaces;

public interface ILogin
{
    Task<SalidaLogin> login(string usuario, string password);
}