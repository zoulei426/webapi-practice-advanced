using YuLinTu.Practice.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace YuLinTu.Practice
{
    public abstract class PracticeController : AbpController
    {
        protected PracticeController()
        {
            LocalizationResource = typeof(PracticeResource);
        }
    }
}
