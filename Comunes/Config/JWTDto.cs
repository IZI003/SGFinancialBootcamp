namespace Comunes.Config;

public class JWTDto
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Subject { get; set; }
    public int expiracion { get; set; }
}