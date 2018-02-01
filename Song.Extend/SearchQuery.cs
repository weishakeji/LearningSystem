using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace Song.Extend
{
    public class SearchQuery
    {
        private System.Web.UI.Control _control = null;
        private string query;
        public SearchQuery(System.Web.UI.Page page)
        {
            if (page is System.Web.UI.Control)
                _control = page as System.Web.UI.Control;
            if (_control != null)
                query = _control.Page.ClientQueryString;
        }
        public SearchQuery(System.Web.UI.WebControls.Panel panel)
        {
            if (panel is System.Web.UI.Control)
                _control = panel as System.Web.UI.Control;
            if (_control != null)
                query = _control.Page.ClientQueryString;
        }

        #region 获取查询字符串
        /// <summary>
        /// 返回查询字符串
        /// </summary>
        /// <returns></returns>
        public string QueryString(){
            //if (string.IsNullOrWhiteSpace(query)) return "";
            query = _QueryString(_control);
            return query;           
        }
        /// <summary>
        /// 递归生成查询字符串
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private string _QueryString(System.Web.UI.Control control)
        {
             foreach (Control c in control.Controls)
                 query = _setQueryPara(c);
             foreach (Control c in control.Controls)
                 query = _QueryString(c);
             return query;
        }
        /// <summary>
        /// 处理查询字串的参数，如果重复则替换，如果不存在则生成
        /// </summary>
        /// <param name="query"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string _setQueryPara(System.Web.UI.Control c)
        {
            if (string.IsNullOrWhiteSpace(c.ID)) return query;

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
            Regex regex = new Regex(@"(?<key>" + c.ID + @")=(?<value>(|.)[^&]*)", RegexOptions.IgnoreCase);
            if (regex.Match(query).Success)
                query = regex.Replace(query, "$2=" + value);
            else
                query += "&" + c.ID + "=" + value;
            return query;
        }
        #endregion

        #region 绑定数据
        /// <summary>
        /// 将查询字串的信息绑定回控件
        /// </summary>
        public void SearchBind()
        {
            if (string.IsNullOrWhiteSpace(query)) return;
            _SearchBind(_control);              
        }
        /// <summary>
        /// 递归设置控件的值
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private void _SearchBind(System.Web.UI.Control control)
        {
            if (string.IsNullOrWhiteSpace(query)) return;
            foreach (Control c in control.Controls)
                _setControlValue(c);
            foreach (Control c in control.Controls)
                _SearchBind(c);
        }
        /// <summary>
        /// 处理查询字串的参数，如果重复则替换，如果不存在则生成
        /// </summary>
        /// <returns></returns>
        private void _setControlValue(System.Web.UI.Control c)
        {
            if (string.IsNullOrWhiteSpace(c.ID)) return;
            Regex regex = new Regex(@"(?<key>" + c.ID + @")=(?<value>(|.)[^&]*)", RegexOptions.IgnoreCase);
            MatchCollection mc = regex.Matches(query);
            if (mc.Count>0)
            {
                string value = mc[0].Groups["value"].Value.Trim();
                value = _control.Page.Server.UrlDecode(value);
                //下拉菜单，多选列表，单选列表
                if (c is DropDownList || c is CheckBoxList || c is RadioButtonList)
                {
                    ListControl ddl = c as ListControl;
                    ListItem li = ddl.Items.FindByValue(value);
                    if (li != null)
                    {
                        ddl.SelectedIndex = -1;
                        li.Selected = true;
                    }
   
                }
                //输入框
                if (c is TextBox)
                {
                    TextBox tb = c as TextBox;
                    tb.Text = value;
                }
                //单选与多选
                if (c is CheckBox || c is RadioButton)
                {
                    CheckBox cb = c as CheckBox;
                    try
                    {
                        cb.Checked = Convert.ToBoolean(value);
                    }
                    catch
                    {
                    }
                }
            }
        }
        #endregion
    }
}
