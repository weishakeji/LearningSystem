namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Accounts 主键列：Ac_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Accounts : WeiSha.Data.Entity {
    		
    		protected Int32 _Ac_ID;
    		
    		protected Int32 _Ac_PID;
    		
    		protected String _Ac_AccName;
    		
    		protected String _Ac_Pw;
    		
    		protected String _Ac_Name;
    		
    		protected String _Ac_Pinyin;
    		
    		protected String _Ac_IDCardNumber;
    		
    		protected String _Ac_Signature;
    		
    		protected Int32 _Ac_Age;
    		
    		protected Int32 _Ac_Sex;
    		
    		protected String _Ac_Photo;
    		
    		protected Decimal _Ac_Money;
    		
    		protected Int32 _Ac_Point;
    		
    		protected Int32 _Ac_Coupon;
    		
    		protected DateTime _Ac_Birthday;
    		
    		protected Boolean _Ac_IsUse;
    		
    		protected Boolean _Ac_IsPass;
    		
    		protected Boolean _Ac_IsTeacher;
    		
    		protected String _Ac_Qus;
    		
    		protected String _Ac_Ans;
    		
    		protected DateTime _Ac_RegTime;
    		
    		protected DateTime _Ac_OutTime;
    		
    		protected DateTime _Ac_LastTime;
    		
    		protected String _Ac_LastIP;
    		
    		protected String _Ac_Tel;
    		
    		protected Boolean _Ac_IsOpenTel;
    		
    		protected String _Ac_MobiTel1;
    		
    		protected String _Ac_MobiTel2;
    		
    		protected Boolean _Ac_IsOpenMobile;
    		
    		protected String _Ac_Email;
    		
    		protected String _Ac_Qq;
    		
    		protected String _Ac_Weixin;
    		
    		protected String _Ac_CheckUID;
    		
    		protected String _Ac_UID;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Ac_Zip;
    		
    		protected String _Ac_LinkMan;
    		
    		protected String _Ac_LinkManPhone;
    		
    		protected String _Ac_Intro;
    		
    		protected String _Ac_Major;
    		
    		protected String _Ac_Education;
    		
    		protected String _Ac_Native;
    		
    		protected String _Ac_Nation;
    		
    		protected String _Ac_CodeNumber;
    		
    		protected String _Ac_Address;
    		
    		protected String _Ac_AddrContact;
    		
    		protected Int32 _Dep_Id;
    		
    		protected Int32 _Sts_ID;
    		
    		protected String _Sts_Name;
    		
    		protected Int32 _Ac_CurrCourse;
    		
    		protected Int32 _Ac_PointAmount;
    		
    		protected String _Ac_School;
    		
    		protected String _Ac_QqOpenID;
    		
    		protected String _Ac_WeixinOpenID;
    		
    		public Int32 Ac_ID {
    			get {
    				return this._Ac_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_ID, _Ac_ID, value);
    				this._Ac_ID = value;
    			}
    		}
    		
    		public Int32 Ac_PID {
    			get {
    				return this._Ac_PID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_PID, _Ac_PID, value);
    				this._Ac_PID = value;
    			}
    		}
    		
    		public String Ac_AccName {
    			get {
    				return this._Ac_AccName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_AccName, _Ac_AccName, value);
    				this._Ac_AccName = value;
    			}
    		}
    		
    		public String Ac_Pw {
    			get {
    				return this._Ac_Pw;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Pw, _Ac_Pw, value);
    				this._Ac_Pw = value;
    			}
    		}
    		
    		public String Ac_Name {
    			get {
    				return this._Ac_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Name, _Ac_Name, value);
    				this._Ac_Name = value;
    			}
    		}
    		
    		public String Ac_Pinyin {
    			get {
    				return this._Ac_Pinyin;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Pinyin, _Ac_Pinyin, value);
    				this._Ac_Pinyin = value;
    			}
    		}
    		
    		public String Ac_IDCardNumber {
    			get {
    				return this._Ac_IDCardNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_IDCardNumber, _Ac_IDCardNumber, value);
    				this._Ac_IDCardNumber = value;
    			}
    		}
    		
    		public String Ac_Signature {
    			get {
    				return this._Ac_Signature;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Signature, _Ac_Signature, value);
    				this._Ac_Signature = value;
    			}
    		}
    		
    		public Int32 Ac_Age {
    			get {
    				return this._Ac_Age;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Age, _Ac_Age, value);
    				this._Ac_Age = value;
    			}
    		}
    		
    		public Int32 Ac_Sex {
    			get {
    				return this._Ac_Sex;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Sex, _Ac_Sex, value);
    				this._Ac_Sex = value;
    			}
    		}
    		
    		public String Ac_Photo {
    			get {
    				return this._Ac_Photo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Photo, _Ac_Photo, value);
    				this._Ac_Photo = value;
    			}
    		}
    		
    		public Decimal Ac_Money {
    			get {
    				return this._Ac_Money;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Money, _Ac_Money, value);
    				this._Ac_Money = value;
    			}
    		}
    		
    		public Int32 Ac_Point {
    			get {
    				return this._Ac_Point;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Point, _Ac_Point, value);
    				this._Ac_Point = value;
    			}
    		}
    		
    		public Int32 Ac_Coupon {
    			get {
    				return this._Ac_Coupon;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Coupon, _Ac_Coupon, value);
    				this._Ac_Coupon = value;
    			}
    		}
    		
    		public DateTime Ac_Birthday {
    			get {
    				return this._Ac_Birthday;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Birthday, _Ac_Birthday, value);
    				this._Ac_Birthday = value;
    			}
    		}
    		
    		public Boolean Ac_IsUse {
    			get {
    				return this._Ac_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_IsUse, _Ac_IsUse, value);
    				this._Ac_IsUse = value;
    			}
    		}
    		
    		public Boolean Ac_IsPass {
    			get {
    				return this._Ac_IsPass;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_IsPass, _Ac_IsPass, value);
    				this._Ac_IsPass = value;
    			}
    		}
    		
    		public Boolean Ac_IsTeacher {
    			get {
    				return this._Ac_IsTeacher;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_IsTeacher, _Ac_IsTeacher, value);
    				this._Ac_IsTeacher = value;
    			}
    		}
    		
    		public String Ac_Qus {
    			get {
    				return this._Ac_Qus;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Qus, _Ac_Qus, value);
    				this._Ac_Qus = value;
    			}
    		}
    		
    		public String Ac_Ans {
    			get {
    				return this._Ac_Ans;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Ans, _Ac_Ans, value);
    				this._Ac_Ans = value;
    			}
    		}
    		
    		public DateTime Ac_RegTime {
    			get {
    				return this._Ac_RegTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_RegTime, _Ac_RegTime, value);
    				this._Ac_RegTime = value;
    			}
    		}
    		
    		public DateTime Ac_OutTime {
    			get {
    				return this._Ac_OutTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_OutTime, _Ac_OutTime, value);
    				this._Ac_OutTime = value;
    			}
    		}
    		
    		public DateTime Ac_LastTime {
    			get {
    				return this._Ac_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_LastTime, _Ac_LastTime, value);
    				this._Ac_LastTime = value;
    			}
    		}
    		
    		public String Ac_LastIP {
    			get {
    				return this._Ac_LastIP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_LastIP, _Ac_LastIP, value);
    				this._Ac_LastIP = value;
    			}
    		}
    		
    		public String Ac_Tel {
    			get {
    				return this._Ac_Tel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Tel, _Ac_Tel, value);
    				this._Ac_Tel = value;
    			}
    		}
    		
    		public Boolean Ac_IsOpenTel {
    			get {
    				return this._Ac_IsOpenTel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_IsOpenTel, _Ac_IsOpenTel, value);
    				this._Ac_IsOpenTel = value;
    			}
    		}
    		
    		public String Ac_MobiTel1 {
    			get {
    				return this._Ac_MobiTel1;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_MobiTel1, _Ac_MobiTel1, value);
    				this._Ac_MobiTel1 = value;
    			}
    		}
    		
    		public String Ac_MobiTel2 {
    			get {
    				return this._Ac_MobiTel2;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_MobiTel2, _Ac_MobiTel2, value);
    				this._Ac_MobiTel2 = value;
    			}
    		}
    		
    		public Boolean Ac_IsOpenMobile {
    			get {
    				return this._Ac_IsOpenMobile;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_IsOpenMobile, _Ac_IsOpenMobile, value);
    				this._Ac_IsOpenMobile = value;
    			}
    		}
    		
    		public String Ac_Email {
    			get {
    				return this._Ac_Email;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Email, _Ac_Email, value);
    				this._Ac_Email = value;
    			}
    		}
    		
    		public String Ac_Qq {
    			get {
    				return this._Ac_Qq;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Qq, _Ac_Qq, value);
    				this._Ac_Qq = value;
    			}
    		}
    		
    		public String Ac_Weixin {
    			get {
    				return this._Ac_Weixin;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Weixin, _Ac_Weixin, value);
    				this._Ac_Weixin = value;
    			}
    		}
    		
    		public String Ac_CheckUID {
    			get {
    				return this._Ac_CheckUID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_CheckUID, _Ac_CheckUID, value);
    				this._Ac_CheckUID = value;
    			}
    		}
    		
    		public String Ac_UID {
    			get {
    				return this._Ac_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_UID, _Ac_UID, value);
    				this._Ac_UID = value;
    			}
    		}
    		
    		public Int32 Org_ID {
    			get {
    				return this._Org_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_ID, _Org_ID, value);
    				this._Org_ID = value;
    			}
    		}
    		
    		public String Ac_Zip {
    			get {
    				return this._Ac_Zip;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Zip, _Ac_Zip, value);
    				this._Ac_Zip = value;
    			}
    		}
    		
    		public String Ac_LinkMan {
    			get {
    				return this._Ac_LinkMan;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_LinkMan, _Ac_LinkMan, value);
    				this._Ac_LinkMan = value;
    			}
    		}
    		
    		public String Ac_LinkManPhone {
    			get {
    				return this._Ac_LinkManPhone;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_LinkManPhone, _Ac_LinkManPhone, value);
    				this._Ac_LinkManPhone = value;
    			}
    		}
    		
    		public String Ac_Intro {
    			get {
    				return this._Ac_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Intro, _Ac_Intro, value);
    				this._Ac_Intro = value;
    			}
    		}
    		
    		public String Ac_Major {
    			get {
    				return this._Ac_Major;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Major, _Ac_Major, value);
    				this._Ac_Major = value;
    			}
    		}
    		
    		public String Ac_Education {
    			get {
    				return this._Ac_Education;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Education, _Ac_Education, value);
    				this._Ac_Education = value;
    			}
    		}
    		
    		public String Ac_Native {
    			get {
    				return this._Ac_Native;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Native, _Ac_Native, value);
    				this._Ac_Native = value;
    			}
    		}
    		
    		public String Ac_Nation {
    			get {
    				return this._Ac_Nation;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Nation, _Ac_Nation, value);
    				this._Ac_Nation = value;
    			}
    		}
    		
    		public String Ac_CodeNumber {
    			get {
    				return this._Ac_CodeNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_CodeNumber, _Ac_CodeNumber, value);
    				this._Ac_CodeNumber = value;
    			}
    		}
    		
    		public String Ac_Address {
    			get {
    				return this._Ac_Address;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Address, _Ac_Address, value);
    				this._Ac_Address = value;
    			}
    		}
    		
    		public String Ac_AddrContact {
    			get {
    				return this._Ac_AddrContact;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_AddrContact, _Ac_AddrContact, value);
    				this._Ac_AddrContact = value;
    			}
    		}
    		
    		public Int32 Dep_Id {
    			get {
    				return this._Dep_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Id, _Dep_Id, value);
    				this._Dep_Id = value;
    			}
    		}
    		
    		public Int32 Sts_ID {
    			get {
    				return this._Sts_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sts_ID, _Sts_ID, value);
    				this._Sts_ID = value;
    			}
    		}
    		
    		public String Sts_Name {
    			get {
    				return this._Sts_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sts_Name, _Sts_Name, value);
    				this._Sts_Name = value;
    			}
    		}
    		
    		public Int32 Ac_CurrCourse {
    			get {
    				return this._Ac_CurrCourse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_CurrCourse, _Ac_CurrCourse, value);
    				this._Ac_CurrCourse = value;
    			}
    		}
    		
    		public Int32 Ac_PointAmount {
    			get {
    				return this._Ac_PointAmount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_PointAmount, _Ac_PointAmount, value);
    				this._Ac_PointAmount = value;
    			}
    		}
    		
    		public String Ac_School {
    			get {
    				return this._Ac_School;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_School, _Ac_School, value);
    				this._Ac_School = value;
    			}
    		}
    		
    		public String Ac_QqOpenID {
    			get {
    				return this._Ac_QqOpenID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_QqOpenID, _Ac_QqOpenID, value);
    				this._Ac_QqOpenID = value;
    			}
    		}
    		
    		public String Ac_WeixinOpenID {
    			get {
    				return this._Ac_WeixinOpenID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_WeixinOpenID, _Ac_WeixinOpenID, value);
    				this._Ac_WeixinOpenID = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Accounts>("Accounts");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Ac_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ac_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ac_ID,
    					_.Ac_PID,
    					_.Ac_AccName,
    					_.Ac_Pw,
    					_.Ac_Name,
    					_.Ac_Pinyin,
    					_.Ac_IDCardNumber,
    					_.Ac_Signature,
    					_.Ac_Age,
    					_.Ac_Sex,
    					_.Ac_Photo,
    					_.Ac_Money,
    					_.Ac_Point,
    					_.Ac_Coupon,
    					_.Ac_Birthday,
    					_.Ac_IsUse,
    					_.Ac_IsPass,
    					_.Ac_IsTeacher,
    					_.Ac_Qus,
    					_.Ac_Ans,
    					_.Ac_RegTime,
    					_.Ac_OutTime,
    					_.Ac_LastTime,
    					_.Ac_LastIP,
    					_.Ac_Tel,
    					_.Ac_IsOpenTel,
    					_.Ac_MobiTel1,
    					_.Ac_MobiTel2,
    					_.Ac_IsOpenMobile,
    					_.Ac_Email,
    					_.Ac_Qq,
    					_.Ac_Weixin,
    					_.Ac_CheckUID,
    					_.Ac_UID,
    					_.Org_ID,
    					_.Ac_Zip,
    					_.Ac_LinkMan,
    					_.Ac_LinkManPhone,
    					_.Ac_Intro,
    					_.Ac_Major,
    					_.Ac_Education,
    					_.Ac_Native,
    					_.Ac_Nation,
    					_.Ac_CodeNumber,
    					_.Ac_Address,
    					_.Ac_AddrContact,
    					_.Dep_Id,
    					_.Sts_ID,
    					_.Sts_Name,
    					_.Ac_CurrCourse,
    					_.Ac_PointAmount,
    					_.Ac_School,
    					_.Ac_QqOpenID,
    					_.Ac_WeixinOpenID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Ac_ID,
    					this._Ac_PID,
    					this._Ac_AccName,
    					this._Ac_Pw,
    					this._Ac_Name,
    					this._Ac_Pinyin,
    					this._Ac_IDCardNumber,
    					this._Ac_Signature,
    					this._Ac_Age,
    					this._Ac_Sex,
    					this._Ac_Photo,
    					this._Ac_Money,
    					this._Ac_Point,
    					this._Ac_Coupon,
    					this._Ac_Birthday,
    					this._Ac_IsUse,
    					this._Ac_IsPass,
    					this._Ac_IsTeacher,
    					this._Ac_Qus,
    					this._Ac_Ans,
    					this._Ac_RegTime,
    					this._Ac_OutTime,
    					this._Ac_LastTime,
    					this._Ac_LastIP,
    					this._Ac_Tel,
    					this._Ac_IsOpenTel,
    					this._Ac_MobiTel1,
    					this._Ac_MobiTel2,
    					this._Ac_IsOpenMobile,
    					this._Ac_Email,
    					this._Ac_Qq,
    					this._Ac_Weixin,
    					this._Ac_CheckUID,
    					this._Ac_UID,
    					this._Org_ID,
    					this._Ac_Zip,
    					this._Ac_LinkMan,
    					this._Ac_LinkManPhone,
    					this._Ac_Intro,
    					this._Ac_Major,
    					this._Ac_Education,
    					this._Ac_Native,
    					this._Ac_Nation,
    					this._Ac_CodeNumber,
    					this._Ac_Address,
    					this._Ac_AddrContact,
    					this._Dep_Id,
    					this._Sts_ID,
    					this._Sts_Name,
    					this._Ac_CurrCourse,
    					this._Ac_PointAmount,
    					this._Ac_School,
    					this._Ac_QqOpenID,
    					this._Ac_WeixinOpenID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_PID))) {
    				this._Ac_PID = reader.GetInt32(_.Ac_PID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_AccName))) {
    				this._Ac_AccName = reader.GetString(_.Ac_AccName);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Pw))) {
    				this._Ac_Pw = reader.GetString(_.Ac_Pw);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Name))) {
    				this._Ac_Name = reader.GetString(_.Ac_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Pinyin))) {
    				this._Ac_Pinyin = reader.GetString(_.Ac_Pinyin);
    			}
    			if ((false == reader.IsDBNull(_.Ac_IDCardNumber))) {
    				this._Ac_IDCardNumber = reader.GetString(_.Ac_IDCardNumber);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Signature))) {
    				this._Ac_Signature = reader.GetString(_.Ac_Signature);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Age))) {
    				this._Ac_Age = reader.GetInt32(_.Ac_Age);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Sex))) {
    				this._Ac_Sex = reader.GetInt32(_.Ac_Sex);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Photo))) {
    				this._Ac_Photo = reader.GetString(_.Ac_Photo);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Money))) {
    				this._Ac_Money = reader.GetDecimal(_.Ac_Money);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Point))) {
    				this._Ac_Point = reader.GetInt32(_.Ac_Point);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Coupon))) {
    				this._Ac_Coupon = reader.GetInt32(_.Ac_Coupon);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Birthday))) {
    				this._Ac_Birthday = reader.GetDateTime(_.Ac_Birthday);
    			}
    			if ((false == reader.IsDBNull(_.Ac_IsUse))) {
    				this._Ac_IsUse = reader.GetBoolean(_.Ac_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Ac_IsPass))) {
    				this._Ac_IsPass = reader.GetBoolean(_.Ac_IsPass);
    			}
    			if ((false == reader.IsDBNull(_.Ac_IsTeacher))) {
    				this._Ac_IsTeacher = reader.GetBoolean(_.Ac_IsTeacher);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Qus))) {
    				this._Ac_Qus = reader.GetString(_.Ac_Qus);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Ans))) {
    				this._Ac_Ans = reader.GetString(_.Ac_Ans);
    			}
    			if ((false == reader.IsDBNull(_.Ac_RegTime))) {
    				this._Ac_RegTime = reader.GetDateTime(_.Ac_RegTime);
    			}
    			if ((false == reader.IsDBNull(_.Ac_OutTime))) {
    				this._Ac_OutTime = reader.GetDateTime(_.Ac_OutTime);
    			}
    			if ((false == reader.IsDBNull(_.Ac_LastTime))) {
    				this._Ac_LastTime = reader.GetDateTime(_.Ac_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Ac_LastIP))) {
    				this._Ac_LastIP = reader.GetString(_.Ac_LastIP);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Tel))) {
    				this._Ac_Tel = reader.GetString(_.Ac_Tel);
    			}
    			if ((false == reader.IsDBNull(_.Ac_IsOpenTel))) {
    				this._Ac_IsOpenTel = reader.GetBoolean(_.Ac_IsOpenTel);
    			}
    			if ((false == reader.IsDBNull(_.Ac_MobiTel1))) {
    				this._Ac_MobiTel1 = reader.GetString(_.Ac_MobiTel1);
    			}
    			if ((false == reader.IsDBNull(_.Ac_MobiTel2))) {
    				this._Ac_MobiTel2 = reader.GetString(_.Ac_MobiTel2);
    			}
    			if ((false == reader.IsDBNull(_.Ac_IsOpenMobile))) {
    				this._Ac_IsOpenMobile = reader.GetBoolean(_.Ac_IsOpenMobile);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Email))) {
    				this._Ac_Email = reader.GetString(_.Ac_Email);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Qq))) {
    				this._Ac_Qq = reader.GetString(_.Ac_Qq);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Weixin))) {
    				this._Ac_Weixin = reader.GetString(_.Ac_Weixin);
    			}
    			if ((false == reader.IsDBNull(_.Ac_CheckUID))) {
    				this._Ac_CheckUID = reader.GetString(_.Ac_CheckUID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_UID))) {
    				this._Ac_UID = reader.GetString(_.Ac_UID);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Zip))) {
    				this._Ac_Zip = reader.GetString(_.Ac_Zip);
    			}
    			if ((false == reader.IsDBNull(_.Ac_LinkMan))) {
    				this._Ac_LinkMan = reader.GetString(_.Ac_LinkMan);
    			}
    			if ((false == reader.IsDBNull(_.Ac_LinkManPhone))) {
    				this._Ac_LinkManPhone = reader.GetString(_.Ac_LinkManPhone);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Intro))) {
    				this._Ac_Intro = reader.GetString(_.Ac_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Major))) {
    				this._Ac_Major = reader.GetString(_.Ac_Major);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Education))) {
    				this._Ac_Education = reader.GetString(_.Ac_Education);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Native))) {
    				this._Ac_Native = reader.GetString(_.Ac_Native);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Nation))) {
    				this._Ac_Nation = reader.GetString(_.Ac_Nation);
    			}
    			if ((false == reader.IsDBNull(_.Ac_CodeNumber))) {
    				this._Ac_CodeNumber = reader.GetString(_.Ac_CodeNumber);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Address))) {
    				this._Ac_Address = reader.GetString(_.Ac_Address);
    			}
    			if ((false == reader.IsDBNull(_.Ac_AddrContact))) {
    				this._Ac_AddrContact = reader.GetString(_.Ac_AddrContact);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Id))) {
    				this._Dep_Id = reader.GetInt32(_.Dep_Id);
    			}
    			if ((false == reader.IsDBNull(_.Sts_ID))) {
    				this._Sts_ID = reader.GetInt32(_.Sts_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sts_Name))) {
    				this._Sts_Name = reader.GetString(_.Sts_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ac_CurrCourse))) {
    				this._Ac_CurrCourse = reader.GetInt32(_.Ac_CurrCourse);
    			}
    			if ((false == reader.IsDBNull(_.Ac_PointAmount))) {
    				this._Ac_PointAmount = reader.GetInt32(_.Ac_PointAmount);
    			}
    			if ((false == reader.IsDBNull(_.Ac_School))) {
    				this._Ac_School = reader.GetString(_.Ac_School);
    			}
    			if ((false == reader.IsDBNull(_.Ac_QqOpenID))) {
    				this._Ac_QqOpenID = reader.GetString(_.Ac_QqOpenID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_WeixinOpenID))) {
    				this._Ac_WeixinOpenID = reader.GetString(_.Ac_WeixinOpenID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Accounts).IsAssignableFrom(obj.GetType()))) {
    				return false;
    			}
    			if ((((object)(this)) == ((object)(obj)))) {
    				return true;
    			}
    			return false;
    		}
    		
    		public class _ {
    			
    			/// <summary>
    			/// 表示选择所有列，与*等同
    			/// </summary>
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Accounts>();
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<Accounts>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_PID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_PID = new WeiSha.Data.Field<Accounts>("Ac_PID");
    			
    			/// <summary>
    			/// 字段名：Ac_AccName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_AccName = new WeiSha.Data.Field<Accounts>("Ac_AccName");
    			
    			/// <summary>
    			/// 字段名：Ac_Pw - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Pw = new WeiSha.Data.Field<Accounts>("Ac_Pw");
    			
    			/// <summary>
    			/// 字段名：Ac_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Name = new WeiSha.Data.Field<Accounts>("Ac_Name");
    			
    			/// <summary>
    			/// 字段名：Ac_Pinyin - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Pinyin = new WeiSha.Data.Field<Accounts>("Ac_Pinyin");
    			
    			/// <summary>
    			/// 字段名：Ac_IDCardNumber - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_IDCardNumber = new WeiSha.Data.Field<Accounts>("Ac_IDCardNumber");
    			
    			/// <summary>
    			/// 字段名：Ac_Signature - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Signature = new WeiSha.Data.Field<Accounts>("Ac_Signature");
    			
    			/// <summary>
    			/// 字段名：Ac_Age - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Age = new WeiSha.Data.Field<Accounts>("Ac_Age");
    			
    			/// <summary>
    			/// 字段名：Ac_Sex - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Sex = new WeiSha.Data.Field<Accounts>("Ac_Sex");
    			
    			/// <summary>
    			/// 字段名：Ac_Photo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Photo = new WeiSha.Data.Field<Accounts>("Ac_Photo");
    			
    			/// <summary>
    			/// 字段名：Ac_Money - 数据类型：Decimal
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Money = new WeiSha.Data.Field<Accounts>("Ac_Money");
    			
    			/// <summary>
    			/// 字段名：Ac_Point - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Point = new WeiSha.Data.Field<Accounts>("Ac_Point");
    			
    			/// <summary>
    			/// 字段名：Ac_Coupon - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Coupon = new WeiSha.Data.Field<Accounts>("Ac_Coupon");
    			
    			/// <summary>
    			/// 字段名：Ac_Birthday - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Birthday = new WeiSha.Data.Field<Accounts>("Ac_Birthday");
    			
    			/// <summary>
    			/// 字段名：Ac_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ac_IsUse = new WeiSha.Data.Field<Accounts>("Ac_IsUse");
    			
    			/// <summary>
    			/// 字段名：Ac_IsPass - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ac_IsPass = new WeiSha.Data.Field<Accounts>("Ac_IsPass");
    			
    			/// <summary>
    			/// 字段名：Ac_IsTeacher - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ac_IsTeacher = new WeiSha.Data.Field<Accounts>("Ac_IsTeacher");
    			
    			/// <summary>
    			/// 字段名：Ac_Qus - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Qus = new WeiSha.Data.Field<Accounts>("Ac_Qus");
    			
    			/// <summary>
    			/// 字段名：Ac_Ans - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Ans = new WeiSha.Data.Field<Accounts>("Ac_Ans");
    			
    			/// <summary>
    			/// 字段名：Ac_RegTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Ac_RegTime = new WeiSha.Data.Field<Accounts>("Ac_RegTime");
    			
    			/// <summary>
    			/// 字段名：Ac_OutTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Ac_OutTime = new WeiSha.Data.Field<Accounts>("Ac_OutTime");
    			
    			/// <summary>
    			/// 字段名：Ac_LastTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Ac_LastTime = new WeiSha.Data.Field<Accounts>("Ac_LastTime");
    			
    			/// <summary>
    			/// 字段名：Ac_LastIP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_LastIP = new WeiSha.Data.Field<Accounts>("Ac_LastIP");
    			
    			/// <summary>
    			/// 字段名：Ac_Tel - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Tel = new WeiSha.Data.Field<Accounts>("Ac_Tel");
    			
    			/// <summary>
    			/// 字段名：Ac_IsOpenTel - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ac_IsOpenTel = new WeiSha.Data.Field<Accounts>("Ac_IsOpenTel");
    			
    			/// <summary>
    			/// 字段名：Ac_MobiTel1 - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_MobiTel1 = new WeiSha.Data.Field<Accounts>("Ac_MobiTel1");
    			
    			/// <summary>
    			/// 字段名：Ac_MobiTel2 - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_MobiTel2 = new WeiSha.Data.Field<Accounts>("Ac_MobiTel2");
    			
    			/// <summary>
    			/// 字段名：Ac_IsOpenMobile - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ac_IsOpenMobile = new WeiSha.Data.Field<Accounts>("Ac_IsOpenMobile");
    			
    			/// <summary>
    			/// 字段名：Ac_Email - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Email = new WeiSha.Data.Field<Accounts>("Ac_Email");
    			
    			/// <summary>
    			/// 字段名：Ac_Qq - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Qq = new WeiSha.Data.Field<Accounts>("Ac_Qq");
    			
    			/// <summary>
    			/// 字段名：Ac_Weixin - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Weixin = new WeiSha.Data.Field<Accounts>("Ac_Weixin");
    			
    			/// <summary>
    			/// 字段名：Ac_CheckUID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_CheckUID = new WeiSha.Data.Field<Accounts>("Ac_CheckUID");
    			
    			/// <summary>
    			/// 字段名：Ac_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_UID = new WeiSha.Data.Field<Accounts>("Ac_UID");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Accounts>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_Zip - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Zip = new WeiSha.Data.Field<Accounts>("Ac_Zip");
    			
    			/// <summary>
    			/// 字段名：Ac_LinkMan - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_LinkMan = new WeiSha.Data.Field<Accounts>("Ac_LinkMan");
    			
    			/// <summary>
    			/// 字段名：Ac_LinkManPhone - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_LinkManPhone = new WeiSha.Data.Field<Accounts>("Ac_LinkManPhone");
    			
    			/// <summary>
    			/// 字段名：Ac_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Intro = new WeiSha.Data.Field<Accounts>("Ac_Intro");
    			
    			/// <summary>
    			/// 字段名：Ac_Major - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Major = new WeiSha.Data.Field<Accounts>("Ac_Major");
    			
    			/// <summary>
    			/// 字段名：Ac_Education - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Education = new WeiSha.Data.Field<Accounts>("Ac_Education");
    			
    			/// <summary>
    			/// 字段名：Ac_Native - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Native = new WeiSha.Data.Field<Accounts>("Ac_Native");
    			
    			/// <summary>
    			/// 字段名：Ac_Nation - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Nation = new WeiSha.Data.Field<Accounts>("Ac_Nation");
    			
    			/// <summary>
    			/// 字段名：Ac_CodeNumber - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_CodeNumber = new WeiSha.Data.Field<Accounts>("Ac_CodeNumber");
    			
    			/// <summary>
    			/// 字段名：Ac_Address - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Address = new WeiSha.Data.Field<Accounts>("Ac_Address");
    			
    			/// <summary>
    			/// 字段名：Ac_AddrContact - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_AddrContact = new WeiSha.Data.Field<Accounts>("Ac_AddrContact");
    			
    			/// <summary>
    			/// 字段名：Dep_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Id = new WeiSha.Data.Field<Accounts>("Dep_Id");
    			
    			/// <summary>
    			/// 字段名：Sts_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sts_ID = new WeiSha.Data.Field<Accounts>("Sts_ID");
    			
    			/// <summary>
    			/// 字段名：Sts_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sts_Name = new WeiSha.Data.Field<Accounts>("Sts_Name");
    			
    			/// <summary>
    			/// 字段名：Ac_CurrCourse - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_CurrCourse = new WeiSha.Data.Field<Accounts>("Ac_CurrCourse");
    			
    			/// <summary>
    			/// 字段名：Ac_PointAmount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_PointAmount = new WeiSha.Data.Field<Accounts>("Ac_PointAmount");
    			
    			/// <summary>
    			/// 字段名：Ac_School - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_School = new WeiSha.Data.Field<Accounts>("Ac_School");
    			
    			/// <summary>
    			/// 字段名：Ac_QqOpenID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_QqOpenID = new WeiSha.Data.Field<Accounts>("Ac_QqOpenID");
    			
    			/// <summary>
    			/// 字段名：Ac_WeixinOpenID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_WeixinOpenID = new WeiSha.Data.Field<Accounts>("Ac_WeixinOpenID");
    		}
    	}
    }
    