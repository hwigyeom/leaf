namespace Leaf.Data.Configuration
{
    /// <inheritdoc />
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class DatabaseInformationFactory : IDatabaseInformationFactory
    {
        public virtual DatabaseInformation Create(DatabasePropertyReaderBase propertyReader)
        {
            DatabaseInformation dbInformation = null;

            switch (propertyReader.DatabaseType?.ToLower())
            {
                case "sqlserver":
                    dbInformation = new SqlDatabaseInformation(propertyReader);
                    break;
            }

            return dbInformation;
        }
    }
}