using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Leaf.Data.Configuration;
using Microsoft.Extensions.Logging;

namespace Leaf.Data.Queries
{
    /// <inheritdoc />
    /// <summary>어셈블리 내에 포함 리소스로 존재하는 SQL 쿼리 문장을 이용하는 명령입니다.</summary>
    public sealed class EmbeddedSqlPack : SqlPackBase
    {
        public EmbeddedSqlPack(IEmbeddedQueryResourceManager resourceManager, IQueryInterpolator interpolator,
            QueryDirectoryOptions directoryOptions, ILogger logger = null) : base(resourceManager, interpolator, logger)
        {
            DirectoryOptions = directoryOptions ?? throw new ArgumentNullException(nameof(directoryOptions));
            HasAltResourceName = DirectoryOptions.UseDbTypeDir.HasValue && DirectoryOptions.UseDbTypeDir.Value;
        }

        private QueryDirectoryOptions DirectoryOptions { get; }

        /// <summary>대체 포함 리소스 이름이 존재하는지 여부를 가져옵니다.</summary>
        public bool HasAltResourceName { get; }

        /// <summary>SQL 쿼리 문장이 포함된 어셈블리를 가져옵니다.</summary>
        /// <remarks>따로 설정하지 않으면 호출한 어셈블리가 사용됩니다.</remarks>
        public Assembly Assembly { get; private set; }

        /// <summary>포함 리소스의 이름을 가져옵니다.</summary>
        public string ResourceName { get; private set; }

        /// <summary><see cref="ResourceName" />으로 찾을 수 없을 때 사용할 대체 포함 리소스 이름을 가져옵니다.</summary>
        public string AltResourceName { get; private set; }

        public void SetResource(string filename, string defaultNamespace, Assembly assembly)
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));

            filename = !filename.ToCharArray().Intersect(Path.GetInvalidPathChars()).Any()
                ? filename.Trim('/', '\\').Replace('/', '.').Replace('\\', '.')
                : throw new ApplicationException("제공된 파일명이 유효하지 않습니다.");

            Assembly = assembly;

            var baseDir = DirectoryOptions.BaseDir.TrimStart('.').Trim('/', '\\').Replace('/', '.').Replace('\\', '.');
            var resourceName = $"{defaultNamespace ?? Assembly.GetName().Name}.{baseDir}";

            if (DirectoryOptions.UseDbNameDir.HasValue && DirectoryOptions.UseDbNameDir.Value)
                resourceName = $"{resourceName}.{DatabaseName}";

            if (HasAltResourceName)
            {
                ResourceName = $"{resourceName}.{DatabaseType}.{filename}";
                AltResourceName = $"{resourceName}.{filename}";
            }
            else
            {
                ResourceName = $"{resourceName}.{filename}";
            }
        }
    }
}