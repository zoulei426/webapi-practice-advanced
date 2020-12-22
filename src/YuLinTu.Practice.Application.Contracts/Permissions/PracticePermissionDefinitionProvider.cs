using YuLinTu.Practice.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace YuLinTu.Practice.Permissions
{
    public class PracticePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(PracticePermissions.GroupName, L("Permission:Practice"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PracticeResource>(name);
        }
    }
}