"# uaa" 


## mysql migration
```
dotnet ef database drop

dotnet ef migrations add InitialCreate --context UaaMysqlDbContext -o Data\MysqlMigrations

dotnet ef database update --context UaaMysqlDbContext
```


CURRENT_TIMESTAMP() ON UPDATE CURRENT_TIMESTAMP()