using System;
using System.IdentityModel.Tokens.Jwt;
using Leaf.Security;

namespace Leaf.Authorization
{
    public class AuthenticationService
    {
        private const int TemporaryTokenExpireMins = 5;

        public AuthenticationService(JwtTokenProvider tokenProvider, IAuthorizationRepository authRepo)
        {
            TokenProvider = tokenProvider;
            AuthRepo = authRepo;
        }

        private JwtTokenProvider TokenProvider { get; }
        private IAuthorizationRepository AuthRepo { get; }

        public AuthenticationResultModel Authenticate(AuthenticationRequestModel request)
        {
            var account = AuthRepo.GetAuthenticationAccount(request.UserId);

            var result = new AuthenticationResultModel();

            if (account == null)
            {
                result.Result = AuthenticationResultType.Invalid;
                return result;
            }

            if (StringHasher.ValidateHashedString(account.HashedPassword, request.Password))
            {
                if (account.Locked)
                {
                    result.Result = AuthenticationResultType.Locked;
                    return result;
                }

                result.Result = account.ChangePassword
                    ? AuthenticationResultType.ChangePassword
                    : AuthenticationResultType.Success;
            }
            else if (!account.Locked && account.HashedPassword.Equals(request.Password))
            {
                // 일반 문자열 비밀번호 확인
                result.Result = AuthenticationResultType.ChangePassword;
            }
            else
            {
                // 비밀번호가 맞지 않음
                result.Result = AuthenticationResultType.Invalid;
                return result;
            }

            var tokenModel = new AuthenticationTokenModel
            {
                UserId = account.UserId,
                Name = account.Name,
                Company = account.CompanyCode ?? "",
                Group = account.GroupCode ?? "",
                UserClass = account.UserClass ?? "",
                UniqueId = Guid.NewGuid().ToString("N")
            };

            var tokenClaims = tokenModel.ToClaims();

            var token =
                TokenProvider.GetJwtSecurityToken(tokenClaims,
                    result.Result == AuthenticationResultType.ChangePassword
                        ? TemporaryTokenExpireMins
                        : (int?) null);

            result.Token = new JwtSecurityTokenHandler().WriteToken(token);

            return result;
        }

        public bool ChangePassword(PasswordRequestModel request)
        {
            throw new NotImplementedException();
        }

        public bool ValidatePassword(PasswordRequestModel request)
        {
            throw new NotImplementedException();
        }
    }
}