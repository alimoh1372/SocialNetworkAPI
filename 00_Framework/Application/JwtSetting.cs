namespace _00_Framework.Application;

public class Jwt
{
    public string Issuer { get; set; }

    public string SigningKey { get; set; }

    public string EncryptKey { get; set; }
    public int ExpireTimeInMinute { get; set; }
}
