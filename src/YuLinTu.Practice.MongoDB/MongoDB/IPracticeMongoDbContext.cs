using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace YuLinTu.Practice.MongoDB
{
    [ConnectionStringName(PracticeDbProperties.ConnectionStringName)]
    public interface IPracticeMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
