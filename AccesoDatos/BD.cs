using System.Data.SqlClient;

using Dapper;

using AccesoDatos.Conexiones;
using AccesoDatos.Excepciones;


namespace AccesoDatos
{
    public class BD
    {
        private readonly IFabricaConexion fabricaConexion;
        public BD(IFabricaConexion fabricaConexion)
        {
            this.fabricaConexion = fabricaConexion;
        }

        public BDConectada Conectar()
        {
            return new BDConectada(fabricaConexion);
        }

        public virtual async Task<IEnumerable<T>> Consultar<T>(string query)
        {
            IEnumerable<T> datos;
            try
            {
                using (var conexion = fabricaConexion.CrearConexion(abierta: true))
                {
                    datos = await conexion.QueryAsync<T>(query,
                        commandTimeout: 200
                    );
                }

                return datos;
            }
            catch (SqlException ex)
            {
                throw new AccesoDatosException($"Error al ejecutar query", ex);
            }
        }

        public System.Data.IDbConnection ObtenerConexion()
        {
            return fabricaConexion.CrearConexion();
        }
    }
}