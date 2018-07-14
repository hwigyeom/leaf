using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Leaf.Data.Queries
{
    /// <inheritdoc />
    /// <summary>파일에 저장된 SQL 문장을 제공합니다.</summary>
    public class FileQueryResourceManager : IFileQueryResourceManager
    {
        public FileQueryResourceManager(IHostingEnvironment env = null)
        {
            ResourceRootPath = env?.ContentRootPath ?? Directory.GetCurrentDirectory();
            FileProvider = new PhysicalFileProvider(ResourceRootPath);
        }

        private string ResourceRootPath { get; }

        private IFileProvider FileProvider { get; }

        public string GetSqlSentence(ISqlPack sqlPack)
        {
            if (sqlPack == null) throw new ArgumentNullException(nameof(sqlPack));

            string sql;
            var filePack = (FileSqlPack) sqlPack;

            var primaryFileInfo = FileProvider.GetFileInfo(filePack.FilePath);
            var secondaryFileInfo = filePack.HasAltPath ? FileProvider.GetFileInfo(filePack.AltFilePath) : null;

            if (!primaryFileInfo.Exists && !(secondaryFileInfo?.Exists ?? false)) return null;

            using (var stream = primaryFileInfo.Exists
                ? primaryFileInfo.CreateReadStream()
                : secondaryFileInfo?.CreateReadStream())
            {
                if (stream == null) return null;

                using (var reader = new StreamReader(stream))
                {
                    sql = reader.ReadToEnd();
                }
            }

            return sql;
        }
    }
}