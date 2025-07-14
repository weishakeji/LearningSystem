using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Song.ViewData.Helper;

namespace UnitTest
{
    /// <summary>
    /// 字符脱敏的测试
    /// </summary>
    [TestClass]
    public class UnitTest_Desensitize
    {
        /// <summary>
        /// 字符长度小于前缀
        /// </summary>
        [TestMethod]
        public void TestMethod_prefix()
        {
            //字符长度小于前缀
            string str1= "1";
            string maskstr1 = Song.ViewData.Helper.Desensitize.MaskString(str1, 2, 0);
            Assert.AreEqual(maskstr1, "1*");

            string str2 = "123456";
            string maskstr2= Song.ViewData.Helper.Desensitize.MaskString(str2, 7, 0);
            Assert.AreEqual(maskstr2, "1*****");

            //Console.WriteLine(maskname);
        }
        /// <summary>
        /// 字符长度小于后缀
        /// </summary>
        [TestMethod]
        public void TestMethod_suffix()
        {
            //字符长度小于前缀
            string str1 = "123";
            string maskstr1 = Song.ViewData.Helper.Desensitize.MaskString(str1, 1, 3);
            Assert.AreEqual(maskstr1, "*23");

            string str2 = "123456";
            string maskstr2 = Song.ViewData.Helper.Desensitize.MaskString(str2, 2, 1);
            Assert.AreEqual(maskstr2, "12***6");

            //Console.WriteLine(maskname);
        }

        /// <summary>
        /// 姓名的脱敏测试
        /// </summary>
        [TestMethod]
        public void TestMethod_Name()
        {
            //字符长度小于前缀
            string str1 = "张";
            string maskstr1 = Song.ViewData.Helper.Desensitize.Name(str1);
            Assert.AreEqual(maskstr1, "张*");

            string str2 = "张三";
            string maskstr2 = Song.ViewData.Helper.Desensitize.Name(str2);
            Assert.AreEqual(maskstr2, "张*");

            string str3 = "张三丰";
            string maskstr3 = Song.ViewData.Helper.Desensitize.Name(str3);
            Assert.AreEqual(maskstr3, "张*丰");

            string str4 = "司马穰苴";
            string maskstr4 = Song.ViewData.Helper.Desensitize.Name(str4);
            Assert.AreEqual(maskstr4, "司**苴");
        }

        /// <summary>
        /// 身份证的脱敏测试
        /// </summary>
        [TestMethod]
        public void TestMethod_IDcard()
        {
            string str1 = "110101200007285306";
            string maskstr1 = str1.MaskIDCard();
            Assert.AreEqual(maskstr1, "1101**********5306");

        }
        /// <summary>
        ///手机号的脱敏测试
        /// </summary>
        [TestMethod]
        public void TestMethod_Phone()
        {
            string str1 = "15678945263";
            string maskstr1 = Song.ViewData.Helper.Desensitize.Phone(str1);
            Assert.AreEqual(maskstr1, "156****5263");

        }
    }
}
