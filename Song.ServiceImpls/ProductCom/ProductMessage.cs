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
    public partial class ProductCom : IProduct
    {

        #region 产品咨询留言的管理

        public int MessageAdd(ProductMessage entity)
        {
            if (entity.Pd_Id >= 0)
            {
                Song.Entities.Product p = new ContentsCom().ProductSingle((int)entity.Pd_Id);
                if (p != null)
                {
                    entity.Pd_Name = p.Pd_Name;
                }
            }
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            return Gateway.Default.Save<ProductMessage>(entity);
        }

        public void MessageSave(ProductMessage entity)
        {
            //如果回复内容不为空，则表示已经回复过
            entity.Pm_IsAns = entity.Pm_Answer !=null && entity.Pm_Answer.Trim() != "";

            Gateway.Default.Save<ProductMessage>(entity);
        }

        public void MessageDelete(ProductMessage entity)
        {
            Gateway.Default.Delete<ProductMessage>(ProductMessage._.Pm_Id == entity.Pm_Id);
        }

        public void MessageDelete(int identify)
        {
            Gateway.Default.Delete<ProductMessage>(ProductMessage._.Pm_Id == identify);
        }

        public ProductMessage MessageSingle(int identify)
        {
            return Gateway.Default.From<ProductMessage>().Where(ProductMessage._.Pm_Id == identify).ToFirst<ProductMessage>();
        }

        public Product ProductByMessage(int pmid)
        {
            return Gateway.Default.From<Product>().InnerJoin<ProductMessage>(Product._.Pd_Id == ProductMessage._.Pd_Id).Where(ProductMessage._.Pm_Id == pmid).ToFirst<Product>();
        }

        public ProductMessage[] GetProductMessagePager(int? pdid, string searTxt, bool? isAns, bool? isShow, int size, int index, out int countSum)
        {
            WhereClip wc = ProductMessage._.Pm_Id != -1;
            if (pdid != -1 && pdid != null)
            {
                wc.And(ProductMessage._.Pd_Id == pdid);
            }
            if (isAns != null)
            {
                wc.And(ProductMessage._.Pm_IsAns == isAns);
            }
            if (isShow != null)
            {
                wc.And(ProductMessage._.Pm_IsShow == isShow);
            }
            if (searTxt != null && searTxt.Trim() != "")
            {
                wc.And(ProductMessage._.Pm_Title.Like("%" + searTxt + "%"));
            }
            countSum = Gateway.Default.Count<ProductMessage>(wc);
            return Gateway.Default.From<ProductMessage>().Where(wc).OrderBy(ProductMessage._.Pm_CrtTime.Desc).ToArray<ProductMessage>(size, (index - 1) * size);
        }
        public ProductMessage[] GetProductMessagePager(string searTxt, bool? isAns, bool? isShow, int size, int index, out int countSum)
        {
            WhereClip wc = ProductMessage._.Pm_Id != -1;
            if (isAns != null)
            {
                wc.And(ProductMessage._.Pm_IsAns == isAns);
            }
            if (isShow != null)
            {
                wc.And(ProductMessage._.Pm_IsShow == isShow);
            }
            if (searTxt != null && searTxt.Trim() != "")
            {
                wc.And(ProductMessage._.Pm_Title.Like("%" + searTxt + "%"));
            }
            countSum = Gateway.Default.Count<ProductMessage>(wc);
            return Gateway.Default.From<ProductMessage>().Where(wc).OrderBy(ProductMessage._.Pm_CrtTime.Desc).ToArray<ProductMessage>(size, (index - 1) * size);
        }
        #endregion
    }
}
