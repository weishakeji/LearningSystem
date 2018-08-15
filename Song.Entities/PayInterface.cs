namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：PayInterface 主键列：Pai_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class PayInterface : WeiSha.Data.Entity {
    		
    		protected Int32 _Pai_ID;
    		
    		protected String _Pai_Name;
    		
    		protected String _Pai_Pattern;
    		
    		protected Int32 _Pai_Tax;
    		
    		protected String _Pai_Intro;
    		
    		protected String _Pai_Currency;
    		
    		protected String _Pai_Platform;
    		
    		protected String _Pai_ParterID;
    		
    		protected String _Pai_Key;
    		
    		protected String _Pai_InterfaceType;
    		
    		protected Single _Pai_Feerate;
    		
    		protected String _Pai_Config;
    		
    		protected Boolean _Pai_IsEnable;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Pai_Returl;
    		
    		protected String _Pai_Scene;
    		
    		public Int32 Pai_ID {
    			get {
    				return this._Pai_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_ID, _Pai_ID, value);
    				this._Pai_ID = value;
    			}
    		}
    		
    		public String Pai_Name {
    			get {
    				return this._Pai_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_Name, _Pai_Name, value);
    				this._Pai_Name = value;
    			}
    		}
    		
    		public String Pai_Pattern {
    			get {
    				return this._Pai_Pattern;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_Pattern, _Pai_Pattern, value);
    				this._Pai_Pattern = value;
    			}
    		}
    		
    		public Int32 Pai_Tax {
    			get {
    				return this._Pai_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_Tax, _Pai_Tax, value);
    				this._Pai_Tax = value;
    			}
    		}
    		
    		public String Pai_Intro {
    			get {
    				return this._Pai_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_Intro, _Pai_Intro, value);
    				this._Pai_Intro = value;
    			}
    		}
    		
    		public String Pai_Currency {
    			get {
    				return this._Pai_Currency;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_Currency, _Pai_Currency, value);
    				this._Pai_Currency = value;
    			}
    		}
    		
    		public String Pai_Platform {
    			get {
    				return this._Pai_Platform;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_Platform, _Pai_Platform, value);
    				this._Pai_Platform = value;
    			}
    		}
    		
    		public String Pai_ParterID {
    			get {
    				return this._Pai_ParterID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_ParterID, _Pai_ParterID, value);
    				this._Pai_ParterID = value;
    			}
    		}
    		
    		public String Pai_Key {
    			get {
    				return this._Pai_Key;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_Key, _Pai_Key, value);
    				this._Pai_Key = value;
    			}
    		}
    		
    		public String Pai_InterfaceType {
    			get {
    				return this._Pai_InterfaceType;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_InterfaceType, _Pai_InterfaceType, value);
    				this._Pai_InterfaceType = value;
    			}
    		}
    		
    		public Single Pai_Feerate {
    			get {
    				return this._Pai_Feerate;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_Feerate, _Pai_Feerate, value);
    				this._Pai_Feerate = value;
    			}
    		}
    		
    		public String Pai_Config {
    			get {
    				return this._Pai_Config;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_Config, _Pai_Config, value);
    				this._Pai_Config = value;
    			}
    		}
    		
    		public Boolean Pai_IsEnable {
    			get {
    				return this._Pai_IsEnable;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_IsEnable, _Pai_IsEnable, value);
    				this._Pai_IsEnable = value;
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
    		
    		public String Pai_Returl {
    			get {
    				return this._Pai_Returl;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_Returl, _Pai_Returl, value);
    				this._Pai_Returl = value;
    			}
    		}
    		
    		public String Pai_Scene {
    			get {
    				return this._Pai_Scene;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_Scene, _Pai_Scene, value);
    				this._Pai_Scene = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<PayInterface>("PayInterface");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Pai_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pai_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pai_ID,
    					_.Pai_Name,
    					_.Pai_Pattern,
    					_.Pai_Tax,
    					_.Pai_Intro,
    					_.Pai_Currency,
    					_.Pai_Platform,
    					_.Pai_ParterID,
    					_.Pai_Key,
    					_.Pai_InterfaceType,
    					_.Pai_Feerate,
    					_.Pai_Config,
    					_.Pai_IsEnable,
    					_.Org_ID,
    					_.Pai_Returl,
    					_.Pai_Scene};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Pai_ID,
    					this._Pai_Name,
    					this._Pai_Pattern,
    					this._Pai_Tax,
    					this._Pai_Intro,
    					this._Pai_Currency,
    					this._Pai_Platform,
    					this._Pai_ParterID,
    					this._Pai_Key,
    					this._Pai_InterfaceType,
    					this._Pai_Feerate,
    					this._Pai_Config,
    					this._Pai_IsEnable,
    					this._Org_ID,
    					this._Pai_Returl,
    					this._Pai_Scene};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Pai_ID))) {
    				this._Pai_ID = reader.GetInt32(_.Pai_ID);
    			}
    			if ((false == reader.IsDBNull(_.Pai_Name))) {
    				this._Pai_Name = reader.GetString(_.Pai_Name);
    			}
    			if ((false == reader.IsDBNull(_.Pai_Pattern))) {
    				this._Pai_Pattern = reader.GetString(_.Pai_Pattern);
    			}
    			if ((false == reader.IsDBNull(_.Pai_Tax))) {
    				this._Pai_Tax = reader.GetInt32(_.Pai_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Pai_Intro))) {
    				this._Pai_Intro = reader.GetString(_.Pai_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Pai_Currency))) {
    				this._Pai_Currency = reader.GetString(_.Pai_Currency);
    			}
    			if ((false == reader.IsDBNull(_.Pai_Platform))) {
    				this._Pai_Platform = reader.GetString(_.Pai_Platform);
    			}
    			if ((false == reader.IsDBNull(_.Pai_ParterID))) {
    				this._Pai_ParterID = reader.GetString(_.Pai_ParterID);
    			}
    			if ((false == reader.IsDBNull(_.Pai_Key))) {
    				this._Pai_Key = reader.GetString(_.Pai_Key);
    			}
    			if ((false == reader.IsDBNull(_.Pai_InterfaceType))) {
    				this._Pai_InterfaceType = reader.GetString(_.Pai_InterfaceType);
    			}
    			if ((false == reader.IsDBNull(_.Pai_Feerate))) {
    				this._Pai_Feerate = reader.GetFloat(_.Pai_Feerate);
    			}
    			if ((false == reader.IsDBNull(_.Pai_Config))) {
    				this._Pai_Config = reader.GetString(_.Pai_Config);
    			}
    			if ((false == reader.IsDBNull(_.Pai_IsEnable))) {
    				this._Pai_IsEnable = reader.GetBoolean(_.Pai_IsEnable);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Pai_Returl))) {
    				this._Pai_Returl = reader.GetString(_.Pai_Returl);
    			}
    			if ((false == reader.IsDBNull(_.Pai_Scene))) {
    				this._Pai_Scene = reader.GetString(_.Pai_Scene);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(PayInterface).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<PayInterface>();
    			
    			/// <summary>
    			/// 字段名：Pai_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pai_ID = new WeiSha.Data.Field<PayInterface>("Pai_ID");
    			
    			/// <summary>
    			/// 字段名：Pai_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pai_Name = new WeiSha.Data.Field<PayInterface>("Pai_Name");
    			
    			/// <summary>
    			/// 字段名：Pai_Pattern - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pai_Pattern = new WeiSha.Data.Field<PayInterface>("Pai_Pattern");
    			
    			/// <summary>
    			/// 字段名：Pai_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pai_Tax = new WeiSha.Data.Field<PayInterface>("Pai_Tax");
    			
    			/// <summary>
    			/// 字段名：Pai_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pai_Intro = new WeiSha.Data.Field<PayInterface>("Pai_Intro");
    			
    			/// <summary>
    			/// 字段名：Pai_Currency - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pai_Currency = new WeiSha.Data.Field<PayInterface>("Pai_Currency");
    			
    			/// <summary>
    			/// 字段名：Pai_Platform - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pai_Platform = new WeiSha.Data.Field<PayInterface>("Pai_Platform");
    			
    			/// <summary>
    			/// 字段名：Pai_ParterID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pai_ParterID = new WeiSha.Data.Field<PayInterface>("Pai_ParterID");
    			
    			/// <summary>
    			/// 字段名：Pai_Key - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pai_Key = new WeiSha.Data.Field<PayInterface>("Pai_Key");
    			
    			/// <summary>
    			/// 字段名：Pai_InterfaceType - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pai_InterfaceType = new WeiSha.Data.Field<PayInterface>("Pai_InterfaceType");
    			
    			/// <summary>
    			/// 字段名：Pai_Feerate - 数据类型：Single
    			/// </summary>
    			public static WeiSha.Data.Field Pai_Feerate = new WeiSha.Data.Field<PayInterface>("Pai_Feerate");
    			
    			/// <summary>
    			/// 字段名：Pai_Config - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pai_Config = new WeiSha.Data.Field<PayInterface>("Pai_Config");
    			
    			/// <summary>
    			/// 字段名：Pai_IsEnable - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pai_IsEnable = new WeiSha.Data.Field<PayInterface>("Pai_IsEnable");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<PayInterface>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Pai_Returl - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pai_Returl = new WeiSha.Data.Field<PayInterface>("Pai_Returl");
    			
    			/// <summary>
    			/// 字段名：Pai_Scene - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pai_Scene = new WeiSha.Data.Field<PayInterface>("Pai_Scene");
    		}
    	}
    }
    