using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;
using NPOI.HSSF.UserModel;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// �������
    /// </summary>
    public interface IQuestions : WeiSha.Common.IBusinessInterface
    {
        #region �������
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        int QuesAdd(Questions entity);       
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void QuesSave(Questions entity);
        /// <summary>
        /// ������������ʱ�ô˷���
        /// </summary>
        /// <param name="entity">����ʵ��</param>
        /// <param name="ansItem">��ʵ��</param>
        /// <returns></returns>
        void QuesInput(Questions entity, List<Song.Entities.QuesAnswer> ansItem);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void QuesDelete(int identify);
        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="ids"></param>
        void QuesDelete(string ids);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Questions QuesSingle(int identify);
        /// <summary>
        ///  ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="cache">�Ƿ����Ի���</param>
        /// <returns></returns>
        Questions QuesSingle(int identify,bool cache);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�UID
        /// </summary>
        /// <param name="uid">ȫ��Ψһid</param>
        /// <returns></returns>
        Questions QuesSingle(string uid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰���Ŀ
        /// </summary>
        /// <param name="titile"></param>
        /// <param name="type">���͵����ֱ�ʶ</param>
        /// <returns></returns>
        Questions QuesSingle(string title, int type);
        /// <summary>
        /// ��ǰ����Ĵ�
        /// </summary>
        /// <param name="qus">�������</param>
        /// <param name="isCorrect">�Ƿ�ȡ��ȷ�𰸣����ΪNullȡ���д𰸣����Ϊtrueȡ��ȷ��</param>
        /// <returns></returns>
        QuesAnswer[] QuestionsAnswer(Questions qus, bool? isCorrect);
        /// <summary>
        /// ��ȡĳ���γ̻��½�����
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="isUse">�Ƿ�չʾ</param>
        /// <param name="count">ȡ��������С��1ȡ����</param>
        /// <returns></returns>
        Questions[] QuesCount(int type, bool? isUse, int count);
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="type">��������</param>
        /// <param name="diff">�Ѷȵȼ�</param>
        /// <param name="isUse"></param>
        /// <param name="count">ȡ������</param>
        /// <returns></returns>
        Questions[] QuesCount(int orgid, int sbjid, int couid, int olid, int type, int diff, bool? isUse, int count);
        /// <summary>
        /// ��ȡĳ���γ̻��½�����
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="type">��������</param>
        /// <param name="diff">�Ѷȵȼ�</param>
        /// <param name="isUse"></param>
        /// <param name="index">��ʼ����</param>
        /// <param name="count">ȡ������</param>
        /// <returns></returns>
        Questions[] QuesCount(int orgid, int sbjid, int couid, int olid, int type, int diff, bool? isUse, int index, int count);
        /// <summary>
        /// �����ж��ٵ���
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="type"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        int QuesOfCount(int orgid, int sbjid, int couid, int olid, int type, bool? isUse);
        int QuesOfCount(int orgid, int sbjid, int couid, int olid, int type, int diff, bool? isUse);
        DataSet QuesAns(int identify);
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">רҵid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="type">��������</param>
        /// <param name="diff1">�Ѷȷ�Χ</param>
        /// <param name="diff2">�Ѷȷ�Χ</param>
        /// <param name="isUse">�Ƿ�����</param>
        /// <param name="count">ȡ������</param>
        /// <returns></returns>
        Questions[] QuesRandom(int orgid, int sbjid, int couid, int olid, int type, int diff1, int diff2, bool? isUse, int count);
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="sbjId">����ѧ��</param>
        /// <param name="diff1">�Ѷȵȼ��������С�ȼ�</param>
        /// <param name="diff2">�Ѷȵȼ������ȼ�</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Questions[] QuesRandom(int type, int sbjId, int couid, int diff1, int diff2, bool? isUse, int count);
        /// <summary>
        /// ��ҳ��ȡ���е����⣻
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="isUse">�Ƿ���ʾ</param>
        /// <param name="searTxt">��ѯ�ַ�</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Questions[] QuesPager(int orgid, int type, bool? isUse, string searTxt, int size, int index, out int countSum);
        Questions[] QuesPager(int orgid, int type, bool? isUse, int diff, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡ���е����⣻
        /// </summary>
        /// <param name="orgid">��������id</param>
        /// <param name="type">��������</param>
        /// <param name="sbjId">רҵid</param>
        /// <param name="couid">�γ�Id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="isUse"></param>
        /// <param name="isError"></param>
        /// <param name="diff">�Ѷȵ�</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Questions[] QuesPager(int orgid, int type, int sbjId, int couid, int olid, bool? isUse, bool? isError, bool? isWrong, int diff, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="orgid">��������</param>
        /// <param name="type">�������ͣ��絥ѡ����ѡ��,��1,2�������ַ�������ʾ</param>
        /// <param name="sbjId">רҵid</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="diff">�Ѷȵȼ�����1,2�������ַ���</param>
        /// <param name="isError">�Ƿ������������⣬���Ϊ�գ������ж�</param>
        /// <param name="isWrong">�Ƿ����ѧԱ���������⣬���Ϊ�գ������ж�</param>
        /// <returns></returns>
        HSSFWorkbook QuestionsExport(int orgid, string type, int sbjId, int couid, int olid, string diff, bool? isError, bool? isWrong);
        
        #endregion

        #region ���͹���������ࣩ
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        int TypeAdd(QuesTypes entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void TypeSave(QuesTypes entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void TypeDelete(int identify);
        /// <summary>
        /// ����γ��µ��������
        /// </summary>
        /// <param name="couid">�γ�id</param>
        void TypeClear(int couid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        QuesTypes TypeSingle(int identify);
        /// <summary>
        /// ��ȡĳ��ѧ�Ƶ������������
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="isUse">�Ƿ�չʾ</param>
        /// <param name="count">ȡ��������С��1ȡ����</param>
        /// <returns></returns>
        QuesTypes[] TypeCount(int couid, bool? isUse, int count);
        /// <summary>
        /// ����ǰ��Ŀ�����ƶ������ڵ�ǰ�����ͬ���ƶ�����ͬһ���ڵ��µĶ�����ǰ�ƶ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns>����Ѿ����ڶ��ˣ��򷵻�false���ƶ��ɹ�������true</returns>
        bool TypeRemoveUp(int id);
        /// <summary>
        /// ����ǰ��Ŀ�����ƶ������ڵ�ǰ�����ͬ���ƶ�����ͬһ���ڵ��µĶ�����ǰ�ƶ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns>����Ѿ����ڶ��ˣ��򷵻�false���ƶ��ɹ�������true</returns>
        bool TypeRemoveDown(int id);
        #endregion

        #region ����𰸻�ѡ��
        /// <summary>
        /// ������Ĵ���ѡ��ת��Ϊxml�ַ���
        /// </summary>
        /// <param name="ans"></param>
        /// <returns></returns>
        string AnswerToItems(Song.Entities.QuesAnswer[] ans);
        /// <summary>
        /// ������ѡ���xml�ַ�����ת��ΪQuesAnswer��������
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="isCorrect">�Ƿ񷵻���ȷ��ѡ�null����ȫ����trueֻ������ȷ�Ĵ𰸣�falseֻ���ش���</param>
        /// <returns></returns>
        Song.Entities.QuesAnswer[] ItemsToAnswer(string xml, bool? isCorrect);
        /// <summary>
        /// ������ѡ���xml�ַ�����ת��ΪQuesAnswer��������
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        Song.Entities.QuesAnswer ItemToAnswer(string xml);
        /// <summary>
        /// ���㵱ǰ����ĵ÷�
        /// </summary>
        /// <param name="id">�����ID</param>
        /// <param name="ans">�𰸣�ѡ����Ϊid���ж���Ϊ���֣���ջ���Ϊ�ַ�</param>
        /// <param name="num">����ķ���</param>
        /// <returns>��ȷ����true</returns>
        bool ClacQues(int id, string ans);
        #endregion

        #region �������
        /// <summary>
        /// ������⻺��
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        string CacheAdd(Questions[] ques, int expires);
        string CacheAdd(Questions[] ques, int expires, string uid);
        /// <summary>
        /// �������⻺��
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="expires"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        string CacheUpdate(Questions[] ques, int expires, string uid);
        /// <summary>
        /// ���´�����Ϣ����
        /// </summary>
        /// <param name="exr"></param>
        /// <param name="expires"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        string CacheUpdate(ExamResults exr, int expires, string uid);
        /// <summary>
        /// �����⻺����ȡ����
        /// </summary>
        /// <param name="qid"></param>
        /// <returns></returns>
        Questions QuesSingle4Cache(int qid);
        /// <summary>
        /// �����⻺����ȡ����
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Questions QuesSingle4Cache(string uid);
        /// <summary>
        /// �ӻ����л�ȡ���⼯
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Questions[] CacheQuestions(string uid);
        /// <summary>
        /// ǿ��ˢ�£�������ڵĻ��棨Ĭ��ÿʮ��������һ�Σ�
        /// </summary>
        void CacheClear();
        /// <summary>
        /// ˢ�»���
        /// </summary>
        /// <param name="key">��������</param>
        void Refresh(string key);
        #endregion
        #region �¼�
        /// <summary>
        /// ���������ʱ
        /// </summary>
        event EventHandler Save;
        event EventHandler Add;
        event EventHandler Delete;
        void OnSave(object sender, EventArgs e);
        void OnAdd(object sender, EventArgs e);
        void OnDelete(object sender, EventArgs e);
        #endregion

    }
}
