using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WeiSha.Core;
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
            //如果文件带有多余路径，只保留文件名
            if (!entity.As_IsOther && !entity.As_IsOuter)
            {
                if (!string.IsNullOrWhiteSpace(entity.As_FileName) && entity.As_FileName.IndexOf("/") > -1)
                    entity.As_FileName = entity.As_FileName.Substring(entity.As_FileName.LastIndexOf("/") + 1);
            }
            Gateway.Default.Save<Accessory>(entity);

            //如果是视频,设置该视频所在的章节是否有视频
            if (entity.As_Type == "CourseVideo" || entity.As_Type == "Course")
            {
                Song.Entities.Outline outline = Gateway.Default.From<Outline>().Where(Outline._.Ol_UID == entity.As_Uid).ToFirst<Outline>();
                if (outline != null) Business.Do<IOutline>().OutlineSave(outline);
            }
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
            //如果文件带有多余路径，只保留文件名
            if (!entity.As_IsOther && !entity.As_IsOuter)
            {
                if (!string.IsNullOrWhiteSpace(entity.As_FileName) && entity.As_FileName.IndexOf("/") > -1)
                    entity.As_FileName = entity.As_FileName.Substring(entity.As_FileName.LastIndexOf("/") + 1);
            }
            Gateway.Default.Save<Accessory>(entity);
            //如果是视频,设置该视频所在的章节是否有视频
            if (entity.As_Type == "CourseVideo" || entity.As_Type == "Course")
            {
                Song.Entities.Outline outline = Gateway.Default.From<Outline>().Where(Outline._.Ol_UID == entity.As_Uid).ToFirst<Outline>();
                if (outline != null) Business.Do<IOutline>().OutlineSave(outline);
            }
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void Delete(int identify)
        {
            Accessory ac = this.GetSingle(identify);
            if (ac == null) return;
            this.Delete(ac);
        }
        /// <summary>
        /// 删除，按系统唯一id
        /// </summary>
        /// <param name="uid">系统唯一id</param>
        /// <param name="type"></param>
        public void Delete(string uid, string type)
        {
            WhereClip wc = Accessory._.As_Uid == uid;
            if (!string.IsNullOrWhiteSpace(type))
                wc.And(Accessory._.As_Type == type);
            List<Accessory> list = Gateway.Default.From<Accessory>().Where(wc).ToList<Accessory>();
            foreach (Accessory ac in list)
                this.Delete(ac);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ac"></param>
        public void Delete(Accessory ac)
        {
            //判断当前附件的文件是否存在其它的引用
            int count = Gateway.Default.Count<Accessory>(Accessory._.As_Id != ac.As_Id && Accessory._.As_Type == ac.As_Type && Accessory._.As_FileName == ac.As_FileName);
            if (count < 1)
            {
                WeiSha.Core.Upload.Get[ac.As_Type].DeleteFile(ac.As_FileName);
                string ext = ac.As_FileName.IndexOf(".") > -1 ? ac.As_FileName.Substring(ac.As_FileName.LastIndexOf(".")) : "";
                if (ext.ToLower() == ".flv")
                {
                    string name = ac.As_FileName.IndexOf(".") > -1 ? ac.As_FileName.Substring(0, ac.As_FileName.LastIndexOf(".")) : ac.As_FileName;
                    WeiSha.Core.Upload.Get[ac.As_Type].DeleteFile(name + ".mp4");
                }
            }
            Gateway.Default.Delete<Accessory>(Accessory._.As_Id == ac.As_Id);
            //如果是视频,设置该视频所在的章节是否有视频
            if (ac.As_Type == "CourseVideo" || ac.As_Type == "Course")
            {
                Song.Entities.Outline outline = Gateway.Default.From<Outline>().Where(Outline._.Ol_UID == ac.As_Uid).ToFirst<Outline>();
                if (outline != null) Business.Do<IOutline>().OutlineSave(outline);
            }
        }

        public void Delete(string uid, WeiSha.Data.DbTrans tran)
        {
            List<Accessory> acs = this.GetAll(uid);
            if (acs == null) return;
            tran.Delete<Accessory>(Accessory._.As_Uid == uid);
            foreach (Accessory ac in acs)
            {
                try
                {
                    //判断当前附件的文件是否存在其它的引用
                    int count = tran.Count<Accessory>(Accessory._.As_Type==ac.As_Type && Accessory._.As_FileName==ac.As_FileName);
                    if (count < 1)
                    {
                        WeiSha.Core.Upload.Get[ac.As_Type].DeleteFile(ac.As_FileName);
                        string ext = ac.As_FileName.IndexOf(".") > -1 ? ac.As_FileName.Substring(ac.As_FileName.LastIndexOf(".")) : "";
                        if (ext.ToLower() == ".flv")
                        {
                            string name = ac.As_FileName.IndexOf(".") > -1 ? ac.As_FileName.Substring(0, ac.As_FileName.LastIndexOf(".")) : ac.As_FileName;
                            WeiSha.Core.Upload.Get[ac.As_Type].DeleteFile(name + ".mp4");
                        }
                    }
                }
                catch
                {
                }
            }
            
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
        public Accessory GetSingle(string uid, string type)
        {
            return Gateway.Default.From<Accessory>().Where(Accessory._.As_Uid == uid && Accessory._.As_Type == type).ToFirst<Accessory>();
        }
        /// <summary>
        ///附件列表
        /// </summary>
        /// <param name="uid">附件主题的uid</param>
        /// <returns></returns>
        public List<Accessory> GetAll(string uid)
        {
            if (string.IsNullOrWhiteSpace(uid)) return null;
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
        public int OfCount(int orgid, string uid, string type)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Accessory._.Org_ID == orgid);
            if (!string.IsNullOrWhiteSpace(uid))           
                wc.And(Accessory._.As_Uid == uid);
            if (!string.IsNullOrWhiteSpace(type))
                wc.And(Accessory._.As_Type == type);

            return Gateway.Default.Count<Accessory>(wc);
        }
    }
}
