using System.Collections.Generic;
using Leaf.Data.Configuration;
using Leaf.Data.Connection;
using Leaf.Data.Queries;

namespace Leaf.Data
{
    public class LeafDataAccessOptions
    {
        /// <summary>
        ///     구성 파일의 데이터베이스 연결 정보를 가져오거나 설정합니다.
        /// </summary>
        public IEnumerable<DatabaseInformationOptions> Databases { get; set; } = new List<DatabaseInformationOptions>();

        public QueryDirectoryOptions Queries { get; set; } = new QueryDirectoryOptions();

        public IDatabaseInformationFactory DatabaseInformationFactory { get; set; } = new DatabaseInformationFactory();

        public IConnectionProviderFactory ConnectionProviderFactory { get; set; } = new ConnectionProviderFactory();

        public IFileQueryResourceManager FileQueryResourceManager { get; set; } = new FileQueryResourceManager();

        public IEmbeddedQueryResourceManager EmbeddedQueryResourceManager { get; set; } =
            new EmbeddedQueryResourceManager();

        public IPlainQueryResourceManager PlainQueryResourceManager { get; set; } = new PlainQueryResourceManager();
    }
}