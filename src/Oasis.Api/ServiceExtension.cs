using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.OData;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceExtension
{
    extension(IServiceCollection services)
    {

        /// <summary>
        /// 添加依赖注入服务
        /// </summary>
        /// <returns></returns>
        public IServiceCollection AddApiServices()
        {
            //TODO:先不启用
            // services.AddControllers(options =>
            // {
            //     // 添加全局 FluentValidation 过滤器
            //     options.Filters.Add<FluentValidationActionFilter>();
            // });

            return services;
        }


        /// <summary>
        /// 添加 HTTP 请求日志服务
        /// </summary>
        /// <returns></returns>
        public IServiceCollection AddHttpLoggingService()
        {
            services.AddHttpLogging(options =>
            {
                // 记录请求/响应头与体（按需调整）
                options.LoggingFields = HttpLoggingFields.All;

                // 限制日志体大小为 64KB
                options.RequestBodyLogLimit = 64 * 1024;
                options.ResponseBodyLogLimit = 64 * 1024;

                // 仅记录文本类 media types，避免记录二进制（根据需要增减）
                options.MediaTypeOptions.Clear();
                options.MediaTypeOptions.AddText("application/json");
                options.MediaTypeOptions.AddText("text/plain");
                options.MediaTypeOptions.AddText("application/xml");
                options.MediaTypeOptions.AddText("text/html");

                // 移除/屏蔽敏感 header（避免在日志中泄露）
                // options.RequestHeaders.Remove("Authorization");
                options.RequestHeaders.Remove("Cookie");

                options.ResponseHeaders.Remove("Set-Cookie");
            });

            return services;
        }

        /// <summary>
        ///  添加身份认证服务
        /// </summary>
        /// <returns></returns>
        public IServiceCollection AddIdentityServices()
        {
            // 注册 Identity 服务
            // services.AddIdentityApiEndpoints<IdentityUser>()
            //     .AddEntityFrameworkStores<Oasis.Infrastructure.Persistence.OasisDbContext>();

            // 配置 Bearer Token 认证
            services.Configure<BearerTokenOptions>(options =>
            {
                options.BearerTokenExpiration = TimeSpan.FromHours(1); // 设置令牌过期时间
            });

            return services;
        }
    }


    extension(IMvcBuilder mvcBuilder)
    {
        /// <summary>
        /// 添加 OData 支持
        /// </summary>
        /// <returns></returns>
        public IMvcBuilder AddODataService()
        {
            return mvcBuilder.AddOData(opt =>
            {
                opt.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100);
                opt.AddRouteComponents("odata", EdmModelConfiguration.GetEdmModel());
            });
        }
    }
}