using Mapster;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace Oasis.Application;

public static partial class ServiceExtension
{

    extension(IServiceCollection services)
    {
        /// <summary>
        /// 注册应用层的服务
        /// </summary>
        /// <returns></returns>
        public IServiceCollection AddApplication()
        {
            var assembly = typeof(ServiceExtension).Assembly;

            // 注册 Mapster 映射配置
            TypeAdapterConfig.GlobalSettings.Scan(assembly);
            //忽略null 值
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);

            // 注册 FluentValidation（自动扫描当前程序集的所有 Validator）
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}