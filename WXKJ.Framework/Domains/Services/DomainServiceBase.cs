using WXKJ.Framework.Logs;
using WXKJ.Framework.Logs.Core;
using WXKJ.Framework.Sessions;

namespace WXKJ.Framework.Domains.Services {
    /// <summary>
    /// 领域服务
    /// </summary>
    public abstract class DomainServiceBase : IDomainService {
        /// <summary>
        /// 初始化领域服务
        /// </summary>
        protected DomainServiceBase() {
            Log = NullLog.Instance;
            Session = NullSession.Instance;
        }

        /// <summary>
        /// 日志
        /// </summary>
        public ILog Log { get; set; }

        /// <summary>
        /// 用户会话
        /// </summary>
        public ISession Session { get; set; }
    }
}
