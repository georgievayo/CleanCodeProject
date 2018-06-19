namespace FindAndBook.Providers.Contracts
{
    public interface IAuthenticationProvider
    {
        string GenerateToken(string username);
    }
}
