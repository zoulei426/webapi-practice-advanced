<!--
 * @Description: ���� ABP vNext �� Web API ���׿����̳�
 * @Author: zoulei
 * @Date: 2020-12-18 13:26:04
 * @LastEditors: zoulei
 * @LastEditTime: 2020-12-22 14:55:31
-->

# ���� ABP vNext �� Web API ���׿����̳�

## Ŀ¼



## 1 ����

���̳̽���ʹ�� ABP vNext ���� Web API �Ľ���֪ʶ����������Ŀʵս��

�ڱ��̳��У��㽫�˽⣺
- ʹ�� ABP CLI ���ٴ��� Web API ��Ŀ
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
      "applicationUrl": "https://localhost:44340",
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

## �ο�����

- [Mircrosoft Docs](https://docs.microsoft.com/zh-cn/aspnet/core/web-api/?view=aspnetcore-5.0)
- [ABP Framework Docs](https://docs.abp.io/zh-Hans/abp/latest/Getting-Started?UI=MVC&DB=EF&Tiered=No)