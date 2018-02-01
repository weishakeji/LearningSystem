namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：StudentSort 主键列：Sts_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class StudentSort : WeiSha.Data.Entity {
    		
    		protected Int32 _Sts_ID;
    		
    		protected String _Sts_Name;
    		
    		protected Int32 _Sts_Tax;
    		
    		protected String _Sts_Intro;
    		
    		protected Boolean _Sts_IsUse;
    		
    		protected Boolean _Sts_IsDefault;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected String _Dep_CnName;
    		
    		protected Int32 _Dep_Id;
    		
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
    		
    		public Int32 Sts_Tax {
    			get {
    				return this._Sts_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sts_Tax, _Sts_Tax, value);
    				this._Sts_Tax = value;
    			}
    		}
    		
    		public String Sts_Intro {
    			get {
    				return this._Sts_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sts_Intro, _Sts_Intro, value);
    				this._Sts_Intro = value;
    			}
    		}
    		
    		public Boolean Sts_IsUse {
    			get {
    				return this._Sts_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sts_IsUse, _Sts_IsUse, value);
    				this._Sts_IsUse = value;
    			}
    		}
    		
    		public Boolean Sts_IsDefault {
    			get {
    				return this._Sts_IsDefault;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sts_IsDefault, _Sts_IsDefault, value);
    				this._Sts_IsDefault = value;
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
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<StudentSort>("StudentSort");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Sts_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Sts_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Sts_ID,
    					_.Sts_Name,
    					_.Sts_Tax,
    					_.Sts_Intro,
    					_.Sts_IsUse,
    					_.Sts_IsDefault,
    					_.Org_ID,
    					_.Org_Name,
    					_.Dep_CnName,
    					_.Dep_Id};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Sts_ID,
    					this._Sts_Name,
    					this._Sts_Tax,
    					this._Sts_Intro,
    					this._Sts_IsUse,
    					this._Sts_IsDefault,
    					this._Org_ID,
    					this._Org_Name,
    					this._Dep_CnName,
    					this._Dep_Id};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Sts_ID))) {
    				this._Sts_ID = reader.GetInt32(_.Sts_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sts_Name))) {
    				this._Sts_Name = reader.GetString(_.Sts_Name);
    			}
    			if ((false == reader.IsDBNull(_.Sts_Tax))) {
    				this._Sts_Tax = reader.GetInt32(_.Sts_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Sts_Intro))) {
    				this._Sts_Intro = reader.GetString(_.Sts_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Sts_IsUse))) {
    				this._Sts_IsUse = reader.GetBoolean(_.Sts_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Sts_IsDefault))) {
    				this._Sts_IsDefault = reader.GetBoolean(_.Sts_IsDefault);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Dep_CnName))) {
    				this._Dep_CnName = reader.GetString(_.Dep_CnName);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Id))) {
    				this._Dep_Id = reader.GetInt32(_.Dep_Id);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(StudentSort).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<StudentSort>();
    			
    			/// <summary>
    			/// 字段名：Sts_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sts_ID = new WeiSha.Data.Field<StudentSort>("Sts_ID");
    			
    			/// <summary>
    			/// 字段名：Sts_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sts_Name = new WeiSha.Data.Field<StudentSort>("Sts_Name");
    			
    			/// <summary>
    			/// 字段名：Sts_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sts_Tax = new WeiSha.Data.Field<StudentSort>("Sts_Tax");
    			
    			/// <summary>
    			/// 字段名：Sts_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sts_Intro = new WeiSha.Data.Field<StudentSort>("Sts_Intro");
    			
    			/// <summary>
    			/// 字段名：Sts_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Sts_IsUse = new WeiSha.Data.Field<StudentSort>("Sts_IsUse");
    			
    			/// <summary>
    			/// 字段名：Sts_IsDefault - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Sts_IsDefault = new WeiSha.Data.Field<StudentSort>("Sts_IsDefault");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<StudentSort>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<StudentSort>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Dep_CnName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_CnName = new WeiSha.Data.Field<StudentSort>("Dep_CnName");
    			
    			/// <summary>
    			/// 字段名：Dep_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Id = new WeiSha.Data.Field<StudentSort>("Dep_Id");
    		}
    	}
    }
    