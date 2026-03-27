### CLI commands for setting environment variable for database connection string
#### bash or zsh
- export ConnectionStrings__oasis_db="Host=localhost;Port=5432;Database=oasis-db;Username=oasis;Password=oasis-9847923;Pooling=true;Minimum Pool Size=5;Maximum Pool Size=100;Connection Idle Lifetime=300;Connection Pruning Interval=60"

#### PowerShell
- $env:ConnectionStrings__oasis_db="Host=localhost;Port=5432;Database=oasis-db;Username=oasis;Password=oasis-9847923;Pooling=true;Minimum Pool Size=5;Maximum Pool Size=100;Connection Idle Lifetime=300;Connection Pruning Interval=60"

### CLI commands for EF Core migrations
#### add migration
```zsh
dotnet ef migrations add <MigrationName>  --project Oasis.Infrastructure --startup-project Oasis.Api
```

#### apply migration to database
```zsh
dotnet ef database update --project Oasis.Infrastructure --startup-project Oasis.Api
```