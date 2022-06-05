using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using System.Reflection;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// 学员能够学习当前课程，才能使用的方法
    /// </summary>
    public class StudyAttribute : PurviewAttribute
    {
        /// <summary>
        /// 验证是否能通过
        /// </summary>
        /// <param name="method">执行的方法</param>
        /// <param name="letter">请求</param>
        /// <returns></returns>
        public static bool Verify(MemberInfo method, Letter letter)
        {
            StudyAttribute study = StudyAttribute.GetAttr<StudyAttribute>(method);
            if (study == null) return true;

            string msg = string.Format("当前方法 {0}.{1} 需要", method.DeclaringType.Name, method.Name);
            //如果未登录，则直接返回false
            Song.Entities.Accounts acc = LoginAccount.Status.User();
            if (acc == null)
            {
                throw new Exception(msg + "学员账户登录后操作");
            }
            //获取章节或课程id
            int couid = 0, olid = 0;
            couid = letter["couid"].Int32 ?? 0;
            olid = letter["olid"].Int32 ?? 0;
            Song.Entities.Outline outline = null;
            if (couid <= 0)
            {
                //如果课程id不存在，通过章节id取课程
                outline = Business.Do<IOutline>().OutlineSingle(olid);
                if (outline != null) couid = outline.Cou_ID;
            }
            if (couid == 0 || olid == 0) throw new Exception(msg + "课程id或章节id不得为空");
            //判断
            Song.Entities.Course couBuy = Business.Do<ICourse>().IsBuyCourse(couid, acc.Ac_ID, 1);
            //如果已经购买，则可以学习
            if (couBuy != null) return true;
            Song.Entities.Course cour = Business.Do<ICourse>().CourseSingle(couid);
            if (cour == null) throw new Exception("课程不存在！");
            if (!cour.Cou_IsUse) throw new Exception("课程被禁止学习！");
            //是否免费，或是限时免费
            if (cour.Cou_IsLimitFree)
            {
                DateTime freeEnd = cour.Cou_FreeEnd.AddDays(1).Date;
                if (!(cour.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                    cour.Cou_IsLimitFree = false;
            }
            if (cour.Cou_IsFree || cour.Cou_IsLimitFree) return true;
            //如果是试用，相关章节的判断
            if (cour.Cou_IsTry)
            {
                if (outline == null && olid > 0) outline = Business.Do<IOutline>().OutlineSingle(olid);
                if (outline == null) throw new Exception("课程或章节不存在！");
                if (!outline.Ol_IsUse || !outline.Ol_IsFinish) return false;
                if (outline.Ol_IsFree) return true;
            }
            throw new Exception("当前内容不可以学习");
            //return false;
        }       
    }
}
