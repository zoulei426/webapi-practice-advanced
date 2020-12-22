using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace YuLinTu.Practice
{
    [DependsOn(
        typeof(PracticeHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class PracticeConsoleApiClientModule : AbpModule
    {
        
    }
}
