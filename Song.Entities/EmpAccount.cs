namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：EmpAccount 主键列：Acc_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class EmpAccount : WeiSha.Data.Entity {
    		
    		protected Int32 _Acc_Id;
    		
    		protected String _Acc_AccName;
    		
    		protected String _Acc_Pw;
    		
    		protected String _Acc_Name;
    		
    		protected String _Acc_NamePinyin;
    		
    		protected String _Acc_IDCardNumber;
    		
    		protected String _Acc_Signature;
    		
    		protected Int32 _Acc_Sex;
    		
    		protected Int32 _Acc_Age;
    		
    		protected DateTime _Acc_Birthday;
    		
    		protected Boolean _Acc_IsUse;
    		
    		protected String _Acc_Email;
    		
    		protected String _Acc_Qus;
    		
    		protected String _Acc_Ans;
    		
    		protected String _Acc_EmpCode;
    		
    		protected DateTime _Acc_RegTime;
    		
    		protected DateTime _Acc_OutTime;
    		
    		protected Boolean _Acc_IsAutoOut;
    		
    		protected DateTime _Acc_LastTime;
    		
    		protected String _Acc_Tel;
    		
    		protected Boolean _Acc_IsOpenTel;
    		
    		protected String _Acc_MobileTel;
    		
    		protected Boolean _Acc_IsOpenMobile;
    		
    		protected String _Acc_QQ;
    		
    		protected Int32 _Dep_Id;
    		
    		protected String _Dep_CnName;
    		
    		protected Int32 _Posi_Id;
    		
    		protected String _Posi_Name;
    		
    		protected Int32 _EGrp_Id;
    		
    		protected String _Acc_Photo;
    		
    		protected Boolean _Acc_IsUseCard;
    		
    		protected String _Acc_Weixin;
    		
    		protected Int32 _Title_Id;
    		
    		protected String _Title_Name;
    		
    		protected Boolean _Acc_IsPartTime;
    		
    		protected Int32 _Team_ID;
    		
    		protected String _Team_Name;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Acc_Id {
    			get {
    				return this._Acc_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Id, _Acc_Id, value);
    				this._Acc_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_AccName {
    			get {
    				return this._Acc_AccName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_AccName, _Acc_AccName, value);
    				this._Acc_AccName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_Pw {
    			get {
    				return this._Acc_Pw;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Pw, _Acc_Pw, value);
    				this._Acc_Pw = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_Name {
    			get {
    				return this._Acc_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Name, _Acc_Name, value);
    				this._Acc_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_NamePinyin {
    			get {
    				return this._Acc_NamePinyin;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_NamePinyin, _Acc_NamePinyin, value);
    				this._Acc_NamePinyin = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_IDCardNumber {
    			get {
    				return this._Acc_IDCardNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_IDCardNumber, _Acc_IDCardNumber, value);
    				this._Acc_IDCardNumber = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_Signature {
    			get {
    				return this._Acc_Signature;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Signature, _Acc_Signature, value);
    				this._Acc_Signature = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Acc_Sex {
    			get {
    				return this._Acc_Sex;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Sex, _Acc_Sex, value);
    				this._Acc_Sex = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Acc_Age {
    			get {
    				return this._Acc_Age;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Age, _Acc_Age, value);
    				this._Acc_Age = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime Acc_Birthday {
    			get {
    				return this._Acc_Birthday;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Birthday, _Acc_Birthday, value);
    				this._Acc_Birthday = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Acc_IsUse {
    			get {
    				return this._Acc_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_IsUse, _Acc_IsUse, value);
    				this._Acc_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_Email {
    			get {
    				return this._Acc_Email;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Email, _Acc_Email, value);
    				this._Acc_Email = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_Qus {
    			get {
    				return this._Acc_Qus;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Qus, _Acc_Qus, value);
    				this._Acc_Qus = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_Ans {
    			get {
    				return this._Acc_Ans;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Ans, _Acc_Ans, value);
    				this._Acc_Ans = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_EmpCode {
    			get {
    				return this._Acc_EmpCode;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_EmpCode, _Acc_EmpCode, value);
    				this._Acc_EmpCode = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime Acc_RegTime {
    			get {
    				return this._Acc_RegTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_RegTime, _Acc_RegTime, value);
    				this._Acc_RegTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime Acc_OutTime {
    			get {
    				return this._Acc_OutTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_OutTime, _Acc_OutTime, value);
    				this._Acc_OutTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Acc_IsAutoOut {
    			get {
    				return this._Acc_IsAutoOut;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_IsAutoOut, _Acc_IsAutoOut, value);
    				this._Acc_IsAutoOut = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime Acc_LastTime {
    			get {
    				return this._Acc_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_LastTime, _Acc_LastTime, value);
    				this._Acc_LastTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_Tel {
    			get {
    				return this._Acc_Tel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Tel, _Acc_Tel, value);
    				this._Acc_Tel = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Acc_IsOpenTel {
    			get {
    				return this._Acc_IsOpenTel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_IsOpenTel, _Acc_IsOpenTel, value);
    				this._Acc_IsOpenTel = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_MobileTel {
    			get {
    				return this._Acc_MobileTel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_MobileTel, _Acc_MobileTel, value);
    				this._Acc_MobileTel = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Acc_IsOpenMobile {
    			get {
    				return this._Acc_IsOpenMobile;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_IsOpenMobile, _Acc_IsOpenMobile, value);
    				this._Acc_IsOpenMobile = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_QQ {
    			get {
    				return this._Acc_QQ;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_QQ, _Acc_QQ, value);
    				this._Acc_QQ = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Dep_Id {
    			get {
    				return this._Dep_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Id, _Dep_Id, value);
    				this._Dep_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dep_CnName {
    			get {
    				return this._Dep_CnName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_CnName, _Dep_CnName, value);
    				this._Dep_CnName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Posi_Id {
    			get {
    				return this._Posi_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Posi_Id, _Posi_Id, value);
    				this._Posi_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Posi_Name {
    			get {
    				return this._Posi_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Posi_Name, _Posi_Name, value);
    				this._Posi_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 EGrp_Id {
    			get {
    				return this._EGrp_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.EGrp_Id, _EGrp_Id, value);
    				this._EGrp_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_Photo {
    			get {
    				return this._Acc_Photo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Photo, _Acc_Photo, value);
    				this._Acc_Photo = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Acc_IsUseCard {
    			get {
    				return this._Acc_IsUseCard;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_IsUseCard, _Acc_IsUseCard, value);
    				this._Acc_IsUseCard = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_Weixin {
    			get {
    				return this._Acc_Weixin;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Weixin, _Acc_Weixin, value);
    				this._Acc_Weixin = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Title_Id {
    			get {
    				return this._Title_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Title_Id, _Title_Id, value);
    				this._Title_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Title_Name {
    			get {
    				return this._Title_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Title_Name, _Title_Name, value);
    				this._Title_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Acc_IsPartTime {
    			get {
    				return this._Acc_IsPartTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_IsPartTime, _Acc_IsPartTime, value);
    				this._Acc_IsPartTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Team_ID {
    			get {
    				return this._Team_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Team_ID, _Team_ID, value);
    				this._Team_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Team_Name {
    			get {
    				return this._Team_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Team_Name, _Team_Name, value);
    				this._Team_Name = value;
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
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<EmpAccount>("EmpAccount");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Acc_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Acc_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Acc_Id,
    					_.Acc_AccName,
    					_.Acc_Pw,
    					_.Acc_Name,
    					_.Acc_NamePinyin,
    					_.Acc_IDCardNumber,
    					_.Acc_Signature,
    					_.Acc_Sex,
    					_.Acc_Age,
    					_.Acc_Birthday,
    					_.Acc_IsUse,
    					_.Acc_Email,
    					_.Acc_Qus,
    					_.Acc_Ans,
    					_.Acc_EmpCode,
    					_.Acc_RegTime,
    					_.Acc_OutTime,
    					_.Acc_IsAutoOut,
    					_.Acc_LastTime,
    					_.Acc_Tel,
    					_.Acc_IsOpenTel,
    					_.Acc_MobileTel,
    					_.Acc_IsOpenMobile,
    					_.Acc_QQ,
    					_.Dep_Id,
    					_.Dep_CnName,
    					_.Posi_Id,
    					_.Posi_Name,
    					_.EGrp_Id,
    					_.Acc_Photo,
    					_.Acc_IsUseCard,
    					_.Acc_Weixin,
    					_.Title_Id,
    					_.Title_Name,
    					_.Acc_IsPartTime,
    					_.Team_ID,
    					_.Team_Name,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Acc_Id,
    					this._Acc_AccName,
    					this._Acc_Pw,
    					this._Acc_Name,
    					this._Acc_NamePinyin,
    					this._Acc_IDCardNumber,
    					this._Acc_Signature,
    					this._Acc_Sex,
    					this._Acc_Age,
    					this._Acc_Birthday,
    					this._Acc_IsUse,
    					this._Acc_Email,
    					this._Acc_Qus,
    					this._Acc_Ans,
    					this._Acc_EmpCode,
    					this._Acc_RegTime,
    					this._Acc_OutTime,
    					this._Acc_IsAutoOut,
    					this._Acc_LastTime,
    					this._Acc_Tel,
    					this._Acc_IsOpenTel,
    					this._Acc_MobileTel,
    					this._Acc_IsOpenMobile,
    					this._Acc_QQ,
    					this._Dep_Id,
    					this._Dep_CnName,
    					this._Posi_Id,
    					this._Posi_Name,
    					this._EGrp_Id,
    					this._Acc_Photo,
    					this._Acc_IsUseCard,
    					this._Acc_Weixin,
    					this._Title_Id,
    					this._Title_Name,
    					this._Acc_IsPartTime,
    					this._Team_ID,
    					this._Team_Name,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_AccName))) {
    				this._Acc_AccName = reader.GetString(_.Acc_AccName);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Pw))) {
    				this._Acc_Pw = reader.GetString(_.Acc_Pw);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Name))) {
    				this._Acc_Name = reader.GetString(_.Acc_Name);
    			}
    			if ((false == reader.IsDBNull(_.Acc_NamePinyin))) {
    				this._Acc_NamePinyin = reader.GetString(_.Acc_NamePinyin);
    			}
    			if ((false == reader.IsDBNull(_.Acc_IDCardNumber))) {
    				this._Acc_IDCardNumber = reader.GetString(_.Acc_IDCardNumber);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Signature))) {
    				this._Acc_Signature = reader.GetString(_.Acc_Signature);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Sex))) {
    				this._Acc_Sex = reader.GetInt32(_.Acc_Sex);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Age))) {
    				this._Acc_Age = reader.GetInt32(_.Acc_Age);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Birthday))) {
    				this._Acc_Birthday = reader.GetDateTime(_.Acc_Birthday);
    			}
    			if ((false == reader.IsDBNull(_.Acc_IsUse))) {
    				this._Acc_IsUse = reader.GetBoolean(_.Acc_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Email))) {
    				this._Acc_Email = reader.GetString(_.Acc_Email);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Qus))) {
    				this._Acc_Qus = reader.GetString(_.Acc_Qus);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Ans))) {
    				this._Acc_Ans = reader.GetString(_.Acc_Ans);
    			}
    			if ((false == reader.IsDBNull(_.Acc_EmpCode))) {
    				this._Acc_EmpCode = reader.GetString(_.Acc_EmpCode);
    			}
    			if ((false == reader.IsDBNull(_.Acc_RegTime))) {
    				this._Acc_RegTime = reader.GetDateTime(_.Acc_RegTime);
    			}
    			if ((false == reader.IsDBNull(_.Acc_OutTime))) {
    				this._Acc_OutTime = reader.GetDateTime(_.Acc_OutTime);
    			}
    			if ((false == reader.IsDBNull(_.Acc_IsAutoOut))) {
    				this._Acc_IsAutoOut = reader.GetBoolean(_.Acc_IsAutoOut);
    			}
    			if ((false == reader.IsDBNull(_.Acc_LastTime))) {
    				this._Acc_LastTime = reader.GetDateTime(_.Acc_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Tel))) {
    				this._Acc_Tel = reader.GetString(_.Acc_Tel);
    			}
    			if ((false == reader.IsDBNull(_.Acc_IsOpenTel))) {
    				this._Acc_IsOpenTel = reader.GetBoolean(_.Acc_IsOpenTel);
    			}
    			if ((false == reader.IsDBNull(_.Acc_MobileTel))) {
    				this._Acc_MobileTel = reader.GetString(_.Acc_MobileTel);
    			}
    			if ((false == reader.IsDBNull(_.Acc_IsOpenMobile))) {
    				this._Acc_IsOpenMobile = reader.GetBoolean(_.Acc_IsOpenMobile);
    			}
    			if ((false == reader.IsDBNull(_.Acc_QQ))) {
    				this._Acc_QQ = reader.GetString(_.Acc_QQ);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Id))) {
    				this._Dep_Id = reader.GetInt32(_.Dep_Id);
    			}
    			if ((false == reader.IsDBNull(_.Dep_CnName))) {
    				this._Dep_CnName = reader.GetString(_.Dep_CnName);
    			}
    			if ((false == reader.IsDBNull(_.Posi_Id))) {
    				this._Posi_Id = reader.GetInt32(_.Posi_Id);
    			}
    			if ((false == reader.IsDBNull(_.Posi_Name))) {
    				this._Posi_Name = reader.GetString(_.Posi_Name);
    			}
    			if ((false == reader.IsDBNull(_.EGrp_Id))) {
    				this._EGrp_Id = reader.GetInt32(_.EGrp_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Photo))) {
    				this._Acc_Photo = reader.GetString(_.Acc_Photo);
    			}
    			if ((false == reader.IsDBNull(_.Acc_IsUseCard))) {
    				this._Acc_IsUseCard = reader.GetBoolean(_.Acc_IsUseCard);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Weixin))) {
    				this._Acc_Weixin = reader.GetString(_.Acc_Weixin);
    			}
    			if ((false == reader.IsDBNull(_.Title_Id))) {
    				this._Title_Id = reader.GetInt32(_.Title_Id);
    			}
    			if ((false == reader.IsDBNull(_.Title_Name))) {
    				this._Title_Name = reader.GetString(_.Title_Name);
    			}
    			if ((false == reader.IsDBNull(_.Acc_IsPartTime))) {
    				this._Acc_IsPartTime = reader.GetBoolean(_.Acc_IsPartTime);
    			}
    			if ((false == reader.IsDBNull(_.Team_ID))) {
    				this._Team_ID = reader.GetInt32(_.Team_ID);
    			}
    			if ((false == reader.IsDBNull(_.Team_Name))) {
    				this._Team_Name = reader.GetString(_.Team_Name);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(EmpAccount).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<EmpAccount>();
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<EmpAccount>("Acc_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_AccName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_AccName = new WeiSha.Data.Field<EmpAccount>("Acc_AccName");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Pw - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Pw = new WeiSha.Data.Field<EmpAccount>("Acc_Pw");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Name = new WeiSha.Data.Field<EmpAccount>("Acc_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_NamePinyin - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_NamePinyin = new WeiSha.Data.Field<EmpAccount>("Acc_NamePinyin");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_IDCardNumber - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_IDCardNumber = new WeiSha.Data.Field<EmpAccount>("Acc_IDCardNumber");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Signature - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Signature = new WeiSha.Data.Field<EmpAccount>("Acc_Signature");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Sex - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Sex = new WeiSha.Data.Field<EmpAccount>("Acc_Sex");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Age - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Age = new WeiSha.Data.Field<EmpAccount>("Acc_Age");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Birthday - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Birthday = new WeiSha.Data.Field<EmpAccount>("Acc_Birthday");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Acc_IsUse = new WeiSha.Data.Field<EmpAccount>("Acc_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Email - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Email = new WeiSha.Data.Field<EmpAccount>("Acc_Email");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Qus - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Qus = new WeiSha.Data.Field<EmpAccount>("Acc_Qus");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Ans - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Ans = new WeiSha.Data.Field<EmpAccount>("Acc_Ans");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_EmpCode - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_EmpCode = new WeiSha.Data.Field<EmpAccount>("Acc_EmpCode");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_RegTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Acc_RegTime = new WeiSha.Data.Field<EmpAccount>("Acc_RegTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_OutTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Acc_OutTime = new WeiSha.Data.Field<EmpAccount>("Acc_OutTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_IsAutoOut - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Acc_IsAutoOut = new WeiSha.Data.Field<EmpAccount>("Acc_IsAutoOut");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_LastTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Acc_LastTime = new WeiSha.Data.Field<EmpAccount>("Acc_LastTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Tel - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Tel = new WeiSha.Data.Field<EmpAccount>("Acc_Tel");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_IsOpenTel - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Acc_IsOpenTel = new WeiSha.Data.Field<EmpAccount>("Acc_IsOpenTel");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_MobileTel - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_MobileTel = new WeiSha.Data.Field<EmpAccount>("Acc_MobileTel");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_IsOpenMobile - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Acc_IsOpenMobile = new WeiSha.Data.Field<EmpAccount>("Acc_IsOpenMobile");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_QQ - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_QQ = new WeiSha.Data.Field<EmpAccount>("Acc_QQ");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Id = new WeiSha.Data.Field<EmpAccount>("Dep_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_CnName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_CnName = new WeiSha.Data.Field<EmpAccount>("Dep_CnName");
    			
    			/// <summary>
    			/// -1 - 字段名：Posi_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Posi_Id = new WeiSha.Data.Field<EmpAccount>("Posi_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Posi_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Posi_Name = new WeiSha.Data.Field<EmpAccount>("Posi_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：EGrp_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field EGrp_Id = new WeiSha.Data.Field<EmpAccount>("EGrp_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Photo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Photo = new WeiSha.Data.Field<EmpAccount>("Acc_Photo");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_IsUseCard - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Acc_IsUseCard = new WeiSha.Data.Field<EmpAccount>("Acc_IsUseCard");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Weixin - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Weixin = new WeiSha.Data.Field<EmpAccount>("Acc_Weixin");
    			
    			/// <summary>
    			/// -1 - 字段名：Title_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Title_Id = new WeiSha.Data.Field<EmpAccount>("Title_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Title_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Title_Name = new WeiSha.Data.Field<EmpAccount>("Title_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_IsPartTime - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Acc_IsPartTime = new WeiSha.Data.Field<EmpAccount>("Acc_IsPartTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Team_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Team_ID = new WeiSha.Data.Field<EmpAccount>("Team_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Team_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Team_Name = new WeiSha.Data.Field<EmpAccount>("Team_Name");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<EmpAccount>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<EmpAccount>("Org_Name");
    		}
    	}
    }
    