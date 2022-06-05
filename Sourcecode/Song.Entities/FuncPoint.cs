namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：FuncPoint 主键列：FPI_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class FuncPoint : WeiSha.Data.Entity {
    		
    		protected Int32 _FPI_Id;
    		
    		protected String _FPI_Name;
    		
    		protected Boolean _FPI_IsUse;
    		
    		protected Boolean _FPI_IsShow;
    		
    		protected Int32? _Org_Id;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32 FPI_Id {
    			get {
    				return this._FPI_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.FPI_Id, _FPI_Id, value);
    				this._FPI_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String FPI_Name {
    			get {
    				return this._FPI_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.FPI_Name, _FPI_Name, value);
    				this._FPI_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Boolean FPI_IsUse {
    			get {
    				return this._FPI_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.FPI_IsUse, _FPI_IsUse, value);
    				this._FPI_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Boolean FPI_IsShow {
    			get {
    				return this._FPI_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.FPI_IsShow, _FPI_IsShow, value);
    				this._FPI_IsShow = value;
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
    			return new WeiSha.Data.Table<FuncPoint>("FuncPoint");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.FPI_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.FPI_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.FPI_Id,
    					_.FPI_Name,
    					_.FPI_IsUse,
    					_.FPI_IsShow,
    					_.Org_Id,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._FPI_Id,
    					this._FPI_Name,
    					this._FPI_IsUse,
    					this._FPI_IsShow,
    					this._Org_Id,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.FPI_Id))) {
    				this._FPI_Id = reader.GetInt32(_.FPI_Id);
    			}
    			if ((false == reader.IsDBNull(_.FPI_Name))) {
    				this._FPI_Name = reader.GetString(_.FPI_Name);
    			}
    			if ((false == reader.IsDBNull(_.FPI_IsUse))) {
    				this._FPI_IsUse = reader.GetBoolean(_.FPI_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.FPI_IsShow))) {
    				this._FPI_IsShow = reader.GetBoolean(_.FPI_IsShow);
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
    			if ((false == typeof(FuncPoint).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<FuncPoint>();
    			
    			/// <summary>
    			/// False - 字段名：FPI_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field FPI_Id = new WeiSha.Data.Field<FuncPoint>("FPI_Id");
    			
    			/// <summary>
    			/// True - 字段名：FPI_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field FPI_Name = new WeiSha.Data.Field<FuncPoint>("FPI_Name");
    			
    			/// <summary>
    			/// False - 字段名：FPI_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field FPI_IsUse = new WeiSha.Data.Field<FuncPoint>("FPI_IsUse");
    			
    			/// <summary>
    			/// False - 字段名：FPI_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field FPI_IsShow = new WeiSha.Data.Field<FuncPoint>("FPI_IsShow");
    			
    			/// <summary>
    			/// 字段名：Org_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Org_Id = new WeiSha.Data.Field<FuncPoint>("Org_Id");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<FuncPoint>("Org_Name");
    		}
    	}
    }
    