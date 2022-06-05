namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Product_Package 主键列：Pdpk_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Product_Package : WeiSha.Data.Entity {
    		
    		protected Int32 _Pdpk_Id;
    		
    		protected Int32? _Pd_Id;
    		
    		protected Int32? _PPK_Id;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Pdpk_Id {
    			get {
    				return this._Pdpk_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pdpk_Id, _Pdpk_Id, value);
    				this._Pdpk_Id = value;
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
    		public Int32? PPK_Id {
    			get {
    				return this._PPK_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.PPK_Id, _PPK_Id, value);
    				this._PPK_Id = value;
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
    			return new WeiSha.Data.Table<Product_Package>("Product_Package");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Pdpk_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pdpk_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pdpk_Id,
    					_.Pd_Id,
    					_.PPK_Id,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Pdpk_Id,
    					this._Pd_Id,
    					this._PPK_Id,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Pdpk_Id))) {
    				this._Pdpk_Id = reader.GetInt32(_.Pdpk_Id);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Id))) {
    				this._Pd_Id = reader.GetInt32(_.Pd_Id);
    			}
    			if ((false == reader.IsDBNull(_.PPK_Id))) {
    				this._PPK_Id = reader.GetInt32(_.PPK_Id);
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
    			if ((false == typeof(Product_Package).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Product_Package>();
    			
    			/// <summary>
    			/// -1 - 字段名：Pdpk_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pdpk_Id = new WeiSha.Data.Field<Product_Package>("Pdpk_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Id = new WeiSha.Data.Field<Product_Package>("Pd_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：PPK_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field PPK_Id = new WeiSha.Data.Field<Product_Package>("PPK_Id");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Product_Package>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Product_Package>("Org_Name");
    		}
    	}
    }
    