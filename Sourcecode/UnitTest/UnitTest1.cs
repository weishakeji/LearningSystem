using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Song.ViewData;
using Song.Entities;
using Song.ServiceInterfaces;
using WeiSha.Core;
using System.Data;
using System.Collections.Generic;
using System.IO;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string tt = Song.ServiceImpls.AccountLogin.Buffer.GenerateName("123");
            Assert.AreEqual(tt, "123456");
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
        
    }
}
