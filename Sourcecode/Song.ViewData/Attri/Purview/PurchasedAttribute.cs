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
    /// 学员购买课程后才能使用的方法，必须有couid的参数
    /// </summary>
    public class PurchasedAttribute : PurviewAttribute
    {
        public static bool Verify(MemberInfo method, Letter letter)
        {
            PurchasedAttribute buy = PurchasedAttribute.GetAttr<PurchasedAttribute>(method);
            if (buy == null) return true;
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
            bool isBuy = IsPurchased(couid, acc.Ac_ID);
            if (!isBuy) throw new Exception("课程未购买！");
            return true;
        }
        /// <summary>
        /// 判断是否购买过课程
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public bool Judge(Letter letter)
        {
            //如果未登录，则直接返回false
            Song.Entities.Accounts acc = LoginAccount.Status.User();
            if (acc == null) return false;
            //
            int couid = 0, olid = 0;
            couid = letter["couid"].Int32 ?? 0;
            olid = letter["olid"].Int32 ?? 0;
            if (couid > 0) return IsPurchased(couid, acc.Ac_ID);
            //如果课程id不存在，通过章节id取课程
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(olid);
            if (outline != null) return IsPurchased(outline.Cou_ID, acc.Ac_ID);

            return false;
        }
        /// <summary>
        /// 判断是否购买过的具体方法
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="acid"></param>
        /// <returns></returns>
        public static bool IsPurchased(int couid, int acid)
        {
            Song.Entities.Course couBuy = Business.Do<ICourse>().IsBuyCourse(couid, acid, 1);
            bool isBuy = couBuy != null;
            return isBuy;
        }
    }
}
