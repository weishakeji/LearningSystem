using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using System.Xml;
using NPOI.SS.UserModel;
using System.IO;



namespace Song.ServiceImpls
{
    /// <summary>
    /// 大语言模型
    /// </summary>
    public class LargeLanguageCom : ILargeLanguage
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void RecordsAdd(LlmRecords entity)
        {
            entity.Llr_CrtTime  = DateTime.Now;
            entity.Llr_LastTime = entity.Llr_CrtTime;
            Gateway.Default.Save<LlmRecords>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void RecordsSave(LlmRecords entity)
        {
            entity.Llr_LastTime = DateTime.Now;
            Gateway.Default.Save<LlmRecords>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void RecordsDelete(int identify)
        {
            Gateway.Default.Delete<LlmRecords>(LlmRecords._.Llr_ID == identify);
        }
        /// <summary>
        /// 清理学员所有记录
        /// </summary>
        /// <param name="acid"></param>
        public int RecordsClear(int acid)
        {
            return Gateway.Default.Delete<LlmRecords>(LlmRecords._.Ac_ID == acid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public LlmRecords RecordsSingle(int identify)
        {
            return Gateway.Default.Single<LlmRecords>(LlmRecords._.Llr_ID == identify);
        }
        /// <summary>
        /// 所有
        /// </summary>
        /// <param name="acid">学员的账号id</param>
        /// <param name="couid"></param>
        /// <returns></returns>
        public List<LlmRecords> RecordsAll(int acid, long couid)
        {
            return RecordsCount(acid, couid, 0);
        }
        /// <summary>
        /// 所有
        /// </summary>
        /// <param name="acid">学员的账号id</param>
        /// <param name="couid"></param>
        /// <param name="count">指定数量的记录</param>
        /// <returns></returns>
        public List<LlmRecords> RecordsCount(int acid, long couid, int count)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(LlmRecords._.Ac_ID == acid);
            if (couid > 0) wc.And(LlmRecords._.Cou_ID == couid);
            else wc.And(LlmRecords._.Cou_ID == 0);
            return Gateway.Default.From<LlmRecords>().Where(wc).OrderBy(LlmRecords._.Llr_LastTime.Desc).ToList<LlmRecords>(count);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="acid">学员的账号id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<LlmRecords> RecordsPager(int acid, long couid, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(LlmRecords._.Ac_ID == acid);
            if (couid > 0) wc.And(LlmRecords._.Cou_ID == couid);
            else wc.And(LlmRecords._.Cou_ID == 0);
            countSum = Gateway.Default.Count<LlmRecords>(wc);
            return Gateway.Default.From<LlmRecords>().Where(wc).OrderBy(LlmRecords._.Llr_LastTime.Desc).ToList<LlmRecords>(size, (index - 1) * size);
        }
    }
}
