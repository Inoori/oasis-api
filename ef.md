<!-- set connection string -->
export ConnectionStrings__oasis_db="Host=localhost;Port=5432;Database=oasis-db;Username=oasis;Password=oasis-9847923"


<!-- dotnet 生成迁移 -->
dotnet ef migrations add <MigrationName>  --project src/Oasis.Infrastructure --startup-project src/Oasis.Api                   

<!-- dotnet 更新数据库 -->
dotnet ef database update --project src/Oasis.Infrastructure --startup-project src/Oasis.Api



dotnet ef migrations add init_table --project src/Oasis.Infrastructure --startup-project src/Oasis.Api