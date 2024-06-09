using AccesoDatos;
using Comunes.Respuesta;
using Dapper;
using Cuenta.Modelos;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

            claims.AddClaim(new Claim (ClaimTypes.NameIdentifier, user.nombre));
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, jwt.Subject.ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()));
            claims.AddClaim(new Claim("id_usuario", user.id_usuario.ToString()));

            var signin = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)), SecurityAlgorithms.HmacSha256);

            var tomkemDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(jwt.expiracion),
                SigningCredentials= signin
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
            await con.QueryAsync($"Insert into usuario ( nombre,usuario,password) values ({entradaUsuario.nombre},{entradaUsuario.usuario}, {entradaUsuario.password})");

            return new RespuestaBD("OK");
        }
        catch (Exception ex)
        {
            return new RespuestaBD($"ERROR|Error al Crear al Usuario. {ex.Message}");
        }
    }

    //public async Task<SalidaLogin> validarToken(ClaimsIdentity claimsIdentity)
    //{
    //    var salida = new SalidaLogin();
    //    try
    //    {
    //        if (claimsIdentity.Claims.Any())
    //        {
    //            salida.respuestaBD = new RespuestaBD($"ERROR|Error al Crear la Operación.");

    //            return salida;
    //        }

    //        var idUser = claimsIdentity.Claims.FirstOrDefault(X=> X.Type == "id_usuario").Value;

    //        var query = @"SELECT id_usuario, nombre, usuario, password
    //                                FROM usuario 
    //                                WHERE id_usuario = @idUser";

    //        using var con = bd.ObtenerConexion();
    //        var user = await con.QueryFirstOrDefaultAsync<Usuario>(query, new { idUser });

    //        if (user == null)
    //        {
    //            salida.respuestaBD = new RespuestaBD($"ERROR|Error Las Credenciales ingresadas no son correctas");

    //            return salida;
    //        }

    //        var jwt = config.GetSection("Jwt").Get<JWTDto>();
    //        var claims = new[]
    //                        {
    //                            new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject.ToString()),
    //                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
    //                            new Claim("id_usuario", user.id_usuario.ToString()),
    //                            new Claim("nombre", user.nombre),
    //                        };

    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
    //        var Signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //        var token = new JwtSecurityToken(//issuer: jwt.Issuer,
    //                                         //audience: jwt.Audience,
    //                                         claims: claims,
    //                                         expires: DateTime.UtcNow.AddMinutes(jwt.expiracion),
    //                                         signingCredentials: Signin);

    //        salida.usuario = new UsuarioLogin
    //        {
    //            id_usuario = user.id_usuario,
    //            nombre = user.nombre,
    //            token = new JwtSecurityTokenHandler().WriteToken(token)
    //        };

    //        salida.respuestaBD = new RespuestaBD("OK");
    //    }
    //    catch (Exception ex)
    //    {
    //        salida.respuestaBD = new RespuestaBD($"ERROR|Error al Realizar el login. {ex.Message}");
    //    }

    //    return salida;
    //}
}