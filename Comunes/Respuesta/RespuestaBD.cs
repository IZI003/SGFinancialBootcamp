using Comunes.Config;

namespace Comunes.Respuesta
{
    public class RespuestaBD
    {
        public const string OK = "OK";
        public const string BD_NORES = "BD_NORES";

        public RespuestaBD() { Mensaje = OK; }

        public RespuestaBD(string resultadoBD)
        {
            if (resultadoBD == OK)
            {
                Mensaje = resultadoBD;
            }
            else
            {
                AgregarErrorBaseDatos(resultadoBD);
            }
        }
        
        public string Mensaje { get; set; }
        public string CodigoError { get; set; }
        public bool Error => !Ok || !string.IsNullOrEmpty(CodigoError);
        public bool Ok => Mensaje == OK;

        public override string ToString()
        {
            if (Error)
            {
                return $"{Mensaje} (Código error: {CodigoError})";
            }

            return Mensaje;
        }

        /// <summary>
        /// Setea un error proveniente de la base de datos de la forma por ejemplo: LOG_USU|El usuario o la contraseña son invalidos.
        /// </summary>
        /// <param name="error">Error de la base de datos en la forma CODIGO|MENSAJE</param>
        public void AgregarErrorBaseDatos(string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                CodigoError = BD_NORES;
                Mensaje = "Resultado de base de base de datos desconocido";
                return;
            }

            try
            {
                var split = error.Split('|');
                CodigoError = split[0];
                Mensaje = split[1];
            }
            catch(IndexOutOfRangeException ex)
            {
                throw new GestioCuentaExcepcion(ex, 
                    descripcion: $"Código de error de base de datos en formato incorrecto ({error})");
            }
        }

        public static RespuestaBD RespuestaOk()
        {
            return new RespuestaBD(OK);
        }
    }
}