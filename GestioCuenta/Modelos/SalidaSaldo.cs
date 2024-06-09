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
        public SaldoTotal saldoTotal { get; set; }
        internal RespuestaBD RespuestaBD { get; set; }
    }

    public class SaldoTotal
    {
        public List<SaldoDto> cuentas { get; set; }
        public decimal saldoTotal { get; set; }
    }

    public class SaldoBD
    {
        public decimal Saldo { get; set; }
        public int Cuenta { get; set; }
        public decimal saldoTotal { get; set; }
    }
}