namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：LogForStudentOnline 主键列：Lso_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class LogForStudentOnline : WeiSha.Data.Entity {
    		
    		protected Int32 _Lso_ID;
    		
    		protected String _Ac_AccName;
    		
    		protected Int32 _Ac_ID;
    		
    		protected String _Ac_Name;
    		
    		protected String _Lso_Address;
    		
    		protected Int32 _Lso_BrowseTime;
    		
    		protected String _Lso_Browser;
    		
    		protected String _Lso_City;
    		
    		protected Int32 _Lso_Code;
    		
    		protected DateTime _Lso_CrtTime;
    		
    		protected String _Lso_District;
    		
    		protected Int32 _Lso_GeogType;
    		
    		protected String _Lso_IP;
    		
    		protected String _Lso_Info;
    		
    		protected DateTime _Lso_LastTime;
    		
    		protected Decimal _Lso_Latitude;
    		
    		protected DateTime _Lso_LoginDate;
    		
    		protected DateTime _Lso_LoginTime;
    		
    		protected DateTime _Lso_LogoutTime;
    		
    		protected Decimal _Lso_Longitude;
    		
    		protected String _Lso_OS;
    		
    		protected Int32 _Lso_OnlineTime;
    		
    		protected String _Lso_Platform;
    		
    		protected String _Lso_Province;
    		
    		protected String _Lso_Source;
    		
    		protected String _Lso_UID;
    		
    		protected Int32 _Org_ID;
    		
    		public Int32 Lso_ID {
    			get {
    				return this._Lso_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_ID, _Lso_ID, value);
    				this._Lso_ID = value;
    			}
    		}
    		
    		public String Ac_AccName {
    			get {
    				return this._Ac_AccName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_AccName, _Ac_AccName, value);
    				this._Ac_AccName = value;
    			}
    		}
    		
    		public Int32 Ac_ID {
    			get {
    				return this._Ac_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_ID, _Ac_ID, value);
    				this._Ac_ID = value;
    			}
    		}
    		
    		public String Ac_Name {
    			get {
    				return this._Ac_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Name, _Ac_Name, value);
    				this._Ac_Name = value;
    			}
    		}
    		
    		public String Lso_Address {
    			get {
    				return this._Lso_Address;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_Address, _Lso_Address, value);
    				this._Lso_Address = value;
    			}
    		}
    		
    		public Int32 Lso_BrowseTime {
    			get {
    				return this._Lso_BrowseTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_BrowseTime, _Lso_BrowseTime, value);
    				this._Lso_BrowseTime = value;
    			}
    		}
    		
    		public String Lso_Browser {
    			get {
    				return this._Lso_Browser;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_Browser, _Lso_Browser, value);
    				this._Lso_Browser = value;
    			}
    		}
    		
    		public String Lso_City {
    			get {
    				return this._Lso_City;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_City, _Lso_City, value);
    				this._Lso_City = value;
    			}
    		}
    		
    		public Int32 Lso_Code {
    			get {
    				return this._Lso_Code;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_Code, _Lso_Code, value);
    				this._Lso_Code = value;
    			}
    		}
    		
    		public DateTime Lso_CrtTime {
    			get {
    				return this._Lso_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_CrtTime, _Lso_CrtTime, value);
    				this._Lso_CrtTime = value;
    			}
    		}
    		
    		public String Lso_District {
    			get {
    				return this._Lso_District;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_District, _Lso_District, value);
    				this._Lso_District = value;
    			}
    		}
    		
    		public Int32 Lso_GeogType {
    			get {
    				return this._Lso_GeogType;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_GeogType, _Lso_GeogType, value);
    				this._Lso_GeogType = value;
    			}
    		}
    		
    		public String Lso_IP {
    			get {
    				return this._Lso_IP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_IP, _Lso_IP, value);
    				this._Lso_IP = value;
    			}
    		}
    		
    		public String Lso_Info {
    			get {
    				return this._Lso_Info;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_Info, _Lso_Info, value);
    				this._Lso_Info = value;
    			}
    		}
    		
    		public DateTime Lso_LastTime {
    			get {
    				return this._Lso_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_LastTime, _Lso_LastTime, value);
    				this._Lso_LastTime = value;
    			}
    		}
    		
    		public Decimal Lso_Latitude {
    			get {
    				return this._Lso_Latitude;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_Latitude, _Lso_Latitude, value);
    				this._Lso_Latitude = value;
    			}
    		}
    		
    		public DateTime Lso_LoginDate {
    			get {
    				return this._Lso_LoginDate;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_LoginDate, _Lso_LoginDate, value);
    				this._Lso_LoginDate = value;
    			}
    		}
    		
    		public DateTime Lso_LoginTime {
    			get {
    				return this._Lso_LoginTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_LoginTime, _Lso_LoginTime, value);
    				this._Lso_LoginTime = value;
    			}
    		}
    		
    		public DateTime Lso_LogoutTime {
    			get {
    				return this._Lso_LogoutTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_LogoutTime, _Lso_LogoutTime, value);
    				this._Lso_LogoutTime = value;
    			}
    		}
    		
    		public Decimal Lso_Longitude {
    			get {
    				return this._Lso_Longitude;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_Longitude, _Lso_Longitude, value);
    				this._Lso_Longitude = value;
    			}
    		}
    		
    		public String Lso_OS {
    			get {
    				return this._Lso_OS;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_OS, _Lso_OS, value);
    				this._Lso_OS = value;
    			}
    		}
    		
    		public Int32 Lso_OnlineTime {
    			get {
    				return this._Lso_OnlineTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_OnlineTime, _Lso_OnlineTime, value);
    				this._Lso_OnlineTime = value;
    			}
    		}
    		
    		public String Lso_Platform {
    			get {
    				return this._Lso_Platform;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_Platform, _Lso_Platform, value);
    				this._Lso_Platform = value;
    			}
    		}
    		
    		public String Lso_Province {
    			get {
    				return this._Lso_Province;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_Province, _Lso_Province, value);
    				this._Lso_Province = value;
    			}
    		}
    		
    		public String Lso_Source {
    			get {
    				return this._Lso_Source;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_Source, _Lso_Source, value);
    				this._Lso_Source = value;
    			}
    		}
    		
    		public String Lso_UID {
    			get {
    				return this._Lso_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lso_UID, _Lso_UID, value);
    				this._Lso_UID = value;
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
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<LogForStudentOnline>("LogForStudentOnline");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Lso_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lso_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lso_ID,
    					_.Ac_AccName,
    					_.Ac_ID,
    					_.Ac_Name,
    					_.Lso_Address,
    					_.Lso_BrowseTime,
    					_.Lso_Browser,
    					_.Lso_City,
    					_.Lso_Code,
    					_.Lso_CrtTime,
    					_.Lso_District,
    					_.Lso_GeogType,
    					_.Lso_IP,
    					_.Lso_Info,
    					_.Lso_LastTime,
    					_.Lso_Latitude,
    					_.Lso_LoginDate,
    					_.Lso_LoginTime,
    					_.Lso_LogoutTime,
    					_.Lso_Longitude,
    					_.Lso_OS,
    					_.Lso_OnlineTime,
    					_.Lso_Platform,
    					_.Lso_Province,
    					_.Lso_Source,
    					_.Lso_UID,
    					_.Org_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Lso_ID,
    					this._Ac_AccName,
    					this._Ac_ID,
    					this._Ac_Name,
    					this._Lso_Address,
    					this._Lso_BrowseTime,
    					this._Lso_Browser,
    					this._Lso_City,
    					this._Lso_Code,
    					this._Lso_CrtTime,
    					this._Lso_District,
    					this._Lso_GeogType,
    					this._Lso_IP,
    					this._Lso_Info,
    					this._Lso_LastTime,
    					this._Lso_Latitude,
    					this._Lso_LoginDate,
    					this._Lso_LoginTime,
    					this._Lso_LogoutTime,
    					this._Lso_Longitude,
    					this._Lso_OS,
    					this._Lso_OnlineTime,
    					this._Lso_Platform,
    					this._Lso_Province,
    					this._Lso_Source,
    					this._Lso_UID,
    					this._Org_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Lso_ID))) {
    				this._Lso_ID = reader.GetInt32(_.Lso_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_AccName))) {
    				this._Ac_AccName = reader.GetString(_.Ac_AccName);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Name))) {
    				this._Ac_Name = reader.GetString(_.Ac_Name);
    			}
    			if ((false == reader.IsDBNull(_.Lso_Address))) {
    				this._Lso_Address = reader.GetString(_.Lso_Address);
    			}
    			if ((false == reader.IsDBNull(_.Lso_BrowseTime))) {
    				this._Lso_BrowseTime = reader.GetInt32(_.Lso_BrowseTime);
    			}
    			if ((false == reader.IsDBNull(_.Lso_Browser))) {
    				this._Lso_Browser = reader.GetString(_.Lso_Browser);
    			}
    			if ((false == reader.IsDBNull(_.Lso_City))) {
    				this._Lso_City = reader.GetString(_.Lso_City);
    			}
    			if ((false == reader.IsDBNull(_.Lso_Code))) {
    				this._Lso_Code = reader.GetInt32(_.Lso_Code);
    			}
    			if ((false == reader.IsDBNull(_.Lso_CrtTime))) {
    				this._Lso_CrtTime = reader.GetDateTime(_.Lso_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Lso_District))) {
    				this._Lso_District = reader.GetString(_.Lso_District);
    			}
    			if ((false == reader.IsDBNull(_.Lso_GeogType))) {
    				this._Lso_GeogType = reader.GetInt32(_.Lso_GeogType);
    			}
    			if ((false == reader.IsDBNull(_.Lso_IP))) {
    				this._Lso_IP = reader.GetString(_.Lso_IP);
    			}
    			if ((false == reader.IsDBNull(_.Lso_Info))) {
    				this._Lso_Info = reader.GetString(_.Lso_Info);
    			}
    			if ((false == reader.IsDBNull(_.Lso_LastTime))) {
    				this._Lso_LastTime = reader.GetDateTime(_.Lso_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Lso_Latitude))) {
    				this._Lso_Latitude = reader.GetDecimal(_.Lso_Latitude);
    			}
    			if ((false == reader.IsDBNull(_.Lso_LoginDate))) {
    				this._Lso_LoginDate = reader.GetDateTime(_.Lso_LoginDate);
    			}
    			if ((false == reader.IsDBNull(_.Lso_LoginTime))) {
    				this._Lso_LoginTime = reader.GetDateTime(_.Lso_LoginTime);
    			}
    			if ((false == reader.IsDBNull(_.Lso_LogoutTime))) {
    				this._Lso_LogoutTime = reader.GetDateTime(_.Lso_LogoutTime);
    			}
    			if ((false == reader.IsDBNull(_.Lso_Longitude))) {
    				this._Lso_Longitude = reader.GetDecimal(_.Lso_Longitude);
    			}
    			if ((false == reader.IsDBNull(_.Lso_OS))) {
    				this._Lso_OS = reader.GetString(_.Lso_OS);
    			}
    			if ((false == reader.IsDBNull(_.Lso_OnlineTime))) {
    				this._Lso_OnlineTime = reader.GetInt32(_.Lso_OnlineTime);
    			}
    			if ((false == reader.IsDBNull(_.Lso_Platform))) {
    				this._Lso_Platform = reader.GetString(_.Lso_Platform);
    			}
    			if ((false == reader.IsDBNull(_.Lso_Province))) {
    				this._Lso_Province = reader.GetString(_.Lso_Province);
    			}
    			if ((false == reader.IsDBNull(_.Lso_Source))) {
    				this._Lso_Source = reader.GetString(_.Lso_Source);
    			}
    			if ((false == reader.IsDBNull(_.Lso_UID))) {
    				this._Lso_UID = reader.GetString(_.Lso_UID);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(LogForStudentOnline).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<LogForStudentOnline>();
    			
    			/// <summary>
    			/// 字段名：Lso_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lso_ID = new WeiSha.Data.Field<LogForStudentOnline>("Lso_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_AccName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_AccName = new WeiSha.Data.Field<LogForStudentOnline>("Ac_AccName");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<LogForStudentOnline>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Name = new WeiSha.Data.Field<LogForStudentOnline>("Ac_Name");
    			
    			/// <summary>
    			/// 字段名：Lso_Address - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lso_Address = new WeiSha.Data.Field<LogForStudentOnline>("Lso_Address");
    			
    			/// <summary>
    			/// 字段名：Lso_BrowseTime - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lso_BrowseTime = new WeiSha.Data.Field<LogForStudentOnline>("Lso_BrowseTime");
    			
    			/// <summary>
    			/// 字段名：Lso_Browser - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lso_Browser = new WeiSha.Data.Field<LogForStudentOnline>("Lso_Browser");
    			
    			/// <summary>
    			/// 字段名：Lso_City - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lso_City = new WeiSha.Data.Field<LogForStudentOnline>("Lso_City");
    			
    			/// <summary>
    			/// 字段名：Lso_Code - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lso_Code = new WeiSha.Data.Field<LogForStudentOnline>("Lso_Code");
    			
    			/// <summary>
    			/// 字段名：Lso_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lso_CrtTime = new WeiSha.Data.Field<LogForStudentOnline>("Lso_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Lso_District - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lso_District = new WeiSha.Data.Field<LogForStudentOnline>("Lso_District");
    			
    			/// <summary>
    			/// 字段名：Lso_GeogType - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lso_GeogType = new WeiSha.Data.Field<LogForStudentOnline>("Lso_GeogType");
    			
    			/// <summary>
    			/// 字段名：Lso_IP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lso_IP = new WeiSha.Data.Field<LogForStudentOnline>("Lso_IP");
    			
    			/// <summary>
    			/// 字段名：Lso_Info - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lso_Info = new WeiSha.Data.Field<LogForStudentOnline>("Lso_Info");
    			
    			/// <summary>
    			/// 字段名：Lso_LastTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lso_LastTime = new WeiSha.Data.Field<LogForStudentOnline>("Lso_LastTime");
    			
    			/// <summary>
    			/// 字段名：Lso_Latitude - 数据类型：Decimal
    			/// </summary>
    			public static WeiSha.Data.Field Lso_Latitude = new WeiSha.Data.Field<LogForStudentOnline>("Lso_Latitude");
    			
    			/// <summary>
    			/// 字段名：Lso_LoginDate - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lso_LoginDate = new WeiSha.Data.Field<LogForStudentOnline>("Lso_LoginDate");
    			
    			/// <summary>
    			/// 字段名：Lso_LoginTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lso_LoginTime = new WeiSha.Data.Field<LogForStudentOnline>("Lso_LoginTime");
    			
    			/// <summary>
    			/// 字段名：Lso_LogoutTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lso_LogoutTime = new WeiSha.Data.Field<LogForStudentOnline>("Lso_LogoutTime");
    			
    			/// <summary>
    			/// 字段名：Lso_Longitude - 数据类型：Decimal
    			/// </summary>
    			public static WeiSha.Data.Field Lso_Longitude = new WeiSha.Data.Field<LogForStudentOnline>("Lso_Longitude");
    			
    			/// <summary>
    			/// 字段名：Lso_OS - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lso_OS = new WeiSha.Data.Field<LogForStudentOnline>("Lso_OS");
    			
    			/// <summary>
    			/// 字段名：Lso_OnlineTime - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lso_OnlineTime = new WeiSha.Data.Field<LogForStudentOnline>("Lso_OnlineTime");
    			
    			/// <summary>
    			/// 字段名：Lso_Platform - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lso_Platform = new WeiSha.Data.Field<LogForStudentOnline>("Lso_Platform");
    			
    			/// <summary>
    			/// 字段名：Lso_Province - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lso_Province = new WeiSha.Data.Field<LogForStudentOnline>("Lso_Province");
    			
    			/// <summary>
    			/// 字段名：Lso_Source - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lso_Source = new WeiSha.Data.Field<LogForStudentOnline>("Lso_Source");
    			
    			/// <summary>
    			/// 字段名：Lso_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lso_UID = new WeiSha.Data.Field<LogForStudentOnline>("Lso_UID");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<LogForStudentOnline>("Org_ID");
    		}
    	}
    }
    