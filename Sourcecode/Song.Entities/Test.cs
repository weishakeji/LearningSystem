namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Test 主键列：Test_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Test : WeiSha.Data.Entity {
    		
    		protected Int32 _Test_ID;
    		
    		protected String _Test_Name;
    		
    		protected DateTime? _Test_StartTime;
    		
    		protected Int32? _Test_Span;
    		
    		protected String _Test_Tearcher;
    		
    		protected String _Test_Room;
    		
    		protected Int32? _Tp_Id;
    		
    		protected String _Tp_Name;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Int32 _Th_ID;
    		
    		protected String _Th_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Test_ID {
    			get {
    				return this._Test_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Test_ID, _Test_ID, value);
    				this._Test_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Test_Name {
    			get {
    				return this._Test_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Test_Name, _Test_Name, value);
    				this._Test_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Test_StartTime {
    			get {
    				return this._Test_StartTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Test_StartTime, _Test_StartTime, value);
    				this._Test_StartTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Test_Span {
    			get {
    				return this._Test_Span;
    			}
    			set {
    				this.OnPropertyValueChange(_.Test_Span, _Test_Span, value);
    				this._Test_Span = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Test_Tearcher {
    			get {
    				return this._Test_Tearcher;
    			}
    			set {
    				this.OnPropertyValueChange(_.Test_Tearcher, _Test_Tearcher, value);
    				this._Test_Tearcher = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Test_Room {
    			get {
    				return this._Test_Room;
    			}
    			set {
    				this.OnPropertyValueChange(_.Test_Room, _Test_Room, value);
    				this._Test_Room = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Tp_Id {
    			get {
    				return this._Tp_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Id, _Tp_Id, value);
    				this._Tp_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Tp_Name {
    			get {
    				return this._Tp_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Name, _Tp_Name, value);
    				this._Tp_Name = value;
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
    		
    		public Int32 Th_ID {
    			get {
    				return this._Th_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_ID, _Th_ID, value);
    				this._Th_ID = value;
    			}
    		}
    		
    		public String Th_Name {
    			get {
    				return this._Th_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Name, _Th_Name, value);
    				this._Th_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Test>("Test");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Test_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Test_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Test_ID,
    					_.Test_Name,
    					_.Test_StartTime,
    					_.Test_Span,
    					_.Test_Tearcher,
    					_.Test_Room,
    					_.Tp_Id,
    					_.Tp_Name,
    					_.Org_ID,
    					_.Org_Name,
    					_.Th_ID,
    					_.Th_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Test_ID,
    					this._Test_Name,
    					this._Test_StartTime,
    					this._Test_Span,
    					this._Test_Tearcher,
    					this._Test_Room,
    					this._Tp_Id,
    					this._Tp_Name,
    					this._Org_ID,
    					this._Org_Name,
    					this._Th_ID,
    					this._Th_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Test_ID))) {
    				this._Test_ID = reader.GetInt32(_.Test_ID);
    			}
    			if ((false == reader.IsDBNull(_.Test_Name))) {
    				this._Test_Name = reader.GetString(_.Test_Name);
    			}
    			if ((false == reader.IsDBNull(_.Test_StartTime))) {
    				this._Test_StartTime = reader.GetDateTime(_.Test_StartTime);
    			}
    			if ((false == reader.IsDBNull(_.Test_Span))) {
    				this._Test_Span = reader.GetInt32(_.Test_Span);
    			}
    			if ((false == reader.IsDBNull(_.Test_Tearcher))) {
    				this._Test_Tearcher = reader.GetString(_.Test_Tearcher);
    			}
    			if ((false == reader.IsDBNull(_.Test_Room))) {
    				this._Test_Room = reader.GetString(_.Test_Room);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Id))) {
    				this._Tp_Id = reader.GetInt32(_.Tp_Id);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Name))) {
    				this._Tp_Name = reader.GetString(_.Tp_Name);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Th_ID))) {
    				this._Th_ID = reader.GetInt32(_.Th_ID);
    			}
    			if ((false == reader.IsDBNull(_.Th_Name))) {
    				this._Th_Name = reader.GetString(_.Th_Name);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Test).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Test>();
    			
    			/// <summary>
    			/// -1 - 字段名：Test_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Test_ID = new WeiSha.Data.Field<Test>("Test_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Test_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Test_Name = new WeiSha.Data.Field<Test>("Test_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Test_StartTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Test_StartTime = new WeiSha.Data.Field<Test>("Test_StartTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Test_Span - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Test_Span = new WeiSha.Data.Field<Test>("Test_Span");
    			
    			/// <summary>
    			/// -1 - 字段名：Test_Tearcher - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Test_Tearcher = new WeiSha.Data.Field<Test>("Test_Tearcher");
    			
    			/// <summary>
    			/// -1 - 字段名：Test_Room - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Test_Room = new WeiSha.Data.Field<Test>("Test_Room");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Id = new WeiSha.Data.Field<Test>("Tp_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Name = new WeiSha.Data.Field<Test>("Tp_Name");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Test>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Test>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Th_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_ID = new WeiSha.Data.Field<Test>("Th_ID");
    			
    			/// <summary>
    			/// 字段名：Th_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Name = new WeiSha.Data.Field<Test>("Th_Name");
    		}
    	}
    }
    