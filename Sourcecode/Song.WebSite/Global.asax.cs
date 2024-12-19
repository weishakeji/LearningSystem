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
using WeiSha.Core;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Song.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //删除X-AspNetMvc-Version header
            MvcHandler.DisableMvcResponseHeader = true;

            //查询开始之前
            WeiSha.Data.Gateway.Default.RegisterLogger(new Song.ViewData.Helper.DatabaseLog());

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //采用自定义模版引擎
            System.Web.Mvc.ViewEngines.Engines.Clear();
            System.Web.Mvc.ViewEngines.Engines.Add(new WeiSha.Core.TemplateEngine());
            try
            {
                //机构中的模版设置，初始化
                if (!WeiSha.Core.PlateOrganInfo.IsInitialization)
                    WeiSha.Core.Business.Do<ITemplate>().SetPlateOrganInfo();
                //初始化七牛云直播的值
                Business.Do<ILive>().Initialization();
                //账号信息，加入内存缓存，方便后续查询
                //Song.ServiceImpls.AccountLogin.Buffer.Init();        
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex);
            }
            ////更新统计数据,延迟执行
            //WeiSha.Core.Business.Do<IOrganization>().UpdateStatisticalData_Delay(10);
            ////创建定时任务
            //WeiSha.Core.Business.Do<IOrganization>().UpdateStatisticalData_CronJob();

            //执行定时任务
            // 启动 Quartz 调度器  
            StartScheduler().GetAwaiter().GetResult();

        }
        // 定时任务的调度器
        private static IScheduler _scheduler;
        private async Task StartScheduler()
        {
            // 创建调度器  
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();


            // 定义作业  
            IJobDetail job = JobBuilder.Create<ScheduledTaskJob>()
                .WithIdentity("ScheduledTaskJob", "group1")
                .Build();

            //定时的时间，CRON 表达式
            string cron = WeiSha.Core.App.Get["QueryDetail_Cron"].String;
            // 创建触发器，设置指定时间执行  
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithCronSchedule(cron) // CRON 表达式
                .Build();

            // 调度作业  
            await _scheduler.ScheduleJob(job, trigger);

            await _scheduler.Start();
        }
        protected void Application_End(object sender, EventArgs e)
        {
            // 停止并释放 Quartz 调度器  
            _scheduler?.Shutdown(waitForJobsToComplete: true);
        }
       
    }
    /// <summary>
    /// 定时任务的执行类
    /// </summary>
    public class ScheduledTaskJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            //执行方法          
            //更新统计数据
            WeiSha.Core.Business.Do<IOrganization>().UpdateStatisticalData();
            await Task.CompletedTask;
        }
    }
}
