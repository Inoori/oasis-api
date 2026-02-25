using Microsoft.Extensions.DependencyInjection;
using FluentValidation;


namespace Oasis.Core;

public static partial class ServiceExtension
{
    extension(IServiceCollection services)
    {

        /// <summary>
        /// 添加 Core 相关的依赖注入服务
        /// </summary>
        /// <returns></returns>
        public IServiceCollection AddCoreServices()
        {
            var assembly = typeof(ServiceExtension).Assembly;
            // 注册 FluentValidation（自动扫描当前程序集的所有 Validator）
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}