using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Leaf.Data.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Leaf.Data
{
    /// <inheritdoc />
    public sealed class DataAccessProvider : IDataAccessProvider
    {
        public DataAccessProvider(IServiceProvider serviceProvider, DatabaseInformationProvider dbInformationProvider)
        {
            ServiceProvider = serviceProvider;
            DbInformationProvider = dbInformationProvider;
            DataAccessServices = new ConcurrentDictionary<DatabaseInformation, IDataAccessService>();
        }

        private IServiceProvider ServiceProvider { get; }
        private IDictionary<DatabaseInformation, IDataAccessService> DataAccessServices { get; }
        private DatabaseInformationProvider DbInformationProvider { get; }

        public IDataAccessService Default => GetDataAccessService(DbInformationProvider.Default);

        public IDataAccessService GetDatabase(string name)
        {
            var dbInfo = DbInformationProvider.GetDatabaseInformation(name);

            return GetDataAccessService(dbInfo);
        }

        private IDataAccessService GetDataAccessService(DatabaseInformation dbInfo)
        {
            if (DataAccessServices.TryGetValue(dbInfo, out var dataService)) return dataService;

            dataService = ServiceProvider.GetService<IDataAccessService>();
            dataService.DatabaseInformation = dbInfo;
            DataAccessServices.Add(dbInfo, dataService);

            return dataService;
        }
    }
}