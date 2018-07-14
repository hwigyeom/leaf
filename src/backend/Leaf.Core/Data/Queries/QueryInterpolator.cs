using System.Linq;

namespace Leaf.Data.Queries
{
    /// <inheritdoc />
    public class QueryInterpolator : IQueryInterpolator
    {
        public string Interpolate(string query, object replacements)
        {
            if (query == null) return null;
            if (replacements == null) return query;

            var type = replacements.GetType();
            var props = type.GetProperties();
            var pairs = props.ToDictionary(p => p.Name, p => p.GetValue(replacements, null));

            return pairs.Aggregate(query, (current, item) => current.Replace($"#{item.Key}#", item.Value.ToString()));
        }
    }
}