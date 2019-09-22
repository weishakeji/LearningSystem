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
    public class Subject : IViewAPI
    {
        [HttpGet(IsAllow = false)]
        public Song.Entities.Subject ForID(int id)
        {
            return Business.Do<ISubject>().SubjectSingle(id);
        }
        
    }
}
