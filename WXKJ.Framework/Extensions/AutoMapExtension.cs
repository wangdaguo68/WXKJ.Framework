using AutoMapper;

namespace WXKJ.Framework.Util.Extensions
{
    /// <summary>
    /// AutoMapper扩展
    /// </summary>
    public static class AutoMapExtension
    {
        /// <summary>
        /// 映射到目标对象
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination MapTo<TDestination>(this object source)
        {
            return Mapper.Map<TDestination>(source);
        }
        /// <summary>
        /// 从一个类型映射到目标类型
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }
    }
}
