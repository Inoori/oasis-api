using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oasis.Application.Interfaces;
using Oasis.Domain;
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

            services.AddIdentity<User, IdentityRole>((options) =>
            {
                // options.SignIn.RequireConfirmedEmail = true;

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<OasisDbContext>()
                .AddDefaultTokenProviders();

            // 添加 JWT 认证服务
            services.AddAuthenticationServices(configuration);

            services.AddFileStorageServices(configuration);

            services.AddScoped(typeof(IBatchOperationHandler<,>), typeof(BatchOperationHandler<,>))
                            .AddScoped<ICabinService, CabinService>()
                            .AddScoped<IGuestService, GuestService>()
                            .AddScoped<IBookingService, BookingService>()
                            .AddScoped<IUserService, UserService>()
                            .AddScoped<TokenService>();

            return services;
        }


        /// <summary>
        /// 注册 JWT 认证服务
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public IServiceCollection AddAuthenticationServices(IConfiguration configuration)
        {
            services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    }).AddJwtBearer(options =>
                    {
                        string? secretKey = configuration["Jwt:SecretKey"];
                        ArgumentNullException.ThrowIfNull(secretKey, "JWT secret key is not configured.");

                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration["Jwt:Issuer"],
                            ValidAudience = configuration["Jwt:Audience"],
                            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
                        };
                    });

            // 添加授权服务
            services.AddAuthorization();
            return services;
        }


        /// <summary>
        /// 注册文件存储服务（AWS S3 兼容的存储服务，SeaweedFS）
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IServiceCollection AddFileStorageServices(IConfiguration configuration)
        {
            // 配置 AWS S3 兼容的存储服务（如 SeaweedFS）
            services.AddSingleton<IAmazonS3>(sp =>
            {
                string accessKey = configuration["S3:AccessKey"] ?? throw new ArgumentNullException("S3:AccessKey", "S3 access key is not configured.");
                string secretKey = configuration["S3:SecretKey"] ?? throw new ArgumentNullException("S3:SecretKey", "S3 secret key is not configured.");
                string serviceUrl = configuration["S3:ServiceUrl"] ?? throw new ArgumentNullException("S3:ServiceUrl", "S3 service URL is not configured.");

                var config = new AmazonS3Config
                {
                    ServiceURL = serviceUrl,
                    ForcePathStyle = true, // 强制使用路径样式访问
                };
                return new AmazonS3Client(accessKey, secretKey, config);
            });

            services.AddScoped<IFileStorageService, FileStorageService>();

            return services;
        }
    }
}