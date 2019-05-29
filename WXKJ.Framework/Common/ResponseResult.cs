using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Threading.Tasks;

namespace WXKJ.Framework.Common
{
    public class ResponseResult:JsonResult
    {
        private readonly bool _success;
        private readonly object _data;
        private readonly string _code;
        private readonly string _message;
        private readonly string _error;
        public enum ErrorCode
        {
            /// <summary>  
            /// 成功  
            /// </summary>  
            [Description("成功")]
            Success = 1000,

            /// <summary>  
            /// 服务器异常  
            /// </summary>  
            [Description("服务器异常")]
            Exception = 1100,

            /// <summary>  
            /// 身份异常
            /// </summary>  
            [Description("身份异常")]
            Unauthorized = 1200,

            /// <summary>  
            /// 系统权限不足
            /// </summary>  
            [Description("权限不足")]
            SysUnauthorized = 1300,

            /// <summary>  
            /// 参数不正确
            /// </summary>  
            [Description("参数不正确")]
            IllegalArgument = 1400,

            /// <summary>
            /// 自定义异常信息
            /// </summary>
            [Description("自定义异常信息")]
            CustomException = 1500,
        }

        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="success"></param>
        /// <param name="message">消息</param>
        /// <param name="error"></param>
        /// <param name="data">数据</param>
        public ResponseResult(bool success, string message, string code,string error, dynamic data = null) : base(null)
        {
            _success = success;
            _code = code;
            _message = message;
            _data = data;
            _error = error;
        }
        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="success"></param>
        /// <param name="message">消息</param>
        /// <param name="error"></param>
        /// <param name="data">数据</param>
        public ResponseResult(bool success, string message, EnumStateCode code, string error, dynamic data = null) : base(null)
        {
            _success = success;
            _code = ((int)code).ToString();
            _message = message;
            _data = data;
            _error = error;
        }
        public ResponseResult(EnumStateCode code, string error) : base(null)
        {
            _success = false;
            _code = ((int)code).ToString();
            _message = code.EnumDescription();
            _data = null;
            _error = error;
        }
        /// <summary>
        /// 执行结果
        /// </summary>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            Value = new
            {
                Success = _success,
                Data = _data,
                Code = _code,
                Message = _message,
                Error = _error,
            };
            return base.ExecuteResultAsync(context);
        }
    }

}
