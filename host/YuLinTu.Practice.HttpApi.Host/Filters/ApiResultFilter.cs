using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YuLinTu.Practice.Filters
{
    public class ApiResultFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is null)
            {
                var result = context.Result as ObjectResult;
                if (result is not null)
                {
                    // 封装结果
                    var apiResult = new ApiResult<object>();
                    apiResult.Success(result.Value);
                    context.Result = new ObjectResult(apiResult);
                }
            }
            base.OnActionExecuted(context);
        }
    }
}