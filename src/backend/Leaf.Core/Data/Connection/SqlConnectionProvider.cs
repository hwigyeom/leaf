using System.Data;
using System.Data.SqlClient;
using Leaf.Data.Configuration;

namespace Leaf.Data.Connection
{
    /// <inheritdoc />
    public class SqlConnectionProvider : IConnectionProvider
    {
        public SqlConnectionProvider(DatabaseInformation databaseInformation)
        {
            DatabaseInformation = databaseInformation;
        }

        public DatabaseInformation DatabaseInformation { get; set; }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(DatabaseInformation.ConnectionString);
        }

        public IDbTransaction GetTransaction(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            return GetTransaction(null, isolation);
        }

        public IDbTransaction GetTransaction(IDbConnection connection,
            IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            connection = (SqlConnection) connection ?? GetConnection();

            connection.Open();
            var transaction = connection.BeginTransaction(isolation);

            return transaction;
        }
    }
}