namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Logs 主键列：Log_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Logs : WeiSha.Data.Entity {
    		
    		protected Int32 _Log_Id;
    		
    		protected Int32? _Acc_Id;
    		
    		protected String _Acc_Name;
    		
    		protected String _Log_Type;
    		
    		protected DateTime? _Log_Time;
    		
    		protected String _Log_IP;
    		
    		protected String _Log_OS;
    		
    		protected String _Log_Browser;
    		
    		protected String _Log_MenuName;
    		
    		protected Int32? _Log_MenuId;
    		
    		protected String _Log_FileName;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32 Log_Id {
    			get {
    				return this._Log_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Log_Id, _Log_Id, value);
    				this._Log_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
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
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Acc_Name {
    			get {
    				return this._Acc_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Name, _Acc_Name, value);
    				this._Acc_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Log_Type {
    			get {
    				return this._Log_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Log_Type, _Log_Type, value);
    				this._Log_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public DateTime? Log_Time {
    			get {
    				return this._Log_Time;
    			}
    			set {
    				this.OnPropertyValueChange(_.Log_Time, _Log_Time, value);
    				this._Log_Time = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Log_IP {
    			get {
    				return this._Log_IP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Log_IP, _Log_IP, value);
    				this._Log_IP = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Log_OS {
    			get {
    				return this._Log_OS;
    			}
    			set {
    				this.OnPropertyValueChange(_.Log_OS, _Log_OS, value);
    				this._Log_OS = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Log_Browser {
    			get {
    				return this._Log_Browser;
    			}
    			set {
    				this.OnPropertyValueChange(_.Log_Browser, _Log_Browser, value);
    				this._Log_Browser = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Log_MenuName {
    			get {
    				return this._Log_MenuName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Log_MenuName, _Log_MenuName, value);
    				this._Log_MenuName = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Log_MenuId {
    			get {
    				return this._Log_MenuId;
    			}
    			set {
    				this.OnPropertyValueChange(_.Log_MenuId, _Log_MenuId, value);
    				this._Log_MenuId = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Log_FileName {
    			get {
    				return this._Log_FileName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Log_FileName, _Log_FileName, value);
    				this._Log_FileName = value;
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
    			return new WeiSha.Data.Table<Logs>("Logs");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Log_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Log_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Log_Id,
    					_.Acc_Id,
    					_.Acc_Name,
    					_.Log_Type,
    					_.Log_Time,
    					_.Log_IP,
    					_.Log_OS,
    					_.Log_Browser,
    					_.Log_MenuName,
    					_.Log_MenuId,
    					_.Log_FileName,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Log_Id,
    					this._Acc_Id,
    					this._Acc_Name,
    					this._Log_Type,
    					this._Log_Time,
    					this._Log_IP,
    					this._Log_OS,
    					this._Log_Browser,
    					this._Log_MenuName,
    					this._Log_MenuId,
    					this._Log_FileName,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Log_Id))) {
    				this._Log_Id = reader.GetInt32(_.Log_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Name))) {
    				this._Acc_Name = reader.GetString(_.Acc_Name);
    			}
    			if ((false == reader.IsDBNull(_.Log_Type))) {
    				this._Log_Type = reader.GetString(_.Log_Type);
    			}
    			if ((false == reader.IsDBNull(_.Log_Time))) {
    				this._Log_Time = reader.GetDateTime(_.Log_Time);
    			}
    			if ((false == reader.IsDBNull(_.Log_IP))) {
    				this._Log_IP = reader.GetString(_.Log_IP);
    			}
    			if ((false == reader.IsDBNull(_.Log_OS))) {
    				this._Log_OS = reader.GetString(_.Log_OS);
    			}
    			if ((false == reader.IsDBNull(_.Log_Browser))) {
    				this._Log_Browser = reader.GetString(_.Log_Browser);
    			}
    			if ((false == reader.IsDBNull(_.Log_MenuName))) {
    				this._Log_MenuName = reader.GetString(_.Log_MenuName);
    			}
    			if ((false == reader.IsDBNull(_.Log_MenuId))) {
    				this._Log_MenuId = reader.GetInt32(_.Log_MenuId);
    			}
    			if ((false == reader.IsDBNull(_.Log_FileName))) {
    				this._Log_FileName = reader.GetString(_.Log_FileName);
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
    			if ((false == typeof(Logs).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Logs>();
    			
    			/// <summary>
    			/// False - 字段名：Log_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Log_Id = new WeiSha.Data.Field<Logs>("Log_Id");
    			
    			/// <summary>
    			/// False - 字段名：Acc_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<Logs>("Acc_Id");
    			
    			/// <summary>
    			/// True - 字段名：Acc_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Name = new WeiSha.Data.Field<Logs>("Acc_Name");
    			
    			/// <summary>
    			/// True - 字段名：Log_Type - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Log_Type = new WeiSha.Data.Field<Logs>("Log_Type");
    			
    			/// <summary>
    			/// False - 字段名：Log_Time - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Log_Time = new WeiSha.Data.Field<Logs>("Log_Time");
    			
    			/// <summary>
    			/// True - 字段名：Log_IP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Log_IP = new WeiSha.Data.Field<Logs>("Log_IP");
    			
    			/// <summary>
    			/// True - 字段名：Log_OS - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Log_OS = new WeiSha.Data.Field<Logs>("Log_OS");
    			
    			/// <summary>
    			/// True - 字段名：Log_Browser - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Log_Browser = new WeiSha.Data.Field<Logs>("Log_Browser");
    			
    			/// <summary>
    			/// True - 字段名：Log_MenuName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Log_MenuName = new WeiSha.Data.Field<Logs>("Log_MenuName");
    			
    			/// <summary>
    			/// False - 字段名：Log_MenuId - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Log_MenuId = new WeiSha.Data.Field<Logs>("Log_MenuId");
    			
    			/// <summary>
    			/// True - 字段名：Log_FileName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Log_FileName = new WeiSha.Data.Field<Logs>("Log_FileName");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Logs>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Logs>("Org_Name");
    		}
    	}
    }
    