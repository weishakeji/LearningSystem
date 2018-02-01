namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ProductMessage 主键列：Pm_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ProductMessage : WeiSha.Data.Entity {
    		
    		protected Int32 _Pm_Id;
    		
    		protected String _Pm_Title;
    		
    		protected String _Pm_Context;
    		
    		protected String _Pm_Answer;
    		
    		protected DateTime? _Pm_CrtTime;
    		
    		protected DateTime? _Pm_AnsTime;
    		
    		protected Boolean _Pm_IsAns;
    		
    		protected Boolean _Pm_IsShow;
    		
    		protected String _Pm_IP;
    		
    		protected String _Pm_Phone;
    		
    		protected String _Pm_Email;
    		
    		protected String _Pm_QQ;
    		
    		protected String _Pm_Address;
    		
    		protected Int32? _Pd_Id;
    		
    		protected String _Pd_Name;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Pm_Id {
    			get {
    				return this._Pm_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pm_Id, _Pm_Id, value);
    				this._Pm_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pm_Title {
    			get {
    				return this._Pm_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pm_Title, _Pm_Title, value);
    				this._Pm_Title = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pm_Context {
    			get {
    				return this._Pm_Context;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pm_Context, _Pm_Context, value);
    				this._Pm_Context = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pm_Answer {
    			get {
    				return this._Pm_Answer;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pm_Answer, _Pm_Answer, value);
    				this._Pm_Answer = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Pm_CrtTime {
    			get {
    				return this._Pm_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pm_CrtTime, _Pm_CrtTime, value);
    				this._Pm_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Pm_AnsTime {
    			get {
    				return this._Pm_AnsTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pm_AnsTime, _Pm_AnsTime, value);
    				this._Pm_AnsTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pm_IsAns {
    			get {
    				return this._Pm_IsAns;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pm_IsAns, _Pm_IsAns, value);
    				this._Pm_IsAns = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pm_IsShow {
    			get {
    				return this._Pm_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pm_IsShow, _Pm_IsShow, value);
    				this._Pm_IsShow = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pm_IP {
    			get {
    				return this._Pm_IP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pm_IP, _Pm_IP, value);
    				this._Pm_IP = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pm_Phone {
    			get {
    				return this._Pm_Phone;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pm_Phone, _Pm_Phone, value);
    				this._Pm_Phone = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pm_Email {
    			get {
    				return this._Pm_Email;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pm_Email, _Pm_Email, value);
    				this._Pm_Email = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pm_QQ {
    			get {
    				return this._Pm_QQ;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pm_QQ, _Pm_QQ, value);
    				this._Pm_QQ = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pm_Address {
    			get {
    				return this._Pm_Address;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pm_Address, _Pm_Address, value);
    				this._Pm_Address = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pd_Id {
    			get {
    				return this._Pd_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Id, _Pd_Id, value);
    				this._Pd_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_Name {
    			get {
    				return this._Pd_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Name, _Pd_Name, value);
    				this._Pd_Name = value;
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
    			return new WeiSha.Data.Table<ProductMessage>("ProductMessage");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Pm_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pm_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pm_Id,
    					_.Pm_Title,
    					_.Pm_Context,
    					_.Pm_Answer,
    					_.Pm_CrtTime,
    					_.Pm_AnsTime,
    					_.Pm_IsAns,
    					_.Pm_IsShow,
    					_.Pm_IP,
    					_.Pm_Phone,
    					_.Pm_Email,
    					_.Pm_QQ,
    					_.Pm_Address,
    					_.Pd_Id,
    					_.Pd_Name,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Pm_Id,
    					this._Pm_Title,
    					this._Pm_Context,
    					this._Pm_Answer,
    					this._Pm_CrtTime,
    					this._Pm_AnsTime,
    					this._Pm_IsAns,
    					this._Pm_IsShow,
    					this._Pm_IP,
    					this._Pm_Phone,
    					this._Pm_Email,
    					this._Pm_QQ,
    					this._Pm_Address,
    					this._Pd_Id,
    					this._Pd_Name,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Pm_Id))) {
    				this._Pm_Id = reader.GetInt32(_.Pm_Id);
    			}
    			if ((false == reader.IsDBNull(_.Pm_Title))) {
    				this._Pm_Title = reader.GetString(_.Pm_Title);
    			}
    			if ((false == reader.IsDBNull(_.Pm_Context))) {
    				this._Pm_Context = reader.GetString(_.Pm_Context);
    			}
    			if ((false == reader.IsDBNull(_.Pm_Answer))) {
    				this._Pm_Answer = reader.GetString(_.Pm_Answer);
    			}
    			if ((false == reader.IsDBNull(_.Pm_CrtTime))) {
    				this._Pm_CrtTime = reader.GetDateTime(_.Pm_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Pm_AnsTime))) {
    				this._Pm_AnsTime = reader.GetDateTime(_.Pm_AnsTime);
    			}
    			if ((false == reader.IsDBNull(_.Pm_IsAns))) {
    				this._Pm_IsAns = reader.GetBoolean(_.Pm_IsAns);
    			}
    			if ((false == reader.IsDBNull(_.Pm_IsShow))) {
    				this._Pm_IsShow = reader.GetBoolean(_.Pm_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Pm_IP))) {
    				this._Pm_IP = reader.GetString(_.Pm_IP);
    			}
    			if ((false == reader.IsDBNull(_.Pm_Phone))) {
    				this._Pm_Phone = reader.GetString(_.Pm_Phone);
    			}
    			if ((false == reader.IsDBNull(_.Pm_Email))) {
    				this._Pm_Email = reader.GetString(_.Pm_Email);
    			}
    			if ((false == reader.IsDBNull(_.Pm_QQ))) {
    				this._Pm_QQ = reader.GetString(_.Pm_QQ);
    			}
    			if ((false == reader.IsDBNull(_.Pm_Address))) {
    				this._Pm_Address = reader.GetString(_.Pm_Address);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Id))) {
    				this._Pd_Id = reader.GetInt32(_.Pd_Id);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Name))) {
    				this._Pd_Name = reader.GetString(_.Pd_Name);
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
    			if ((false == typeof(ProductMessage).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<ProductMessage>();
    			
    			/// <summary>
    			/// -1 - 字段名：Pm_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pm_Id = new WeiSha.Data.Field<ProductMessage>("Pm_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Pm_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pm_Title = new WeiSha.Data.Field<ProductMessage>("Pm_Title");
    			
    			/// <summary>
    			/// -1 - 字段名：Pm_Context - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pm_Context = new WeiSha.Data.Field<ProductMessage>("Pm_Context");
    			
    			/// <summary>
    			/// -1 - 字段名：Pm_Answer - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pm_Answer = new WeiSha.Data.Field<ProductMessage>("Pm_Answer");
    			
    			/// <summary>
    			/// -1 - 字段名：Pm_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pm_CrtTime = new WeiSha.Data.Field<ProductMessage>("Pm_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Pm_AnsTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pm_AnsTime = new WeiSha.Data.Field<ProductMessage>("Pm_AnsTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Pm_IsAns - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pm_IsAns = new WeiSha.Data.Field<ProductMessage>("Pm_IsAns");
    			
    			/// <summary>
    			/// -1 - 字段名：Pm_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pm_IsShow = new WeiSha.Data.Field<ProductMessage>("Pm_IsShow");
    			
    			/// <summary>
    			/// -1 - 字段名：Pm_IP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pm_IP = new WeiSha.Data.Field<ProductMessage>("Pm_IP");
    			
    			/// <summary>
    			/// -1 - 字段名：Pm_Phone - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pm_Phone = new WeiSha.Data.Field<ProductMessage>("Pm_Phone");
    			
    			/// <summary>
    			/// -1 - 字段名：Pm_Email - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pm_Email = new WeiSha.Data.Field<ProductMessage>("Pm_Email");
    			
    			/// <summary>
    			/// -1 - 字段名：Pm_QQ - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pm_QQ = new WeiSha.Data.Field<ProductMessage>("Pm_QQ");
    			
    			/// <summary>
    			/// -1 - 字段名：Pm_Address - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pm_Address = new WeiSha.Data.Field<ProductMessage>("Pm_Address");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Id = new WeiSha.Data.Field<ProductMessage>("Pd_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Name = new WeiSha.Data.Field<ProductMessage>("Pd_Name");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<ProductMessage>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<ProductMessage>("Org_Name");
    		}
    	}
    }
    