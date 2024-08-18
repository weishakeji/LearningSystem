using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 学习卡管理
    /// </summary>
    public interface ILearningCard : WeiSha.Core.IBusinessInterface
    {
        #region 学习卡设置管理
        /// <summary>
        /// 添加学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SetAdd(LearningCardSet entity);
        /// <summary>
        /// 修改学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <param name="scope">更改范围，1为更改使用的，已经使用的不改；2为更改全部，默认是1</param>
        void SetSave(LearningCardSet entity, int scope);
        /// <summary>
        /// 删除学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SetDelete(LearningCardSet entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void SetDelete(int identify);
        /// <summary>
        /// 判断学习卡名称是否重复
        /// </summary>
        /// <param name="name">学习卡名称</param>
        /// <param name="id">学习卡id</param>
        /// <returns></returns>
        bool SetIsExist(string name, int id);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        LearningCardSet SetSingle(int identify);
        /// <summary>
        /// 获取所有设置项
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        LearningCardSet[] SetCount(int orgid, bool? isEnable, int count);
        /// <summary>
        /// 所有设置项数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        int SetOfCount(int orgid, bool? isEnable);
        /// <summary>
        /// 学习卡的最小长度，取机构id最大数、学习设置项id最大值
        /// </summary>
        /// <returns></returns>
        int MinLength();
        /// <summary>
        /// 分页获取学习卡设置项
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LearningCardSet[] SetPager(int orgid, bool? isEnable, string searTxt, int size, int index, out int countSum);
        #endregion

        #region 关联课程
        /// <summary>
        /// 获取学习卡主题所关联的课程
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        List<Course> CoursesGet(LearningCardSet set);
        /// <summary>
        ///  获取学习卡主题所关联的课程
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        List<Course> CoursesGet(string xml);
        /// <summary>
        /// 学习卡关联的课程
        /// </summary>
        /// <param name="code">学习卡编码</param>
        /// <param name="pw">学习卡密码</param>
        /// <returns></returns>
        List<Course> CoursesForCard(string code, string pw);
        /// <summary>
        /// 设置关联的课程
        /// </summary>
        /// <param name="set"></param>
        /// <param name="courses"></param>
        /// <returns>LearningCardSet对象中的Lcs_RelatedCourses将记录关联信息</returns>
        LearningCardSet CoursesSet(LearningCardSet set, Course[] courses);
        LearningCardSet CoursesSet(LearningCardSet set, long[] couid);
        /// <summary>
        /// 设置关联的课程
        /// </summary>
        /// <param name="set"></param>
        /// <param name="couids">课程id串，以逗号分隔</param>
        /// <returns></returns>
        LearningCardSet CoursesSet(LearningCardSet set, string  couids);
        #endregion

        #region 学习卡管理
        /// <summary>
        /// 生成单张学习卡对象
        /// </summary>
        /// <param name="set">学习卡的设置项</param>
        /// <param name="code">卡号</param>
        /// <returns></returns>
        LearningCard CardGenerateObject(LearningCardSet set, string code);
        /// <summary>
        /// 批量生成学习卡
        /// </summary>
        /// <param name="set">学习卡的设置项</param>        
        /// <returns></returns>
        LearningCard[] CardGenerate(LearningCardSet set);
        /// <summary>
        /// 批量生成学习卡
        /// </summary>
        /// <param name="set"></param>
        /// <param name="tran">事务</param>
        /// <returns></returns>
        LearningCard[] CardGenerate(LearningCardSet set, DbTrans tran);
        /// <summary>
        /// 新增学习卡
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CardAdd(LearningCard entity);
        /// <summary>
        /// 修改学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CardSave(LearningCard entity);
        /// <summary>
        /// 删除学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CardDelete(LearningCard entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void CardDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        LearningCard CardSingle(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="code">学习卡编码</param>
        /// <param name="pw">学习卡密码</param>
        /// <returns></returns>
        LearningCard CardSingle(string code,string pw);
        /// <summary>
        /// 校验学习卡是否存在，或过期
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        LearningCard CardCheck(string code);
        /// <summary>
        /// 学习卡的使用数量
        /// </summary>
        /// <param name="lscid">学习卡设置项的ID</param>
        /// <returns></returns>
        int CardUsedCount(int lscid);
        /// <summary>
        /// 使用该学习卡
        /// </summary>
        /// <param name="card">学习卡</param>
        /// <param name="acc">学员账号</param>
        void CardUse(LearningCard card, Accounts acc);
        /// <summary>
        /// 获取该学习卡，只是暂存在学员账户名下，并不使用
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="acc">学员账号</param>
        void CardReceive(LearningCard entity, Accounts acc);
        /// <summary>
        /// 学习卡使用后的回滚，将删除学员的关联课程
        /// </summary>
        /// <param name="entity"></param>
        void CardRollback(LearningCard entity);
        /// <summary>
        /// 学习卡使用后的回滚，将删除学员的关联课程
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isclear">是否清理学习记录</param>
        void CardRollback(LearningCard entity,bool isclear);
        /// <summary>
        /// 学习卡设置项下的所有学习卡
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="lcsid">学习卡设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <returns></returns>
        List<LearningCard> CardCount(int orgid, int lcsid, bool? isEnable, bool? isUsed, int count);
        /// <summary>
        /// 学习卡设置项下的学习卡数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="lcsid">充码设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <param name="isback">是否被回滚</param>
        /// <returns></returns>
        int CardOfCount(int orgid, int lcsid, bool? isEnable, bool? isUsed, bool? isback);
        /// <summary>
        /// 导出Excel格式的学习卡信息
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgid">机构id</param>
        /// <param name="rsid">学习卡设置项的id</param>
        /// <returns></returns>
        string Card4Excel(string path, int orgid, int rsid);
        /// <summary>
        /// 分页获取学习卡设置项
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="lcsid">学习卡设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LearningCard[] CardPager(int orgid, int lcsid, bool? isEnable, bool? isUsed, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取学习卡设置项
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="lcsid"></param>
        /// <param name="code"></param>
        /// <param name="account">使用人账号</param>
        /// <param name="isEnable"></param>
        /// <param name="isUsed"></param>
        /// <param name="isBack"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LearningCard[] CardPager(int orgid, int lcsid, string code,string account, bool? isEnable, bool? isUsed, bool? isBack,int size, int index, out int countSum);
        #endregion

        #region 学员的卡
        /// <summary>
        /// 学员名下学习卡的数量
        /// </summary>
        /// <param name="accid">学员账号id</param>
        /// <returns></returns>
        int AccountCardOfCount(int accid);
        /// <summary>
        /// 学员名下学习卡的数量
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="state">状态，0为初始，1为使用，-1为回滚</param>
        /// <returns></returns>
        int AccountCardOfCount(int accid,int state);
        /// <summary>
        /// 学员名下的学习卡
        /// </summary>
        /// <param name="accid">学员账号id</param>
        /// <param name="state">状态，0为初始，1为使用，-1为回滚</param>
        /// <returns></returns>
        LearningCard[] AccountCards(int accid, int state);
        /// <summary>
        /// 学员名下的所有学习卡
        /// </summary>
        /// <returns></returns>
        LearningCard[] AccountCards(int accid);
        /// <summary>
        /// 学员名下的学习卡，分页获取
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="isused"></param>
        /// <param name="isback"></param>
        /// <param name="isdisable"></param>
        /// <param name="code"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="countSum">
        LearningCard[] AccountCards(int accid, bool? isused, bool? isback, bool? isdisable, string code, int index, int size, out int countSum);
        #endregion

        /// <summary>
        /// 导出学员的学习成果
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="acid"></param>
        /// <returns></returns>
        string ResultScoreToExcel(string filepath, int acid);
    }
}
