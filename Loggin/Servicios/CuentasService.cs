using AccesoDatos;
using Comunes.Respuesta;
using Dapper;
using Login.Modelos;

namespace Login.Servicios;

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
            salida.saldo = await con.QueryFirstAsync<SaldoDto>(query, new { idCuenta });
        }
        catch (Exception ex)
        {
            salida.RespuestaBD = new RespuestaBD($"ERROR|Error al consultar los datos. {ex.Message}");
        }

        salida.RespuestaBD = new RespuestaBD("OK");
        return salida;
    }

    public async Task<SalidaSaldoTotal> ObtenerSaldoIdUsuario(int idUsuario)
    {
        var salida = new SalidaSaldoTotal();
        try
        {
            var query = @"SELECT SUM(saldo) as saldo
                          FROM cuenta WHERE id_usuario = @idUsuario";

            using var con = bd.ObtenerConexion();
            salida.saldo = await con.QueryFirstAsync<decimal>(query, new { idUsuario });
            salida.RespuestaBD = new RespuestaBD("OK");
        }
        catch (Exception ex)
        {
            salida.RespuestaBD = new RespuestaBD($"ERROR|Error al consultar los datos. {ex.Message}");
        }

        return salida;
    }

    public async Task<RespuestaBD> CrearCuenta(int idUsuario)
    {
        var salida = new RespuestaBD();
        try
        {
            using var con = bd.ObtenerConexion();
            await con.QueryAsync($"Insert into cuenta (saldo,id_usuario) values (0,{idUsuario})");
            salida = new RespuestaBD("OK");
        }
        catch (Exception ex)
        {
            salida = new RespuestaBD($"ERROR|Error al Crear la cuenta. {ex.Message}");
        }

        return salida;
    }

    
}