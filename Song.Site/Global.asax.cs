using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
//using System.Data;
using System.Timers;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Web.Mvc;
using System.Web.Routing;
using System.Threading;

namespace Song.Site
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// 在应用程序被实例化或第一次被调用时，该事件被触发。对于所有的HttpApplication 对象实例，它都会被调用。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Init(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// 在HttpApplication 类的第一个实例被创建时，该事件被触发。它允许你创建可以由所有HttpApplication 实例访问的对象。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            //创建路由
            RegisterRoutes(RouteTable.Routes);

            //生成所有机构的二维码
            new Thread(new ThreadStart(() =>
            {
                Business.Do<IOrganization>().OrganBuildQrCode();
            })).Start();

            ////章节缓存创建      
            //new Thread(new ThreadStart(() =>
            //{                    
                
            //})).Start();
            Business.Do<IOutline>().OutlineBuildCache();


            #region  试题的事件            
            Business.Do<IQuestions>().OnSave(null, EventArgs.Empty);
            #endregion

            

        }    

        private void RegisterRoutes(RouteCollection routes){
            //伪静态页面，自动转到ashx动态页（ashx又自动取了/tempates/中的模板用于展示）
            routes.MapPageRoute(
                "web",
                "{term}.htm",
                "~/{term}.ashx"
            );
            //自定义页面（.cs为custom缩写，不是csharp哟），系统自动取/tempates/中的模板用于展示
            routes.MapPageRoute(
                "custom",
                "{term}.cshtm",
                "~/TemplatePage.ashx",
                false
             );
            //************************************
            //同上，只是此为手机端的ashx动态页
            routes.MapPageRoute(
                "mobile",
                "mobile/{term}.htm",
                "~/mobile/{term}.ashx"
                );
            //同上，此为手机端自定义页面
            routes.MapPageRoute(
                "mcustom",
                "mobile/{term}.cshtm",
                "~/TemplatePage.ashx"
                );
            //视频播放的防下载处理
            routes.MapPageRoute(
                "video",
                "v/{term}.aspx",
                "~/VideoUrl.aspx"
                );
        }


        protected void Application_End(object sender, EventArgs e)
        {

        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }
        protected void Session_End(object sender, EventArgs e)
        {

        }

        
    }
}