namespace Leaf.Data
{
    /// <summary>관계형 데이터베이스 작업을 위한 데이터 연결 인터페이스입니다.</summary>
    public interface IDataAccessProvider
    {
        /// <summary>
        ///     구성 파일의 데이터베이스 연결 정보 중 기본 데이터베이스로 지정된 연결정보를 이용하여
        ///     <see cref="IDataAccessService" /> 인스턴스를 가져옵니다.
        /// </summary>
        /// <remarks>
        ///     연결 정보 중 기본 데이터베이스로 설정된 연결 정보가 없을 경우에는 첫번째 항목을 이용합니다.
        /// </remarks>
        IDataAccessService Default { get; }

        /// <summary>
        ///     구성 파일의 데이터베이스 연결 정보 중 지정한 이름의 연결정보를 이용하여 <see cref="IDataAccessService" /> 인스턴스를 가져옵니다.
        /// </summary>
        /// <param name="name">연결문자열의 이름</param>
        /// <returns>데이터베이스 작업을 위한 <see cref="IDataAccessService" /> 객체입니다.</returns>
        IDataAccessService GetDatabase(string name);
    }
}