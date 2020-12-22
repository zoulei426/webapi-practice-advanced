<!--
 * @Description: 基于 ABP vNext 的 Web API 进阶开发教程
 * @Author: zoulei
 * @Date: 2020-12-18 13:26:04
 * @LastEditors: zoulei
 * @LastEditTime: 2020-12-22 14:55:31
-->

# 基于 ABP vNext 的 Web API 进阶开发教程

## 目录



## 1 概述

本教程介绍使用 ABP vNext 构建 Web API 的进阶知识，更贴近项目实战。

在本教程中，你将了解：
- 使用 ABP CLI 快速创建 Web API 项目
- 使用 IdentityServer4, AutoMapper...

谁适合阅读本教程：
- 已完成《基于 ABP vNext 的 Web API 开发教程》

## 2 环境

### 2.1 先决条件

- .NET 5.0 SDK 或更高版本
- 具有“ASP.NET 和 Web 开发”工作负载的 Visual Studio 2019 16.8 或更高版本
- API CLI 4.0 或更高版本
- Postgresql 9.2 或更高版本
- Redis

### 2.2 安装 ABP CLI

安装 ABP CLI

```cmd
dotnet tool install -g Volo.Abp.Cli
```

或升级至最新版本。

```cmd
dotnet tool update -g Volo.Abp.Cli
```

### 2.3 安装 Redis

## 3 快速创建项目

使用ABP CLI的 new 命令创建新项目。

```cmd
abp new YuLinTu.Practice -t module --no-ui
```

运行以上命令后将会创建一个模块化的解决方案结构：

- src 文件夹包含基于DDD原则分层的实际模块
- test 文件夹包含单元和集成测试
- host 文件夹包含具有不同配置的应用程序，用于演示在应用程序中如何托管模块。

### 3.1 修改 IdentityServer 项目

将 YuLinTu.Practice.IdentityServer 设为启动项，在程序包管理控制台选择此项目为默认项目。

安装 ABP 组件：

```PM
Install-Package Volo.Abp.EntityFrameworkCore.PostgreSql
```

修改 appsettings.json 文件中的 ConnectionStrings 节点：

```json
"ConnectionStrings": {
    "Default": "Server=127.0.0.1;Port=5432;Database=practice_main;User Id=postgres;Password=123456;"
  }
```

修改 Properties 目录下的 launchSettings.json 文件：

```json
{
  "profiles": {
    "IdentityServerHost": {
      "commandName": "Project",
      "launchBrowser": true,
      "applicationUrl": "https://localhost:44312",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

修改 PracticeIdentityServerModule.cs 文件：

> 在 DependsOn 属性中添加 AbpEntityFrameworkCorePostgreSqlModule 模块。  
> 修改 ConfigureServices 方法中的 AbpDbContextOptions 配置：
> ```c#
> Configure<AbpDbContextOptions>(options =>
> {
>     //options.UseSqlServer();
>     options.UseNpgsql();
> });
> ```

修改 EntityFrameworkCore 目录下的 IdentityServerHostMigrationsDbContextFactory.cs 文件中的 CreateDbContext方法：

```c#
public IdentityServerHostMigrationsDbContext CreateDbContext(string[] args)
{
    var configuration = BuildConfiguration();
    var builder = new DbContextOptionsBuilder<IdentityServerHostMigrationsDbContext>()
        .UseNpgsql(configuration.GetConnectionString("Default"));
    return new IdentityServerHostMigrationsDbContext(builder.Options);
}
```

删除 Migrations 目录。

在程序包管理控制台中输入以下命令创建数据迁移文件：

```cmd
Add-Migration "Init"
```

再输入以下命令更新数据库：

```cmd
Update-Database
```

运行 Redis。

在项目根路径下使用 dotnet run 启动项目。

### 3.2 修改 Api Host 项目

将 YuLinTu.Practice.HttpApi.Host 设为启动项，在程序包管理控制台选择此项目为默认项目。

安装 ABP 组件：

```PM
Install-Package Volo.Abp.EntityFrameworkCore.PostgreSql
```

修改 appsettings.json 文件中的 ConnectionStrings 节点：

```json
"ConnectionStrings": {
    "Default": "Server=127.0.0.1;Port=5432;Database=practice_main;User Id=postgres;Password=123456;",
    "Practice": "Server=127.0.0.1;Port=5432;Database=practice_module;User Id=postgres;Password=123456;"
  }
```

修改 Properties 目录下的 launchSettings.json 文件：

```json
{
  "profiles": {
    "YuLinTu.Practice.DemoApp": {
      "commandName": "Project",
      "launchBrowser": true,
      "applicationUrl": "https://localhost:44340",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

修改 PracticeHttpApiHostModule.cs 文件：

> 在 DependsOn 属性中添加 AbpEntityFrameworkCorePostgreSqlModule 模块。  
> 修改 ConfigureServices 方法中的 AbpDbContextOptions 配置：
> ```c#
> Configure<AbpDbContextOptions>(options =>
> {
>     //options.UseSqlServer();
>     options.UseNpgsql();
> });
> ```

修改 EntityFrameworkCore 目录下的 IdentityServerHostMigrationsDbContextFactory.cs 文件中的 CreateDbContext方法：

```c#
public PracticeHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
{
    var configuration = BuildConfiguration();
    var builder = new DbContextOptionsBuilder<PracticeHttpApiHostMigrationsDbContext>()
        .UseNpgsql(configuration.GetConnectionString("Practice"));
    return new PracticeHttpApiHostMigrationsDbContext(builder.Options);
}
```

运行此项目。

## 参考文献

- [Mircrosoft Docs](https://docs.microsoft.com/zh-cn/aspnet/core/web-api/?view=aspnetcore-5.0)
- [ABP Framework Docs](https://docs.abp.io/zh-Hans/abp/latest/Getting-Started?UI=MVC&DB=EF&Tiered=No)