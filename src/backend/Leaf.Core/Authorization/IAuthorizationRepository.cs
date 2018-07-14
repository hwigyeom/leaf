namespace Leaf.Authorization
{
    public interface IAuthorizationRepository
    {
        AuthenticationAccountModel GetAuthenticationAccount(string userId);
        bool UpdatePassword(string userId, string hashedPassword);
    }
}