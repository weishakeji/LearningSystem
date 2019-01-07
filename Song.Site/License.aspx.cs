using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Song.Site
{
    public partial class License : System.Web.UI.Page
    {
        //根域名
        protected string mainname = WeiSha.Common.Server.MainName;
        //版权信息
        protected WeiSha.Common.Copyright<string, string> copyright = WeiSha.Common.Request.Copyright;
        protected void Page_Load(object sender, EventArgs e)
        {    
            //默认打开方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                /*基本信息*/

                //授权信息
                WeiSha.Common.License lic = WeiSha.Common.License.Value;
                //限制的主域授权类型
                foreach (string d in lic.LimitDomain) lbRootLimit.Text += "." + d + "、";
                if (lbRootLimit.Text.EndsWith("、")) lbRootLimit.Text = lbRootLimit.Text.Substring(0, lbRootLimit.Text.Length - 1);
                //
                DateTime initTime = lic.InitDate;
                lbVersion.Text = lic.VersionName;   //当前版本            
                ltVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();  //内部版本号
                //系统初始时间
                lbInitDate.Text = lic.InitDate.ToString("yyyy年MM月dd日 HH:mm:ss");
                //已经运行了多少时间
                TimeSpan runTime = DateTime.Now - lic.InitDate;
                lbRunTime.Text = Math.Floor(runTime.TotalDays) + "天" + runTime.Hours + "小时";
                //当前版本的限制
                rptLimit.DataSource = lic.LimitItems;
                rptLimit.DataBind();
                //各版本之间的差别数据
                BindVersionLevel();

                //是否获得授权
                licInfo.Visible = lic.VersionLevel > 0;
                if (lic.VersionLevel > 0)
                {
                    //授权类型，授权主体，起始时间
                    ltLicType.Text = lic.Type.ToString();
                    if ((int)lic.Type == 1 || (int)lic.Type == 2)
                        ltLicTarget.Text = lic.Serial;
                    else
                        ltLicTarget.Text = lic.Serial + ":" + lic.Port;
                    ltStartTime.Text = lic.StartTime.ToString("yyyy-MM-dd");
                    ltEndTime.Text = lic.EndTime.ToString("yyyy-MM-dd");
                    //在激活类型上显示当前类型
                    ListItem litype = rblActivType.Items.FindByValue(((int)lic.Type).ToString());
                    if (litype != null)
                    {
                        rblActivType.SelectedIndex = -1;
                        litype.Selected = true;
                    }
                }
                //假如授权信息不为空，例如授权过期了，虽然显示为免费版，但仍然会有授权信息。
                if (!string.IsNullOrWhiteSpace(lic.FullText))
                {
                    //完整的授权信息
                    ltLicInfo.Text = lic.FullText.Replace("\n", "<br/>");
                    plLicInfoBox.Visible = true;
                    //如果授权时间过期了，这里醒目提示
                    if (lic.EndTime < DateTime.Now)
                    {
                        ltLicInfo.Text = new Regex(@"结束时间：\d{4}年\d{1,2}月\d{1,2}日")
                            .Replace(ltLicInfo.Text, "结束时间：<span style=\"color:red;\">" + lic.EndTime.ToString("yyyy年MM月dd日") + "</span>");
                    }
                    UTF8Encoding utf8 = new UTF8Encoding();
                    Byte[] encodedBytes = utf8.GetBytes(ltLicInfo.Text);
                    String decodedString = utf8.GetString(encodedBytes);
                    ltLicInfo.Text = decodedString;
                }
                //生成激活码
                int type = Convert.ToInt32(rblActivType.SelectedValue);
                lbActivCode.Text = getActiveCode(type);     //初始激活码的问题
            }
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {                
                string action = WeiSha.Common.Request.Form["action"].String;
                switch (action)
                {
                    case "code":
                        int type = WeiSha.Common.Request.Form["type"].Int16 ?? 4;
                        string code=getActiveCode(type);
                        Response.Write(code);
                        break;
                    case "refresh":
                        WeiSha.Common.License.Value.Init();
                        break;
                }
                Response.End();
            }
        }
        /// <summary>
        /// 获取激活码
        /// </summary>
        private string getActiveCode(int type)
        {            
            string code = string.Empty;
            if (type == 1) code = WeiSha.Common.Activationcode.CodeForCPU;
            if (type == 2) code = WeiSha.Common.Activationcode.CodeForHardDisk;
            if (type == 3) code = WeiSha.Common.Activationcode.CodeForIP;
            if (type == 4) code = WeiSha.Common.Activationcode.CodeForDomain;       //域名激活码
            if (type == 5) code = WeiSha.Common.Activationcode.CodeForRoot;         //主域激活码
            return code;            
        }
        /// <summary>
        /// 绑定各版本之间的差别数据
        /// </summary>
        protected void BindVersionLevel()
        {
            DataTable dt = WeiSha.Common.Parameters.Authorization.VersionLevel.LevelTable;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 1; j < dt.Columns.Count; j++)
                {
                    int t=-1;
                    int.TryParse(dt.Rows[i][j].ToString(),out t);
                    if (t == 0)
                    {
                        //dt.Rows[i][j] = "∞";
                        dt.Rows[i][j] = "不限";
                    }
                }
            }
            gvLimit.DataSource = dt;
            gvLimit.DataBind();
        }
        /// <summary>
        /// 获取系统的详细介绍，我在这里写了发布日期
        /// 例如：[assembly: AssemblyDescription("ReleaseDate：2016-06-17")]
        /// </summary>
        /// <returns></returns>
        protected string Description()
        {
            // 获取此程序集上的所有 Title 属性  
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            string desc = string.Empty;
            // 如果至少有一个 Title 属性  
            if (attributes.Length > 0)
            {
                // 请选择第一个属性  
                AssemblyDescriptionAttribute titleAttribute = (AssemblyDescriptionAttribute)attributes[0];
                // 如果该属性为非空字符串，则将其返回  
                if (titleAttribute.Description != "")
                    desc = titleAttribute.Description;
            }
            return desc;
        }
        /// <summary>
        /// 获取系统AssemblyInfo中的文件名称
        /// </summary>
        /// <returns></returns>
        protected string AssemblyTitle()
        {
            // 获取此程序集上的所有 Title 属性  
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), false);
            string title = string.Empty;
            // 如果至少有一个 Title 属性  
            if (attributes.Length > 0)
            {
                // 请选择第一个属性  
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                // 如果该属性为非空字符串，则将其返回  
                if (titleAttribute.Title != "")
                    title = titleAttribute.Title;
            }
            return title;
        }
    }
}