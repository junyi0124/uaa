"# uaa"

## dotnet-ef update
```cmd
dotnet tool update -g dotnet-ef
```

## mysql migration
```cmd
dotnet ef database drop  -c UaaMysqlDbContext

dotnet ef migrations add InitialCreate -c UaaMysqlDbContext -o Data\MysqlMigrations

dotnet ef database update -c UaaMysqlDbContext
```


CURRENT_TIMESTAMP() ON UPDATE CURRENT_TIMESTAMP()