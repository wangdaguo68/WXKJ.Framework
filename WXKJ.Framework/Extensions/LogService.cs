using NLog;
using System;
using System.Diagnostics;

namespace WXKJ.Framework.Extensions
{
    /// <summary>
    /// 日志服务
    /// </summary>
    public class LogService
    {

        private readonly Logger _Logger = LogManager.GetCurrentClassLogger(); //获得日志实;

        /// <summary>
        /// 正常日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void Info(string message, params object[] args)
        {
            var (className, methodName) = GetsStackTree();
            // create or edit
            LogManager.Configuration.Variables["LoggerClass"] = className;
            LogManager.Configuration.Variables["LoggerMethod"] = methodName;
            _Logger.Log(LogLevel.Info, message, args);
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionMessage"></param>
        /// <param name="args"></param>
        public void Error(Exception ex, string additionMessage, params object[] args)
        {
            var stackTree = GetsStackTree();
            // create or edit
            LogManager.Configuration.Variables["LoggerClass"] = stackTree.ClassName;
            LogManager.Configuration.Variables["LoggerMethod"] = stackTree.MethodName;
            _Logger.Log(LogLevel.Error, ex, additionMessage + "-----" + ex.StackTrace, args);
        }

        /// <summary>
        /// 严重错误日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionMessage"></param>
        /// <param name="args"></param>
        public void Fatal(Exception ex, string additionMessage, params object[] args)
        {
            var (className, methodName) = GetsStackTree();
            // create or edit
            LogManager.Configuration.Variables["LoggerClass"] = className;
            LogManager.Configuration.Variables["LoggerMethod"] = methodName;
            _Logger.Log(LogLevel.Fatal, ex, additionMessage + "-----" + ex, args);
        }

        private static (string ClassName, string MethodName) GetsStackTree()
        {
            var trace = new StackTrace();
            var frame = trace.GetFrame(2);//1代表上级，2代表上上级，以此类推
            var method = frame.GetMethod();
            var className = method.ReflectedType.Name;
            Console.Write(@"ClassName:" + className + @"
MethodName:" + method.Name);
            return (className, method.Name);
        }
    }
}
