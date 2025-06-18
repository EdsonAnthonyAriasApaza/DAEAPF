namespace DAEAPF.Infrastructure.Encryptor;
using BCrypt = BCrypt.Net.BCrypt;
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.HashPassword(password);
    public bool Verify(string password, string hash) => BCrypt.Verify(password, hash);
}
