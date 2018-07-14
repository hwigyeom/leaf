using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Leaf.Authorization
{
    public class AuthenticationTokenModel
    {
        private const string UserIdClaimName = JwtRegisteredClaimNames.Sub;
        private const string NameClaimName = "name";
        private const string CompanyClaimName = "company";
        private const string GroupClaimName = "group";
        private const string UniqueIdClaimName = JwtRegisteredClaimNames.Jti;
        private const string UserClassCliamName = "class";

        public string UserId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Group { get; set; }
        public string UserClass { get; set; }
        public string UniqueId { get; set; }

        public IEnumerable<Claim> ToClaims()
        {
            return new List<Claim>
            {
                new Claim(UserIdClaimName, UserId),
                new Claim(NameClaimName, Name),
                new Claim(CompanyClaimName, Company),
                new Claim(GroupClaimName, Group),
                new Claim(UserClassCliamName, UserClass),
                new Claim(UniqueIdClaimName, UniqueId)
            };
        }

        public static AuthenticationTokenModel FromClaims(IEnumerable<Claim> claims)
        {
            if (claims == null) throw new ArgumentNullException(nameof(claims));

            var claimList = claims.ToList();

            return new AuthenticationTokenModel
            {
                UserId = claimList.SingleOrDefault(c => c.Type == UserIdClaimName)?.Value,
                Name = claimList.SingleOrDefault(c => c.Type == NameClaimName)?.Value,
                Company = claimList.SingleOrDefault(c => c.Type == CompanyClaimName)?.Value,
                Group = claimList.SingleOrDefault(c => c.Type == GroupClaimName)?.Value,
                UserClass = claimList.SingleOrDefault(c => c.Type == UserClassCliamName)?.Value,
                UniqueId = claimList.SingleOrDefault(c => c.Type == UniqueIdClaimName)?.Value
            };
        }
    }
}