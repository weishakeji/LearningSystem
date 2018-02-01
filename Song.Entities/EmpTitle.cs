namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：EmpTitle 主键列：Title_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class EmpTitle : WeiSha.Data.Entity {
    		
    		protected Int32 _Title_Id;
    		
    		protected String _Title_Name;
    		
    		protected Boolean _Title_IsUse;
    		
    		protected Int32? _Title_Tax;
    		
    		protected String _Title_Intro;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
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
    		public Boolean Title_IsUse {
    			get {
    				return this._Title_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Title_IsUse, _Title_IsUse, value);
    				this._Title_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Title_Tax {
    			get {
    				return this._Title_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Title_Tax, _Title_Tax, value);
    				this._Title_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Title_Intro {
    			get {
    				return this._Title_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Title_Intro, _Title_Intro, value);
    				this._Title_Intro = value;
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
    			return new WeiSha.Data.Table<EmpTitle>("EmpTitle");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Title_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Title_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Title_Id,
    					_.Title_Name,
    					_.Title_IsUse,
    					_.Title_Tax,
    					_.Title_Intro,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Title_Id,
    					this._Title_Name,
    					this._Title_IsUse,
    					this._Title_Tax,
    					this._Title_Intro,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Title_Id))) {
    				this._Title_Id = reader.GetInt32(_.Title_Id);
    			}
    			if ((false == reader.IsDBNull(_.Title_Name))) {
    				this._Title_Name = reader.GetString(_.Title_Name);
    			}
    			if ((false == reader.IsDBNull(_.Title_IsUse))) {
    				this._Title_IsUse = reader.GetBoolean(_.Title_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Title_Tax))) {
    				this._Title_Tax = reader.GetInt32(_.Title_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Title_Intro))) {
    				this._Title_Intro = reader.GetString(_.Title_Intro);
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
    			if ((false == typeof(EmpTitle).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<EmpTitle>();
    			
    			/// <summary>
    			/// -1 - 字段名：Title_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Title_Id = new WeiSha.Data.Field<EmpTitle>("Title_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Title_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Title_Name = new WeiSha.Data.Field<EmpTitle>("Title_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Title_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Title_IsUse = new WeiSha.Data.Field<EmpTitle>("Title_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Title_Tax - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Title_Tax = new WeiSha.Data.Field<EmpTitle>("Title_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Title_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Title_Intro = new WeiSha.Data.Field<EmpTitle>("Title_Intro");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<EmpTitle>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<EmpTitle>("Org_Name");
    		}
    	}
    }
    