using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 试题练习的分页导航
    /// </summary>
    public class QuesPager : BasePage
    {
        //章节id,课程id
        protected int olid = WeiSha.Common.Request.QueryString["olid"].Int32 ?? 0;
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //每页多少条
        int pagerSize = WeiSha.Common.Request.QueryString["size"].Int32 ?? 100;
        //不分页的最大值,例如小于200时不分页
        int noPager = WeiSha.Common.Request.QueryString["onpager"].Int32 ?? 200;
        //要跳转的页
        string url = WeiSha.Common.Request.QueryString["url"].String;
        protected override void InitPageTemplate(HttpContext context)
        {
            if (!Extend.LoginState.Accounts.IsLogin)
                this.Response.Redirect("login.ashx");
            this.Document.SetValue("url", url);
            this.Document.SetValue("couid", couid);   
            //当前章节
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(olid);
            this.Document.SetValue("outline", outline);
            if (outline != null) couid = outline.Cou_ID;
             //当前课程
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null) return;
            this.Document.SetValue("course", course);
            //是否购买该课程
            Song.Entities.Course couBuy = Business.Do<ICourse>().IsBuyCourse(couid, Extend.LoginState.Accounts.CurrentUser.Ac_ID, 1);
            bool isBuy = couBuy != null;
            //是否购买，如果免费也算已经购买
            this.Document.SetValue("isBuy", isBuy || course.Cou_IsFree);
            if (couBuy == null) couBuy = course;
            this.Document.SetValue("couBuy", couBuy);
            int sumCount = isBuy || course.Cou_IsFree ? Business.Do<IQuestions>().QuesOfCount(-1, -1, couid, olid, -1, true) : course.Cou_TryNum;
            this.Document.SetValue("sumCount", sumCount);
            //分页计算
            List<TrainingPager_Item> list = TrainingPager_Item.Pager(sumCount, pagerSize, noPager);
            if (list.Count <= 1)
            {
                this.Response.Redirect(string.Format(url + "?olid={0}&couid={1}&size={2}", olid, couid, list.Last<TrainingPager_Item>().Size));
            }
            this.Document.SetValue("list", list);
            this.Document.SetValue("size", pagerSize);            
        }    
        
    }
    /// <summary>
    /// 分页项
    /// </summary>
    public class TrainingPager_Item
    {
        public int Start { get; set; }  //当前项的起始索引
        public int End { get; set; }    //当前项的末尾索引
        public int Size { get; set; }   //当前项取多少记录
        public int Index { get; set; }  //当前页码
        public int Sumcount { get; set; }   //总记录数

        /// <summary>
        /// 实现分页
        /// </summary>
        /// <param name="sumcount">用于分页的总记录数</param>
        /// <param name="size">每页多少条</param>
        /// <param name="nopager">多少条以内，不用分页，例如size为100，但是nopager设置200以后就不必分页了，都算一页</param>
        /// <returns></returns>
        public static List<TrainingPager_Item> Pager(int sumcount, int size, int nopager)
        {
            List<TrainingPager_Item> list = new List<TrainingPager_Item>();
            //1.如果总计录数小于不要求页的数值，不需要分页
            //2.如果总记录数，小于或等于分页数的1.5倍，则不分页
            if (sumcount <= nopager || sumcount <= size * 1.5)
            {
                list.Add(new TrainingPager_Item()
                {
                    Start = 1,
                    End = sumcount,
                    Size = sumcount,
                    Index=1,
                    Sumcount=sumcount
                });
                return list;
            }
            //如果需要分页
            //总页数
            int sumPager = sumcount % size == 0 ? sumcount / size : sumcount / size + 1;
            for (int i = 0; i < sumPager; i++)
            {
                list.Add(new TrainingPager_Item()
                {
                    Start = i * size + 1,
                    End = (i+1) * size,
                    Size = size,
                    Sumcount = sumcount
                });
            }
            //如果最后一页题量较少，合并到前一个内容去
            if (sumPager > 2 && (sumcount % size != 0 && sumcount % size < size / 2))
            {
                list.RemoveAt(list.Count - 1);
                list.Last<TrainingPager_Item>().End = list.Last<TrainingPager_Item>().End + sumcount % size;
                list.Last<TrainingPager_Item>().Size = size + sumcount % size;
            }
            return list;
        }
    }
}