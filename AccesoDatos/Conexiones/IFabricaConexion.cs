
using System.Data.SqlClient;

namespace AccesoDatos.Conexiones
{
    public interface IFabricaConexion
    {
        SqlConnection CrearConexion(bool abierta = false);

        int DefaultCommandTimeout { get; }
    }
}
