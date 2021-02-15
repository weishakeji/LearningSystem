using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Common;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 试题
    /// </summary>
    public class Question : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 当前登录学员的试题练习记录
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id，如果为空，或小于1，则取当前课程</param>
        /// <returns></returns>
        [Student, HttpPost]
        public Song.Entities.LogForStudentQuestions LogForSelf(int couid, int olid)
        {
            Song.Entities.Accounts acc = Song.Extend.LoginState.Accounts.CurrentUser;
            if (olid <= 0) olid = 0;
            Song.Entities.LogForStudentQuestions log = Business.Do<ILogs>().QuestionSingle(acc.Ac_ID, couid, olid);
            return log;
        }
    }
}
