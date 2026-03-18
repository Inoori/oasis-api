using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Oasis.Application.Interfaces;
using Oasis.Infrastructure.Persistence;
using Oasis.Infrastructure.Services;

namespace Oasis.Infrastructure;

public static partial class ServiceExtension
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// 注册基础设施层的服务
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public IServiceCollection AddInfrastructureServices(IConfiguration configuration)
        {
            services.AddDbContext<OasisDbContext>(options =>
            {
                //使用 Npgsql 作为数据库提供程序
                options.UseNpgsql(configuration.GetConnectionString("oasis_db"),
                    sql => sql.MigrationsAssembly("Oasis.Infrastructure")); // 指定迁移程序集
            });



            services.AddScoped(typeof(IBatchOperationHandler<,>), typeof(BatchOperationHandler<,>))
                            .AddScoped<ICabinService, CabinService>()
                            .AddScoped<IGuestService, GuestService>()
                            .AddScoped<IBookingService, BookingService>();

            return services;
        }
    }

}