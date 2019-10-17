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
        
    }
}
