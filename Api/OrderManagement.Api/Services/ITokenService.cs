namespace OrderManagement.Api.Services;

public interface ITokenService
{
    string GenerateToken(string email, string customerId);
}
