using AccesoDatos;
using Comunes.Parametros;
using Comunes.Respuesta;
using Dapper;
using Cuenta.Modelos;
using Cuenta.Servicios.Interfaces;

namespace Cuenta.Servicios;

public class OperacionesService: IOperacion
{
    private BD bd;
    public OperacionesService(BD bd)
    {
        this.bd = bd;
    }

    public async Task<RespuestaBD> CrearOperacion(EntradaCrearOperacion entradaOperacion)
    {
        var salida = new RespuestaBD();
        try
        {
            //controlamos el tipo de operacion y la cuenta existan
            var queryValidador = @"SELECT cta.saldo,concep.tipo_operacion, cta.id_cuenta
                                    FROM cuenta cta 
                                   INNER JOIN concepto concep ON (concep.id_concepto = @idConcepto)
                                    WHERE cta.id_cuenta = @idCuenta";
            using var con = bd.ObtenerConexion();
            con.Open();

            var validador = await con.QueryFirstOrDefaultAsync<validadorCuenta>(queryValidador, new { entradaOperacion.idCuenta, entradaOperacion.idConcepto });

            if (validador == null || entradaOperacion.monto <= 0)
            {
                salida = new RespuestaBD($"ERROR|Error al Crear la Operacion, parametros incorrectos");

                return salida;
            }

            //sumamos o restamos dependiendo del tipo de operacion
            var saldo = validador?.tipo_operacion == 1 ? (validador?.saldo + entradaOperacion.monto) : (validador?.saldo - entradaOperacion.monto);

            if (saldo < 0)
            {
                salida = new RespuestaBD($"ERROR|Error al Crear la Operación, Saldo Insuficiente");

                return salida;
            }

            using var transaction = con.BeginTransaction();

            try
            {
                var queryInsertOperacion = @"INSERT INTO operacion (id_concepto,monto,saldo_actual,id_cuenta,fecha ) 
                                        VALUES (@idConcepto,@monto,@saldo,@idCuenta,GETDATE())";

                await con.ExecuteAsync(queryInsertOperacion, new
                {
                    entradaOperacion.idConcepto,
                    entradaOperacion.monto,
                    saldo,
                    entradaOperacion.idCuenta
                }, transaction);

                var queryUpdateCuenta = @"UPDATE cuenta
                                        SET saldo = @saldo 
                                        WHERE id_cuenta = @idCuenta;";

                await con.ExecuteAsync(queryUpdateCuenta, new { saldo, entradaOperacion.idCuenta }, transaction);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return new RespuestaBD("Error|Existen problemas al Crear Operacion");
            }

            salida = new RespuestaBD("OK");
        }
        catch (Exception ex)
        {
            salida = new RespuestaBD($"ERROR|Error al Crear la Operación. {ex.Message}");
        }

        return salida;
    }

    public List<TipoOperacion> OptenerTipoOperacion()
    {
        return  new List<TipoOperacion>
        {
            new TipoOperacion { valor = (int)TipoOperacionEnum.CREDITO, descripcion = TipoOperacionEnum.CREDITO.ToString() },
            new TipoOperacion { valor = (int)TipoOperacionEnum.DEBITO, descripcion = TipoOperacionEnum.DEBITO.ToString() }
        };
    }

    public async Task<SalidaConcepto> OptenerConceptos(int tipoOperacion)
    {
        var salida = new SalidaConcepto();
        try
        {
            var queryConceptos = @"SELECT id_concepto,tipo_operacion , descripcion
                                    FROM  concepto 
                                    WHERE tipo_operacion = @tipoOperacion";

            using var con = bd.ObtenerConexion();
            salida.listaConceptos = (await con.QueryAsync<Concepto>(queryConceptos, new { tipoOperacion })).ToList();
            salida.respuestaBD = new RespuestaBD("OK");
        }
        catch (Exception ex)
        {
            salida.respuestaBD = new RespuestaBD($"ERROR|Error al Obtener Conceptos. {ex.Message}");
        }

        return salida;
    }

    public async Task<SalidaOperacionesCuenta> ObtenerOperacionesCuenta(int idCuenta)
    {
        var salida = new SalidaOperacionesCuenta();
        try
        {
            var query = @"SELECT op.id_operacion,op.monto,concep.descripcion  as n_concepto, op.id_cuenta as cuenta ,op.fecha
                            FROM operacion op
                            INNER JOIN concepto concep on (concep.id_concepto = op.id_concepto)
                            WHERE op.id_cuenta = @idCuenta";

            using var con = bd.ObtenerConexion();
            salida.Operaciones = await con.QueryAsync<OperacionesCuenta>(query, new { idCuenta });
            salida.RespuestaBD = new RespuestaBD("OK");
        }
        catch (Exception ex)
        {
            salida.RespuestaBD = new RespuestaBD($"ERROR|Error al consultar los datos. {ex.Message}");
        }

        return salida;
    }
}