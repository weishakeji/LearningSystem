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
            int maxscore = 90;
            int minscore = 60;
            
            //按学历计算权重
            int eduweight = 10, acedu = 71;
            if (acedu > 0 && acedu < 90)
            {
                if (acedu == 81) eduweight = 1;     //小学
                if (acedu == 71) eduweight = 3;     //初中
                if (acedu == 61) eduweight = 6;     //高中
                if (acedu == 41) eduweight = 7;
                if (acedu == 31) eduweight = 9;     //专科
                if (acedu <= 21) eduweight = 10;    //本科
            }
            //按年龄权重
            double ageweight = 10; int acage = 65;
            if (acage > 70) acage = 70;
            if (acage < 20) acage = 20;
            // 定义峰值年龄（权重最高的年龄）
            const int peakAge = 30;
            int ageDifference = Math.Abs(acage - peakAge);
            const int maxAgeDifference = 30; // 年龄差超过此值权重为1                                             
            ageweight = 10 - 10 * ageDifference / maxAgeDifference;
            ageweight = Math.Max(0, Math.Min(10, ageweight));

            //最终权重
            double weight = (eduweight + ageweight) / 2/10;
            //供选择的得分区间
            int scorerange = maxscore - minscore;
            int seek = (int)(scorerange * (1 - weight));
            int randomScore = new Random((int)(WeiSha.Core.Request.SnowID() % int.MaxValue)).Next(seek);
            randomScore += (int)(scorerange * weight) + minscore;
            //int randomScore = new Random((int)(WeiSha.Core.Request.SnowID() % int.MaxValue)).Next(scorerange);

            ////生成得分
            //randomScore = randomScore * (eduweight + ageweight) / 2 / 10;
            //randomScore += minscore;
            Assert.Equals(randomScore, 3);
        }
    }
}
