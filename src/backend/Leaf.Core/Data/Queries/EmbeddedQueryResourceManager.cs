using System.IO;
using System.Linq;
using System.Resources;

namespace Leaf.Data.Queries
{
    /// <inheritdoc />
    /// <summary>어셈블리에 포함 리소스 형태로 저장된 SQL 문장을 제공합니다.</summary>
    public class EmbeddedQueryResourceManager : IEmbeddedQueryResourceManager
    {
        public string GetSqlSentence(ISqlPack sqlPack)
        {
            string sql;
            var embeddedPack = (EmbeddedSqlPack) sqlPack;

            var resources = embeddedPack.Assembly.GetManifestResourceNames();
            string name = null;
            if (resources.Any(n => n == embeddedPack.ResourceName))
                name = embeddedPack.ResourceName;
            else if (embeddedPack.HasAltResourceName && resources.Any(n => n == embeddedPack.AltResourceName))
                name = embeddedPack.AltResourceName;

            if (name == null)
                throw new MissingManifestResourceException(embeddedPack.HasAltResourceName
                    ? $"'{embeddedPack.ResourceName}' 또는 '{embeddedPack.AltResourceName}' 으로 등록된 리소스를 찾을 수 없습니다."
                    : $"'{embeddedPack.ResourceName}' 이름으로 등록된 리소스를 찾을 수 없습니다.");

            using (var stream = embeddedPack.Assembly.GetManifestResourceStream(name))
            {
                using (var reader = new StreamReader(stream))
                {
                    sql = reader.ReadToEnd();
                }
            }

            return sql;
        }
    }
}