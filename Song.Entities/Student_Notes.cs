namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Student_Notes 主键列：Stn_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Student_Notes : WeiSha.Data.Entity {
    		
    		protected Int32 _Stn_ID;
    		
    		protected Int32? _Stn_PID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected Int32 _Cou_ID;
    		
    		protected Int32 _Qus_ID;
    		
    		protected Int32 _Qus_Type;
    		
    		protected String _Qus_Title;
    		
    		protected String _Stn_Title;
    		
    		protected String _Stn_Context;
    		
    		protected DateTime _Stn_CrtTime;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		public Int32 Stn_ID {
    			get {
    				return this._Stn_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stn_ID, _Stn_ID, value);
    				this._Stn_ID = value;
    			}
    		}
    		
    		public Int32? Stn_PID {
    			get {
    				return this._Stn_PID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stn_PID, _Stn_PID, value);
    				this._Stn_PID = value;
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
    		
    		public Int32 Cou_ID {
    			get {
    				return this._Cou_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ID, _Cou_ID, value);
    				this._Cou_ID = value;
    			}
    		}
    		
    		public Int32 Qus_ID {
    			get {
    				return this._Qus_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_ID, _Qus_ID, value);
    				this._Qus_ID = value;
    			}
    		}
    		
    		public Int32 Qus_Type {
    			get {
    				return this._Qus_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Type, _Qus_Type, value);
    				this._Qus_Type = value;
    			}
    		}
    		
    		public String Qus_Title {
    			get {
    				return this._Qus_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Title, _Qus_Title, value);
    				this._Qus_Title = value;
    			}
    		}
    		
    		public String Stn_Title {
    			get {
    				return this._Stn_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stn_Title, _Stn_Title, value);
    				this._Stn_Title = value;
    			}
    		}
    		
    		public String Stn_Context {
    			get {
    				return this._Stn_Context;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stn_Context, _Stn_Context, value);
    				this._Stn_Context = value;
    			}
    		}
    		
    		public DateTime Stn_CrtTime {
    			get {
    				return this._Stn_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stn_CrtTime, _Stn_CrtTime, value);
    				this._Stn_CrtTime = value;
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
    			return new WeiSha.Data.Table<Student_Notes>("Student_Notes");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Stn_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Stn_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Stn_ID,
    					_.Stn_PID,
    					_.Ac_ID,
    					_.Cou_ID,
    					_.Qus_ID,
    					_.Qus_Type,
    					_.Qus_Title,
    					_.Stn_Title,
    					_.Stn_Context,
    					_.Stn_CrtTime,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Stn_ID,
    					this._Stn_PID,
    					this._Ac_ID,
    					this._Cou_ID,
    					this._Qus_ID,
    					this._Qus_Type,
    					this._Qus_Title,
    					this._Stn_Title,
    					this._Stn_Context,
    					this._Stn_CrtTime,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Stn_ID))) {
    				this._Stn_ID = reader.GetInt32(_.Stn_ID);
    			}
    			if ((false == reader.IsDBNull(_.Stn_PID))) {
    				this._Stn_PID = reader.GetInt32(_.Stn_PID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qus_ID))) {
    				this._Qus_ID = reader.GetInt32(_.Qus_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Type))) {
    				this._Qus_Type = reader.GetInt32(_.Qus_Type);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Title))) {
    				this._Qus_Title = reader.GetString(_.Qus_Title);
    			}
    			if ((false == reader.IsDBNull(_.Stn_Title))) {
    				this._Stn_Title = reader.GetString(_.Stn_Title);
    			}
    			if ((false == reader.IsDBNull(_.Stn_Context))) {
    				this._Stn_Context = reader.GetString(_.Stn_Context);
    			}
    			if ((false == reader.IsDBNull(_.Stn_CrtTime))) {
    				this._Stn_CrtTime = reader.GetDateTime(_.Stn_CrtTime);
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
    			if ((false == typeof(Student_Notes).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Student_Notes>();
    			
    			/// <summary>
    			/// 字段名：Stn_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Stn_ID = new WeiSha.Data.Field<Student_Notes>("Stn_ID");
    			
    			/// <summary>
    			/// 字段名：Stn_PID - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Stn_PID = new WeiSha.Data.Field<Student_Notes>("Stn_PID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<Student_Notes>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<Student_Notes>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Qus_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_ID = new WeiSha.Data.Field<Student_Notes>("Qus_ID");
    			
    			/// <summary>
    			/// 字段名：Qus_Type - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Type = new WeiSha.Data.Field<Student_Notes>("Qus_Type");
    			
    			/// <summary>
    			/// 字段名：Qus_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Title = new WeiSha.Data.Field<Student_Notes>("Qus_Title");
    			
    			/// <summary>
    			/// 字段名：Stn_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Stn_Title = new WeiSha.Data.Field<Student_Notes>("Stn_Title");
    			
    			/// <summary>
    			/// 字段名：Stn_Context - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Stn_Context = new WeiSha.Data.Field<Student_Notes>("Stn_Context");
    			
    			/// <summary>
    			/// 字段名：Stn_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Stn_CrtTime = new WeiSha.Data.Field<Student_Notes>("Stn_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Student_Notes>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Student_Notes>("Org_Name");
    		}
    	}
    }
    