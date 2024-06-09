using Comunes.Respuesta;
using Login.Modelos;

namespace Login.Servicios;

public interface ILogin
{
    Task<SalidaLogin> login(string usuario, string password);
}