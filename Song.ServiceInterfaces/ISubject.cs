using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 学科管理
    /// </summary>
    public interface ISubject : WeiSha.Common.IBusinessInterface
    {
        /// <summary>
        /// 添加学科与专业
        /// </summary>
        /// <param name="entity">业务实体</param>
        int SubjectAdd(Subject entity);
        /// <summary>
        /// 批量添加专业，可用于导入时
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="names">专业名称，可以是用逗号分隔的多个名称</param>
        /// <returns></returns>
        Subject SubjectBatchAdd(int orgid, string names);
        /// <summary>
        /// 是否已经存在专业
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Subject SubjectIsExist(int orgid, int pid, string name);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SubjectSave(Subject entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void SubjectDelete(int identify);
        /// <summary>
        /// 清空专业下的所有试题
        /// </summary>
        /// <param name="identify"></param>
        void SubjectClear(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Subject SubjectSingle(int identify);
        /// <summary>
        /// 当前专业下的所有子专业id
        /// </summary>
        /// <param name="sbjid">当前专业id</param>
        /// <returns></returns>
        List<int> TreeID(int sbjid);
        /// <summary>
        /// 获取专业名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        string SubjectName(int identify);
        /// <summary>
        /// 当前专业，是否有子专业
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="identify">当前专业Id</param>
        /// <returns>有子级，返回true</returns>
        bool SubjectIsChildren(int orgid, int identify, bool? isUse);
        /// <summary>
        /// 获取学科/专业
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Subject[] SubjectCount(bool? isUse, int count);
        /// <summary>
        /// 获取学科/专业
        /// </summary>
        /// <param name="orgid">机构ID</param>
        /// <param name="sear">搜索关键字</param>
        /// <param name="isUse"></param>
        /// <param name="pid">上级ID</param>
        /// <param name="count"></param>
        /// <returns></returns>
        Subject[] SubjectCount(int orgid, string sear, bool? isUse, int pid, int count);
        /// <summary>
        /// 取指定个数的学科或专业
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sear"></param>
        /// <param name="isUse"></param>
        /// <param name="pid"></param>
        /// <param name="order">排序方式，def默认排序（先推荐，后排序号），tax按排序号,rec按推荐</param>
        /// <param name="index">启始索引</param>
        /// <param name="size">取多少条</param>
        /// <returns></returns>
        Subject[] SubjectCount(int orgid, string sear, bool? isUse, int pid, string order, int index, int size);
        /// <summary>
        /// 获取学科/专业
        /// </summary>
        /// <param name="orgid">机构ID</param>
        /// <param name="sear">搜索关键字</param>
        /// <param name="isUse"></param>
        /// <param name="pid">上级ID</param>
        /// <param name="count"></param>
        /// <returns></returns>
        Subject[] SubjectCount(int orgid, int depid, string sear, bool? isUse, int pid, int count);
        /// <summary>
        /// 当前专业的上级父级
        /// </summary>
        /// <param name="sbjid"></param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        List<Subject> Parents(int sbjid, bool isself);
        /// <summary>
        /// 计算专业数量
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="pid">上级id</param>
        /// <param name="count"></param>
        /// <returns></returns>
        int SubjectOfCount(int orgid, bool? isUse, int pid);
        /// <summary>
        /// 当前学科下的所有试题
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="identify"></param>
        /// <param name="qusType">试题类型</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Questions[] QusForSubject(int orgid, int identify, int qusType, bool? isUse, int count);
        /// <summary>
        /// 获取专业的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="identify">专业id</param>
        /// <param name="qusType">试题分类</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <returns></returns>
        int QusCountForSubject(int orgid, int identify, int qusType, bool? isUse);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Subject[] SubjectPager(int orgid, int depid, bool? isUse, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool RemoveUp(int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool RemoveDown(int id);

    }
}
