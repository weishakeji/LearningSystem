using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// 请求的区间限制
    /// </summary>
    public class RangeAttribute : WeishaAttr
    {
        /// <summary>
        /// 验证是否满足特性的限定
        /// </summary>
        /// <param name="method">执行的方法</param>
        /// <param name="letter">请求</param>
        /// <returns></returns>
        public static bool Verify(MemberInfo method, Letter letter)
        {
            List<RangeAttribute> ranges = WeishaAttr.GetAttrs<RangeAttribute>(method);
            if (ranges.Count < 1) return true;
            //任意范围，有此特性则直接跳过
            AnywhereAttribute any = WeishaAttr.GetAttr<AnywhereAttribute>(method);
            if (any != null && !any.Ignore) return true;
            //局域网内访问
            if (letter.Sever.IsIntranetIP)
            {
                IntranetAttribute intranet = WeishaAttr.GetAttr<IntranetAttribute>(method);
                if (intranet != null && !intranet.Ignore) return true;
            }
            //本机访问
            if (letter.Sever.IsLocalIP)
            {
                LocalhostAttribute local = WeishaAttr.GetAttr<LocalhostAttribute>(method);
                if (local != null && !local.Ignore) return true;
            }
            //限制同域
            DomainAttribute domainAttr = WeishaAttr.GetAttr<DomainAttribute>(method);
            if (domainAttr != null && !domainAttr.Ignore)
            {
                string host = letter.Sever.Domain.ToLower();
                if (letter.HTTP_REFERER.ToLower().StartsWith("http://" + host)) return true;
                if (letter.HTTP_REFERER.ToLower().StartsWith("https://" + host)) return true;
            }
            //没有通过，则返回异常
            string msg = string.Empty;
            for (int i = 0; i < ranges.Count; i++)
            {
                if (ranges[i] is LocalhostAttribute) msg += "本机";
                if (ranges[i] is IntranetAttribute) msg += "局域网";
                if (ranges[i] is DomainAttribute) msg += "同域";
                if (i < ranges.Count - 1) msg += ",";
            }
            throw new Exception(string.Format("接口 '{0}/{1}' 仅限{2}访问", method.DeclaringType.Name, method.Name, msg));
        }
        /// <summary>
        /// 将执行结果写入日志
        /// </summary>
        /// <param name="execResult"></param>
        public void LogWrite(object execResult)
        {

        }
    }
}
