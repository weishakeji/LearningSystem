using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using VTemplate.Engine;

namespace Song.Template
{
    /// <summary>
    /// 注册模板中使用中的全局方法
    /// </summary>
    public class RegisterFunction
    {
        public static void Register(TemplateDocument doc)
        {
            //一些个常用方法
            doc.RegisterGlobalFunction(Path);   //获取路径           
            doc.RegisterGlobalFunction(getCourses);
            doc.RegisterGlobalFunction(getCoursePrice);//产品价格
            doc.RegisterGlobalFunction(getArticle);
            doc.RegisterGlobalFunction(getConfig);  //当前机构的参数
            doc.RegisterGlobalFunction(getLinks);//友情链接
            doc.RegisterGlobalFunction(ClearHtml);//清理html标签
        }
        #region
        /// <summary>
        /// 清理Html标签
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected static string ClearHtml(object[] p)
        {
            string html = string.Empty;
            try
            {
                if (p.Length > 0 && p[0] != null) html = p[0].ToString();
            }
            catch
            {
                html = "";
            }
            return WeiSha.Common.HTML.ClearTag(html);
        }
        /// <summary>
        /// 获取web.config中上传路径的设置的完整虚拟路径
        /// </summary>
        /// <param name="p">例如Org，即机构上传资料路径</param>
        /// <returns></returns>
        protected static string Path(object[] p)
        {
            string key = null;
            if (p.Length > 0 && p[0] != null) key = p[0].ToString();
            return Upload.Get[key].Virtual;
        }
        /// <summary>
        /// 获取当前机构的参数
        /// </summary>
        /// <param name="p">参数名称</param>
        /// <returns></returns>
        protected static string getConfig(object[] p)
        {
            string key = null;
            if (p.Length > 0 && p[0] != null) key = p[0].ToString();
            if (string.IsNullOrWhiteSpace(key)) return string.Empty;
            //获取当前机构的配置参数
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            return config[key].Value.String;
        }

        /// <summary>
        /// 获取课程
        /// </summary>
        /// <param name="para">两个参数，一个是专业id，第二个为数量</param>
        /// <returns></returns>
        protected static List<Song.Entities.Course> getCourses(object[] para)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //专业id
            int sbj = 0, count = 0;
            if (para.Length > 0 && para[0] is int)
                int.TryParse(para[0].ToString(), out sbj);
            //数量
            if (para.Length > 1 && para[1] is int)
                int.TryParse(para[1].ToString(), out count);
            //排序方式，排序方式，默认null按排序顺序，flux流量最大优先,def推荐、流量，tax排序号，new最新,rec推荐
            string order = "def";
            if (para.Length > 2) order = para[2].ToString();
            List<Song.Entities.Course> cour = Business.Do<ICourse>().CourseCount(org.Org_ID, sbj, null, true, order, count);
            foreach (Song.Entities.Course c in cour)
            {
                c.Cou_LogoSmall = Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                c.Cou_Logo = Upload.Get["Course"].Virtual + c.Cou_Logo;
                c.Cou_Intro = HTML.ClearTag(c.Cou_Intro);
            }
            return cour;
        }
        /// <summary>
        /// 获取课程价格
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        protected static Song.Entities.CoursePrice[] getCoursePrice(object[] para)
        {            
            //课程uid
            string couuid = null;
            if (para.Length > 0 && para[0] != null) couuid = para[0].ToString();
            if (string.IsNullOrWhiteSpace(couuid)) return null;
            Song.Entities.CoursePrice[] prices = Business.Do<ICourse>().PriceCount(0, couuid, true, 0);
            return prices;
        }
        /// <summary>
        /// 获取新闻
        /// </summary>
        /// <param name="para">两个参数，一个是分类id，第二个为数量</param>
        /// <returns></returns>
        protected static Song.Entities.Article[] getArticle(object[] para)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int col = 0, count = 0;
            if (para.Length > 0 && para[0] is int)
                int.TryParse(para[0].ToString(), out col);
            if (para.Length > 1 && para[1] is int)
                int.TryParse(para[1].ToString(), out count);
            Song.Entities.Article[] arts = Business.Do<IContents>().ArticleCount(org.Org_ID, col, count, "hot");
            foreach (Song.Entities.Article c in arts)
                c.Art_Logo = Upload.Get["News"].Virtual + c.Art_Logo;
            return arts;
        }
        /// <summary>
        /// 友情链接
        /// </summary>
        /// <returns></returns>
        protected static Song.Entities.Links[] getLinks(object[] paras)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int sortid = 0, count = 0;
            string sortName = string.Empty;
            if (paras != null && paras.Length > 0)
            {
                if (paras[0] != null)
                {
                    int.TryParse(paras[0].ToString(), out sortid);
                    if (sortid == 0) sortName = paras[0].ToString();
                }
            }
            if (paras != null && paras.Length > 1)
            {
                if (paras[1] != null)
                {
                    int.TryParse(paras[1].ToString(), out count);
                }
            }
            Song.Entities.Links[] links = null;
            if (sortid != 0) links = Business.Do<ILinks>().GetLinks(org.Org_ID, sortid, true, true, count);
            if (sortid == 0) links = Business.Do<ILinks>().GetLinks(org.Org_ID, sortName, true, true, count);
            if (links != null && links.Length > 0)
            {
                foreach (Song.Entities.Links l in links)
                {
                    l.Lk_Logo = Upload.Get["Links"].Virtual + l.Lk_Logo;
                }
            }
            return links;
        }
        #endregion
    }
}
