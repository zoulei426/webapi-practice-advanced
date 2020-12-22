using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace YuLinTu.Practice
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(PracticeDomainSharedModule)
    )]
    public class PracticeDomainModule : AbpModule
    {

    }
}
