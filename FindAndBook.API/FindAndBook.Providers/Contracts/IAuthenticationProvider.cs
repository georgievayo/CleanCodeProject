using System;

namespace FindAndBook.Providers.Contracts
{
    public interface IAuthenticationProvider
    {
        string GenerateToken(string username, string userRole);

        Guid CurrentUserID { get; }

        string CurrentUserRole { get; }
    }
}
