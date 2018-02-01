namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Purview 主键列：Pur_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Purview : WeiSha.Data.Entity {
    		
    		protected Int32 _Pur_Id;
    		
    		protected Int32? _Dep_Id;
    		
    		protected Int32? _EGrp_Id;
    		
    		protected Int32? _Posi_Id;
    		
    		protected Int32? _MM_Id;
    		
    		protected String _Pur_State;
    		
    		protected String _Pur_Type;
    		
    		protected Int32? _Org_ID;
    		
    		protected Int32? _Olv_ID;
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32 Pur_Id {
    			get {
    				return this._Pur_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pur_Id, _Pur_Id, value);
    				this._Pur_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Dep_Id {
    			get {
    				return this._Dep_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Id, _Dep_Id, value);
    				this._Dep_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? EGrp_Id {
    			get {
    				return this._EGrp_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.EGrp_Id, _EGrp_Id, value);
    				this._EGrp_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Posi_Id {
    			get {
    				return this._Posi_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Posi_Id, _Posi_Id, value);
    				this._Posi_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? MM_Id {
    			get {
    				return this._MM_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Id, _MM_Id, value);
    				this._MM_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Pur_State {
    			get {
    				return this._Pur_State;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pur_State, _Pur_State, value);
    				this._Pur_State = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Pur_Type {
    			get {
    				return this._Pur_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pur_Type, _Pur_Type, value);
    				this._Pur_Type = value;
    			}
    		}
    		
    		public Int32? Org_ID {
    			get {
    				return this._Org_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_ID, _Org_ID, value);
    				this._Org_ID = value;
    			}
    		}
    		
    		public Int32? Olv_ID {
    			get {
    				return this._Olv_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Olv_ID, _Olv_ID, value);
    				this._Olv_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Purview>("Purview");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Pur_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pur_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pur_Id,
    					_.Dep_Id,
    					_.EGrp_Id,
    					_.Posi_Id,
    					_.MM_Id,
    					_.Pur_State,
    					_.Pur_Type,
    					_.Org_ID,
    					_.Olv_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Pur_Id,
    					this._Dep_Id,
    					this._EGrp_Id,
    					this._Posi_Id,
    					this._MM_Id,
    					this._Pur_State,
    					this._Pur_Type,
    					this._Org_ID,
    					this._Olv_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Pur_Id))) {
    				this._Pur_Id = reader.GetInt32(_.Pur_Id);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Id))) {
    				this._Dep_Id = reader.GetInt32(_.Dep_Id);
    			}
    			if ((false == reader.IsDBNull(_.EGrp_Id))) {
    				this._EGrp_Id = reader.GetInt32(_.EGrp_Id);
    			}
    			if ((false == reader.IsDBNull(_.Posi_Id))) {
    				this._Posi_Id = reader.GetInt32(_.Posi_Id);
    			}
    			if ((false == reader.IsDBNull(_.MM_Id))) {
    				this._MM_Id = reader.GetInt32(_.MM_Id);
    			}
    			if ((false == reader.IsDBNull(_.Pur_State))) {
    				this._Pur_State = reader.GetString(_.Pur_State);
    			}
    			if ((false == reader.IsDBNull(_.Pur_Type))) {
    				this._Pur_Type = reader.GetString(_.Pur_Type);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Olv_ID))) {
    				this._Olv_ID = reader.GetInt32(_.Olv_ID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Purview).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Purview>();
    			
    			/// <summary>
    			/// False - 字段名：Pur_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pur_Id = new WeiSha.Data.Field<Purview>("Pur_Id");
    			
    			/// <summary>
    			/// False - 字段名：Dep_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Id = new WeiSha.Data.Field<Purview>("Dep_Id");
    			
    			/// <summary>
    			/// False - 字段名：EGrp_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field EGrp_Id = new WeiSha.Data.Field<Purview>("EGrp_Id");
    			
    			/// <summary>
    			/// False - 字段名：Posi_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Posi_Id = new WeiSha.Data.Field<Purview>("Posi_Id");
    			
    			/// <summary>
    			/// False - 字段名：MM_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field MM_Id = new WeiSha.Data.Field<Purview>("MM_Id");
    			
    			/// <summary>
    			/// True - 字段名：Pur_State - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pur_State = new WeiSha.Data.Field<Purview>("Pur_State");
    			
    			/// <summary>
    			/// True - 字段名：Pur_Type - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pur_Type = new WeiSha.Data.Field<Purview>("Pur_Type");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Purview>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Olv_ID - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Olv_ID = new WeiSha.Data.Field<Purview>("Olv_ID");
    		}
    	}
    }
    