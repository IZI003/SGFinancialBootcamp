using Comunes.Config;
using Comunes.Respuesta;
using Cuenta.Modelos;
using Cuenta.Servicios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cuenta.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly ICuenta CuentaServices;

        public CuentaController(ICuenta CuentaServices)
        {
            this.CuentaServices = CuentaServices;
        }

        [HttpGet("saldo/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = new RespuestaApi<SaldoDto>();
            var salida = await CuentaServices.ObtenerSaldoIdCuenta(id);

            if (!salida.RespuestaBD.Ok)
            {
                response.Resultado.AgregarError(GestionErrores.C_Men_1000, 403, mensaje: salida.RespuestaBD.Mensaje, codigoInterno: GestionErrores.C_Cod_1000);

                return response.ObtenerResult();
            }
            response.Resultado.Datos = salida.saldo;
            return Ok(response);
        }

        [HttpGet("saldo-total/{idusuario}")]
        public async Task<IActionResult> GetSaldoUsuario(int idusuario)
        {
            var response = new RespuestaApi<SaldoTotal>();
            var salida = await CuentaServices.ObtenerSaldoIdUsuario(idusuario);

            if (!salida.RespuestaBD.Ok)
            {
                response.Resultado.AgregarError(GestionErrores.C_Men_1000, 400, mensaje: salida.RespuestaBD.Mensaje, codigoInterno: GestionErrores.C_Cod_1000);

                return response.ObtenerResult();
            }
            response.Resultado.Datos = salida.saldoTotal;
            return Ok(response);
        }

        [HttpPost("{idusuario}")]
        public async Task<IActionResult> CrearCuenta(int idusuario)
        {
            var response = new RespuestaApi<RespuestaBD>();

            var salida = await CuentaServices.CrearCuenta(idusuario);

            if (!salida.Ok)
            {
                response.Resultado.AgregarError(GestionErrores.C_Men_1001, 400, mensaje: salida.Mensaje, codigoInterno: GestionErrores.C_Cod_1001);

                return response.ObtenerResult();
            }

            response.Resultado.Datos = salida;

            return Ok(response);
        }
    }
}