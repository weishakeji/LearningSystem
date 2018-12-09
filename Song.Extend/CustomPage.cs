using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Song.Extend;
using System.Text.RegularExpressions;
using System.Reflection;
using System.ComponentModel;
using Song.ServiceInterfaces;
using WeiSha.Common;

namespace Song.Extend
{
    public class CustomPage : System.Web.UI.Page
    {
        /// <summary>
        /// 系统版本号
        /// </summary>
        protected static string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        /// <summary>
        /// 常用脚本,如alert等
        /// </summary>
        public Extend.Scripts Scripts
        {
            get
            {
                return new Scripts(this);
            }
        }
        /// <summary>
        /// 获取当前页面的唯一id
        /// </summary>
        /// <returns></returns>
        public string getUID()
        {
            if (ViewState["UID"] != null)
            {
                return ViewState["UID"].ToString();
            }
            ViewState["UID"] = WeiSha.Common.Request.UniqueID();
            return ViewState["UID"].ToString();
        }
        protected override void OnInitComplete(EventArgs e)
        {
            //插入相应的Javascript脚本；
            string scriptPath = "~/Manage/CoreScripts/";
            scriptPath = this.ResolveUrl(scriptPath);
            //脚本集
            string[] scriptFile = new string[] {
                "jquery.js", 
                "GridView.js",
                "Extend.js",
                "PageExt.js",
                "Verify.js",
                "HoldMode.js"               
            };
            Page.Header.Controls.Add(new System.Web.UI.LiteralControl("\r\n"));
            foreach (string file in scriptFile)
            {
                Page.Header.Controls.Add(new System.Web.UI.LiteralControl("<script type=\"text/javascript\" src=\""+scriptPath+file+"?ver=" + version + "\"></script>\r\n"));
            }
            //插入对应的css文件与js文件
            string name = WeiSha.Common.Request.Page.Name;
            if (System.IO.File.Exists(WeiSha.Common.Request.Page.PhysicsPath + "styles/public.css"))
                Page.Header.Controls.Add(new System.Web.UI.LiteralControl("<link href=\"styles/public.css?ver=" + version + "\" type=\"text/css\" rel=\"stylesheet\" />\r\n"));
            //字体库
            Page.Header.Controls.Add(new System.Web.UI.LiteralControl("<link href=\"/Utility/iconfont/iconfont.css?ver=" + version + "\" type=\"text/css\" rel=\"stylesheet\" />\r\n"));
            if (System.IO.File.Exists(WeiSha.Common.Request.Page.PhysicsPath + "styles/"+ name+".css" ))
                Page.Header.Controls.Add(new System.Web.UI.LiteralControl("<link href=\"styles/" + name + ".css?ver=" + version + "\" type=\"text/css\" rel=\"stylesheet\" />\r\n"));
            if (System.IO.File.Exists(WeiSha.Common.Request.Page.PhysicsPath + "scripts/"+name+".js"))
                Page.Header.Controls.Add(new System.Web.UI.LiteralControl("<script type=\"text/javascript\" src=\"scripts/" + name + ".js?ver=" + version + "\"></script>\r\n"));
            //Response.Write(Extend.ManageSession.Session.Name);
            #region 验证是否登录

            //如果处于管理登录状态
            if (LoginState.Admin.IsLogin)
            {
                //重新写入session
                LoginState.Admin.Write();
                //判断权限
                LoginState.Admin.VerifyPurview();
                //记录操作
                if (!this.IsPostBack)
                {
                    bool isWorkLogs = Business.Do<ISystemPara>()["SysIsWorkLogs"].Boolean ?? true;
                    if (Extend.LoginState.Admin.isForRoot)
                        if (isWorkLogs) Business.Do<ILogs>().AddOperateLogs();
                }
            }
            else
            {
                //如果处于学员登录状态
                if (LoginState.Accounts.IsLogin)
                {                    
                   
                    //当前页面所在的功能模块名称；即相对于/Manage文件夹的路径，因为每一个模块在Manage目录下是一个单独目录
                    string module = WeiSha.Common.Request.Page.Module;
                    if (module.ToLower() != "student")
                    {

                        Song.Entities.Teacher th = LoginState.Accounts.Teacher;
                        if (th == null)
                        {
                            Response.Write("未登录，或不具有操作权限。");
                            Response.End();
                        }
                    }
                }
                else
                {
                    Response.Write("未登录，或同一账号在异地登录，当前登录状态被取消。");
                    Response.End();
                }
            }
            //catch (System.Data.DataException ex)
            //{
            //    Message.ExceptionShow(ex);
            //}
            //catch (NBear.Common.ExceptionForNoLogin ex)
            //{
            //    Message.ExceptionShow(ex);
            //}
            //catch (NBear.Common.ExceptionForLicense ex)
            //{
            //    Message.License(ex.Message);
            //}
            //catch (Exception ex)
            //{
            //    Message.ExceptionShow(ex);
            //}
            #endregion
            //this.Form.Attributes.Add("onsubmit", "this.action=document.location.href");
            
            base.OnInitComplete(e);
        }

