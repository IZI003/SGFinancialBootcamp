using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Dapper;

using Microsoft.IdentityModel.Tokens;

using AccesoDatos;
using Comunes.Respuesta;

using Cuenta.Modelos;
using Cuenta.Servicios.Interfaces;

namespace Cuenta.Servicios;

public class LoginService : ILogin
{
    private BD bd;
    private IConfiguration config;
    public LoginService(BD bd, IConfiguration config)
    {
        this.bd = bd;
        this.config = config;
    }

    public async Task<SalidaLogin> login(EntradaLogin entradaLogin)
    {
        var salida = new SalidaLogin();
        try
        {
            var query = @"SELECT id_usuario, nombre, usuario, password
                                    FROM usuario 
                                    WHERE usuario = @usuario AND password = @password";

            using var con = bd.ObtenerConexion();
            var user = await con.QueryFirstOrDefaultAsync<Usuario>(query, new { entradaLogin.usuario, entradaLogin.password });

            if (user == null)
            {
                salida.respuestaBD = new RespuestaBD($"ERROR|Error Las Credenciales ingresadas no son correctas");

                return salida;
            }

            var jwt = config.GetSection("Jwt").Get<JWTDto>();

            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.nombre));
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, jwt.Subject.ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()));
            claims.AddClaim(new Claim("id_usuario", user.id_usuario.ToString()));

            var signin = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)), SecurityAlgorithms.HmacSha256);

            var tomkemDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(jwt.expiracion),
                SigningCredentials = signin
            };
            var tokenConfig = new JwtSecurityTokenHandler().CreateToken(tomkemDescriptor);

            salida.usuario = new UsuarioLogin
            {
                id_usuario = user.id_usuario,
                nombre = user.nombre,
                token = new JwtSecurityTokenHandler().WriteToken(tokenConfig)
            };

            salida.respuestaBD = new RespuestaBD("OK");
        }
        catch (Exception ex)
        {
            salida.respuestaBD = new RespuestaBD($"ERROR|Error al realizar Login. {ex.Message}");
        }

        return salida;
    }

    public async Task<RespuestaBD> CrearUsuario(EntradaUsuario entradaUsuario)
    {
        try
        {
            using var con = bd.ObtenerConexion();
            var query = "INSERT INTO usuario ( nombre,usuario,password) VALUES (@nombre,@usuario, @password)";
            await con.QueryAsync(query,  new { entradaUsuario.nombre, entradaUsuario.usuario, entradaUsuario.password });

            return new RespuestaBD("OK");
        }
        catch (Exception ex)
        {
            return new RespuestaBD($"ERROR|Error al Crear al Usuario. {ex.Message}");
        }
    }
}