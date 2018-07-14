using Leaf.Data;

namespace Leaf.Authorization
{
    public class AccountRepository
    {
        public AccountRepository(IDataAccessProvider dbProvider)
        {
            Database = dbProvider.Default;
        }

        private IDataAccessService Database { get; }

        public AuthenticationAccountModel GetAuthenticationAccount(string userId)
        {
            var sqlPack = Database.GetSqlPackBuilder()
                .Embedded("SA/SA_USER_SEL_AUTH.sql", defaultNamespace: "Leaf")
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
                .Embedded("SA/SA_USER_UPD_PASS.sql", defaultNamespace: "Leaf")
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