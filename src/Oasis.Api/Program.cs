using Microsoft.AspNetCore.Identity;
using Oasis.Api.Middleware;
using Oasis.Core;
using Oasis.Infrastructure;


var builder = WebApplication.CreateBuilder(args);


// 启用端点 API 探索器
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpLoggingService();

// 注册 Swagger 服务
// builder.Services.AddSwaggerService();

builder.Services.AddProblemDetails();

// 添加控制器服务并启用 OData支持
builder.Services.AddControllers().AddODataService();

// 身份认证服务
builder.Services.AddIdentityServices();

builder.Services.AddApiServices()
.AddCoreServices()
.AddInfrastructureServices(builder.Configuration);


var app = builder.Build();


app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseStatusCodePages();

// app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

// map identity api endpoints
app.MapControllers();

// app.MapIdentityApi<IdentityUser>();

//使用全局异常处理中间件
app.UseMiddleware<ExceptionHandler>();

app.Run();