namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Special_Article 主键列：Spa_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Special_Article : WeiSha.Data.Entity {
    		
    		protected Int32 _Spa_Id;
    		
    		protected Int32? _Sp_Id;
    		
    		protected Int32? _Art_Id;
    		
    		protected Int32? _Org_Id;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Spa_Id {
    			get {
    				return this._Spa_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Spa_Id, _Spa_Id, value);
    				this._Spa_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Sp_Id {
    			get {
    				return this._Sp_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_Id, _Sp_Id, value);
    				this._Sp_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Art_Id {
    			get {
    				return this._Art_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Id, _Art_Id, value);
    				this._Art_Id = value;
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
    			return new WeiSha.Data.Table<Special_Article>("Special_Article");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Spa_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Spa_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Spa_Id,
    					_.Sp_Id,
    					_.Art_Id,
    					_.Org_Id,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Spa_Id,
    					this._Sp_Id,
    					this._Art_Id,
    					this._Org_Id,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Spa_Id))) {
    				this._Spa_Id = reader.GetInt32(_.Spa_Id);
    			}
    			if ((false == reader.IsDBNull(_.Sp_Id))) {
    				this._Sp_Id = reader.GetInt32(_.Sp_Id);
    			}
    			if ((false == reader.IsDBNull(_.Art_Id))) {
    				this._Art_Id = reader.GetInt32(_.Art_Id);
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
    			if ((false == typeof(Special_Article).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Special_Article>();
    			
    			/// <summary>
    			/// -1 - 字段名：Spa_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Spa_Id = new WeiSha.Data.Field<Special_Article>("Spa_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Sp_Id = new WeiSha.Data.Field<Special_Article>("Sp_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Art_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Art_Id = new WeiSha.Data.Field<Special_Article>("Art_Id");
    			
    			/// <summary>
    			/// 字段名：Org_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Org_Id = new WeiSha.Data.Field<Special_Article>("Org_Id");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Special_Article>("Org_Name");
    		}
    	}
    }
    