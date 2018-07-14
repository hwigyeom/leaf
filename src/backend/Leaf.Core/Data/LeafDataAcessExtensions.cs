using System;
using Leaf.Data.Configuration;
using Leaf.Data.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Leaf.Data
{
    public static class LeafDataAcessExtensions
    {
        public static IServiceCollection AddLeafDataAccess(this IServiceCollection services,
            Action<LeafDataAccessOptions> setup)
        {
            if (setup == null) throw new ArgumentNullException(nameof(setup));

            AddDefaultDependencies(services);

            var options = new LeafDataAccessOptions();
            setup(options);

            services.AddSingleton(options.Databases);
            services.AddSingleton(options.Queries);
            services.AddSingleton(options.DatabaseInformationFactory);
            services.AddSingleton(options.ConnectionProviderFactory);
            services.AddSingleton(options.FileQueryResourceManager);
            services.AddSingleton(options.EmbeddedQueryResourceManager);
            services.AddSingleton(options.PlainQueryResourceManager);

            return services;
        }

        private static void AddDefaultDependencies(IServiceCollection services)
        {
            services.AddSingleton<IDataAccessProvider, DataAccessProvider>();
            services.AddSingleton<IDataAccessService, DataAccessService>();
            services.AddSingleton<DatabaseInformationProvider>();

            // ISqlPack 관련
            services.AddTransient<SqlPackBuilder>();
            services.AddTransient<FileSqlPack>();
            services.AddTransient<EmbeddedSqlPack>();
            services.AddTransient<PlainSqlPack>();
            services.AddSingleton<IQueryInterpolator, QueryInterpolator>();
        }
    }
}