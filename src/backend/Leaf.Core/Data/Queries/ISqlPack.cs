using System.Data;

namespace Leaf.Data.Queries
{
    /// <summary>데이터베이스에서 실행할 명령을 나타내는 인터페이스입니다.</summary>
    public interface ISqlPack
    {
        /// <summary>쿼리 명령을 실행할 데이터베이스 연결 정보 이름을 가져옵니다.</summary>
        string DatabaseName { get; }

        /// <summary>쿼리 명령을 실행할 데이터베이스의 형식을 가져옵니다.</summary>
        string DatabaseType { get; }

        /// <summary>쿼리 명령을 실행할 데이터베이스의 연결 문자열을 가져옵니다.</summary>
        string ConnectionString { get; }

        /// <summary>실제 데이터베이스에서 실행할 SQL 쿼리 문장을 가져옵니다.</summary>
        string Query { get; }

        /// <summary>리소스에 등록된 SQL 쿼리 문장을 그대로 가져옵니다.</summary>
        string Raw { get; }

        /// <summary>명령을 수행하기 위한 파라미터를 가져옵니다.</summary>
        object Parameters { get; }

        /// <summary>쿼리 문장의 보간에 사용할 파라미터를 가져옵니다.</summary>
        object Replacements { get; }

        /// <summary>
        ///     쿼리 명령의 형식을 가져옵니다.
        ///     설정하지 않았다면 자동으로 판단합니다.
        /// </summary>
        CommandType? CommandType { get; }

        /// <summary>
        ///     쿼리를 실행의 타임아웃 시간을 가져옵니다.
        ///     설정하지 않았다면 기본 값을 사용합니다.
        /// </summary>
        int? CommandTimeout { get; }

        /// <summary>
        ///     쿼리 실행 시 사용하기 위해 직접 설정한 연결 객체를 가져옵니다.
        ///     설정하지 않았다면 기본 연결 객체 사용 방법을 따릅니다.
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        ///     쿼리 실행 시 사용하기 위해 직접 설정한 트랜잭션 객체를 가져옵니다.
        ///     설정하지 않으면 트랜젝션 객체를 사용하지 않습니다.
        /// </summary>
        IDbTransaction Transaction { get; }
    }
}