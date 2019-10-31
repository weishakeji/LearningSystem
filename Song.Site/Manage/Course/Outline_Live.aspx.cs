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
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;


namespace Song.Site.Manage.Course
{
    public partial class Outline_Live : Extend.CustomPage
    {
        //章节ID
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)            
                initBind();   
        }
        /// <summary>
        /// 附件绑定
        /// </summary>
        protected void initBind()
        {
            Song.Entities.Outline mm =  Business.Do<IOutline>().OutlineSingle(id);
            //是否为直播课
            cbIsLive.Checked = mm.Ol_IsLive;
            tbLiveTime.Text = mm.Ol_LiveTime < DateTime.Now.AddYears(-100) ? "" : mm.Ol_LiveTime.ToString("yyyy-MM-dd HH:mm");
            tbLiveSpan.Text = mm.Ol_LiveSpan == 0 ? "" : mm.Ol_LiveSpan.ToString();
            //直播流地址
            pili_sdk.pili.Stream stream = null;
            try
            {
                stream = Business.Do<ILive>().StreamGet(mm.Ol_LiveID);
                if (stream != null)
                {
                    //推流地址
                    ltPublish.Text = string.Format("rtmp://{0}/{1}/{2}", stream.PublishRtmpHost, stream.HubName, stream.Title);
                    //直播地址
                    string proto = Business.Do<ILive>().GetProtocol;    //协议，http还是https
                    ltPlayHls.Text = string.Format("{0}://{1}/{2}/{3}.m3u8", proto, stream.LiveHlsHost, stream.HubName, stream.Title);
                    ltPlayRtmp.Text = string.Format("rtmp://{0}/{1}/{2}", stream.LiveRtmpHost, stream.HubName, stream.Title);
                }
            }
            catch (Exception ex)
            {
                panelError.Visible = true;
                lbError.Text = "直播调用发生异常：" + ex.Message;
            }            

        }

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Outline ol = Business.Do<IOutline>().OutlineSingle(id);
            //是否为直播
            ol.Ol_IsLive = cbIsLive.Checked;
            DateTime timeLive = DateTime.Now;   //直播开始时间
            DateTime.TryParse(tbLiveTime.Text, out timeLive);
            ol.Ol_LiveTime = timeLive;  //
            int liveSpan = 0;       //直播计划时长
            int.TryParse(tbLiveSpan.Text, out liveSpan);
            ol.Ol_LiveSpan = liveSpan;
            try
            {

                Business.Do<IOutline>().OutlineSave(ol);

                Master.AlertCloseAndRefresh("操作完成");
            }
            catch
            {
                throw;
            }
        }    
        
    }
}
