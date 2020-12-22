using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace YuLinTu.Practice
{
    [DependsOn(
        typeof(PracticeDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class PracticeApplicationContractsModule : AbpModule
    {

    }
}
