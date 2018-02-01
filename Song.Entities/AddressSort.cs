namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：AddressSort 主键列：Ads_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class AddressSort : WeiSha.Data.Entity {
    		
    		protected Int32 _Ads_Id;
    		
    		protected String _Ads_Name;
    		
    		protected Boolean _Ads_IsUse;
    		
    		protected DateTime? _Ads_CrtTime;
    		
    		protected Int32 _Ads_Tax;
    		
    		protected Int32? _Acc_Id;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Ads_Id {
    			get {
    				return this._Ads_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ads_Id, _Ads_Id, value);
    				this._Ads_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Ads_Name {
    			get {
    				return this._Ads_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ads_Name, _Ads_Name, value);
    				this._Ads_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Ads_IsUse {
    			get {
    				return this._Ads_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ads_IsUse, _Ads_IsUse, value);
    				this._Ads_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Ads_CrtTime {
    			get {
    				return this._Ads_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ads_CrtTime, _Ads_CrtTime, value);
    				this._Ads_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Ads_Tax {
    			get {
    				return this._Ads_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ads_Tax, _Ads_Tax, value);
    				this._Ads_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Acc_Id {
    			get {
    				return this._Acc_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Id, _Acc_Id, value);
    				this._Acc_Id = value;
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
    			return new WeiSha.Data.Table<AddressSort>("AddressSort");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Ads_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ads_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ads_Id,
    					_.Ads_Name,
    					_.Ads_IsUse,
    					_.Ads_CrtTime,
    					_.Ads_Tax,
    					_.Acc_Id,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Ads_Id,
    					this._Ads_Name,
    					this._Ads_IsUse,
    					this._Ads_CrtTime,
    					this._Ads_Tax,
    					this._Acc_Id,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Ads_Id))) {
    				this._Ads_Id = reader.GetInt32(_.Ads_Id);
    			}
    			if ((false == reader.IsDBNull(_.Ads_Name))) {
    				this._Ads_Name = reader.GetString(_.Ads_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ads_IsUse))) {
    				this._Ads_IsUse = reader.GetBoolean(_.Ads_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Ads_CrtTime))) {
    				this._Ads_CrtTime = reader.GetDateTime(_.Ads_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Ads_Tax))) {
    				this._Ads_Tax = reader.GetInt32(_.Ads_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
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
    			if ((false == typeof(AddressSort).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<AddressSort>();
    			
    			/// <summary>
    			/// -1 - 字段名：Ads_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ads_Id = new WeiSha.Data.Field<AddressSort>("Ads_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Ads_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ads_Name = new WeiSha.Data.Field<AddressSort>("Ads_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Ads_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ads_IsUse = new WeiSha.Data.Field<AddressSort>("Ads_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Ads_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Ads_CrtTime = new WeiSha.Data.Field<AddressSort>("Ads_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Ads_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ads_Tax = new WeiSha.Data.Field<AddressSort>("Ads_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<AddressSort>("Acc_Id");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<AddressSort>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<AddressSort>("Org_Name");
    		}
    	}
    }
    