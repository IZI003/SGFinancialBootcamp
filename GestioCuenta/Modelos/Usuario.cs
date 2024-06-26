﻿namespace Cuenta.Modelos;

public class Usuario
{
    public int id_usuario { get; set; }
    public string nombre { get; set; }
    public string usuario { get; set; }
    public string password { get; set; }
}

public class UsuarioLogin
{
    public int id_usuario { get; set; }
    public string nombre { get; set; }
    public string token { get; set; }
}

public class EntradaLogin
{
    public string usuario { get; set; }
    public string password { get; set; }
}
public class EntradaUsuario
{
    public string nombre { get; set; }
    public string usuario { get; set; }
    public string password { get; set; }
}