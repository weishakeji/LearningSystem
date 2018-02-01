using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace Song.Extend.Html
{
    public class Context
    {        
        private string _text;
        /// <summary>
        /// 完整的HTML数据
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        private string _title;
        /// <summary>
        /// HTML页面的标题
        /// </summary>
        public string Title
        {
            get
            {
                Regex re = new Regex(@"(?<=<[\s]*title[^>]*>)(.*?)(?=</[\s]*title[^>]*>)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                MatchCollection mc = re.Matches(this._text);
                foreach (Match ma in mc)
                {
                    this._title = ma.Value;
                }
                return _title;

            }
            set
            {
                this._title = value;
                this._text = Regex.Replace(this._text, @"(?<=<[\s]*title[^>]*>)(.*?)(?=</[\s]*title[^>]*>)", value);
            }
        }
        private string _body;
        public string Body
        {
            get
            {
                Regex re = new Regex(@"(?<=<[\s]*body[^>]*>)(.*?)(?=</[\s]*body[^>]*>)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                MatchCollection mc = re.Matches(this._text);
                foreach (Match ma in mc)
                {
                    this._body = ma.Value;
                }
                return _body;

            }            
        }
        /// <summary>
        /// 获取某个标签里内的文本
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public string GetInnerText(string label)
        {
            string start = "<!--" + label + "-start-->";
            string end = "<!--" + label + "-end-->";
            int s = _text.IndexOf(start);
            int e = _text.IndexOf(end);
            if (s < 0 || e < 0 || s >= e) return "";
            string tmp = this._text.Substring(s + start.Length, e-s-start.Length);
            return tmp;
        }
        public string GetInnerText(string source,string label)
        {
            string start = "<!--" + label + "-start-->";
            string end = "<!--" + label + "-end-->";
            int s = source.IndexOf(start);
            int e = source.IndexOf(end);
            if (s < 0 || e < 0 || s >= e) return "";
            string tmp = source.Substring(s + start.Length, e - s - start.Length);
            return tmp;
        }
        /// <summary>
        /// 简单替换
        /// </summary>
        /// <param name="newStr"></param>
        /// <param name="oldStr">要替换的标签</param>      
        /// <returns></returns>
        public string Replace(string newStr, string oldStr)
        {
            oldStr = "{%=" + oldStr + "}";
            this._text = Regex.Replace(this._text, oldStr, newStr);
            return this._text;
        }
        public string Replace(string source,object newStr, string oldStr)
        {
            oldStr = "{%=" + oldStr + "}";
            source = Regex.Replace(source, oldStr, newStr.ToString(), RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            return source;
        }
        /// <summary>
        /// 往标签内追加信息,只能对单个标签操作
        /// </summary>
        /// <param name="str"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public string AddTo(string str, string label)
        {
            string start = "<!--" + label + "-start-->";
            string end = "<!--" + label + "-end-->";
            this._text = this._addTo(str, this._text, start, end);
            return this._text;
        }
        public string AddTo(string source,string str, string label)
        {
            string start = "<!--" + label + "-start-->";
            string end = "<!--" + label + "-end-->";
            source = this._addTo(str, source, start, end);
            return source;
        }
        /// <summary>
        /// 向HTML中的标签位置插入数据
        /// </summary>
        /// <param name="insStr">要插入的文本</param>
        /// <param name="label">标签</param>
        public string Insert(string insStr, string label)
        {
            string start = "<!--" + label + "-start-->";
            string end = "<!--" + label + "-end-->";
            this._text = this._insert(insStr, this._text, start, end);
            return this._text;
        }
        /// <summary>
        /// 向HTML中的标签位置插入数据
        /// </summary>
        /// <param name="source">原文本</param>
        /// <param name="insStr">要插入的文本</param>
        /// <param name="label">标签</param>
        public string Insert(string source,string insStr, string label)
        {
            string start = "<!--" + label + "-start-->";
            string end = "<!--" + label + "-end-->";
            source = this._insert(insStr, source, start, end);
            return source;
        }
        /// <summary>
        /// 向HTML中的标签位置插入数据
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private string _insert(string rep, string source, string start, string end)
        {
            int sIndex = source.IndexOf(start);
            int eIndex = source.IndexOf(end);
            //如果没有开始标签
            if (sIndex < 0) return source;
            //如果结束标签在开始标签之前
            if (sIndex > eIndex) return source;
            //前字符串
            string strStart = source.Substring(0, sIndex + start.Length);
            //后字符串
            string strEnd = source.Substring(eIndex);
            //递归处理其它相同标签
            strEnd = end + this._insert(rep, strEnd.Substring(end.Length), start, end);
            //填充后的字符串
            string fullStr = strStart + rep + strEnd;
            //
            return fullStr;
        }
        private string _addTo(string rep, string source, string start, string end)
        {
            int sIndex = source.IndexOf(start);
            int eIndex = source.IndexOf(end);
            //如果没有开始标签
            if (sIndex < 0) return source;
            //如果结束标签在开始标签之前
            if (sIndex > eIndex) return source;
            //
            //前字符串
            string strStart = source.Substring(0, sIndex + start.Length);
            //后字符串
            string strEnd = source.Substring(eIndex);
            //中间的字符串
            string strMidd = source.Substring(strStart.Length, eIndex - sIndex - start.Length);
            //递归处理其它相同标签
            //strEnd = end + this._addTo(rep, strEnd.Substring(end.Length), start, end);
            //填充后的字符串
            string fullStr = strStart + rep + strMidd + strEnd;
            //
            return fullStr;
        }
    }
}
