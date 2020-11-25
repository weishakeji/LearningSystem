using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Common;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 通知公告
    /// </summary>
    [HttpGet]
    public class Notice : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 通过机构id获取机构信息
        /// </summary>
        /// <param name="id">机构id</param>
        /// <returns>机构实体</returns>
        public Song.Entities.Notice ForID(int id)
        {
            return Business.Do<INotice>().NoticeSingle(id);
        }
        /// <summary>
        /// 修改通知信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool Modify(Song.Entities.Notice entity)
        {
            Song.Entities.Notice old = Business.Do<INotice>().NoticeSingle(entity.No_Id);
            if (old == null) throw new Exception("对象不存在！");

            old.Copy<Song.Entities.Notice>(entity);
            Business.Do<INotice>().Save(old);
            return true;
        }
        /// <summary>
        /// 添加通知信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool Add(Song.Entities.Notice entity)
        {
            try
            {
                Business.Do<INotice>().Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult Pager(int size, int index)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int count = 0;
            Song.Entities.Notice[] eas = Business.Do<INotice>().GetPager(org.Org_ID, size, index, out count);           
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        public ListResult OpenItems(string forpage)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.Notice[] eas = Business.Do<INotice>().List(org.Org_ID, 2, forpage, DateTime.Now, true, -1);
            ListResult result = new ListResult(eas);
            result.Index = 1;
            result.Size = eas.Length;
            result.Total = eas.Length;
            return result;
        }
    }
}
