namespace AccesoDatos;

using AccesoDatos.Conexiones;
using AccesoDatos.Excepciones;
using System.Data.SqlClient;

public class BDConectada : BD, IDisposable
{

    private readonly SqlConnection _conexion;
    private SqlTransaction _transaccion = null;
    private bool _disposed;

    public BDConectada(IFabricaConexion fabricaConexion)
        : base(fabricaConexion)
    {
        try
        {
            _conexion = fabricaConexion.CrearConexion(abierta: true);
        }
        catch (SqlException ex)
        {
            throw new AccesoDatosException("Error al conectar a base de datos", ex);
        }
    }

    public void IniciarTransaccion()
    {
        _transaccion = _conexion.BeginTransaction();
    }

    public void Commit()
    {
        if (_transaccion != null)
        {
            _transaccion.Commit();
        }
    }

    public void Rollback()
    {
        if (_transaccion != null)
        {
            _transaccion.Rollback();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            if (_transaccion != null)
            {
                _transaccion.Dispose();
            }

            if (_conexion != null)
            {
                if (_conexion.State == System.Data.ConnectionState.Open)
                {
                    _conexion.Close();
                }

                _conexion.Dispose();
            }
        }

        _disposed = true;
    }

    ~BDConectada()
    {
        Dispose(false);
    }
}