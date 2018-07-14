using Leaf.Data.Configuration;

namespace Leaf.Data.Connection
{
    /// <inheritdoc />
    public class ConnectionProviderFactory : IConnectionProviderFactory
    {
        public virtual IConnectionProvider Create(DatabaseInformation databaseInformation)
        {
            switch (databaseInformation.DatabaseType.ToLower())
            {
                case "sqlserver":
                    return new SqlConnectionProvider(databaseInformation);

                default:
                    return null;
            }
        }
    }
}