        /// <summary>
        /// 重新加载当前页面
        /// </summary>
        protected void Reload()
        {
            string url = Request.Path;
            this.Response.Redirect(url);
        }
        /// <summary>
        /// 消息管理
        /// </summary>
        public Message Message
        {
            get { return new Message(this); }
        }
        /// <summary>
        /// 弹出提示
        /// </summary>
        /// <param name="alert"></param>
        public void Alert(string alert)
        {
            alert = alert.Replace("\r","");
            alert = alert.Replace("\n", "");
            new Extend.Scripts(this).Alert(alert);
        }
        /// <summary>
        /// 利用JavaScript显示提示,提示完，关闭窗口，一般用于弹出窗口完成后的提示
        /// </summary>
        /// <param name="say"></param>
        public void AlertAndClose(string say)
        {
            if (!string.IsNullOrWhiteSpace(say) && say.Trim() != "")
            {
                say = say.Replace("\r", "\n");
                say = "<script type=\"text/javascript\">alert(\"" + say + "\");new top.PageBox().Close();</script>";
                //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), "Alert", say, true);
                Page.ClientScript.RegisterStartupScript(typeof(string), "alert", say);
                //Page.ClientScript.RegisterClientScriptBlock(typeof(string), "alert", say); 
            }
            else
            {
                this.Close();
            }
        }
        /// <summary>
        /// 利用JavaScript显示提示,提示完，关闭窗口，一般用于弹出窗口完成后的提示
        /// </summary>
        /// <param name="say"></param>
        public void AlertCloseAndRefresh(string say)
        {
            if (!string.IsNullOrWhiteSpace(say) && say.Trim() != "")
            {
                say = say.Replace("\r", "\n");
                say = "<script type=\"text/javascript\">alert(\"" + say + "\"); window.top.PageBox.CloseAndRefresh(window.name);</script>";
                //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), "Alert", say, true);
                Page.ClientScript.RegisterStartupScript(typeof(string), "alert", say);
            }
            else
            {
                this.Close();
            }
        }
        /// <summary>
        /// 利用JavaScript显示提示,提示完，关闭窗口，
        /// </summary>
        /// <param name="say"></param>
        public void Close(string say)
        {
            if (!string.IsNullOrWhiteSpace(say) && say.Trim() != "")
            {
                say = say.Replace("\r", "\n");
                say = "<script type=\"text/javascript\">alert(\"" + say + "\");new top.PageBox().CloseAndRefresh();</script>";
                //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), "Alert", say, true);
            }
            else
            {
                say = "<script type=\"text/javascript\">new top.PageBox().CloseAndRefresh();</script>";
                //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), "Alert", say, true);
            }
            Page.ClientScript.RegisterStartupScript(typeof(string), "close", say);
        }
        /// <summary>
        /// 关闭窗口，
        /// </summary>
        public void Close()
        {
            string say = "";
            say = "<script type=\"text/javascript\">new top.PageBox().Close();</script>";
            //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), "Alert", say, true);
            Page.ClientScript.RegisterStartupScript(typeof(string), "close", say);

        }
        /// <summary>
        /// 执行js脚本方法
        /// </summary>
        /// <param name="func"></param>
        /// <param name="values"></param>
        public void JsFunction(string func, params string[] values)
        {
            string script = "<script language='JavaScript' type='text/javascript'>{js}</script>";            
            string para = "";
            for (int i = 0; i < values.Length; i++)
            {
                para += "\"" + values[i] + "\"";
                if (i < values.Length-1) para += ",";
            }
            string js = func + "(" + para + ");";
            script = script.Replace("{js}", js);
            if (this == null) return;
            if (!ClientScript.IsStartupScriptRegistered(this.GetType(), "JsFunction"))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "JsFunction", script);
            }
        }
        #region 查询字符串相关
        /// <summary>
        /// 将查询区域的控件转换成query字符串
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        protected string SearchQuery(System.Web.UI.WebControls.Panel panel)
        {
            return new SearchQuery(panel).QueryString();
        }
        protected string SearchQuery()
        {
            return new SearchQuery(this).QueryString();
        }
        /// <summary>
        /// 增加地址的参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string AddPara(string url, string key, object value)
        {
            string query = string.Empty;
            if (url.IndexOf('?') > -1)
            {
                query = url.Substring(url.IndexOf('?') + 1);
                url = url.Substring(0, url.IndexOf('?'));
            }
            if (string.IsNullOrWhiteSpace(query)) return string.Format("{0}?{1}={2}", url, key, value.ToString());
            //参数组
            string[] arr = query.Split('&');
            string tmQuery = string.Empty;
            bool isExist = false;
            for (int i = 0; i < arr.Length;i++ )
            {
                string[] t = arr[i].Split('=');
                if (t.Length < 2) continue;
                if (key.ToLower() == t[0].ToLower())
                {
                    t[1] = value.ToString();
                    isExist = true;
                }
                tmQuery += string.Format("{0}={1}&", t[0], t[1]);
            }
            if (!isExist) tmQuery += string.Format("{0}={1}&", key, value.ToString());
            if (tmQuery.EndsWith("&")) tmQuery = tmQuery.Substring(0, tmQuery.Length - 1);          
            return url + "?" + tmQuery;
        }
        /// <summary>
        /// 用于拼接页面的查询字串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string AddPara(string key, object value)
        {
            return this.AddPara(this.Page.Request.RawUrl, key, value);            
        }
        /// <summary>
        /// 将查询字串绑定到查询控件上
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        protected void SearchBind(System.Web.UI.WebControls.Panel panel)
        {
            new SearchQuery(panel).SearchBind();
        }
        protected void SearchBind()
        {
            new SearchQuery(this).SearchBind();
        }
        #endregion

        #region 将实体的值绑定到控件
        protected void EntityBind(WeiSha.Data.Entity entity)
        {
            if (entity == null) return;
            _entityBind(this, entity);
        }
        protected void EntityBind(System.Web.UI.WebControls.Panel panel,WeiSha.Data.Entity entity)
        {
            if (entity == null) return;
            _entityBind(panel, entity);            
        }
        /// <summary>
        /// 递归设置控件的值
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        protected void _entityBind(System.Web.UI.Control control, WeiSha.Data.Entity entity)
        {
            foreach (Control c in control.Controls)
            {
                if (string.IsNullOrWhiteSpace(c.ID)) continue;
                _entityBindSingle(c, entity);
            }
            foreach (Control c in control.Controls)
                _entityBind(c,entity);
        }
        /// <summary>
        /// 向单个控件绑定
        /// </summary>
        /// <param name="control"></param>
        /// <param name="entity"></param>
        private void _entityBindSingle(System.Web.UI.Control c, WeiSha.Data.Entity entity)
        {
            //遍历实体属性
            Type info = entity.GetType();
            PropertyInfo[] properties = info.GetProperties();
            for (int j = 0; j < properties.Length; j++)
            {
                PropertyInfo pi = properties[j];
                if (c.ID.Equals(pi.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    //当前属性的值
                    object obj = info.GetProperty(pi.Name).GetValue(entity, null);
                    if (obj != null) _controlBindFunc(c, obj);
                    break;
                }                
            }
        }
        /// <summary>
        /// 向控件赋值
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        private void _controlBindFunc(System.Web.UI.Control c, object value)
        {
            //下拉菜单，多选列表，单选列表
            if (c is DropDownList || c is CheckBoxList || c is RadioButtonList)
            {
                ListControl ddl = c as ListControl;
                ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(value.ToString()));
            }
            //输入框
            if (c is TextBox || c is Label || c is Literal)
            {
                ITextControl txt = c as ITextControl;
                if (value == null) return;                
                if (c is Literal)
                {
                    txt.Text = value.ToString();
                    return;
                }
                WebControl wc = c as WebControl;
                //格式化字符
                string fmt = wc.Attributes["Format"] == null ? null : wc.Attributes["Format"];                    
                if (fmt == null)
                {
                    txt.Text = value.ToString();
                    return;
                }                                                               
                if (value is System.DateTime)
                    txt.Text = System.Convert.ToDateTime(value).ToString(fmt);
                if (value is int)
                    txt.Text = System.Convert.ToInt32(value).ToString(fmt);
                if (value is float)
                    txt.Text = System.Convert.ToSingle(value).ToString(fmt);
                if (value is double)
                    txt.Text = System.Convert.ToDouble(value).ToString(fmt); 
                if(value is decimal)
                    txt.Text = System.Convert.ToDecimal(value).ToString(fmt); 
                
            }            
            //单选与多选
            if (c is CheckBox || c is RadioButton)
                (c as CheckBox).Checked = Convert.ToBoolean(value);
        }
        #endregion

        #region 将实体从界面控件中取值回来
        /// <summary>
        /// 给指定实体填充数据
        /// </summary>
        /// <param name="entity"></param>
        protected WeiSha.Data.Entity EntityFill(WeiSha.Data.Entity entity)
        {
            return _entityFill(this, entity);
        }
        private WeiSha.Data.Entity _entityFill(System.Web.UI.Control control, WeiSha.Data.Entity entity)
        {
            foreach (Control c in control.Controls)
            {
                if (string.IsNullOrWhiteSpace(c.ID)) continue;
                //遍历实体属性
                Type info = entity.GetType();
                PropertyInfo[] properties = info.GetProperties();
                for (int j = 0; j < properties.Length; j++)
                {
                    PropertyInfo pi = properties[j];
                    if (pi.Name == c.ID)
                    {
                        entity = _entityFillSingle(c, entity, pi.Name);
                        break;
                    }
                   
                }
            }
            foreach (Control c in control.Controls)
                entity = _entityFill(c, entity);
            return entity;
        }
        private WeiSha.Data.Entity _entityFillSingle(System.Web.UI.Control c, WeiSha.Data.Entity entity, string piName)
        {
            string value = "";
            //下拉菜单，多选列表，单选列表
            if (c is DropDownList || c is CheckBoxList || c is RadioButtonList)
            {
                ListControl ddl = c as ListControl;
                value = ddl.SelectedValue;
            }
            //输入框
            if (c is TextBox)
            {
                TextBox tb = c as TextBox;
                value = tb.Text;
            }
            //单选与多选
            if (c is CheckBox || c is RadioButton)
            {
                CheckBox cb = c as CheckBox;
                value = cb.Checked.ToString();
            }
            //获取值，转的成属性的数据类型，并赋值
            var property = entity.GetType().GetProperty(piName);
            object tm = string.IsNullOrEmpty(value) ? null : WeiSha.Common.DataConvert.ChangeType(value, property.PropertyType);
            property.SetValue(entity,tm , null);
            return entity;
        }
        #endregion

    }
    
}
