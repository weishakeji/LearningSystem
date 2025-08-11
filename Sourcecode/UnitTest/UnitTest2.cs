using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            Dictionary<int, string> _areas = Song.ViewData.Helper.AreaCode.Areas;
            int count = _areas.Count;

            //省级单位
            Dictionary<int, string> province = Song.ViewData.Helper.AreaCode.Provinces();
            //城市
            Dictionary<int, string> cities = Song.ViewData.Helper.AreaCode.Cities(130000);

            Dictionary<int, string> districts = Song.ViewData.Helper.AreaCode.Districts(130100);
        }

        [TestMethod]
        public void test2()
        {
            double randomMilliseconds = new Random((int)(WeiSha.Core.Request.SnowID() % int.MaxValue)).NextDouble();

        }
    }
}
