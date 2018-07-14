using System;
using System.Collections.Generic;
using System.Linq;

namespace Leaf.Data.Configuration
{
    public class DatabaseInformationProvider
    {
        public DatabaseInformationProvider(IEnumerable<DatabaseInformationOptions> config,
            IDatabaseInformationFactory factory)
        {
            DatabaseOptions = config;
            InformationFactory = factory;
        }

        private IEnumerable<DatabaseInformationOptions> DatabaseOptions { get; }

        private IDatabaseInformationFactory InformationFactory { get; }

        private IList<DatabaseInformation> DatabaseInformations { get; } = new List<DatabaseInformation>();

        private string DefaultDatabaseName => DatabaseOptions?.SingleOrDefault(c => c.IsDefault)?.Name
                                              ?? DatabaseOptions?.FirstOrDefault()?.Name
                                              ?? throw new ApplicationException("데이터베이스 연결 정보가 존재하지 않습니다.");

        public virtual DatabaseInformation Default => GetDatabaseInformation(DefaultDatabaseName);

        public virtual DatabaseInformation GetDatabaseInformation(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            // 한 번 이상 호출한 적이 있으면 이전에 만들어진 항목을 반환
            var dbInfo =
                DatabaseInformations.SingleOrDefault(c => name.Equals(c.Name, StringComparison.OrdinalIgnoreCase));

            if (dbInfo != null) return dbInfo;

            // 구성 옵션에서 이름을 찾아 항목을 생성
            dbInfo = DatabaseOptions.Where(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                         .Select(ConvertConfigurationToDatabaseInformation).SingleOrDefault()
                     ?? throw new KeyNotFoundException($"이름이 '{name}'인 데이터베이스 연결 정보를 찾을 수 없습니다.");

            DatabaseInformations.Add(dbInfo);

            return dbInfo;
        }

        private DatabaseInformation ConvertConfigurationToDatabaseInformation(DatabaseInformationOptions options)
        {
            DatabasePropertyReaderBase propertyReader = null;

            // 저장 형식
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (options.StoreType)
            {
                // TODO: 암호화 문자열, 키 스토어 용 IDatabasePropertyReader 구현
                case DatabaseInformationStoreType.Plain: // 일반 문자열 이용
                case DatabaseInformationStoreType.Secure: // 암호화 문자열 이용
                case DatabaseInformationStoreType.KeyStore: // 키 스토어 이용
                    propertyReader = new PlainDatabasePropertyReader(options);
                    break;
            }

            var dbInfo = InformationFactory.Create(propertyReader);

            if (dbInfo == null) throw new ApplicationException($"'{options.DatabaseType}' 은 지원하지 않은 데이터베이스 형식입니다.");

            return dbInfo;
        }
    }
}