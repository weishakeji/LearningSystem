using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 域名管理
    /// </summary>
    [HttpGet]
    public class Domain : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 添加二级域名的保留项
        /// </summary>
        /// <param name="entity">业务实体</param>
        [HttpPost]
        [SuperAdmin]
        public bool Add(LimitDomain entity)
        {
            if (Business.Do<ILimitDomain>().DomainIsExist(entity))
            {
                throw new Exception("当前域名已经存在！");
            }
            else
            {
                Business.Do<ILimitDomain>().DomainAdd(entity);
                return true;
            }
        }
        /// <summary>
        /// 修改二级域名的保留项
        /// </summary>
        /// <param name="entity">业务实体</param>
        [HttpPost]
        [SuperAdmin]
        public bool Modify(LimitDomain entity)
        {
            if (Business.Do<ILimitDomain>().DomainIsExist(entity))
            {
                throw new Exception("当前域名已经存在！");
            }
            else
            {
                Song.Entities.LimitDomain old = Business.Do<ILimitDomain>().DomainSingle(entity.LD_ID);
                if (old == null) throw new Exception("Not found entity for Accounts！");
                old.Copy<Song.Entities.LimitDomain>(entity);
                Business.Do<ILimitDomain>().DomainSave(old);
                return true;
            }
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        [HttpDelete]
        [SuperAdmin]
        public int Delete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                { 
                    Business.Do<ILimitDomain>().DomainDelete(idval);
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;            
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        public LimitDomain ForID(int id)
        {
            return Business.Do<ILimitDomain>().DomainSingle(id);
        }
        /// <summary>
        /// 获取指定数量的实体
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public LimitDomain[] Count(int count)
        {
            return Business.Do<ILimitDomain>().DomainCount(true, count);
        }

        /// <summary>
        /// 分页获取域名
        /// </summary>
        /// <param name="search"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult Pager(string search, int size, int index)
        {
            int count = 0;
            Song.Entities.LimitDomain[] eas = null;           
            eas = Business.Do<ILimitDomain>().DomainPager(null, search, size, index, out count);
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
    }
}
