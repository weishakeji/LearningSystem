using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData
{
    public class LoginToken:IDisposable
    {
        /// <summary>
        /// 登录的类型
        /// </summary>
        public LoginTokenType TokenType { get; set; }
        /// <summary>
        /// 角色的账号ID
        /// </summary>
        public long RoleID { get; set; }
        /// <summary>
        /// 登录的KeyName，用于token的前缀
        /// </summary>
        public string KeyName { get; set; }
        /// <summary>
        /// 过期时效，单位分钟
        /// </summary>
        public int Expire { get; set; }
        /// <summary>
        /// 过期的时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// 唯一标识符,用于登录校验
        /// </summary>
        public string UniqueID { get; set; }   
        /// <summary>
        /// 密钥，用于加密、解密
        /// </summary>
        public string SecretKey { get; set; }

        public LoginToken(LoginTokenType tokenType)
        {
            this.TokenType = tokenType;
            WeiSha.Core.LoginItem login = WeiSha.Core.Login.Get[tokenType.ToString()];
            //加密字符
            this.SecretKey = login.Secretkey.String;
            this.KeyName = login.KeyName.String;
            this.Expire = login.Expires.Int32 ?? 0;
        }
        /// <summary>
        /// 生成加密字符串
        /// </summary>
        /// <param name="checkuid">用于校验的uid</param>
        /// <param name="roleid">角色的id</param>
        /// <returns></returns>
        public string Generate(string checkuid,long roleid)
        {
            //校验码,依次为：标识,id,角色,时效,识别码
            string checkcode = "{0},{1},{2},{3},{4}";
            string role = this.TokenType.ToString().ToLower();      //角色
            //时效
            DateTime exp = DateTime.Now.AddMinutes(this.Expire > 0 ? this.Expire : 10);
            //拼接字符串
            checkcode = string.Format(checkcode,this.KeyName, roleid, role, exp.ToString("yyyy-MM-dd HH:mm:ss"), checkuid);
            //加密         
            checkcode = WeiSha.Core.DataConvert.EncryptForDES(checkcode, SecretKey);
            return KeyName + checkcode;
        }
        /// <summary>
        /// 解析Token信息
        /// </summary>
        /// <param name="encrypt"></param>
        /// <returns>角色的账号id</returns>
        public LoginToken Analysis(string encrypt)
        {
            string str = WeiSha.Core.DataConvert.DecryptForDES(encrypt, SecretKey);
            if (string.IsNullOrWhiteSpace(str)) return null;
            //解析后信息,依次为：标识,id,角色,时效,识别码
            string[] status = str.Split(',');
            if (status.Length != 5) return null;
            //角色是否匹配
            if(!this.TokenType.ToString().Equals(status[2],StringComparison.CurrentCultureIgnoreCase)) return null;
            //时间是否过期
            DateTime exp = DateTime.MaxValue;
            DateTime.TryParse(status[3], out exp);
            if (exp < DateTime.Now) return null;

            LoginToken token = new LoginToken(this.TokenType);
            token.ExpireTime = exp;
            //角色账号id         
            long roleid = 0;
            long.TryParse(status[1], out roleid);
            token.RoleID = roleid;
            //识别码
            token.UniqueID = status[4];  
            return token;
        }
        /// <summary>
        /// 解析Token信息
        /// </summary>
        /// <param name="status">来自letter.LoginStatus</param>
        /// <returns></returns>
        public LoginToken Analysis(string[] status)
        {
            string code = string.Empty;
            foreach (string s in status)
            {
                if (!s.StartsWith(this.KeyName)) continue;
                code = s.Substring(KeyName.Length);
            }
           return Analysis(code);
        }

        public void Dispose()
        {
           
        }
    }
    /// <summary>
    /// 登录的Token类型
    /// </summary>
    public enum LoginTokenType
    {
        Account,Admin
    }
}
