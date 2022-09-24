namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：LogForStudentExercise 主键列：Lse_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class LogForStudentExercise : WeiSha.Data.Entity {
    		
    		protected Int32 _Lse_ID;
    		
    		protected Int32 _Org_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected String _Ac_AccName;
    		
    		protected String _Ac_Name;
    		
    		protected Int64 _Cou_ID;
    		
    		protected Int64 _Ol_ID;
    		
    		protected String _Lse_UID;
    		
    		protected DateTime _Lse_CrtTime;
    		
    		protected DateTime _Lse_LastTime;
    		
    		protected String _Lse_JsonData;
    		
    		protected Int32 _Lse_Sum;
    		
    		protected Int32 _Lse_Answer;
    		
    		protected Int32 _Lse_Correct;
    		
    		protected Int32 _Lse_Wrong;
    		
    		protected Decimal _Lse_Rate;
    		
    		protected String _Lse_IP;
    		
    		protected String _Lse_Browser;
    		
    		protected String _Lse_Platform;
    		
    		protected String _Lse_OS;
    		
    		public Int32 Lse_ID {
    			get {
    				return this._Lse_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lse_ID, _Lse_ID, value);
    				this._Lse_ID = value;
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
    		
    		public Int64 Cou_ID {
    			get {
    				return this._Cou_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ID, _Cou_ID, value);
    				this._Cou_ID = value;
    			}
    		}
    		
    		public Int64 Ol_ID {
    			get {
    				return this._Ol_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_ID, _Ol_ID, value);
    				this._Ol_ID = value;
    			}
    		}
    		
    		public String Lse_UID {
    			get {
    				return this._Lse_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lse_UID, _Lse_UID, value);
    				this._Lse_UID = value;
    			}
    		}
    		
    		public DateTime Lse_CrtTime {
    			get {
    				return this._Lse_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lse_CrtTime, _Lse_CrtTime, value);
    				this._Lse_CrtTime = value;
    			}
    		}
    		
    		public DateTime Lse_LastTime {
    			get {
    				return this._Lse_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lse_LastTime, _Lse_LastTime, value);
    				this._Lse_LastTime = value;
    			}
    		}
    		
    		public String Lse_JsonData {
    			get {
    				return this._Lse_JsonData;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lse_JsonData, _Lse_JsonData, value);
    				this._Lse_JsonData = value;
    			}
    		}
    		
    		public Int32 Lse_Sum {
    			get {
    				return this._Lse_Sum;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lse_Sum, _Lse_Sum, value);
    				this._Lse_Sum = value;
    			}
    		}
    		
    		public Int32 Lse_Answer {
    			get {
    				return this._Lse_Answer;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lse_Answer, _Lse_Answer, value);
    				this._Lse_Answer = value;
    			}
    		}
    		
    		public Int32 Lse_Correct {
    			get {
    				return this._Lse_Correct;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lse_Correct, _Lse_Correct, value);
    				this._Lse_Correct = value;
    			}
    		}
    		
    		public Int32 Lse_Wrong {
    			get {
    				return this._Lse_Wrong;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lse_Wrong, _Lse_Wrong, value);
    				this._Lse_Wrong = value;
    			}
    		}
    		
    		public Decimal Lse_Rate {
    			get {
    				return this._Lse_Rate;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lse_Rate, _Lse_Rate, value);
    				this._Lse_Rate = value;
    			}
    		}
    		
    		public String Lse_IP {
    			get {
    				return this._Lse_IP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lse_IP, _Lse_IP, value);
    				this._Lse_IP = value;
    			}
    		}
    		
    		public String Lse_Browser {
    			get {
    				return this._Lse_Browser;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lse_Browser, _Lse_Browser, value);
    				this._Lse_Browser = value;
    			}
    		}
    		
    		public String Lse_Platform {
    			get {
    				return this._Lse_Platform;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lse_Platform, _Lse_Platform, value);
    				this._Lse_Platform = value;
    			}
    		}
    		
    		public String Lse_OS {
    			get {
    				return this._Lse_OS;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lse_OS, _Lse_OS, value);
    				this._Lse_OS = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<LogForStudentExercise>("LogForStudentExercise");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Lse_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lse_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lse_ID,
    					_.Org_ID,
    					_.Ac_ID,
    					_.Ac_AccName,
    					_.Ac_Name,
    					_.Cou_ID,
    					_.Ol_ID,
    					_.Lse_UID,
    					_.Lse_CrtTime,
    					_.Lse_LastTime,
    					_.Lse_JsonData,
    					_.Lse_Sum,
    					_.Lse_Answer,
    					_.Lse_Correct,
    					_.Lse_Wrong,
    					_.Lse_Rate,
    					_.Lse_IP,
    					_.Lse_Browser,
    					_.Lse_Platform,
    					_.Lse_OS};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Lse_ID,
    					this._Org_ID,
    					this._Ac_ID,
    					this._Ac_AccName,
    					this._Ac_Name,
    					this._Cou_ID,
    					this._Ol_ID,
    					this._Lse_UID,
    					this._Lse_CrtTime,
    					this._Lse_LastTime,
    					this._Lse_JsonData,
    					this._Lse_Sum,
    					this._Lse_Answer,
    					this._Lse_Correct,
    					this._Lse_Wrong,
    					this._Lse_Rate,
    					this._Lse_IP,
    					this._Lse_Browser,
    					this._Lse_Platform,
    					this._Lse_OS};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Lse_ID))) {
    				this._Lse_ID = reader.GetInt32(_.Lse_ID);
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
    				this._Cou_ID = reader.GetInt64(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ol_ID))) {
    				this._Ol_ID = reader.GetInt64(_.Ol_ID);
    			}
    			if ((false == reader.IsDBNull(_.Lse_UID))) {
    				this._Lse_UID = reader.GetString(_.Lse_UID);
    			}
    			if ((false == reader.IsDBNull(_.Lse_CrtTime))) {
    				this._Lse_CrtTime = reader.GetDateTime(_.Lse_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Lse_LastTime))) {
    				this._Lse_LastTime = reader.GetDateTime(_.Lse_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Lse_JsonData))) {
    				this._Lse_JsonData = reader.GetString(_.Lse_JsonData);
    			}
    			if ((false == reader.IsDBNull(_.Lse_Sum))) {
    				this._Lse_Sum = reader.GetInt32(_.Lse_Sum);
    			}
    			if ((false == reader.IsDBNull(_.Lse_Answer))) {
    				this._Lse_Answer = reader.GetInt32(_.Lse_Answer);
    			}
    			if ((false == reader.IsDBNull(_.Lse_Correct))) {
    				this._Lse_Correct = reader.GetInt32(_.Lse_Correct);
    			}
    			if ((false == reader.IsDBNull(_.Lse_Wrong))) {
    				this._Lse_Wrong = reader.GetInt32(_.Lse_Wrong);
    			}
    			if ((false == reader.IsDBNull(_.Lse_Rate))) {
    				this._Lse_Rate = reader.GetDecimal(_.Lse_Rate);
    			}
    			if ((false == reader.IsDBNull(_.Lse_IP))) {
    				this._Lse_IP = reader.GetString(_.Lse_IP);
    			}
    			if ((false == reader.IsDBNull(_.Lse_Browser))) {
    				this._Lse_Browser = reader.GetString(_.Lse_Browser);
    			}
    			if ((false == reader.IsDBNull(_.Lse_Platform))) {
    				this._Lse_Platform = reader.GetString(_.Lse_Platform);
    			}
    			if ((false == reader.IsDBNull(_.Lse_OS))) {
    				this._Lse_OS = reader.GetString(_.Lse_OS);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(LogForStudentExercise).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<LogForStudentExercise>();
    			
    			/// <summary>
    			/// 字段名：Lse_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lse_ID = new WeiSha.Data.Field<LogForStudentExercise>("Lse_ID");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<LogForStudentExercise>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<LogForStudentExercise>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_AccName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_AccName = new WeiSha.Data.Field<LogForStudentExercise>("Ac_AccName");
    			
    			/// <summary>
    			/// 字段名：Ac_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Name = new WeiSha.Data.Field<LogForStudentExercise>("Ac_Name");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<LogForStudentExercise>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Ol_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Ol_ID = new WeiSha.Data.Field<LogForStudentExercise>("Ol_ID");
    			
    			/// <summary>
    			/// 字段名：Lse_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lse_UID = new WeiSha.Data.Field<LogForStudentExercise>("Lse_UID");
    			
    			/// <summary>
    			/// 字段名：Lse_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lse_CrtTime = new WeiSha.Data.Field<LogForStudentExercise>("Lse_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Lse_LastTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lse_LastTime = new WeiSha.Data.Field<LogForStudentExercise>("Lse_LastTime");
    			
    			/// <summary>
    			/// 字段名：Lse_JsonData - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lse_JsonData = new WeiSha.Data.Field<LogForStudentExercise>("Lse_JsonData");
    			
    			/// <summary>
    			/// 字段名：Lse_Sum - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lse_Sum = new WeiSha.Data.Field<LogForStudentExercise>("Lse_Sum");
    			
    			/// <summary>
    			/// 字段名：Lse_Answer - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lse_Answer = new WeiSha.Data.Field<LogForStudentExercise>("Lse_Answer");
    			
    			/// <summary>
    			/// 字段名：Lse_Correct - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lse_Correct = new WeiSha.Data.Field<LogForStudentExercise>("Lse_Correct");
    			
    			/// <summary>
    			/// 字段名：Lse_Wrong - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lse_Wrong = new WeiSha.Data.Field<LogForStudentExercise>("Lse_Wrong");
    			
    			/// <summary>
    			/// 字段名：Lse_Rate - 数据类型：Decimal
    			/// </summary>
    			public static WeiSha.Data.Field Lse_Rate = new WeiSha.Data.Field<LogForStudentExercise>("Lse_Rate");
    			
    			/// <summary>
    			/// 字段名：Lse_IP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lse_IP = new WeiSha.Data.Field<LogForStudentExercise>("Lse_IP");
    			
    			/// <summary>
    			/// 字段名：Lse_Browser - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lse_Browser = new WeiSha.Data.Field<LogForStudentExercise>("Lse_Browser");
    			
    			/// <summary>
    			/// 字段名：Lse_Platform - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lse_Platform = new WeiSha.Data.Field<LogForStudentExercise>("Lse_Platform");
    			
    			/// <summary>
    			/// 字段名：Lse_OS - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lse_OS = new WeiSha.Data.Field<LogForStudentExercise>("Lse_OS");
    		}
    	}
    }
    