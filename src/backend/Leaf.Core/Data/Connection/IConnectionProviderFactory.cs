using Leaf.Data.Configuration;

namespace Leaf.Data.Connection
{
    /// <summary>
    ///     데이터베이스 형식에 맞는 <see cref="IConnectionProvider" /> 인스턴스를 제공하는 팩토리의 인터페이스입니다.
    /// </summary>
    public interface IConnectionProviderFactory
    {
        IConnectionProvider Create(DatabaseInformation databaseInformation);
    }
}