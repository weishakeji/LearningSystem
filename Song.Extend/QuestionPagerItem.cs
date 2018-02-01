using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Song.Extend
{
    public class QuestionPagerItem
    {
        #region 属性
        //不分页的最大值,例如小于200时不分页
        private int _noPagerNum = 200;
        /// <summary>
        /// 不分页的最大值,例如小于200时不分页
        /// </summary>
        public int NoPagerNum
        {
            get { return _noPagerNum; }
            set { _noPagerNum = value; }
        }

        //默认的每页多少记录
        private int _pagerCount = 100;
        /// <summary>
        /// 默认的每页多少记录
        /// </summary>
        public int PagerCount
        {
            get { return _pagerCount; }
            set { _pagerCount = value; }
        }

        //最多分多少页
        private int _maxPager = 10;
        /// <summary>
        /// 最多分多少页
        /// </summary>
        public int MaxPager
        {
            get{return _maxPager;}
            set { _maxPager = value; }
        }

        public int Index { get; set; }  //当前项的起始索引
        public int End { get; set; }    //当前项的末尾索引
        public int Size { get; set; }   //当前项取多少记录
        #endregion

        #region 构造函数
        public QuestionPagerItem()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nopagernum">不分页的最大值,例如小于200时不分页</param>
        /// <param name="pagercount">默认的每页多少记录</param>
        /// <param name="maxpager">最多分多少页</param>
        public QuestionPagerItem(int nopagernum, int pagercount, int maxpager)
        {
            _noPagerNum = nopagernum;
            _pagerCount = pagercount;
            _maxPager = maxpager;
        }
        #endregion

        /// <summary>
        /// 创建分页对象
        /// </summary>
        /// <param name="sumCount"></param>
        /// <returns></returns>
        public List<QuestionPagerItem> Builder(int sumCount)
        {
            List<QuestionPagerItem> list = new List<QuestionPagerItem>();
            if (sumCount <= 0)
            {
                list.Add(new QuestionPagerItem() { Index = 0, End = sumCount, Size = sumCount });
                return list;
            }
            //总数小于“不分页的最大值”，所以不用分页
            if (sumCount <= _noPagerNum)
            {
                list.Add(new QuestionPagerItem() { Index = 1, End = sumCount, Size = sumCount });
                return list;
            }
            //总数大于最大分页，需要重新计算每页多少条
            if (sumCount >= _pagerCount * _maxPager) _pagerCount = sumCount / _maxPager;
            //总页数
            int sumPager = sumCount % _pagerCount == 0 ? sumCount / _pagerCount : sumCount / _pagerCount + 1;
            for (int i = 0; i < sumPager; i++)
            {
                list.Add(new QuestionPagerItem()
                {
                    Index = i * _pagerCount + 1,
                    //End = i == sumPager - 1 ? i * _pagerCount + sumCount % _pagerCount : (i + 1) * _pagerCount,
                    End = (i + 1) * _pagerCount,
                    Size = _pagerCount
                });
            }            
            //计算最后一页的情况
            if (sumPager > 1 && sumCount % _pagerCount > 0)
            {
                //如果最后一页题量较少，合并到前一个内容去
                if (sumCount % _pagerCount <= _pagerCount / 2)
                {
                    list.RemoveAt(list.Count - 1);
                    list.Last<QuestionPagerItem>().End = list.Last<QuestionPagerItem>().End + sumCount % _pagerCount;
                }
                else
                {
                    list.Last<QuestionPagerItem>().End = list.Last<QuestionPagerItem>().Index + sumCount % _pagerCount - 1;
                }
            }
            list.Last<QuestionPagerItem>().Size = list.Last<QuestionPagerItem>().End - list.Last<QuestionPagerItem>().Index + 1;
            return list;
        }
    }
}
