using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Imaging;

namespace Song.Site.Manage.Utility
{
    /// <summary>
    /// 生成计算公式的验证码？
    /// </summary>
    public partial class CalcImg : System.Web.UI.Page
    {
        //随机字符串的长度
        int _maxResu = WeiSha.Common.Request.QueryString["max"].Int16 ?? 4;
        //字符类型：1为加公式，2为加减，3为四则运算；
        int _type = WeiSha.Common.Request.QueryString["type"].Int16 ?? 2;
        //cookies的名称
        string _name = WeiSha.Common.Request.QueryString["name"].String ?? "default";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Src"></param>
        /// <param name="E"></param>
        protected void Page_Load(Object Src, EventArgs E)
        {
            string opero = this.getOperator(_type);
            int result = 0;
            string formula = this.getFormula(_maxResu, opero, out result);
            //存储随机数
            HttpCookie cookie = new HttpCookie(_name);
            cookie.Value = Md5(result.ToString());
            Response.Cookies.Add(cookie);

            //生成校验码的图片
            ValidateCode(this, formula);           
        }
        /// <summary>
        /// 生成Md5加密码，Md5为不可逆加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="code">Md5加密分16位与32位，此处应设16或32</param>
        /// <returns>返回值为加密后的16位32位字符</returns>
        public static string Md5(string str, int code)
        {
            if (code == 16) //16位MD5加密（取32位加密的9~25字符） 
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
            }
            else//32位加密 
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
            }
        }
        /// <summary>
        /// 生成Md5加密码，Md5为不可逆加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>返回值为加密后的32位字符</returns>
        public static string Md5(string str)
        {
            //16位MD5加密（取32位加密的9~25字符） 
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
        }
        /// <summary>
        /// 获取随机运算符
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string getOperator(int type)
        {
            if (type == 1) return "+";
            string num = "+,-,*,/";
            string[] array = num.Split(',');
            Random rand = new Random();
            if (type == 2)
            {
                return array[rand.Next(2)];
            }
            return array[rand.Next(4) ];
        }
        /// <summary>
        /// 获取计算公式
        /// </summary>
        /// <param name="max"></param>
        /// <param name="operate"></param>
        /// <returns></returns>
        private string getFormula(int max, string operate,out int result)
        {
            Random rand = new Random();
            int n1 = 0;
            int n2 = 0;
            result = 0;
            string formula = "";
            switch (operate)
            {
                case "+":
                    n1 = rand.Next(max);
                    n2 = rand.Next(max - n1);
                    formula = n1 + "+" + n2;
                    result = n1 + n2;
                    break;
                case "-":
                    n1 = rand.Next(max);
                    n2 = rand.Next(n1);
                    formula = n1 + "-" + n2;
                    result = n1 - n2;
                    break;
            }
            return formula + "=?";
        }
        /// <summary>
        /// 生成随机字符串，可以选择长度与类型
        /// </summary>
        /// <param name="VcodeNum">随机字符串的长度</param>
        /// <param name="type">生成的随机数类型，0为数字与大小写字母，1为纯数字，2为纯小字母，3为纯大写字母，4为大小写字母，5数字加小写，6数字加大写，</param>
        /// <returns></returns>
        public static string RndNum(int VcodeNum, int type)
        {
            string Vchar;
            //数字串
            string num = "0,1,2,3,4,5,6,7,8,9,";
            //小写字母
            string lower = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            //大写字母
            string upper = lower.ToUpper();
            switch (type)
            {
                case 0:
                    Vchar = num + lower + upper;
                    break;
                case 1:
                    Vchar = num;
                    break;
                case 2:
                    Vchar = lower;
                    break;
                case 3:
                    Vchar = upper;
                    break;
                case 4:
                    Vchar = upper + lower;
                    break;
                case 5:
                    Vchar = num + lower;
                    break;
                case 6:
                    Vchar = num + upper;
                    break;
                default:
                    Vchar = num + lower + upper;
                    break;
            }
            Vchar = Vchar.Substring(0, Vchar.Length - 1);
            string[] VcArray = Vchar.Split(new Char[] { ',' });
            string VNum = "";
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                VNum += VcArray[rand.Next(VcArray.Length)];
            }
            return VNum;
        }
        /// <summary>
        /// 生成验证码的图片，将字符串填充到图片，并输出到数据流
        /// </summary>
        /// <param name="pg">页面，例如this</param>
        /// <param name="checkCode">需要生成图片的字符串</param>
        public static void ValidateCode(Page pg, string checkCode)
        {
            int iwidth = (int)(checkCode.Length * 13);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 18);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //定义颜色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            Random rand = new Random();
            //随机输出噪点

            for (int i = 0; i < 20; i++)
            {
                int x = rand.Next(image.Width);
                int y = rand.Next(image.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            }

            //输出不同字体和颜色的验证码字符
            for (int i = 0; i < checkCode.Length; i++)
            {
                int cindex = rand.Next(c.Length);
                int findex = rand.Next(font.Length);

                Font f = new System.Drawing.Font(font[findex], 12, System.Drawing.FontStyle.Bold);
                Brush b = new System.Drawing.SolidBrush(c[cindex]);
                //字符上下位置不同
                int ii = 0;
                if ((i + 1) % 2 == 0)
                {
                    ii = 1;
                }
                g.DrawString(checkCode.Substring(i, 1), f, b, 1 + (i * 13), ii);
            }
            //画一个边框
            //g.DrawRectangle(new Pen(Color.Black,0),0,0,image.Width-1,image.Height-1);

            //输出到浏览器
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            pg.Response.ClearContent();
            pg.Response.ContentType = "image/Gif";
            pg.Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            image.Dispose();
        }
    }
}
