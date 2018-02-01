using System;
using System.Collections.Generic;
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

namespace Song.Site.Manage.Utility
{
    public partial class EmplyeeSelectBox : System.Web.UI.UserControl
    {
        #region 属性
        private string _width = "100";
        public string Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }
        private int _height = 200;
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }
        private string _target = "";
        public string Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }
        private string _targetTextBox = "";
        public string TargetTextBox
        {
            get
            {
                return _targetTextBox;
            }
            set
            {
                _targetTextBox = value;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                bind();
            }
            //加载专用js文件与css文件
            this.Page.Header.Controls.Add(new System.Web.UI.LiteralControl("<script type=\"text/javascript\"  src=\"/Manage/Utility/Scripts/EmplyeeSelectBox.js\"></script>"));
            this.Page.Header.Controls.Add(new System.Web.UI.LiteralControl("<link href=\"/Manage/Utility/Style/EmplyeeSelectBox.css\" type=\"text/css\" rel=\"stylesheet\" />"));
        }
         private void bind()
        {
            //输出院系
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            Song.Entities.Depart[] depart = Business.Do<IDepart>().GetAll(orgid, true, true);
            string tmp = this.buildDepart(0, depart);
            ltDepart.Text = tmp;
             //输出岗位
            Song.Entities.Position[] posi = Business.Do<IPosition>().GetAll(orgid,true);
            ltPosi.Text = this.buildPosi(posi);
            //输出工作组
            Song.Entities.EmpGroup[] group = Business.Do<IEmpGroup>().GetAll(orgid,true);
            this.lgGroup.Text = buildGroup(group);
             //输出在线用户
            this.ltOnline.Text = buildOnline();
            
        }
        #region 输出html
        /// <summary>
        /// 用于院系递归输出
        /// </summary>
        /// <param name="dep"></param>
        /// <param name="depart"></param>
        /// <returns></returns>
        private string  buildDepart(int patId,Song.Entities.Depart[] depart)
        {
            bool isChild = false;
            foreach (Song.Entities.Depart d in depart)
            {
                if (d.Dep_PatId == patId)
                {
                    isChild = true;
                    break;
                }
            }
            if (!isChild) return "";
            string tmp = "";
            tmp += "<div type=\"departpanel\" patId=\"" + patId + "\" class=\"panel\">";
            if (patId == 0)
            {
                //顶级节点
                tmp += "<div type=\"allnode\" class=\"nodeline\">";
                //前面的图标
                tmp += "<div nodetype=\"ico\" class=\"ico\">";
                tmp += "<img src=\"/Manage/Images/tree/root.gif\"/>";
                tmp += "</div>";
                //院系名称
                tmp += "<div nodetype=\"text\" class=\"ptext\" nodeid=\"0\">";
                tmp += "全部";
                tmp += "</div>";
                tmp += "</div>";
            }
            foreach (Song.Entities.Depart d in depart)
            {
                if (d.Dep_PatId == patId)
                {
                    tmp += "<div type=\"departnode\" class=\"nodeline\">";
                    //前面的图标
                    tmp += "<div nodetype=\"ico\" class=\"ico\">";
                    tmp += "<img src=\"/Manage/Images/tree/minus.gif\"/>";
                    tmp += "<img src=\"/Manage/Images/tree/folderopen.gif\"/>";
                    tmp += "</div>";
                    //院系名称
                    tmp += "<div nodetype=\"text\" class=\"ptext\" nodeid=\""+d.Dep_Id+"\">";
                    tmp += d.Dep_CnName;
                    tmp += "</div>";
                    tmp += "</div>";
                     //输出员工
                    int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                    Song.Entities.EmpAccount[] ema = Business.Do<IEmployee>().GetAll(orgid,d.Dep_Id,true,"");
                    if (ema != null && ema.Length>0)
                    {
                        tmp += "<div type=\"emplyeepanel\" patId=\"" + d.Dep_Id + "\" class=\"panel\">";
                        foreach (Song.Entities.EmpAccount ea in ema)
                        {
                            tmp += "<div type=\"empnode\" class=\"nodeline\">";
                            //前面的图标
                            tmp += "<div nodetype=\"ico\" class=\"ico\">";
                            tmp += "<img src=\"/Manage/Images/tree/join.gif\"/>";
                            tmp += "<img src=\"/Manage/Images/tree/page.gif\"/>";
                            tmp += "</div>";
                            //员工名称
                            tmp += "<div nodetype=\"text\" class=\"ctext\" nodeid=\"" + ea.Acc_Id + "\">";
                            tmp += ea.Acc_Name;
                            tmp += "</div>";
                            tmp += "</div>";
                        }
                        tmp += "</div>";
                    }
                   
                    tmp += this.buildDepart(d.Dep_Id, depart);
                }
            }
            tmp += "</div>";
            return tmp;
        }
        /// <summary>
        /// 输入岗位（角色）
        /// </summary>
        /// <param name="posi"></param>
        /// <returns></returns>
        private string buildPosi(Song.Entities.Position[] posi)
        {
            string tmp = "";
            tmp += "<div type=\"departpanel\" patId=\"0\" class=\"panel\">";            
            //顶级节点
            tmp += "<div type=\"allnode\" class=\"nodeline\">";
            //前面的图标
            tmp += "<div nodetype=\"ico\" class=\"ico\">";
            tmp += "<img src=\"/Manage/Images/tree/root.gif\"/>";
            tmp += "</div>";
            //院系名称
            tmp += "<div nodetype=\"text\" class=\"ptext\" nodeid=\"0\">";
            tmp += "全部";
            tmp += "</div>";
            tmp += "</div>";            
            foreach (Song.Entities.Position d in posi)
            {
                
                tmp += "<div type=\"departnode\" class=\"nodeline\">";
                //前面的图标
                tmp += "<div nodetype=\"ico\" class=\"ico\">";
                tmp += "<img src=\"/Manage/Images/tree/minus.gif\"/>";
                tmp += "<img src=\"/Manage/Images/tree/folderopen.gif\"/>";
                tmp += "</div>";
                //组名称
                tmp += "<div nodetype=\"text\" class=\"ptext\" nodeid=\"" + d.Posi_Id + "\">";
                tmp += d.Posi_Name;
                tmp += "</div>";
                tmp += "</div>";
                //输出员工
                Song.Entities.EmpAccount[] ema = Business.Do<IPosition>().GetAllEmplyee(d.Posi_Id,true);
                if (ema != null && ema.Length > 0)
                {
                    tmp += "<div type=\"emplyeepanel\" patId=\"" + d.Posi_Id + "\" class=\"panel\">";
                    foreach (Song.Entities.EmpAccount ea in ema)
                    {
                        tmp += "<div type=\"empnode\" class=\"nodeline\">";
                        //前面的图标
                        tmp += "<div nodetype=\"ico\" class=\"ico\">";
                        tmp += "<img src=\"/Manage/Images/tree/join.gif\"/>";
                        tmp += "<img src=\"/Manage/Images/tree/page.gif\"/>";
                        tmp += "</div>";
                        //员工名称
                        tmp += "<div nodetype=\"text\" class=\"ctext\" nodeid=\"" + ea.Acc_Id + "\">";
                        tmp += ea.Acc_Name;
                        tmp += "</div>";
                        tmp += "</div>";
                    }
                    tmp += "</div>";
                }
            }
            tmp += "</div>";
            return tmp;
        }
        /// <summary>
        /// 工作组
        /// </summary>
        /// <param name="posi"></param>
        /// <returns></returns>
        private string buildGroup(Song.Entities.EmpGroup[] group)
        {
            string tmp = "";
            tmp += "<div type=\"departpanel\" patId=\"0\" class=\"panel\">";
            //顶级节点
            tmp += "<div type=\"allnode\" class=\"nodeline\">";
            //前面的图标
            tmp += "<div nodetype=\"ico\" class=\"ico\">";
            tmp += "<img src=\"/Manage/Images/tree/root.gif\"/>";
            tmp += "</div>";
            //院系名称
            tmp += "<div nodetype=\"text\" class=\"ptext\" nodeid=\"0\">";
            tmp += "全部";
            tmp += "</div>";
            tmp += "</div>";
            foreach (Song.Entities.EmpGroup d in group)
            {

                tmp += "<div type=\"departnode\" class=\"nodeline\">";
                //前面的图标
                tmp += "<div nodetype=\"ico\" class=\"ico\">";
                tmp += "<img src=\"/Manage/Images/tree/minus.gif\"/>";
                tmp += "<img src=\"/Manage/Images/tree/folderopen.gif\"/>";
                tmp += "</div>";
                //组名称
                tmp += "<div nodetype=\"text\" class=\"ptext\" nodeid=\"" + d.EGrp_Id + "\">";
                tmp += d.EGrp_Name;
                tmp += "</div>";
                tmp += "</div>";
                //输出员工
                Song.Entities.EmpAccount[] ema = Business.Do<IEmpGroup>().GetAll4Group(d.EGrp_Id, true);
                if (ema != null && ema.Length > 0)
                {
                    tmp += "<div type=\"emplyeepanel\" patId=\"" + d.EGrp_Id + "\" class=\"panel\">";
                    foreach (Song.Entities.EmpAccount ea in ema)
                    {
                        tmp += "<div type=\"empnode\" class=\"nodeline\">";
                        //前面的图标
                        tmp += "<div nodetype=\"ico\" class=\"ico\">";
                        tmp += "<img src=\"/Manage/Images/tree/join.gif\"/>";
                        tmp += "<img src=\"/Manage/Images/tree/page.gif\"/>";
                        tmp += "</div>";
                        //员工名称
                        tmp += "<div nodetype=\"text\" class=\"ctext\" nodeid=\"" + ea.Acc_Id + "\">";
                        tmp += ea.Acc_Name;
                        tmp += "</div>";
                        tmp += "</div>";
                    }
                    tmp += "</div>";
                }
            }
            tmp += "</div>";
            return tmp;
        }
        /// <summary>
        /// 在线用户
        /// </summary>
        /// <returns></returns>
        private string buildOnline()
        {
            string tmp = "";
            tmp += "<div type=\"departpanel\" patId=\"0\" class=\"panel\">";
            //顶级节点
            tmp += "<div type=\"allnode\" class=\"nodeline\">";
            //前面的图标
            tmp += "<div nodetype=\"ico\" class=\"ico\">";
            tmp += "<img src=\"/Manage/Images/tree/root.gif\"/>";
            tmp += "</div>";
            //院系名称
            tmp += "<div nodetype=\"text\" class=\"ptext\" nodeid=\"0\">";
            tmp += "全部";
            tmp += "</div>";
            tmp += "</div>";

            tmp += "<div type=\"departnode\" class=\"nodeline\">";
            //前面的图标
            tmp += "<div nodetype=\"ico\" class=\"ico\">";
            tmp += "<img src=\"/Manage/Images/tree/minus.gif\"/>";
            tmp += "<img src=\"/Manage/Images/tree/folderopen.gif\"/>";
            tmp += "</div>";
            //组名称
            tmp += "<div nodetype=\"text\" class=\"ptext\" nodeid=\"1\">";
            tmp += "在线人员";
            tmp += "</div>";
            tmp += "</div>";
            List<EmpAccount> acc = Extend.LoginState.Admin.OnlineUser;
            //DataTable dt = Extend.LoginState.Admin.GetOnlineUser();
            if (acc.Count > 0)
            {
                tmp += "<div type=\"emplyeepanel\" patId=\"1\" class=\"panel\">";
                foreach(EmpAccount ea in acc)                
                {
                    tmp += "<div type=\"empnode\" class=\"nodeline\">";
                    //前面的图标
                    tmp += "<div nodetype=\"ico\" class=\"ico\">";
                    tmp += "<img src=\"/Manage/Images/tree/join.gif\"/>";
                    tmp += "<img src=\"/Manage/Images/tree/page.gif\"/>";
                    tmp += "</div>";
                    //员工名称
                    tmp += "<div nodetype=\"text\" class=\"ctext\" nodeid=\"" +ea.Acc_Id + "\" title=\"" + ea.Acc_LastTime.ToString() + "\">";
                    tmp += ea.Acc_Name;
                    //tmp += +dr["time"].ToString();
                    tmp += "</div>";
                    tmp += "</div>";
                }
                tmp += "</div>";
            }
            tmp += "</div>";
            return tmp;
        }
        #endregion
    }
}