using Microsoft.Extensions.Logging;

namespace Leaf.Data.Queries
{
    /// <inheritdoc />
    public sealed class PlainSqlPack : SqlPackBase
    {
        public PlainSqlPack(IPlainQueryResourceManager resourceManager, IQueryInterpolator interpolator,
            ILogger logger = null) : base(resourceManager, interpolator, logger)
        {
        }

        public string Text { get; internal set; }
    }
}