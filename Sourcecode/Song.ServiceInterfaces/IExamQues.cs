using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using Song.Entities;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 在线考试中的试题管理
    /// </summary>
    public interface IExamQues : WeiSha.Core.IBusinessInterface
    {
        #region 试题
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        Questions QuesSingle(long id);
        /// <summary>
        /// 试题是否已经存在
        /// </summary>
        /// <param name="qus"></param>
        /// <returns></returns>
        Questions QuesIsExist(Questions qus);
        /// <summary>
        /// 可用的试题，用于出卷时抽题，所谓可用，即存在，use为true,deleted为false,没有错误
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Questions QuesAvailable(long id);
        /// <summary>
        /// 添加试题
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <param name="tags">试题关键字</param>
        /// <param name="parts">试题分类</param>
        /// <param name="knls">知识点</param>
        /// <param name="isFreshcount">是否刷新统计数量</param>
        long QuesAdd(Questions entity, QuesPart[] parts, QuesTags[] tags, QuesKnowledge[] knls, bool isFreshcount = true);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">要修改的试题</param>
        /// <param name="tags">试题关键字</param>
        /// <param name="parts">试题分类</param>
        /// <param name="knls">知识点</param>
        /// <param name="isFreshcount"></param>
        void QuesSave(Questions entity, QuesPart[] parts, QuesTags[] tags, QuesKnowledge[] knls, bool isFreshcount = true);
        /// <summary>
        /// 批量导入试题时用此方法
        /// </summary>
        /// <param name="entity">考试实体</param>
        /// <param name="ansItem">答案实体</param>
        /// <param name="parts"></param>
        /// <param name="tags"></param>
        /// <param name="knls"></param>
        /// <returns></returns>
        void QuesInput(Questions entity, List<QuesAnswer> ansItem, List<QuesPart> parts, List<QuesTags> tags, List<QuesKnowledge> knls);
        /// <summary>
        /// 删除试题
        /// </summary>
        /// <param name="entity">试题实体</param>
        int QuesDelete(Questions entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        int QuesDelete(long id);
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        int QuesRecycle(long id);
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        int QuesRemove(long id);
        /// <summary>
        /// 试题是否存在于试卷
        /// </summary>
        /// <param name="qid"></param>
        /// <returns></returns>
        bool QuesExistTestpaper(long qid);
        /// <summary>
        /// 获取随机试题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="qpid">分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点id</param> 
        /// <param name="type">试题类型</param>
        /// <param name="diff1">难度范围</param>
        /// <param name="diff2">难度范围</param>
        /// <param name="isUse">是否允许</param>
        /// <param name="count">取的数量</param>
        /// <returns></returns>
        List<Questions> QuesRandom(int orgid, long[] qpid, long[] tagid, long[] knlid, int type, int diff1, int diff2, bool? isUse, int count);
        /// <summary>
        /// 获取试题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="search"></param>
        /// <param name="isdeleted"></param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">试题难度</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isError">是否有格式错误</param>
        /// <param name="isWrong">是否有反馈的错误</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Questions> QuesPager(int orgid, string search, bool? isdeleted, long[] qpid, long[] tagid, long[] knlid,
            int[] type, int[] diff, bool? isUse, bool? isError, bool? isWrong,
            int size, int index, out int countSum);
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary>
        /// <param name="ques">要更新的试题对象</param>
        /// <param name="former">原来的试题对象</param>
        void QuesStatisticalUpdate(Questions ques, Questions former);
        /// <summary>
        /// 获取试题数量,按题型分类
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="qpid">分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点id</param>
        /// <param name="isdeleted">是否删除的</param>
        /// <param name="diff">难度等级</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isError">是否错误</param>
        /// <param name="isWrong">是否有回馈问题</param>
        /// <returns>试题类型(数字），数量</returns>
        Dictionary<int, int> QuesTotal(int orgid, long[] qpid, long[] tagid, long[] knlid, bool? isdeleted, int[] diff, bool? isUse, bool? isError, bool? isWrong);
        /// <summary>
        /// 获取试题数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="types">题型</param>
        /// <param name="qpid">分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点id</param>
        /// <param name="isdeleted">是否删除的</param>
        /// <param name="diffs">难度等级</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isError">是否错误</param>
        /// <param name="isWrong">是否有回馈问题</param>
        /// <returns></returns>
        int Total(int orgid, int[] types, long[] qpid, long[] tagid, long[] knlid, bool? isdeleted, int[] diffs, bool? isUse, bool? isError, bool? isWrong);
        #endregion

        #region 试题分类
        /// <summary>
        /// 添加试题分类
        /// </summary>
        /// <param name="entity">业务实体</param>
        int PartAdd(QuesPart entity);
        /// <summary>
        /// 批量添加分类，可用于导入时
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="names">分类名称，可以是用逗号分隔的多个名称</param>
        /// <returns></returns>
        QuesPart PartBatchAdd(int orgid, string names);
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        QuesPart PartIsExist(int orgid, long pid, string name);
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        QuesPart PartIsExist(QuesPart entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        QuesPart PartSave(QuesPart entity);
        /// <summary>
        /// 修改试题分类的某些项
        /// </summary>
        /// <param name="qpid">试题分类id</param>
        /// <param name="fields">字段</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        int PartUpdate(long qpid, Field[] fields, object[] objs);
        /// <summary>
        /// 修改试题分类的某些项
        /// </summary>
        /// <param name="qpid">试题分类id</param>
        /// <param name="field">字段</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        int PartUpdate(long qpid, Field field, object obj);
        /// <summary>
        /// 逻辑删除，标记删除状态为true
        /// </summary>
        /// <param name="id">实体的主键</param>
        int PartDelete(long id);
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        int PartRecycle(long id);
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        int PartRemove(long id);
        /// <summary>
        /// 清空试题分类下的所有试题关联关联（并不删除试题）
        /// </summary>
        /// <param name="qpid"></param>
        void PartClear(long qpid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        QuesPart PartSingle(long id);
        /// <summary>
        /// 按主键获取实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<QuesPart> PartSingle(long[] id);
        /// <summary>
        /// 当前试题分类下的所有子试题分类id
        /// </summary>
        /// <param name="qpid">当前试题分类id</param>
        /// <param name="orgid">试题分类所属机构的ID,如果小于等于零，则取从数据库读取qpid再取orgid，所以建议正确赋值，可以减少数据库读取次数</param>
        List<long> PartTreeID(long qpid, int orgid);
        /// <summary>
        /// 当前试题分类下的所有子试题分类id
        /// </summary>
        /// <param name="qpid"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        List<long> PartTreeID(long[] qpid, int orgid);
        /// <summary>
        /// 获取试题分类名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="partid">试题分类的id</param>
        /// <returns></returns>
        string PartName(long partid);
        /// <summary>
        /// 获取试题分类名称，如果为多级，则带上父级名称
        /// </summary>
        string PartName(long[] partid);
        /// <summary>
        /// 获取试题分类名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="qid">试题的id</param>
        /// <returns></returns>
        string QuesPartName(long qid);
        /// <summary>
        /// 当前试题分类，是否有子试题分类
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="id">当前试题分类Id</param>
        /// <param name="isUse">是否启用</param>
        /// <returns>有子级，返回true</returns>
        bool PartIsChildren(int orgid, long id, bool? isUse);
        /// <summary>
        /// 获取试题分类
        /// </summary>
        /// <param name="orgid">机构ID</param>
        /// <param name="sear">搜索关键字</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted"></param>
        /// <param name="pid">上级ID</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<QuesPart> PartCount(int orgid, string sear, bool? isUse, bool? isdeleted, long pid, int count);
        /// <summary>
        /// 当前试题分类的上级父级
        /// </summary>
        /// <param name="qpid">当前试题分类的id</param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        List<QuesPart> PartParents(long qpid, bool isself);
        /// <summary>
        /// 计算试题分类的数量
        /// </summary>
        /// <param name="orgid">机构id</param>       
        /// <param name="pid">上级id</param>
        /// <param name="isUse">是否启用的，null取所有</param>
        /// <param name="children">是否包括子级</param>
        /// <returns></returns>
        int PartOfCount(int orgid, long pid, bool? isUse, bool children);
        /// <summary>
        /// 当前试题分类下的所有试题
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qpid"></param>
        /// <param name="qtype">试题类型</param>
        /// <param name="isUse"></param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Questions> PartQuestions(int orgid, long qpid, int qtype, bool? isUse,bool children,  int count);
        /// <summary>
        /// 获取试题分类的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="qtype">题型</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        int PartQusTotal(int orgid, long qpid, int qtype, bool? isUse, bool children);
        /// <summary>
        /// 获取试题分类的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="qtype">题型</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        int PartQusTotal(int orgid, long[] qpid, int qtype, bool? isUse, bool children);
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary>
        void PartQusTotalUpdate(List<QuesPart> parts);
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary>
        void PartQusTotalUpdate(Questions ques);
        /// <summary>
        /// 更新试题分类所有试题数量
        /// </summary>
        void PartQusTotalUpdate();
        /// <summary>
        /// 试题所属的分类，由于是多对多关联，试题可能会属于多个分类
        /// </summary>
        /// <param name="quesid">试题id</param>
        /// <returns></returns>
        List<QuesPart> PartForQues(long quesid);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid">上级id</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<QuesPart> PartPager(int orgid, long pid, bool? isUse, bool? isdeleted, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 更改试题分类的排序
        /// </summary>
        /// <param name="list">试题分类列表，对象中只有Qp_ID、Qp_PID、Qp_Order</param>
        /// <returns></returns>
        bool PartUpdateTaxis(QuesPart[] list);
        /// <summary>
        /// 创建试题分类与试题的关联
        /// </summary>
        /// <param name="qpid"></param>
        /// <param name="qusid"></param> 
        /// <returns></returns>
        int PartConnectionQues(long qpid, long qusid);
        /// <summary>
        /// 创建试题分类与试题的关联
        /// </summary>
        int PartConnectionQues(QuesPart[] parts, long qusid);
        /// <summary>
        /// 创建试题分类与试题的关联
        /// </summary>
        int PartConnectionQues(QuesPart[] parts, Questions ques);
        #endregion

        #region 收藏
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="accid">管理员id</param>
        /// <param name="qusid">试题id</param>
        /// <returns></returns>
        bool CollectAdd(int accid, long qusid);
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="accid">管理员id</param>
        /// <param name="qusid">试题id</param>
        /// <returns></returns>
        bool CollectRemove(int accid, long qusid);
        /// <summary>
        /// 批量取消收藏
        /// </summary>
        int CollectRemove(int accid, long[] qusid);
        /// <summary>
        /// 试题是否被收藏
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="qusid"></param>
        /// <returns></returns>
        bool Collected(int accid, long qusid);
        /// <summary>
        /// 获取收藏的试题
        /// </summary>
        /// <param name="acid">管理员id</param>
        /// <param name="search">搜索题干</param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="tagid">试题标签id</param>
        /// <param name="knlid">试题知识点id</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">试题难度</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isError">是否有格式错误</param>
        /// <param name="isWrong">是否有反馈的错误</param>
        /// <param name="size">分页大小</param>
        /// <param name="index">分页索引</param>
        /// <param name="countSum">总记录数</param>
        /// <returns></returns>
        List<Questions> CollectPager(int acid, string search, long[] qpid, long[] tagid, long[] knlid, 
            int[] type, int[] diff, bool? isUse, bool? isError, bool? isWrong,
            int size, int index, out int countSum);
        #endregion

        #region 知识点
        /// <summary>
        /// 添加试题知识点
        /// </summary>
        /// <param name="entity">业务实体</param>
        int KnlAdd(QuesKnowledge entity);
        /// <summary>
        /// 批量添加知识点，可用于导入时
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="names">知识点，可以是用逗号分隔的多个名称</param>
        /// <returns></returns>
        QuesKnowledge KnlBatchAdd(int orgid, string names);
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        QuesKnowledge KnlIsExist(int orgid, long pid, string name);
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        QuesKnowledge KnlIsExist(QuesKnowledge entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void KnlSave(QuesKnowledge entity);
        /// <summary>
        /// 修改试题知识点的某些项
        /// </summary>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="fields">字段</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        int KnlUpdate(long qkid, Field[] fields, object[] objs);
        /// <summary>
        /// 修改试题知识点的某些项
        /// </summary>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="field">字段</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        int KnlUpdate(long qkid, Field field, object obj);
        /// <summary>
        /// 逻辑删除，标记删除状态为true
        /// </summary>
        /// <param name="id">实体的主键</param>
        int KnlDelete(long id);
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        int KnlRecycle(long id);
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        int KnlRemove(long id);
        /// <summary>
        /// 清空试题知识点下的所有试题关联关联（并不删除试题）
        /// </summary>
        /// <param name="qkid"></param>
        void KnlClear(long qkid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        QuesKnowledge KnlSingle(long id);
        /// <summary>
        /// 按ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<QuesKnowledge> KnlSingle(long[] id);
        /// <summary>
        /// 当前试题知识点下的所有子试题知识点id
        /// </summary>
        /// <param name="qkid">当前试题知识点id</param>
        /// <param name="orgid">试题知识点所属机构的ID,如果小于等于零，则取从数据库读取qkid再取orgid，所以建议正确赋值，可以减少数据库读取次数</param>
        List<long> KnlTreeID(long qkid, int orgid);
        /// <summary>
        /// 当前试题知识点下的所有子试题知识点id
        /// </summary>
        List<long> KnlTreeID(long[] qkid, int orgid);
        /// <summary>
        /// 获取试题知识点名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="id">试题知识点的id</param>
        /// <returns></returns>
        string KnlName(long id);
        /// <summary>
        /// 获取试题分类名称，如果为多级，则带上父级名称
        /// </summary>
        string KnlName(long[] knlid);
        /// <summary>
        /// 获取试题分类名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="qid">试题的id</param>
        /// <returns></returns>
        string QuesKnlName(long qid);
        /// <summary>
        /// 当前试题知识点，是否有子试题知识点
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="id">当前试题知识点Id</param>
        /// <param name="isUse">是否启用</param>
        /// <returns>有子级，返回true</returns>
        bool KnlIsChildren(int orgid, long id, bool? isUse);
        /// <summary>
        /// 获取试题知识点
        /// </summary>
        /// <param name="orgid">机构ID</param>
        /// <param name="sear">搜索关键字</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted"></param>
        /// <param name="pid">上级ID</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<QuesKnowledge> KnlCount(int orgid, string sear, bool? isUse, bool? isdeleted, long pid, int count);
        /// <summary>
        /// 当前试题知识点的上级父级
        /// </summary>
        /// <param name="qkid">当前试题知识点的id</param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        List<QuesKnowledge> KnlParents(long qkid, bool isself);
        /// <summary>
        /// 计算试题知识点的数量
        /// </summary>
        /// <param name="orgid">机构id</param>       
        /// <param name="pid">上级id</param>
        /// <param name="isUse">是否启用的，null取所有</param>
        /// <param name="children">是否包括子级</param>
        /// <returns></returns>
        int KnlOfCount(int orgid, long pid, bool? isUse, bool children);
        /// <summary>
        /// 当前试题知识点下的所有试题
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qkid"></param>
        /// <param name="qtype">试题类型</param>
        /// <param name="isUse"></param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Questions> KnlQuestions(int orgid, long qkid, int qtype, bool? isUse, bool children, int count);
        /// <summary>
        /// 获取试题知识点的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="qtype">题型</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        int KnlQusTotal(int orgid, long qkid, int qtype, bool? isUse, bool children);
        /// <summary>
        /// 获取试题知识点的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="qtype">题型</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        int KnlQusTotal(int orgid, long[] qkid, int qtype, bool? isUse, bool children);
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary> 
        void KnlQusTotalUpdate(List<QuesKnowledge> knls);
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary>
        void KnlQusTotalUpdate(Questions ques);
        /// <summary>
        /// 更新试题分类所有试题数量
        /// </summary>
        void KnlQusTotalUpdate();
        /// <summary>
        /// 试题关联的知识点
        /// </summary>
        /// <param name="quesid">试题id</param>
        /// <returns></returns>
        List<QuesKnowledge> KnlForQues(long quesid);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid">上级id</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<QuesKnowledge> KnlPager(int orgid, long pid, bool? isUse, bool? isdeleted, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 更改试题知识点的排序
        /// </summary>
        /// <param name="list">试题知识点列表，对象中只有Qp_ID、Qp_PID、Qp_Order</param>
        /// <returns></returns>
        bool KnlUpdateTaxis(QuesKnowledge[] list);
        /// <summary>
        /// 创建知识点与试题的关联
        /// </summary>
        /// <param name="qkid"></param>
        /// <param name="qusid"></param>
        /// <returns></returns>
        int KnlConnectionQues(long qkid, long qusid);
        /// <summary>
        /// 创建知识点与试题的关联
        /// </summary>
        int KnlConnectionQues(QuesKnowledge[] knls, long qusid);
        /// <summary>
        /// 创建知识点与试题的关联
        /// </summary>
        int KnlConnectionQues(QuesKnowledge[] knls, Questions ques);
        #endregion

        #region 关键字
        /// <summary>
        /// 添加关键字
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int TagAdd(QuesTags entity);
        /// <summary>
        /// 添加关键字
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        QuesTags TagAdd(string tag);
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool TagIsExist(int orgid, long couid, string name);
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="oneself">是否包括自身</param>
        /// <returns></returns>
        bool TagIsExist(QuesTags entity, bool oneself = false);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void TagSave(QuesTags entity);
        /// <summary>
        /// 逻辑删除，标记删除状态为true
        /// </summary>
        /// <param name="id">实体的主键</param>
        int TagDelete(long id);
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        int TagRecycle(long id);
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        int TagRemove(long id);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        QuesTags TagSingle(long id);
        /// <summary>
        /// 通过id获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<QuesTags> TagSingle(long[] id);
        /// <summary>
        /// 获取单一实体对象，按主键名称；
        /// </summary>
        /// <param name="name"></param>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        QuesTags TagSingle(string name, int orgid, long couid);
        /// <summary>
        /// 获取关键字名称
        /// </summary>
        string TagName(long[] tagid);
        /// <summary>
        /// 获取试题标签名称
        /// </summary>
        /// <param name="qid">试题的id</param>
        /// <returns></returns>
        string QuesTagName(long qid);
        /// <summary>
        /// 创建关键字与试题的关联
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="quesid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        int TagConnectionQues(string tag, long quesid, long couid);
        /// <summary>
        /// 创建关键字与试题的关联
        /// </summary>
        /// <param name="tagid"></param>
        /// <param name="quesid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        int TagConnectionQues(long tagid, long quesid, long couid);
        /// <summary>
        /// 创建关键字与试题的关联
        /// </summary>
        int TagConnectionQues(QuesTags[] tags, long quesid, long couid);
        /// <summary>
        /// 创建关键字与试题的关联
        /// </summary>
        int TagConnectionQues(QuesTags[] tags, Questions ques);
        /// <summary>
        /// 获取试题标签
        /// </summary>
        /// <param name="orgid">机构ID</param>
        /// <param name="sear">搜索关键字</param>
        /// <param name="couid"></param>
        /// <param name="isdeleted"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<QuesTags> TagCount(int orgid, string sear, long couid, bool? isdeleted, int count);
        /// <summary>
        /// 计算试题标签的数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="couid"></param>
        /// <param name="isdeleted"></param> 
        /// <returns></returns>
        int TagOfCount(int orgid, long couid, bool? isdeleted);
        /// <summary>
        /// 当前试题标签下的所有试题
        /// </summary>
        /// <param name="qtagid"></param>
        /// <param name="couid"></param>
        /// <param name="qtype">试题类型</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Questions> TagQuestions(long qtagid, long couid, int qtype, bool? isuse, int count);
        /// <summary>
        /// 某道试题的关键字
        /// </summary>
        /// <param name="quesid">试题id</param>
        /// <returns></returns>
        List<QuesTags> TagForQues(long quesid);
        /// <summary>
        /// 获取试题标签的下的试题数量
        /// </summary>
        /// <param name="qtagid">试题标签id</param>
        /// <param name="couid"></param>
        /// <param name="qtype">题型</param>
        /// <param name="isuse"></param>
        /// <returns></returns>
        int TagQusTotal(long qtagid, long couid, int qtype, bool? isuse);
        /// <summary>
        /// 获取试题标签的下的试题数量
        /// </summary>
        /// <param name="qtagid">试题标签id</param>
        /// <param name="couid"></param>
        /// <param name="qtype">题型</param>
        /// <param name="isuse"></param>
        int TagQusTotal(long[] qtagid, long couid, int qtype, bool? isuse);
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题标签下的试题数量
        /// </summary>
        void TagQusTotalUpdate(List<QuesTags> tags);
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题标签下的试题数量
        /// </summary>
        void TagQusTotalUpdate(Questions ques);
        /// <summary>
        /// 更新试题标签所有试题数量
        /// </summary>
        void TagQusTotalUpdate();
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<QuesTags> TagPager(int orgid, long couid, bool? isdeleted, string searTxt, int size, int index, out int countSum);
        #endregion

        #region 回收站
        #endregion

        #region 试题导出
        /// <summary>
        /// 导出试题
        /// </summary>
        /// <param name="subpath"></param>
        /// <param name="orgid">所属机构</param>
        /// <param name="types">题型</param>
        /// <param name="qpid">分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点id</param>
        /// <param name="isdeleted">是否删除的</param>
        /// <param name="diffs">难度等级</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isError">是否错误</param>
        /// <param name="isWrong">是否有回馈问题</param>
        /// <returns></returns>
        HSSFWorkbook QuesExport(string subpath, int orgid, int[] types, long[] qpid, long[] tagid, long[] knlid, bool? isdeleted, int[] diffs, bool? isUse, bool? isError, bool? isWrong);
        /// <summary>
        /// 导出试题,生成文件
        /// </summary>
        /// <param name="subpath">导出文件的路径（服务器端），相对临时路径的子路径</param>
        /// <param name="orgid">所属机构</param>
        /// <param name="types">题型</param>
        /// <param name="qpid">分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点id</param>
        /// <param name="isdeleted">是否删除的</param>
        /// <param name="diffs">难度等级</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isError">是否错误</param>
        /// <param name="isWrong">是否有回馈问题</param>     
        /// <returns></returns>
        JObject QuesExportExcel(string subpath, int orgid, int[] types, long[] qpid, long[] tagid, long[] knlid, bool? isdeleted, int[] diffs, bool? isUse, bool? isError, bool? isWrong);
        #endregion
    }
}
