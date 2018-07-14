using System;
using System.Data;
using System.Reflection;
using Leaf.Data.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Leaf.Data.Queries
{
    public class SqlPackBuilder
    {
        private Assembly _assembly;
        private int? _commandTimeout;
        private CommandType? _commandType;
        private IDbConnection _connection;
        private DatabaseInformation _dbInformation;
        private string _defaultNamespace;
        private string _filename;
        private bool _init;
        private object _parameters;
        private string _plain;
        private object _replacements;
        private QueryResourceType _resourceType;
        private IDbTransaction _transaction;

        public SqlPackBuilder(IQueryInterpolator interpolator, IServiceProvider serviceProvider)
        {
            Interpolator = interpolator ?? throw new ArgumentNullException(nameof(interpolator));
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private IQueryInterpolator Interpolator { get; }
        private IServiceProvider ServiceProvider { get; }

        public SqlPackBuilder DatabaseInformation(DatabaseInformation database)
        {
            _dbInformation = database ?? throw new ArgumentNullException(nameof(database));
            return this;
        }

        public SqlPackBuilder File(string filename)
        {
            _init = !_init ? true : throw new ApplicationException("이미 SqlPack의 형식이 정해진 상태입니다.");
            _resourceType = QueryResourceType.File;
            _filename = filename;
            return this;
        }

        public SqlPackBuilder Embedded(string filename, Assembly assembly = null, string defaultNamespace = null)
        {
            _init = !_init ? true : throw new ApplicationException("이미 SqlPack의 형식이 정해진 상태입니다.");
            _resourceType = QueryResourceType.Embedded;
            _filename = filename;
            _assembly = assembly ?? Assembly.GetCallingAssembly();
            _defaultNamespace = defaultNamespace;
            return this;
        }

        public SqlPackBuilder Plain(string sql)
        {
            _init = !_init ? true : throw new ApplicationException("이미 SqlPack의 형식이 정해진 상태입니다.");
            _resourceType = QueryResourceType.Plain;
            _plain = sql;
            return this;
        }

        public SqlPackBuilder Parameters(object parameters)
        {
            _parameters = parameters;
            return this;
        }

        public SqlPackBuilder Replacements(object replacements)
        {
            _replacements = replacements;
            return this;
        }

        public SqlPackBuilder Connection(IDbConnection connection)
        {
            if (_transaction != null)
                throw new ApplicationException("이미 데이터베이스에 대한 연결이 설정되어 있습니다.");
            _connection = connection;
            return this;
        }

        public SqlPackBuilder Transaction(IDbTransaction transaction)
        {
            if (_connection != null)
                throw new ApplicationException("이미 데이터베이스에 대한 연결이 설정되어 있습니다.");
            _transaction = transaction;
            return this;
        }

        public SqlPackBuilder CommandType(CommandType commandType)
        {
            _commandType = commandType;
            return this;
        }

        public SqlPackBuilder CommandTimeout(int commandTimeout)
        {
            _commandTimeout = commandTimeout;
            return this;
        }

        public ISqlPack Build()
        {
            if (!_init) throw new ApplicationException("SqlPack 형식이 설정되지 않았습니다.");
            if (_dbInformation == null) throw new ApplicationException("데이터베이스 연결 정보가 제공되지 않았습니다.");

            ISqlPack sqlPack;

            switch (_resourceType)
            {
                case QueryResourceType.File:
                    var filePack = ServiceProvider.GetService<FileSqlPack>();
                    filePack.Filename = _filename;

                    sqlPack = filePack;
                    break;
                case QueryResourceType.Embedded:
                    var embeddedPack = ServiceProvider.GetService<EmbeddedSqlPack>();
                    embeddedPack.SetResource(_filename, _defaultNamespace, _assembly);

                    sqlPack = embeddedPack;
                    break;
                default:
                    var plainPack = ServiceProvider.GetService<PlainSqlPack>();
                    plainPack.Text = _plain;

                    sqlPack = plainPack;
                    break;
            }

            ((SqlPackBase) sqlPack).SetInformations(_dbInformation, _connection, _transaction, _parameters,
                _replacements, _commandType, _commandTimeout);

            return sqlPack;
        }
    }
}