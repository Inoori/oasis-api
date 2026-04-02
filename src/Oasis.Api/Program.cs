using Microsoft.AspNetCore.Identity;
using Oasis.Api.Middleware;
using Oasis.Infrastructure;
using Oasis.Application;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;  // 校验作用域捕获问题
    options.ValidateOnBuild = true; // 构建时校验所有服务能否被创建
});


// 启用端点 API 探索器
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpLoggingService();

// 注册 Swagger 服务
// builder.Services.AddSwaggerService();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// 添加控制器服务并启用 OData支持
builder.Services.AddControllers().AddODataService();


builder.Services.AddApiServices()
.AddApplication()
.AddInfrastructureServices(builder);



var app = builder.Build();


app.UseExceptionHandler();

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseCors(options =>
{
    // options.WithOrigins("http://192.168.1.100")
    // 允许任何来源、方法和头部（根据需要调整）
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseAuthentication();
app.UseAuthorization();

// map identity api endpoints
app.MapControllers();

// app.MapIdentityApi<IdentityUser>();


app.Run();