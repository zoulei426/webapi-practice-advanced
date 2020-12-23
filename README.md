<!--
 * @Description: ���� ABP vNext �� Web API ���׿����̳�
 * @Author: zoulei
 * @Date: 2020-12-18 13:26:04
 * @LastEditors: zoulei
 * @LastEditTime: 2020-12-23 11:20:55
-->

# ���� ABP vNext �� Web API ���׿����̳�

## Ŀ¼

<!-- TOC -->

- [���� ABP vNext �� Web API ���׿����̳�](#����-abp-vnext-��-web-api-���׿����̳�)
  - [Ŀ¼](#Ŀ¼)
  - [1 ����](#1-����)
  - [2 ����](#2-����)
    - [2.1 �Ⱦ�����](#21-�Ⱦ�����)
    - [2.2 ��װ ABP CLI](#22-��װ-abp-cli)
    - [2.3 ��װ Redis](#23-��װ-redis)
  - [3 ���ٴ�����Ŀ](#3-���ٴ�����Ŀ)
    - [3.1 �޸� IdentityServer ��Ŀ](#31-�޸�-identityserver-��Ŀ)
    - [3.2 �޸� Api Host ��Ŀ](#32-�޸�-api-host-��Ŀ)
  - [4 ҵ�����](#4-ҵ�����)
    - [4.1 �����](#41-�����)
    - [4.2 ���ݿ⼯��](#42-���ݿ⼯��)
    - [4.3 Ӧ�ò�](#43-Ӧ�ò�)
  - [�ο�����](#�ο�����)

<!-- /TOC -->

## 1 ����

���̳̽���ʹ�� ABP vNext ���� Web API �Ľ���֪ʶ����������Ŀʵս��

�ڱ��̳��У��㽫�˽⣺
- ʹ�� ABP CLI ���ٴ��� Web API ��Ŀ
- DDD �����������
- ʹ�� IdentityServer4, AutoMapper...

˭�ʺ��Ķ����̳̣�
- ����ɡ����� ABP vNext �� Web API �����̡̳�

## 2 ����

### 2.1 �Ⱦ�����

- .NET 5.0 SDK ����߰汾
- ���С�ASP.NET �� Web �������������ص� Visual Studio 2019 16.8 ����߰汾
- API CLI 4.0 ����߰汾
- Postgresql 9.2 ����߰汾
- Redis

### 2.2 ��װ ABP CLI

��װ ABP CLI

```cmd
dotnet tool install -g Volo.Abp.Cli
```

�����������°汾��

```cmd
dotnet tool update -g Volo.Abp.Cli
```

### 2.3 ��װ Redis

## 3 ���ٴ�����Ŀ

ʹ��ABP CLI�� new ���������Ŀ��

```cmd
abp new YuLinTu.Practice -t module --no-ui
```

������������󽫻ᴴ��һ��ģ�黯�Ľ�������ṹ��

- src �ļ��а�������DDDԭ��ֲ��ʵ��ģ��
- test �ļ��а�����Ԫ�ͼ��ɲ���
- host �ļ��а������в�ͬ���õ�Ӧ�ó���������ʾ��Ӧ�ó���������й�ģ�顣

### 3.1 �޸� IdentityServer ��Ŀ

�� YuLinTu.Practice.IdentityServer ��Ϊ������ڳ�����������̨ѡ�����ĿΪĬ����Ŀ��

��װ ABP �����

```PM
Install-Package Volo.Abp.EntityFrameworkCore.PostgreSql
```

�޸� appsettings.json �ļ��е� ConnectionStrings �ڵ㣺

```json
"ConnectionStrings": {
    "Default": "Server=127.0.0.1;Port=5432;Database=practice_main;User Id=postgres;Password=123456;"
  }
```

�޸� Properties Ŀ¼�µ� launchSettings.json �ļ���

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

�޸� PracticeIdentityServerModule.cs �ļ���

> �� DependsOn ��������� AbpEntityFrameworkCorePostgreSqlModule ģ�顣  
> �޸� ConfigureServices �����е� AbpDbContextOptions ���ã�
> ```c#
> Configure<AbpDbContextOptions>(options =>
> {
>     //options.UseSqlServer();
>     options.UseNpgsql();
> });
> ```

�޸� EntityFrameworkCore Ŀ¼�µ� IdentityServerHostMigrationsDbContextFactory.cs �ļ��е� CreateDbContext������

```c#
public IdentityServerHostMigrationsDbContext CreateDbContext(string[] args)
{
    var configuration = BuildConfiguration();
    var builder = new DbContextOptionsBuilder<IdentityServerHostMigrationsDbContext>()
        .UseNpgsql(configuration.GetConnectionString("Default"));
    return new IdentityServerHostMigrationsDbContext(builder.Options);
}
```

ɾ�� Migrations Ŀ¼��

�ڳ�����������̨�������������������Ǩ���ļ���

```cmd
Add-Migration "Init"
```

��������������������ݿ⣺

```cmd
Update-Database
```

���� Redis��

����Ŀ��·����ʹ�� dotnet run ������Ŀ��

### 3.2 �޸� Api Host ��Ŀ

�� YuLinTu.Practice.HttpApi.Host ��Ϊ������ڳ�����������̨ѡ�����ĿΪĬ����Ŀ��

��װ ABP �����

```PM
Install-Package Volo.Abp.EntityFrameworkCore.PostgreSql
```

�޸� appsettings.json �ļ��е� ConnectionStrings �ڵ㣺

```json
"ConnectionStrings": {
    "Default": "Server=127.0.0.1;Port=5432;Database=practice_main;User Id=postgres;Password=123456;",
    "Practice": "Server=127.0.0.1;Port=5432;Database=practice_module;User Id=postgres;Password=123456;"
  }
```

�޸� Properties Ŀ¼�µ� launchSettings.json �ļ���

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

�޸� PracticeHttpApiHostModule.cs �ļ���

> �� DependsOn ��������� AbpEntityFrameworkCorePostgreSqlModule ģ�顣  
> �޸� ConfigureServices �����е� AbpDbContextOptions ���ã�
> ```c#
> Configure<AbpDbContextOptions>(options =>
> {
>     //options.UseSqlServer();
>     options.UseNpgsql();
> });
> ```

�޸� EntityFrameworkCore Ŀ¼�µ� IdentityServerHostMigrationsDbContextFactory.cs �ļ��е� CreateDbContext������

```c#
public PracticeHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
{
    var configuration = BuildConfiguration();
    var builder = new DbContextOptionsBuilder<PracticeHttpApiHostMigrationsDbContext>()
        .UseNpgsql(configuration.GetConnectionString("Practice"));
    return new PracticeHttpApiHostMigrationsDbContext(builder.Options);
}
```

���д���Ŀ��

## 4 ҵ�����

### 4.1 �����

������һ�̴̳��� Book ʵ���� BookType ö�٣������������� Author ʵ�壬��һ��ʵ�� DDD ģʽ��


�� YuLinTu.Practice.Domain �д��� Author ʵ�壺

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

- Author �̳��� FullAuditedAggregateRoot<Guid>��ʹʵ����ɾ�����Ұ�������������ԡ�
- Author �Ĺ��캯���� internal �ģ�������ֻ�����������������ͬʱ��Ҫ��һ�� private / protected ���޲ι��캯������������ݿ��ȡ����ʱ�����л����衣
- Name ���Ե� setter ��˽�еģ����� ChangeName ������ά����
- �������������[ʵ��](https://docs.abp.io/zh-Hans/abp/latest/Entities)��

�� YuLinTu.Practice.Domain.Shared �д������� AuthorConsts��

```c#
namespace YuLinTu.Practice.Authors
{
    public static class AuthorConsts
    {
        public const int MaxNameLength = 64;
    }
}
```

�� YuLinTu.Practice.Domain �д����ִ��ӿ� IAuthorRepository��

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

�� YuLinTu.Practice.Domain �д�������� AuthorManager��

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

- �������ʹ�� Manager / Service ��Ϊ��׺��
- �ڹ��캯����ע�� IAuthorRepository �ӿڡ�

�� YuLinTu.Practice.Domain �д���ҵ���쳣 AuthorAlreadyExistsException��

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

- AuthorAlreadyExistsException �̳��� BusinessException��BusinessException ��������ҵ������쳣�����Ը����׵�ʵ�ֱ��ػ�����

�޸� YuLinTu.Practice.Domain.Shared �е� PracticeErrorCodes.cs �ļ���

```c#
namespace YuLinTu.Practice
{
    public static class PracticeErrorCodes
    {
        public const string AuthorAlreadyExists = "Practice:00001";
    }
}
```

�� YuLinTu.Practice.Domain.Shared ��Ŀ�е� Localization/Practice/zh-Hans.json �ļ�������ӣ�

```json
"Practice:00001": "�Ѵ�����ͬ���������ߣ�{name}"
```

### 4.2 ���ݿ⼯��

�� YuLinTu.Practice.EntityFrameworkCore ��Ŀ�е� PracticeDbContext.cs �ļ�������� Book �� Author ���ݼ���

```c#
public DbSet<Book> Books { get; set; }

public DbSet<Author> Authors { get; set; }
```

�� PracticeDbContextModelCreatingExtensions.cs �ļ������ Book �� Author �����ݿ�ӳ�䣺

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

            // �����ݿ��ֶ�ȫСд��ͨ���»��߷ָ�
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

���� IAuthorRepository ��ʵ���� EfCoreAuthorRepository��

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

- ������������� [EntityFrameworkCore](https://docs.abp.io/zh-Hans/abp/latest/Entity-Framework-Core)

### 4.3 Ӧ�ò�

������һ�̴̳��� BookDto, CreateUpdateBookDto, IBookAppService, BookAppService��



## �ο�����

- [Mircrosoft Docs](https://docs.microsoft.com/zh-cn/aspnet/core/web-api/?view=aspnetcore-5.0)
- [ABP Framework Docs](https://docs.abp.io/zh-Hans/abp/latest/Getting-Started?UI=MVC&DB=EF&Tiered=No)

