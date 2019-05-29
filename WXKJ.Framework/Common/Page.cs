using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WXKJ.Framework.Common
{
    /// <summary>
    /// 分页
    /// </summary>
    public class BasePage
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Page { set; get; }

        /// <summary>
        /// 每页显示记录数
        /// </summary>
        [Range(0, int.MaxValue)]
        public int? PageSize { set; get; }
    }
    /// <summary>
    /// 分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Page<T>
    {
        public int PageIndex { get; set; } //当前页码
        public int PageSize = Utils.PageSize; //每页显示记录数
        public int PageCount { get; set; } //总页数
        public int DataCount { get; set; } //总记录数
        public bool HasPreviousPage //是否有上一页
            => PageIndex > 1;

        public bool HasNextPage //是否有下一页
            => PageIndex < PageCount;

        public IEnumerable<T> DataList { get; set; }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="basePage"></param>
        public Page(IEnumerable<T> dataList, BasePage basePage)
        {
            if (basePage.PageSize != null && basePage.PageSize > 0)
            {
                PageSize = (int)basePage.PageSize;
            }
            PageIndex = basePage.Page;
            DataCount = dataList?.Count() ?? 0;
            PageCount = (int)Math.Ceiling((decimal)DataCount / PageSize);
            DataList = dataList?.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="basePage"></param>
        public Page(IQueryable<T> dataList, BasePage basePage)
        {
            if (basePage.PageSize != null && basePage.PageSize > 0)
            {
                PageSize = (int)basePage.PageSize;
            }
            PageIndex = basePage.Page;
            DataCount = dataList?.Count() ?? 0;
            PageCount = (int)Math.Ceiling((decimal)DataCount / PageSize);
            DataList = dataList?.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        public Page(IQueryable<T> dataList, int pageSize, int pageIndex)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            DataCount = dataList?.Count() ?? 0;
            PageCount = (int)Math.Ceiling((decimal)DataCount / PageSize);
            DataList = dataList?.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="dataCount"></param>
        public Page(IEnumerable<T> dataList, int pageSize, int pageIndex, int dataCount)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            DataCount = dataCount;
            PageCount = (int)Math.Ceiling((decimal)DataCount / PageSize);
            DataList = dataList.ToList();
        }
    }

}
