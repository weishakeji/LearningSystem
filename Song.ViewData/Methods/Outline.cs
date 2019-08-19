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
    public class Outline
    {
        /// <summary>
        /// 章节列表
        /// </summary>
        /// <param name="couid">所属课程的id</param>
        /// <param name="pid">上级id</param>
        /// <returns></returns>
        [HttpGet(IsAllow = false)]
        public Song.Entities.Outline[] List(int couid, int pid)
        {
            return Business.Do<IOutline>().OutlineCount(couid, pid, true, 0);
        }
        /// <summary>
        /// 树形章节列表
        /// </summary>
        /// <param name="couid">所属课程的id</param>
        /// <returns></returns>
        public DataTable Tree(int couid)
        {
            // 当前课程的所有章节            
            Song.Entities.Outline[] outlines = Business.Do<IOutline>().OutlineAll(couid, true);
            //树形章节输出
            DataTable dt = Business.Do<IOutline>().OutlineTree(outlines);
            return dt;      
        }
    }
}
