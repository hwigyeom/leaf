using System;
using System.Collections.Generic;

namespace Leaf.Data.Configuration
{
    public class DatabaseInformationOptions : Dictionary<string, string>
    {
        private const string NamePropertyName = "name";
        private const string DatabaseTypePropertyName = "type";
        private const string StoreTypePropertyName = "store";
        private const string IsDefaultPropertyName = "default";

        public string Name => TryGetValue(NamePropertyName, out var value)
            ? value
            : throw new ApplicationException($"{NamePropertyName} 속성이 존재하지 않습니다.");

        public string DatabaseType => TryGetValue(DatabaseTypePropertyName, out var value)
            ? value
            : throw new ApplicationException($"{DatabaseTypePropertyName} 속성이 존재하지 않습니다.");

        public DatabaseInformationStoreType StoreType =>
            Enum.TryParse<DatabaseInformationStoreType>(
                TryGetValue(StoreTypePropertyName, out var value) ? value : null, out var result)
                ? result
                : throw new ApplicationException($"{StoreTypePropertyName} 속성에 지원하지 않는 값이 설정되었습니다.");

        public bool IsDefault => TryGetValue(IsDefaultPropertyName, out var value) &&
                                 bool.TryParse(value, out var result) && result;

        public string GetValue(string name)
        {
            return TryGetValue(name, out var result) ? result : null;
        }
    }
}