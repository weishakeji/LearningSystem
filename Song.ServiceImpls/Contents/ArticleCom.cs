using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Resources;
using System.Reflection;

namespace Song.ServiceImpls
{
    public partial class ContentsCom : IContents
    {
        private string _artUppath = "News";
        public int ArticleAdd(Article entity)
        {
            //创建时间
            entity.Art_CrtTime = DateTime.Now;
            if (entity.Art_PushTime < DateTime.Now.AddYears(-100))
                entity.Art_PushTime = entity.Art_CrtTime;
            //所在机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            //所属栏目
            Song.Entities.Columns nc = Business.Do<IColumns>().Single((int)entity.Col_Id);
            if (nc != null) entity.Col_Name = nc.Col_Name;
            //如果是置顶新闻
            if (entity.Art_IsTop)
            {
                int topNumber = Business.Do<ISystemPara>()["NewsMaxTop"].Int16 ?? 10;
                int count = Gateway.Default.Count<Article>(Article._.Art_IsTop == true);
                //如果置顶信息过多
                if (count >= topNumber)
                {
                    Song.Entities.Article[] tna = Gateway.Default.From<Article>().Where(Article._.Art_IsTop == true).OrderBy(Article._.Art_CrtTime.Asc).ToArray<Article>(count - topNumber + 1);
                    if (tna != null)
                    {
                        foreach (Article na in tna)
                        {
                            na.Art_IsTop = false;
                            Gateway.Default.Save<Article>(na);
                        }
                    }
                }
            }
            //如果是推荐的新闻
            if (entity.Art_IsRec)
            {
                int recNumber = Business.Do<ISystemPara>()["NewsMaxRec"].Int16 ?? 10;
                int reccount = Gateway.Default.Count<Article>(Article._.Art_IsRec == true);
                //如果置顶信息过多
                if (reccount >= recNumber)
                {
                    Song.Entities.Article[] tna = Gateway.Default.From<Article>().Where(Article._.Art_IsRec == true).OrderBy(Article._.Art_CrtTime.Asc).ToArray<Article>(reccount - recNumber + 1);
                    if (tna != null)
                    {
                        foreach (Article na in tna)
                        {
                            na.Art_IsRec = false;
                            Gateway.Default.Save<Article>(na);
                        }
                    }
                }
            }
            //如果不需要审核
            bool isveri = Business.Do<ISystemPara>()["NewsIsVerify"].Boolean ?? true;
            if (!isveri)
            {
                entity.Art_IsVerify = true;
            }
            entity.Art_IsUse = true;
            return Gateway.Default.Save<Article>(entity);
        }

        public void ArticleSave(Article entity)
        {
            entity.Art_LastTime = DateTime.Now;
            if (entity.Art_PushTime < DateTime.Now.AddYears(-100))
                entity.Art_PushTime = entity.Art_CrtTime;
            Song.Entities.Columns nc = Business.Do<IColumns>().Single((int)entity.Col_Id);
            if (nc != null) entity.Col_Name = nc.Col_Name;
            //如果是置顶新闻
            if (entity.Art_IsTop)
            {
                int topNumber = Business.Do<ISystemPara>()["NewsMaxTop"].Int16 ?? 10;
                int count = Gateway.Default.Count<Article>(Article._.Art_IsTop == true);
                //如果置顶信息过多
                if (count >= topNumber)
                {
                    Song.Entities.Article[] tna = Gateway.Default.From<Article>().Where(Article._.Art_IsTop == true).OrderBy(Article._.Art_CrtTime.Asc).ToArray<Article>(count - topNumber + 1);
                    if (tna != null)
                    {
                        foreach (Article na in tna)
                        {
                            na.Art_IsTop = false;
                            Gateway.Default.Save<Article>(na);
                        }
                    }
                }
            }
            //如果是推荐的新闻
            if (entity.Art_IsRec)
            {
                int recNumber = Business.Do<ISystemPara>()["NewsMaxRec"].Int16 ?? 10;
                int reccount = Gateway.Default.Count<Article>(Article._.Art_IsRec == true);
                //如果置顶信息过多
                if (reccount >= recNumber)
                {
                    Song.Entities.Article[] tna = Gateway.Default.From<Article>().Where(Article._.Art_IsRec == true).OrderBy(Article._.Art_CrtTime.Asc).ToArray<Article>(reccount - recNumber + 1);
                    if (tna != null)
                    {
                        foreach (Article na in tna)
                        {
                            na.Art_IsRec = false;
                            Gateway.Default.Save<Article>(na);
                        }
                    }
                }
            }
            //如果不需要审核
            bool isveri = Business.Do<ISystemPara>()["NewsIsReVeri"].Boolean ?? true;
            if (!isveri)
            {
                entity.Art_IsVerify = true;
            }
            //如果修改后需要重新审核
            bool isrevi = Business.Do<ISystemPara>()["NewsIsReVeri"].Boolean ?? true;
            if (isveri)
            {
                entity.Art_IsVerify = false;
            }
            Gateway.Default.Save<Article>(entity);
        }


