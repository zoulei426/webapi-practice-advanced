using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace YuLinTu.Practice.MongoDB
{
    public static class PracticeMongoDbContextExtensions
    {
        public static void ConfigurePractice(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new PracticeMongoModelBuilderConfigurationOptions(
                PracticeDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}