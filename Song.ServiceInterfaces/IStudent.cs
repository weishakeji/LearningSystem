using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ѧԱ�Ĺ���
    /// </summary>
    public interface IStudent : WeiSha.Common.IBusinessInterface
    {
        #region ѧԱ�������
        /// <summary>
        /// ���ѧԱ����
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void SortAdd(StudentSort entity);
        /// <summary>
        /// �޸�ѧԱ����
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void SortSave(StudentSort entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns>���ɾ���ɹ�������0����������ѧԱ������-1�������Ĭ���飬����-2</returns>
        int SortDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        StudentSort SortSingle(int identify);
        /// <summary>
        /// ��ȡĬ��ѧԱ��
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <returns></returns>
        StudentSort SortDefault(int orgid);
        /// <summary>
        /// ����Ĭ��ѧԱ����
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="identify"></param>
        /// <returns></returns>
        void SortSetDefault(int orgid, int identify);
        /// <summary>
        /// ��ȡ���󣻼�����ѧԱ�飻
        /// </summary>
        /// <returns></returns>
        StudentSort[] SortAll(int orgid, bool? isUse);
        /// <summary>
        /// ��ȡָ�������Ķ���
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        StudentSort[] SortCount(int orgid, bool? isUse, int count);
        /// <summary>
        /// ��ȡĳ��վѧԱ�������飻
        /// </summary>
        /// <param name="StudentId">��վѧԱid</param>
        /// <returns></returns>
        StudentSort Sort4Student(int studentId);
        /// <summary>
        /// ��ȡĳ�����������վѧԱ
        /// </summary>
        /// <param name="sortid">����id</param>
        /// <returns></returns>
        Accounts[] Student4Sort(int sortid, bool? isUse);
        /// <summary>
        /// ��ǰ���������Ƿ�����
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <returns></returns>
        bool SortIsExist(StudentSort entity);
        /// <summary>
        /// ����ǰ��Ŀ�����ƶ������ڵ�ǰ�����ͬ���ƶ�����ͬһ���ڵ��µĶ�����ǰ�ƶ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns>����Ѿ����ڶ��ˣ��򷵻�false���ƶ��ɹ�������true</returns>
        bool SortRemoveUp(int orgid, int id);
        /// <summary>
        /// ����ǰ��Ŀ�����ƶ������ڵ�ǰ�����ͬ���ƶ�����ͬһ���ڵ��µĶ�����ǰ�ƶ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns>����Ѿ����ڶ��ˣ��򷵻�false���ƶ��ɹ�������true</returns>
        bool SortRemoveDown(int orgid, int id);
        /// <summary>
        /// ��ҳ��ȡѧԱ��
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="isUse"></param>
        /// <param name="name">��������</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        StudentSort[] SortPager(int orgid, bool? isUse, string name, int size, int index, out int countSum);
        #endregion

        //#region ѧԱ����
        ///// <summary>
        ///// ���ѧԱ
        ///// </summary>
        ///// <param name="entity">ҵ��ʵ��</param>
        ///// <returns>����Ѿ����ڸ�ѧԱ���򷵻�-1</returns>
        //int StudentAdd(Student entity);
        ///// <summary>
        ///// �޸�ѧԱ
        ///// </summary>
        ///// <param name="entity">ҵ��ʵ��</param>
        //void StudentSave(Student entity);
        ///// <summary>
        ///// ɾ����������ID��
        ///// </summary>
        ///// <param name="identify">ʵ�������</param>
        //void StudentDelete(int identify);        
        ///// <summary>
        ///// ɾ��������վѧԱ�ʺ���
        ///// </summary>
        ///// <param name="accname">��վѧԱ�˺�</param>
        ///// <param name="orgid"></param>
        //void StudentDelete(string accname, int orgid);
        ///// <summary>
        ///// ɾ��ѧԱ
        ///// </summary>
        ///// <param name="entity"></param>
        //void StudentDelete(Song.Entities.Student entity);
        ///// <summary>
        ///// ɾ��ѧԱ
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="tran"></param>
        //void StudentDelete(Song.Entities.Student entity, DbTrans tran);
        ///// <summary>
        ///// ��ȡ��һʵ����󣬰�����ID��
        ///// </summary>
        ///// <param name="identify">ʵ�������</param>
        ///// <returns></returns>
        //Student StudentSingle(int identify);
        ///// <summary>
        ///// ��ȡ��һʵ�壬���˺�
        ///// </summary>
        ///// <param name="accname"></param>
        ///// <param name="orgid"></param>
        ///// <returns></returns>
        //Student StudentSingle(string accname, int orgid);
        ///// <summary>
        ///// ��ȡ��һʵ����󣬰���վѧԱ����
        ///// </summary>
        ///// <param name="name">�ʺ�����</param>
        ///// <returns></returns>
        //Student StudentSingle(string accname, string pw, int orgid);
        ///// <summary>
        ///// ��ȡ��һʵ�壬ͨ��id����֤��
        ///// </summary>
        ///// <param name="id">ѧԱId</param>
        ///// <param name="uid">ѧԱ��¼ʱ��������ַ��������ж�ͬһ�˺Ų�ͬ�˵�¼������</param>
        ///// <returns></returns>
        //Student StudentSingle(int id, string uid);
        ///// <summary>
        ///// ��¼��֤
        ///// </summary>
        ///// <param name="acc">�˺ţ������֤�����ֻ�</param>
        ///// <param name="pw">���루���ģ�δ��md5���ܣ�</param>
        ///// <param name="orgid"></param>
        ///// <returns></returns>
        //Student StudentLogin(string acc, string pw, int orgid, bool? isPass);
        ///// <summary>
        ///// ��¼�ж�
        ///// </summary>
        ///// <param name="accid">ѧԱid</param>
        ///// <param name="pw">���룬md5���ܺ��</param>
        ///// <param name="orgid"></param>
        ///// <param name="isPass"></param>
        ///// <returns></returns>
        //Student StudentLogin(int accid, string pw, int orgid, bool? isPass);
        ///// <summary>
        ///// ��ǰ���ʺ��Ƿ�����
        ///// </summary>
        ///// <param name="name">ѧԱ�ʺ�</param>
        ///// <returns></returns>
        //bool IsStudentExist(int orgid, string accname);
        ///// <summary>
        ///// �ж�ѧԱ�Ƿ��Ѿ��ڴ棬���ж��˺����ֻ���
        ///// </summary>
        ///// <param name="orgid"></param>
        ///// <param name="enity"></param>
        ///// <returns></returns>
        //bool IsStudentExist(int orgid, Student enity);
        ///// <summary>
        ///// ��ǰ���ʺ��Ƿ�����
        ///// </summary>
        ///// <param name="orgid"></param>
        ///// <param name="accname"></param>
        ///// <param name="answer">��ȫ�����</param>
        ///// <returns></returns>
        //bool IsStudentExist(int orgid, string accname, string answer);
        ///// <summary>
        ///// ��ȡ���󣻼�������վѧԱ��
        ///// </summary>
        ///// <returns></returns>
        //Student[] StudentAll(int orgid, bool? isUse);
        ///// <summary>
        ///// ��ȡѧԱ
        ///// </summary>
        ///// <param name="orgid">����id</param>
        ///// <param name="isUse"></param>
        ///// <param name="count"></param>
        ///// <returns></returns>
        //Student[] StudentCount(int orgid, bool? isUse, int count);
        ///// <summary>
        ///// �����ж���ѧԱ
        ///// </summary>
        ///// <param name="orgid"></param>
        ///// <param name="isUse"></param>
        ///// <returns></returns>
        //int StudentOfCount(int orgid, bool? isUse);
        ///// <summary>
        ///// ����Excel��ʽ��ѧԱ��Ϣ
        ///// </summary>
        ///// <param name="path">�����ļ���·�����������ˣ�</param>
        ///// <param name="orgid">����id</param>
        ///// <param name="sortid">ѧԱ����id��С��0Ϊȫ��</param>
        ///// <returns></returns>
        //string StudentExport4Excel(string path, int orgid, int sortid);
        ///// <summary>
        ///// ��ҳ��ȡ���е���վѧԱ�ʺţ�
        ///// </summary>
        ///// <param name="size">ÿҳ��ʾ������¼</param>
        ///// <param name="index">��ǰ�ڼ�ҳ</param>
        ///// <param name="countSum">��¼����</param>
        ///// <returns></returns>
        //Student[] StudentPager(int orgid, int size, int index, out int countSum);
        ///// <summary>
        ///// ��ҳ��ȡĳѧԱ�飬���е���վѧԱ�ʺţ�
        ///// </summary>
        ///// <param name="orgid">����id</param>
        ///// <param name="sortid">ѧԱ����id</param>
        ///// <param name="isUse"></param>
        ///// <param name="name">ѧԱ����</param>
        ///// <param name="phone">ѧԱ�˺�</param>
        ///// <param name="size"></param>
        ///// <param name="index"></param>
        ///// <param name="countSum"></param>
        ///// <returns></returns>
        //Student[] StudentPager(int orgid, int? sortid, bool? isUse, string name, string phone, int size, int index, out int countSum);
        //#endregion

        #region ѧԱ��¼�����߼�¼
        /// <summary>
        /// ��ӵ�¼��¼
        /// </summary>
        /// <returns></returns>
        void LogForLoginAdd(Accounts st);
        /// <summary>
        /// �޸ĵ�¼�ǣ�ˢ��һ�µ�¼��Ϣ����������ʱ��
        /// </summary>
        /// <param name="interval">�����ύ�ļ��ʱ�䣬Ҳ��ÿ���ύ�����ӵ�����ʱ��������λ��</param>
        /// <param name="plat">�豸���ƣ�PCΪ���Զˣ�MobiΪ�ֻ���</param>
        void LogForLoginFresh(int interval, string plat);
        /// <summary>
        /// �˳���¼֮ǰ�ļ�¼����
        /// </summary>
        /// <param name="plat">�豸���ƣ�PCΪ���Զˣ�MobiΪ�ֻ���</param>
        void LogForLoginOut(string plat);
        /// <summary>
        /// ����ѧԱid���¼ʱ���ɵ�Uid����ʵ��
        /// </summary>
        /// <param name="stid">ѧԱId</param>
        /// <param name="stuid">��¼ʱ���ɵ�����ַ�����ȫ��Ψһ</param>
        /// <param name="plat">�豸���ƣ�PCΪ���Զˣ�MobiΪ�ֻ���</param>
        /// <returns></returns>
        LogForStudentOnline LogForLoginSingle(int stid, string stuid, string plat);
        /// <summary>
        /// ���ؼ�¼
        /// </summary>
        /// <param name="identify">��¼ID</param>
        /// <returns></returns>
        LogForStudentOnline LogForLoginSingle(int identify);
        /// <summary>
        /// ɾ��ѧԱ���߼�¼
        /// </summary>
        /// <param name="identify"></param>
        void StudentOnlineDelete(int identify);
        /// <summary>
        /// ��ҳ��ȡ
        /// </summary>
        /// <param name="orgid">����Id</param>
        /// <param name="stid">ѧԱId</param>
        /// <param name="platform">ѧԱ����ƽ̨��PC��Mobi</param>
        /// <param name="start">ͳ�ƵĿ�ʼʱ��</param>
        /// <param name="end">ͳ�ƵĽ���ʱ��</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LogForStudentOnline[] LogForLoginPager(int orgid, int stid, string platform, DateTime? start, DateTime? end, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡ
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="stid"></param>
        /// <param name="platform"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="stname">ѧԱ����</param>
        /// <param name="stmobi">ѧԱ�ֻ���</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LogForStudentOnline[] LogForLoginPager(int orgid, int stid, string platform, DateTime? start, DateTime? end, string stname, string stmobi, int size, int index, out int countSum);
        #endregion

        #region ѧԱ����ѧϰ�ļ�¼        
        /// <summary>
        /// ��¼ѧԱѧϰʱ��
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid">�½�id</param>
        /// <param name="st">ѧԱ�˻�</param>
        /// <param name="playTime">���Ž���</param>
        /// <param name="studyInterval">ѧϰʱ�䣬��Ϊʱ������ÿ���ύѧϰʱ��������</param>
        /// <param name="totalTime">��Ƶ�ܳ���</param>
        void LogForStudyFresh(int couid, int olid, Accounts st, int playTime, int studyInterval, int totalTime);
        /// <summary>
        /// ��¼ѧԱѧϰʱ��
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid">�½�id</param>
        /// <param name="st">ѧԱ�˻�</param>
        /// <param name="playTime">���Ž���</param>
        /// <param name="studyTime">ѧϰʱ�䣬��Ϊ�ۼ�ʱ��</param>
        /// <param name="totalTime">��Ƶ�ܳ���</param>
        /// <returns>ѧϰ���Ȱٷֱȣ��������ʱ���������Ϊ-1�����ʾʧ��</returns>
        double LogForStudyUpdate(int couid, int olid, Accounts st, int playTime, int studyTime, int totalTime);
        /// <summary>
        /// ����ѧԱid���¼ʱ���ɵ�Uid����ʵ��
        /// </summary>
        /// <param name="stid">ѧԱId</param>
        /// <param name="olid">�½�id</param>
        /// <returns></returns>
        LogForStudentStudy LogForStudySingle(int stid, int olid);
        /// <summary>
        /// ���ؼ�¼
        /// </summary>
        /// <param name="identify">��¼ID</param>
        /// <returns></returns>
        LogForStudentStudy LogForStudySingle(int identify);
        /// <summary>
        /// ����ѧϰ��¼
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="olid">�½�id</param>
        /// <param name="stid">ѧԱid</param>
        /// <param name="platform">ƽ̨��PC��Mobi</param>
        /// <param name="count"></param>
        /// <returns></returns>
        LogForStudentStudy[] LogForStudyCount(int orgid, int couid, int olid, int stid, string platform, int count);   
        /// <summary>
        /// ��ҳ��ȡ
        /// </summary>
        /// <param name="orgid">����Id</param>
        /// <param name="stid">ѧԱId</param>
        /// <param name="platform">ѧԱ����ƽ̨��PC��Mobi</param>
        /// <param name="start">ͳ�ƵĿ�ʼʱ��</param>
        /// <param name="end">ͳ�ƵĽ���ʱ��</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LogForStudentStudy[] LogForStudyPager(int orgid, int couid, int olid, int stid, string platform, int size, int index, out int countSum);
        /// <summary>
        /// ѧԱ����ѧϰ�γ̵ļ�¼
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="stid"></param>
        /// <returns>datatable��LastTime��Ϊѧϰʱ�䣻studyTime��ѧϰʱ��</returns>
        DataTable StudentStudyCourseLog(int stid);
        /// <summary>
        /// ѧԱָ��ѧϰ�γ̵ļ�¼
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couids">�γ�id,���ŷָ�</param>
        /// <returns></returns>
        DataTable StudentStudyCourseLog(int stid,string couids);
        /// <summary>
        /// ѧԱ����ѧϰĳһ�γ̵ļ�¼
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="couid">�γ�id</param>
        /// <returns></returns>
        DataTable StudentStudyCourseLog(int stid,int couid);
        /// <summary>
        /// ѧԱѧϰĳһ�γ��������½ڵļ�¼
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="acid">ѧԱ�˻�id</param>
        /// <returns>datatable�У�LastTime�����ѧϰʱ�䣻totalTime����Ƶʱ�䳤��playTime�����Ž��ȣ�studyTime��ѧϰʱ�䣬complete����ɶȰٷֱ�</returns>
        DataTable StudentStudyOutlineLog(int couid, int stid);
        #endregion

        #region ѧԱ�Ĵ���ع�
        /// <summary>
        /// ������ѧԱ�Ĵ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void QuesAdd(Student_Ques entity);
        /// <summary>
        /// �޸�ѧԱ�Ĵ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void QuesSave(Student_Ques entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        void QuesDelete(int identify);
        /// <summary>
        /// ɾ����������id������id
        /// </summary>
        /// <param name="quesid">����id</param>
        /// <param name="stid">ѧԱid</param>
        void QuesDelete(int quesid, int stid);
        /// <summary>
        /// ��մ���
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="stid">ѧԱid</param>
        void QuesClear(int couid, int stid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Student_Ques QuesSingle(int identify);
        /// <summary>
        /// ��ǰѧԱ�����д���
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="type">��������</param>
        /// <returns></returns>
        Questions[] QuesAll(int stid, int sbjid, int couid, int type);
        /// <summary>
        /// ��ȡָ�������Ķ���
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="type">��������</param>
        /// <returns></returns>
        Questions[] QuesCount(int stid, int sbjid, int couid, int type, int count);
        /// <summary>
        /// ��Ƶ����
        /// </summary>
        /// <param name="couid">�γ�ID</param>
        /// <param name="type">����</param>
        /// <param name="count">ȡ������</param>
        /// <returns>����������ṹ+count�У�ȡ����Ĵ������</returns>
        Questions[] QuesOftenwrong(int couid, int type, int count);
        /// <summary>
        /// ��ҳ��ȡѧԱ�Ĵ�������
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="type">��������</param>
        /// <param name="diff">���׶�</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Questions[] QuesPager(int stid, int sbjid, int couid, int type, int diff, int size, int index, out int countSum);
        #endregion

        #region ѧԱ���ղ�
        /// <summary>
        /// ������ѧԱ�ղص�����
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void CollectAdd(Student_Collect entity);
        /// <summary>
        /// �޸�ѧԱ�ղص�����
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void CollectSave(Student_Collect entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        void CollectDelete(int identify);
        /// <summary>
        /// ɾ����������id������id
        /// </summary>
        /// <param name="quesid"></param>
        /// <param name="stid"></param>
        void CollectDelete(int quesid, int stid);
        /// <summary>
        /// ��մ���
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="stid">ѧԱid</param>
        void CollectClear(int couid, int stid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Student_Collect CollectSingle(int identify);
        /// <summary>
        /// ��ǰѧԱ�ղص�����
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="type">��������</param>
        /// <returns></returns>
        Questions[] CollectAll4Ques(int stid, int sbjid, int couid, int type);
        Student_Collect[] CollectAll(int stid, int sbjid, int couid, int type);
        /// <summary>
        /// ��ȡָ�������Ķ���
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="type">��������</param>
        /// <returns></returns>
        Questions[] CollectCount(int stid, int sbjid, int couid, int type, int count);
        /// <summary>
        /// ��ҳ��ȡѧԱ�Ĵ�������
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="type">��������</param>
        /// <param name="diff">���׶�</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Questions[] CollectPager(int stid, int sbjid, int couid, int type, int diff, int size, int index, out int countSum);
        #endregion

        #region ѧԱ�ıʼ�
        /// <summary>
        /// ������ѧԱ�ıʼ�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void NotesAdd(Student_Notes entity);
        /// <summary>
        /// �޸�ѧԱ�ıʼ�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void NotesSave(Student_Notes entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        void NotesDelete(int identify);
        /// <summary>
        /// ɾ����������id������id
        /// </summary>
        /// <param name="quesid"></param>
        /// <param name="stid"></param>
        void NotesDelete(int quesid, int stid);
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="stid">ѧԱid</param>
        void NotesClear(int couid, int stid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Student_Notes NotesSingle(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����id��ѧԱid
        /// </summary>
        /// <param name="quesid">����id</param>
        /// <param name="stid">ѧԱid</param>
        /// <returns></returns>
        Student_Notes NotesSingle(int quesid, int stid);
        /// <summary>
        /// ��ǰѧԱ�����бʼ�
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="type">��������</param>
        /// <returns></returns>
        Student_Notes[] NotesAll(int stid, int type);
        /// <summary>
        /// ȡ��ǰѧԱ�ıʼ�
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Questions[] NotesCount(int stid, int couid, int type, int count);
        /// <summary>
        /// ��ȡָ�������Ķ���
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="type">��������</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        Questions[] NotesCount(int stid, int type, int count);
        /// <summary>
        /// ��ҳ��ȡѧԱ�Ĵ�������
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="quesid">����id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Student_Notes[] NotesPager(int stid, int quesid, string searTxt, int size, int index, out int countSum);
        #endregion
    }
}
