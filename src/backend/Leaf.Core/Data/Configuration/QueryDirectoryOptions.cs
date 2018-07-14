using System.IO;

namespace Leaf.Data.Configuration
{
    /// <summary>SQL 쿼리문의 경로 설정 구성 옵션</summary>
    public class QueryDirectoryOptions
    {
        private string _baseDir;

        /// <summary>SQL 쿼리 파일의 기준 경로를 가져오거나 설정합니다.</summary>
        public string BaseDir
        {
            get => _baseDir;
            set => _baseDir = value?.Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>데이터베이스 이름 별로 분리된 디렉토리를 사용하는지 여부를 가져오거나 설정합니다.</summary>
        public bool? UseDbNameDir { get; set; }

        /// <summary>데이터베이스 형식 별로 분리된 디렉토리를 사용하는지 여부를 가져오거나 설정합니다.</summary>
        public bool? UseDbTypeDir { get; set; }
    }
}