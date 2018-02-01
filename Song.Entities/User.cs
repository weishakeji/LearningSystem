namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：User 主键列：User_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class User : WeiSha.Data.Entity {
    		
    		protected Int32 _User_Id;
    		
    		protected String _User_AccName;
    		
    		protected String _User_Name;
    		
    		protected Int32? _User_Sex;
    		
    		protected Boolean _User_IsUse;
    		
    		protected String _User_Pw;
    		
    		protected String _User_Email;
    		
    		protected String _User_Qus;
    		
    		protected String _User_Ans;
    		
    		protected DateTime? _User_RegTime;
    		
    		protected DateTime? _User_LastTime;
    		
    		protected Int32? _User_LoginNumber;
    		
    		protected String _User_Tel;
    		
    		protected String _User_MobileTel;
    		
    		protected String _User_QQ;
    		
    		protected String _User_Msn;
    		
    		protected Int32? _UGrp_Id;
    		
    		protected String _UGrp_Name;
    		
    		protected Int32? _Org_Id;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 User_Id {
    			get {
    				return this._User_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_Id, _User_Id, value);
    				this._User_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String User_AccName {
    			get {
    				return this._User_AccName;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_AccName, _User_AccName, value);
    				this._User_AccName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String User_Name {
    			get {
    				return this._User_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_Name, _User_Name, value);
    				this._User_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? User_Sex {
    			get {
    				return this._User_Sex;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_Sex, _User_Sex, value);
    				this._User_Sex = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean User_IsUse {
    			get {
    				return this._User_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_IsUse, _User_IsUse, value);
    				this._User_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String User_Pw {
    			get {
    				return this._User_Pw;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_Pw, _User_Pw, value);
    				this._User_Pw = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String User_Email {
    			get {
    				return this._User_Email;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_Email, _User_Email, value);
    				this._User_Email = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String User_Qus {
    			get {
    				return this._User_Qus;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_Qus, _User_Qus, value);
    				this._User_Qus = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String User_Ans {
    			get {
    				return this._User_Ans;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_Ans, _User_Ans, value);
    				this._User_Ans = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? User_RegTime {
    			get {
    				return this._User_RegTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_RegTime, _User_RegTime, value);
    				this._User_RegTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? User_LastTime {
    			get {
    				return this._User_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_LastTime, _User_LastTime, value);
    				this._User_LastTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? User_LoginNumber {
    			get {
    				return this._User_LoginNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_LoginNumber, _User_LoginNumber, value);
    				this._User_LoginNumber = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String User_Tel {
    			get {
    				return this._User_Tel;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_Tel, _User_Tel, value);
    				this._User_Tel = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String User_MobileTel {
    			get {
    				return this._User_MobileTel;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_MobileTel, _User_MobileTel, value);
    				this._User_MobileTel = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String User_QQ {
    			get {
    				return this._User_QQ;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_QQ, _User_QQ, value);
    				this._User_QQ = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String User_Msn {
    			get {
    				return this._User_Msn;
    			}
    			set {
    				this.OnPropertyValueChange(_.User_Msn, _User_Msn, value);
    				this._User_Msn = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? UGrp_Id {
    			get {
    				return this._UGrp_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.UGrp_Id, _UGrp_Id, value);
    				this._UGrp_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String UGrp_Name {
    			get {
    				return this._UGrp_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.UGrp_Name, _UGrp_Name, value);
    				this._UGrp_Name = value;
    			}
    		}
    		
    		public Int32? Org_Id {
    			get {
    				return this._Org_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Id, _Org_Id, value);
    				this._Org_Id = value;
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
    			return new WeiSha.Data.Table<User>("User");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.User_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.User_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.User_Id,
    					_.User_AccName,
    					_.User_Name,
    					_.User_Sex,
    					_.User_IsUse,
    					_.User_Pw,
    					_.User_Email,
    					_.User_Qus,
    					_.User_Ans,
    					_.User_RegTime,
    					_.User_LastTime,
    					_.User_LoginNumber,
    					_.User_Tel,
    					_.User_MobileTel,
    					_.User_QQ,
    					_.User_Msn,
    					_.UGrp_Id,
    					_.UGrp_Name,
    					_.Org_Id,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._User_Id,
    					this._User_AccName,
    					this._User_Name,
    					this._User_Sex,
    					this._User_IsUse,
    					this._User_Pw,
    					this._User_Email,
    					this._User_Qus,
    					this._User_Ans,
    					this._User_RegTime,
    					this._User_LastTime,
    					this._User_LoginNumber,
    					this._User_Tel,
    					this._User_MobileTel,
    					this._User_QQ,
    					this._User_Msn,
    					this._UGrp_Id,
    					this._UGrp_Name,
    					this._Org_Id,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.User_Id))) {
    				this._User_Id = reader.GetInt32(_.User_Id);
    			}
    			if ((false == reader.IsDBNull(_.User_AccName))) {
    				this._User_AccName = reader.GetString(_.User_AccName);
    			}
    			if ((false == reader.IsDBNull(_.User_Name))) {
    				this._User_Name = reader.GetString(_.User_Name);
    			}
    			if ((false == reader.IsDBNull(_.User_Sex))) {
    				this._User_Sex = reader.GetInt32(_.User_Sex);
    			}
    			if ((false == reader.IsDBNull(_.User_IsUse))) {
    				this._User_IsUse = reader.GetBoolean(_.User_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.User_Pw))) {
    				this._User_Pw = reader.GetString(_.User_Pw);
    			}
    			if ((false == reader.IsDBNull(_.User_Email))) {
    				this._User_Email = reader.GetString(_.User_Email);
    			}
    			if ((false == reader.IsDBNull(_.User_Qus))) {
    				this._User_Qus = reader.GetString(_.User_Qus);
    			}
    			if ((false == reader.IsDBNull(_.User_Ans))) {
    				this._User_Ans = reader.GetString(_.User_Ans);
    			}
    			if ((false == reader.IsDBNull(_.User_RegTime))) {
    				this._User_RegTime = reader.GetDateTime(_.User_RegTime);
    			}
    			if ((false == reader.IsDBNull(_.User_LastTime))) {
    				this._User_LastTime = reader.GetDateTime(_.User_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.User_LoginNumber))) {
    				this._User_LoginNumber = reader.GetInt32(_.User_LoginNumber);
    			}
    			if ((false == reader.IsDBNull(_.User_Tel))) {
    				this._User_Tel = reader.GetString(_.User_Tel);
    			}
    			if ((false == reader.IsDBNull(_.User_MobileTel))) {
    				this._User_MobileTel = reader.GetString(_.User_MobileTel);
    			}
    			if ((false == reader.IsDBNull(_.User_QQ))) {
    				this._User_QQ = reader.GetString(_.User_QQ);
    			}
    			if ((false == reader.IsDBNull(_.User_Msn))) {
    				this._User_Msn = reader.GetString(_.User_Msn);
    			}
    			if ((false == reader.IsDBNull(_.UGrp_Id))) {
    				this._UGrp_Id = reader.GetInt32(_.UGrp_Id);
    			}
    			if ((false == reader.IsDBNull(_.UGrp_Name))) {
    				this._UGrp_Name = reader.GetString(_.UGrp_Name);
    			}
    			if ((false == reader.IsDBNull(_.Org_Id))) {
    				this._Org_Id = reader.GetInt32(_.Org_Id);
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
    			if ((false == typeof(User).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<User>();
    			
    			/// <summary>
    			/// -1 - 字段名：User_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field User_Id = new WeiSha.Data.Field<User>("User_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：User_AccName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field User_AccName = new WeiSha.Data.Field<User>("User_AccName");
    			
    			/// <summary>
    			/// -1 - 字段名：User_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field User_Name = new WeiSha.Data.Field<User>("User_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：User_Sex - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field User_Sex = new WeiSha.Data.Field<User>("User_Sex");
    			
    			/// <summary>
    			/// -1 - 字段名：User_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field User_IsUse = new WeiSha.Data.Field<User>("User_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：User_Pw - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field User_Pw = new WeiSha.Data.Field<User>("User_Pw");
    			
    			/// <summary>
    			/// -1 - 字段名：User_Email - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field User_Email = new WeiSha.Data.Field<User>("User_Email");
    			
    			/// <summary>
    			/// -1 - 字段名：User_Qus - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field User_Qus = new WeiSha.Data.Field<User>("User_Qus");
    			
    			/// <summary>
    			/// -1 - 字段名：User_Ans - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field User_Ans = new WeiSha.Data.Field<User>("User_Ans");
    			
    			/// <summary>
    			/// -1 - 字段名：User_RegTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field User_RegTime = new WeiSha.Data.Field<User>("User_RegTime");
    			
    			/// <summary>
    			/// -1 - 字段名：User_LastTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field User_LastTime = new WeiSha.Data.Field<User>("User_LastTime");
    			
    			/// <summary>
    			/// -1 - 字段名：User_LoginNumber - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field User_LoginNumber = new WeiSha.Data.Field<User>("User_LoginNumber");
    			
    			/// <summary>
    			/// -1 - 字段名：User_Tel - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field User_Tel = new WeiSha.Data.Field<User>("User_Tel");
    			
    			/// <summary>
    			/// -1 - 字段名：User_MobileTel - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field User_MobileTel = new WeiSha.Data.Field<User>("User_MobileTel");
    			
    			/// <summary>
    			/// -1 - 字段名：User_QQ - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field User_QQ = new WeiSha.Data.Field<User>("User_QQ");
    			
    			/// <summary>
    			/// -1 - 字段名：User_Msn - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field User_Msn = new WeiSha.Data.Field<User>("User_Msn");
    			
    			/// <summary>
    			/// -1 - 字段名：UGrp_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field UGrp_Id = new WeiSha.Data.Field<User>("UGrp_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：UGrp_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field UGrp_Name = new WeiSha.Data.Field<User>("UGrp_Name");
    			
    			/// <summary>
    			/// 字段名：Org_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Org_Id = new WeiSha.Data.Field<User>("Org_Id");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<User>("Org_Name");
    		}
    	}
    }
    