using AccesoDatos.DBConfig;
using System.Data.SqlClient;

namespace AccesoDatos.Conexiones;

public class FabricaConexionDesdeConfig : IFabricaConexion
{
    private readonly BDConfig _bdConfig;

    public FabricaConexionDesdeConfig(BDConfig bdConfig)
    {
        _bdConfig = bdConfig;
    }

    public int DefaultCommandTimeout => _bdConfig.CommandTimeout;

    public SqlConnection CrearConexion(bool abierta = false)
    {
        string stringConexion = _bdConfig.CrearStringConexion();
        var conexion = new SqlConnection(stringConexion);

        if (abierta) conexion.Open();

        return conexion;
    }

    public void CerrarConexion(SqlConnection conexion)
    {
        conexion.Close();
    }
}
