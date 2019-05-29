using System.ComponentModel;

namespace WXKJ.Framework.Common
{
    /// <summary>
    /// 状态码
    /// </summary>
    public enum StateCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Ok = 1,
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 2
    }
    /// <summary>
    /// 模块代码枚举
    /// </summary>
    public enum EnumModuleCode
    {
        全局 = 0,
        //后台
        账号类型 = 10,
        系统配置 = 11,
        功能权限 = 12,
        用户 = 13,
        角色 = 14,
        人员 = 15,
        操作日志 = 16,
        账号 = 17,
        子系统 = 18,
        消息通知 = 19,
        文种=20,
        审批预设意见 = 21,
        表单 = 22,
        工作流 = 23,
        历史库 = 24,
    }
    /// <summary>
    /// 模块动作枚举
    /// </summary>
    public enum EnumActionCode
    {
        全局 = 0,
        登录 = 10,
        登出 = 11,
        密码 = 12,
        获取 = 13,
        新增 = 14,
        编辑 = 15,
        删除 = 16,
        排序 = 17,
        图表 = 18,
        列表 = 19,
        明细 = 20,
        实时 = 21,
        导出 = 22,
        初始 = 23,
        重算 = 24,
        重置 = 25,
        设置 = 26,
        更新 = 27,
        回复 = 28
    }
    /// <summary>
    /// 全局状态码
    /// </summary>
    public enum EnumStateCode
    {
        [Description("成功")]
        Ok = 1,
        [Description("失败")]
        Fail = 2,
        [Description("无操作权限")]
        无操作权限 = 3,
        [Description("参数不正确")]
        模型验证失败 = 4,
        [Description("未知错误")]
        未知错误 = 5,
    }
}
