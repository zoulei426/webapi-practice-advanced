using YuLinTu.Practice.Localization;
using Volo.Abp.Application.Services;

namespace YuLinTu.Practice
{
    public abstract class PracticeAppService : ApplicationService
    {
        protected PracticeAppService()
        {
            LocalizationResource = typeof(PracticeResource);
            ObjectMapperContext = typeof(PracticeApplicationModule);
        }
    }
}
