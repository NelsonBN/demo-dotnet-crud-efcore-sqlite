# Demo - .NET With EF Core and SQLite

## EF Core


### Install EF Core tools

```bash
dotnet tool install --global dotnet-ef
```


## Remove old migrations

```bash
dotnet ef migrations remove
```


## Add new migration

```bash
dotnet ef migrations add Init
dotnet ef migrations add Init --output-dir Data/Migrations
```


## Update database

```bash
dotnet ef database update
```


## Database

### VSCode Extension

- [SQLite](https://marketplace.visualstudio.com/items?itemName=alexcvzz.vscode-sqlite)
