namespace YuLinTu.Practice.Filters
{
    public class ApiResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public StatusCode Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public void Success(T data, string message = null)
        {
            Status = StatusCode.Success;
            Message = message;
            Data = data;
        }
    }
}