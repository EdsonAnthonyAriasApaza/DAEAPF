namespace DAEAPF.Infrastructure.JWT;

public interface IJwtService
{
    string GenerateToken(string userId, string email, string role);

}