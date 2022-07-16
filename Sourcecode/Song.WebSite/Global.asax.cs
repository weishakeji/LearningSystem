using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Song.ServiceInterfaces;
using Song.Entities;
using System.Data;
using WeiSha.Data;

namespace Song.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WeiSha.Data.Gateway.Default.RegisterLogger(new logger());

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //采用自定义模版引擎
            System.Web.Mvc.ViewEngines.Engines.Clear();
            System.Web.Mvc.ViewEngines.Engines.Add(new WeiSha.Core.TemplateEngine());
            //机构中的模版设置，初始化
            if (!WeiSha.Core.PlateOrganInfo.IsInitialization)
            {               
                WeiSha.Core.Business.Do<ITemplate>().SetPlateOrganInfo();
            }
            //账号信息，加入内存缓存，方便后续查询
            //Song.ServiceImpls.AccountLogin.Buffer.Init();
        }
    }
    /// <summary>
    /// SQL查询监控
    /// </summary>
    public class logger : WeiSha.Data.Logger.IExecuteLog
    {
        public void Begin(IDbCommand command)
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            if (_context == null) return;
            string path = _context.Request.Url.AbsolutePath.Replace("/", "_");

            string sql = command.CommandText;
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                System.Data.SqlClient.SqlParameter para = (System.Data.SqlClient.SqlParameter)command.Parameters[i];
                string vl = para.Value.ToString();
                string tp = para.DbType.ToString();
                if (tp.IndexOf("Int") > -1)
                    sql = sql.Replace("@p" + i.ToString(), vl);
                if (tp == "String")
                    sql = sql.Replace("@p" + i.ToString(), "'" + vl + "'");
                if (tp == "Boolean")
                    sql = sql.Replace("@p" + i.ToString(), vl == "True" ? "1" : "0");
                if (tp == "DateTime")
                    sql = sql.Replace("@p" + i.ToString(), "'" + ((DateTime)para.Value).ToString("yyyy/MM/dd HH:mm:ss") + "'");
            }
            //string t = command.Connection
            //WeiSha.Common.Log.Info(path, sql);
            WeiSha.Core.Log.Info(path, sql);
            //WeiSha.Common.Log.Info(path, command.Connection.ConnectionString);
            //throw new NotImplementedException();
        }

        public void End(IDbCommand command, ReturnValue retValue, long elapsedTime)
        {
            //throw new NotImplementedException();
        }
    }
}
