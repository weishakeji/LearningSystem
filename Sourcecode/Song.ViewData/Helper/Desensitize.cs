using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData.Helper
{
    /// <summary>
    /// 信息脱敏的处理
    /// </summary>
    public static class Desensitize
    {
        /// <summary>
        /// 字符脱敏
        /// </summary>
        /// <param name="character">要操作的字符</param>
        /// <param name="prefix">不脱敏的前缀字数</param>
        /// <param name="suffix">不脱敏的后缀字数</param>
        /// <param name="maskChar">替代的字符，默认是 * 号</param>
        /// <returns></returns>
        public static string MaskString(string character, int prefix, int suffix, char maskChar = '*')
        {
            if (string.IsNullOrEmpty(character)) return character;
            character = character.Trim();
            if (string.IsNullOrEmpty(character)) return character;
            //替代字符
            string maskstr = maskChar.ToString();
            //如果长度小于等于prefix
            if (character.Length <= prefix)
                return character.Length == 1 ? character + maskstr : character[0] + new string(maskChar, character.Length - 1);
            else if (character.Length <= suffix)
                return character.Length == 1 ? maskstr + character : maskstr + character.Substring(1);
            else if (character.Length <= prefix + suffix)
                return character.Length == 1 ? character + maskstr : character[0] + maskstr + character.Substring(character.Length - suffix + 1);

            return character.Substring(0, prefix) + new string(maskChar, character.Length - prefix - suffix) + character.Substring(character.Length - suffix);
        }
        #region 常用脱敏方法
        /// <summary>
        /// 姓名脱敏
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maskChar"></param>
        /// <returns></returns>
        public static string Name(string name, char maskChar = '*') => MaskString(name, 1, 1, maskChar);
        /// <summary>
        /// 账号脱敏
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maskChar"></param>
        /// <returns></returns>
        public static string Account(string name, char maskChar = '*') => MaskString(name, 2, 1, maskChar);
        /// <summary>
        /// 身份证号码脱敏
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="maskChar"></param>
        /// <returns></returns>
        public static string IDCard(string idCard, char maskChar = '*') => MaskString(idCard, 4, 4, maskChar);
        /// <summary>
        /// 手机号的脱敏
        /// </summary>
        /// <param name="number"></param>
        /// <param name="maskChar"></param>
        /// <returns></returns>
        public static string Phone(string number, char maskChar = '*') => MaskString(number, 3, 4, maskChar);
        #endregion

        #region 字符串扩展方法
        public static string Mask(this string character, int prefix, int suffix, char maskChar = '*') => MaskString(character, prefix, suffix, maskChar);
        /// <summary>
        /// 姓名脱敏
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maskChar"></param>
        /// <returns></returns>
        public static string MaskName(this string name, char maskChar = '*') => MaskString(name, 1, 1, maskChar);
        /// <summary>
        /// 身份证号码脱敏
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="maskChar"></param>
        /// <returns></returns>
        public static string MaskIDCard(this string idCard, char maskChar = '*') => MaskString(idCard, 4, 4, maskChar);
        /// <summary>
        /// 手机号的脱敏
        /// </summary>
        /// <param name="number"></param>
        /// <param name="maskChar"></param>
        /// <returns></returns>
        public static string MaskPhone(this string number, char maskChar = '*') => MaskString(number, 3, 4, maskChar);
        #endregion
    }
}
