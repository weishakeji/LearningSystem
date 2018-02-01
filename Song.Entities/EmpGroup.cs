namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：EmpGroup 主键列：EGrp_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class EmpGroup : WeiSha.Data.Entity {
    		
    		protected Int32 _EGrp_Id;
    		
    		protected String _EGrp_Name;
    		
    		protected Boolean _EGrp_IsUse;
    		
    		protected Int32 _EGrp_Tax;
    		
    		protected String _EGrp_Intro;
    		
    		protected Boolean _EGrp_IsSystem;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
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
    		public String EGrp_Name {
    			get {
    				return this._EGrp_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.EGrp_Name, _EGrp_Name, value);
    				this._EGrp_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean EGrp_IsUse {
    			get {
    				return this._EGrp_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.EGrp_IsUse, _EGrp_IsUse, value);
    				this._EGrp_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 EGrp_Tax {
    			get {
    				return this._EGrp_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.EGrp_Tax, _EGrp_Tax, value);
    				this._EGrp_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String EGrp_Intro {
    			get {
    				return this._EGrp_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.EGrp_Intro, _EGrp_Intro, value);
    				this._EGrp_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean EGrp_IsSystem {
    			get {
    				return this._EGrp_IsSystem;
    			}
    			set {
    				this.OnPropertyValueChange(_.EGrp_IsSystem, _EGrp_IsSystem, value);
    				this._EGrp_IsSystem = value;
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
    			return new WeiSha.Data.Table<EmpGroup>("EmpGroup");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.EGrp_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.EGrp_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.EGrp_Id,
    					_.EGrp_Name,
    					_.EGrp_IsUse,
    					_.EGrp_Tax,
    					_.EGrp_Intro,
    					_.EGrp_IsSystem,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._EGrp_Id,
    					this._EGrp_Name,
    					this._EGrp_IsUse,
    					this._EGrp_Tax,
    					this._EGrp_Intro,
    					this._EGrp_IsSystem,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.EGrp_Id))) {
    				this._EGrp_Id = reader.GetInt32(_.EGrp_Id);
    			}
    			if ((false == reader.IsDBNull(_.EGrp_Name))) {
    				this._EGrp_Name = reader.GetString(_.EGrp_Name);
    			}
    			if ((false == reader.IsDBNull(_.EGrp_IsUse))) {
    				this._EGrp_IsUse = reader.GetBoolean(_.EGrp_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.EGrp_Tax))) {
    				this._EGrp_Tax = reader.GetInt32(_.EGrp_Tax);
    			}
    			if ((false == reader.IsDBNull(_.EGrp_Intro))) {
    				this._EGrp_Intro = reader.GetString(_.EGrp_Intro);
    			}
    			if ((false == reader.IsDBNull(_.EGrp_IsSystem))) {
    				this._EGrp_IsSystem = reader.GetBoolean(_.EGrp_IsSystem);
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
    			if ((false == typeof(EmpGroup).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<EmpGroup>();
    			
    			/// <summary>
    			/// -1 - 字段名：EGrp_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field EGrp_Id = new WeiSha.Data.Field<EmpGroup>("EGrp_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：EGrp_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field EGrp_Name = new WeiSha.Data.Field<EmpGroup>("EGrp_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：EGrp_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field EGrp_IsUse = new WeiSha.Data.Field<EmpGroup>("EGrp_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：EGrp_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field EGrp_Tax = new WeiSha.Data.Field<EmpGroup>("EGrp_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：EGrp_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field EGrp_Intro = new WeiSha.Data.Field<EmpGroup>("EGrp_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：EGrp_IsSystem - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field EGrp_IsSystem = new WeiSha.Data.Field<EmpGroup>("EGrp_IsSystem");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<EmpGroup>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<EmpGroup>("Org_Name");
    		}
    	}
    }
    