using Leaf;
using Leaf.Authorization;
using Leaf.Data;

namespace Ensys.SA.Repositories
{
    [DependencyService(typeof(IAuthorizationRepository))]
    public class AuthorizationRepository : IAuthorizationRepository
    {
        public AuthorizationRepository(IDataAccessProvider dbProvider)
        {
            Database = dbProvider.Default;
        }

        private IDataAccessService Database { get; }

        public AuthenticationAccountModel GetAuthenticationAccount(string userId)
        {
            var sqlPack = Database.GetSqlPackBuilder()
                .Embedded("SA_USER_SEL_AUTH.sql")
                .Parameters(new
                {
                    ID_USER = userId
                })
                .Build();

            var account = Database.QueryFirstOrDefault<AuthenticationAccountModel>(sqlPack);

            return account;
        }

        public bool UpdatePassword(string userId, string hashedPassword)
        {
            var sqlPack = Database.GetSqlPackBuilder()
                .Embedded("SA_USER_UPD_PASS.sql")
                .Parameters(new
                {
                    hashedPassword,
                    UserId = userId
                })
                .Build();

            var result = false;

            using (var tran = Database.CreateTransaction())
            {
                var count = Database.Execute(sqlPack);

                if (count == 1)
                {
                    tran.Commit();
                    result = true;
                }
                else
                {
                    tran.Rollback();
                }
            }

            return result;
        }
    }
}