namespace Leaf.Data.Queries
{
    /// <inheritdoc />
    /// <summary>일반 문자열로 제공된 SQL 문자을 가져옵니다.</summary>
    public class PlainQueryResourceManager : IPlainQueryResourceManager
    {
        public string GetSqlSentence(ISqlPack sqlPack)
        {
            var plainPack = (PlainSqlPack) sqlPack;
            return plainPack.Text;
        }
    }
}