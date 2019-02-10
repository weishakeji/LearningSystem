using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;
using WeiSha.Data;
using NPOI.HSSF.UserModel;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 账户管理
    /// </summary>
    public interface IAccounts : WeiSha.Common.IBusinessInterface
    {
        #region 事件
        /// <summary>
        /// 账户信息变更时
        /// </summary>
        event EventHandler Save;
        event EventHandler Add;
        event EventHandler Delete;
        void OnSave(object sender, EventArgs e);
        void OnAdd(object sender, EventArgs e);
        void OnDelete(object sender, EventArgs e);
        #endregion

        #region 账户管理
        /// <summary>
        /// 注册协议
        /// </summary>
        /// <returns></returns>
        string RegAgreement();
        /// <summary>
        /// 添加账户
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns>如果已经存在该账户，则返回-1</returns>
        int AccountsAdd(Accounts entity);
        /// <summary>
        /// 修改账户
        /// </summary>
        /// <param name="entity">业务实体</param>
        void AccountsSave(Accounts entity);
        /// <summary>
        /// 修改账户，按条件修改
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        void AccountsUpdate(Accounts entity, Field[] fiels, object[] objs);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void AccountsDelete(int identify);
        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="entity"></param>
        void AccountsDelete(Song.Entities.Accounts entity);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Accounts AccountsSingle(int identify);
        /// <summary>
        /// 获取单一实体对象，按网站账户名称
        /// </summary>
        /// <param name="accname">账户名称</param>
        /// <param name="pw">密码</param>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        Accounts AccountsSingle(string accname, string pw, int orgid);
        /// <summary>
        /// 通过账号名获取
        /// </summary>
        /// <param name="accname">账户名称</param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        Accounts AccountsSingle(string accname, int orgid);
        /// <summary>
        /// 通过手机号获取账户
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="isPass">是否通过审核</param>
        /// <param name="isUse">是否启用</param>
        /// <returns></returns>
        Accounts AccountsSingle(string phone, bool? isPass, bool? isUse);
        /// <summary>
        /// 获取单一实体，通过id与验证码
        /// </summary>
        /// <param name="id">账户Id</param>
        /// <param name="uid">账户登录时产生随机字符，用于判断同一账号不同人登录的问题</param>
        /// <returns></returns>
        Accounts AccountsSingle(int id, string uid);
        /// <summary>
        /// 通过QQ的openid获取账户
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        Accounts Account4QQ(string openid);
        /// <summary>
        /// 通过微信的openid获取账户
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        Accounts Account4Weixin(string openid);
        /// <summary>
        /// 通过基础账号的id，获取教师账户
        /// </summary>
        /// <param name="acid"></param>
        /// <returns></returns>
        Teacher GetTeacher(int acid, bool? isPass);
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="acc">账号，或身份证，或手机</param>
        /// <param name="pw">密码（明文，未经md5加密）</param>
        /// <param name="isPass">是否审核通过</param>
        /// <returns></returns>
        Accounts AccountsLogin(string acc, string pw, bool? isPass);
        /// <summary>
        /// 登录判断
        /// </summary>
        /// <param name="accid">账户id</param>
        /// <param name="pw">密码，md5加密后的</param>
        /// <param name="isPass">是否审核通过</param>
        /// <returns></returns>
        Accounts AccountsLogin(int accid, string pw, bool? isPass);        
        /// <summary>
        /// 判断账号是否存在
        /// </summary>
        /// <param name="accname">账号名</param>
        /// <returns></returns>
        Accounts IsAccountsExist(string accname);
        /// <summary>
        /// 判断账号是否存在
        /// </summary>
        /// <param name="accname"></param>
        /// <returns></returns>
        Accounts IsAccountsExist(int orgid, string accname);
        /// <summary>
        /// 当前用帐号是否重名
        /// </summary>
        /// <param name="accname">账户帐号</param>
        /// <param name="type">判断类型，默认为账号，1为手机号,2为邮箱</param>
        /// <returns></returns>
        Accounts IsAccountsExist(int orgid, string accname, int type);
        /// <summary>
        /// 判断账户是否已经在存，将判断账号与手机号
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="enity"></param>
        /// <returns></returns>
        Accounts IsAccountsExist(int orgid, Accounts enity);
        /// <summary>
        /// 当前用帐号是否重名
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="accname"></param>
        /// <param name="answer">安全问题答案</param>
        /// <returns></returns>
        Accounts IsAccountsExist(int orgid, string accname, string answer);
        /// <summary>
        /// 获取账户
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Accounts[] AccountsCount(int orgid, bool? isUse, int count);
        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="sts">班组的id，多个id用逗号分隔</param>
        /// <param name="count">取多少条，小于1为所有</param>
        /// <returns></returns>
        List<Accounts> AccountsCount(int orgid, bool? isUse, string sts, int count);
        /// <summary>
        /// 计算有多少账户
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        int AccountsOfCount(int orgid, bool? isUse);
        /// <summary>
        /// 分页获取所有的网站账户帐号；
        /// </summary>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        Accounts[] AccountsPager(int orgid, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取某账户组，所有的网站账户帐号；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sortid">学员分组id</param>
        /// <param name="isUse"></param>
        /// <param name="acc">账户名称</param>
        /// <param name="name">姓名或昵称</param>
        /// <param name="phone">手机号</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Accounts[] AccountsPager(int orgid, int sortid, bool? isUse, string acc,string name, string phone, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取某账户组，所有的网站账户帐号；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sortid">学员分组id</param>
        /// <param name="pid">推荐人id</param>
        /// <param name="isUse"></param>
        /// <param name="acc">账户名称</param>
        /// <param name="name">姓名或昵称</param>
        /// <param name="phone">手机号</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Accounts[] AccountsPager(int orgid, int sortid, int pid, bool? isUse, string acc, string name, string phone, int size, int index, out int countSum);
        /// <summary>
        /// 学员账号信息导出
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgid">机构id</param>
        /// <param name="sorts">学员分组id，用逗号分隔</param>
        /// <returns></returns>
        string AccountsExport4Excel(string path, int orgid, string sorts);
        /// <summary>
        /// 学员账户号导出
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgs">机构id,用逗号分隔</param>
        /// <returns></returns>
        string AccountsExport4Excel(string path, string orgs);
        #endregion

        #region 下级账户，上级账户
        /// <summary>
        /// 下级会员数据
        /// </summary>
        /// <param name="acid">当前账号ID</param>
        /// <param name="isAll">是否包括所有下级，true是所有，false只取直接下级</param>
        /// <returns></returns>
        int SubordinatesCount(int acid, bool isAll);
        /// <summary>
        /// 下级会员分页获取
        /// </summary>
        /// <param name="acid">当前账号id</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="acc"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Accounts[] SubordinatesPager(int acid, bool? isUse, string acc, string name, string phone, int size, int index, out int countSum);
        /// <summary>
        /// 当前账户的所有父级账户，依次向上
        /// </summary>
        /// <param name="accid">当前账户id</param>
        /// <returns></returns>
        Accounts[] Parents(int accid);
        Accounts[] Parents(Accounts acc);
        #endregion

        #region 积分管理
        /// <summary>
        /// 收入
        /// </summary>
        /// <param name="entity">业务实体</param>
        PointAccount PointAdd(PointAccount entity);
        /// <summary>
        /// 增加登录积分
        /// </summary>
        /// <param name="acc">学员账户</param>
        /// <returns></returns>
        /// <returns>此次登录所增加的积分数</returns>
        int PointAdd4Login(Accounts acc);
        /// <summary>
        /// 增加登录积分
        /// </summary>
        /// <param name="acc">学员账户</param>
        /// <param name="source">来源信息</param>
        /// <param name="info">信息</param>
        /// <param name="remark">备注</param>
        /// <returns>此次登录所增加的积分数</returns>
        int PointAdd4Login(Accounts acc,string source,string info,string remark);
        /// <summary>
        /// 增加分享链接的访问积分
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        int PointAdd4Share(Accounts acc);
        /// <summary>
        /// 增加分享链接的注册积分
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        int PointAdd4Register(Accounts acc);
        /// <summary>
        /// 支出
        /// </summary>
        /// <param name="entity">业务实体</param>
        PointAccount PointPay(PointAccount entity);
        /// <summary>
        /// 删除流水
        /// </summary>
        /// <param name="entity">业务实体</param>
        void PointDelete(PointAccount entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void PointDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        PointAccount PointSingle(int identify);
        /// <summary>
        /// 获取单一实体对象，按流水号
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        PointAccount PointSingle(string serial);
        /// <summary>
        /// 修改流水信息
        /// </summary>
        /// <param name="entity"></param>
        void PointSave(PointAccount entity);
        /// <summary>
        /// 获取指定个数的记录
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="count"></param>
        /// <returns></returns>
        PointAccount[] PointCount(int orgid, int stid, int type, int count);
        /// <summary>
        /// 计算某一个时间区间的积分
        /// </summary>
        /// <param name="acid">学员账户</param>
        /// <param name="formType">来源分类，1登录，2分享访问；3分享注册；4兑换; </param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        int PointClac(int acid, int formType, DateTime? start, DateTime? end);
        /// <summary>
        /// 分页获取所有的公告；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        PointAccount[] PointPager(int orgid, int stid, int type, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取所有的公告；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="searTxt">按信息检索</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        PointAccount[] PointPager(int orgid, int st, int type, string searTxt, DateTime? start, DateTime? end, int size, int index, out int countSum);        
        #endregion

        #region 卡券管理
        /// <summary>
        /// 收入
        /// </summary>
        /// <param name="entity">业务实体</param>
        CouponAccount CouponAdd(CouponAccount entity);
        /// <summary>
        /// 支出
        /// </summary>
        /// <param name="entity">业务实体</param>
        CouponAccount CouponPay(CouponAccount entity);
        /// <summary>
        /// 积分兑换卡券
        /// </summary>
        /// <param name="accid">学员id</param>
        /// <param name="coupon">要兑换的卡券数量</param>
        /// <returns></returns>
        void CouponExchange(int accid,int coupon);
        /// <summary>
        /// 积分兑换卡券
        /// </summary>
        /// <param name="acc">学员</param>
        /// <param name="coupon">要兑换的卡券数量</param>
        void CouponExchange(Accounts acc, int coupon);
        /// <summary>
        /// 删除流水
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CouponDelete(CouponAccount entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void CouponDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        CouponAccount CouponSingle(int identify);
        /// <summary>
        /// 获取单一实体对象，按流水号
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        CouponAccount CouponSingle(string serial);
        /// <summary>
        /// 修改流水信息
        /// </summary>
        /// <param name="entity"></param>
        void CouponSave(CouponAccount entity);
        /// <summary>
        /// 获取指定个数的记录
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="count"></param>
        /// <returns></returns>
        CouponAccount[] CouponCount(int orgid, int stid, int type, int count);
        /// <summary>
        /// 计算某一个时间区间的积分
        /// </summary>
        /// <param name="acid">学员账户</param>
        /// <param name="formType">来源分类，1兑换，2消费支出；5分润；4管理员充值；</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        int CouponClac(int acid, int formType, DateTime? start, DateTime? end);
        /// <summary>
        /// 分页获取所有的公告；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        CouponAccount[] CouponPager(int orgid, int stid, int type, int size, int index, out int countSum);
        CouponAccount[] CouponPager(int orgid, int stid, int type, DateTime? start, DateTime? end,int size, int index, out int countSum);
        /// <summary>
        /// 分页获取所有的公告；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="searTxt">按信息检索</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        CouponAccount[] CouponPager(int orgid, int st, int type, string searTxt, int size, int index, out int countSum);
        #endregion

        #region 资金管理
        /// <summary>
        /// 收入
        /// </summary>
        /// <param name="entity">业务实体</param>
        MoneyAccount MoneyIncome(MoneyAccount entity);
        /// <summary>
        /// 支出
        /// </summary>
        /// <param name="entity">业务实体</param>
        MoneyAccount MoneyPay(MoneyAccount entity);
        /// <summary>
        /// 通过流水号进行资金支出或收入的确认操作
        /// </summary>
        /// <param name="serial">流水号</param>
        /// <returns></returns>
        MoneyAccount MoneyConfirm(string serial);
        /// <summary>
        /// 通过交易记录的对象，进行资金支出或收入的确认操作
        /// </summary>
        /// <param name="ma"></param>
        /// <returns></returns>
        MoneyAccount MoneyConfirm(MoneyAccount ma);        
        /// <summary>
        /// 删除流水
        /// </summary>
        /// <param name="entity">业务实体</param>
        void MoneyDelete(MoneyAccount entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void MoneyDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        MoneyAccount MoneySingle(int identify);
        /// <summary>
        /// 获取单一实体对象，按流水号
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        MoneyAccount MoneySingle(string serial);
        /// <summary>
        /// 计算资金收益
        /// </summary>
        /// <param name="accid">账号id</param>
        /// <param name="type">1支出，2收入（包括充值、分润等）</param>
        /// <param name="from">类型，来源，1为管理员操作，2为充值码充值；3这在线支付；4购买课程,5分润</param>
        /// <returns></returns>
        decimal MoneySum(int accid, int type, int from);
        /// <summary>
        /// 修改流水信息
        /// </summary>
        /// <param name="entity"></param>
        void MoneySave(MoneyAccount entity);
        /// <summary>
        /// 获取指定个数的记录
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="isSuccess">是否操作成功</param>
        /// <param name="count"></param>
        /// <returns></returns>
        MoneyAccount[] MoneyCount(int orgid, int stid, int type, bool? isSuccess, int count);
        /// <summary>
        /// 计算某一个时间区间的现金
        /// </summary>
        /// <param name="acid">学员账户</param>
        /// <param name="formType">1为管理员操作，2为充值码充值；3在线支付；4购买课程,5分润</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        int MoneyClac(int acid, int formType, DateTime? start, DateTime? end);
        /// <summary>
        /// 分页获取资金流水；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        MoneyAccount[] MoneyPager(int orgid, int stid, int type, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取资金流水；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="searTxt">按信息检索</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        MoneyAccount[] MoneyPager(int orgid, int st, int type, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取资金流水；
        /// </summary>
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="from">来源，1为管理员，2为充值码，3为在线支付</param>
        /// <param name="searTxt">按信息检索</param>
        /// <param name="start">按时间检索区间，此为开始时间</param>
        /// <param name="end">按时间检索区间，此为结束时间</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        MoneyAccount[] MoneyPager(int orgid, int st, int type, int from, string searTxt, DateTime? start, DateTime? end, int size, int index, out int countSum);
        #endregion
        
    }
}
