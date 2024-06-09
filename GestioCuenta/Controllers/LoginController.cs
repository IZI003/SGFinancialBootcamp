using Comunes.Config;
using Comunes.Respuesta;
using Cuenta.Modelos;
using Cuenta.Servicios.Interfaces;
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
       
        [HttpGet()]
        public async Task<IActionResult> GetLogin(string usuario, string password)
        {
            var response = new RespuestaApi<UsuarioLogin>();

            var salida =await loginService.login(usuario, password);
            
            if (!salida.respuestaBD.Ok)
            {
               response.Resultado.AgregarError(GestionErrores.O_Men_2001, 401, mensaje: salida.respuestaBD.Mensaje, codigoInterno: GestionErrores.O_Cod_2001);

                return response.ObtenerResult();
            }

            response.Resultado.Datos = salida.usuario;
            return Ok(response);
        }

        // POST api/<LogginController>
        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult Post()
        {
            return Ok();
        }
    }
}