        public int ArticleAddNumber(int id, int addNum)
        {
            object obj = Gateway.Default.Max<Article>(Article._.Art_Number, Article._.Art_Id == id);
            int i = 0;
            try
            {
                i = (int)obj;
            }
            catch { }
            //如果不用增加，则直接返回当前浏览量
            if (addNum < 1) return i;

            //增加记数
            i += addNum;
            Gateway.Default.Update<Article>(new Field[] { Article._.Art_Number }, new object[] { i }, Article._.Art_Id == id);
            return i;
        }

        public void ArticleDelete(Article entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                ArticleDelete(entity, null);
            }       
        }
        public void ArticleDelete(Article entity, DbTrans tran)
        {
            if (tran == null) tran = Gateway.Default.BeginTrans();
            try
            {
                //删除附件
                Business.Do<IAccessory>().Delete(entity.Art_Uid);
                tran.Delete<Article>(Article._.Art_Id == entity.Art_Id);
                //删除图片文件
                string img = WeiSha.Common.Upload.Get[_artUppath].Physics + entity.Art_Logo;
                if (System.IO.File.Exists(img))
                    System.IO.File.Delete(img);
                //删除文章
                tran.Delete<Article>(Article._.Art_Id == entity.Art_Id);
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;

            }
            finally
            {
                tran.Close();
            }
        }
        public void ArticleDelete(int identify)
        {
            Article na = this.ArticleSingle(identify);
            this.ArticleDelete(na);
        }

