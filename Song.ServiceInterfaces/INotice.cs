using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 院系职位的管理
    /// </summary>
    public interface INotice : WeiSha.Common.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Add(Notice entity);
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
        void Delete(int identify);
        /// <summary>
        /// 删除，按公告名称
        /// </summary>
        /// <param name="name">公告名称</param>
        void Delete(string name);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Notice NoticeSingle(int identify);
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
        Notice NoticePrev(int identify, int orgid);
        /// <summary>
        /// 当前公告的下一条公告
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        Notice NoticeNext(int identify, int orgid);
        /// <summary>
        /// 获取对象；即所有公告；
        /// </summary>
        /// <returns></returns>
        Notice[] GetAll();
        /// <summary>
        /// 获取某个院系的所有公告；
        /// </summary>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        Notice[] GetAll(int orgid, bool? isShow);
        /// <summary>
        /// 获取指定个数的记录
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isShow"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Notice[] GetCount(int orgid, bool? isShow, int count);
        /// <summary>
        /// 取具体的数量
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        int OfCount(int orgid, bool? isShow);
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
        /// <param name="isShow">是否显示</param>
        /// <param name="searTxt">查询字符</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Notice[] GetPager(int orgid, bool? isShow, string searTxt, int size, int index, out int countSum);
        
    }
}
