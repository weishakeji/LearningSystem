using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 网站内容的管理
    /// </summary>
    public interface IContents : WeiSha.Core.IBusinessInterface
    {
        #region 新闻文章的管理
        /// <summary>
        /// 添加新闻文章
        /// </summary>
        /// <param name="entity">业务实体</param>
        int ArticleAdd(Article entity);
        /// <summary>
        /// 修改新闻文章
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ArticleSave(Article entity);
        /// <summary>
        /// 修改新闻文章的状态
        /// </summary>
        /// <param name="artid"></param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        bool ArticleUpdate(long artid, Field[] fiels, object[] objs);
        /// <summary>
        /// 使当前文章浏览计数加一，仅传入id，返回浏览数，效率更高
        /// </summary>
        /// <param name="artid"></param>
        /// <param name="id">新闻文章的id</param>
        /// <param name="addNum">每次浏览增加几个数</param>
        /// <returns></returns>
        int ArticleAddNumber(long artid, int addNum);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ArticleDelete(Article entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">文章实体</param>
        /// <param name="tran">事务对象</param>
        void ArticleDelete(Article entity, DbTrans tran);
        /// <summary>
        /// 彻底删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void ArticleDelete(long identify);
        /// <summary>
        /// 删除所有新闻文章
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="coluid">栏目uid</param>
        void ArticleDeleteAll(int orgid, string coluid);
        /// <summary>
        /// 标准文章是否处于删除状态，即进入回收站
        /// </summary>
        /// <param name="identify"></param>
        void ArticleIsDelete(long identify);
        /// <summary>
        /// 文章还原，从回收站回到文章列表
        /// </summary>
        /// <param name="identify"></param>
        void ArticleRecover(long identify);
        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="identify">文章id</param>
        /// <param name="verMan">审核人</param>
        void ArticlePassVerify(long identify, string verMan);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Article ArticleSingle(long identify);
        /// <summary>
        /// 当前新闻的上一条新闻
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        Article ArticlePrev(long identify, int orgid);
        /// <summary>
        /// 当前新闻的下一条新闻
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        Article ArticleNext(long identify, int orgid);
        /// <summary>
        /// 当前新闻所在的专题
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        Special[] Article4Special(long identify);
        /// <summary>
        /// 按新闻栏目获取新闻文章
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="coluid">栏目的UID</param>
        /// <param name="topNum">获取记录数</param>
        /// <param name="order">获取类别，默认null取最新置顶的优先，hot热点优先，flux流量最大优先,img为图片新闻</param>
        /// <returns></returns>
        Article[] ArticleCount(int orgid, string coluid, int topNum, string order);
        /// <summary>
        /// 按新闻栏目获取新闻文章
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="coluid">栏目的UID</param>
        /// <param name="topNum">获取记录数</param>
        /// <param name="isuse">是否启用</param>
        /// <param name="order">获取类别，默认null取最新置顶的优先，hot热点优先，flux流量最大优先,img为图片新闻</param>
        /// <returns></returns>
        Article[] ArticleCount(int orgid, string coluid, int topNum, bool? isuse, string order);
        /// <summary>
        /// 统计文章数量
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="coluid">栏目uid</param>
        /// <param name="isuse">是否启用的</param>
        /// <returns></returns>
        int ArticleOfCount(int orgid, string coluid, bool? isuse);
        /// <summary>
        /// 分页获取新闻
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="coluid">栏目id,如果id小于0，则取全部<</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="searTxt">按标题检索</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Article[] ArticlePager(int orgid, string coluid, bool? isShow, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 按栏目，标题，是否审核来分页
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="coluid"></param>
        /// <param name="isVerify">是否审核</param>
        /// <param name="isuse">是否启用</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Article[] ArticlePager(int orgid, string coluid, bool? isVerify, bool? isuse, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取新闻
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="coluid">新闻栏目</param>
        /// <param name="searTxt">按标题的检索的字符串</param>
        /// <param name="isVerify">是否审核</param>
        /// <param name="isuse">是否启用</param>
        /// <param name="order">获取类别，默认null取最新置顶的优先，hot热点优先，flux流量最大优先,img为图片新闻</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Article[] ArticlePager(int orgid, string coluid, string searTxt, bool? isVerify, bool? isuse, string order, int size, int index, out int countSum);
        #endregion

        #region 新闻专题管理
        /// <summary>
        /// 添加新闻专题
        /// </summary>
        /// <param name="entity">业务实体</param>
        int SpecialAdd(Special entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SpecialSave(Special entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SpecialDelete(Special entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void SpecialDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Special SpecialSingle(int identify);
        /// <summary>
        /// 当前专题所辖的文章
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="searTxt"></param>
        /// <returns></returns>
        Article[] Special4Article(int identify, string searTxt);
        /// <summary>
        /// 当前专题所辖的文章
        /// </summary>
        /// <param name="identify">专题id</param>
        /// <param name="searTxt">检索的信息</param>
        /// <param name="num">取多少条</param>
        /// <param name="type">获取类别，默认null取最新置顶的优先，hot热点优先，maxFlux流量最大优先</param>
        /// <returns></returns>
        Article[] Special4Article(int identify, string searTxt, int num, string type);
        /// <summary>
        /// 取新闻专题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isShow"></param>
        /// <param name="isUse"></param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        Special[] SpecialCount(int orgid, bool? isShow, bool? isUse, int count);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool SpecialUp(int orgid, int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool SpecialDown(int orgid, int id);
        /// <summary>
        /// 创建专题与文章的关联
        /// </summary>
        /// <param name="spId"></param>
        /// <param name="artId"></param>
        /// <returns></returns>
        bool SpecialAndArticle(int spId, int artId);
        /// <summary>
        /// 删除专题与文章的关联
        /// </summary>
        /// <param name="spId"></param>
        /// <param name="artId"></param>
        /// <returns></returns>
        bool SpecialAndArticleDel(int spId, int artId);
        /// <summary>
        /// 专题列表
        /// </summary>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Special[] SpecialPager(string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 专题下的文章列表
        /// </summary>
        /// <param name="spId">专题id</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Article[] SpecialArticlePager(int spId, string searTxt, int size, int index, out int countSum);
        Article[] SpecialArticlePager(int spId, string searTxt, int size, int index, out int countSum, bool? isShow, bool? isUse);
        Article[] SpecialArticlePager(int spId, string searTxt, int size, int index, out int countSum, bool? isuse, bool? isShow, bool? isUse);
        Article[] SpecialArticle(int spId, string searTxt, int count);
        /// <summary>
        /// 专题下的文章
        /// </summary>
        /// <param name="spId">专题Id</param>
        /// <param name="searTxt">检索的字符</param>
        /// <param name="isuse">是否删除</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="isUse">是否使用</param>
        /// <param name="count">取多少条，小于等于0，取所有</param>
        /// <param name="type">获取类别，默认null取最新置顶的优先，hot热点优先，maxFlux流量最大优先</param>
        /// <returns></returns>
        Article[] SpecialArticle(int spId, string searTxt, bool? isuse, bool? isShow, bool? isUse, int count, string type);
        #endregion

        #region 新闻评论管理
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int NoteAdd(NewsNote entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void NoteSave(NewsNote entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void NoteDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        NewsNote NoteSingle(int identify);
        /// <summary>
        /// 新闻的评论
        /// </summary>
        /// <param name="artid">新闻id</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="count"></param>
        /// <returns></returns>
        NewsNote[] NoteCount(int artid, bool? isShow, int count);
        /// <summary>
        /// 文章的评论
        /// </summary>
        /// <param name="artid">文章id</param>
        /// <param name="searTxt"></param>
        /// <param name="isShow"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        NewsNote[] NotePager(int artid, string searTxt, bool? isShow, int size, int index, out int countSum);
        #endregion

        #region 新闻统计
        /// <summary>
        /// 新闻资源存储大小
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isreal">是否真实大小，如果为true，则去硬盘验证是否存在该文件，并以物理文件大小计算文件大小；如果为false则以数据库记录的文件大小计算</param>
        /// <param name="count">视频个数</param>
        /// <returns>文件总大小，单位为字节</returns>
        long StorageResources(int orgid, bool isreal, out int count);
        #endregion
    }
}
