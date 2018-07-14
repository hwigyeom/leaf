namespace Leaf.Data.Queries
{
    /// <summary>
    ///     쿼리 문장의 보간 문자열을 교체하여 완성된 쿼리문을 만드는 인터페이스입니다.
    /// </summary>
    public interface IQueryInterpolator
    {
        /// <summary>
        ///     대상 쿼리문과 교체 문자열 사전을 받아 보간을 실시합니다.
        /// </summary>
        /// <param name="query">대상 쿼리문</param>
        /// <param name="replacements">교체 문자열 사전</param>
        /// <returns>보간된 쿼리문</returns>
        string Interpolate(string query, object replacements);
    }
}