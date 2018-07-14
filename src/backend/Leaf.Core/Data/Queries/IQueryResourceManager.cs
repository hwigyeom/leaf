namespace Leaf.Data.Queries
{
    /// <summary>각각의 리소스에 저장된 SQL 문장을 제공하는 인터페이스입니다.</summary>
    public interface IQueryResourceManager
    {
        /// <summary>지정한 키의 SQL 문장을 가져옵니다.</summary>
        /// <param name="sqlPack">SQL 문장을 찾기 위한 <see cref="ISqlPack" /> 인스턴스입니다.</param>
        /// <returns>SQL 문장</returns>
        string GetSqlSentence(ISqlPack sqlPack);
    }
}