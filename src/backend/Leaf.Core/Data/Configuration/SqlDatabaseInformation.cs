using System;
using System.Data.SqlClient;

namespace Leaf.Data.Configuration
{
    /**
     * SQL Server 설정 샘플
     * {
     *   "name": "ntils",
     *   "type": "SqlServer",
     *   "store": "Plain",
     *   "server": "home.ntils.com,12433",
     *   "database": "ntils",
     *   "userid": "ntils",
     *   "password": "***",
     *   "commandTimeout": 300,
     *   "default": true
     * }
     */

    /// <inheritdoc />
    public class SqlDatabaseInformation : DatabaseInformation
    {
        private const string ServerPropertyName = "server";
        private const string DatabasePropertyName = "database";
        private const string UserIdPropertyName = "userid";
        private const string PasswordPropertyName = "password";
        private const string CommandTimeoutPropertyName = "commandTimeout";

        private int? _commandTimeout;
        private string _connectionString;

        public SqlDatabaseInformation(DatabasePropertyReaderBase propertyReader)
        {
            PropertyReader = propertyReader ?? throw new ArgumentNullException(nameof(propertyReader));
            Name = propertyReader.Name;
            DatabaseType = propertyReader.DatabaseType;
            IsDefault = propertyReader.IsDefault;
        }

        public override string ConnectionString
        {
            get
            {
                _connectionString = _connectionString ?? CreateConnectionString();
                return _connectionString;
            }
        }

        public override int? CommandTimeout
        {
            get
            {
                _commandTimeout = _commandTimeout ??
                                  (int.TryParse(PropertyReader.GetValue(CommandTimeoutPropertyName), out var result)
                                      ? result
                                      : (int?) null);
                return _commandTimeout;
            }
            internal set => _commandTimeout = value;
        }

        private string CreateConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = PropertyReader.GetValue(ServerPropertyName),
                InitialCatalog = PropertyReader.GetValue(DatabasePropertyName),
                UserID = PropertyReader.GetValue(UserIdPropertyName),
                Password = PropertyReader.GetValue(PasswordPropertyName)
            };

            return builder.ConnectionString;
        }
    }
}