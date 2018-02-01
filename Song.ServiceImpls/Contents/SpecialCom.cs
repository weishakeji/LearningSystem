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
        public int SpecialAdd(Special entity)
        {
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<Special>(Special._.Sp_Tax, Special._.Sp_Tax > -1);
            entity.Sp_Tax = obj is int ? (int)obj + 1 : 0;
            //所在机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            return Gateway.Default.Save<Special>(entity);
        }

        public void SpecialSave(Special entity)
        {
            Gateway.Default.Save<Special>(entity);
        }

        public void SpecialDelete(Special entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<Special>(Special._.Sp_Id == entity.Sp_Id);
                    tran.Delete<Special_Article>(Special_Article._.Sp_Id == entity.Sp_Id);
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
        }

        public void SpecialDelete(int identify)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<Special>(Special._.Sp_Id == identify);
                    tran.Delete<Special_Article>(Special_Article._.Sp_Id == identify);
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
        }

        public Special SpecialSingle(int identify)
        {
            return Gateway.Default.From<Special>().Where(Special._.Sp_Id == identify).ToFirst<Special>();
        }

        public Article[] Special4Article(int identify, string searTxt)
        {
            WhereClip wc = Special_Article._.Sp_Id == identify;
            if (searTxt.Trim() != "" && searTxt != null)
            {
                wc.And(Article._.Art_Title.Like("%" + searTxt + "%"));
            }
            return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id).Where(wc).ToArray<Article>();
 
        }

        public Article[] Special4Article(int identify, string searTxt, int num, string type)
        {
            WhereClip wc = Special_Article._.Sp_Id == identify;
            if (searTxt != null && searTxt.Trim() != "")
            {
                wc.And(Article._.Art_Title.Like("%" + searTxt + "%"));
            }
            if (num <= 1) num = int.MaxValue;
            if (type == null || type == "")
            {
                return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id).Where(wc).OrderBy(Article._.Art_IsTop.Desc).OrderBy(Article._.Art_IsHot.Desc).OrderBy(Article._.Art_CrtTime.Desc).ToArray<Article>(num);
            }
            if (type == "hot")
            {
                return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id).Where(wc).OrderBy(Article._.Art_IsHot.Desc).OrderBy(Article._.Art_IsTop.Desc).OrderBy(Article._.Art_CrtTime.Desc).ToArray<Article>(num);
            }
            if (type == "maxflux")
            {
                return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id).Where(wc).OrderBy(Article._.Art_Number.Desc).ToArray<Article>(num);
            }
            if (type == "new")
            {
                return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id).Where(wc).OrderBy(Article._.Art_CrtTime.Desc).ToArray<Article>(num);
            }
            return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id).Where(wc).OrderBy(Article._.Art_CrtTime.Desc).ToArray<Article>(num);
 
        }

        public Special[] SpecialCount(int orgid, bool? isShow, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Special._.Org_ID == orgid);
            if (isShow != null) wc.And(Special._.Sp_IsShow == isShow);
            if (isUse != null) wc.And(Special._.Sp_IsUse == isUse); 
            return Gateway.Default.From<Special>().Where(wc).OrderBy(Special._.Sp_Tax.Desc).ToArray<Special>(count);    
        }

        public bool SpecialUp(int orgid, int id)
        {
            //当前对象
            Special current = Gateway.Default.From<Special>().Where(Special._.Sp_Id == id).ToFirst<Special>();
            //当前对象排序号
            int orderValue = (int)current.Sp_Tax; ;
            //上一个对象，即兄长对象；
            Special up = Gateway.Default.From<Special>()
                .Where(Special._.Sp_Tax > orderValue && Special._.Org_ID == orgid).OrderBy(Special._.Sp_Tax.Desc).ToFirst<Special>();
            if (up == null)
            {
                //如果兄长对象不存在，则表示当前节点在兄弟中是老大；即是最顶点；
                return false;
            }
            //交换排序号
            current.Sp_Tax = up.Sp_Tax;
            up.Sp_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Special>(current);
                    tran.Save<Special>(up);
                    tran.Commit();
                    return true;
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
        }

        public bool SpecialDown(int orgid, int id)
        {
            //当前对象
            Special current = Gateway.Default.From<Special>().Where(Special._.Sp_Id == id).ToFirst<Special>();
            //当前对象排序号
            int orderValue = (int)current.Sp_Tax;
            //下一个对象，即弟弟对象；
            Special down = Gateway.Default.From<Special>()
                .Where(Special._.Sp_Tax < orderValue && Special._.Org_ID == orgid).OrderBy(Special._.Sp_Tax.Asc).ToFirst<Special>();
            if (down == null)
            {
                //如果弟对象不存在，则表示当前节点在兄弟中是老幺；即是最底端；
                return false;
            }
            //交换排序号
            current.Sp_Tax = down.Sp_Tax;
            down.Sp_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Special>(current);
                    tran.Save<Special>(down);
                    tran.Commit();
                    return true;
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
         }

        public bool SpecialAndArticle(int spId, int artId)
        {
            Song.Entities.Special_Article nsa = Gateway.Default.From<Special_Article>().Where(Special_Article._.Sp_Id == spId && Special_Article._.Art_Id == artId).ToFirst<Special_Article>();
            if (nsa == null)
            {
                nsa = new Special_Article();
                nsa.Art_Id = artId;
                nsa.Sp_Id = spId;
                Gateway.Default.Save<Special_Article>(nsa);
                return true;
            }
            return false;
        }

        public bool SpecialAndArticleDel(int spId, int artId)
        {
            Gateway.Default.Delete<Special_Article>(Special_Article._.Sp_Id == spId && Special_Article._.Art_Id == artId);
            //Song.Entities.Special_Article nsa = Gateway.Default.From<Special_Article>().Where(Special_Article._.Sp_Id == spId && Special_Article._.Art_Id == artId).ToFirst<Special_Article>();
            return true;
        }

        public Special[] SpecialPager(string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = Special._.Sp_Id != -1;
            if (searTxt.Trim() != "" && searTxt != null)
            {
                wc.And(Special._.Sp_Name.Like("%" + searTxt + "%"));
            }
            countSum = Gateway.Default.Count<Special>(wc);
            return Gateway.Default.From<Special>().Where(wc).OrderBy(Special._.Sp_Tax.Desc).ToArray<Special>(size, (index - 1) * size);
        }

        public Article[] SpecialArticlePager(int spId, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = Special_Article._.Sp_Id == spId;
            if (searTxt != null && searTxt.Trim() != "")
            {
                wc.And(Article._.Art_Title.Like("%" + searTxt + "%"));
            }
            countSum = this.Special4Article(spId, searTxt).Length;
            return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id).Where(wc).ToArray<Article>(size, (index - 1) * size);
        }

        public Article[] SpecialArticlePager(int spId, string searTxt, int size, int index, out int countSum, bool? isShow, bool? isUse)
        {
            WhereClip wc = Special_Article._.Sp_Id == spId && Article._.Art_IsDel == false;
            if (isShow != null) wc.And(Article._.Art_IsShow == (bool)isShow);
            if (isUse != null) wc.And(Article._.Art_IsUse == isUse);
            if (searTxt != null && searTxt.Trim() != "")wc.And(Article._.Art_Title.Like("%" + searTxt + "%"));
            countSum = this.Special4Article(spId, searTxt).Length;
            return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id).Where(wc).ToArray<Article>(size, (index - 1) * size);
        }

        public Article[] SpecialArticlePager(int spId, string searTxt, int size, int index, out int countSum, bool? isDel, bool? isShow, bool? isUse)
        {
            WhereClip wc = Special_Article._.Sp_Id == spId;
            if (isShow != null) wc.And(Article._.Art_IsShow == (bool)isShow);
            if (isDel != null) wc.And(Article._.Art_IsDel == (bool)isDel);
            if (isUse != null) wc.And(Article._.Art_IsUse == (bool)isUse);
            if (!string.IsNullOrEmpty(searTxt) && searTxt.Trim() != "") wc.And(Article._.Art_Title.Like("%" + searTxt + "%"));
            countSum = this.Special4Article(spId, searTxt).Length;
            return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id)
                .Where(wc).ToArray<Article>(size, (index - 1) * size);
        }

        public Article[] SpecialArticle(int spId, string searTxt, int count)
        {
            WhereClip wc = Special_Article._.Sp_Id == spId;
            if (searTxt != null && searTxt.Trim() != "")
            {
                wc.And(Article._.Art_Title.Like("%" + searTxt + "%"));
            }
            if (count > 0)
            {
                return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id).Where(wc).ToArray<Article>(count);
            }
            //否则取所有记录
            return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id).Where(wc).ToArray<Article>();
        }

        public Article[] SpecialArticle(int spId, string searTxt, bool? isDel, bool? isShow, bool? isUse, int count, string type)
        {
            WhereClip wc = Special_Article._.Sp_Id == spId;
            if (count < 1) count = int.MaxValue;
            if (searTxt != null && searTxt.Trim() != "")
                  wc.And(Article._.Art_Title.Like("%" + searTxt + "%"));
            if (isShow != null) wc.And(Article._.Art_IsShow == (bool)isShow);
            if (isDel != null) wc.And(Article._.Art_IsDel == (bool)isDel);
            if (isUse != null) wc.And(Article._.Art_IsUse == (bool)isUse);
            if (type == "hot")
            {
                return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id).Where(wc)
                    .OrderBy(Article._.Art_IsHot.Desc).OrderBy(Article._.Art_IsTop.Desc).OrderBy(Article._.Art_CrtTime.Desc).ToArray<Article>(count);
            }
            if (type == "maxflux")
            {
                return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id).Where(wc)
                    .OrderBy(Article._.Art_Number.Desc).ToArray<Article>(count);
            }
            if (type == "new")
            {
                return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id).Where(wc)
                    .OrderBy(Article._.Art_CrtTime.Desc).ToArray<Article>(count);
            }
            //否则取所有记录
            return Gateway.Default.From<Article>().InnerJoin<Special_Article>(Special_Article._.Art_Id == Article._.Art_Id).Where(wc)
                .OrderBy(Article._.Art_CrtTime.Desc).ToArray<Article>(count);
        }

    }
}
