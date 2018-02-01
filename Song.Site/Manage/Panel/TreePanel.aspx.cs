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

namespace Song.Site.Manage.Panel
{
    public partial class TreePanel : System.Web.UI.Page
    {
        //菜单总数据源
        Song.Entities.ManageMenu[] _allMM;
        //小图标路径
        private string empty = "<img src=\"/Manage/Images/tree/empty.gif\"/>";
        private string line = "<img src=\"/Manage/Images/tree/line.gif\"/>";
        protected void Page_Load(object sender, EventArgs e)
        {
            _allMM = this.GetPurview();
            //生成左侧标题部分，也就是一级菜单
            string tm = "";
            if (_allMM != null)
            {
                tm = "<div type=\"titlePanel\" class=\"titlePanel\">";
                for (int i = 0; i < _allMM.Length; i++)
                {
                    ManageMenu m = _allMM[i];
                    if (m.MM_PatId == 0)
                    {
                        string title = m.MM_Intro.Trim() == "" ? m.MM_Name : m.MM_Intro;
                        string style = i == 0 ? "current" : "out";
                        tm += "<div id=\"consTreeTitle_" + m.MM_Id + "\"  class=\"" + style + "\" nodeId=\"" + m.MM_Id + "\"  title=\"" + title + "\" ><SPAN>" + m.MM_Name + "</SPAN></div>";
                    }
                }
                tm += "</div>";
                //生成标题结束
                tm += _ConsLevel2Menu();
            }
            else
            {
                tm = "<div class=\"noPur\">";
                tm += "没有管理权限</div>";
            }
            Response.Write(tm);
            Response.End();
        }
        /// <summary>
        /// 员工的所有权限，包括所在院系、所属角色、所在员工组的所有权限,输出菜单id,如“12,34,22”
        /// </summary>
        /// <returns></returns>
        private Song.Entities.ManageMenu[] GetPurview()
        {
            Song.Entities.ManageMenu[] mm = null;
            if (Extend.LoginState.Admin.IsSuperAdmin)
            {
                //如果是超级管理员，返回所有可用菜单项
                mm = Business.Do<IManageMenu>().GetAll(null,true,"func");
            }
            else
            {
                if (Extend.LoginState.Admin.IsAdmin)
                {
                    //如果是机构管理员，返回所有机构的所有菜单项
                    int org = Extend.LoginState.Admin.CurrentUser.Org_ID;
                    mm = Business.Do<IPurview>().GetAll4Org(org);
                }
                else
                {
                    //获取功能菜单树的所有菜单项
                    mm = Business.Do<IPurview>().GetAll4Emplyee(Extend.LoginState.Admin.CurrentUserId);
                }
            }
            return mm;
        }
        private string _ConsLevel2Menu()
        {
            string tm = "";
            try
            {
                //最顶级节点，为空节点；
                Extend.MenuNode node = new Song.Extend.MenuNode(null, _allMM);
                foreach (ManageMenu m in node.Childs)
                {
                    tm += "<DIV id=\"consTreePanel_" + m.MM_Id + "\"  class=\"treepanel\" patId=\"" + m.MM_Id + "\"  style=\"display:none\"> ";
                    Extend.MenuNode node1 = new Song.Extend.MenuNode(m, _allMM);
                    //生成二级菜单
                    foreach (ManageMenu t in node1.Childs)
                    {
                        Extend.MenuNode node2 = new Song.Extend.MenuNode(t, _allMM);
                        string title = string.IsNullOrWhiteSpace(t.MM_Intro) || t.MM_Intro.Trim() == "" ? t.MM_Name : t.MM_Intro;
                        //二级菜单条
                        tm += "<div class=\"menuBar\" type=\"menuBar\" title=\"" + title + "\"  name=\"" + m.MM_Name + "\"  IsChilds=\""
                            + node2.IsChilds + "\" nodeId=\"" + t.MM_Id + "\" patId=\"" + m.MM_Id + "\" tax=\"" + (int)t.MM_Tax + "\">";
                        tm += "  <span>" + t.MM_Name + "</span>";
                        tm += "</div>";
                        //三级菜单，至无限级
                        tm += "<div class=\"treeBox\" type=\"treeBox\" patId=\"" + t.MM_Id + "\">";
                        tm += _ConsLevel3Menu(t, m.MM_Name + "," + t.MM_Name, t.MM_Id);
                        tm += "</div>";

                    }

                    tm += "</DIV>";

                }
            }
            catch
            {
                throw;
            } 
            return tm;
        }
        /// <summary>
        /// 生成三级菜单的树形，也就是无限级
        /// </summary>
        /// <param name="single">当前菜单节点对象</param>
        /// <param name="path">当前节点上朔至根节点的路径</param>
        /// <returns></returns>
        private string _ConsLevel3Menu(ManageMenu single, string path, int topid)
        {
            string tm = "";
            try
            {
                Extend.MenuNode node = new Song.Extend.MenuNode(single, _allMM);
                if (!node.IsChilds) return "";
                tm += "<div class=\"nodePanel\" type=\"nodePanel\" panelId=\"" + single.MM_Id + "\">";
                foreach (ManageMenu m in node.Childs)
                {
                    Extend.MenuNode node1 = new Song.Extend.MenuNode(m, _allMM);
                    string title = string.IsNullOrWhiteSpace(m.MM_Intro) || m.MM_Intro.Trim() == "" ? m.MM_Name : m.MM_Intro;
                    //节点项
                    tm += "<div class=\"nodeline\" type=\"nodeline\" nodeId=\"" + m.MM_Id + "\" >";
                    //节点前的图片（连线与标记）
                    tm += "<div class=\"nodeIco\" type=\"nodeIco\" IsChilds=\"" + node1.IsChilds + "\">";
                    tm += nodeLine(m, topid) + nodeIco(m);
                    tm += "</div>";
                    //节点名称，记得删除样式
                    tm += "<div title=\"" + title + "\" name=\"" + m.MM_Name + "\" href=\"" + m.MM_Link + "\" class=\"text\" type=\"text\" nodeId=\"" + m.MM_Id
                        + "\" nodeType=\"item\" IsChilds=\"" + node1.IsChilds + "\" patId=\"" + m.MM_PatId + "\" tax=\"" + (int)m.MM_Tax
                        + "\" path=\"" + (path + "," + m.MM_Name) + "\">";
                    tm += _BuildNode(m);
                    tm += "</div>";
                    //
                    tm += "</div>";
                    tm += _ConsLevel3Menu(m, path + "," + m.MM_Name, topid);

                }
                //tm += "</DIV>";
                tm += "</DIV>";
            }
            catch (Exception)
            {
                return null;
            } 
            return tm;
        }
        /// <summary>
        /// 生成节点文件项
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private string _BuildNode(Song.Entities.ManageMenu m)
        {
            string temp = "";
            try
            {
                //temp = "<div style=\"width:auto;line-height: 18px;display: table;	cursor: default;\"";
                //temp += " title='" + (m.MM_Intro == "" ? m.MM_Name : m.MM_Intro) + "'";
                //temp += " nodeId='" + m.MM_Id + "' text=\"" + m.MM_Name + "\"";
                ////temp+=" path=\""+node.Path+"\"";
                //temp += " tax='" + m.MM_Tax + "' patId=\"" + m.MM_PatId + "\" >";
                //菜单节点的自定义样式
                string style = "font-size: 13px; ";
                if (m.MM_Color != String.Empty && m.MM_Color != null) style += "color: " + m.MM_Color + ";";
                if (m.MM_IsBold) style += "font-weight: bold;";
                if (m.MM_IsItalic) style += "font-style: italic;";
                if (m.MM_Font != String.Empty && m.MM_Font != null) style += "font-family: '" + m.MM_Font + "';";
                //
                string name = "<span style=\"" + style + "\">" + m.MM_Name + "</span>";
                if (m.MM_Link != string.Empty && m.MM_Link != null && m.MM_Link.Trim() != "")
                {
                    temp += "<a href=\"" + m.MM_Link + "\"  target=\"_blank\">" + name + "</a>";
                }
                else
                {
                    temp += name;
                }
            }
            catch (Exception)
            {
                return null;
            } 
            //temp += "</div>";
            return temp;
        }
        /// <summary>
        /// 生成菜单项前的链接线
        /// </summary>
        /// <param name="m">当前节点</param>
        /// <param name="topid">当前节点上溯到最顶节点的id</param>
        /// <returns></returns>
        private string nodeLine(ManageMenu m, int topid)
        {
            string temp = ""; try
            {
                Extend.MenuNode mn = new Song.Extend.MenuNode(m, _allMM);
                //当前菜单项的上级菜单项
                Extend.MenuNode p = new Song.Extend.MenuNode(mn.Parent, _allMM);
                while (p.Item.MM_Id != topid)
                {
                    //如果是最后一个节点
                    if (p.IsLast) temp = empty + temp;
                    else temp = line + temp;
                    //temp = empty + temp;
                    p = new Song.Extend.MenuNode(p.Parent, _allMM);
                }
            }

            catch (Exception)
            {
                return null;
            } 
            return temp;
        }
        private string nodeIco(ManageMenu m)
        {
            string temp = "";
            Extend.MenuNode n = new Song.Extend.MenuNode(m, _allMM);
            if (n.IsChilds && n.IsLast) temp += "<img src=\"/Manage/Images/tree/minusBottom.gif\"/>" + "<img src=\"/Manage/Images/tree/folderOpen.gif\"/>";
            if (n.IsChilds && !n.IsLast) temp += "<img src=\"/Manage/Images/tree/minus.gif\"/>" + "<img src=\"/Manage/Images/tree/folderOpen.gif\"/>";
            if (!n.IsChilds && n.IsLast) temp += "<img src=\"/Manage/Images/tree/joinBottom.gif\"/>" + "<img src=\"/Manage/Images/tree/page.gif\"/>";
            if (!n.IsChilds && !n.IsLast) temp += "<img src=\"/Manage/Images/tree/join.gif\"/>" + "<img src=\"/Manage/Images/tree/page.gif\"/>";
            return temp.ToLower();
        }
    }
}
