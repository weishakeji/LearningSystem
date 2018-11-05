using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using Song.Entities;
namespace Song.Site.Mobile
{
    /// <summary>
    /// 课程留言板列表页
    /// </summary>
    public class MsgBoards : BasePage
    {
        //当前课程id
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //当前课程
        Song.Entities.Course currCourse = null;
        //状态
        private string state = WeiSha.Common.Request.QueryString["state"].String;
        protected override void InitPageTemplate(HttpContext context)
        {
            //获取当前课程
            if (couid > 0) currCourse = Business.Do<ICourse>().CourseSingle(couid);
            if (currCourse != null) couid = currCourse.Cou_ID;
            this.Document.SetValue("state", state);  
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                switch (action)
                {
                    case "submit":
                        submit();
                        break;
                    case "list":
                        list();
                        break;
                }
                Response.End();
            }
        }
        /// <summary>
        /// 接收提交的信息
        /// </summary>
        private void submit()
        {
            string msg = WeiSha.Common.Request.Form["msg"].String;  //提交的信息
            string imgCode = WeiSha.Common.Request.Cookies["accmsgcode"].ParaValue;     //取图片验证码
            string vcode = WeiSha.Common.Request.Form["vcode"].MD5;                 //取工输入的验证码
            //如果验证不通过
            if (imgCode != vcode)
            {
                Response.Write("-1");
            }
            else
            {
                //添加留言
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    Song.Entities.Accounts student = this.Account;
                    if (student != null)
                    {
                        Song.Entities.MessageBoard mb = new MessageBoard();
                        mb.Ac_ID = student.Ac_ID;
                        mb.Ac_Name = student.Ac_Name;
                        mb.Ac_Photo = student.Ac_Photo;
                        mb.Mb_Content = msg;
                        mb.Cou_ID = couid;
                        mb.Mb_IsTheme = true;
                        Business.Do<IMessageBoard>().ThemeAdd(mb);
                        Response.Write(1);
                    }
                }
            }
            Response.End();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        private void list()
        {
            int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
            int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页
    
            int sumcount = 0;
            Song.Entities.MessageBoard[] msgBoards = Business.Do<IMessageBoard>().ThemePager(Organ.Org_ID, couid, false, true, "", size, index, out sumcount);
            //冒泡排序
            Song.Entities.MessageBoard temp = null;
            for (int i = 0; i < msgBoards.Length; i++)
            {
                for (int j = i + 1; j < msgBoards.Length; j++)
                {
                    if (msgBoards[j].Mb_CrtTime > msgBoards[i].Mb_CrtTime)
                    {
                        temp = msgBoards[j];
                        msgBoards[j] = msgBoards[i];
                        msgBoards[i] = temp;
                    }
                }
            }
            string json = "{\"size\":" + size + ",\"index\":" + index + ",\"sumcount\":" + sumcount + ",";
            json += "\"items\":[";
            for (int n = 0; n < msgBoards.Length; n++)
            {
                Song.Entities.MessageBoard mb = msgBoards[n];
                mb.Mb_Content = WeiSha.Common.HTML.ClearTag(mb.Mb_Content);
                if (string.IsNullOrWhiteSpace(mb.Mb_Answer))
                    mb.Mb_IsAns = false;
                
                if (!mb.Mb_IsAns || string.IsNullOrWhiteSpace(mb.Mb_Answer) || mb.Mb_Answer.Trim()=="")
                {
                    json += mb.ToJson() + ",";
                    continue;
                }
                //如果教师进行了回复，则增加教师相关输出
                if (mb.Mb_IsAns)
                {
                    //增加输出项
                    Dictionary<string, object> addParas = new Dictionary<string, object>();
                    Song.Entities.Teacher th = Business.Do<ITeacher>().TeacherSingle(mb.Th_ID);
                    if (th != null)
                    {
                        addParas.Add("Th_Name", th.Th_Name);
                        addParas.Add("Th_Photo", th.Th_Photo);
                        json += mb.ToJson(null,null,addParas) + ",";
                    }
                }
            }
            if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
            json += "]}";
            Response.Write(json);
            Response.End();
        }
    }
}