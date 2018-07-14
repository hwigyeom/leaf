using System.Collections.Generic;
using Leaf.Data.Queries;

#pragma warning disable 1573

namespace Leaf.Data
{
    public static class DataAccessServiceExtensions
    {
        /// <summary>설정된 쿼리 문장의 명령을 수행하고 수행 결과의 컬럼 이름과 매치되는 속성을 가진 다이나믹 객체의 목록을 반환합니다.</summary>
        /// <param name="sqlPack">데이터베이스에서 실행한 SQL 쿼리 명령</param>
        /// <returns>컬럼 이름과 매치되는 속성을 가진 다이나믹 객체의 목록</returns>
        public static IEnumerable<dynamic> Query(this IDataAccessService service, ISqlPack sqlPack)
        {
            return service.Query<dynamic>(sqlPack);
        }

        /// <summary>설정된 쿼리 문장의 명령을 수행하고 수행 결과의 첫번째 항목을 다이나믹 객체로 가져옵니다.</summary>
        /// <param name="sqlPack">데이터베이스에서 실행한 SQL 쿼리 명령</param>
        /// <returns>명령 수행 결과</returns>
        public static dynamic QueryFirst(this IDataAccessService service, ISqlPack sqlPack)
        {
            return service.QueryFirst<dynamic>(sqlPack);
        }

        /// <summary>
        ///     <para>설정된 쿼리 문장의 명령을 수행하고 수행 결과의 첫번째 항목을 다이나믹 객체로 가져옵니다.</para>
        ///     <para>데이터가 존재하지 않으면 지정한 형식의 기본값을 반환합니다.</para>
        /// </summary>
        /// <param name="sqlPack">데이터베이스에서 실행한 SQL 쿼리 명령</param>
        /// <returns>명령 수행 결과</returns>
        public static dynamic QueryFirstOrDefault(this IDataAccessService service, ISqlPack sqlPack)
        {
            return service.QueryFirstOrDefault<dynamic>(sqlPack);
        }

        /// <summary>
        ///     <para>설정된 쿼리 문장의 명령을 수행하고 하나의 결과만을 다이나믹 객체로 가져옵니다.</para>
        ///     <para>하나의 결과만 반환하지 않으면 예외가 발생합니다.</para>
        /// </summary>
        /// <param name="sqlPack">데이터베이스에서 실행한 SQL 쿼리 명령</param>
        /// <returns>명령 수행 결과</returns>
        public static dynamic QuerySingle(this IDataAccessService service, ISqlPack sqlPack)
        {
            return service.QuerySingle<dynamic>(sqlPack);
        }

        /// <summary>
        ///     <para>설정된 쿼리 문장의 명령을 수행하고 하나의 결과를 다이나믹 객체로 가져오고 존재하지 않으면 기본값을 가져옵니다.</para>
        /// </summary>
        /// <param name="sqlPack">데이터베이스에서 실행한 SQL 쿼리 명령</param>
        /// <returns>명령 수행 결과</returns>
        public static dynamic QuerySingleOrDefault(this IDataAccessService service, ISqlPack sqlPack)
        {
            return service.QuerySingle<dynamic>(sqlPack);
        }
    }
}