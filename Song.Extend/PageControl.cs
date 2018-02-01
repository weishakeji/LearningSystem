using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Song.Extend
{
    public class PageControl
    {
        #region 单件对象
        private static readonly PageControl _p = new PageControl();
        /// <summary>
        /// 获取系统参数
        /// </summary>
        public static PageControl Gateway
        {
            get
            {
                return _p;
            }
        }
        private PageControl()
        {
        }
        #endregion
        /// <summary>
        /// 设置区域内的控件，是否为允许状态
        /// </summary>
        /// <param name="container"></param>
        /// <param name="isEnabled"></param>
        public void SetInnnerEnabled(System.Web.UI.Control container,bool isEnabled)
        {
            foreach (Control ctrl in container.Controls)
            {
                if (ctrl is WebControl)
                {
                    ((WebControl)ctrl).Enabled = isEnabled;
                }
                if (ctrl is HtmlControl)
                {
                    ((HtmlControl)ctrl).Disabled = !isEnabled;
                }
            }
        }
        /// <summary>
        /// 设置区域内的控件，是否显示
        /// </summary>
        /// <param name="container"></param>
        /// <param name="isVisible"></param>
        public void SetInnnerVisible(System.Web.UI.Control container, bool isVisible)
        {
            foreach (Control ctrl in container.Controls)
            {
                if (ctrl is WebControl)
                {
                    ((WebControl)ctrl).Visible = isVisible;
                }
                if (ctrl is HtmlControl)
                {
                    ((HtmlControl)ctrl).Visible = isVisible;
                }
            }
        }
    }
}
