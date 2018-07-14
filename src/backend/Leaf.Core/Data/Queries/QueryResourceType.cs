namespace Leaf.Data.Queries
{
    /// <summary>SQL 문장이 제공될 리소스 형태를 나타내는 열거형입니다.</summary>
    internal enum QueryResourceType
    {
        /// <summary>SQL 문장이 문자열로 바로 제공됩니다.</summary>
        Plain,

        /// <summary>SQL 문장이 파일에 저장되어 있습니다.</summary>
        File,

        /// <summary>SQL 문장이 어셈블리에 포함 리소스 형태로 저장되어 있습니다.</summary>
        Embedded
    }
}