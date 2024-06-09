using Comunes.Respuesta;

namespace Operaciones.Modelos;

public class EntradaCrearOperacion
{
   public int idCuenta { get; set; }
    public int idTipoOperacion { get; set; }
    public int idConcepto { get; set; }
    public decimal monto { get; set; }
}

public class Concepto
{
    public int id_Concepto { get; set; }
    public int tipo_operacion { get; set; }
    public string descripcion { get; set; }
}

public class TipoOperacion
{
    public int valor { get; set; }
    public string descripcion { get; set; }
}

public class SalidaConcepto
{
    public List<Concepto> listaConceptos{ get; set; }
    public RespuestaBD respuestaBD { get; set; }
}