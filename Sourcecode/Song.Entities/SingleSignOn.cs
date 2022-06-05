namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：SingleSignOn 主键列：SSO_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class SingleSignOn : WeiSha.Data.Entity {
    		
    		protected Int32 _SSO_ID;
    		
    		protected String _SSO_Name;
    		
    		protected Boolean? _SSO_IsUse;
    		
    		protected String _SSO_APPID;
    		
    		protected Boolean _SSO_IsAdd;
    		
    		protected String _SSO_Domain;
    		
    		protected String _SSO_Direction;
    		
    		protected String _SSO_Phone;
    		
    		protected String _SSO_Email;
    		
    		protected String _SSO_Info;
    		
    		protected DateTime _SSO_CrtTime;
    		
    		protected String _SSO_Power;
    		
    		protected String _SSO_Config;
    		
    		public Int32 SSO_ID {
    			get {
    				return this._SSO_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.SSO_ID, _SSO_ID, value);
    				this._SSO_ID = value;
    			}
    		}
    		
    		public String SSO_Name {
    			get {
    				return this._SSO_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.SSO_Name, _SSO_Name, value);
    				this._SSO_Name = value;
    			}
    		}
    		
    		public Boolean? SSO_IsUse {
    			get {
    				return this._SSO_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.SSO_IsUse, _SSO_IsUse, value);
    				this._SSO_IsUse = value;
    			}
    		}
    		
    		public String SSO_APPID {
    			get {
    				return this._SSO_APPID;
    			}
    			set {
    				this.OnPropertyValueChange(_.SSO_APPID, _SSO_APPID, value);
    				this._SSO_APPID = value;
    			}
    		}
    		
    		public Boolean SSO_IsAdd {
    			get {
    				return this._SSO_IsAdd;
    			}
    			set {
    				this.OnPropertyValueChange(_.SSO_IsAdd, _SSO_IsAdd, value);
    				this._SSO_IsAdd = value;
    			}
    		}
    		
    		public String SSO_Domain {
    			get {
    				return this._SSO_Domain;
    			}
    			set {
    				this.OnPropertyValueChange(_.SSO_Domain, _SSO_Domain, value);
    				this._SSO_Domain = value;
    			}
    		}
    		
    		public String SSO_Direction {
    			get {
    				return this._SSO_Direction;
    			}
    			set {
    				this.OnPropertyValueChange(_.SSO_Direction, _SSO_Direction, value);
    				this._SSO_Direction = value;
    			}
    		}
    		
    		public String SSO_Phone {
    			get {
    				return this._SSO_Phone;
    			}
    			set {
    				this.OnPropertyValueChange(_.SSO_Phone, _SSO_Phone, value);
    				this._SSO_Phone = value;
    			}
    		}
    		
    		public String SSO_Email {
    			get {
    				return this._SSO_Email;
    			}
    			set {
    				this.OnPropertyValueChange(_.SSO_Email, _SSO_Email, value);
    				this._SSO_Email = value;
    			}
    		}
    		
    		public String SSO_Info {
    			get {
    				return this._SSO_Info;
    			}
    			set {
    				this.OnPropertyValueChange(_.SSO_Info, _SSO_Info, value);
    				this._SSO_Info = value;
    			}
    		}
    		
    		public DateTime SSO_CrtTime {
    			get {
    				return this._SSO_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.SSO_CrtTime, _SSO_CrtTime, value);
    				this._SSO_CrtTime = value;
    			}
    		}
    		
    		public String SSO_Power {
    			get {
    				return this._SSO_Power;
    			}
    			set {
    				this.OnPropertyValueChange(_.SSO_Power, _SSO_Power, value);
    				this._SSO_Power = value;
    			}
    		}
    		
    		public String SSO_Config {
    			get {
    				return this._SSO_Config;
    			}
    			set {
    				this.OnPropertyValueChange(_.SSO_Config, _SSO_Config, value);
    				this._SSO_Config = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<SingleSignOn>("SingleSignOn");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.SSO_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.SSO_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.SSO_ID,
    					_.SSO_Name,
    					_.SSO_IsUse,
    					_.SSO_APPID,
    					_.SSO_IsAdd,
    					_.SSO_Domain,
    					_.SSO_Direction,
    					_.SSO_Phone,
    					_.SSO_Email,
    					_.SSO_Info,
    					_.SSO_CrtTime,
    					_.SSO_Power,
    					_.SSO_Config};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._SSO_ID,
    					this._SSO_Name,
    					this._SSO_IsUse,
    					this._SSO_APPID,
    					this._SSO_IsAdd,
    					this._SSO_Domain,
    					this._SSO_Direction,
    					this._SSO_Phone,
    					this._SSO_Email,
    					this._SSO_Info,
    					this._SSO_CrtTime,
    					this._SSO_Power,
    					this._SSO_Config};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.SSO_ID))) {
    				this._SSO_ID = reader.GetInt32(_.SSO_ID);
    			}
    			if ((false == reader.IsDBNull(_.SSO_Name))) {
    				this._SSO_Name = reader.GetString(_.SSO_Name);
    			}
    			if ((false == reader.IsDBNull(_.SSO_IsUse))) {
    				this._SSO_IsUse = reader.GetBoolean(_.SSO_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.SSO_APPID))) {
    				this._SSO_APPID = reader.GetString(_.SSO_APPID);
    			}
    			if ((false == reader.IsDBNull(_.SSO_IsAdd))) {
    				this._SSO_IsAdd = reader.GetBoolean(_.SSO_IsAdd);
    			}
    			if ((false == reader.IsDBNull(_.SSO_Domain))) {
    				this._SSO_Domain = reader.GetString(_.SSO_Domain);
    			}
    			if ((false == reader.IsDBNull(_.SSO_Direction))) {
    				this._SSO_Direction = reader.GetString(_.SSO_Direction);
    			}
    			if ((false == reader.IsDBNull(_.SSO_Phone))) {
    				this._SSO_Phone = reader.GetString(_.SSO_Phone);
    			}
    			if ((false == reader.IsDBNull(_.SSO_Email))) {
    				this._SSO_Email = reader.GetString(_.SSO_Email);
    			}
    			if ((false == reader.IsDBNull(_.SSO_Info))) {
    				this._SSO_Info = reader.GetString(_.SSO_Info);
    			}
    			if ((false == reader.IsDBNull(_.SSO_CrtTime))) {
    				this._SSO_CrtTime = reader.GetDateTime(_.SSO_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.SSO_Power))) {
    				this._SSO_Power = reader.GetString(_.SSO_Power);
    			}
    			if ((false == reader.IsDBNull(_.SSO_Config))) {
    				this._SSO_Config = reader.GetString(_.SSO_Config);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(SingleSignOn).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<SingleSignOn>();
    			
    			/// <summary>
    			/// 字段名：SSO_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field SSO_ID = new WeiSha.Data.Field<SingleSignOn>("SSO_ID");
    			
    			/// <summary>
    			/// 字段名：SSO_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field SSO_Name = new WeiSha.Data.Field<SingleSignOn>("SSO_Name");
    			
    			/// <summary>
    			/// 字段名：SSO_IsUse - 数据类型：Boolean(可空)
    			/// </summary>
    			public static WeiSha.Data.Field SSO_IsUse = new WeiSha.Data.Field<SingleSignOn>("SSO_IsUse");
    			
    			/// <summary>
    			/// 字段名：SSO_APPID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field SSO_APPID = new WeiSha.Data.Field<SingleSignOn>("SSO_APPID");
    			
    			/// <summary>
    			/// 字段名：SSO_IsAdd - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field SSO_IsAdd = new WeiSha.Data.Field<SingleSignOn>("SSO_IsAdd");
    			
    			/// <summary>
    			/// 字段名：SSO_Domain - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field SSO_Domain = new WeiSha.Data.Field<SingleSignOn>("SSO_Domain");
    			
    			/// <summary>
    			/// 字段名：SSO_Direction - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field SSO_Direction = new WeiSha.Data.Field<SingleSignOn>("SSO_Direction");
    			
    			/// <summary>
    			/// 字段名：SSO_Phone - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field SSO_Phone = new WeiSha.Data.Field<SingleSignOn>("SSO_Phone");
    			
    			/// <summary>
    			/// 字段名：SSO_Email - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field SSO_Email = new WeiSha.Data.Field<SingleSignOn>("SSO_Email");
    			
    			/// <summary>
    			/// 字段名：SSO_Info - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field SSO_Info = new WeiSha.Data.Field<SingleSignOn>("SSO_Info");
    			
    			/// <summary>
    			/// 字段名：SSO_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field SSO_CrtTime = new WeiSha.Data.Field<SingleSignOn>("SSO_CrtTime");
    			
    			/// <summary>
    			/// 字段名：SSO_Power - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field SSO_Power = new WeiSha.Data.Field<SingleSignOn>("SSO_Power");
    			
    			/// <summary>
    			/// 字段名：SSO_Config - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field SSO_Config = new WeiSha.Data.Field<SingleSignOn>("SSO_Config");
    		}
    	}
    }
    