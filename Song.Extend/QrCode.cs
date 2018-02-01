using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.Common.Param;

namespace Song.Extend
{
    /// <summary>
    /// 生成二维码
    /// </summary>
    public class QrCode
    {
        /// <summary>
        /// 通过实体，生成二维码
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="template">二维码内容模板</param>
        /// <param name="wh">宽高</param>
        /// <param name="qrcodeImgPath">二维码文件路径</param>
        /// <returns></returns>
        public static string Creat4Entity(WeiSha.Data.Entity entity, string template, string qrcodeImgPath, int wh)
        {
            Type info = entity.GetType();
            //获取对象的属性列表
            PropertyInfo[] properties = info.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo pi = properties[i];
                //当前属性的值
                object obj = info.GetProperty(pi.Name).GetValue(entity, null);
                string patt = @"{\#\s*{0}\s*}";
                patt = patt.Replace("{0}", pi.Name);
                Regex re = new Regex(patt, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
                template = re.Replace(template, obj == null ? "" : obj.ToString());
            }
            template = QrCode.tranUrl(template);
            //是否生成中心logo
            bool isCenterImg = Business.Do<ISystemPara>()["IsQrConterImage"].Boolean ?? true;
            string color = Business.Do<ISystemPara>()["QrColor"].String; 
            System.Drawing.Image image=null;
            if (isCenterImg)
            {
                string centerImg = Upload.Get["Org"].Physics + "QrCodeLogo.png";
                image = WeiSha.Common.QrcodeHepler.Encode(template, wh, centerImg, color, null);
            }
            else
            {
                image = WeiSha.Common.QrcodeHepler.Encode(template, wh, color, null);
            }
            image.Save(qrcodeImgPath);
            return qrcodeImgPath;
        }             
        /// <summary>
        /// 处理二维码模板的超链接，将~转为根路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string tranUrl(string url)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganDefault();
            //企业网站域名
            string domain = org.Org_WebSite;
            if (domain != null && domain != "" && domain.Length >= 7)
            {
                if (domain.Substring(0, 7).ToLower() != "http://")
                {
                    domain = "http://" + domain;
                }
            }
            url = url.Replace("~", domain);
            return url;
        }
    }
}
