namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：DailyLog 主键列：Dlog_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class DailyLog : WeiSha.Data.Entity {
    		
    		protected Int32 _Dlog_Id;
    		
    		protected String _Dlog_Type;
    		
    		protected String _Dlog_Note;
    		
    		protected String _Dlog_Plan;
    		
    		protected DateTime? _Dlog_CrtTime;
    		
    		protected DateTime? _Dlog_WrtTime;
    		
    		protected Int32? _Acc_Id;
    		
    		protected String _Acc_Name;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32 Dlog_Id {
    			get {
    				return this._Dlog_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dlog_Id, _Dlog_Id, value);
    				this._Dlog_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Dlog_Type {
    			get {
    				return this._Dlog_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dlog_Type, _Dlog_Type, value);
    				this._Dlog_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Dlog_Note {
    			get {
    				return this._Dlog_Note;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dlog_Note, _Dlog_Note, value);
    				this._Dlog_Note = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Dlog_Plan {
    			get {
    				return this._Dlog_Plan;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dlog_Plan, _Dlog_Plan, value);
    				this._Dlog_Plan = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public DateTime? Dlog_CrtTime {
    			get {
    				return this._Dlog_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dlog_CrtTime, _Dlog_CrtTime, value);
    				this._Dlog_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public DateTime? Dlog_WrtTime {
    			get {
    				return this._Dlog_WrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dlog_WrtTime, _Dlog_WrtTime, value);
    				this._Dlog_WrtTime = value;
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
    			return new WeiSha.Data.Table<DailyLog>("DailyLog");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Dlog_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Dlog_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Dlog_Id,
    					_.Dlog_Type,
    					_.Dlog_Note,
    					_.Dlog_Plan,
    					_.Dlog_CrtTime,
    					_.Dlog_WrtTime,
    					_.Acc_Id,
    					_.Acc_Name,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Dlog_Id,
    					this._Dlog_Type,
    					this._Dlog_Note,
    					this._Dlog_Plan,
    					this._Dlog_CrtTime,
    					this._Dlog_WrtTime,
    					this._Acc_Id,
    					this._Acc_Name,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Dlog_Id))) {
    				this._Dlog_Id = reader.GetInt32(_.Dlog_Id);
    			}
    			if ((false == reader.IsDBNull(_.Dlog_Type))) {
    				this._Dlog_Type = reader.GetString(_.Dlog_Type);
    			}
    			if ((false == reader.IsDBNull(_.Dlog_Note))) {
    				this._Dlog_Note = reader.GetString(_.Dlog_Note);
    			}
    			if ((false == reader.IsDBNull(_.Dlog_Plan))) {
    				this._Dlog_Plan = reader.GetString(_.Dlog_Plan);
    			}
    			if ((false == reader.IsDBNull(_.Dlog_CrtTime))) {
    				this._Dlog_CrtTime = reader.GetDateTime(_.Dlog_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Dlog_WrtTime))) {
    				this._Dlog_WrtTime = reader.GetDateTime(_.Dlog_WrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Name))) {
    				this._Acc_Name = reader.GetString(_.Acc_Name);
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
    			if ((false == typeof(DailyLog).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<DailyLog>();
    			
    			/// <summary>
    			/// False - 字段名：Dlog_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dlog_Id = new WeiSha.Data.Field<DailyLog>("Dlog_Id");
    			
    			/// <summary>
    			/// True - 字段名：Dlog_Type - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dlog_Type = new WeiSha.Data.Field<DailyLog>("Dlog_Type");
    			
    			/// <summary>
    			/// True - 字段名：Dlog_Note - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dlog_Note = new WeiSha.Data.Field<DailyLog>("Dlog_Note");
    			
    			/// <summary>
    			/// True - 字段名：Dlog_Plan - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dlog_Plan = new WeiSha.Data.Field<DailyLog>("Dlog_Plan");
    			
    			/// <summary>
    			/// False - 字段名：Dlog_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Dlog_CrtTime = new WeiSha.Data.Field<DailyLog>("Dlog_CrtTime");
    			
    			/// <summary>
    			/// False - 字段名：Dlog_WrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Dlog_WrtTime = new WeiSha.Data.Field<DailyLog>("Dlog_WrtTime");
    			
    			/// <summary>
    			/// False - 字段名：Acc_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<DailyLog>("Acc_Id");
    			
    			/// <summary>
    			/// True - 字段名：Acc_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Name = new WeiSha.Data.Field<DailyLog>("Acc_Name");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<DailyLog>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<DailyLog>("Org_Name");
    		}
    	}
    }
    