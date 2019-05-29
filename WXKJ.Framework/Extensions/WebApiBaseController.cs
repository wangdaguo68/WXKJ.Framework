using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXKJ.Framework.Common;

namespace WXKJ.Framework.Extensions
{
    [ApiController]
    public class WebApiBaseController : Controller
    {
        //public virtual ISession Session => Security.Sessions.Session.Instance;
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual IActionResult Success(dynamic data = null)
        {
            return new ResponseResult(true, EnumStateCode.Ok.EnumDescription(), ((int)EnumStateCode.Ok).ToString(), null, data);
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="message"></param>       
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual IActionResult Success(string message, dynamic data = null)
        {
            return new ResponseResult(true, message, ((int)EnumStateCode.Ok).ToString(), null, data);
        }
        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        protected IActionResult Fail(string message, string code = "")
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Log(LogLevel.Error, message);
            return new ResponseResult(false, message, code, message);
        }
        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected IActionResult Fail(EnumStateCode code)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Log(LogLevel.Error, code.EnumDescription());
            return new ResponseResult(false, code.EnumDescription(), ((int)code).ToString(), ModelState.ModelStateMessage());
        }
        /// <summary>
        /// 模型验证
        /// </summary>
        protected virtual string ModelValidErrorMessage()
        {
            var sb = new StringBuilder();
            var Keys = ModelState.Keys.ToList();
            foreach (var key in Keys)
            {
                var errors = ModelState[key].Errors.ToList();
                foreach (var error in errors)
                {
                    sb.Append(error.ErrorMessage + ";");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成有序GUID
        /// </summary>
        /// <returns></returns>
        protected virtual Guid NewGuid()
        {
            return SequentialGuidGenerator.Create();
        }
        #region 所有请求入口参数检测

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var pardDics = context.ActionArguments;

            var paraStr = JsonConvert.SerializeObject(pardDics);

            using (var scope = DI.ServiceProvider.CreateScope())
            {
                var _logService = scope.ServiceProvider.GetRequiredService<LogService>();
                var controllerName = context.Controller.ToString();
                var actionName = context.ActionDescriptor.DisplayName;

                //轮询待收数量的不记录
                if (!actionName.Contains("GetToBeSignedNum"))
                {
                    Task.Run(() =>
                    {
                        _logService.Info($"时间:{DateTime.Now},关于控制器{controllerName}请求{actionName}的请求参数为{paraStr}");
                    });
                }


            }
        }
        #endregion
    }
}