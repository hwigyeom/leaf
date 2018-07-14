using System.Collections.Generic;
using System.Data;
using Leaf.Data.Configuration;
using Leaf.Data.Queries;

namespace Leaf.Data
{
    /// <summary>데이터베이스 처리를 관장하는 인터페이스입니다.</summary>
    public interface IDataAccessService
    {
        /// <summary>데이터베이스 연결 정보를 가져옵니다.</summary>
        DatabaseInformation DatabaseInformation { get; set; }

        /// <summary>
        ///     새로운 데이터베이스 연결 객체(<see cref="IDbConnection" />)를 가져옵니다.
        /// </summary>
        /// <returns>데이터베이스에 대한 새로운 연결 객체</returns>
        IDbConnection CreateDbConnection();

        /// <summary>
        ///     새로운 연결을 생성하고 그 연결 객체(<see cref="IDbConnection" />)에 연결된 트랜젝션을 생성하여 반환합니다.
        /// </summary>
        /// <param name="isolationLevel">트랜젝션 격리 수준</param>
        /// <returns>새로운 트랜젝션 객체</returns>
        IDbTransaction CreateTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        ///     지정된 연결에서 새로운 트랜젝션을 생성하여 반환합니다.
        /// </summary>
        /// <param name="connection">트랜젝션을 생성할 연결 객체</param>
        /// <param name="isolationLevel">트랜젝션 격리 수준</param>
        /// <returns>새로 생성된 트랜젝션 객체</returns>
        IDbTransaction CreateTransaction(IDbConnection connection,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        ///     데이터베이스에서 쿼리문을 실행하기 위한 SQL 명령 정보 빌더인 <see cref="SqlPackBuilder" /> 객체를 생성합니다.
        /// </summary>
        /// <returns>SQL 명령 정보 빌더 객체</returns>
        SqlPackBuilder GetSqlPackBuilder();

        /// <summary>설정된 쿼리 문장의 명령을 수행하고 영향을 받은 행의 수를 반환합니다.</summary>
        /// <param name="sqlPack">데이터베이스에서 실행한 SQL 쿼리 명령</param>
        /// <returns>적용 받은 행의 수</returns>
        int Execute(ISqlPack sqlPack);

        /// <summary>설정된 쿼리 문장의 명령을 수행하고 수행 결과를 지정한 형식으로 가져옵니다.</summary>
        /// <typeparam name="T">반환할 형식</typeparam>
        /// <returns>명령 수행 결과</returns>
        IEnumerable<T> Query<T>(ISqlPack sqlPack);

        /// <summary>설정된 쿼리 문장의 명령을 수행하고 수행 결과의 첫번째 항목을 지정한 형식으로 가져옵니다.</summary>
        /// <typeparam name="T">반환할 형식</typeparam>
        /// <returns>명령 수행 결과</returns>
        T QueryFirst<T>(ISqlPack sqlPack);

        /// <summary>
        ///     <para>설정된 쿼리 문장의 명령을 수행하고 수행 결과의 첫번째 항목을 지정한 형식으로 가져옵니다.</para>
        ///     <para>데이터가 존재하지 않으면 지정한 형식의 기본값을 반환합니다.</para>
        /// </summary>
        /// <typeparam name="T">반환할 형식</typeparam>
        /// <returns>명령 수행 결과</returns>
        T QueryFirstOrDefault<T>(ISqlPack sqlPack);

        /// <summary>
        ///     <para>설정된 쿼리 문장의 명령을 수행하고 하나의 결과만을 지정한 형식으로 가져옵니다.</para>
        ///     <para>하나의 결과만 반환하지 않으면 예외가 발생합니다.</para>
        /// </summary>
        /// <typeparam name="T">반환할 형식</typeparam>
        /// <returns>명령 수행 결과</returns>
        T QuerySingle<T>(ISqlPack sqlPack);

        /// <summary>
        ///     <para>설정된 쿼리 문장의 명령을 수행하고 하나의 결과를 지정한 형식으로 가져오거나 존재하지 않으면 지정한 형식의 기본값을 가져옵니다.</para>
        /// </summary>
        /// <typeparam name="T">반환할 형식</typeparam>
        /// <returns>명령 수행 결과</returns>
        T QuerySingleOrDefault<T>(ISqlPack sqlPack);
    }
}