using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Common;
using System.Data;


namespace Song.ViewData.Methods
{
    /// <summary>
    /// 专业管理
    /// </summary>
    [HttpGet]
    public class Subject : IViewAPI
    {
        /// <summary>
        /// 通过专业ID，获取专业信息
        /// </summary>
        /// <param name="id">专业id</param>
        /// <returns></returns>
        public Song.Entities.Subject ForID(int id)
        {
            return Business.Do<ISubject>().SubjectSingle(id);
        }
        /// <summary>
        /// 某个机构下的专业
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns>专业列表</returns>
        public Song.Entities.Subject[] List(int orgid)
        {
            return Business.Do<ISubject>().SubjectCount(orgid, string.Empty, true, -1, -1);
        }
        /// <summary>
        /// 某个机构下的专业
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns>专业列表</returns>
        public DataTable Tree(int orgid)
        {
            Song.Entities.Subject[] sbjs= Business.Do<ISubject>().SubjectCount(orgid, string.Empty, true, -1, -1);
            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(sbjs);
            WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            tree.IdKeyName = "Sbj_ID";
            tree.ParentIdKeyName = "Sbj_PID";
            tree.TaxKeyName = "Sbj_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);

            return dt;
        }
    }
}
