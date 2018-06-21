using System;

namespace FindAndBook.Providers.Contracts
{
    public interface IAuthenticationProvider
    {
        string GenerateToken(string username);

        Guid CurrentUserID { get; }
    }
}
