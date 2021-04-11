"# uaa" 


## mysql migration
```
dotnet ef database drop  -c UaaMysqlDbContext

dotnet ef migrations add InitialCreate -c UaaMysqlDbContext -o Data\MysqlMigrations

dotnet ef database update --context UaaMysqlDbContext
```


CURRENT_TIMESTAMP() ON UPDATE CURRENT_TIMESTAMP()