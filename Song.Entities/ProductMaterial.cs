namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ProductMaterial 主键列：Pmat_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ProductMaterial : WeiSha.Data.Entity {
    		
    		protected Int32 _Pmat_Id;
    		
    		protected String _Pmat_Name;
    		
    		protected String _Pmat_Intro;
    		
    		protected Boolean _Pmat_IsUse;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Pmat_Id {
    			get {
    				return this._Pmat_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pmat_Id, _Pmat_Id, value);
    				this._Pmat_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pmat_Name {
    			get {
    				return this._Pmat_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pmat_Name, _Pmat_Name, value);
    				this._Pmat_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pmat_Intro {
    			get {
    				return this._Pmat_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pmat_Intro, _Pmat_Intro, value);
    				this._Pmat_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pmat_IsUse {
    			get {
    				return this._Pmat_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pmat_IsUse, _Pmat_IsUse, value);
    				this._Pmat_IsUse = value;
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
    			return new WeiSha.Data.Table<ProductMaterial>("ProductMaterial");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Pmat_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pmat_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pmat_Id,
    					_.Pmat_Name,
    					_.Pmat_Intro,
    					_.Pmat_IsUse,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Pmat_Id,
    					this._Pmat_Name,
    					this._Pmat_Intro,
    					this._Pmat_IsUse,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Pmat_Id))) {
    				this._Pmat_Id = reader.GetInt32(_.Pmat_Id);
    			}
    			if ((false == reader.IsDBNull(_.Pmat_Name))) {
    				this._Pmat_Name = reader.GetString(_.Pmat_Name);
    			}
    			if ((false == reader.IsDBNull(_.Pmat_Intro))) {
    				this._Pmat_Intro = reader.GetString(_.Pmat_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Pmat_IsUse))) {
    				this._Pmat_IsUse = reader.GetBoolean(_.Pmat_IsUse);
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
    			if ((false == typeof(ProductMaterial).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<ProductMaterial>();
    			
    			/// <summary>
    			/// -1 - 字段名：Pmat_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pmat_Id = new WeiSha.Data.Field<ProductMaterial>("Pmat_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Pmat_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pmat_Name = new WeiSha.Data.Field<ProductMaterial>("Pmat_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Pmat_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pmat_Intro = new WeiSha.Data.Field<ProductMaterial>("Pmat_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Pmat_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pmat_IsUse = new WeiSha.Data.Field<ProductMaterial>("Pmat_IsUse");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<ProductMaterial>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<ProductMaterial>("Org_Name");
    		}
    	}
    }
    