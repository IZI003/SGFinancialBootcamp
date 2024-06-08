using System.Data.SqlClient;

namespace AccesoDatos.Excepciones;

internal class AccesoDatosException : Exception
{
    public AccesoDatosException(string mensaje) : base(mensaje) { }

    public AccesoDatosException(string mensaje, Exception innerException) : base(mensaje, innerException)
    {
        if (innerException is SqlException sqlEx)
        {
            EsSqkServer = true;
            SqlServerErrorNumber = sqlEx.Number;
        }
    }

    public int? SqlServerErrorNumber { get; }
    public bool EsSqkServer { get; }
}