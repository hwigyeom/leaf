using System;
using System.IO;
using System.Linq;
using Leaf.Data.Configuration;
using Microsoft.Extensions.Logging;

namespace Leaf.Data.Queries
{
    /// <inheritdoc />
    /// <summary>물리 파일 경로에 제공된 SQL 쿼리 문장을 이용하는 명령입니다.</summary>
    public sealed class FileSqlPack : SqlPackBase
    {
        private string _filename;

        public FileSqlPack(IFileQueryResourceManager resourceManager, IQueryInterpolator interpolator,
            QueryDirectoryOptions directoryOptions, ILogger logger = null) : base(resourceManager, interpolator, logger)
        {
            DirectoryOptions = directoryOptions ?? throw new ArgumentNullException(nameof(directoryOptions));
            HasAltPath = directoryOptions.UseDbTypeDir.HasValue && directoryOptions.UseDbTypeDir.Value;
        }

        /// <summary>대체 경로가 존재하는지 여부를 가져옵니다.</summary>
        public bool HasAltPath { get; }

        private QueryDirectoryOptions DirectoryOptions { get; }

        /// <summary>SQL 쿼리 파일명을 가져옵니다.</summary>
        public string Filename
        {
            get => _filename;
            internal set
            {
                _filename = !string.IsNullOrWhiteSpace(value) &&
                            !value.ToCharArray().Intersect(Path.GetInvalidPathChars()).Any()
                    ? value
                    : throw new ApplicationException("제공된 파일명이 유효하지 않습니다.");

                string filePath;

                if (DirectoryOptions.UseDbNameDir.HasValue && DirectoryOptions.UseDbNameDir.Value)
                    filePath = Path.Combine(DirectoryOptions.BaseDir, DatabaseName);
                else
                    filePath = DirectoryOptions.BaseDir;

                if (HasAltPath)
                {
                    AltFilePath = Path.Combine(filePath, _filename);
                    FilePath = Path.Combine(filePath, DatabaseType, _filename);
                }
                else
                {
                    FilePath = Path.Combine(filePath, _filename);
                }
            }
        }

        /// <summary>SQL 쿼리 파일을 찾을 첫번째 경로를 가져옵니다.</summary>
        public string FilePath { get; set; }

        /// <summary><see cref="FilePath" />에 파일이 존재하지 않을 경우 사용한 대체 경로를 가져옵니다.</summary>
        public string AltFilePath { get; set; }
    }
}