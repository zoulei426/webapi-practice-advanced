<!--
 * @Description: 基于 ABP vNext 的 Web API 进阶开发教程
 * @Author: zoulei
 * @Date: 2020-12-18 13:26:04
 * @LastEditors: zoulei
 * @LastEditTime: 2020-12-23 11:20:55
-->

# 基于 ABP vNext 的 Web API 进阶开发教程

## 目录

<!-- TOC -->

- [基于 ABP vNext 的 Web API 进阶开发教程](#基于-abp-vnext-的-web-api-进阶开发教程)
  - [目录](#目录)
  - [1 概述](#1-概述)
  - [2 环境](#2-环境)
    - [2.1 先决条件](#21-先决条件)
    - [2.2 安装 ABP CLI](#22-安装-abp-cli)
    - [2.3 安装 Redis](#23-安装-redis)
  - [3 快速创建项目](#3-快速创建项目)
    - [3.1 修改 IdentityServer 项目](#31-修改-identityserver-项目)
    - [3.2 修改 Api Host 项目](#32-修改-api-host-项目)
  - [4 业务代码](#4-业务代码)
    - [4.1 领域层](#41-领域层)
    - [4.2 数据库集成](#42-数据库集成)
    - [4.3 应用层](#43-应用层)
  - [参考文献](#参考文献)

<!-- /TOC -->

## 1 概述

本教程介绍使用 ABP vNext 构建 Web API 的进阶知识，更贴近项目实战。

在本教程中，你将了解：
- 使用 ABP CLI 快速创建 Web API 项目
- DDD 领域驱动设计
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
      "applicationUrl": "http://localhost:44307",
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

## 4 业务代码

### 4.1 领域层

按照上一教程创建 Book 实体与 BookType 枚举，接下来将创建 Author 实体，进一步实践 DDD 模式。


在 YuLinTu.Practice.Domain 中创建 Author 实体：

```c#
using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace YuLinTu.Practice.Authors
{
    public class Author : FullAuditedAggregateRoot<Guid>
    {
        #region Properties

        public string Name { get; private set; }

        public DateTime BirthDate { get; set; }

        public string ShortBio { get; set; }

        #endregion Properties

        #region Ctor

        private Author()
        {
        }

        internal Author(
            Guid id,
            [NotNull] string name,
            DateTime birthDate,
            [CanBeNull] string shortBio = null)
            : base(id)
        {
            SetName(name);
            BirthDate = birthDate;
            ShortBio = shortBio;
        }

        #endregion Ctor

        #region Methods

        internal Author ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }

        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
               name,
               nameof(name),
               maxLength: AuthorConsts.MaxNameLength
           );
        }

        #endregion Methods
    }
}
```

- Author 继承自 FullAuditedAggregateRoot<Guid>，使实体软删除，且包含所有审计属性。
- Author 的构造函数是 internal 的，所以它只能由领域层来创建。同时需要有一个 private / protected 的无参构造函数，满足从数据库读取对象时反序列化所需。
- Name 属性的 setter 是私有的，将由 ChangeName 方法来维护。
- 更多内容请查阅[实体](https://docs.abp.io/zh-Hans/abp/latest/Entities)。

在 YuLinTu.Practice.Domain.Shared 中创建常量 AuthorConsts：

```c#
namespace YuLinTu.Practice.Authors
{
    public static class AuthorConsts
    {
        public const int MaxNameLength = 64;
    }
}
```

在 YuLinTu.Practice.Domain 中创建仓储接口 IAuthorRepository：

```c#
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace YuLinTu.Practice.Authors
{
    public interface IAuthorRepository : IRepository<Author, Guid>
    {
        Task<Author> FindByNameAsync(string name);

        Task<List<Author>> GetListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string filter = null
        );
    }
}
```

在 YuLinTu.Practice.Domain 中创建域服务 AuthorManager：

```c#
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace YuLinTu.Practice.Authors
{
    public class AuthorManager : DomainService
    {
        private readonly IAuthorRepository authorRepository;

        public AuthorManager(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        public async Task<Author> CreateAsync(
            [NotNull] string name,
            DateTime birthDate,
            [CanBeNull] string shortBio = null)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existingAuthor = await authorRepository.FindByNameAsync(name);
            if (existingAuthor != null)
            {
                throw new AuthorAlreadyExistsException(name);
            }

            return new Author(
                GuidGenerator.Create(),
                name,
                birthDate,
                shortBio
            );
        }

        public async Task ChangeNameAsync(
            [NotNull] Author author,
            [NotNull] string newName)
        {
            Check.NotNull(author, nameof(author));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existingAuthor = await authorRepository.FindByNameAsync(newName);
            if (existingAuthor != null && existingAuthor.Id != author.Id)
            {
                throw new AuthorAlreadyExistsException(newName);
            }

            author.ChangeName(newName);
        }
    }
}
```

- 域服务建议使用 Manager / Service 作为后缀。
- 在构造函数中注入 IAuthorRepository 接口。

在 YuLinTu.Practice.Domain 中创建业务异常 AuthorAlreadyExistsException：

```c#
using Volo.Abp;

namespace YuLinTu.Practice.Authors
{
    public class AuthorAlreadyExistsException : BusinessException
    {
        public AuthorAlreadyExistsException(string name)
            : base(PracticeErrorCodes.AuthorAlreadyExists)
        {
            WithData("name", name);
        }
    }
}
```

- AuthorAlreadyExistsException 继承自 BusinessException，BusinessException 用于引发业务相关异常，可以更容易地实现本地化需求。

修改 YuLinTu.Practice.Domain.Shared 中的 PracticeErrorCodes.cs 文件：

```c#
namespace YuLinTu.Practice
{
    public static class PracticeErrorCodes
    {
        public const string AuthorAlreadyExists = "Practice:00001";
    }
}
```

打开 YuLinTu.Practice.Domain.Shared 项目中的 Localization/Practice/zh-Hans.json 文件，并添加：

```json
"Practice:00001": "已存在相同姓名的作者：{name}"
```

### 4.2 数据库集成

打开 YuLinTu.Practice.EntityFrameworkCore 项目中的 PracticeDbContext.cs 文件，并添加 Book 与 Author 数据集：

```c#
public DbSet<Book> Books { get; set; }

public DbSet<Author> Authors { get; set; }
```

打开 PracticeDbContextModelCreatingExtensions.cs 文件，添加 Book 与 Author 的数据库映射：

```c#
using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using YuLinTu.Practice.Authors;
using YuLinTu.Practice.Books;

namespace YuLinTu.Practice.EntityFrameworkCore
{
    public static class PracticeDbContextModelCreatingExtensions
    {
        public static void ConfigurePractice(
            this ModelBuilder builder,
            Action<PracticeModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new PracticeModelBuilderConfigurationOptions(
                PracticeDbProperties.DbTablePrefix,
                PracticeDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<Book>(b =>
            {
                b.ToTable(options.TablePrefix + "Books",
                          options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            });

            builder.Entity<Author>(b =>
            {
                b.ToTable(options.TablePrefix + "Authors",
                    options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(AuthorConsts.MaxNameLength);

                b.HasIndex(x => x.Name);
            });

            // 将数据库字段全小写且通过下划线分隔
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToSnakeCase());
                }
            }
        }
    }
}
```

创建 IAuthorRepository 的实现类 EfCoreAuthorRepository：

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using YuLinTu.Practice.EntityFrameworkCore;

namespace YuLinTu.Practice.Authors
{
    public class EfCoreAuthorRepository
           : EfCoreRepository<PracticeDbContext, Author, Guid>,
               IAuthorRepository
    {
        public EfCoreAuthorRepository(
            IDbContextProvider<PracticeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Author> FindByNameAsync(string name)
        {
            return await DbSet.FirstOrDefaultAsync(author => author.Name == name);
        }

        public async Task<List<Author>> GetListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string filter = null)
        {
            return await DbSet
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    author => author.Name.Contains(filter)
                 )
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
```

- 更多内容请查阅 [EntityFrameworkCore](https://docs.abp.io/zh-Hans/abp/latest/Entity-Framework-Core)

### 4.3 应用层

按照上一教程创建 BookDto, CreateUpdateBookDto, IBookAppService, BookAppService。



## 参考文献

- [Mircrosoft Docs](https://docs.microsoft.com/zh-cn/aspnet/core/web-api/?view=aspnetcore-5.0)
- [ABP Framework Docs](https://docs.abp.io/zh-Hans/abp/latest/Getting-Started?UI=MVC&DB=EF&Tiered=No)

