using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;
using System.Data.Common;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 大语言模型的业务接口
    /// </summary>
    public interface ILargeLanguage : WeiSha.Core.IBusinessInterface
    {        
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void RecordsAdd(LlmRecords entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void RecordsSave(LlmRecords entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void RecordsDelete(int identify);
        /// <summary>
        /// 清理学员所有记录
        /// </summary>
        /// <param name="acid"></param>
        int RecordsClear(int acid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        LlmRecords RecordsSingle(int identify);
        /// <summary>
        /// 所有
        /// </summary>
        /// <param name="acid">学员的账号id</param>
        /// <returns></returns>
        List<LlmRecords> RecordsAll(int acid);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="acid">学员的账号id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<LlmRecords> RecordsPager(int acid, int size, int index, out int countSum);
    }
}
