namespace AccesoDatos.DBConfig;

using System.Data.Common;

using Microsoft.Extensions.Configuration;

using AccesoDatos.Excepciones;

public class BDConfig
{
    public const int DefaultCommandTimeout = 30; // segundos
    public const int DefaultMaxPoolSize = 100;
    public const int DefaultMinPoolSize = 1;
    public const int DefaultIncrPoolSize = 5;

    private const string BDKey = "BD";
    private const string DataSourceKey = "Server";
    private const string DatabaseKey = "Database";
    private const string UsuarioKey = "Usuario";
    private const string PassKey = "Password";
    private const string MaxPoolSizeKey = "MaxPoolSize";
    private const string MinPoolSizeKey = "MinPoolSize";
    private const string IncrPoolSizeKey = "IncrPoolSize";
    private const string CommandTimeoutKey = "CommandTimeout";

    private readonly IConfiguration _config;

    public BDConfig(IConfiguration config)
    {
        _config = config;
        SetearConfiguracion();
    }

    public string DataSource { get; set; }
    public string Database { get; set; }
    public string Usuario { get; set; }
    public string Password { get; set; }
    public int MaxPoolSize { get; set; } = DefaultMaxPoolSize;
    public int MinPoolSize { get; set; } = DefaultMinPoolSize;
    public int IncrPoolSize { get; set; } = DefaultIncrPoolSize;
    public int CommandTimeout { get; set; } = DefaultCommandTimeout;

    private void SetearConfiguracion()
    {
        string bdKey = BDKey;

        if (int.TryParse(_config.GetSection($"{bdKey}:{MaxPoolSizeKey}").Value, out int max) && max > 0)
            MaxPoolSize = max;

        if (int.TryParse(_config.GetSection($"{bdKey}:{MinPoolSizeKey}").Value, out int min) && min > 0)
            MinPoolSize = min;

        if (int.TryParse(_config.GetSection($"{bdKey}:{IncrPoolSizeKey}").Value, out int incr) && incr > 0)
            IncrPoolSize = incr;

        if (int.TryParse(_config.GetSection($"{bdKey}:{CommandTimeoutKey}").Value, out int cmdTout) && cmdTout > 0)
            CommandTimeout = cmdTout;

        Usuario = _config.GetSection($"{bdKey}:{UsuarioKey}").Value;
        Password = _config.GetSection($"{bdKey}:{PassKey}").Value;
        DataSource = _config.GetSection($"{bdKey}:{DataSourceKey}").Value;
        Database = _config.GetSection($"{bdKey}:{DatabaseKey}").Value;

        if (string.IsNullOrWhiteSpace(Database))
            throw new AccesoDatosException("No se ha especificado Nombre de la Base de datos");

        if (string.IsNullOrWhiteSpace(Usuario))
            throw new AccesoDatosException("No se ha especificado usuario de base de datos");

        if (string.IsNullOrWhiteSpace(Password))
            throw new AccesoDatosException("No se ha especificado password de base de datos");

        if (string.IsNullOrWhiteSpace(DataSource))
            throw new AccesoDatosException("No se ha especificado el data source de base de datos");
    }

    public string CrearStringConexion()
    {
        var builder = new DbConnectionStringBuilder 
        {
            ConnectionString =$"{VariablesConectionEnum.server}={DataSource};{VariablesConectionEnum.Database}={Database};{ VariablesConectionEnum.user } ={Usuario} ;{VariablesConectionEnum.password}={Password};",
        };

        return builder.ConnectionString;
    }

}
public enum VariablesConectionEnum
{
    user=1,
    password = 2,
    server=3,
    Database=4
}