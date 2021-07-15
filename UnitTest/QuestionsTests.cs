using Microsoft.VisualStudio.TestTools.UnitTesting;
using Song.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.Extend.Tests
{
    [TestClass()]
    public class QuestionsTests
    {
        [TestMethod()]
        public void TransformImagePathTest()
        {
            string txt = "1、[简答题] 图5是利用基因工程培育抗虫植物的示意图。<img src=/Upload/Question/jiaoshikaoshijszgzzxshengwuxuekezhishi/9590609_0.png/>(1)从④-⑤的过程，利用了__________技术，该技术的原理是利用植物细胞的__________性。(2)从④-⑤使用的培养基中，需添加__________和__________两种关键性的植物激素。(3)转基因抗虫作物的获得，说明性状和基因之间的关系是__________。";
            //string val = Extend.Questions.TransformImagePath(txt);

            Assert.Fail();
        }
    }
}