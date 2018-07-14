namespace Leaf.Data.Configuration
{
    /// <inheritdoc />
    /// <summary>구성 파일에 일반 문자열로 저장된 데이터베이스 연결 정보의 속성을 가져옵니다.</summary>
    internal class PlainDatabasePropertyReader : DatabasePropertyReaderBase
    {
        public PlainDatabasePropertyReader(DatabaseInformationOptions options) : base(options)
        {
        }

        public override string GetValue(string name)
        {
            return DatabaseOptions.GetValue(name);
        }
    }
}