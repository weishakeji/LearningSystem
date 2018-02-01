namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：LogForStudentStudy 主键列：Lss_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class LogForStudentStudy : WeiSha.Data.Entity {
    		
    		protected Int32 _Lss_ID;
    		
    		protected Int32 _Org_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected String _Ac_AccName;
    		
    		protected String _Ac_Name;
    		
    		protected Int32 _Cou_ID;
    		
    		protected Int32 _Ol_ID;
    		
    		protected String _Lss_UID;
    		
    		protected DateTime _Lss_CrtTime;
    		
    		protected Int32 _Lss_PlayTime;
    		
    		protected Int32 _Lss_StudyTime;
    		
    		protected Int32 _Lss_Duration;
    		
    		protected DateTime _Lss_LastTime;
    		
    		protected String _Lss_IP;
    		
    		protected String _Lss_Browser;
    		
    		protected String _Lss_Platform;
    		
    		protected String _Lss_OS;
    		
    		public Int32 Lss_ID {
    			get {
    				return this._Lss_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lss_ID, _Lss_ID, value);
    				this._Lss_ID = value;
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
    		
    		public Int32 Ac_ID {
    			get {
    				return this._Ac_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_ID, _Ac_ID, value);
    				this._Ac_ID = value;
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
    		
    		public String Ac_Name {
    			get {
    				return this._Ac_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Name, _Ac_Name, value);
    				this._Ac_Name = value;
    			}
    		}
    		
    		public Int32 Cou_ID {
    			get {
    				return this._Cou_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ID, _Cou_ID, value);
    				this._Cou_ID = value;
    			}
    		}
    		
    		public Int32 Ol_ID {
    			get {
    				return this._Ol_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_ID, _Ol_ID, value);
    				this._Ol_ID = value;
    			}
    		}
    		
    		public String Lss_UID {
    			get {
    				return this._Lss_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lss_UID, _Lss_UID, value);
    				this._Lss_UID = value;
    			}
    		}
    		
    		public DateTime Lss_CrtTime {
    			get {
    				return this._Lss_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lss_CrtTime, _Lss_CrtTime, value);
    				this._Lss_CrtTime = value;
    			}
    		}
    		
    		public Int32 Lss_PlayTime {
    			get {
    				return this._Lss_PlayTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lss_PlayTime, _Lss_PlayTime, value);
    				this._Lss_PlayTime = value;
    			}
    		}
    		
    		public Int32 Lss_StudyTime {
    			get {
    				return this._Lss_StudyTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lss_StudyTime, _Lss_StudyTime, value);
    				this._Lss_StudyTime = value;
    			}
    		}
    		
    		public Int32 Lss_Duration {
    			get {
    				return this._Lss_Duration;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lss_Duration, _Lss_Duration, value);
    				this._Lss_Duration = value;
    			}
    		}
    		
    		public DateTime Lss_LastTime {
    			get {
    				return this._Lss_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lss_LastTime, _Lss_LastTime, value);
    				this._Lss_LastTime = value;
    			}
    		}
    		
    		public String Lss_IP {
    			get {
    				return this._Lss_IP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lss_IP, _Lss_IP, value);
    				this._Lss_IP = value;
    			}
    		}
    		
    		public String Lss_Browser {
    			get {
    				return this._Lss_Browser;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lss_Browser, _Lss_Browser, value);
    				this._Lss_Browser = value;
    			}
    		}
    		
    		public String Lss_Platform {
    			get {
    				return this._Lss_Platform;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lss_Platform, _Lss_Platform, value);
    				this._Lss_Platform = value;
    			}
    		}
    		
    		public String Lss_OS {
    			get {
    				return this._Lss_OS;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lss_OS, _Lss_OS, value);
    				this._Lss_OS = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<LogForStudentStudy>("LogForStudentStudy");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Lss_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lss_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lss_ID,
    					_.Org_ID,
    					_.Ac_ID,
    					_.Ac_AccName,
    					_.Ac_Name,
    					_.Cou_ID,
    					_.Ol_ID,
    					_.Lss_UID,
    					_.Lss_CrtTime,
    					_.Lss_PlayTime,
    					_.Lss_StudyTime,
    					_.Lss_Duration,
    					_.Lss_LastTime,
    					_.Lss_IP,
    					_.Lss_Browser,
    					_.Lss_Platform,
    					_.Lss_OS};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Lss_ID,
    					this._Org_ID,
    					this._Ac_ID,
    					this._Ac_AccName,
    					this._Ac_Name,
    					this._Cou_ID,
    					this._Ol_ID,
    					this._Lss_UID,
    					this._Lss_CrtTime,
    					this._Lss_PlayTime,
    					this._Lss_StudyTime,
    					this._Lss_Duration,
    					this._Lss_LastTime,
    					this._Lss_IP,
    					this._Lss_Browser,
    					this._Lss_Platform,
    					this._Lss_OS};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Lss_ID))) {
    				this._Lss_ID = reader.GetInt32(_.Lss_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_AccName))) {
    				this._Ac_AccName = reader.GetString(_.Ac_AccName);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Name))) {
    				this._Ac_Name = reader.GetString(_.Ac_Name);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ol_ID))) {
    				this._Ol_ID = reader.GetInt32(_.Ol_ID);
    			}
    			if ((false == reader.IsDBNull(_.Lss_UID))) {
    				this._Lss_UID = reader.GetString(_.Lss_UID);
    			}
    			if ((false == reader.IsDBNull(_.Lss_CrtTime))) {
    				this._Lss_CrtTime = reader.GetDateTime(_.Lss_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Lss_PlayTime))) {
    				this._Lss_PlayTime = reader.GetInt32(_.Lss_PlayTime);
    			}
    			if ((false == reader.IsDBNull(_.Lss_StudyTime))) {
    				this._Lss_StudyTime = reader.GetInt32(_.Lss_StudyTime);
    			}
    			if ((false == reader.IsDBNull(_.Lss_Duration))) {
    				this._Lss_Duration = reader.GetInt32(_.Lss_Duration);
    			}
    			if ((false == reader.IsDBNull(_.Lss_LastTime))) {
    				this._Lss_LastTime = reader.GetDateTime(_.Lss_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Lss_IP))) {
    				this._Lss_IP = reader.GetString(_.Lss_IP);
    			}
    			if ((false == reader.IsDBNull(_.Lss_Browser))) {
    				this._Lss_Browser = reader.GetString(_.Lss_Browser);
    			}
    			if ((false == reader.IsDBNull(_.Lss_Platform))) {
    				this._Lss_Platform = reader.GetString(_.Lss_Platform);
    			}
    			if ((false == reader.IsDBNull(_.Lss_OS))) {
    				this._Lss_OS = reader.GetString(_.Lss_OS);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(LogForStudentStudy).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<LogForStudentStudy>();
    			
    			/// <summary>
    			/// 字段名：Lss_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lss_ID = new WeiSha.Data.Field<LogForStudentStudy>("Lss_ID");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<LogForStudentStudy>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<LogForStudentStudy>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_AccName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_AccName = new WeiSha.Data.Field<LogForStudentStudy>("Ac_AccName");
    			
    			/// <summary>
    			/// 字段名：Ac_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Name = new WeiSha.Data.Field<LogForStudentStudy>("Ac_Name");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<LogForStudentStudy>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Ol_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ol_ID = new WeiSha.Data.Field<LogForStudentStudy>("Ol_ID");
    			
    			/// <summary>
    			/// 字段名：Lss_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lss_UID = new WeiSha.Data.Field<LogForStudentStudy>("Lss_UID");
    			
    			/// <summary>
    			/// 字段名：Lss_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lss_CrtTime = new WeiSha.Data.Field<LogForStudentStudy>("Lss_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Lss_PlayTime - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lss_PlayTime = new WeiSha.Data.Field<LogForStudentStudy>("Lss_PlayTime");
    			
    			/// <summary>
    			/// 字段名：Lss_StudyTime - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lss_StudyTime = new WeiSha.Data.Field<LogForStudentStudy>("Lss_StudyTime");
    			
    			/// <summary>
    			/// 字段名：Lss_Duration - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lss_Duration = new WeiSha.Data.Field<LogForStudentStudy>("Lss_Duration");
    			
    			/// <summary>
    			/// 字段名：Lss_LastTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lss_LastTime = new WeiSha.Data.Field<LogForStudentStudy>("Lss_LastTime");
    			
    			/// <summary>
    			/// 字段名：Lss_IP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lss_IP = new WeiSha.Data.Field<LogForStudentStudy>("Lss_IP");
    			
    			/// <summary>
    			/// 字段名：Lss_Browser - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lss_Browser = new WeiSha.Data.Field<LogForStudentStudy>("Lss_Browser");
    			
    			/// <summary>
    			/// 字段名：Lss_Platform - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lss_Platform = new WeiSha.Data.Field<LogForStudentStudy>("Lss_Platform");
    			
    			/// <summary>
    			/// 字段名：Lss_OS - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lss_OS = new WeiSha.Data.Field<LogForStudentStudy>("Lss_OS");
    		}
    	}
    }
    