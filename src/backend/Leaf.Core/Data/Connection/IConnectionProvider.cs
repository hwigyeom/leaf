using System.Data;
using Leaf.Data.Configuration;

namespace Leaf.Data.Connection
{
    /// <summary>
    ///     데이터베이스 형식에 맞는 연결 객체(<see cref="IDbConnection" /> 인스턴스)를 생성하여 제공하는 인터페이스입니다.
    /// </summary>
    public interface IConnectionProvider
    {
        /// <summary>데이터베이스 연결 정보를 가져오거나 설정합니다.</summary>
        DatabaseInformation DatabaseInformation { get; set; }

        /// <summary>
        ///     설정된 데이터베이스 연결 정보를 기반으로 <see cref="IDbConnection" />을 생성하여 반환합니다.
        /// </summary>
        /// <returns>데이터베이스 연결 객체</returns>
        IDbConnection GetConnection();

        /// <summary>
        ///     새로운 연결을 생성하고 그 연결 객체(<see cref="IDbConnection" />)에 연결된 트랜젝션을 생성하여 반환합니다.
        /// </summary>
        /// <param name="isolation">트랜젝션 격리 수준</param>
        /// <returns>새로운 트랜젝션 객체</returns>
        IDbTransaction GetTransaction(IsolationLevel isolation = IsolationLevel.ReadCommitted);

        /// <summary>
        ///     지정된 연결에서 새로운 트랜젝션을 생성하여 반환합니다.
        /// </summary>
        /// <param name="connection">트랜젝션을 생성할 연결 객체</param>
        /// <param name="isolation">트랜젝션 격리 수준</param>
        /// <returns>새로 생성된 트랜젝션 객체</returns>
        IDbTransaction GetTransaction(IDbConnection connection,
            IsolationLevel isolation = IsolationLevel.ReadCommitted);
    }
}