using Comunes.Respuesta;

namespace Cuenta.Modelos
{
    public class SaldoDto
    {
        public decimal Saldo { get; set; }
        public int Cuenta { get; set; }
    }

    public class SalidaSaldo
    {
        public SaldoDto? saldo { get; set; }
        internal RespuestaBD RespuestaBD { get; set; }
    }

    public class SalidaSaldoTotal
    {
        public decimal saldo { get; set; }
        internal RespuestaBD RespuestaBD { get; set; }
    }
}