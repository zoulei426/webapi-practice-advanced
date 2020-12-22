using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace YuLinTu.Practice.MongoDB
{
    [ConnectionStringName(PracticeDbProperties.ConnectionStringName)]
    public class PracticeMongoDbContext : AbpMongoDbContext, IPracticeMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigurePractice();
        }
    }
}