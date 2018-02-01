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
using WeiSha.WebControl;

namespace Song.Site.Manage.Sys
{
    public partial class MenuBuilder :Extend.CustomPage
    {
        //存放路径
        private string _staticPath = "~/Static/";
        //管理界面左侧功能菜单的静态化数据
        //private string _consMenu = "ConsoleMenuTree.html";
        //权限设置中，树形菜单的静态化数据
        private string _purMenu = "PurviewMenuTree.html";
        //系统菜单的静态化数据
        private string _sysMenu = "SysMenuTree.html";
        //系统菜单面板Z轴
        private int _sysZIndex = 4000;
        //菜单总数据源
        Song.Entities.ManageMenu[] _allMM;
        //小图标路径
        private string empty = "<img src=\"/Manage/Images/tree/empty.gif\"/>";
        private string line = "<img src=\"/Manage/Images/tree/line.gif\"/>";
        //
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region 生成管理界面左侧菜单
        //protected void btnConsoleTree_Click(object sender, EventArgs e)
        //{
        //    _allMM = Business.Do<IManageMenu>().GetTree("func",true, true);
        //    //生成左侧标题部分，也就是一级菜单
        //    string tm = "<div type=\"titlePanel\" class=\"titlePanel\">";
        //    for (int i = 0; i < _allMM.Length;i++ )
        //    {
        //        ManageMenu m = _allMM[i];
        //        if (m.MM_PatId == 0)
        //        {
        //            string title = m.MM_Intro.Trim() == "" ? m.MM_Name : m.MM_Intro;
        //            string style = i == 0 ? "current" : "out";
        //            tm += "<div id=\"consTreeTitle_" + m.MM_Id + "\"  class=\"" + style + "\" nodeId=\"" + m.MM_Id + "\"  title=\"" + title + "\" ><SPAN>" + m.MM_Name + "</SPAN></div>";
        //        }
        //    } 
        //    tm += "</div>";
        //    //生成标题结束
        //    tm += _ConsLevel2Menu();
        //    //创建静态文件
        //    System.IO.StreamWriter sw = new System.IO.StreamWriter(Server.MapPath(this._staticPath)+this._consMenu,false,System.Text.Encoding.UTF8);
        //    sw.Write(tm);
        //    sw.Flush();
        //    sw.Close();
        //    sw.Dispose();
        //    //生成静态文件
        //}
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
                        string title = t.MM_Intro.Trim() == "" ? t.MM_Name : t.MM_Intro;
                        //二级菜单条
                        tm += "<div class=\"menuBar\" type=\"menuBar\" title=\"" + title + "\" IsChilds=\"" + node2.IsChilds + "\" nodeId=\"" + t.MM_Id + "\" patId=\"" + m.MM_Id + "\" tax=\"" + t.MM_Tax + "\">";
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
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            return tm;
        }
        /// <summary>
        /// 生成三级菜单的树形，也就是无限级
        /// </summary>
        /// <param name="single">当前菜单节点对象</param>
        /// <param name="path">当前节点上朔至根节点的路径</param>
        /// <returns></returns>
        private string _ConsLevel3Menu(ManageMenu single, string path,int topid)
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
                    string title = m.MM_Intro.Trim() == "" ? m.MM_Name : m.MM_Intro;
                    //节点项
                    tm += "<div class=\"nodeline\" type=\"nodeline\" nodeId=\"" + m.MM_Id + "\" >";
                    //节点前的图片（连线与标记）
                    tm += "<div class=\"nodeIco\" type=\"nodeIco\" IsChilds=\"" + node1.IsChilds + "\">";
                    tm += nodeLine(m, topid) + nodeIco(m);
                    tm += "</div>";
                    //节点名称，记得删除样式
                    tm += "<div title=\"" + title + "\" href=\"" + m.MM_Link + "\" class=\"text\" type=\"text\" nodeId=\"" + m.MM_Id + "\" nodeType=\"item\" IsChilds=\"" + node1.IsChilds + "\" patId=\"" + m.MM_PatId + "\" tax=\"" + m.MM_Tax + "\" path=\"" + (path + "," + m.MM_Name) + "\">";
                    tm += _BuildNode(m);
                    tm += "</div>";
                    //
                    tm += "</div>";
                    tm += _ConsLevel3Menu(m, path + "," + m.MM_Name, topid);

                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
             //tm += "</DIV>";
            tm += "</DIV>";
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
                if (m.MM_Link != "")
                {
                    temp += "<a href=\"" + m.MM_Link + "\"  target=\"_blank\">" + name + "</a>";
                }
                else
                {
                    temp += name;
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
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
        private string nodeLine(ManageMenu m,int topid)
        {
	        string temp="";
            try
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
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            
	        return temp;
        }
        private string nodeIco(ManageMenu m)
        {
            string temp = "";
            try
            {
                Extend.MenuNode n = new Song.Extend.MenuNode(m, _allMM);
                if (n.IsChilds && n.IsLast) temp += "<img src=\"/Manage/Images/tree/minusBottom.gif\"/>" + "<img src=\"/Manage/Images/tree/folderOpen.gif\"/>";
                if (n.IsChilds && !n.IsLast) temp += "<img src=\"/Manage/Images/tree/minus.gif\"/>" + "<img src=\"/Manage/Images/tree/folderOpen.gif\"/>";
                if (!n.IsChilds && n.IsLast) temp += "<img src=\"/Manage/Images/tree/joinBottom.gif\"/>" + "<img src=\"/Manage/Images/tree/page.gif\"/>";
                if (!n.IsChilds && !n.IsLast) temp += "<img src=\"/Manage/Images/tree/join.gif\"/>" + "<img src=\"/Manage/Images/tree/page.gif\"/>";
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            return temp.ToLower();
        }
        #endregion

        #region 生成权限菜单
        protected void btnPurTree_Click(object sender, EventArgs e)
        {
            System.IO.StreamWriter sw = null;
            try
            {
                _allMM = Business.Do<IManageMenu>().GetTree("func", true, true);
                //生成左侧标题部分，也就是一级菜单
                string tm = "";
                Extend.MenuNode top = new Song.Extend.MenuNode(null, _allMM);
                //生成每个菜单树的外框
                foreach (Song.Entities.ManageMenu m in top.Childs)
                {
                    tm += "<DIV class=\"TreeBox\">";
                    //生成菜单树
                    tm += this._PurBuidTree(m);
                    tm += "</DIV>";
                }
                //创建静态文件
                sw = new System.IO.StreamWriter(Server.MapPath(this._staticPath) + this._purMenu, false, System.Text.Encoding.UTF8);
                sw.Write(tm);
                sw.Flush();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            finally
            {
                sw.Close();
                sw.Dispose();
            }
            //生成静态文件
        }
        private string _PurBuidTree(Song.Entities.ManageMenu m)
        {
            //开始生成
            string temp = "";
            try
            {
                //当前节点对象
                Extend.MenuNode n = new Song.Extend.MenuNode(m, _allMM);
                temp += "<div type=\"nodeline\" class=\"nodeline\">";
                //节点前的图标区域//树的连线与图标
                temp += "<div style='width:auto;float:left;' state='" + m.MM_State + "' ";
                temp += "type='nodeIco' ";
                temp += "IsChilds='" + (n.IsChilds ? "True" : "False") + "'>";
                temp += this._PurNodeLine(m, 0) + this._PurNodeIco(m);
                temp += "</div>";
                //菜单项文本
                temp += this._PurBuildNode(m);
                temp += "</div>";
                if (n.IsChilds)
                {
                    temp += "<div style=\"float: none;\" type=\"nodePanel\" panelId=\"" + m.MM_Id + "\">";
                    for (int i = 0; i < n.Childs.Length; i++)
                    {
                        temp += this._PurBuidTree(n.Childs[i]);
                    }
                    temp += "</div>";
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }        
            return temp;
        }
        /// <summary>
        /// 生成权限菜单的节点文件项
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private string _PurBuildNode(Song.Entities.ManageMenu m)
        {
            string temp = "";
            try
            {
                Extend.MenuNode node = new Song.Extend.MenuNode(m, _allMM);
                temp += "<div style='width:auto;float:left;' state=\"nosel\" type=\"select\" nodeId=\"" + m.MM_Id + "\" ";
                temp += "IsChilds='" + (node.IsChilds ? "True" : "False") + "'>";
                temp += "<img src=\"/Manage/Images/tree/noSel.gif\"/>";
                temp += "</div>";
                temp += "<div style=\"width:auto;line-height: 18px;display: table;font-size: 12px;cursor: default;\"";
                string title = m.MM_Intro.Trim() == "" ? m.MM_Name : m.MM_Intro;
                temp += "title='" + title + "'";
                temp += " nodeId='" + m.MM_Id + "' text=\"" + m.MM_Name + "\"";
                temp += " tax='" + m.MM_Tax + "' patId=\"" + m.MM_PatId + "\" ";
                temp += " type='text'>";
                //菜单节点的自定义样式
                string style = "font-size: 13px; ";
                if (m.MM_Color != String.Empty && m.MM_Color != null) style += "color: " + m.MM_Color + ";";
                if (m.MM_IsBold) style += "font-weight: bold;";
                if (m.MM_IsItalic) style += "font-style: italic;";
                if (m.MM_Font != String.Empty && m.MM_Font != null) style += "font-family: '" + m.MM_Font + "';";
                string name = "<span style=\"" + style + "\">" + m.MM_Name + "</span>";
                temp += name;
                //如果当前节点显示状态为false
                if (!m.MM_IsShow)
                {
                    temp += "  <span style=\"color:red\" title=\"该节点项在使用中将不显示\">[隐]</span>";
                }
                if (!m.MM_IsUse)
                {
                    temp += "  <span style=\"color:red\" title=\"菜单项被禁用；&#10;具体作用：&#10;在使用中将不响应鼠标事件\">[禁]</span>";
                }
                temp += "</div>";
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            return temp;
        }
        private string _PurNodeIco(ManageMenu m)
        {
            string temp = "";
            try
            {
                Extend.MenuNode n = new Song.Extend.MenuNode(m, _allMM);
                //如果是根节点
                if (n.Parent == null) return "<img src=\"/Manage/Images/tree/root.gif\"/>";
                //如果有子节点，且是最后一个，等判断
                if (n.IsChilds && n.IsLast) temp += "<img src=\"/Manage/Images/tree/minusBottom.gif\"/>";
                if (n.IsChilds && !n.IsLast) temp += "<img src=\"/Manage/Images/tree/minus.gif\"/>";
                if (!n.IsChilds && n.IsLast) temp += "<img src=\"/Manage/Images/tree/joinBottom.gif\"/>";
                if (!n.IsChilds && !n.IsLast) temp += "<img src=\"/Manage/Images/tree/join.gif\"/>";
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            return temp.ToLower();
        }
        /// <summary>
        /// 生成菜单项前的链接线
        /// </summary>
        /// <param name="m">当前节点</param>
        /// <param name="topid">当前节点上溯到最顶节点的id</param>
        /// <returns></returns>
        private string _PurNodeLine(ManageMenu m, int topid)
        {
            string temp = "";
            try
            {
                Extend.MenuNode mn = new Song.Extend.MenuNode(m, _allMM);
                //当前菜单项的上级菜单项
                Extend.MenuNode p = new Song.Extend.MenuNode(mn.Parent, _allMM);
                while (p.Item.MM_Id != topid)
                {
                    //如果是当前子树的最后一个
                    if (p.Item.MM_PatId == 0)
                    {
                        temp = empty + temp;
                        break;
                    }
                    //如果是最后一个节点
                    if (p.IsLast) temp = empty + temp;
                    else temp = line + temp;
                    //temp = empty + temp;
                    p = new Song.Extend.MenuNode(p.Parent, _allMM);
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            return temp;
        }
        #endregion


        #region 生成系统菜单
        protected void btnSysMenu_Click(object sender, EventArgs e)
        {
            System.IO.StreamWriter sw = null;
            try
            {
                _allMM = Business.Do<IManageMenu>().GetTree("sys", true, true);
                string tmp = "";
                //当前根节点
                Extend.MenuNode root = new Extend.MenuNode(null, _allMM);
                //for (int i = 0; i < root.Childs.Length; i++)
                //{
                //    var n = new Node(root.Childs[i], data);
                //    //tmp+=this.BuildNode(n,data,"rootMenu");			
                //}
                if (root.IsChilds)
                {
                    //递归生成子菜单
                    tmp += this._BuildMenuItem(root.Childs[0], 0, root.Childs[0].MM_Name);
                }
                //创建静态文件
                sw = new System.IO.StreamWriter(Server.MapPath(this._staticPath) + this._sysMenu, false, System.Text.Encoding.UTF8);
                sw.Write(tmp);
                sw.Flush();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            finally
            {
                sw.Close();
                sw.Dispose();
            }
        }
        //生成下拉菜单，子菜单面板
        private string _BuildMenuItem(Song.Entities.ManageMenu m,int level,string path)
        {
            Extend.MenuNode node = new Song.Extend.MenuNode(m, _allMM);
	        //如果没有子节点，则直接返回
	        if(!node.IsChilds)return "";	
	        //
            string tmp = "";
            try
            {
                if (level == 0)
                    tmp = "<div style=\"z-index: " + (_sysZIndex++) + ";\"";
                else
                    tmp = "<div style=\"position: absolute; z-index: " + (_sysZIndex++) + ";\"";
                tmp += " patId='" + node.Item.MM_Id + "' ";
                tmp += " class='menuPanel' type='menuPanel' level=\"" + (level++) + "\">";
                for (int i = 0; i < node.Childs.Length; i++)
                {
                    //生成菜单点
                    Song.Entities.ManageMenu n = node.Childs[i];
                    tmp += this._SysBuildNode(n, "MenuNode", path + "," + n.MM_Name);
                    //tmp+="<div>"+n.Name+"</div>";
                }
                tmp += "</div>";
                //递归生成子菜单
                for (int i = 0; i < node.Childs.Length; i++)
                {
                    Song.Entities.ManageMenu n = node.Childs[i];
                    tmp += this._BuildMenuItem(n, level, path + "," + n.MM_Name);
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
	        return tmp;
        }
        //生成节点项文本
        //node:当前节点
        //data:完整数据源
        //clas:当前节点的style
        private string _SysBuildNode(Song.Entities.ManageMenu m,string clas,string path)
        {
            Extend.MenuNode node = new Song.Extend.MenuNode(m, _allMM);
            string temp = "";
            try
            {
                temp = "<div nodeId=\"" + m.MM_Id + "\"";
                temp += " nodetype=\"" + m.MM_Type + "\" ";
                temp += " title='" + (m.MM_Intro == "" ? m.MM_Name : m.MM_Intro) + "'";
                temp += " isChild=\"" + node.IsChilds + "\"  type=\"" + clas + "\" >";
                //菜单节点的自定义样式
                string style = "font-size: 13px; ";
                if (m.MM_Color != String.Empty && m.MM_Color != null) style += "color: " + m.MM_Color + ";";
                if (m.MM_IsBold) style += "font-weight: bold;";
                if (m.MM_IsItalic) style += "font-style: italic;";
                if (m.MM_Font != String.Empty && m.MM_Font != null) style += "font-family: '" + m.MM_Font + "';";
                string name = "<span style=\"" + style + "\">" + m.MM_Name + "</span>";
                if (m.MM_Link != "")
                {
                    //如果是javascript事件
                    if (m.MM_Type.ToLower() == "event")
                    {
                        temp += "<a href=\"javascript:" + m.MM_Link + "\" isChild=\"" + node.IsChilds + "\" IsUse=\"" + m.MM_IsUse + "\" path=\"" + path + "\" target=\"_blank\">" + name + "</a>";
                    }
                    else
                    {
                        temp += "<a href=\"" + m.MM_Link + "\" isChild=\"" + node.IsChilds + "\" IsUse=\"" + m.MM_IsUse + "\" path=\"" + path + "\" target=\"_blank\">" + name + "</a>";
                    }
                }
                else
                {
                    temp += name;
                }
                temp += "</div>";
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
	        return temp;
}
        #endregion

        
    }
}
