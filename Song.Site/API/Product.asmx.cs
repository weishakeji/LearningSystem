using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.SOAP
{
    /// <summary>
    /// Product 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class Product : System.Web.Services.WebService
    {

        [WebMethod]
        //检索产品
        public Song.Entities.Product[] GetProduct(int ps, string searTxt)
        {
            //产品所有资源的路径
            string resPath = Upload.Get["Product"].Virtual;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.Product[] pds = Business.Do<IContents>().ProductCount(org.Org_ID, 0, 0, false, true, "");
            foreach (Song.Entities.Product entity in pds)
            {
                //将文章内容与简介去除，以方便数据传输
                entity.Pd_Details = "";
                entity.Pd_Logo = resPath + entity.Pd_Logo;
                entity.Pd_LogoSmall = resPath + entity.Pd_LogoSmall;
                entity.Pd_QrCode = resPath + entity.Pd_QrCode;
            }
            return pds;
        }
    }
}
