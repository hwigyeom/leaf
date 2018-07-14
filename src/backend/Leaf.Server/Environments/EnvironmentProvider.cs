using Microsoft.Extensions.Configuration;

namespace Leaf.Environments
{
    public class EnvironmentProvider : IEnvironmentProvider
    {
        private readonly IConfiguration _configuration;

        public EnvironmentProvider(IConfiguration config)
        {
            _configuration = config;
        }

        public T GetGlobalEnvironments<T>() where T : new()
        {
            return GetConfiguredEnvironments<T>("environments:global");
        }

        public T GetLoginEnvironments<T>() where T : new()
        {
            return GetConfiguredEnvironments<T>("environments:login");
        }

        public T GetMainEnvironments<T>() where T : new()
        {
            return GetConfiguredEnvironments<T>("environments:main");
        }

        private T GetConfiguredEnvironments<T>(string sectionName) where T : new()
        {
            var env = new T();
            _configuration.GetSection(sectionName).Bind(env);
            return env;
        }
    }
}