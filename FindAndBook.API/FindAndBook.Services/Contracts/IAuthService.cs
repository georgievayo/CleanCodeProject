namespace FindAndBook.Services.Contracts
{
    public interface IAuthService
    {
        void SaveUserToken(string userId, string tokenValue);

        void DeleteExpiredTokens();

        void RenewToken(string tokenValue);

        void DeleteToken(string tokenValue);
    }
}
