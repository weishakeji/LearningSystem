namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Task 主键列：Task_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Task : WeiSha.Data.Entity {
    		
    		protected Int32 _Task_Id;
    		
    		protected String _Task_Name;
    		
    		protected String _Task_Context;
    		
    		protected DateTime? _Task_CrtTime;
    		
    		protected Int32? _Acc_Id;
    		
    		protected String _Acc_Name;
    		
    		protected DateTime? _Task_StartTime;
    		
    		protected DateTime? _Task_EndTime;
    		
    		protected String _Task_State;
    		
    		protected Boolean _Task_IsComplete;
    		
    		protected DateTime? _Task_CompleteTime;
    		
    		protected Int32? _Task_Level;
    		
    		protected Int32? _Task_Tax;
    		
    		protected Int32? _Task_WorkerId;
    		
    		protected String _Task_WorkerName;
    		
    		protected Boolean _Task_IsUse;
    		
    		protected String _Task_WorkLog;
    		
    		protected Int32? _Task_CompletePer;
    		
    		protected Boolean _Task_IsGoback;
    		
    		protected String _Task_GobackText;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Task_Id {
    			get {
    				return this._Task_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_Id, _Task_Id, value);
    				this._Task_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Task_Name {
    			get {
    				return this._Task_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_Name, _Task_Name, value);
    				this._Task_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Task_Context {
    			get {
    				return this._Task_Context;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_Context, _Task_Context, value);
    				this._Task_Context = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Task_CrtTime {
    			get {
    				return this._Task_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_CrtTime, _Task_CrtTime, value);
    				this._Task_CrtTime = value;
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
    		
    		/// <summary>
    		/// -1
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
    		/// -1
    		/// </summary>
    		public DateTime? Task_StartTime {
    			get {
    				return this._Task_StartTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_StartTime, _Task_StartTime, value);
    				this._Task_StartTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Task_EndTime {
    			get {
    				return this._Task_EndTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_EndTime, _Task_EndTime, value);
    				this._Task_EndTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Task_State {
    			get {
    				return this._Task_State;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_State, _Task_State, value);
    				this._Task_State = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Task_IsComplete {
    			get {
    				return this._Task_IsComplete;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_IsComplete, _Task_IsComplete, value);
    				this._Task_IsComplete = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Task_CompleteTime {
    			get {
    				return this._Task_CompleteTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_CompleteTime, _Task_CompleteTime, value);
    				this._Task_CompleteTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Task_Level {
    			get {
    				return this._Task_Level;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_Level, _Task_Level, value);
    				this._Task_Level = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Task_Tax {
    			get {
    				return this._Task_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_Tax, _Task_Tax, value);
    				this._Task_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Task_WorkerId {
    			get {
    				return this._Task_WorkerId;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_WorkerId, _Task_WorkerId, value);
    				this._Task_WorkerId = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Task_WorkerName {
    			get {
    				return this._Task_WorkerName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_WorkerName, _Task_WorkerName, value);
    				this._Task_WorkerName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Task_IsUse {
    			get {
    				return this._Task_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_IsUse, _Task_IsUse, value);
    				this._Task_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Task_WorkLog {
    			get {
    				return this._Task_WorkLog;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_WorkLog, _Task_WorkLog, value);
    				this._Task_WorkLog = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Task_CompletePer {
    			get {
    				return this._Task_CompletePer;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_CompletePer, _Task_CompletePer, value);
    				this._Task_CompletePer = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Task_IsGoback {
    			get {
    				return this._Task_IsGoback;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_IsGoback, _Task_IsGoback, value);
    				this._Task_IsGoback = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Task_GobackText {
    			get {
    				return this._Task_GobackText;
    			}
    			set {
    				this.OnPropertyValueChange(_.Task_GobackText, _Task_GobackText, value);
    				this._Task_GobackText = value;
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
    			return new WeiSha.Data.Table<Task>("Task");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Task_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Task_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Task_Id,
    					_.Task_Name,
    					_.Task_Context,
    					_.Task_CrtTime,
    					_.Acc_Id,
    					_.Acc_Name,
    					_.Task_StartTime,
    					_.Task_EndTime,
    					_.Task_State,
    					_.Task_IsComplete,
    					_.Task_CompleteTime,
    					_.Task_Level,
    					_.Task_Tax,
    					_.Task_WorkerId,
    					_.Task_WorkerName,
    					_.Task_IsUse,
    					_.Task_WorkLog,
    					_.Task_CompletePer,
    					_.Task_IsGoback,
    					_.Task_GobackText,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Task_Id,
    					this._Task_Name,
    					this._Task_Context,
    					this._Task_CrtTime,
    					this._Acc_Id,
    					this._Acc_Name,
    					this._Task_StartTime,
    					this._Task_EndTime,
    					this._Task_State,
    					this._Task_IsComplete,
    					this._Task_CompleteTime,
    					this._Task_Level,
    					this._Task_Tax,
    					this._Task_WorkerId,
    					this._Task_WorkerName,
    					this._Task_IsUse,
    					this._Task_WorkLog,
    					this._Task_CompletePer,
    					this._Task_IsGoback,
    					this._Task_GobackText,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Task_Id))) {
    				this._Task_Id = reader.GetInt32(_.Task_Id);
    			}
    			if ((false == reader.IsDBNull(_.Task_Name))) {
    				this._Task_Name = reader.GetString(_.Task_Name);
    			}
    			if ((false == reader.IsDBNull(_.Task_Context))) {
    				this._Task_Context = reader.GetString(_.Task_Context);
    			}
    			if ((false == reader.IsDBNull(_.Task_CrtTime))) {
    				this._Task_CrtTime = reader.GetDateTime(_.Task_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Name))) {
    				this._Acc_Name = reader.GetString(_.Acc_Name);
    			}
    			if ((false == reader.IsDBNull(_.Task_StartTime))) {
    				this._Task_StartTime = reader.GetDateTime(_.Task_StartTime);
    			}
    			if ((false == reader.IsDBNull(_.Task_EndTime))) {
    				this._Task_EndTime = reader.GetDateTime(_.Task_EndTime);
    			}
    			if ((false == reader.IsDBNull(_.Task_State))) {
    				this._Task_State = reader.GetString(_.Task_State);
    			}
    			if ((false == reader.IsDBNull(_.Task_IsComplete))) {
    				this._Task_IsComplete = reader.GetBoolean(_.Task_IsComplete);
    			}
    			if ((false == reader.IsDBNull(_.Task_CompleteTime))) {
    				this._Task_CompleteTime = reader.GetDateTime(_.Task_CompleteTime);
    			}
    			if ((false == reader.IsDBNull(_.Task_Level))) {
    				this._Task_Level = reader.GetInt32(_.Task_Level);
    			}
    			if ((false == reader.IsDBNull(_.Task_Tax))) {
    				this._Task_Tax = reader.GetInt32(_.Task_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Task_WorkerId))) {
    				this._Task_WorkerId = reader.GetInt32(_.Task_WorkerId);
    			}
    			if ((false == reader.IsDBNull(_.Task_WorkerName))) {
    				this._Task_WorkerName = reader.GetString(_.Task_WorkerName);
    			}
    			if ((false == reader.IsDBNull(_.Task_IsUse))) {
    				this._Task_IsUse = reader.GetBoolean(_.Task_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Task_WorkLog))) {
    				this._Task_WorkLog = reader.GetString(_.Task_WorkLog);
    			}
    			if ((false == reader.IsDBNull(_.Task_CompletePer))) {
    				this._Task_CompletePer = reader.GetInt32(_.Task_CompletePer);
    			}
    			if ((false == reader.IsDBNull(_.Task_IsGoback))) {
    				this._Task_IsGoback = reader.GetBoolean(_.Task_IsGoback);
    			}
    			if ((false == reader.IsDBNull(_.Task_GobackText))) {
    				this._Task_GobackText = reader.GetString(_.Task_GobackText);
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
    			if ((false == typeof(Task).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Task>();
    			
    			/// <summary>
    			/// -1 - 字段名：Task_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Task_Id = new WeiSha.Data.Field<Task>("Task_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Task_Name = new WeiSha.Data.Field<Task>("Task_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_Context - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Task_Context = new WeiSha.Data.Field<Task>("Task_Context");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Task_CrtTime = new WeiSha.Data.Field<Task>("Task_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<Task>("Acc_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Name = new WeiSha.Data.Field<Task>("Acc_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_StartTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Task_StartTime = new WeiSha.Data.Field<Task>("Task_StartTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_EndTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Task_EndTime = new WeiSha.Data.Field<Task>("Task_EndTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_State - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Task_State = new WeiSha.Data.Field<Task>("Task_State");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_IsComplete - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Task_IsComplete = new WeiSha.Data.Field<Task>("Task_IsComplete");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_CompleteTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Task_CompleteTime = new WeiSha.Data.Field<Task>("Task_CompleteTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_Level - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Task_Level = new WeiSha.Data.Field<Task>("Task_Level");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_Tax - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Task_Tax = new WeiSha.Data.Field<Task>("Task_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_WorkerId - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Task_WorkerId = new WeiSha.Data.Field<Task>("Task_WorkerId");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_WorkerName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Task_WorkerName = new WeiSha.Data.Field<Task>("Task_WorkerName");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Task_IsUse = new WeiSha.Data.Field<Task>("Task_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_WorkLog - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Task_WorkLog = new WeiSha.Data.Field<Task>("Task_WorkLog");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_CompletePer - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Task_CompletePer = new WeiSha.Data.Field<Task>("Task_CompletePer");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_IsGoback - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Task_IsGoback = new WeiSha.Data.Field<Task>("Task_IsGoback");
    			
    			/// <summary>
    			/// -1 - 字段名：Task_GobackText - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Task_GobackText = new WeiSha.Data.Field<Task>("Task_GobackText");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Task>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Task>("Org_Name");
    		}
    	}
    }
    