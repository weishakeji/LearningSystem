namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Student 主键列：St_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Student : WeiSha.Data.Entity {
    		
    		protected Int32 _St_ID;
    		
    		protected String _St_AccName;
    		
    		protected String _St_Pw;
    		
    		protected String _St_Qus;
    		
    		protected String _St_Anwser;
    		
    		protected String _St_Name;
    		
    		protected String _St_Pinyin;
    		
    		protected Int32 _St_Age;
    		
    		protected DateTime _St_Birthday;
    		
    		protected String _St_IDCardNumber;
    		
    		protected String _St_Photo;
    		
    		protected String _St_Signature;
    		
    		protected String _St_Intro;
    		
    		protected String _St_Major;
    		
    		protected String _St_Education;
    		
    		protected Int32 _St_Sex;
    		
    		protected String _St_Native;
    		
    		protected String _St_Nation;
    		
    		protected String _St_CodeNumber;
    		
    		protected String _St_Address;
    		
    		protected String _St_AddrContact;
    		
    		protected String _St_Phone;
    		
    		protected Boolean _St_IsOpenPhone;
    		
    		protected String _St_PhoneMobi;
    		
    		protected Boolean _St_IsOpenMobi;
    		
    		protected String _St_Email;
    		
    		protected String _St_Qq;
    		
    		protected String _St_Weixin;
    		
    		protected String _St_Zip;
    		
    		protected String _St_LinkMan;
    		
    		protected String _St_LinkManPhone;
    		
    		protected Boolean _St_IsPass;
    		
    		protected Boolean _St_IsUse;
    		
    		protected DateTime _St_RegTime;
    		
    		protected DateTime _St_LastTime;
    		
    		protected DateTime _St_CrtTime;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Int32 _Sts_ID;
    		
    		protected String _Sts_Name;
    		
    		protected Single _St_Money;
    		
    		protected String _St_LastIP;
    		
    		protected String _Dep_CnName;
    		
    		protected Int32 _Dep_Id;
    		
    		protected String _St_CheckUID;
    		
    		protected Int32 _St_CurrCourse;
    		
    		protected Int32 _Ac_ID;
    		
    		protected String _Ac_UID;
    		
    		public Int32 St_ID {
    			get {
    				return this._St_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_ID, _St_ID, value);
    				this._St_ID = value;
    			}
    		}
    		
    		public String St_AccName {
    			get {
    				return this._St_AccName;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_AccName, _St_AccName, value);
    				this._St_AccName = value;
    			}
    		}
    		
    		public String St_Pw {
    			get {
    				return this._St_Pw;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Pw, _St_Pw, value);
    				this._St_Pw = value;
    			}
    		}
    		
    		public String St_Qus {
    			get {
    				return this._St_Qus;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Qus, _St_Qus, value);
    				this._St_Qus = value;
    			}
    		}
    		
    		public String St_Anwser {
    			get {
    				return this._St_Anwser;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Anwser, _St_Anwser, value);
    				this._St_Anwser = value;
    			}
    		}
    		
    		public String St_Name {
    			get {
    				return this._St_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Name, _St_Name, value);
    				this._St_Name = value;
    			}
    		}
    		
    		public String St_Pinyin {
    			get {
    				return this._St_Pinyin;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Pinyin, _St_Pinyin, value);
    				this._St_Pinyin = value;
    			}
    		}
    		
    		public Int32 St_Age {
    			get {
    				return this._St_Age;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Age, _St_Age, value);
    				this._St_Age = value;
    			}
    		}
    		
    		public DateTime St_Birthday {
    			get {
    				return this._St_Birthday;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Birthday, _St_Birthday, value);
    				this._St_Birthday = value;
    			}
    		}
    		
    		public String St_IDCardNumber {
    			get {
    				return this._St_IDCardNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_IDCardNumber, _St_IDCardNumber, value);
    				this._St_IDCardNumber = value;
    			}
    		}
    		
    		public String St_Photo {
    			get {
    				return this._St_Photo;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Photo, _St_Photo, value);
    				this._St_Photo = value;
    			}
    		}
    		
    		public String St_Signature {
    			get {
    				return this._St_Signature;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Signature, _St_Signature, value);
    				this._St_Signature = value;
    			}
    		}
    		
    		public String St_Intro {
    			get {
    				return this._St_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Intro, _St_Intro, value);
    				this._St_Intro = value;
    			}
    		}
    		
    		public String St_Major {
    			get {
    				return this._St_Major;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Major, _St_Major, value);
    				this._St_Major = value;
    			}
    		}
    		
    		public String St_Education {
    			get {
    				return this._St_Education;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Education, _St_Education, value);
    				this._St_Education = value;
    			}
    		}
    		
    		public Int32 St_Sex {
    			get {
    				return this._St_Sex;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Sex, _St_Sex, value);
    				this._St_Sex = value;
    			}
    		}
    		
    		public String St_Native {
    			get {
    				return this._St_Native;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Native, _St_Native, value);
    				this._St_Native = value;
    			}
    		}
    		
    		public String St_Nation {
    			get {
    				return this._St_Nation;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Nation, _St_Nation, value);
    				this._St_Nation = value;
    			}
    		}
    		
    		public String St_CodeNumber {
    			get {
    				return this._St_CodeNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_CodeNumber, _St_CodeNumber, value);
    				this._St_CodeNumber = value;
    			}
    		}
    		
    		public String St_Address {
    			get {
    				return this._St_Address;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Address, _St_Address, value);
    				this._St_Address = value;
    			}
    		}
    		
    		public String St_AddrContact {
    			get {
    				return this._St_AddrContact;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_AddrContact, _St_AddrContact, value);
    				this._St_AddrContact = value;
    			}
    		}
    		
    		public String St_Phone {
    			get {
    				return this._St_Phone;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Phone, _St_Phone, value);
    				this._St_Phone = value;
    			}
    		}
    		
    		public Boolean St_IsOpenPhone {
    			get {
    				return this._St_IsOpenPhone;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_IsOpenPhone, _St_IsOpenPhone, value);
    				this._St_IsOpenPhone = value;
    			}
    		}
    		
    		public String St_PhoneMobi {
    			get {
    				return this._St_PhoneMobi;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_PhoneMobi, _St_PhoneMobi, value);
    				this._St_PhoneMobi = value;
    			}
    		}
    		
    		public Boolean St_IsOpenMobi {
    			get {
    				return this._St_IsOpenMobi;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_IsOpenMobi, _St_IsOpenMobi, value);
    				this._St_IsOpenMobi = value;
    			}
    		}
    		
    		public String St_Email {
    			get {
    				return this._St_Email;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Email, _St_Email, value);
    				this._St_Email = value;
    			}
    		}
    		
    		public String St_Qq {
    			get {
    				return this._St_Qq;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Qq, _St_Qq, value);
    				this._St_Qq = value;
    			}
    		}
    		
    		public String St_Weixin {
    			get {
    				return this._St_Weixin;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Weixin, _St_Weixin, value);
    				this._St_Weixin = value;
    			}
    		}
    		
    		public String St_Zip {
    			get {
    				return this._St_Zip;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Zip, _St_Zip, value);
    				this._St_Zip = value;
    			}
    		}
    		
    		public String St_LinkMan {
    			get {
    				return this._St_LinkMan;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_LinkMan, _St_LinkMan, value);
    				this._St_LinkMan = value;
    			}
    		}
    		
    		public String St_LinkManPhone {
    			get {
    				return this._St_LinkManPhone;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_LinkManPhone, _St_LinkManPhone, value);
    				this._St_LinkManPhone = value;
    			}
    		}
    		
    		public Boolean St_IsPass {
    			get {
    				return this._St_IsPass;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_IsPass, _St_IsPass, value);
    				this._St_IsPass = value;
    			}
    		}
    		
    		public Boolean St_IsUse {
    			get {
    				return this._St_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_IsUse, _St_IsUse, value);
    				this._St_IsUse = value;
    			}
    		}
    		
    		public DateTime St_RegTime {
    			get {
    				return this._St_RegTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_RegTime, _St_RegTime, value);
    				this._St_RegTime = value;
    			}
    		}
    		
    		public DateTime St_LastTime {
    			get {
    				return this._St_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_LastTime, _St_LastTime, value);
    				this._St_LastTime = value;
    			}
    		}
    		
    		public DateTime St_CrtTime {
    			get {
    				return this._St_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_CrtTime, _St_CrtTime, value);
    				this._St_CrtTime = value;
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
    		
    		public String Org_Name {
    			get {
    				return this._Org_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Name, _Org_Name, value);
    				this._Org_Name = value;
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
    		
    		public Single St_Money {
    			get {
    				return this._St_Money;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Money, _St_Money, value);
    				this._St_Money = value;
    			}
    		}
    		
    		public String St_LastIP {
    			get {
    				return this._St_LastIP;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_LastIP, _St_LastIP, value);
    				this._St_LastIP = value;
    			}
    		}
    		
    		public String Dep_CnName {
    			get {
    				return this._Dep_CnName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_CnName, _Dep_CnName, value);
    				this._Dep_CnName = value;
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
    		
    		public String St_CheckUID {
    			get {
    				return this._St_CheckUID;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_CheckUID, _St_CheckUID, value);
    				this._St_CheckUID = value;
    			}
    		}
    		
    		public Int32 St_CurrCourse {
    			get {
    				return this._St_CurrCourse;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_CurrCourse, _St_CurrCourse, value);
    				this._St_CurrCourse = value;
    			}
    		}
    		
    		public Int32 Ac_ID {
    			get {
    				return this._Ac_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_ID, _Ac_ID, value);
    				this._Ac_ID = value;
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
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Student>("Student");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.St_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.St_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.St_ID,
    					_.St_AccName,
    					_.St_Pw,
    					_.St_Qus,
    					_.St_Anwser,
    					_.St_Name,
    					_.St_Pinyin,
    					_.St_Age,
    					_.St_Birthday,
    					_.St_IDCardNumber,
    					_.St_Photo,
    					_.St_Signature,
    					_.St_Intro,
    					_.St_Major,
    					_.St_Education,
    					_.St_Sex,
    					_.St_Native,
    					_.St_Nation,
    					_.St_CodeNumber,
    					_.St_Address,
    					_.St_AddrContact,
    					_.St_Phone,
    					_.St_IsOpenPhone,
    					_.St_PhoneMobi,
    					_.St_IsOpenMobi,
    					_.St_Email,
    					_.St_Qq,
    					_.St_Weixin,
    					_.St_Zip,
    					_.St_LinkMan,
    					_.St_LinkManPhone,
    					_.St_IsPass,
    					_.St_IsUse,
    					_.St_RegTime,
    					_.St_LastTime,
    					_.St_CrtTime,
    					_.Org_ID,
    					_.Org_Name,
    					_.Sts_ID,
    					_.Sts_Name,
    					_.St_Money,
    					_.St_LastIP,
    					_.Dep_CnName,
    					_.Dep_Id,
    					_.St_CheckUID,
    					_.St_CurrCourse,
    					_.Ac_ID,
    					_.Ac_UID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._St_ID,
    					this._St_AccName,
    					this._St_Pw,
    					this._St_Qus,
    					this._St_Anwser,
    					this._St_Name,
    					this._St_Pinyin,
    					this._St_Age,
    					this._St_Birthday,
    					this._St_IDCardNumber,
    					this._St_Photo,
    					this._St_Signature,
    					this._St_Intro,
    					this._St_Major,
    					this._St_Education,
    					this._St_Sex,
    					this._St_Native,
    					this._St_Nation,
    					this._St_CodeNumber,
    					this._St_Address,
    					this._St_AddrContact,
    					this._St_Phone,
    					this._St_IsOpenPhone,
    					this._St_PhoneMobi,
    					this._St_IsOpenMobi,
    					this._St_Email,
    					this._St_Qq,
    					this._St_Weixin,
    					this._St_Zip,
    					this._St_LinkMan,
    					this._St_LinkManPhone,
    					this._St_IsPass,
    					this._St_IsUse,
    					this._St_RegTime,
    					this._St_LastTime,
    					this._St_CrtTime,
    					this._Org_ID,
    					this._Org_Name,
    					this._Sts_ID,
    					this._Sts_Name,
    					this._St_Money,
    					this._St_LastIP,
    					this._Dep_CnName,
    					this._Dep_Id,
    					this._St_CheckUID,
    					this._St_CurrCourse,
    					this._Ac_ID,
    					this._Ac_UID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.St_ID))) {
    				this._St_ID = reader.GetInt32(_.St_ID);
    			}
    			if ((false == reader.IsDBNull(_.St_AccName))) {
    				this._St_AccName = reader.GetString(_.St_AccName);
    			}
    			if ((false == reader.IsDBNull(_.St_Pw))) {
    				this._St_Pw = reader.GetString(_.St_Pw);
    			}
    			if ((false == reader.IsDBNull(_.St_Qus))) {
    				this._St_Qus = reader.GetString(_.St_Qus);
    			}
    			if ((false == reader.IsDBNull(_.St_Anwser))) {
    				this._St_Anwser = reader.GetString(_.St_Anwser);
    			}
    			if ((false == reader.IsDBNull(_.St_Name))) {
    				this._St_Name = reader.GetString(_.St_Name);
    			}
    			if ((false == reader.IsDBNull(_.St_Pinyin))) {
    				this._St_Pinyin = reader.GetString(_.St_Pinyin);
    			}
    			if ((false == reader.IsDBNull(_.St_Age))) {
    				this._St_Age = reader.GetInt32(_.St_Age);
    			}
    			if ((false == reader.IsDBNull(_.St_Birthday))) {
    				this._St_Birthday = reader.GetDateTime(_.St_Birthday);
    			}
    			if ((false == reader.IsDBNull(_.St_IDCardNumber))) {
    				this._St_IDCardNumber = reader.GetString(_.St_IDCardNumber);
    			}
    			if ((false == reader.IsDBNull(_.St_Photo))) {
    				this._St_Photo = reader.GetString(_.St_Photo);
    			}
    			if ((false == reader.IsDBNull(_.St_Signature))) {
    				this._St_Signature = reader.GetString(_.St_Signature);
    			}
    			if ((false == reader.IsDBNull(_.St_Intro))) {
    				this._St_Intro = reader.GetString(_.St_Intro);
    			}
    			if ((false == reader.IsDBNull(_.St_Major))) {
    				this._St_Major = reader.GetString(_.St_Major);
    			}
    			if ((false == reader.IsDBNull(_.St_Education))) {
    				this._St_Education = reader.GetString(_.St_Education);
    			}
    			if ((false == reader.IsDBNull(_.St_Sex))) {
    				this._St_Sex = reader.GetInt32(_.St_Sex);
    			}
    			if ((false == reader.IsDBNull(_.St_Native))) {
    				this._St_Native = reader.GetString(_.St_Native);
    			}
    			if ((false == reader.IsDBNull(_.St_Nation))) {
    				this._St_Nation = reader.GetString(_.St_Nation);
    			}
    			if ((false == reader.IsDBNull(_.St_CodeNumber))) {
    				this._St_CodeNumber = reader.GetString(_.St_CodeNumber);
    			}
    			if ((false == reader.IsDBNull(_.St_Address))) {
    				this._St_Address = reader.GetString(_.St_Address);
    			}
    			if ((false == reader.IsDBNull(_.St_AddrContact))) {
    				this._St_AddrContact = reader.GetString(_.St_AddrContact);
    			}
    			if ((false == reader.IsDBNull(_.St_Phone))) {
    				this._St_Phone = reader.GetString(_.St_Phone);
    			}
    			if ((false == reader.IsDBNull(_.St_IsOpenPhone))) {
    				this._St_IsOpenPhone = reader.GetBoolean(_.St_IsOpenPhone);
    			}
    			if ((false == reader.IsDBNull(_.St_PhoneMobi))) {
    				this._St_PhoneMobi = reader.GetString(_.St_PhoneMobi);
    			}
    			if ((false == reader.IsDBNull(_.St_IsOpenMobi))) {
    				this._St_IsOpenMobi = reader.GetBoolean(_.St_IsOpenMobi);
    			}
    			if ((false == reader.IsDBNull(_.St_Email))) {
    				this._St_Email = reader.GetString(_.St_Email);
    			}
    			if ((false == reader.IsDBNull(_.St_Qq))) {
    				this._St_Qq = reader.GetString(_.St_Qq);
    			}
    			if ((false == reader.IsDBNull(_.St_Weixin))) {
    				this._St_Weixin = reader.GetString(_.St_Weixin);
    			}
    			if ((false == reader.IsDBNull(_.St_Zip))) {
    				this._St_Zip = reader.GetString(_.St_Zip);
    			}
    			if ((false == reader.IsDBNull(_.St_LinkMan))) {
    				this._St_LinkMan = reader.GetString(_.St_LinkMan);
    			}
    			if ((false == reader.IsDBNull(_.St_LinkManPhone))) {
    				this._St_LinkManPhone = reader.GetString(_.St_LinkManPhone);
    			}
    			if ((false == reader.IsDBNull(_.St_IsPass))) {
    				this._St_IsPass = reader.GetBoolean(_.St_IsPass);
    			}
    			if ((false == reader.IsDBNull(_.St_IsUse))) {
    				this._St_IsUse = reader.GetBoolean(_.St_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.St_RegTime))) {
    				this._St_RegTime = reader.GetDateTime(_.St_RegTime);
    			}
    			if ((false == reader.IsDBNull(_.St_LastTime))) {
    				this._St_LastTime = reader.GetDateTime(_.St_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.St_CrtTime))) {
    				this._St_CrtTime = reader.GetDateTime(_.St_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Sts_ID))) {
    				this._Sts_ID = reader.GetInt32(_.Sts_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sts_Name))) {
    				this._Sts_Name = reader.GetString(_.Sts_Name);
    			}
    			if ((false == reader.IsDBNull(_.St_Money))) {
    				this._St_Money = reader.GetFloat(_.St_Money);
    			}
    			if ((false == reader.IsDBNull(_.St_LastIP))) {
    				this._St_LastIP = reader.GetString(_.St_LastIP);
    			}
    			if ((false == reader.IsDBNull(_.Dep_CnName))) {
    				this._Dep_CnName = reader.GetString(_.Dep_CnName);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Id))) {
    				this._Dep_Id = reader.GetInt32(_.Dep_Id);
    			}
    			if ((false == reader.IsDBNull(_.St_CheckUID))) {
    				this._St_CheckUID = reader.GetString(_.St_CheckUID);
    			}
    			if ((false == reader.IsDBNull(_.St_CurrCourse))) {
    				this._St_CurrCourse = reader.GetInt32(_.St_CurrCourse);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_UID))) {
    				this._Ac_UID = reader.GetString(_.Ac_UID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Student).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Student>();
    			
    			/// <summary>
    			/// 字段名：St_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field St_ID = new WeiSha.Data.Field<Student>("St_ID");
    			
    			/// <summary>
    			/// 字段名：St_AccName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_AccName = new WeiSha.Data.Field<Student>("St_AccName");
    			
    			/// <summary>
    			/// 字段名：St_Pw - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Pw = new WeiSha.Data.Field<Student>("St_Pw");
    			
    			/// <summary>
    			/// 字段名：St_Qus - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Qus = new WeiSha.Data.Field<Student>("St_Qus");
    			
    			/// <summary>
    			/// 字段名：St_Anwser - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Anwser = new WeiSha.Data.Field<Student>("St_Anwser");
    			
    			/// <summary>
    			/// 字段名：St_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Name = new WeiSha.Data.Field<Student>("St_Name");
    			
    			/// <summary>
    			/// 字段名：St_Pinyin - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Pinyin = new WeiSha.Data.Field<Student>("St_Pinyin");
    			
    			/// <summary>
    			/// 字段名：St_Age - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field St_Age = new WeiSha.Data.Field<Student>("St_Age");
    			
    			/// <summary>
    			/// 字段名：St_Birthday - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field St_Birthday = new WeiSha.Data.Field<Student>("St_Birthday");
    			
    			/// <summary>
    			/// 字段名：St_IDCardNumber - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_IDCardNumber = new WeiSha.Data.Field<Student>("St_IDCardNumber");
    			
    			/// <summary>
    			/// 字段名：St_Photo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Photo = new WeiSha.Data.Field<Student>("St_Photo");
    			
    			/// <summary>
    			/// 字段名：St_Signature - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Signature = new WeiSha.Data.Field<Student>("St_Signature");
    			
    			/// <summary>
    			/// 字段名：St_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Intro = new WeiSha.Data.Field<Student>("St_Intro");
    			
    			/// <summary>
    			/// 字段名：St_Major - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Major = new WeiSha.Data.Field<Student>("St_Major");
    			
    			/// <summary>
    			/// 字段名：St_Education - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Education = new WeiSha.Data.Field<Student>("St_Education");
    			
    			/// <summary>
    			/// 字段名：St_Sex - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field St_Sex = new WeiSha.Data.Field<Student>("St_Sex");
    			
    			/// <summary>
    			/// 字段名：St_Native - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Native = new WeiSha.Data.Field<Student>("St_Native");
    			
    			/// <summary>
    			/// 字段名：St_Nation - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Nation = new WeiSha.Data.Field<Student>("St_Nation");
    			
    			/// <summary>
    			/// 字段名：St_CodeNumber - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_CodeNumber = new WeiSha.Data.Field<Student>("St_CodeNumber");
    			
    			/// <summary>
    			/// 字段名：St_Address - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Address = new WeiSha.Data.Field<Student>("St_Address");
    			
    			/// <summary>
    			/// 字段名：St_AddrContact - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_AddrContact = new WeiSha.Data.Field<Student>("St_AddrContact");
    			
    			/// <summary>
    			/// 字段名：St_Phone - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Phone = new WeiSha.Data.Field<Student>("St_Phone");
    			
    			/// <summary>
    			/// 字段名：St_IsOpenPhone - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field St_IsOpenPhone = new WeiSha.Data.Field<Student>("St_IsOpenPhone");
    			
    			/// <summary>
    			/// 字段名：St_PhoneMobi - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_PhoneMobi = new WeiSha.Data.Field<Student>("St_PhoneMobi");
    			
    			/// <summary>
    			/// 字段名：St_IsOpenMobi - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field St_IsOpenMobi = new WeiSha.Data.Field<Student>("St_IsOpenMobi");
    			
    			/// <summary>
    			/// 字段名：St_Email - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Email = new WeiSha.Data.Field<Student>("St_Email");
    			
    			/// <summary>
    			/// 字段名：St_Qq - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Qq = new WeiSha.Data.Field<Student>("St_Qq");
    			
    			/// <summary>
    			/// 字段名：St_Weixin - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Weixin = new WeiSha.Data.Field<Student>("St_Weixin");
    			
    			/// <summary>
    			/// 字段名：St_Zip - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_Zip = new WeiSha.Data.Field<Student>("St_Zip");
    			
    			/// <summary>
    			/// 字段名：St_LinkMan - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_LinkMan = new WeiSha.Data.Field<Student>("St_LinkMan");
    			
    			/// <summary>
    			/// 字段名：St_LinkManPhone - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_LinkManPhone = new WeiSha.Data.Field<Student>("St_LinkManPhone");
    			
    			/// <summary>
    			/// 字段名：St_IsPass - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field St_IsPass = new WeiSha.Data.Field<Student>("St_IsPass");
    			
    			/// <summary>
    			/// 字段名：St_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field St_IsUse = new WeiSha.Data.Field<Student>("St_IsUse");
    			
    			/// <summary>
    			/// 字段名：St_RegTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field St_RegTime = new WeiSha.Data.Field<Student>("St_RegTime");
    			
    			/// <summary>
    			/// 字段名：St_LastTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field St_LastTime = new WeiSha.Data.Field<Student>("St_LastTime");
    			
    			/// <summary>
    			/// 字段名：St_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field St_CrtTime = new WeiSha.Data.Field<Student>("St_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Student>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Student>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Sts_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sts_ID = new WeiSha.Data.Field<Student>("Sts_ID");
    			
    			/// <summary>
    			/// 字段名：Sts_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sts_Name = new WeiSha.Data.Field<Student>("Sts_Name");
    			
    			/// <summary>
    			/// 字段名：St_Money - 数据类型：Single
    			/// </summary>
    			public static WeiSha.Data.Field St_Money = new WeiSha.Data.Field<Student>("St_Money");
    			
    			/// <summary>
    			/// 字段名：St_LastIP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_LastIP = new WeiSha.Data.Field<Student>("St_LastIP");
    			
    			/// <summary>
    			/// 字段名：Dep_CnName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_CnName = new WeiSha.Data.Field<Student>("Dep_CnName");
    			
    			/// <summary>
    			/// 字段名：Dep_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Id = new WeiSha.Data.Field<Student>("Dep_Id");
    			
    			/// <summary>
    			/// 字段名：St_CheckUID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_CheckUID = new WeiSha.Data.Field<Student>("St_CheckUID");
    			
    			/// <summary>
    			/// 字段名：St_CurrCourse - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field St_CurrCourse = new WeiSha.Data.Field<Student>("St_CurrCourse");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<Student>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_UID = new WeiSha.Data.Field<Student>("Ac_UID");
    		}
    	}
    }
    