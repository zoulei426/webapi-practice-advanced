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

�ڱ��̳��У����ǽ��� Author �� Book ʵ��֮�佨�� 1 �� N �Ĺ�ϵ����һ��ʵ�� DDD ģʽ��

### 4.1 �����

������һ�̴̳��� Book ʵ���� BookType ö�٣����� Book ������ӣ�

```c#
public Guid AuthorId { get; set; }
```

- Ϊ����ѭ DDD ���ʵ���������ͨ�� ID ���������ۺϡ�


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

- Author �̳��� FullAuditedAggregateRoot<Guid>��ʹʵ����ɾ�����Ұ�������������ԡ�
- Author �Ĺ��캯���� internal �ģ�������ֻ�����������������ͬʱ��Ҫ��һ�� private / protected ���޲ι��캯������������ݿ��ȡ����ʱ�����л����衣
- FirstName, LastName ���Ե� setter ��˽�еģ����� ChangeName ������ά����
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

- �������ʹ�� Manager / Service ��Ϊ��׺��
- �ڹ��캯����ע�� IAuthorRepository �ӿڡ�
- ����������Ӧ�ò�ķ�����ԣ���Ҫ����ۺϵġ���һ��ҵ�񣬶�Ӧ�ò�ķ����������ͳ��ġ�

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

�޸� PracticeEntityFrameworkCoreModule.cs �ļ���

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
                // ���ȱʡ�ִ�
                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}
```

- ������������� [EntityFrameworkCore](https://docs.abp.io/zh-Hans/abp/latest/Entity-Framework-Core)

### 4.3 Ӧ�ò�

���� BookDto��

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

���� CreateUpdateBookDto��

```c#
using System;
using System.ComponentModel.DataAnnotations;

namespace YuLinTu.Practice.Books
{
    public class CreateUpdateBookDto
    {
        [Required]
        public Guid AuthorId { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        public BookType Type { get; set; } = BookType.Undefined;

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; } = DateTime.Now;

        [Required]
        public float Price { get; set; }
    }
}
```

���� IBookAppService��

```c#
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace YuLinTu.Practice.Books
{
    public interface IBookAppService :
          ICrudAppService<
              BookDto,
              Guid,
              PagedAndSortedResultRequestDto,
              CreateUpdateBookDto>
    {
    }
}
```

���� BookAppService ʵ�� IBookAppService������д��ȡ���ݵķ�����

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using YuLinTu.Practice.Authors;

namespace YuLinTu.Practice.Books
{
    public class BookAppService :
         CrudAppService<
             Book,
             BookDto,
             Guid,
             PagedAndSortedResultRequestDto,
             CreateUpdateBookDto>,
         IBookAppService
    {
        private readonly IAuthorRepository authorRepository;

        public BookAppService(IRepository<Book, Guid> repository, IAuthorRepository authorRepository)
            : base(repository)
        {
            this.authorRepository = authorRepository;
        }

        public async override Task<BookDto> GetAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            var query = from book in Repository
                        join author in authorRepository on book.AuthorId equals author.Id
                        where book.Id == id
                        select new { book, author };

            // ��ȡ book �� author
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Book), id);
            }

            var bookDto = ObjectMapper.Map<Book, BookDto>(queryResult.book);
            bookDto.AuthorName = queryResult.author.FirstName + queryResult.author.LastName;
            return bookDto;
        }

        public async override Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            await CheckGetListPolicyAsync();

            var query = from book in Repository
                        join author in authorRepository on book.AuthorId equals author.Id
                        orderby input.Sorting
                        select new { book, author };

            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var queryResult = await AsyncExecuter.ToListAsync(query);

            var bookDtos = queryResult.Select(x =>
            {
                var bookDto = ObjectMapper.Map<Book, BookDto>(x.book);
                bookDto.AuthorName = x.author.FirstName + x.author.LastName;
                return bookDto;
            }).ToList();

            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<BookDto>(
                totalCount,
                bookDtos
            );
        }
    }
}
```

���� AuthorDto��

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

- Ϊ��������� AutoMapper �����ã��������⽫ AuthorDto �� Name ��Ӧ Author �� FirstName, LastName������ Age ������ BirthDate ���㡣

���� CreateAuthorDto��

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

���� UpdateAuthorDto��

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

���� GetAuthorListDto �̳��� PagedAndSortedResultRequestDto������ӹ����ֶ� Filter��

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

���� IAuthorAppService��

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

���� AuthorAppService ʵ�� IAuthorAppService��

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


## �ο�����

- [Mircrosoft Docs](https://docs.microsoft.com/zh-cn/aspnet/core/web-api/?view=aspnetcore-5.0)
- [ABP Framework Docs](https://docs.abp.io/zh-Hans/abp/latest/Getting-Started?UI=MVC&DB=EF&Tiered=No)

