using System;
using System.Data;
using Leaf.Data.Configuration;
using Microsoft.Extensions.Logging;

namespace Leaf.Data.Queries
{
    /// <inheritdoc />
    public abstract class SqlPackBase : ISqlPack
    {
        protected SqlPackBase(IQueryResourceManager resourceManager, IQueryInterpolator interpolator,
            ILogger logger = null)
        {
            ResourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
            Interpolator = interpolator ?? throw new ArgumentNullException(nameof(interpolator));
            Logger = logger;
        }

        private IQueryInterpolator Interpolator { get; }
        private IQueryResourceManager ResourceManager { get; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private ILogger Logger { get; }

        public string DatabaseName { get; private set; }
        public string DatabaseType { get; private set; }
        public string ConnectionString { get; private set; }
        public object Parameters { get; private set; }
        public object Replacements { get; private set; }
        public CommandType? CommandType { get; private set; }
        public int? CommandTimeout { get; private set; }
        public IDbConnection Connection { get; private set; }
        public IDbTransaction Transaction { get; private set; }

        public virtual string Query
        {
            get
            {
                var query = Raw;
                return Interpolator?.Interpolate(query, Replacements) ?? query;
            }
        }

        public virtual string Raw => ResourceManager.GetSqlSentence(this);

        internal void SetInformations(DatabaseInformation dbInformation, IDbConnection connection = null,
            IDbTransaction transaction = null, object parameters = null, object replacement = null,
            CommandType? commandType = null, int? commandTimeout = null)
        {
            DatabaseName = dbInformation.Name;
            DatabaseType = dbInformation.DatabaseType;
            ConnectionString = dbInformation.ConnectionString;
            Connection = connection;
            Transaction = transaction;
            Parameters = parameters;
            Replacements = replacement;
            CommandType = commandType;
            CommandTimeout = commandTimeout ?? dbInformation.CommandTimeout;
        }
    }
}