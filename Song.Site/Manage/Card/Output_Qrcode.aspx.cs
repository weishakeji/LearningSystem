using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Song.ServiceInterfaces;
using WeiSha.Common;
using System.IO;

namespace Song.Site.Manage.Card
{
    public partial class Output_Qrcode : Extend.CustomPage
    {
        //学习卡设置项的id
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        Song.Entities.Organization org;
        string centerImg = string.Empty;    //二维码中心的图片
        string color = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.LearningCardSet set = Business.Do<ILearningCard>().SetSingle(id);
            this.EntityBind(set);
            if (set != null) this.Title += set.Lcs_Theme;
            //生成二维码的配置
            //各项配置           
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);   //自定义配置项  
            centerImg = Upload.Get["Org"].Virtual + config["QrConterImage"].Value.String;     //中心图片
            centerImg = WeiSha.Common.Server.MapPath(centerImg);
            color = config["QrColor"].Value.String;  //二维码前景色  

            //当前学习卡关联的课程
            Song.Entities.Course[] courses = Business.Do<ILearningCard>().CoursesGet(set);
            if (courses != null)
            {
                dlCourses.DataSource = courses;
                dlCourses.DataBind();                
            }
            //当前学习卡的编码
            Song.Entities.LearningCard[] cards = Business.Do<ILearningCard>().CardCount(-1, set.Lcs_ID, true, null, -1);
            if (courses != null)
            {
                rtpCode.DataSource = cards;
                rtpCode.DataBind();
            }
           
        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        protected string build_Qrcode(string code, string pw)
        {
            string url = lbUrl.Text.Trim();
            string domain = this.Request.Url.Scheme + "://" + this.Request.Url.Host + ":" + this.Request.Url.Port;
            url = string.Format(url, domain, code, pw);
            //二维码图片对象
            System.Drawing.Image image = null;
            if (System.IO.File.Exists(centerImg))
                image = WeiSha.Common.QrcodeHepler.Encode(url, 200, centerImg, color, "#fff");
            else
                image = WeiSha.Common.QrcodeHepler.Encode(url, 200, color, "#fff");
            //将image转为base64
            string base64 = WeiSha.Common.Images.ImageTo.ToBase64(image);
            return base64;
        }
    }
}