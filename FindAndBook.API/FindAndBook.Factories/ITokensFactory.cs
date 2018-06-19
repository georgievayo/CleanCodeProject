using FindAndBook.Models;
using System;

namespace FindAndBook.Factories
{
    public interface ITokensFactory
    {
        Token Create(string userId, string value, DateTime expirationTime);
    }
}
