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

namespace Song.Site.Manage.Panel
{
    /// <summary>
    /// 系统管理界面的下拉菜单
    /// </summary>
    public partial class SysMenuTree : System.Web.UI.Page
    {
        //菜单总数据源
        Song.Entities.ManageMenu[] _allMM;
        //系统菜单面板Z轴
        private int _sysZIndex = 4000;
        protected void Page_Load(object sender, EventArgs e)
        {
            //获取数据派
            _allMM = Business.Do<IManageMenu>().GetTree("sys", true, true);
            if (_allMM != null)
            {
                Response.Write(_BuildMenu(_allMM));
            }
            Response.End();
        }
        /// <summary>
        /// 生成系统菜单
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
        private string _BuildMenuItem(Song.Entities.ManageMenu m, int level, string path)
        {
            Extend.MenuNode node = new Song.Extend.MenuNode(m, _allMM);
            //如果没有子节点，则直接返回
            if (!node.IsChilds) return "";
            //
            string tmp = "";
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
