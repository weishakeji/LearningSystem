using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 轮换图片
    /// </summary>
    [HttpPut, HttpGet]
    public class Showpic : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        public static string VirPath = WeiSha.Core.Upload.Get["ShowPic"].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get["ShowPic"].Physics;
        /// <summary>
        /// 手机端轮换图片
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet]        
        public Song.Entities.ShowPicture[] Mobi(int orgid)
        {
            Song.Entities.ShowPicture[] shp = Business.Do<IStyle>().ShowPicAll(true, "mobi", orgid);
            foreach (Song.Entities.ShowPicture s in shp)
            {
                s.Shp_File = System.IO.File.Exists(PhyPath + s.Shp_File) ? VirPath + s.Shp_File : "";              

                //s.Shp_File = WeiSha.Core.Images.FileTo.ToBase64Html(PhyPath + s.Shp_File);
            }
            return shp;
        }
        /// <summary>
        /// 电脑端轮换图片
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet]
        [Cache]
        public Song.Entities.ShowPicture[] Web(int orgid)
        {
            Song.Entities.ShowPicture[] shp = Business.Do<IStyle>().ShowPicAll(true, "web", orgid);
            foreach (Song.Entities.ShowPicture s in shp)
            {
                s.Shp_File = System.IO.File.Exists(PhyPath + s.Shp_File) ? VirPath + s.Shp_File : "";

                //s.Shp_File = WeiSha.Core.Images.FileTo.ToBase64Html(PhyPath + s.Shp_File);
            }
            return shp;
        }
        /// <summary>
        /// 所有轮换图片
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="site">所属站点</param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.ShowPicture[] All(int orgid, string site)
        {
            Song.Entities.ShowPicture[] shp = Business.Do<IStyle>().ShowPicAll(null, site, orgid);
            foreach (Song.Entities.ShowPicture s in shp)
            {
                s.Shp_File = System.IO.File.Exists(PhyPath + s.Shp_File) ? VirPath + s.Shp_File : "";              
            }
            return shp;
        }
        #region 新增、编辑、删除

        /// <summary>
        /// 修改轮换图片的信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Modify(ShowPicture entity)
        {
            try
            {
                Song.Entities.ShowPicture old = Business.Do<IStyle>().ShowPicSingle(entity.Shp_ID);
                if (old == null) throw new Exception("Not found entity for ShowPicture！");
                old.Copy<Song.Entities.ShowPicture>(entity);
                Business.Do<IStyle>().ShowPicSave(old);
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        [HttpDelete]
        public bool Delete(int id)
        {

            try
            {
                Business.Do<IStyle>().ShowPicDelete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更改排序
        /// </summary>
        /// <param name="items">数组</param>
        /// <returns></returns>
        [HttpPost]
        public bool ModifyTaxis(Song.Entities.ShowPicture[] items)
        {
            try
            {
                Business.Do<IStyle>().ShowUpdateTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 新增轮换图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Upload(Extension = "jpg,png", MaxSize = 2048, CannotEmpty = true)]
        public bool AddPicture(int orgid, string site)
        {
            string filename = string.Empty; 
            try
            {
                //只保存第一张图片
                foreach (string key in this.Files)
                {
                    HttpPostedFileBase file = this.Files[key];
                    //filename = file.FileName;
                    filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                    file.SaveAs(PhyPath + filename);
                    break;
                }

                Song.Entities.ShowPicture mm = new Song.Entities.ShowPicture();
                mm.Shp_IsShow = true;
                mm.Shp_File = filename;
                mm.Org_ID = orgid;
                mm.Shp_Site = site;
                Business.Do<IStyle>().ShowPicAdd(mm);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }
        /// <summary>
        /// 修改轮换图片的图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Upload(Extension = "jpg,png", MaxSize = 2048, CannotEmpty = true)]
        public ShowPicture ModifyPicture(ShowPicture entity)
        {
            string filename = string.Empty;
            try
            {
                //只保存第一张图片
                foreach (string key in this.Files)
                {
                    HttpPostedFileBase file = this.Files[key];
                    //filename = file.FileName;
                    filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                    file.SaveAs(PhyPath + filename);
                    break;
                }

                Song.Entities.ShowPicture old = Business.Do<IStyle>().ShowPicSingle(entity.Shp_ID);
                if (old == null)
                {
                    entity.Shp_File = System.IO.File.Exists(PhyPath + entity.Shp_File) ? VirPath + entity.Shp_File : "";
                    return entity;
                }
                if (!string.IsNullOrWhiteSpace(old.Shp_File))
                {
                    string filehy = PhyPath + old.Shp_File;
                    try
                    {
                        //删除原图
                        if (System.IO.File.Exists(filehy))
                            System.IO.File.Delete(filehy);
                        //删除缩略图，如果有
                        string smallfile = WeiSha.Core.Images.Name.ToSmall(filehy);
                        if (System.IO.File.Exists(smallfile))
                            System.IO.File.Delete(smallfile);
                    }
                    catch { }
                }
                old.Shp_File = filename;
                Business.Do<IStyle>().ShowPicSave(old);

                //
                old.Shp_File = System.IO.File.Exists(PhyPath + old.Shp_File) ? VirPath + old.Shp_File : "";
                return old;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
