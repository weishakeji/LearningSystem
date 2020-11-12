using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WeiSha.Common;
using Song.Entities;
using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;

namespace Song.ServiceImpls
{
    public class AccessoryCom : IAccessory
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Add(Accessory entity)
        {
            entity.As_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            //如果没有填写扩展名，则自动加上
            if (string.IsNullOrWhiteSpace(entity.As_Extension))
            {
                string extension = System.IO.Path.GetExtension(entity.As_FileName);
                if (entity.As_FileName.IndexOf(".") > -1) extension = extension.Replace(".", "");
                entity.As_Extension = extension;
            }
            Gateway.Default.Save<Accessory>(entity);  
            //如果是视频,设置该视频所在的章节是否有视频
            Song.Entities.Outline outline = Gateway.Default.From<Outline>().Where(Outline._.Ol_UID == entity.As_Uid).ToFirst<Outline>();
            if (outline != null) Business.Do<IOutline>().OutlineSave(outline);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Save(Accessory entity)
        {
            //如果没有填写扩展名，则自动加上
            if (string.IsNullOrWhiteSpace(entity.As_Extension))
            {
                string extension = System.IO.Path.GetExtension(entity.As_FileName);
                if (entity.As_FileName.IndexOf(".") > -1) extension = extension.Replace(".", "");
                entity.As_Extension = extension;
            }
            Gateway.Default.Save<Accessory>(entity);
            //如果是视频,设置该视频所在的章节是否有视频
            Song.Entities.Outline outline = Gateway.Default.From<Outline>().Where(Outline._.Ol_UID == entity.As_Uid).ToFirst<Outline>();
            if (outline != null) Business.Do<IOutline>().OutlineSave(outline);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void Delete(int identify)
        {
            Accessory ac = this.GetSingle(identify);
            if (ac == null) return;
            WeiSha.WebControl.FileUpload.Delete(ac.As_Type, ac.As_FileName);
            string ext = ac.As_FileName.IndexOf(".") > -1 ? ac.As_FileName.Substring(ac.As_FileName.LastIndexOf(".")) : "";
            if (ext.ToLower() == ".flv")
            {
                string name = ac.As_FileName.IndexOf(".") > -1 ? ac.As_FileName.Substring(0, ac.As_FileName.LastIndexOf(".")) : ac.As_FileName;
                WeiSha.WebControl.FileUpload.Delete(ac.As_Type, name + ".mp4");
            }
            Gateway.Default.Delete<Accessory>(Accessory._.As_Id == identify);
            //如果是视频,设置该视频所在的章节是否有视频
            Song.Entities.Outline outline = Gateway.Default.From<Outline>().Where(Outline._.Ol_UID == ac.As_Uid).ToFirst<Outline>();
            if (outline != null) Business.Do<IOutline>().OutlineSave(outline);
        }
        /// <summary>
        /// 删除，按系统唯一id
        /// </summary>
        /// <param name="uid">系统唯一id</param>
        public void Delete(string uid)
        {
            Delete(uid, true);
        }
        /// <summary>
        /// 删除，按系统唯一id
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="isDelfile">是否删除文件</param>
        public void Delete(string uid, bool isDelfile)
        {           
            if (isDelfile)
            {
                List<Accessory> acs = this.GetAll(uid);
                foreach (Accessory ac in acs)
                {
                    WeiSha.WebControl.FileUpload.Delete(ac.As_Type, ac.As_FileName);
                    string ext = ac.As_FileName.IndexOf(".") > -1 ? ac.As_FileName.Substring(ac.As_FileName.LastIndexOf(".")) : "";
                    if (ext.ToLower() == ".flv")
                    {
                        string name = ac.As_FileName.IndexOf(".") > -1 ? ac.As_FileName.Substring(0, ac.As_FileName.LastIndexOf(".")) : ac.As_FileName;
                        WeiSha.WebControl.FileUpload.Delete(ac.As_Type, name + ".mp4");
                    }
                }
            }
            Gateway.Default.Delete<Accessory>(Accessory._.As_Uid == uid);
            //如果是视频,设置该视频所在的章节是否有视频
            //Song.Entities.Outline outline = Gateway.Default.From<Outline>().Where(Outline._.Ol_UID == uid).ToFirst<Outline>();
            //if (outline != null) Business.Do<IOutline>().OutlineSave(outline);
        }
        public void Delete(string uid, WeiSha.Data.DbTrans tran)
        {
            List<Accessory> acs = this.GetAll(uid);
            if (acs == null) return;
            foreach (Accessory ac in acs)
            {
                try
                {
                    WeiSha.WebControl.FileUpload.Delete(ac.As_Type, ac.As_FileName);
                    string ext = ac.As_FileName.IndexOf(".") > -1 ? ac.As_FileName.Substring(ac.As_FileName.LastIndexOf(".")) : "";
                    if (ext.ToLower() == ".flv")
                    {
                        string name = ac.As_FileName.IndexOf(".") > -1 ? ac.As_FileName.Substring(0, ac.As_FileName.LastIndexOf(".")) : ac.As_FileName;
                        WeiSha.WebControl.FileUpload.Delete(ac.As_Type, name + ".mp4");
                    }
                }
                catch
                {
                }
            }
            tran.Delete<Accessory>(Accessory._.As_Uid == uid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Accessory GetSingle(int identify)
        {
            return Gateway.Default.From<Accessory>().Where(Accessory._.As_Id == identify).ToFirst<Accessory>();
        }
        public Accessory GetSingle(string uid)
        {
            return Gateway.Default.From<Accessory>().Where(Accessory._.As_Uid == uid).ToFirst<Accessory>();
        }
        /// <summary>
        /// 获取某个院系的所有人员；
        /// </summary>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        public List<Accessory> GetAll(string uid)
        {
            if (uid == null || uid == string.Empty) return null;
            return Gateway.Default.From<Accessory>().Where(Accessory._.As_Uid == uid).ToList<Accessory>();
        }
        public List<Accessory> GetAll(string uid, string type)
        {
            if (uid == null || uid == string.Empty) return null;
            return Gateway.Default.From<Accessory>().Where(Accessory._.As_Uid == uid && Accessory._.As_Type == type).ToList<Accessory>();
        }
        /// <summary>
        /// 共计多少个记录
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int OfCount(string uid, string type)
        {
            WhereClip wc = Accessory._.As_Uid == uid && Accessory._.As_Type == type;
            return Gateway.Default.Count<Accessory>(wc);
        }
    }
}
