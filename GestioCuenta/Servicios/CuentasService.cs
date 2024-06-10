using System.Data;

using Dapper;

using AccesoDatos;
using Comunes.Respuesta;

using Cuenta.Modelos;
using Cuenta.Servicios.Interfaces;

namespace Cuenta.Servicios;

public class CuentasService : ICuenta
{
    private BD bd;
    public CuentasService(BD bd)
    {
        this.bd = bd;
    }

    public async Task<SalidaSaldo> ObtenerSaldoIdCuenta(int idCuenta)
    {
        var salida = new SalidaSaldo();
        try
        {
            var query = @"SELECT saldo,id_cuenta as cuenta 
                        FROM cuenta WHERE id_cuenta = @idCuenta";

            using var con = bd.ObtenerConexion();
            var saldoDto= await con.QueryAsync<SaldoDto>(query, new { idCuenta });

            salida.saldo = saldoDto?.FirstOrDefault();

            salida.RespuestaBD = new RespuestaBD("OK");

            return salida;
        }
        catch (Exception ex)
        {
            salida.RespuestaBD = new RespuestaBD($"ERROR|Error al consultar los datos. {ex.Message}");

            return salida;
        }
    }

    public async Task<SalidaSaldoTotal> ObtenerSaldoIdUsuario(int idUsuario)
    {
        var salida = new SalidaSaldoTotal();
        try
        {
            using var con = bd.ObtenerConexion();

            if (!await validarUsuario(idUsuario, con))
            {
                salida.RespuestaBD = new RespuestaBD("ERROR|Error al consultar saldos. no se encontro al Usuario");

                return salida;
            }

            var query = @"SELECT id_cuenta as cuenta, saldo, id_usuario
                            FROM cuenta 
                            WHERE id_usuario = @idUsuario";

            var saldodb = await con.QueryAsync<SaldoBD>(query, new { idUsuario });

            salida.saldoTotal = new SaldoTotal
            {
                cuentas = saldodb.Select(s => new SaldoDto
                {
                    Cuenta = s.Cuenta,
                    Saldo = s.Saldo
                }).ToList(),
                saldoTotal = saldodb.Sum(s => s.Saldo)
            };
           
            salida.RespuestaBD = new RespuestaBD("OK");

            return salida;
        }
        catch (Exception ex)
        {
            salida.RespuestaBD = new RespuestaBD($"ERROR|Error al al consultar saldos. {ex.Message}");

            return salida;
        }
    }

    public async Task<RespuestaBD> CrearCuenta(int idUsuario)
    {
        try
        {
            using var con = bd.ObtenerConexion();

            if (!await validarUsuario(idUsuario, con))
            {
                return new RespuestaBD("ERROR|Error al Crear la cuenta. no se encontro al Usuario");
            }

            await con.QueryAsync($"Insert into cuenta (saldo,id_usuario) values (0,{idUsuario})");

            return new RespuestaBD("OK");
        }
        catch (Exception ex)
        {
            return new RespuestaBD($"ERROR|Error al Crear la cuenta. {ex.Message}");
        }
    }

    private async Task<bool> validarUsuario(int idUsuario, IDbConnection con)
    {
        var query = @"SELECT id_usuario
                          FROM usuario 
                          WHERE id_usuario = @idUsuario";

        var existe = await con.QueryAsync<decimal?>(query, new { idUsuario });

        return existe.Any();
    }
}