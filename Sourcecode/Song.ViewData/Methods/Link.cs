using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Web;
using System.IO;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 友情链接的管理
    /// </summary>
    [HttpPut, HttpGet]
    public class Link : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        public static string VirPath = WeiSha.Core.Upload.Get["Links"].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get["Links"].Physics;

        #region 链接的分类
        /// <summary>
        /// 获取链接分类的单一实体
        /// </summary>
        /// <param name="id">链接分类的id</param>
        /// <returns></returns>
        public Song.Entities.LinksSort SortForID(int id)
        {
            return Business.Do<ILinks>().SortSingle(id);
        }
        /// <summary>
        /// 添加友情链接分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool SortAdd(Song.Entities.LinksSort entity)
        {
            try
            {               
                Business.Do<ILinks>().SortAdd(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改友情链接的分类
        /// </summary>
        /// <param name="entity">友情链接的分类</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool SortModify(LinksSort entity)
        {
            Song.Entities.LinksSort old = Business.Do<ILinks>().SortSingle(entity.Ls_Id);
            if (old == null) throw new Exception("Not found entity for Accounts！");

            old.Copy<Song.Entities.LinksSort>(entity);
            Business.Do<ILinks>().SortSave(old);
            return true;
        }
        /// <summary>
        /// 删除链接分类
        /// </summary>
        /// <param name="id">账户id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete]
        public int SortDelete(string id)
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
                    Business.Do<ILinks>().SortDelete(idval);
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
        /// 分页获取友情链接的分类
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="use">是否显示启用，默认为null，即显示所有</param>
        /// <param name="show">是否在前端显示，默认为null，即显示所有</param>
        /// <param name="search">检索字符</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult SortPager(int orgid, bool? use, bool? show, string search, int size, int index)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            //总记录数
            int count = 0;
            Song.Entities.LinksSort[] arr = Business.Do<ILinks>().SortPager(orgid, use, show, search, size, index, out count);
            ListResult result = new ListResult(arr);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 获取指定数量的链接分类
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="use">是否启用，null为所有</param>
        /// <param name="show">是否显示，null为所有</param>
        /// <param name="search"></param>
        /// <param name="count">指定数量的结果，少于等于零取所有</param>
        /// <returns></returns>
        [HttpPost,HttpGet]
        public Song.Entities.LinksSort[] SortCount(int orgid, bool? use, bool? show, string search,int count)
        {
            return Business.Do<ILinks>().SortCount(orgid, use, show, count);
        }
        /// <summary>
        /// 修改链接分类的状态
        /// </summary>
        /// <param name="id">链接分类的id</param>
        /// <param name="use">是否启用</param>
        /// <param name="show">是否显示</param>
        /// <returns></returns>
        public bool SortModifyState(int id, bool use, bool show)
        {
            try
            {
                Business.Do<ILinks>().SortUpdate(id,
                    new WeiSha.Data.Field[] {
                        Song.Entities.LinksSort._.Ls_IsUse,
                        Song.Entities.LinksSort._.Ls_IsShow },
                    new object[] { use, show });
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更改链接分类的排序
        /// </summary>
        /// <param name="items">链接分类的数组</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool SortUpdateTaxis(Song.Entities.LinksSort[] items)
        {
            try
            {
                Business.Do<ILinks>().SortUpdateTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取链接分类下的链接个数
        /// </summary>
        /// <param name="sortid">链接分类id</param>
        /// <param name="use">是启包括启用与未启用，为null时取所有</param>
        /// <returns></returns>
        public int SortOfCount(int sortid, bool? use)
        {
            return Business.Do<ILinks>().SortOfCount(sortid, use, null); 
        }
        #endregion

        #region 友情链接
        /// <summary>
        /// 获取链接的单一实体
        /// </summary>
        /// <param name="id">链接的id</param>
        /// <returns></returns>
        public Song.Entities.Links ForID(int id)
        {
            return _tran(Business.Do<ILinks>().LinksSingle(id));
        }
        /// <summary>
        /// 添加友情
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        public Song.Entities.Links Add(Song.Entities.Links entity)
        {
            string filename = string.Empty, smallfile = string.Empty;
            try
            {
                //只保存第一张图片
                foreach (string key in this.Files)
                {
                    HttpPostedFileBase file = this.Files[key];
                    filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                    file.SaveAs(PhyPath + filename);
                    //生成缩略图
                    smallfile = WeiSha.Core.Images.Name.ToSmall(filename);
                    WeiSha.Core.Images.FileTo.Thumbnail(PhyPath + filename, PhyPath + smallfile, 100, 100, 0);
                    break;
                }
                entity.Lk_Logo = filename;
                entity.Lk_LogoSmall = smallfile;

                Business.Do<ILinks>().LinksAdd(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改友情的分类
        /// </summary>
        /// <param name="entity">友情链接的实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        public Song.Entities.Links Modify(Song.Entities.Links entity)
        {
            string filename = string.Empty, smallfile = string.Empty;
            try
            {
                Song.Entities.Links old = Business.Do<ILinks>().LinksSingle(entity.Lk_Id);
                if (old == null) throw new Exception("Not found entity for Links！");
                //如果有上传文件
                if (this.Files.Count > 0)
                {
                    //只保存第一张图片
                    foreach (string key in this.Files)
                    {
                        HttpPostedFileBase file = this.Files[key];
                        filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                        file.SaveAs(PhyPath + filename);
                        //生成缩略图
                        smallfile = WeiSha.Core.Images.Name.ToSmall(filename);
                        WeiSha.Core.Images.FileTo.Thumbnail(PhyPath + filename, PhyPath + smallfile, 100, 100, 2);
                        break;
                    }
                    entity.Lk_Logo = filename;
                    entity.Lk_LogoSmall = smallfile;

                    if (!string.IsNullOrWhiteSpace(old.Lk_Logo))                  
                        WeiSha.Core.Upload.Get["Links"].DeleteFile(old.Lk_Logo); 
                }
                else
                {
                    //如果没有上传图片，且新对象没有图片，则删除旧图
                    if (string.IsNullOrWhiteSpace(entity.Lk_Logo) && !string.IsNullOrWhiteSpace(old.Lk_Logo))
                    {
                        WeiSha.Core.Upload.Get["Links"].DeleteFile(old.Lk_Logo);
                    }
                }

                old.Copy<Song.Entities.Links>(entity);
                Business.Do<ILinks>().LinksSave(old);
                return old;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除链接
        /// </summary>
        /// <param name="id">账户id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete]
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
                    Business.Do<ILinks>().LinksDelete(idval);
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
        /// 分页获取友情链接
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sortid">友情链接分类的id</param>
        /// <param name="use">是否显示启用，默认为null，即显示所有</param>
        /// <param name="show">是否在前端显示，默认为null，即显示所有</param>
        /// <param name="name">按链接名称检索字符</param>
        /// <param name="link">按链接地址检索</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult Pager(int orgid, int sortid, bool? use, bool? show, string name, string link, int size, int index)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            //总记录数
            int count = 0;
            Song.Entities.Links[] arr = Business.Do<ILinks>().GetLinksPager(orgid, sortid, use, show, name, link, size, index, out count);
            foreach (Song.Entities.Links l in arr)
                _tran(l);
            ListResult result = new ListResult(arr);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 获取指定数量的链接
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sortid">友情链接分类的id</param>
        /// <param name="use">是否启用，null为所有</param>
        /// <param name="show">是否显示，null为所有</param>
        /// <param name="search"></param>
        /// <param name="count">指定数量的结果，少于等于零取所有</param>
        /// <returns></returns>
        [HttpPost, HttpGet]
        public Song.Entities.Links[] Count(int orgid,int sortid, bool? use, bool? show, string search, int count)
        {
            Song.Entities.Links[] entities = Business.Do<ILinks>().GetLinks(orgid, sortid, show, use, count);
            foreach(Song.Entities.Links l in entities)          
                _tran(l);           
            return entities;
        }
        #endregion

        #region 私有方法，处理实体信息
        /// <summary>
        /// 处理学员信息，密码清空、头像转为全路径，并生成clone对象
        /// </summary>
        /// <param name="entity">实体的clone对象</param>
        /// <returns></returns>
        private Song.Entities.Links _tran(Song.Entities.Links entity)
        {
            if (entity == null) return entity;         
            entity.Lk_Logo = System.IO.File.Exists(PhyPath + entity.Lk_Logo) ? VirPath + entity.Lk_Logo : "";
            entity.Lk_LogoSmall = System.IO.File.Exists(PhyPath + entity.Lk_LogoSmall) ? VirPath + entity.Lk_LogoSmall : "";
            return entity;
        }
        #endregion
    }
}
