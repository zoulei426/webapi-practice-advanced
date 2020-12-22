using Volo.Abp.Reflection;

namespace YuLinTu.Practice.Permissions
{
    public class PracticePermissions
    {
        public const string GroupName = "Practice";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(PracticePermissions));
        }
    }
}