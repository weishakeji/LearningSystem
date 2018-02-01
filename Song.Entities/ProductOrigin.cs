namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ProductOrigin 主键列：Pori_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ProductOrigin : WeiSha.Data.Entity {
    		
    		protected Int32 _Pori_Id;
    		
    		protected String _Pori_Name;
    		
    		protected String _Pori_Intro;
    		
    		protected Boolean _Pori_IsUse;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Pori_Id {
    			get {
    				return this._Pori_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pori_Id, _Pori_Id, value);
    				this._Pori_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pori_Name {
    			get {
    				return this._Pori_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pori_Name, _Pori_Name, value);
    				this._Pori_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pori_Intro {
    			get {
    				return this._Pori_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pori_Intro, _Pori_Intro, value);
    				this._Pori_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pori_IsUse {
    			get {
    				return this._Pori_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pori_IsUse, _Pori_IsUse, value);
    				this._Pori_IsUse = value;
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
    			return new WeiSha.Data.Table<ProductOrigin>("ProductOrigin");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Pori_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pori_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pori_Id,
    					_.Pori_Name,
    					_.Pori_Intro,
    					_.Pori_IsUse,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Pori_Id,
    					this._Pori_Name,
    					this._Pori_Intro,
    					this._Pori_IsUse,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Pori_Id))) {
    				this._Pori_Id = reader.GetInt32(_.Pori_Id);
    			}
    			if ((false == reader.IsDBNull(_.Pori_Name))) {
    				this._Pori_Name = reader.GetString(_.Pori_Name);
    			}
    			if ((false == reader.IsDBNull(_.Pori_Intro))) {
    				this._Pori_Intro = reader.GetString(_.Pori_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Pori_IsUse))) {
    				this._Pori_IsUse = reader.GetBoolean(_.Pori_IsUse);
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
    			if ((false == typeof(ProductOrigin).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<ProductOrigin>();
    			
    			/// <summary>
    			/// -1 - 字段名：Pori_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pori_Id = new WeiSha.Data.Field<ProductOrigin>("Pori_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Pori_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pori_Name = new WeiSha.Data.Field<ProductOrigin>("Pori_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Pori_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pori_Intro = new WeiSha.Data.Field<ProductOrigin>("Pori_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Pori_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pori_IsUse = new WeiSha.Data.Field<ProductOrigin>("Pori_IsUse");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<ProductOrigin>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<ProductOrigin>("Org_Name");
    		}
    	}
    }
    