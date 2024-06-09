using Comunes.Config;
using Comunes.Respuesta;
using Cuenta.Modelos;
using Cuenta.Servicios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cuenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogin loginService;

        public LoginController(ILogin loginService)
        {
            this.loginService = loginService;
        }

        [HttpPost("oauth")]
        public async Task<IActionResult> GetLogin([FromBody] EntradaLogin entradaLogin)
        {
            var response = new RespuestaApi<UsuarioLogin>();

            var salida = await loginService.login(entradaLogin);

            if (!salida.respuestaBD.Ok)
            {
                response.Resultado.AgregarError(GestionErrores.L_Men_3000, 400, mensaje: salida.respuestaBD.Mensaje, codigoInterno: GestionErrores.L_Cod_3000);

                return response.ObtenerResult();
            }

            response.Resultado.Datos = salida.usuario;
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] EntradaUsuario entradaUsuario)
        {
            var response = new RespuestaApi<RespuestaBD>();

            var salida = await loginService.CrearUsuario(entradaUsuario);

            if (!salida.Ok)
            {
                response.Resultado.AgregarError(GestionErrores.L_Men_3001, 400, mensaje: salida.Mensaje, codigoInterno: GestionErrores.L_Cod_3001);

                return response.ObtenerResult();
            }

            response.Resultado.Datos = salida;

            return Ok(response);
        }
    }
}
