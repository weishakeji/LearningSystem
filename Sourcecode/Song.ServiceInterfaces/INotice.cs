using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 院系职位的管理
    /// </summary>
    public interface INotice : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        long Add(Notice entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Save(Notice entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Delete(Notice entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void Delete(long identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Notice NoticeSingle(long identify);
        /// <summary>
        /// 获取单一实体对象，按公告名称
        /// </summary>
        /// <param name="ttl">公告名称</param>
        /// <returns></returns>
        Notice NoticeSingle(string ttl);
        /// <summary>
        /// 当前公告的上一条公告
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        Notice NoticePrev(long identify, int orgid);
        /// <summary>
        /// 当前公告的下一条公告
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        Notice NoticeNext(long identify, int orgid);
        /// <summary>
        /// 获取对象；即所有公告；
        /// </summary>
        /// <returns></returns>
        Notice[] GetAll();
        /// <summary>
        /// 获取所有公告；
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        Notice[] GetAll(int orgid, bool? isShow);
        /// <summary>
        /// 获取指定个数的记录
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="type">1为普通通知，2为弹窗通知，-1取所有</param>
        /// <param name="isShow"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Notice[] GetCount(int orgid, int type, bool? isShow, int count);
        /// <summary>
        /// 取具体的数量
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        int OfCount(int orgid, bool? isShow);
        /// <summary>
        /// 增加浏览数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="num">要增加的数量</param>
        /// <returns></returns>
        int ViewNum(long id, int num);
        /// <summary>
        /// 分页获取所有的公告；
        /// </summary>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        Notice[] GetPager(int orgid, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取所有的公告；
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isShow">是否显示</param>
        /// <param name="searTxt">查询字符</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Notice[] GetPager(int orgid, bool? isShow, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 获取通知公告
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="type">1为普通通知，2为弹窗通知</param>
        /// <param name="forpage">弹窗所在页</param>
        /// <param name="time">当前时间</param> 
        /// <param name="isShow">是否显示</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        Notice[] List(int orgid, int type, string forpage, DateTime? time, bool? isShow, int count);

        /// <summary>
        /// <summary>
        /// 更改顺序
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool UpdateTaxis(Notice[] items);
    }
}
