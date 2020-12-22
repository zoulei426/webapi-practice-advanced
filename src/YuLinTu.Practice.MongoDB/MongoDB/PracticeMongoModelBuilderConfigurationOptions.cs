using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace YuLinTu.Practice.MongoDB
{
    public class PracticeMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public PracticeMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}