        public void ArticleDeleteAll(int orgid, int colid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Article._.Org_ID == orgid);
            if (colid > 0) wc.And(Article._.Col_Id == colid);
            Song.Entities.Article[] entities = Gateway.Default.From<Article>().Where(wc).ToArray<Article>();
            foreach (Song.Entities.Article entity in entities)
            {
                ArticleDelete(entity);
            }
        }

        public void ArticleIsDelete(int identify)
        {
            Gateway.Default.Update<Article>(new Field[] { Article._.Art_IsDel }, new object[] { true }, Article._.Art_Id == identify);
        }

        public void ArticleRecover(int identify)
        {
            Gateway.Default.Update<Article>(new Field[] { Article._.Art_IsDel }, new object[] { false }, Article._.Art_Id == identify);
        }

        public void ArticlePassVerify(int identify, string verMan)
        {
            Gateway.Default.Update<Article>(new Field[] { Article._.Art_IsVerify, Article._.Art_VerifyTime, Article._.Art_VerifyMan }, new object[] { true, DateTime.Now, verMan }, Article._.Art_Id == identify);
        }

        public Article ArticleSingle(int identify)
        {
            return Gateway.Default.From<Article>().Where(Article._.Art_Id == identify).ToFirst<Article>();
        }
        /// <summary>
        /// 当前新闻的上一条新闻
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public Article ArticlePrev(int identify, int orgid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Article._.Org_ID == orgid;
            wc &= Article._.Art_IsShow == true;
            wc &= Article._.Art_PushTime < DateTime.Now;
            Song.Entities.Article art = this.ArticleSingle(identify);
            return Gateway.Default.From<Article>().OrderBy(Article._.Art_PushTime.Asc)
                .Where(wc && Article._.Art_PushTime > art.Art_PushTime).ToFirst<Article>();
        }
        /// <summary>
        /// 当前新闻的下一条新闻
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public Article ArticleNext(int identify, int orgid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Article._.Org_ID == orgid;
            wc &= Article._.Art_IsShow == true;
            wc &= Article._.Art_PushTime < DateTime.Now;
            Song.Entities.Article art = this.ArticleSingle(identify);
            return Gateway.Default.From<Article>().OrderBy(Article._.Art_PushTime.Desc)
                .Where(wc && Article._.Art_PushTime < art.Art_PushTime).ToFirst<Article>();
        }
        public Special[] Article4Special(int identify)
        {
            return Gateway.Default.From<Special>().InnerJoin<Special_Article>(Special_Article._.Sp_Id == Special._.Sp_Id)
                .Where(Special_Article._.Art_Id == identify).ToArray<Special>();
        }

        public Article[] ArticleCount(int orgid, int colid, int topNum, string order)
        {
            WhereClip wc = Article._.Art_IsDel == false && Article._.Art_IsShow == true;
            if (orgid > 0) wc.And(Article._.Org_ID == orgid);
            //if (colid > 0) wc.And(Article._.Col_Id == colid);
            if (colid > 0)
            {
                WhereClip wcColid = new WhereClip();
                List<int> list = Business.Do<IColumns>().TreeID((int)colid);
                foreach (int l in list)
                    wcColid.Or(Article._.Col_Id == l);
                wc.And(wcColid);
            }
            OrderByClip wcOrder = new OrderByClip();
            if (order == "hot") wcOrder = Article._.Art_IsHot.Desc;
            if (order == "img") wcOrder = Article._.Art_IsImg.Desc;
            if (order == "rec") wcOrder = Article._.Art_IsRec.Desc;
            if (order == "flux") wcOrder = Article._.Art_Number.Desc;
            Song.Entities.Article[] arts = Gateway.Default.From<Article>().Where(wc).OrderBy(wcOrder && Article._.Art_PushTime.Desc && Article._.Art_CrtTime.Desc).ToArray<Article>(topNum);
            return arts;
        }

        public int ArticleOfCount(int orgid, int colId)
        {
            WhereClip wc = Article._.Art_IsDel == false && Article._.Art_IsShow == true && Article._.Art_IsVerify == true;
            if (orgid > 0) wc.And(Article._.Org_ID == orgid);
            if (colId > 0) wc.And(Article._.Col_Id == colId);
            return Gateway.Default.Count<Article>(wc);
        }

        public Article[] ArticlePager(int orgid, int? colid, bool? isShow, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = Article._.Art_IsDel == false;
            if (orgid > 0) wc.And(Article._.Org_ID == orgid);
            //if (colid > 0) wc.And(Article._.Col_Id == colid);
            if (colid > 0)
            {
                WhereClip wcColid = new WhereClip();
                List<int> list = Business.Do<IColumns>().TreeID((int)colid);
                foreach (int l in list)
                    wcColid.Or(Article._.Col_Id == l);
                wc.And(wcColid);
            }
            if (isShow != null) wc.And(Article._.Art_IsShow == (bool)isShow);
            if (searTxt != null && searTxt.Trim() != "")
                  wc.And(Article._.Art_Title.Like("%" + searTxt + "%"));
              countSum = Gateway.Default.Count<Article>(wc);
              return Gateway.Default.From<Article>().Where(wc).OrderBy(Article._.Art_PushTime.Desc).ToArray<Article>(size, (index - 1) * size);
        }

        public Article[] ArticlePager(int orgid, int? colid, bool? isVerify, bool? isDel, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Article._.Org_ID == orgid);
            if (colid > 0)
            {
                WhereClip wcColid = new WhereClip();
                List<int> list = Business.Do<IColumns>().TreeID((int)colid);
                foreach (int l in list)
                    wcColid.Or(Article._.Col_Id == l);
                wc.And(wcColid);
            }
            if (searTxt != null && searTxt.Trim() != "") wc.And(Article._.Art_Title.Like("%" + searTxt + "%"));
            if (isVerify != null) wc.And(Article._.Art_IsVerify == (bool)isVerify);
            if (isDel != null) wc.And(Article._.Art_IsDel == (bool)isDel);
            countSum = Gateway.Default.Count<Article>(wc);
            return Gateway.Default.From<Article>().Where(wc).OrderBy(Article._.Art_PushTime.Desc).ToArray<Article>(size, (index - 1) * size);
        }

        public Article[] ArticlePager(int orgid, int? colid, string searTxt, bool? isVerify, bool? isDel, bool? isTop, bool? isHot, bool? isRec, bool? isImg, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Article._.Org_ID == orgid);
            if (colid > 0)
            {
                WhereClip wcColid = new WhereClip();
                List<int> list = Business.Do<IColumns>().TreeID((int)colid);
                foreach (int l in list)
                    wcColid.Or(Article._.Col_Id == l);
                wc.And(wcColid);
            }
            if (searTxt != null && searTxt.Trim() != "") wc.And(Article._.Art_Title.Like("%" + searTxt + "%"));
            if (isVerify != null) wc.And(Article._.Art_IsVerify == (bool)isVerify);
            if (isDel != null) wc.And(Article._.Art_IsDel == (bool)isDel);
            if (isTop != null) wc.And(Article._.Art_IsTop == (bool)isTop);
            if (isHot != null) wc.And(Article._.Art_IsHot == (bool)isHot);
            if (isRec != null) wc.And(Article._.Art_IsRec == (bool)isRec);
            if (isImg != null) wc.And(Article._.Art_IsImg == (bool)isImg);
            countSum = Gateway.Default.Count<Article>(wc);
            return Gateway.Default.From<Article>().Where(wc).OrderBy(Article._.Art_PushTime.Desc).ToArray<Article>(size, (index - 1) * size);
        }

    }
}
