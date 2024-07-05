namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ThirdpartyLogin 主键列：Tl_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ThirdpartyLogin : WeiSha.Data.Entity {
    		
    		protected Int32 _Tl_ID;
    		
    		protected String _Tl_APPID;
    		
    		protected String _Tl_Account;
    		
    		protected String _Tl_Config;
    		
    		protected String _Tl_Domain;
    		
    		protected Boolean _Tl_IsRegister;
    		
    		protected Boolean _Tl_IsUse;
    		
    		protected String _Tl_Name;
    		
    		protected String _Tl_Returl;
    		
    		protected String _Tl_Secret;
    		
    		protected String _Tl_Tag;
    		
    		protected Int32 _Tl_Tax;
    		
    		public Int32 Tl_ID {
    			get {
    				return this._Tl_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tl_ID, _Tl_ID, value);
    				this._Tl_ID = value;
    			}
    		}
    		
    		public String Tl_APPID {
    			get {
    				return this._Tl_APPID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tl_APPID, _Tl_APPID, value);
    				this._Tl_APPID = value;
    			}
    		}
    		
    		public String Tl_Account {
    			get {
    				return this._Tl_Account;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tl_Account, _Tl_Account, value);
    				this._Tl_Account = value;
    			}
    		}
    		
    		public String Tl_Config {
    			get {
    				return this._Tl_Config;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tl_Config, _Tl_Config, value);
    				this._Tl_Config = value;
    			}
    		}
    		
    		public String Tl_Domain {
    			get {
    				return this._Tl_Domain;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tl_Domain, _Tl_Domain, value);
    				this._Tl_Domain = value;
    			}
    		}
    		
    		public Boolean Tl_IsRegister {
    			get {
    				return this._Tl_IsRegister;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tl_IsRegister, _Tl_IsRegister, value);
    				this._Tl_IsRegister = value;
    			}
    		}
    		
    		public Boolean Tl_IsUse {
    			get {
    				return this._Tl_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tl_IsUse, _Tl_IsUse, value);
    				this._Tl_IsUse = value;
    			}
    		}
    		
    		public String Tl_Name {
    			get {
    				return this._Tl_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tl_Name, _Tl_Name, value);
    				this._Tl_Name = value;
    			}
    		}
    		
    		public String Tl_Returl {
    			get {
    				return this._Tl_Returl;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tl_Returl, _Tl_Returl, value);
    				this._Tl_Returl = value;
    			}
    		}
    		
    		public String Tl_Secret {
    			get {
    				return this._Tl_Secret;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tl_Secret, _Tl_Secret, value);
    				this._Tl_Secret = value;
    			}
    		}
    		
    		public String Tl_Tag {
    			get {
    				return this._Tl_Tag;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tl_Tag, _Tl_Tag, value);
    				this._Tl_Tag = value;
    			}
    		}
    		
    		public Int32 Tl_Tax {
    			get {
    				return this._Tl_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tl_Tax, _Tl_Tax, value);
    				this._Tl_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<ThirdpartyLogin>("ThirdpartyLogin");
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Tl_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Tl_ID,
    					_.Tl_APPID,
    					_.Tl_Account,
    					_.Tl_Config,
    					_.Tl_Domain,
    					_.Tl_IsRegister,
    					_.Tl_IsUse,
    					_.Tl_Name,
    					_.Tl_Returl,
    					_.Tl_Secret,
    					_.Tl_Tag,
    					_.Tl_Tax};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Tl_ID,
    					this._Tl_APPID,
    					this._Tl_Account,
    					this._Tl_Config,
    					this._Tl_Domain,
    					this._Tl_IsRegister,
    					this._Tl_IsUse,
    					this._Tl_Name,
    					this._Tl_Returl,
    					this._Tl_Secret,
    					this._Tl_Tag,
    					this._Tl_Tax};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Tl_ID))) {
    				this._Tl_ID = reader.GetInt32(_.Tl_ID);
    			}
    			if ((false == reader.IsDBNull(_.Tl_APPID))) {
    				this._Tl_APPID = reader.GetString(_.Tl_APPID);
    			}
    			if ((false == reader.IsDBNull(_.Tl_Account))) {
    				this._Tl_Account = reader.GetString(_.Tl_Account);
    			}
    			if ((false == reader.IsDBNull(_.Tl_Config))) {
    				this._Tl_Config = reader.GetString(_.Tl_Config);
    			}
    			if ((false == reader.IsDBNull(_.Tl_Domain))) {
    				this._Tl_Domain = reader.GetString(_.Tl_Domain);
    			}
    			if ((false == reader.IsDBNull(_.Tl_IsRegister))) {
    				this._Tl_IsRegister = reader.GetBoolean(_.Tl_IsRegister);
    			}
    			if ((false == reader.IsDBNull(_.Tl_IsUse))) {
    				this._Tl_IsUse = reader.GetBoolean(_.Tl_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Tl_Name))) {
    				this._Tl_Name = reader.GetString(_.Tl_Name);
    			}
    			if ((false == reader.IsDBNull(_.Tl_Returl))) {
    				this._Tl_Returl = reader.GetString(_.Tl_Returl);
    			}
    			if ((false == reader.IsDBNull(_.Tl_Secret))) {
    				this._Tl_Secret = reader.GetString(_.Tl_Secret);
    			}
    			if ((false == reader.IsDBNull(_.Tl_Tag))) {
    				this._Tl_Tag = reader.GetString(_.Tl_Tag);
    			}
    			if ((false == reader.IsDBNull(_.Tl_Tax))) {
    				this._Tl_Tax = reader.GetInt32(_.Tl_Tax);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(ThirdpartyLogin).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<ThirdpartyLogin>();
    			
    			/// <summary>
    			/// 字段名：Tl_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tl_ID = new WeiSha.Data.Field<ThirdpartyLogin>("Tl_ID");
    			
    			/// <summary>
    			/// 字段名：Tl_APPID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tl_APPID = new WeiSha.Data.Field<ThirdpartyLogin>("Tl_APPID");
    			
    			/// <summary>
    			/// 字段名：Tl_Account - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tl_Account = new WeiSha.Data.Field<ThirdpartyLogin>("Tl_Account");
    			
    			/// <summary>
    			/// 字段名：Tl_Config - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tl_Config = new WeiSha.Data.Field<ThirdpartyLogin>("Tl_Config");
    			
    			/// <summary>
    			/// 字段名：Tl_Domain - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tl_Domain = new WeiSha.Data.Field<ThirdpartyLogin>("Tl_Domain");
    			
    			/// <summary>
    			/// 字段名：Tl_IsRegister - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Tl_IsRegister = new WeiSha.Data.Field<ThirdpartyLogin>("Tl_IsRegister");
    			
    			/// <summary>
    			/// 字段名：Tl_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Tl_IsUse = new WeiSha.Data.Field<ThirdpartyLogin>("Tl_IsUse");
    			
    			/// <summary>
    			/// 字段名：Tl_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tl_Name = new WeiSha.Data.Field<ThirdpartyLogin>("Tl_Name");
    			
    			/// <summary>
    			/// 字段名：Tl_Returl - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tl_Returl = new WeiSha.Data.Field<ThirdpartyLogin>("Tl_Returl");
    			
    			/// <summary>
    			/// 字段名：Tl_Secret - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tl_Secret = new WeiSha.Data.Field<ThirdpartyLogin>("Tl_Secret");
    			
    			/// <summary>
    			/// 字段名：Tl_Tag - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tl_Tag = new WeiSha.Data.Field<ThirdpartyLogin>("Tl_Tag");
    			
    			/// <summary>
    			/// 字段名：Tl_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tl_Tax = new WeiSha.Data.Field<ThirdpartyLogin>("Tl_Tax");
    		}
    	}
    }
