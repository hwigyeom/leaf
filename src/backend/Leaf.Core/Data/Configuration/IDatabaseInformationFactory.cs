namespace Leaf.Data.Configuration
{
    /// <summary>
    ///     구성파일에서 제공된 <see cref="DatabaseInformationOptions" /> 정보를 이용하여
    ///     데이터베이스 접속 정보인 <see cref="DatabaseInformation" /> 클래스의 인스턴스를 생성하여 제공하는 팩토리 클래스입니다.
    /// </summary>
    public interface IDatabaseInformationFactory
    {
        /// <summary>
        ///     구성파 일의 데이터베이스 연결 정보 속성 읽기 기능을 제공하는 <see cref="DatabasePropertyReaderBase" />의 인스턴스를 제공받아
        ///     데이터베이스 접속 정보를 생성합니다.
        /// </summary>
        /// <param name="propertyReader">속성 값의 저장 형태에 따른 읽기 방법을 제공하는 리더 객체</param>
        /// <returns>데이터베이스 연결 정보</returns>
        DatabaseInformation Create(DatabasePropertyReaderBase propertyReader);
    }
}