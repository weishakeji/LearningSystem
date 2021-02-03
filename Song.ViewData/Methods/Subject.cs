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
    public class Subject : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 通过专业ID，获取专业信息
        /// </summary>
        /// <param name="id">专业id</param>
        /// <returns></returns>
        public Song.Entities.Subject ForID(int id)
        {
            Song.Entities.Subject sbj = Business.Do<ISubject>().SubjectSingle(id);
            return _tran(sbj);
        }
        /// <summary>
        /// 某个机构下的专业
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns>专业列表</returns>
        public Song.Entities.Subject[] List(int orgid)
        {
            Song.Entities.Subject[] sbjs = Business.Do<ISubject>().SubjectCount(orgid, string.Empty, true, -1, -1);
            for (int i = 0; i < sbjs.Length; i++)
            {
                sbjs[i] = _tran(sbjs[i]);
            }
            return sbjs;
        }
        /// <summary>
        /// 某个机构下的专业
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns>专业列表</returns>
        public DataTable Tree(int orgid)
        {
            Song.Entities.Subject[] sbjs= Business.Do<ISubject>().SubjectCount(orgid, string.Empty, true, -1, -1);
            for (int i = 0; i < sbjs.Length; i++)
            {
                sbjs[i] = _tran(sbjs[i]);
            }
            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(sbjs);
            WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            tree.IdKeyName = "Sbj_ID";
            tree.ParentIdKeyName = "Sbj_PID";
            tree.TaxKeyName = "Sbj_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);

            return dt;
        }
        /// <summary>
        /// 供前端显示的专业最顶级分类
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Song.Entities.Subject[] ShowRoot(int orgid, int count)
        {
            Song.Entities.Subject[] sbjs = Business.Do<ISubject>().SubjectCount(orgid, string.Empty, true, 0, count);
            string path = Upload.Get["Subject"].Virtual;
            foreach (Song.Entities.Subject c in sbjs)
            {
                c.Sbj_Logo = path + c.Sbj_Logo;
                c.Sbj_LogoSmall = path + c.Sbj_LogoSmall;
                //如果别名为空，则别名等于专业名称
                if (string.IsNullOrWhiteSpace(c.Sbj_ByName) || c.Sbj_ByName.Trim() == "")
                    c.Sbj_ByName = c.Sbj_Name;
                c.Sbj_Intro = HTML.ClearTag(c.Sbj_Intro);
            }
            return sbjs;
        }
        #region 私有方法，处理对象的关联信息
        /// <summary>
        /// 处理专业信息，图片转为全路径，并生成clone对象
        /// </summary>
        /// <param name="sbj">专业对象的clone</param>
        /// <returns></returns>
        private Song.Entities.Subject _tran(Song.Entities.Subject sbj)
        {
            if (sbj == null) return sbj;
            Song.Entities.Subject clone = sbj.Clone<Song.Entities.Subject>();
            clone.Sbj_Logo = WeiSha.Common.Upload.Get["Subject"].Virtual + clone.Sbj_Logo;
            clone.Sbj_LogoSmall = WeiSha.Common.Upload.Get["Subject"].Virtual + clone.Sbj_LogoSmall;
            return clone;
        }
        #endregion
    }
}
