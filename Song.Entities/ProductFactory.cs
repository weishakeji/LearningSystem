namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ProductFactory 主键列：Pfact_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ProductFactory : WeiSha.Data.Entity {
    		
    		protected Int32 _Pfact_Id;
    		
    		protected String _Pfact_Name;
    		
    		protected String _Pfact_Addr;
    		
    		protected String _Pfact_Zip;
    		
    		protected String _Pfact_Intro;
    		
    		protected String _Pfact_Tel;
    		
    		protected String _Pfact_Website;
    		
    		protected Boolean _Pfact_IsUse;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Pfact_Id {
    			get {
    				return this._Pfact_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pfact_Id, _Pfact_Id, value);
    				this._Pfact_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pfact_Name {
    			get {
    				return this._Pfact_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pfact_Name, _Pfact_Name, value);
    				this._Pfact_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pfact_Addr {
    			get {
    				return this._Pfact_Addr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pfact_Addr, _Pfact_Addr, value);
    				this._Pfact_Addr = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pfact_Zip {
    			get {
    				return this._Pfact_Zip;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pfact_Zip, _Pfact_Zip, value);
    				this._Pfact_Zip = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pfact_Intro {
    			get {
    				return this._Pfact_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pfact_Intro, _Pfact_Intro, value);
    				this._Pfact_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pfact_Tel {
    			get {
    				return this._Pfact_Tel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pfact_Tel, _Pfact_Tel, value);
    				this._Pfact_Tel = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pfact_Website {
    			get {
    				return this._Pfact_Website;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pfact_Website, _Pfact_Website, value);
    				this._Pfact_Website = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pfact_IsUse {
    			get {
    				return this._Pfact_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pfact_IsUse, _Pfact_IsUse, value);
    				this._Pfact_IsUse = value;
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
    			return new WeiSha.Data.Table<ProductFactory>("ProductFactory");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Pfact_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pfact_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pfact_Id,
    					_.Pfact_Name,
    					_.Pfact_Addr,
    					_.Pfact_Zip,
    					_.Pfact_Intro,
    					_.Pfact_Tel,
    					_.Pfact_Website,
    					_.Pfact_IsUse,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Pfact_Id,
    					this._Pfact_Name,
    					this._Pfact_Addr,
    					this._Pfact_Zip,
    					this._Pfact_Intro,
    					this._Pfact_Tel,
    					this._Pfact_Website,
    					this._Pfact_IsUse,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Pfact_Id))) {
    				this._Pfact_Id = reader.GetInt32(_.Pfact_Id);
    			}
    			if ((false == reader.IsDBNull(_.Pfact_Name))) {
    				this._Pfact_Name = reader.GetString(_.Pfact_Name);
    			}
    			if ((false == reader.IsDBNull(_.Pfact_Addr))) {
    				this._Pfact_Addr = reader.GetString(_.Pfact_Addr);
    			}
    			if ((false == reader.IsDBNull(_.Pfact_Zip))) {
    				this._Pfact_Zip = reader.GetString(_.Pfact_Zip);
    			}
    			if ((false == reader.IsDBNull(_.Pfact_Intro))) {
    				this._Pfact_Intro = reader.GetString(_.Pfact_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Pfact_Tel))) {
    				this._Pfact_Tel = reader.GetString(_.Pfact_Tel);
    			}
    			if ((false == reader.IsDBNull(_.Pfact_Website))) {
    				this._Pfact_Website = reader.GetString(_.Pfact_Website);
    			}
    			if ((false == reader.IsDBNull(_.Pfact_IsUse))) {
    				this._Pfact_IsUse = reader.GetBoolean(_.Pfact_IsUse);
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
    			if ((false == typeof(ProductFactory).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<ProductFactory>();
    			
    			/// <summary>
    			/// -1 - 字段名：Pfact_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pfact_Id = new WeiSha.Data.Field<ProductFactory>("Pfact_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Pfact_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pfact_Name = new WeiSha.Data.Field<ProductFactory>("Pfact_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Pfact_Addr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pfact_Addr = new WeiSha.Data.Field<ProductFactory>("Pfact_Addr");
    			
    			/// <summary>
    			/// -1 - 字段名：Pfact_Zip - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pfact_Zip = new WeiSha.Data.Field<ProductFactory>("Pfact_Zip");
    			
    			/// <summary>
    			/// -1 - 字段名：Pfact_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pfact_Intro = new WeiSha.Data.Field<ProductFactory>("Pfact_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Pfact_Tel - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pfact_Tel = new WeiSha.Data.Field<ProductFactory>("Pfact_Tel");
    			
    			/// <summary>
    			/// -1 - 字段名：Pfact_Website - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pfact_Website = new WeiSha.Data.Field<ProductFactory>("Pfact_Website");
    			
    			/// <summary>
    			/// -1 - 字段名：Pfact_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pfact_IsUse = new WeiSha.Data.Field<ProductFactory>("Pfact_IsUse");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<ProductFactory>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<ProductFactory>("Org_Name");
    		}
    	}
    }
    