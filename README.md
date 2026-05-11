# Oasis API

基于 **ASP.NET Core / .NET 10** 的后端 API，提供用户认证、Cabin/Guest/Booking 业务管理、OData 查询与文件上传能力，数据存储使用 PostgreSQL，文件存储使用 S3 兼容接口（当前对接 SeaweedFS）。

## 功能概览

- 用户注册、登录、登出、个人信息查询与更新
- Cabin、Guest、Booking 批量导入与管理
- Booking 状态流转（checkin / checkout / unconfirm）
- OData 查询接口（Cabin/Guest/Booking）
- 文件上传与删除（S3 兼容对象存储）

## 技术栈

- .NET 10（`net10.0`）
- ASP.NET Core Web API
- Entity Framework Core + Npgsql (PostgreSQL)
- ASP.NET Core Identity
- FluentValidation / FluentResults / Mapster
- SeaweedFS（S3 兼容）+ AWS SDK for .NET

## 项目结构

```text
src/
  Oasis.Api/             # Web API 入口层（Controller、中间件、Program）
  Oasis.Application/     # 应用层（DTO、接口、校验器、映射配置）
  Oasis.Domain/          # 领域模型
  Oasis.Infrastructure/  # 基础设施层（EF Core、Identity、S3、业务服务实现）
  oasis.slnx             # 解决方案文件
```

## 本地运行

### 1) 启动依赖服务

项目根目录已提供容器编排文件：

- `compose.yaml`：PostgreSQL + pgweb
- `seaweedfs-compose.yaml`：SeaweedFS 集群（含 S3 网关）

```bash
docker compose -f compose.yaml up -d
docker compose -f seaweedfs-compose.yaml up -d
```

### 2) 配置环境变量

至少需要以下配置（示例）：

```bash
# PostgreSQL
export ConnectionStrings__oasis_db="Host=localhost;Port=5432;Database=<db>;Username=<user>;Password=<password>"

# JWT
export Jwt__SecretKey="<your-secret>"
export Jwt__Issuer="oasis-api"
export Jwt__Audience="oasis-web"
export Jwt__AccessTokenExpiresInMinutes="60"

# S3 / SeaweedFS
export S3__AccessKey="<access-key>"
export S3__SecretKey="<secret-key>"
export S3__ServiceUrl="http://localhost:8333"
export S3__BucketName="oasis-bucket"
```

> `src/Oasis.Api/appsettings.json` 中默认监听端口：
>
> - HTTP: `5148`
> - HTTPS: `5149`

### 3) 还原、构建、运行

```bash
cd src
dotnet restore oasis.slnx
dotnet build oasis.slnx
dotnet run --project Oasis.Api
```

### 4) 数据库迁移

```bash
cd src
dotnet ef migrations add <MigrationName> --project Oasis.Infrastructure --startup-project Oasis.Api
dotnet ef database update --project Oasis.Infrastructure --startup-project Oasis.Api
```

## API 路由概览

以下为主要路由（完整参数与响应请以代码为准）：

### Auth

- `POST /api/auth/register`
- `POST /api/auth/login`
- `POST /api/auth/logout`

### Users

- `GET /api/users/me`
- `POST /api/users/update-profile`

### Cabins

- `POST /api/cabins`
- `POST /api/cabins/upload`
- `PUT /api/cabins/{id}`
- `DELETE /api/cabins/{id}`
- `DELETE /api/cabins`

### Guests

- `POST /api/guests/upload`
- `DELETE /api/guests`

### Bookings

- `POST /api/bookings/upload`
- `DELETE /api/bookings`
- `POST /api/bookings/{id}/checkin`
- `POST /api/bookings/{id}/checkout`
- `POST /api/bookings/{id}/unconfirm`

### OData

- `GET /odata/cabins`
- `GET /odata/guests`
- `GET /odata/bookings`

支持 `$select` / `$filter` / `$orderby` / `$expand` / `$count`，并设置了 `maxTop=100`。

### Files

- `POST /api/files/upload`（`multipart/form-data`）
- `DELETE /api/files/{fileKey}`

## 常用开发命令

```bash
cd src
dotnet build oasis.slnx
dotnet test oasis.slnx
```

## 说明

- 当前代码中 Swagger 注册被注释，默认未启用 Swagger UI。
- 如需启用 Swagger，可在 `Oasis.Api` 中补充 Swagger 相关服务注册与中间件（如 `AddSwaggerGen`、`UseSwagger`、`UseSwaggerUI`），并在 `Program.cs` 中开启对应调用。
- 本项目含若干用户密钥与环境变量读取逻辑，建议仅通过本地环境变量或安全配置中心注入，不要将敏感信息提交到仓库。
