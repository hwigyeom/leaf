using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Leaf.Data.Configuration;
using Leaf.Data.Connection;
using Leaf.Data.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Leaf.Data
{
    /// <inheritdoc />
    public class DataAccessService : IDataAccessService
    {
        private DatabaseInformation _dbInformation;

        public DataAccessService(IServiceProvider serviceProvider, IConnectionProviderFactory connectionProviderFactory,
            ILogger<DataAccessService> logger)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            ConnectionProviderFactory = connectionProviderFactory ??
                                        throw new ArgumentNullException(nameof(connectionProviderFactory));
            Logger = logger;
        }

        private IServiceProvider ServiceProvider { get; }
        private IConnectionProviderFactory ConnectionProviderFactory { get; }
        private ILogger<DataAccessService> Logger { get; }

        private IConnectionProvider ConnectionProvider { get; set; }

        public DatabaseInformation DatabaseInformation
        {
            get => _dbInformation;
            set
            {
                _dbInformation = value;
                ConnectionProvider = ConnectionProviderFactory.Create(value);
            }
        }

        public IDbConnection CreateDbConnection()
        {
            return ConnectionProvider?.GetConnection();
        }

        public IDbTransaction CreateTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return ConnectionProvider?.GetTransaction(isolationLevel);
        }

        public IDbTransaction CreateTransaction(IDbConnection connection,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return ConnectionProvider?.GetTransaction(connection, isolationLevel);
        }

        public SqlPackBuilder GetSqlPackBuilder()
        {
            return ServiceProvider.GetService<SqlPackBuilder>().DatabaseInformation(DatabaseInformation);
        }

        public int Execute(ISqlPack sqlPack)
        {
            var connection = GetConnection(sqlPack);
            Log(sqlPack);
            return connection.Execute(sqlPack.Query, sqlPack.Parameters, sqlPack.Transaction, sqlPack.CommandTimeout,
                sqlPack.CommandType);
        }

        public IEnumerable<T> Query<T>(ISqlPack sqlPack)
        {
            var connection = GetConnection(sqlPack);
            Log(sqlPack);
            return connection.Query<T>(sqlPack.Query, sqlPack.Parameters, sqlPack.Transaction, true,
                sqlPack.CommandTimeout, sqlPack.CommandType);
        }

        public T QueryFirst<T>(ISqlPack sqlPack)
        {
            var connection = GetConnection(sqlPack);
            Log(sqlPack);
            return connection.QueryFirst<T>(sqlPack.Query, sqlPack.Parameters, sqlPack.Transaction,
                sqlPack.CommandTimeout, sqlPack.CommandType);
        }

        public T QueryFirstOrDefault<T>(ISqlPack sqlPack)
        {
            var connection = GetConnection(sqlPack);
            Log(sqlPack);
            return connection.QueryFirstOrDefault<T>(sqlPack.Query, sqlPack.Parameters, sqlPack.Transaction,
                sqlPack.CommandTimeout, sqlPack.CommandType);
        }

        public T QuerySingle<T>(ISqlPack sqlPack)
        {
            var connection = GetConnection(sqlPack);
            Log(sqlPack);
            return connection.QuerySingle<T>(sqlPack.Query, sqlPack.Parameters, sqlPack.Transaction,
                sqlPack.CommandTimeout,
                sqlPack.CommandType);
        }

        public T QuerySingleOrDefault<T>(ISqlPack sqlPack)
        {
            var connection = GetConnection(sqlPack);
            Log(sqlPack);
            return connection.QuerySingleOrDefault<T>(sqlPack.Query, sqlPack.Parameters, sqlPack.Transaction,
                sqlPack.CommandTimeout, sqlPack.CommandType);
        }

        private IDbConnection GetConnection(ISqlPack sqlPack)
        {
            return sqlPack.Transaction?.Connection ?? sqlPack.Connection ?? CreateDbConnection();
        }

        private void Log(ISqlPack sqlPack)
        {
            Logger.LogDebug(sqlPack.Query);
        }
    }
}