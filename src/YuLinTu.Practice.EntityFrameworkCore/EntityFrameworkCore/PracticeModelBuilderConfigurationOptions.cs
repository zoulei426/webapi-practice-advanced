using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace YuLinTu.Practice.EntityFrameworkCore
{
    public class PracticeModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public PracticeModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}