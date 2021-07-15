using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Xml;
using System.Reflection;

using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Manage.SOAP
{
    /// <summary>
    /// ManageMenu 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://yuefan.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class ManageMenu : System.Web.Services.WebService
    {

        #region 主要用于显示时的一些方法
        [WebMethod]
        //functype:菜单按功能分类；sys为系统菜单，func为功能菜单
        public Song.Entities.ManageMenu[] Roots(string functype)
        {
            //获取菜单树的列表，默认取显示状态的
            return Business.Do<IManageMenu>().GetRoot(functype, true);
        }
        [WebMethod]
        public Song.Entities.ManageMenu Root(int id)
        {
            //获取单个菜单树的对象
            return Business.Do<IManageMenu>().GetSingle(id);
        }
        [WebMethod]
        //用于前台显示的系统菜单
        public Song.Entities.ManageMenu[] DropMenu()
        {
            //获取菜单树的列表
            return Business.Do<IManageMenu>().GetTree("sys", true);
        }
        [WebMethod]
        //用于前台显示的功能菜单
        public Song.Entities.ManageMenu[] FuncMenu(int rootId)
        {
            //获取菜单树的所有菜单项
            return Business.Do<IManageMenu>().GetRoot(rootId, true);
        }
        #endregion
        #region 新增方法
        [WebMethod]
        //设置栏目顺序，并返回共显示的所有栏目
        public string Order(string result, int rootid, string type)
        {
            this.SaveTreeOrder(Server.UrlDecode(result), rootid);
            return _getTreeHtml(rootid, type);
        }
        [WebMethod]
        //获取所有可显示的栏目
        public Song.Entities.ManageMenu Menu(int id)
        {
            return Business.Do<IManageMenu>().GetSingle(id);
        }
        [WebMethod]
        //获取单个栏目信息
        public string ManageMenuJson(int id)
        {
            Song.Entities.ManageMenu nc = Business.Do<IManageMenu>().GetSingle(id);
            return nc == null ? "" : nc.ToJson();
        }
        [WebMethod]
        //修改栏目，并返回共显示的所有栏目
        public string Update(string result, int pid, string type)
        {
            this._updateTreeNode(Server.UrlDecode(result));
            return _getTreeHtml(pid, type);
        }
        [WebMethod]
        //添加栏目，并返回共显示的所有栏目
        public string Add(string result, int pid, string type)
        {
            this._updateTreeNode(Server.UrlDecode(result));
            return _getTreeHtml(pid, type);
        }
        [WebMethod]
        //添加栏目，并返回共显示的所有栏目
        public string Del(string result, int pid, string type)
        {
            this.DeleteTreeNode(Server.UrlDecode(result));
            return _getTreeHtml(pid, type);
        }
        #endregion
        /// <summary>
        /// 用于管理菜单树时，获取菜单树
        /// </summary>
        /// <param name="rootId"></param>
        /// <param name="result">用于接收客户端数据</param>
        /// <returns></returns>
        [WebMethod]
        public Song.Entities.ManageMenu[] Tree(int rootId, string result)
        {
            int tmrootid = 0;
            if (result != null && result != "")
            {
                //tmrootid=SaveTreeOrder(Server.UrlDecode(result));
            }
            if (tmrootid != 0)
            {
                rootId = tmrootid;
            }
            //获取菜单树的列表
            return Business.Do<IManageMenu>().GetRoot(rootId, null);
        }

        /// <summary>
        /// 修改菜单树信息
        /// </summary>
        /// <param name="rootId">菜单树id</param>
        /// <param name="nodexml">当前节点的xml，由客户端ajax发送</param>
        /// <returns></returns>
        [WebMethod]
        public Song.Entities.ManageMenu[] UpdateTree(int rootId, string nodexml)
        {
            try
            {
                _updateTreeNode(nodexml);
                //获取菜单树的列表
                return Business.Do<IManageMenu>().GetRoot(rootId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 删除某个节点
        /// </summary>
        /// <param name="rootId"></param>
        /// <param name="nodexml"></param>
        /// <returns></returns>
        [WebMethod]
        public Song.Entities.ManageMenu[] DelTree(int rootId, string nodexml)
        {
            DeleteTreeNode(Server.UrlDecode(nodexml));
            //获取菜单树的列表
            return Business.Do<IManageMenu>().GetRoot(rootId);
        }
        /// <summary>
        /// 生成管理菜单的权限编辑树
        /// </summary>
        /// <param name="type">权限类型</param>
        /// <returns></returns>
        [WebMethod]
        public string GetPurViewTree(string type)
        {
            return btnPurTree(type);
        }
        #region 私有方法
        /// <summary>
        /// 返回树形菜单的HTML代码
        /// </summary>
        /// <returns></returns>
        private string _getTreeHtml(int rootid, string type)
        {
            Song.Entities.ManageMenu[] mm = Business.Do<IManageMenu>().GetAll(rootid, null, null, type);
            WeiSha.WebControl.MenuTree mt = new WeiSha.WebControl.MenuTree();
            mt.Title = type == "sys" ? "系统菜单" : "功能菜单";
            mt.Root = rootid;
            mt.DataTextField = "MM_Name";
            mt.IdKeyName = "MM_Id";
            mt.ParentIdKeyName = "MM_PatId";
            mt.TaxKeyName = "MM_Tax";
            mt.SourcePath = "/manage/Images/tree";
            mt.TypeKeyName = "";
            mt.IsUseKeyName = "MM_IsUse";
            mt.IsShowKeyName = "MM_IsShow";
            mt.DataSource = mm;
            mt.DataBind();
            return mt.HTML;
        }
        /// <summary>
        /// 保存菜单树的顺序状态
        /// </summary>
        /// <param name="result"></param>
        private int SaveTreeOrder(string result, int rootid)
        {
            if (result == "" || result == null) return 0;
            Business.Do<IManageMenu>().SaveOrder(result);                 
            return rootid;           
        }
        /// <summary>
        /// 更改菜单项的信息
        /// </summary>
        /// <param name="nodexml"></param>
        private void _updateTreeNode(string nodexml)
        {
            if (nodexml == "" || nodexml == null) return;
            XmlDocument resXml = new XmlDocument();
            resXml.LoadXml(nodexml, false);
            XmlNode node = resXml.SelectSingleNode("node");
            string type = ((XmlElement)node).Attributes["type"].Value;
            int id = Convert.ToInt32(((XmlElement)node).Attributes["id"].Value);
            int rootid = Convert.ToInt32(((XmlElement)node).Attributes["rootid"].Value);
            //当前对象
            Song.Entities.ManageMenu mm = null;
            if (type == "edit" || type == "root") mm = Business.Do<IManageMenu>().GetSingle(id);
            if (type == "add") mm = new Song.Entities.ManageMenu();
            //不管是新增还是修改（根节点、子节点），都需要用到的赋值
            mm.MM_Name = getNodeText(node, "name", "").ToString();
            mm.MM_WinWidth = Convert.ToInt32(getNodeText(node, "winwidth", 400));
            mm.MM_WinHeight = Convert.ToInt32(getNodeText(node, "winheight", 300));
            mm.MM_Func = getNodeText(node, "func", "").ToString();
            mm.MM_IsBold = Convert.ToBoolean(getNodeText(node, "isbold", false));
            mm.MM_IsItalic = Convert.ToBoolean(getNodeText(node, "isitalic", false));
            mm.MM_IsShow = Convert.ToBoolean(getNodeText(node, "isshow", true));
            mm.MM_IsUse = Convert.ToBoolean(getNodeText(node, "isuse", true));
            mm.MM_Intro = getNodeText(node, "intro", "").ToString();
            mm.MM_IcoX = (int)Convert.ToDouble(getNodeText(node, "icox", "0"));
            mm.MM_IcoY = (int)Convert.ToDouble(getNodeText(node, "icoy", "0"));
            //修改与新增才用到的赋值
            if (type == "edit" || type == "add")
            {
                mm.MM_Type = getNodeText(node, "type", "").ToString();
                mm.MM_Link = getNodeText(node, "link", "").ToString();
                //
                mm.MM_Marker = getNodeText(node, "marker", "").ToString();
                mm.MM_Marker = mm.MM_Marker.Replace("|", ",");
                mm.MM_Marker = mm.MM_Marker.Replace(" ", ",");
                string marker = "";
                if (mm.MM_Marker != "" && mm.MM_Marker != null)
                {
                    foreach (string t in mm.MM_Marker.Split(','))
                        if (t.Trim() != "") marker += t + ",";
                    if (marker.Substring(marker.Length - 1) == ",")
                        marker = marker.Substring(0, marker.Length - 1);
                }
                mm.MM_Marker = marker;
                mm.MM_Func = getNodeText(node, "func", "").ToString();
            }
            //修改根节点
            if (type == "root")
            {
                Business.Do<IManageMenu>().Save(mm);
            }
            if (type == "add")
            {
                mm.MM_Root = rootid;
                mm.MM_PatId = getNodeText(node, "patid", 0).ToString();
                Business.Do<IManageMenu>().Add(mm);
            }
            if (type == "edit")
            {
                if (mm.MM_Func == "sys")
                {
                    Business.Do<IManageMenu>().Save(mm);
                }
                else
                {
                    //移动到
                    int mrootid = Convert.ToInt32(getNodeText(node, "moveto", 0));
                    //复制到
                    int crootid = Convert.ToInt32(getNodeText(node, "copyto", 0));
                    //如果既不移动与不复制
                    if (rootid == mrootid && rootid == crootid) Business.Do<IManageMenu>().Save(mm);
                    //如果移动
                    if (rootid != 0 && rootid != mrootid) Business.Do<IManageMenu>().Move(mm, mrootid);
                    //如果拷贝
                    if (crootid != 0 && rootid != crootid) Business.Do<IManageMenu>().Copy(mm, crootid);             
                }
            }

        }
        /// <summary>
        /// 删除某点节
        /// </summary>
        /// <param name="nodexml"></param>
        private void DeleteTreeNode(string nodexml)
        {
            if (nodexml == "" || nodexml == null) return;
            XmlDocument resXml = new XmlDocument();
            resXml.LoadXml(nodexml, false);
            XmlNode node = resXml.SelectSingleNode("node");
            int id = Convert.ToInt32(((XmlElement)node).Attributes["id"].Value);
            Business.Do<IManageMenu>().Delete(id);
        }
        /// <summary>
        /// 获取节点的内容
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="name">子节点名称</param>
        /// <param name="def">当节点不存时的默认值</param>
        /// <returns></returns>
        private object getNodeText(XmlNode node, string name, object def)
        {
            XmlElement el = (XmlElement)node.SelectSingleNode(name);
            if (el == null)
                return def;
            return this.Server.UrlDecode(el.InnerText);
        }
        #endregion

        #region 生成权限菜单
        //系统菜单面板Z轴
        //private int _sysZIndex = 4000;
        //菜单总数据源
        Song.Entities.ManageMenu[] _allMM;
        //小图标路径
        private string empty = "<img src=\"/Manage/Images/tree/empty.gif\"/>";
        private string line = "<img src=\"/Manage/Images/tree/line.gif\"/>";
        /// <summary>
        /// 生成树
        /// </summary>
        /// <returns></returns>
        protected string btnPurTree(string type)
        {
            //机构的权限
            if (type.Equals("orglevel", StringComparison.CurrentCultureIgnoreCase))
            {
                _allMM = Business.Do<IPurview>().GetOrganPurview();
            }
            else
            {
                if (Extend.LoginState.Admin.IsSuperAdmin)
                {
                    //如果是超级管理员，返回所有可用菜单项
                    _allMM = Business.Do<IManageMenu>().GetTree("func", null, true);
                }
                else
                {
                    if (Extend.LoginState.Admin.IsAdmin)
                    {
                        //如果是机构管理员，返回所有机构的所有菜单项
                        Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                        _allMM = Business.Do<IPurview>().GetAll4Org(org.Org_ID, "organAdmin");
                    }
                    else
                    {
                        //获取功能菜单树的所有菜单项
                        _allMM = Business.Do<IPurview>().GetAll4Emplyee(Extend.LoginState.Admin.CurrentUserId);
                    }
                }
            }
            //生成左侧标题部分，也就是一级菜单
            string tm = "";
            foreach (Song.Entities.ManageMenu m in _allMM)
            {
                if (m.MM_PatId != "0") m.MM_IsShow = true;
            }
            Extend.MenuNode top = new Song.Extend.MenuNode(null, _allMM);
            //生成每个菜单树的外框
            foreach (Song.Entities.ManageMenu m in top.Childs)
            {
                if (m.MM_PatId =="0" && m.MM_IsShow) continue;
                tm += "<DIV class=\"TreeBox\">";
                //生成菜单树
                tm += this._PurBuidTree(m);
                tm += "</DIV>";
            }
            return tm;
        }
        private string _PurBuidTree(Song.Entities.ManageMenu m)
        {
            //当前节点对象
            Extend.MenuNode n = new Song.Extend.MenuNode(m, _allMM);
            //开始生成
            string temp = "";
            temp += "<div type=\"nodeline\" class=\"nodeline\">";
            //节点前的图标区域//树的连线与图标
            temp += "<div style='width:auto;float:left;' state='' ";
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
            return temp;
        }
        private string _PurNodeIco(Song.Entities.ManageMenu m)
        {
            string temp = "";
            Extend.MenuNode n = new Song.Extend.MenuNode(m, _allMM);
            //如果是根节点
            if (n.Parent == null) return "<img src=\"/Manage/Images/tree/root.gif\"/>";
            //如果有子节点，且是最后一个，等判断
            if (n.IsChilds && n.IsLast) temp += "<img src=\"/Manage/Images/tree/minusBottom.gif\"/>";
            if (n.IsChilds && !n.IsLast) temp += "<img src=\"/Manage/Images/tree/minus.gif\"/>";
            if (!n.IsChilds && n.IsLast) temp += "<img src=\"/Manage/Images/tree/joinBottom.gif\"/>";
            if (!n.IsChilds && !n.IsLast) temp += "<img src=\"/Manage/Images/tree/join.gif\"/>";
            return temp.ToLower();
        }
        /// <summary>
        /// 生成菜单项前的链接线
        /// </summary>
        /// <param name="m">当前节点</param>
        /// <param name="topid">当前节点上溯到最顶节点的id</param>
        /// <returns></returns>
        private string _PurNodeLine(Song.Entities.ManageMenu m, int topid)
        {
            string temp = "";
            Extend.MenuNode mn = new Song.Extend.MenuNode(m, _allMM);
            //当前菜单项的上级菜单项
            Extend.MenuNode p = new Song.Extend.MenuNode(mn.Parent, _allMM);
            while (p.Item.MM_Id != topid)
            {
                //如果是当前子树的最后一个
                if (p.Item.MM_PatId == "0")
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

            return temp;
        }
        #endregion
    }
}
