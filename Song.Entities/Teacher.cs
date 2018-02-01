namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Teacher 主键列：Th_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Teacher : WeiSha.Data.Entity {
    		
    		protected Int32 _Th_ID;
    		
    		protected String _Th_AccName;
    		
    		protected String _Th_Pw;
    		
    		protected String _Th_Qus;
    		
    		protected String _Th_Anwser;
    		
    		protected String _Th_Name;
    		
    		protected String _Th_Title;
    		
    		protected String _Th_Pinyin;
    		
    		protected Int32 _Th_Age;
    		
    		protected DateTime _Th_Birthday;
    		
    		protected String _Th_IDCardNumber;
    		
    		protected String _Th_Photo;
    		
    		protected String _Th_Signature;
    		
    		protected String _Th_Intro;
    		
    		protected String _Th_Job;
    		
    		protected String _Th_Major;
    		
    		protected String _Th_Education;
    		
    		protected Int32 _Th_Sex;
    		
    		protected String _Th_Native;
    		
    		protected String _Th_Nation;
    		
    		protected String _Th_CodeNumber;
    		
    		protected String _Th_Address;
    		
    		protected String _Th_AddrContact;
    		
    		protected String _Th_Phone;
    		
    		protected Boolean _Th_IsOpenPhone;
    		
    		protected String _Th_PhoneMobi;
    		
    		protected Boolean _Th_IsOpenMobi;
    		
    		protected String _Th_Email;
    		
    		protected String _Th_Qq;
    		
    		protected String _Th_Weixin;
    		
    		protected String _Th_Zip;
    		
    		protected String _Th_LinkMan;
    		
    		protected String _Th_LinkManPhone;
    		
    		protected Boolean _Th_IsPass;
    		
    		protected Boolean _Th_IsUse;
    		
    		protected DateTime _Th_RegTime;
    		
    		protected DateTime _Th_LastTime;
    		
    		protected DateTime _Th_CrtTime;
    		
    		protected Int32 _Th_Tax;
    		
    		protected Int32 _Th_Score;
    		
    		protected Int32 _Th_ViewNum;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Int32 _Ths_ID;
    		
    		protected String _Ths_Name;
    		
    		protected Int32 _Dep_Id;
    		
    		protected Boolean _Th_IsShow;
    		
    		protected Int32 _Ac_ID;
    		
    		protected String _Ac_UID;
    		
    		public Int32 Th_ID {
    			get {
    				return this._Th_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_ID, _Th_ID, value);
    				this._Th_ID = value;
    			}
    		}
    		
    		public String Th_AccName {
    			get {
    				return this._Th_AccName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_AccName, _Th_AccName, value);
    				this._Th_AccName = value;
    			}
    		}
    		
    		public String Th_Pw {
    			get {
    				return this._Th_Pw;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Pw, _Th_Pw, value);
    				this._Th_Pw = value;
    			}
    		}
    		
    		public String Th_Qus {
    			get {
    				return this._Th_Qus;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Qus, _Th_Qus, value);
    				this._Th_Qus = value;
    			}
    		}
    		
    		public String Th_Anwser {
    			get {
    				return this._Th_Anwser;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Anwser, _Th_Anwser, value);
    				this._Th_Anwser = value;
    			}
    		}
    		
    		public String Th_Name {
    			get {
    				return this._Th_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Name, _Th_Name, value);
    				this._Th_Name = value;
    			}
    		}
    		
    		public String Th_Title {
    			get {
    				return this._Th_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Title, _Th_Title, value);
    				this._Th_Title = value;
    			}
    		}
    		
    		public String Th_Pinyin {
    			get {
    				return this._Th_Pinyin;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Pinyin, _Th_Pinyin, value);
    				this._Th_Pinyin = value;
    			}
    		}
    		
    		public Int32 Th_Age {
    			get {
    				return this._Th_Age;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Age, _Th_Age, value);
    				this._Th_Age = value;
    			}
    		}
    		
    		public DateTime Th_Birthday {
    			get {
    				return this._Th_Birthday;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Birthday, _Th_Birthday, value);
    				this._Th_Birthday = value;
    			}
    		}
    		
    		public String Th_IDCardNumber {
    			get {
    				return this._Th_IDCardNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_IDCardNumber, _Th_IDCardNumber, value);
    				this._Th_IDCardNumber = value;
    			}
    		}
    		
    		public String Th_Photo {
    			get {
    				return this._Th_Photo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Photo, _Th_Photo, value);
    				this._Th_Photo = value;
    			}
    		}
    		
    		public String Th_Signature {
    			get {
    				return this._Th_Signature;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Signature, _Th_Signature, value);
    				this._Th_Signature = value;
    			}
    		}
    		
    		public String Th_Intro {
    			get {
    				return this._Th_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Intro, _Th_Intro, value);
    				this._Th_Intro = value;
    			}
    		}
    		
    		public String Th_Job {
    			get {
    				return this._Th_Job;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Job, _Th_Job, value);
    				this._Th_Job = value;
    			}
    		}
    		
    		public String Th_Major {
    			get {
    				return this._Th_Major;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Major, _Th_Major, value);
    				this._Th_Major = value;
    			}
    		}
    		
    		public String Th_Education {
    			get {
    				return this._Th_Education;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Education, _Th_Education, value);
    				this._Th_Education = value;
    			}
    		}
    		
    		public Int32 Th_Sex {
    			get {
    				return this._Th_Sex;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Sex, _Th_Sex, value);
    				this._Th_Sex = value;
    			}
    		}
    		
    		public String Th_Native {
    			get {
    				return this._Th_Native;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Native, _Th_Native, value);
    				this._Th_Native = value;
    			}
    		}
    		
    		public String Th_Nation {
    			get {
    				return this._Th_Nation;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Nation, _Th_Nation, value);
    				this._Th_Nation = value;
    			}
    		}
    		
    		public String Th_CodeNumber {
    			get {
    				return this._Th_CodeNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_CodeNumber, _Th_CodeNumber, value);
    				this._Th_CodeNumber = value;
    			}
    		}
    		
    		public String Th_Address {
    			get {
    				return this._Th_Address;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Address, _Th_Address, value);
    				this._Th_Address = value;
    			}
    		}
    		
    		public String Th_AddrContact {
    			get {
    				return this._Th_AddrContact;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_AddrContact, _Th_AddrContact, value);
    				this._Th_AddrContact = value;
    			}
    		}
    		
    		public String Th_Phone {
    			get {
    				return this._Th_Phone;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Phone, _Th_Phone, value);
    				this._Th_Phone = value;
    			}
    		}
    		
    		public Boolean Th_IsOpenPhone {
    			get {
    				return this._Th_IsOpenPhone;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_IsOpenPhone, _Th_IsOpenPhone, value);
    				this._Th_IsOpenPhone = value;
    			}
    		}
    		
    		public String Th_PhoneMobi {
    			get {
    				return this._Th_PhoneMobi;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_PhoneMobi, _Th_PhoneMobi, value);
    				this._Th_PhoneMobi = value;
    			}
    		}
    		
    		public Boolean Th_IsOpenMobi {
    			get {
    				return this._Th_IsOpenMobi;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_IsOpenMobi, _Th_IsOpenMobi, value);
    				this._Th_IsOpenMobi = value;
    			}
    		}
    		
    		public String Th_Email {
    			get {
    				return this._Th_Email;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Email, _Th_Email, value);
    				this._Th_Email = value;
    			}
    		}
    		
    		public String Th_Qq {
    			get {
    				return this._Th_Qq;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Qq, _Th_Qq, value);
    				this._Th_Qq = value;
    			}
    		}
    		
    		public String Th_Weixin {
    			get {
    				return this._Th_Weixin;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Weixin, _Th_Weixin, value);
    				this._Th_Weixin = value;
    			}
    		}
    		
    		public String Th_Zip {
    			get {
    				return this._Th_Zip;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Zip, _Th_Zip, value);
    				this._Th_Zip = value;
    			}
    		}
    		
    		public String Th_LinkMan {
    			get {
    				return this._Th_LinkMan;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_LinkMan, _Th_LinkMan, value);
    				this._Th_LinkMan = value;
    			}
    		}
    		
    		public String Th_LinkManPhone {
    			get {
    				return this._Th_LinkManPhone;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_LinkManPhone, _Th_LinkManPhone, value);
    				this._Th_LinkManPhone = value;
    			}
    		}
    		
    		public Boolean Th_IsPass {
    			get {
    				return this._Th_IsPass;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_IsPass, _Th_IsPass, value);
    				this._Th_IsPass = value;
    			}
    		}
    		
    		public Boolean Th_IsUse {
    			get {
    				return this._Th_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_IsUse, _Th_IsUse, value);
    				this._Th_IsUse = value;
    			}
    		}
    		
    		public DateTime Th_RegTime {
    			get {
    				return this._Th_RegTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_RegTime, _Th_RegTime, value);
    				this._Th_RegTime = value;
    			}
    		}
    		
    		public DateTime Th_LastTime {
    			get {
    				return this._Th_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_LastTime, _Th_LastTime, value);
    				this._Th_LastTime = value;
    			}
    		}
    		
    		public DateTime Th_CrtTime {
    			get {
    				return this._Th_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_CrtTime, _Th_CrtTime, value);
    				this._Th_CrtTime = value;
    			}
    		}
    		
    		public Int32 Th_Tax {
    			get {
    				return this._Th_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Tax, _Th_Tax, value);
    				this._Th_Tax = value;
    			}
    		}
    		
    		public Int32 Th_Score {
    			get {
    				return this._Th_Score;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Score, _Th_Score, value);
    				this._Th_Score = value;
    			}
    		}
    		
    		public Int32 Th_ViewNum {
    			get {
    				return this._Th_ViewNum;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_ViewNum, _Th_ViewNum, value);
    				this._Th_ViewNum = value;
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
    		
    		public Int32 Ths_ID {
    			get {
    				return this._Ths_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ths_ID, _Ths_ID, value);
    				this._Ths_ID = value;
    			}
    		}
    		
    		public String Ths_Name {
    			get {
    				return this._Ths_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ths_Name, _Ths_Name, value);
    				this._Ths_Name = value;
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
    		
    		public Boolean Th_IsShow {
    			get {
    				return this._Th_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_IsShow, _Th_IsShow, value);
    				this._Th_IsShow = value;
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
    			return new WeiSha.Data.Table<Teacher>("Teacher");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Th_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Th_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Th_ID,
    					_.Th_AccName,
    					_.Th_Pw,
    					_.Th_Qus,
    					_.Th_Anwser,
    					_.Th_Name,
    					_.Th_Title,
    					_.Th_Pinyin,
    					_.Th_Age,
    					_.Th_Birthday,
    					_.Th_IDCardNumber,
    					_.Th_Photo,
    					_.Th_Signature,
    					_.Th_Intro,
    					_.Th_Job,
    					_.Th_Major,
    					_.Th_Education,
    					_.Th_Sex,
    					_.Th_Native,
    					_.Th_Nation,
    					_.Th_CodeNumber,
    					_.Th_Address,
    					_.Th_AddrContact,
    					_.Th_Phone,
    					_.Th_IsOpenPhone,
    					_.Th_PhoneMobi,
    					_.Th_IsOpenMobi,
    					_.Th_Email,
    					_.Th_Qq,
    					_.Th_Weixin,
    					_.Th_Zip,
    					_.Th_LinkMan,
    					_.Th_LinkManPhone,
    					_.Th_IsPass,
    					_.Th_IsUse,
    					_.Th_RegTime,
    					_.Th_LastTime,
    					_.Th_CrtTime,
    					_.Th_Tax,
    					_.Th_Score,
    					_.Th_ViewNum,
    					_.Org_ID,
    					_.Org_Name,
    					_.Ths_ID,
    					_.Ths_Name,
    					_.Dep_Id,
    					_.Th_IsShow,
    					_.Ac_ID,
    					_.Ac_UID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Th_ID,
    					this._Th_AccName,
    					this._Th_Pw,
    					this._Th_Qus,
    					this._Th_Anwser,
    					this._Th_Name,
    					this._Th_Title,
    					this._Th_Pinyin,
    					this._Th_Age,
    					this._Th_Birthday,
    					this._Th_IDCardNumber,
    					this._Th_Photo,
    					this._Th_Signature,
    					this._Th_Intro,
    					this._Th_Job,
    					this._Th_Major,
    					this._Th_Education,
    					this._Th_Sex,
    					this._Th_Native,
    					this._Th_Nation,
    					this._Th_CodeNumber,
    					this._Th_Address,
    					this._Th_AddrContact,
    					this._Th_Phone,
    					this._Th_IsOpenPhone,
    					this._Th_PhoneMobi,
    					this._Th_IsOpenMobi,
    					this._Th_Email,
    					this._Th_Qq,
    					this._Th_Weixin,
    					this._Th_Zip,
    					this._Th_LinkMan,
    					this._Th_LinkManPhone,
    					this._Th_IsPass,
    					this._Th_IsUse,
    					this._Th_RegTime,
    					this._Th_LastTime,
    					this._Th_CrtTime,
    					this._Th_Tax,
    					this._Th_Score,
    					this._Th_ViewNum,
    					this._Org_ID,
    					this._Org_Name,
    					this._Ths_ID,
    					this._Ths_Name,
    					this._Dep_Id,
    					this._Th_IsShow,
    					this._Ac_ID,
    					this._Ac_UID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Th_ID))) {
    				this._Th_ID = reader.GetInt32(_.Th_ID);
    			}
    			if ((false == reader.IsDBNull(_.Th_AccName))) {
    				this._Th_AccName = reader.GetString(_.Th_AccName);
    			}
    			if ((false == reader.IsDBNull(_.Th_Pw))) {
    				this._Th_Pw = reader.GetString(_.Th_Pw);
    			}
    			if ((false == reader.IsDBNull(_.Th_Qus))) {
    				this._Th_Qus = reader.GetString(_.Th_Qus);
    			}
    			if ((false == reader.IsDBNull(_.Th_Anwser))) {
    				this._Th_Anwser = reader.GetString(_.Th_Anwser);
    			}
    			if ((false == reader.IsDBNull(_.Th_Name))) {
    				this._Th_Name = reader.GetString(_.Th_Name);
    			}
    			if ((false == reader.IsDBNull(_.Th_Title))) {
    				this._Th_Title = reader.GetString(_.Th_Title);
    			}
    			if ((false == reader.IsDBNull(_.Th_Pinyin))) {
    				this._Th_Pinyin = reader.GetString(_.Th_Pinyin);
    			}
    			if ((false == reader.IsDBNull(_.Th_Age))) {
    				this._Th_Age = reader.GetInt32(_.Th_Age);
    			}
    			if ((false == reader.IsDBNull(_.Th_Birthday))) {
    				this._Th_Birthday = reader.GetDateTime(_.Th_Birthday);
    			}
    			if ((false == reader.IsDBNull(_.Th_IDCardNumber))) {
    				this._Th_IDCardNumber = reader.GetString(_.Th_IDCardNumber);
    			}
    			if ((false == reader.IsDBNull(_.Th_Photo))) {
    				this._Th_Photo = reader.GetString(_.Th_Photo);
    			}
    			if ((false == reader.IsDBNull(_.Th_Signature))) {
    				this._Th_Signature = reader.GetString(_.Th_Signature);
    			}
    			if ((false == reader.IsDBNull(_.Th_Intro))) {
    				this._Th_Intro = reader.GetString(_.Th_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Th_Job))) {
    				this._Th_Job = reader.GetString(_.Th_Job);
    			}
    			if ((false == reader.IsDBNull(_.Th_Major))) {
    				this._Th_Major = reader.GetString(_.Th_Major);
    			}
    			if ((false == reader.IsDBNull(_.Th_Education))) {
    				this._Th_Education = reader.GetString(_.Th_Education);
    			}
    			if ((false == reader.IsDBNull(_.Th_Sex))) {
    				this._Th_Sex = reader.GetInt32(_.Th_Sex);
    			}
    			if ((false == reader.IsDBNull(_.Th_Native))) {
    				this._Th_Native = reader.GetString(_.Th_Native);
    			}
    			if ((false == reader.IsDBNull(_.Th_Nation))) {
    				this._Th_Nation = reader.GetString(_.Th_Nation);
    			}
    			if ((false == reader.IsDBNull(_.Th_CodeNumber))) {
    				this._Th_CodeNumber = reader.GetString(_.Th_CodeNumber);
    			}
    			if ((false == reader.IsDBNull(_.Th_Address))) {
    				this._Th_Address = reader.GetString(_.Th_Address);
    			}
    			if ((false == reader.IsDBNull(_.Th_AddrContact))) {
    				this._Th_AddrContact = reader.GetString(_.Th_AddrContact);
    			}
    			if ((false == reader.IsDBNull(_.Th_Phone))) {
    				this._Th_Phone = reader.GetString(_.Th_Phone);
    			}
    			if ((false == reader.IsDBNull(_.Th_IsOpenPhone))) {
    				this._Th_IsOpenPhone = reader.GetBoolean(_.Th_IsOpenPhone);
    			}
    			if ((false == reader.IsDBNull(_.Th_PhoneMobi))) {
    				this._Th_PhoneMobi = reader.GetString(_.Th_PhoneMobi);
    			}
    			if ((false == reader.IsDBNull(_.Th_IsOpenMobi))) {
    				this._Th_IsOpenMobi = reader.GetBoolean(_.Th_IsOpenMobi);
    			}
    			if ((false == reader.IsDBNull(_.Th_Email))) {
    				this._Th_Email = reader.GetString(_.Th_Email);
    			}
    			if ((false == reader.IsDBNull(_.Th_Qq))) {
    				this._Th_Qq = reader.GetString(_.Th_Qq);
    			}
    			if ((false == reader.IsDBNull(_.Th_Weixin))) {
    				this._Th_Weixin = reader.GetString(_.Th_Weixin);
    			}
    			if ((false == reader.IsDBNull(_.Th_Zip))) {
    				this._Th_Zip = reader.GetString(_.Th_Zip);
    			}
    			if ((false == reader.IsDBNull(_.Th_LinkMan))) {
    				this._Th_LinkMan = reader.GetString(_.Th_LinkMan);
    			}
    			if ((false == reader.IsDBNull(_.Th_LinkManPhone))) {
    				this._Th_LinkManPhone = reader.GetString(_.Th_LinkManPhone);
    			}
    			if ((false == reader.IsDBNull(_.Th_IsPass))) {
    				this._Th_IsPass = reader.GetBoolean(_.Th_IsPass);
    			}
    			if ((false == reader.IsDBNull(_.Th_IsUse))) {
    				this._Th_IsUse = reader.GetBoolean(_.Th_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Th_RegTime))) {
    				this._Th_RegTime = reader.GetDateTime(_.Th_RegTime);
    			}
    			if ((false == reader.IsDBNull(_.Th_LastTime))) {
    				this._Th_LastTime = reader.GetDateTime(_.Th_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Th_CrtTime))) {
    				this._Th_CrtTime = reader.GetDateTime(_.Th_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Th_Tax))) {
    				this._Th_Tax = reader.GetInt32(_.Th_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Th_Score))) {
    				this._Th_Score = reader.GetInt32(_.Th_Score);
    			}
    			if ((false == reader.IsDBNull(_.Th_ViewNum))) {
    				this._Th_ViewNum = reader.GetInt32(_.Th_ViewNum);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ths_ID))) {
    				this._Ths_ID = reader.GetInt32(_.Ths_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ths_Name))) {
    				this._Ths_Name = reader.GetString(_.Ths_Name);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Id))) {
    				this._Dep_Id = reader.GetInt32(_.Dep_Id);
    			}
    			if ((false == reader.IsDBNull(_.Th_IsShow))) {
    				this._Th_IsShow = reader.GetBoolean(_.Th_IsShow);
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
    			if ((false == typeof(Teacher).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Teacher>();
    			
    			/// <summary>
    			/// 字段名：Th_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_ID = new WeiSha.Data.Field<Teacher>("Th_ID");
    			
    			/// <summary>
    			/// 字段名：Th_AccName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_AccName = new WeiSha.Data.Field<Teacher>("Th_AccName");
    			
    			/// <summary>
    			/// 字段名：Th_Pw - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Pw = new WeiSha.Data.Field<Teacher>("Th_Pw");
    			
    			/// <summary>
    			/// 字段名：Th_Qus - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Qus = new WeiSha.Data.Field<Teacher>("Th_Qus");
    			
    			/// <summary>
    			/// 字段名：Th_Anwser - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Anwser = new WeiSha.Data.Field<Teacher>("Th_Anwser");
    			
    			/// <summary>
    			/// 字段名：Th_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Name = new WeiSha.Data.Field<Teacher>("Th_Name");
    			
    			/// <summary>
    			/// 字段名：Th_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Title = new WeiSha.Data.Field<Teacher>("Th_Title");
    			
    			/// <summary>
    			/// 字段名：Th_Pinyin - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Pinyin = new WeiSha.Data.Field<Teacher>("Th_Pinyin");
    			
    			/// <summary>
    			/// 字段名：Th_Age - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_Age = new WeiSha.Data.Field<Teacher>("Th_Age");
    			
    			/// <summary>
    			/// 字段名：Th_Birthday - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Th_Birthday = new WeiSha.Data.Field<Teacher>("Th_Birthday");
    			
    			/// <summary>
    			/// 字段名：Th_IDCardNumber - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_IDCardNumber = new WeiSha.Data.Field<Teacher>("Th_IDCardNumber");
    			
    			/// <summary>
    			/// 字段名：Th_Photo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Photo = new WeiSha.Data.Field<Teacher>("Th_Photo");
    			
    			/// <summary>
    			/// 字段名：Th_Signature - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Signature = new WeiSha.Data.Field<Teacher>("Th_Signature");
    			
    			/// <summary>
    			/// 字段名：Th_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Intro = new WeiSha.Data.Field<Teacher>("Th_Intro");
    			
    			/// <summary>
    			/// 字段名：Th_Job - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Job = new WeiSha.Data.Field<Teacher>("Th_Job");
    			
    			/// <summary>
    			/// 字段名：Th_Major - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Major = new WeiSha.Data.Field<Teacher>("Th_Major");
    			
    			/// <summary>
    			/// 字段名：Th_Education - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Education = new WeiSha.Data.Field<Teacher>("Th_Education");
    			
    			/// <summary>
    			/// 字段名：Th_Sex - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_Sex = new WeiSha.Data.Field<Teacher>("Th_Sex");
    			
    			/// <summary>
    			/// 字段名：Th_Native - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Native = new WeiSha.Data.Field<Teacher>("Th_Native");
    			
    			/// <summary>
    			/// 字段名：Th_Nation - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Nation = new WeiSha.Data.Field<Teacher>("Th_Nation");
    			
    			/// <summary>
    			/// 字段名：Th_CodeNumber - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_CodeNumber = new WeiSha.Data.Field<Teacher>("Th_CodeNumber");
    			
    			/// <summary>
    			/// 字段名：Th_Address - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Address = new WeiSha.Data.Field<Teacher>("Th_Address");
    			
    			/// <summary>
    			/// 字段名：Th_AddrContact - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_AddrContact = new WeiSha.Data.Field<Teacher>("Th_AddrContact");
    			
    			/// <summary>
    			/// 字段名：Th_Phone - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Phone = new WeiSha.Data.Field<Teacher>("Th_Phone");
    			
    			/// <summary>
    			/// 字段名：Th_IsOpenPhone - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Th_IsOpenPhone = new WeiSha.Data.Field<Teacher>("Th_IsOpenPhone");
    			
    			/// <summary>
    			/// 字段名：Th_PhoneMobi - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_PhoneMobi = new WeiSha.Data.Field<Teacher>("Th_PhoneMobi");
    			
    			/// <summary>
    			/// 字段名：Th_IsOpenMobi - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Th_IsOpenMobi = new WeiSha.Data.Field<Teacher>("Th_IsOpenMobi");
    			
    			/// <summary>
    			/// 字段名：Th_Email - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Email = new WeiSha.Data.Field<Teacher>("Th_Email");
    			
    			/// <summary>
    			/// 字段名：Th_Qq - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Qq = new WeiSha.Data.Field<Teacher>("Th_Qq");
    			
    			/// <summary>
    			/// 字段名：Th_Weixin - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Weixin = new WeiSha.Data.Field<Teacher>("Th_Weixin");
    			
    			/// <summary>
    			/// 字段名：Th_Zip - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Zip = new WeiSha.Data.Field<Teacher>("Th_Zip");
    			
    			/// <summary>
    			/// 字段名：Th_LinkMan - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_LinkMan = new WeiSha.Data.Field<Teacher>("Th_LinkMan");
    			
    			/// <summary>
    			/// 字段名：Th_LinkManPhone - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_LinkManPhone = new WeiSha.Data.Field<Teacher>("Th_LinkManPhone");
    			
    			/// <summary>
    			/// 字段名：Th_IsPass - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Th_IsPass = new WeiSha.Data.Field<Teacher>("Th_IsPass");
    			
    			/// <summary>
    			/// 字段名：Th_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Th_IsUse = new WeiSha.Data.Field<Teacher>("Th_IsUse");
    			
    			/// <summary>
    			/// 字段名：Th_RegTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Th_RegTime = new WeiSha.Data.Field<Teacher>("Th_RegTime");
    			
    			/// <summary>
    			/// 字段名：Th_LastTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Th_LastTime = new WeiSha.Data.Field<Teacher>("Th_LastTime");
    			
    			/// <summary>
    			/// 字段名：Th_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Th_CrtTime = new WeiSha.Data.Field<Teacher>("Th_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Th_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_Tax = new WeiSha.Data.Field<Teacher>("Th_Tax");
    			
    			/// <summary>
    			/// 字段名：Th_Score - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_Score = new WeiSha.Data.Field<Teacher>("Th_Score");
    			
    			/// <summary>
    			/// 字段名：Th_ViewNum - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_ViewNum = new WeiSha.Data.Field<Teacher>("Th_ViewNum");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Teacher>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Teacher>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Ths_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ths_ID = new WeiSha.Data.Field<Teacher>("Ths_ID");
    			
    			/// <summary>
    			/// 字段名：Ths_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ths_Name = new WeiSha.Data.Field<Teacher>("Ths_Name");
    			
    			/// <summary>
    			/// 字段名：Dep_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Id = new WeiSha.Data.Field<Teacher>("Dep_Id");
    			
    			/// <summary>
    			/// 字段名：Th_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Th_IsShow = new WeiSha.Data.Field<Teacher>("Th_IsShow");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<Teacher>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_UID = new WeiSha.Data.Field<Teacher>("Ac_UID");
    		}
    	}
    }
    