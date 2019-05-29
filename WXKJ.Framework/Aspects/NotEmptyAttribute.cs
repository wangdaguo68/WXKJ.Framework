using AspectCore.DynamicProxy.Parameters;
using System;
using System.Threading.Tasks;
using WXKJ.Framework.Aspects.Base;
using WXKJ.Framework.Extensions;
using WXKJ.Framework.Util.Extensions;

namespace WXKJ.Framework.Aspects
{
    /// <summary>
    /// 验证不能为空
    /// </summary>
    public class NotEmptyAttribute : ParameterInterceptorBase {
        /// <summary>
        /// 执行
        /// </summary>
        public override Task Invoke( ParameterAspectContext context, ParameterAspectDelegate next ) {
            if( string.IsNullOrWhiteSpace( context.Parameter.Value.SafeString() ) )
                throw new ArgumentNullException( context.Parameter.Name );
            return next( context );
        }
    }
}
