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
    public interface IContents : WeiSha.Common.IBusinessInterface
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
        /// 使当前文章浏览计数加一，仅传入id，返回浏览数，效率更高
        /// </summary>
        /// <param name="id">新闻文章的id</param>
        /// <param name="addNum">每次浏览增加几个数</param>
        /// <returns></returns>
        int ArticleAddNumber(int id, int addNum);
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
        void ArticleDelete(int identify);
        /// <summary>
        /// 删除所有新闻文章
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="colid">栏目id</param>
        void ArticleDeleteAll(int orgid, int colid);
        /// <summary>
        /// 标准文章是否处于删除状态，即进入回收站
        /// </summary>
        /// <param name="identify"></param>
        void ArticleIsDelete(int identify);
        /// <summary>
        /// 文章还原，从回收站回到文章列表
        /// </summary>
        /// <param name="identify"></param>
        void ArticleRecover(int identify);
        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="identify">文章id</param>
        /// <param name="verMan">审核人</param>
        void ArticlePassVerify(int identify, string verMan);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Article ArticleSingle(int identify);
        /// <summary>
        /// 当前新闻的上一条新闻
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        Article ArticlePrev(int identify, int orgid);
        /// <summary>
        /// 当前新闻的下一条新闻
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        Article ArticleNext(int identify, int orgid);
        /// <summary>
        /// 当前新闻所在的专题
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        Special[] Article4Special(int identify);
        /// <summary>
        /// 按新闻栏目获取新闻文章
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="colId">栏目id,如果id小于0，则取全部</param>
        /// <param name="topNum">获取记录数</param>
        /// <param name="order">获取类别，默认null取最新置顶的优先，hot热点优先，flux流量最大优先,img为图片新闻</param>
        /// <returns></returns>
        Article[] ArticleCount(int orgid, int colId, int topNum, string order);
        /// <summary>
        /// 统计文章数量
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="colId"></param>
        /// <returns></returns>
        int ArticleOfCount(int orgid, int colId);
        /// <summary>
        /// 分页获取新闻
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="colid">栏目id,如果id小于0，则取全部<</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="searTxt">按标题检索</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Article[] ArticlePager(int orgid, int? colid, bool? isShow, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 按栏目，标题，是否审核来分页
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="colid"></param>
        /// <param name="isVerify">是否审核</param>
        /// <param name="isDel">是否删除</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Article[] ArticlePager(int orgid, int? colid, bool? isVerify, bool? isDel, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取新闻
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="colid">新闻栏目</param>
        /// <param name="searTxt">按标题的检索的字符串</param>
        /// <param name="isVerify">是否审核</param>
        /// <param name="isDel">是否删除</param>
        /// <param name="isTop">是否置顶</param>
        /// <param name="isHot">是否热点</param>
        /// <param name="isRec">是否推荐</param>
        /// <param name="isImg">是否是图片新闻</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Article[] ArticlePager(int orgid, int? colid, string searTxt, bool? isVerify, bool? isDel, bool? isTop, bool? isHot, bool? isRec, bool? isImg, int size, int index, out int countSum);
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
        Article[] SpecialArticlePager(int spId, string searTxt, int size, int index, out int countSum, bool? isDel, bool? isShow, bool? isUse);
        Article[] SpecialArticle(int spId, string searTxt, int count);
        /// <summary>
        /// 专题下的文章
        /// </summary>
        /// <param name="spId">专题Id</param>
        /// <param name="searTxt">检索的字符</param>
        /// <param name="isDel">是否删除</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="isUse">是否使用</param>
        /// <param name="count">取多少条，小于等于0，取所有</param>
        /// <param name="type">获取类别，默认null取最新置顶的优先，hot热点优先，maxFlux流量最大优先</param>
        /// <returns></returns>
        Article[] SpecialArticle(int spId, string searTxt, bool? isDel, bool? isShow, bool? isUse, int count, string type);
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

        #region 图片信息的管理
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int PictureAdd(Picture entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void PictureSave(Picture entity);
        /// <summary>
        /// 彻底删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void PictureDelete(int identify);
        void PictureDelete(Picture entity);
        /// <summary>
        /// 删除所有图片
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="colid">栏目id</param>
        void PictureDeleteAll(int orgid, int colid);
        /// <summary>
        /// 标注是否处于删除状态，即进入回收站
        /// </summary>
        /// <param name="identify"></param>
        void PictureIsDelete(int identify);
        /// <summary>
        /// 图片还原，从回收站回到列表
        /// </summary>
        /// <param name="identify"></param>
        void PictureRecover(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Picture PictureSingle(int identify);
        /// <summary>
        /// 设置当前图片为相册封面
        /// </summary>
        /// <param name="colid">所属栏目的id</param>
        /// <param name="pid">当前图片的Id</param>
        void PictureSetCover(int colid, int pid);
        void PictureSetCover(string uid, int pid);
        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="colid">栏目id</param>
        /// <param name="isDel">是否删除</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="searTxt">检索信息</param>
        /// <param name="count">获取多少条</param>
        /// <returns></returns>
        Picture[] PictureCount(int orgid, int? colid, bool? isDel, bool? isShow, string searTxt, int count);
        Picture[] PictureCount(int orgid, string uid, bool? isDel, bool? isShow, string searTxt, int count);
        /// <summary>
        /// 按分类分页
        /// </summary>
        /// <param name="colid">栏目id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>        
        Picture[] PicturePager(int orgid, int? colid, bool? isDel, string searTxt, int size, int index, out int countSum);
        Picture[] PicturePager(int orgid, int? colid, bool? isDel, bool? isShow, string searTxt, int size, int index, out int countSum);
        Picture[] PicturePager(int orgid, int? colid, bool? isDel, bool? isShow, string searTxt, bool? isHot, bool? isRec, bool? isTop, int size, int index, out int countSum);
        #endregion   
     
        #region 产品管理
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int ProductAdd(Product entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ProductSave(Product entity);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool ProductUp(int orgid, int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool ProductDown(int orgid, int id);
        /// <summary>
        /// 使当前产品浏览计数加一
        /// </summary>
        /// <param name="id">产品的id</param>
        /// <param name="addNum">每次浏览增加几个数</param>
        /// <returns></returns>
        int ProductNumber(int id, int addNum);
        /// <summary>
        /// 彻底删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void ProductDelete(int identify);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="colid">栏目分类id</param>
        void ProductDeleteAll(int orgid, int colid);
        /// <summary>
        /// 标准文章是否处于删除状态，即进入回收站
        /// </summary>
        /// <param name="identify"></param>
        void ProductIsDelete(int identify);
        /// <summary>
        /// 文章还原，从回收站回到文章列表
        /// </summary>
        /// <param name="identify"></param>
        void ProductRecover(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Product ProductSingle(int identify);
        /// <summary>
        /// 获取单一实体对象，按全局唯一值UID；
        /// </summary>
        /// <param name="uid">全局唯一值</param>
        /// <returns></returns>
        Product ProductSingle(string uid);
        /// <summary>
        /// 按栏目分页
        /// </summary>
        /// <param name="nc_id">栏目id，为空则返回所有</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Product[] ProductPager(int orgid, int? colid, int size, int index, out int countSum);
        Product[] ProductPager(int orgid, int? colid, string searTxt, bool? isDel, int size, int index, out int countSum);
        /// <summary>
        /// 按栏目分页
        /// </summary>
        /// <param name="ps_id">栏目id</param>
        /// <param name="searTxt">要检索的信息</param>
        /// <param name="isDel">是否删除的</param>
        /// <param name="isUse">是否使用的</param>
        /// <param name="type">按类别排序，最热hot,最新new，推荐rec,浏览器flux</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Product[] ProductPager(int orgid, int? colid, string searTxt, bool? isDel, bool? isUse, bool? isNew, bool? isRec, string type, int size, int index, out int countSum);
        /// <summary>
        /// 获取产品列表，不分页
        /// </summary>
        /// <param name="ps_id"></param>
        /// <param name="count"></param>
        /// <param name="isDel"></param>
        /// <param name="isUse"></param>
        /// <param name="type">按类别取，最热hot,最新new，推荐rec</param>
        /// <returns></returns>
        Product[] ProductCount(int orgid, int? colid, int count, bool? isDel, bool? isUse, string type);
            
        #endregion

        #region 资源信息的管理

        #region 资源
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int DownloadAdd(Download entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void DownloadSave(Download entity);
        /// <summary>
        /// 使当前下载的浏览计数加一，仅传入id，返回浏览数，效率更高
        /// </summary>
        /// <param name="id">下载信息的id</param>
        /// <param name="addNum">每次浏览增加几个数</param>
        /// <returns></returns>
        int DownloadNumber(int id, int addNum);
        /// <summary>
        /// 使当前下载信息的下载量加一
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="addNum">每次增加几个数</param>
        /// <returns></returns>
        int DownloadNumber(string file, int addNum);
        /// <summary>
        /// 彻底删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void DownloadDelete(int identify);
        void DownloadDelete(Download entity);
        /// <summary>
        /// 删除所有
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="colid">栏目id</param>
        void DownloadDeleteAll(int orgid, int colid);
        /// <summary>
        /// 标准文章是否处于删除状态，即进入回收站
        /// </summary>
        /// <param name="identify"></param>
        void DownloadIsDelete(int identify);
        /// <summary>
        /// 文章还原，从回收站回到文章列表
        /// </summary>
        /// <param name="identify"></param>
        void DownloadRecover(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Download DownloadSingle(int identify);
        /// <summary>
        ///  获取单一实体对象，按全局唯一；
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Download DownloadSingle(string uid);
        /// <summary>
        /// 分页返回
        /// </summary>
        /// <param name="colid"></param>
        /// <param name="isDel"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Download[] DownloadPager(int orgid, int? colid, bool? isDel, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="colid"></param>
        /// <param name="type">排序的类别，按热点hot,按推荐rec，按置顶top，按最新new，按流量flux</param>
        /// <param name="isDel"></param>
        /// <param name="isShow"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Download[] DownloadPager(int orgid, int? colid, string type, bool? isDel, bool? isShow, int size, int index, out int countSum);
        Download[] DownloadPager(int orgid, int? colid, string searTxt, string type, bool? isDel, bool? isShow, bool? isHot, bool? isRec, bool? isTop, int size, int index, out int countSum);
        /// <summary>
        /// 获取下载资料的信息
        /// </summary>
        /// <param name="dc_id">分类id,为0取所有信息</param>
        /// <param name="type">排序的类别，按热点hot,按推荐rec，按置顶top，按最新new，按流量flux</param>
        /// <param name="isDel"></param>
        /// <param name="isShow"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Download[] DownloadCount(int orgid, int? colid, string type, bool? isDel, bool? isShow, int count);
        #endregion

        #region 下载的资源分类
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int DownloadTypeAdd(DownloadType entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void DownloadTypeSave(DownloadType entity);
        /// <summary>
        /// 彻底删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void DownloadTypeDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        DownloadType DownloadTypeSingle(int identify);
        /// <summary>
        /// 取多少条记录
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        DownloadType[] DownloadTypeCount(int orgid, bool? isUse, int count);
        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        bool DownloadTypeUp(int orgid, int identify);
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        bool DownloadTypeDown(int orgid, int identify);
        #endregion

        #region 适用的系统
        /// <summary>
        /// 添加下载中的适用系统
        /// </summary>
        /// <param name="entity">业务实体</param>
        int DownloadOSAdd(DownloadOS entity);
        /// <summary>
        /// 修改下载中的适用系统
        /// </summary>
        /// <param name="entity">业务实体</param>
        void DownloadOSSave(DownloadOS entity);
        /// <summary>
        /// 彻底删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void DownloadOSDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        DownloadOS DownloadOSSingle(int identify);
        /// <summary>
        /// 取多少条记录
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        DownloadOS[] DownloadOSCount(int orgid, bool? isUse, int count);
        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        bool DownloadOSUp(int orgid, int identify);
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        bool DownloadOSDown(int orgid, int identify);
        #endregion

        #endregion

        #region 视频信息的管理
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int VideoAdd(Video entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void VideoSave(Video entity);
        /// <summary>
        /// 彻底删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void VideoDelete(int identify);
        void VideoDelete(Video entity);
        /// <summary>
        /// 删除所有
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="colid">栏目分类id</param>
        void VideoDeleteAll(int orgid, int colid);
        /// <summary>
        /// 标注是否处于删除状态，即进入回收站
        /// </summary>
        /// <param name="identify"></param>
        void VideoIsDelete(int identify);
        /// <summary>
        /// 视频还原，从回收站回到列表
        /// </summary>
        /// <param name="identify"></param>
        void VideoRecover(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Video VideoSingle(int identify);
        /// <summary>
        /// 设置当前视频为相册封面
        /// </summary>
        /// <param name="colid">当前分类</param>
        /// <param name="vid">当前视频的Id</param>
        void VideoSetCover(int colid, int vid);
        /// <summary>
        /// 获取视频信息
        /// </summary>
        /// <param name="colid">视频分类</param>
        /// <param name="isDel">是否删除</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="searTxt">检索信息</param>
        /// <param name="count">获取多少条</param>
        /// <returns></returns>
        Video[] VideoCount(int orgid, int? colid, bool? isDel, bool? isShow, string searTxt, int count);
        Video[] VideoPager(int orgid, int? colid, int size, int index, out int countSum);
        /// <summary>
        /// 按分类分页
        /// </summary>
        /// <param name="colid">视频分类</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>        
        Video[] VideoPager(int orgid, int? colid, bool? isDel, string searTxt, int size, int index, out int countSum);
        Video[] VideoPager(int orgid, int? colid, bool? isDel, bool? isShow, string searTxt, int size, int index, out int countSum);
        Video[] VideoPager(int orgid, int? colid, bool? isDel, bool? isShow, bool? isHot, bool? isRec, bool? isTop, string searTxt, int size, int index, out int countSum);
        #endregion       
        
    }
}
