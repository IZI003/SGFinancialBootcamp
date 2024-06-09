using Microsoft.AspNetCore.Mvc;

using Comunes.Config;
using Comunes.Respuesta;

using Cuenta.Modelos;
using Cuenta.Servicios.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Cuenta.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OperacionController : Controller
    {
        private readonly IOperacion operacionService;

        public OperacionController(IOperacion operacionService)
        {
            this.operacionService = operacionService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearOperacion([FromBody] EntradaCrearOperacion entradaOperacion)
        {
            var response = new RespuestaApi<RespuestaBD>();
            var salida = await operacionService.CrearOperacion(entradaOperacion);

            if (!salida.Ok)
            {
                response.Resultado.AgregarError(GestionErrores.O_Men_2000, 400, mensaje: salida.Mensaje, codigoInterno: GestionErrores.O_Cod_2000);

                return response.ObtenerResult();
            }

            response.Resultado.Datos = salida;

            return Ok(response);
        }

        [HttpGet("tipo-operacion")]
        public IActionResult ObtenerTipoOperacion()
        {
            var response = new RespuestaApi<List<TipoOperacion>>();
            response.Resultado.Datos = operacionService.OptenerTipoOperacion();

            return Ok(response);
        }

        [HttpGet("conceptos/{tipo_operacion}")]
        public async Task<IActionResult> ObtenerConcepto(int tipo_operacion)
        {
            var response = new RespuestaApi<IEnumerable<Concepto>>();
            var salida = await operacionService.OptenerConceptos(tipo_operacion);

            if (!salida.respuestaBD.Ok)
            {
                response.Resultado.AgregarError(GestionErrores.O_Men_2001 , 400, mensaje: salida.respuestaBD.Mensaje, codigoInterno: GestionErrores.O_Cod_2001);

                return response.ObtenerResult();
            }

            response.Resultado.Datos = salida.listaConceptos;

            return Ok(response);
        }

        [HttpGet("operaciones/{idCuenta}")] 
        public async Task<IActionResult> ObtenerOperacionesCuenta(int idCuenta)
        {
            var response = new RespuestaApi<IEnumerable<OperacionesCuenta>>();
            var salida = await operacionService.ObtenerOperacionesCuenta(idCuenta);

            if (!salida.RespuestaBD.Ok)
            {
                response.Resultado.AgregarError(GestionErrores.C_Men_1000, 400, mensaje: salida.RespuestaBD.Mensaje, codigoInterno: GestionErrores.C_Cod_1000);

                return response.ObtenerResult();
            }

            response.Resultado.Datos = salida.Operaciones;

            return Ok(response);
        }
    }
}