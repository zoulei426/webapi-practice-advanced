using Volo.Abp;

namespace YuLinTu.Practice.Authors
{
    public class AuthorAlreadyExistsException : BusinessException
    {
        public AuthorAlreadyExistsException(string name)
            : base(PracticeErrorCodes.AuthorAlreadyExists)
        {
            WithData("name", name);
        }
    }
}