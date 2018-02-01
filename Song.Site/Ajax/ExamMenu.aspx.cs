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

namespace Song.Site.Ajax
{
    /// <summary>
    /// 系统管理界面的下拉菜单
    /// </summary>
    public partial class ExamMenu : System.Web.UI.Page
    {
        //菜单总数据源
        Song.Entities.ManageMenu[] _allMM;
        //根菜单项
        private int root = WeiSha.Common.Request.QueryString["root"].Int32 ?? 441;
        //取几级，小于1取所有
        private int level = WeiSha.Common.Request.QueryString["level"].Int32 ?? 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (level == 1)
            {
                Response.Write(_BuildLevelOne());
            }
            else
            {
                if (Extend.LoginState.Admin.IsAdmin)
                {
                    //如果是超级管理员，返回所有可用菜单项
                    _allMM = Business.Do<IManageMenu>().GetAll(true, true, "func");
                }
                else
                {
                    //获取功能菜单树的所有菜单项
                    _allMM = Business.Do<IPurview>().GetAll4Emplyee(Extend.LoginState.Admin.CurrentUserId);
                }
                if (_allMM != null)
                {
                    Response.Write(_BuildMenu(_allMM));
                }
            }
            Response.End();
        }
        /// <summary>
        /// 仅生成一级菜单，即根菜单
        /// </summary>
        /// <returns></returns>
        private string _BuildLevelOne()
        {
            Song.Entities.ManageMenu[] mm = Business.Do<IManageMenu>().GetChilds(root, true, true);
            string tm = "";
            foreach (Song.Entities.ManageMenu m in mm)
            {
                tm += " <div class=\"rootItem\"><a href=\"" + m.MM_Link + "\" type=\"" + m.MM_Type + "\">" + m.MM_Name + "</a></div>";
            }
            return tm;
        }
        /// <summary>
        /// 生成权限菜单
        /// </summary>
        /// <returns></returns>
        private string _BuildMenu(Song.Entities.ManageMenu[] mm)
        {           
            string tmp = "";
            //当前根节点
            Extend.MenuNode root = new Extend.MenuNode(null, mm);
            if (root.IsChilds)
            {
                //递归生成子菜单
                tmp += this._BuildMenuItem(root.Childs[0], 0, root.Childs[0].MM_Name);
            }
            return tmp;
        }
        //生成下拉菜单，子菜单面板
        private string _BuildMenuItem(Song.Entities.ManageMenu mm, int level, string path)
        {
            Extend.MenuNode node = new Song.Extend.MenuNode(mm, _allMM);
            //如果没有子节点，则直接返回
            if (!node.IsChilds) return "";
            //
            string tmp = "";
            //是否根菜单           
            string itemClass = level == 0 ? "rootItem" : "item";
            string panelClass = level == 0 ? "rootPanel" : "";           
            //一级菜单，即要前台显示的菜单
            for (int i = 0; i < node.Childs.Length; i++)
            {
                Song.Entities.ManageMenu m = node.Childs[i];                
                Extend.MenuNode n = new Song.Extend.MenuNode(m, _allMM);
                tmp += " <div class=\"" + itemClass + " " + (n.IsChilds ? "child" : "") + "\" mid=\"" + m.MM_Id + "\" tax=\"" + i + "\">";
                tmp += "<a href=\"" + m.MM_Link + "\" type=\"" + m.MM_Type + "\">" + m.MM_Name + "</a></div>";
                if (n.IsChilds)
                {
                    tmp += "<div class=\"MenuPanel "+panelClass+"\" style=\"display:none;z-index:"+(level+100)+"\" pid=\"" + m.MM_Id + "\">";
                    tmp += this._BuildMenuItem(m, level+1, path + "," + m.MM_Name);
                    tmp += "</div>";
                }
            }
            return tmp;
        }
        //生成节点项文本
        //node:当前节点
        //data:完整数据源
        //clas:当前节点的style
        private string _SysBuildNode(Song.Entities.ManageMenu m, string clas, string path)
        {
            Extend.MenuNode node = new Song.Extend.MenuNode(m, _allMM);
            string temp = "<div nodeId=\"" + m.MM_Id + "\"";
            temp += " nodetype=\"" + m.MM_Type + "\" ";
            temp += " title='" + (m.MM_Intro == "" ? m.MM_Name : m.MM_Intro) + "'";
            temp += " isChild=\"" + node.IsChilds + "\"  type=\"" + clas + "\" >";
            //菜单节点的自定义样式
            string style = " ";
            if (m.MM_Color != String.Empty && m.MM_Color != null) style += "color: " + m.MM_Color + ";";
            if (m.MM_IsBold) style += "font-weight: bold;";
            if (m.MM_IsItalic) style += "font-style: italic;";
            if (m.MM_Font != String.Empty && m.MM_Font != null) style += "font-family: '" + m.MM_Font + "';";
            string name = "<span style=\"" + style + "\">" + m.MM_Name + "</span>";
            if (m.MM_Link != "")
            {
                string link = "";
                link += "<{0} href=\"";                
                link += m.MM_Link + "\" isChild=\"" + node.IsChilds + "\" IsUse=\"" + m.MM_IsUse
                     + "\" width=\"" + m.MM_WinWidth + "\" height=\""+m.MM_WinHeight
                     + "\" path=\"" + path + "\" target=\"_blank\" class=\"a\">";
                link += name + "</{0}>";
                switch(m.MM_Type.ToLower())
                {
                    case "link":
                        link = string.Format(link, "a");
                        break;
                    default:
                        link = link.Replace("{0}", "span");
                        break;
                }

                temp += link;
            }
            else
            {
                temp += name;
            }
            temp += "</div>";
            return temp;
        }
    }
}
