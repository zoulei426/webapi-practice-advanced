using YuLinTu.Practice.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace YuLinTu.Practice
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(PracticeEntityFrameworkCoreTestModule)
        )]
    public class PracticeDomainTestModule : AbpModule
    {
        
    }
}
