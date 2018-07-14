using System;

namespace Leaf.Data.Configuration
{
    /// <summary>데이터베이스 연결정보를 나타내는 추상클래스입니다.</summary>
    public abstract class DatabaseInformation
    {
        /// <summary>데이터베이스 연결 정보의 이름을 가져옵니다.</summary>
        public string Name { get; internal set; }

        /// <summary>연결할 데이터베이스의 형식을 가져옵니다.</summary>
        public string DatabaseType { get; internal set; }

        /// <summary>명령 실행을 종료하고 오류를 생성하기 전 대기 시간을 가져옵니다.</summary>
        public virtual int? CommandTimeout { get; internal set; }

        /// <summary>데이터베이스 연결 문자열을 가져옵니다.</summary>
        public abstract string ConnectionString { get; }

        /// <summary>기본 데이터베이스인지 여부를 가져옵니다.</summary>
        public bool IsDefault { get; set; }

        /// <summary>
        ///     데이터베이스별로 다를 수 있는 데이터베이스 연결 속성 정보를 제공하는
        ///     <see cref="DatabasePropertyReaderBase" /> 객체를 가져옵니다.
        /// </summary>
        protected DatabasePropertyReaderBase PropertyReader { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is DatabaseInformation compare)
                return Name.Equals(compare.Name, StringComparison.OrdinalIgnoreCase);
            return false;
        }

        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}