using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Check
{
    public partial class DB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 测试数据库链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDblink_Click(object sender, EventArgs e)
        {
            _isCorrect();
        }
        /// <summary>
        /// 检测数据库是否完整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDbTable_Click(object sender, EventArgs e)
        {

            if (!_isCorrect()) return;
            //
            List<string> error = Business.Do<ISystemPara>().DatabaseCompleteTest();
            if (error == null)
            {
                lbShowText.Text = "数据库完整！";
            }
            else
            {
                lbShowText.Text = "数据库结构不完整，请查看以下缺失情况：<br/>";
                foreach (string s in error)
                {
                    lbShowText.Text += s + "<br/>";
                }
            }
            
        }
        /// <summary>
        /// 数据库链接是否正确
        /// </summary>
        /// <returns></returns>
        private bool _isCorrect()
        {
            bool isCorrect = Business.Do<ISystemPara>().DatabaseLinkTest();
            if (isCorrect)
            {
                lbShowText.Text = "数据库链接正确";
                plCorrectShow.Visible = false;
            }
            else
            {
                lbShowText.Text = "";
                plCorrectShow.Visible = true;
            }
            return isCorrect;
        }
    }
}