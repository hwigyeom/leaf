using System;

namespace Leaf.Data.Configuration
{
    /// <summary>데이터베이스 연결 정보 저장소에서 연결 정보 개별 항목을 제공하는 추상클래스입니다.</summary>
    public abstract class DatabasePropertyReaderBase
    {
        /// <summary>데이터베이스 연결 정보 항목 구성 섹션을 이용하여 초기화합니다.</summary>
        /// <param name="options">개별 데이터베이스 연결 정보 구성 섹션</param>
        protected DatabasePropertyReaderBase(DatabaseInformationOptions options)
        {
            DatabaseOptions = options ?? throw new ArgumentNullException(nameof(options));
        }

        public string Name => DatabaseOptions.Name;

        public string DatabaseType => DatabaseOptions.DatabaseType;

        public DatabaseInformationStoreType StoreType => DatabaseOptions.StoreType;

        public bool IsDefault => DatabaseOptions.IsDefault;

        protected DatabaseInformationOptions DatabaseOptions { get; }

        /// <summary>지정한 이름의 속성 값을 가져옵니다.</summary>
        /// <param name="name">속성 이름</param>
        /// <returns>속성 값</returns>
        public abstract string GetValue(string name);
    }
}