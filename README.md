<!--
 * @Description: 基于 ABP vNext 的 Web API 进阶开发教程
 * @Author: zoulei
 * @Date: 2020-12-18 13:26:04
 * @LastEditors: zoulei
 * @LastEditTime: 2020-12-25 18:09:52
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
    - [4.4 控制器层](#44-控制器层)
  - [5 响应结果封装](#5-响应结果封装)
  - [6 参数验证](#6-参数验证)
  - [7 认证 / 授权](#7-认证--授权)
  - [8 缓存](#8-缓存)
  - [9 REST](#9-rest)
    - [9.1 URI](#91-uri)
    - [9.2 HTTP 动词](#92-http-动词)
    - [9.3 媒体类型 Media-Type](#93-媒体类型-media-type)
    - [9.4 HATEOAS](#94-hateoas)
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

在本教程中，我们将在 Author 和 Book 实体之间建立 1 到 N 的关系，进一步实践 DDD 模式。

### 4.1 领域层

按照上一教程创建 Book 实体与 BookType 枚举，并在 Book 类中添加：

```c#
public Guid AuthorId { get; set; }
```

- 为了遵循 DDD 最佳实践，建议仅通过 ID 引用其他聚合。


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

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public DateTime BirthDate { get; set; }

        public string ShortBio { get; set; }

        #endregion Properties

        #region Ctor

        private Author()
        {
        }

        internal Author(
            Guid id,
            [NotNull] string firstName,
            [NotNull] string lastName,
            DateTime birthDate,
            [CanBeNull] string shortBio = null)
            : base(id)
        {
            SetName(firstName, lastName);
            BirthDate = birthDate;
            ShortBio = shortBio;
        }

        #endregion Ctor

        #region Methods

        internal Author ChangeName([NotNull] string firstName, [NotNull] string lastName)
        {
            SetName(firstName, lastName);
            return this;
        }

        private void SetName([NotNull] string firstName, [NotNull] string lastName)
        {
            FirstName = Check.NotNullOrWhiteSpace(
               firstName,
               nameof(firstName),
               maxLength: AuthorConsts.MaxNameLength
           );

            LastName = Check.NotNullOrWhiteSpace(
               lastName,
               nameof(lastName),
               maxLength: AuthorConsts.MaxNameLength
           );
        }

        #endregion Methods
    }
}
```

- Author 继承自 FullAuditedAggregateRoot<Guid>，使实体软删除，且包含所有审计属性。
- Author 的构造函数是 internal 的，所以它只能由领域层来创建。同时需要有一个 private / protected 的无参构造函数，满足从数据库读取对象时反序列化所需。
- FirstName, LastName 属性的 setter 是私有的，将由 ChangeName 方法来维护。
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
        Task<Author> FindByNameAsync(string firstName, string lastName);

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
            [NotNull] string firstName,
            [NotNull] string lastName,
            DateTime birthDate,
            [CanBeNull] string shortBio = null)
        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName));

            var existingAuthor = await authorRepository.FindByNameAsync(firstName, lastName);
            if (existingAuthor != null)
            {
                throw new AuthorAlreadyExistsException(firstName + lastName);
            }

            return new Author(
                GuidGenerator.Create(),
                firstName,
                lastName,
                birthDate,
                shortBio
            );
        }

        public async Task ChangeNameAsync(
            [NotNull] Author author,
            [NotNull] string newFirstName,
            [NotNull] string newLastName)
        {
            Check.NotNull(author, nameof(author));
            Check.NotNullOrWhiteSpace(newFirstName, nameof(newFirstName));
            Check.NotNullOrWhiteSpace(newLastName, nameof(newLastName));

            var existingAuthor = await authorRepository.FindByNameAsync(newFirstName, newLastName);
            if (existingAuthor != null && existingAuthor.Id != author.Id)
            {
                throw new AuthorAlreadyExistsException(newFirstName + newLastName);
            }

            author.ChangeName(newFirstName, newLastName);
        }
    }
}
```

- 域服务建议使用 Manager / Service 作为后缀。
- 在构造函数中注入 IAuthorRepository 接口。
- 域服务相对于应用层的服务而言，主要处理聚合的、单一的业务，而应用层的服务则相对是统筹的。

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

                b.HasOne<Author>().WithMany().HasForeignKey(x => x.AuthorId).IsRequired();
            });

            builder.Entity<Author>(b =>
            {
                b.ToTable(options.TablePrefix + "Authors",
                    options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.FirstName)
                    .IsRequired()
                    .HasMaxLength(AuthorConsts.MaxNameLength);

                b.Property(x => x.LastName)
                    .IsRequired()
                    .HasMaxLength(AuthorConsts.MaxNameLength);

                b.HasIndex("FirstName", "LastName").IsUnique();
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

        public async Task<Author> FindByNameAsync(string firstName, string lastName)
        {
            return await DbSet.FirstOrDefaultAsync(
                author => author.FirstName == firstName
                       && author.LastName == lastName);
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
                    author => author.FirstName.Contains(filter)
                    || author.LastName.Contains(filter)
                 )
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
```

修改 PracticeEntityFrameworkCoreModule.cs 文件：

```c#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace YuLinTu.Practice.EntityFrameworkCore
{
    [DependsOn(
        typeof(PracticeDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class PracticeEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PracticeDbContext>(options =>
            {
                // 添加缺省仓储
                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}
```

- 更多内容请查阅 [EntityFrameworkCore](https://docs.abp.io/zh-Hans/abp/latest/Entity-Framework-Core)

将 YuLinTu.Practice.HttpApi.Host 设为启动项，在程序包管理控制台选择此项目为默认项目。

在程序包管理控制台中输入以下命令创建数据迁移文件：

```cmd
Add-Migration "Init"
```

再输入以下命令更新数据库：

```cmd
Update-Database
```

### 4.3 应用层

创建 BookDto：

```c#
using System;
using Volo.Abp.Application.Dtos;

namespace YuLinTu.Practice.Books
{
    public class BookDto : AuditedEntityDto<Guid>
    {
        public Guid AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }
    }
}
```

按照上一教程创建 CreateUpdateBookDto, IBookAppService, BookAppService。

```c#
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace YuLinTu.Practice.Authors
{
    public class AuthorDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public int Age { get; set; }

        public string ShortBio { get; set; }
    }
}
```

- 为了体验更多 AutoMapper 的配置，所以特意将 AuthorDto 的 Name 对应 Author 的 FirstName, LastName，并且 Age 将根据 BirthDate 计算。

创建 CreateAuthorDto：

```c#
using System;
using System.ComponentModel.DataAnnotations;

namespace YuLinTu.Practice.Authors
{
    public class CreateAuthorDto
    {
        [Required]
        [StringLength(AuthorConsts.MaxNameLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(AuthorConsts.MaxNameLength)]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public string ShortBio { get; set; }
    }
}
```

创建 UpdateAuthorDto：

```c#
using System;
using System.ComponentModel.DataAnnotations;

namespace YuLinTu.Practice.Authors
{
    public class UpdateAuthorDto
    {
        [Required]
        [StringLength(AuthorConsts.MaxNameLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(AuthorConsts.MaxNameLength)]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public string ShortBio { get; set; }
    }
}
```

创建 GetAuthorListDto 继承自 PagedAndSortedResultRequestDto，并添加过滤字段 Filter：

```c#
using Volo.Abp.Application.Dtos;

namespace YuLinTu.Practice.Authors
{
    public class GetAuthorListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
```

创建 IAuthorAppService：

```c#
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace YuLinTu.Practice.Authors
{
    public interface IAuthorAppService : IApplicationService
    {
        Task<AuthorDto> GetAsync(Guid id);

        Task<PagedResultDto<AuthorDto>> GetListAsync(GetAuthorListDto input);

        Task<AuthorDto> CreateAsync(CreateAuthorDto input);

        Task UpdateAsync(Guid id, UpdateAuthorDto input);

        Task DeleteAsync(Guid id);
    }
}
```

创建 AuthorAppService 实现 IAuthorAppService：

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace YuLinTu.Practice.Authors
{
    public class AuthorAppService : PracticeAppService, IAuthorAppService
    {
        private readonly IAuthorRepository authorRepository;
        private readonly AuthorManager authorManager;

        public AuthorAppService(IAuthorRepository authorRepository, AuthorManager authorManager)
        {
            this.authorRepository = authorRepository;
            this.authorManager = authorManager;
        }

        public async Task<AuthorDto> CreateAsync(CreateAuthorDto input)
        {
            var author = await authorManager.CreateAsync(
                input.FirstName,
                input.LastName,
                input.BirthDate,
                input.ShortBio);

            await authorRepository.InsertAsync(author);

            return ObjectMapper.Map<Author, AuthorDto>(author);
        }

        public async Task DeleteAsync(Guid id)
        {
            await authorRepository.DeleteAsync(id);
        }

        public async Task<AuthorDto> GetAsync(Guid id)
        {
            var author = await authorRepository.GetAsync(id);
            return ObjectMapper.Map<Author, AuthorDto>(author);
        }

        public async Task<PagedResultDto<AuthorDto>> GetListAsync(GetAuthorListDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(Author.FirstName);
            }

            var authors = await authorRepository.GetListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Filter
            );

            var totalCount = await AsyncExecuter.CountAsync(
                authorRepository.WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    author => author.FirstName.Contains(input.Filter)
                    || author.LastName.Contains(input.Filter)
                )
            );

            return new PagedResultDto<AuthorDto>(
                totalCount,
                ObjectMapper.Map<List<Author>, List<AuthorDto>>(authors)
            );
        }

        public async Task UpdateAsync(Guid id, UpdateAuthorDto input)
        {
            var author = await authorRepository.GetAsync(id);

            if (author.FirstName != input.FirstName || author.LastName != input.LastName)
            {
                await authorManager.ChangeNameAsync(author, input.FirstName, input.LastName);
            }

            author.BirthDate = input.BirthDate;
            author.ShortBio = input.ShortBio;

            await authorRepository.UpdateAsync(author);
        }
    }
}
```

修改 PracticeApplicationAutoMapperProfile.cs 文件配置实体与 DTO 的映射：

```c#
using AutoMapper;
using System;
using System.Linq;
using YuLinTu.Practice.Authors;
using YuLinTu.Practice.Books;

namespace YuLinTu.Practice
{
    public class PracticeApplicationAutoMapperProfile : Profile
    {
        public PracticeApplicationAutoMapperProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateUpdateBookDto, Book>();

            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName}{src.LastName}"))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => (DateTime.Now - src.BirthDate).Days / 365));

            CreateMap<CreateAuthorDto, Author>();
            CreateMap<UpdateAuthorDto, Author>();

            ForAllMaps((typeMap, mappingExpr) =>
            {
                // 忽略未映射的映射
                foreach (var dest in typeMap.DestinationTypeDetails.PublicReadAccessors)
                {
                    if (!typeMap.PropertyMaps.Any(t => t.DestinationName.Equals(dest.Name)))
                    {
                        mappingExpr.ForMember(dest.Name, opt => opt.Ignore());
                    }
                }
            });
        }
    }
}
```

由于 ABP 框架默认配置了从应用服务自动生成 API，此时已经可以启动 YuLinTu.Practice.HttpApi.Host 项目，在 Swagger 查看到 Author 与 Book 接口。

### 4.4 控制器层

取消从应用层自动生成API的功能，修改 YuLinTu.Practice.HttpApi.Host 项目中的 PracticeHttpApiHostModule.cs 文件，注释以下代码：

```c#
 //Configure<AbpAspNetCoreMvcOptions>(options =>
//{
//    options.ConventionalControllers.Create(typeo(PracticeApplicationModule).Assembly);
//});
```

在 YuLinTu.Practice.HttpApi 项目中创建 BookController：

```c#
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using YuLinTu.Practice.Authors;

namespace YuLinTu.Practice.Books
{
    [RemoteService]
    [Route("api/practice/authors/{authorId}/books")]
    public class BookController : PracticeController
    {
        private readonly IBookAppService bookAppService;
        private readonly IAuthorAppService authorAppService;

        public BookController(IBookAppService bookAppService, IAuthorAppService authorAppService)
        {
            Check.NotNull(bookAppService, nameof(bookAppService));
            Check.NotNull(authorAppService, nameof(authorAppService));

            this.bookAppService = bookAppService;
            this.authorAppService = authorAppService;
        }
    }
}
```

- 由于在本教程中 Author 和 Book 实体之间是 1 到 N 的关系，所以在 Route 属性中的路由配置为 authors/{authorId}/books，这样更能体现 Author 与 Book 资源的关系。当然也可以将接口设计为 api/practice/books，当根据实际团队的约定而决定。
- Route 路由中的 authors, books 是复数形式，当然也可以是单数形式，同样根据实际的约定或习惯而决定。
- 在 Book 控制器中可能会使用 Author, Book 的服务，因此在构造函数中注入了 IBookAppService, IAuthorAppService 服务，同时使用 Check 进行参数检查。

在 BookController 中添加 GetBookForAuthor 方法：

```c#
[HttpGet("{bookId}")]
public async Task<IActionResult> GetBookForAuthor(Guid authorId, Guid bookId)
{
    if (!await authorAppService.IsExistedAsync(authorId))
        return NotFound();
    var result = await bookAppService.GetBookForAuthorAsync(authorId, bookId);
    return Ok(result);
}
```

- 此方法使用 GET 方法，并配置路由 bookId，这样访问此方法的完整路由就是 api/practice/authors/{authorId}/books/{bookId}。
- NotFound, Ok 方法将分别返回 404, 200 的状态码。
- IsExistedAsync 方法根据 authorId 判断 Author 是否存在，GetBookForAuthorAsync 方法根据 authorId, bookId 返回 BookDto，具体实现请查阅源代码。

在 BookController 中添加 CreateBookForAuthor 方法：

```c#
[HttpPost]
public async Task<IActionResult> CreateBookForAuthor(Guid authorId,CreateUpdateBookDto book)
{
    if (!await authorAppService.IsExistedAsync(authorId))
        return NotFound();
    var result = await bookAppService.CreateBookForAuthorAsync(authorId, book);
    return Ok(result);
}
```

## 5 响应结果封装

在 YuLinTu.Practice.HttpApi.Host 项目中创建过滤器 ApiResultFilter：

```c#
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YuLinTu.Practice.Filters
{
    public class ApiResultFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is null)
            {
                var result = context.Result as ObjectResult;
                if (result is not null)
                {
                    // 封装结果
                    var apiResult = new ApiResult<object>();
                    apiResult.Success(result.Value);
                    context.Result = new ObjectResult(apiResult);
                }
            }
            base.OnActionExecuted(context);
        }
    }
}
```

在 PracticeHttpApiHostModule 中配置 ApiResultFilter 过滤器：

```c#
Configure<MvcOptions>(options =>
{
    options.Filters.Add(typeof(ApiResultFilter));
});
```

- ApiResult 的实现请参考源代码或根据实际业务需求而定。
- ApiResultFilter 的使用可以全局配置，也可以在各个 Controller 上通过添加 [ApiResult] 属性达到更颗粒化的控制。

## 6 参数验证

## 7 认证 / 授权

## 8 缓存

## 9 REST

### 9.1 URI

### 9.2 HTTP 动词

- RESTFul API 中的幂等性是指调用某个方法 1 次或 N 次对资源产生的影响结果都是相同的。
- 接口符合幂等性可以降低系统实现的复杂性，并能保证资源状态的一致性。

> HTTP 方法 | 是否幂等 | 是否安全
> :-: | :-: | :-:
> OPTIONS | Y | Y
> HEAD | Y | Y
> GET | Y | Y
> PUT | Y | N
> DELETE | Y | N
> POST | N | N
> PATCH | N | N

使用 PUT 更新 Book，在 BookController 中添加 UpdateBookForAuthor 方法：

```c#
[HttpPut("{bookId}")]
public async Task<IActionResult> UpdateBookForAuthor(Guid authorId,Guid bookId, CreateUpdateBookDto book)
{
    if (!await authorAppService.IsExistedAsync(authorId))
        return NotFound();
    await bookAppService.UpdateBookForAuthorAsync(authorId, bookId, book);
    return NoContent();
}
```

使用 Patch 局部更新 Book，在 BookController 中添加 PartiallyUpdateBookForAuthor 方法：

```c#
[HttpPatch("{bookId}")]
public async Task<IActionResult> PartiallyUpdateBookForAuthor(
    Guid authorId,
    Guid bookId,
    JsonPatchDocument<CreateUpdateBookDto> patchDocument)
{
    if (!await authorAppService.IsExistedAsync(authorId))
        return NotFound();
    var bookDto = await bookAppService.GetBookForAuthorAsync(authorId, bookId);
    var dtoToPatch = ObjectMapper.Map<BookDto, CreateUpdateBookDto>(bookDto);
    patchDocument.ApplyTo(dtoToPatch, ModelState);
    await bookAppService.UpdateBookForAuthorAsync(authorId, bookId, dtoToPatch);
    return NoContent();
}
```

在 PracticeHttpApiHostModule.cs 文件中增加 application/json-patch+json 媒体类型配置：

```c#
public override void ConfigureServices(ServiceConfigurationContext context)
{
    ConfigureInputFormatters();
    // ...
}
private void ConfigureInputFormatters()
{
    Configure<MvcOptions>(options =>
    {
        options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
    });
}
private static NewtonsoftJsonPatchInputFormatterGetJsonPatchInputFormatter()
{
    var builder = new ServiceCollection()
        .AddLogging()
        .AddMvc()
        .AddNewtonsoftJson()
        .Services.BuildServiceProvider();
    return builder
        .GetRequiredService<IOptions<MvcOptions>>()
        .Value
        .InputFormatters
        .OfType<NewtonsoftJsonPatchInputFormatter>()
        .First();
}
```

- 更多 Patch 的内容请查阅[处理 JSON Patch 请求](https://docs.microsoft.com/zh-cn/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0)。


### 9.3 媒体类型 Media-Type

### 9.4 HATEOAS

## 参考文献

- [Mircrosoft Docs](https://docs.microsoft.com/zh-cn/aspnet/core/web-api/?view=aspnetcore-5.0)
- [ABP Framework Docs](https://docs.abp.io/zh-Hans/abp/latest/Getting-Started?UI=MVC&DB=EF&Tiered=No)

