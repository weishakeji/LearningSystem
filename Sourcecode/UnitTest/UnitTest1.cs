using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Song.ViewData;
using Song.Entities;
using Song.ServiceInterfaces;
using WeiSha.Core;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string uid = "ExamResults：{0}-{1}-{2}";
            if (uid.IndexOf("：") > -1) uid = uid.Substring(uid.IndexOf("：") + 1);

            if (uid.IndexOf("-") > -1) uid = uid.Substring(0,uid.IndexOf("-"));

            Assert.AreEqual(uid, "123456");
        }
       
        [TestMethod]
        public void generateCode()
        {
            int test = 456;

            int max = 1002;

            string res = _lenFormatstring(test,max);
         
            Assert.AreEqual(res, "0456");
        }
        /// <summary>
        /// 格式化数值
        /// </summary>
        /// <param name="number">要格式化的数值</param>
        /// <param name="max">字符最大宽度，例如1234，宽度为4</param>
        /// <returns></returns>
        private string _lenFormatstring(int number, object max)
        {
            int len = max.ToString().Length;
            string format = string.Empty;
            while (len-- > 0) format += "0";
            return number.ToString(format);
        }
        [TestMethod]
        public void testList()
        {
            List<int> list = null;

            foreach(int i in list ?? new List<int>() )
            {
                //int t = i;
            }
            int j =0;
            string file = "aaa.txt";
            string ext = Path.GetExtension(file);
            string prefix = file.IndexOf(".") > -1 ? file.Substring(0, file.LastIndexOf(".")) : file;
            string ext1 = file.IndexOf(".") > -1 ? file.Substring(file.LastIndexOf(".")) : file;
            Assert.AreEqual(ext, ".txt");
            Assert.AreEqual(prefix, "aaa");
        }
        [TestMethod]
        public void SnowFlake()
        {
            List<long> list = new List<long>();
            for (int i = 0; i < 100000; i++)
            {
                long id = WeiSha.Core.Request.SnowID();

                list.Add(id);

            }

            Assert.AreEqual(list, "0456");
        }

        [TestMethod]
        public void encodeURIComponent_test()
        {
            float f = 0;
            double d = 0;
            decimal de = 0;
            string ff = f.GetType().Name;
            string dd = d.GetType().Name;
            string ecc = de.GetType().Name;

            char c = 'a';
            string cccc = c.GetType().Name;
        }

        [TestMethod]
        public void testXXE()
        {
            string s1 = @"<?xml version=""1.0"" encoding=""UTF-8"" ?>< !DOCTYPE ANY [<!ENTITY % name SYSTEM ""http://localhost/tp5/test.xml"">%name;]>";
            string s2 = @"<?xml version=""1.0""?>
< !DOCTYPE test[
 <!ENTITY writer ""Bill Gates"" >
 < !ENTITY copyright ""Copyright W3School.com.cn"" >
 ] > ";
            string r1 = ClearDoctype(s1);
            string r2 = ClearDoctype(s2);

            Assert.AreEqual(r1, "0456");
        }
        public string ClearDoctype(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;
            html = Regex.Replace(html, @"<(\s*)!DOCTYPE[^>]*>", "", option);
            html = Regex.Replace(html, @"<(\s*)!ENTITY[^>]*>", "", option);
            return html;
        }
    }
}
