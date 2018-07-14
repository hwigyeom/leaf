namespace Leaf.Data.Configuration
{
    /// <summary>데이터베이스 연결 정보가 저장된 형태를 나타내는 열거형입니다.</summary>
    public enum DatabaseInformationStoreType
    {
        /// <summary>일반 문자열</summary>
        Plain,

        /// <summary>암호화된 문자열</summary>
        Secure,

        /// <summary>키 스토어</summary>
        KeyStore
    }
}