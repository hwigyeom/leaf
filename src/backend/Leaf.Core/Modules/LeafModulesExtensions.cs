using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Leaf.Modules
{
    public static class LeafModulesExtensions
    {
        public static IServiceCollection AddLeafModules(this IServiceCollection services, LeafModulesOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            RegisterModulesDependencies(services, options);
            return services;
        }

        public static IServiceCollection AddLeafModules(this IServiceCollection services,
            Func<LeafModulesOptions, LeafModulesOptions> setup)
        {
            RegisterModulesDependencies(services, setup(new LeafModulesOptions()));
            return services;
        }

        private static void RegisterModulesDependencies(IServiceCollection services, LeafModulesOptions options)
        {
            // TODO: 어셈블리에서 디펜던시를 판단해서 등록하는 코드를 분리할 것
            var fileProvider = new PhysicalFileProvider(options.BasePath);

            var dllFiles = fileProvider.GetDirectoryContents("");

            if (!dllFiles.Exists) return;

            foreach (var dllFile in dllFiles
                .Where(f => !f.IsDirectory && f.Exists &&
                            Path.GetExtension(f.Name).Equals(@".dll", StringComparison.OrdinalIgnoreCase)))
            {
                // TODO: try catch 처리
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllFile.PhysicalPath);

                var classes =
                    from type in assembly.GetTypes()
                    let attribute = type.GetCustomAttribute(typeof(DependencyServiceAttribute), true)
                    where attribute != null
                    select new {Type = type, Attribute = (DependencyServiceAttribute) attribute};

                foreach (var serviceClass in classes)
                    if (serviceClass.Attribute.ImplemenType != null)
                    {
                        // TODO: ImplementType을 지정한 인터페이스를 구현한 클래스가 아닐 경우 예외를 발생시킬지 결정 - 예외 발생 시 시스템 스타트 오류 처리 필요
                        if (!serviceClass.Attribute.ImplemenType.IsAssignableFrom(serviceClass.Type)) continue;

                        // TODO: 지정한 인터페이스 형식으로 등록된 클래스가 있을 경우의 처리

                        if (serviceClass.Attribute.Singleton)
                            services.AddSingleton(serviceClass.Attribute.ImplemenType, serviceClass.Type);
                        else
                            services.AddTransient(serviceClass.Attribute.ImplemenType, serviceClass.Type);
                    }
                    else
                    {
                        if (serviceClass.Attribute.Singleton)
                            services.AddSingleton(serviceClass.Type);
                        else
                            services.AddTransient(serviceClass.Type);
                    }
            }

            // TODO: Dll 파일의 변경이 있을 때 어플리케이션을 셧다운 - 재시작은 운영체제에 일임 (systemd 나 supervisord 이용)
            // https://www.blakepell.com/asp-net-core-ability-to-restart-your-site-programatically-updated-for-2-0
            // https://www.blakepell.com/asp-net-core-ability-to-restart-your-site-programmatically

            var token = fileProvider.Watch("**/*.dll");

            token.RegisterChangeCallback(state => { options.Shutdown?.Invoke(); }, null);
        }
    }
}