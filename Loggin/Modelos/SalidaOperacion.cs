using Comunes.Respuesta;

namespace Operaciones.Modelos;

public class validadorCuenta
{
    public bool? tipo_operacion { get; set; }
    public decimal saldo { get; set; }
    public int? id_cuenta { get; set; }
}
public class SalidaOperacionesCuenta
{
    public IEnumerable<OperacionesCuenta> Operaciones { get; set; }
    internal RespuestaBD RespuestaBD { get; set; }
}

public class OperacionesCuenta
{
    public int id_operacion { get; set; }
    public decimal monto { get; set; }
    public int tipo_operacion { get; set; }
    public string n_concepto { get; set; }
    public DateTime fecha { get; set; }
    public int id_cuenta { get; set; }
}