namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：TeacherComment 主键列：Thc_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class TeacherComment : WeiSha.Data.Entity {
    		
    		protected Int32 _Thc_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected Int32 _Th_ID;
    		
    		protected String _Th_Name;
    		
    		protected Double _Thc_Score;
    		
    		protected String _Thc_Comment;
    		
    		protected String _Thc_Reply;
    		
    		protected DateTime _Thc_CrtTime;
    		
    		protected String _Thc_Device;
    		
    		protected String _Thc_IP;
    		
    		protected Boolean _Thc_IsUse;
    		
    		protected Boolean _Thc_IsShow;
    		
    		protected Int32 _Org_ID;
    		
    		public Int32 Thc_ID {
    			get {
    				return this._Thc_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thc_ID, _Thc_ID, value);
    				this._Thc_ID = value;
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
    		
    		public Double Thc_Score {
    			get {
    				return this._Thc_Score;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thc_Score, _Thc_Score, value);
    				this._Thc_Score = value;
    			}
    		}
    		
    		public String Thc_Comment {
    			get {
    				return this._Thc_Comment;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thc_Comment, _Thc_Comment, value);
    				this._Thc_Comment = value;
    			}
    		}
    		
    		public String Thc_Reply {
    			get {
    				return this._Thc_Reply;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thc_Reply, _Thc_Reply, value);
    				this._Thc_Reply = value;
    			}
    		}
    		
    		public DateTime Thc_CrtTime {
    			get {
    				return this._Thc_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thc_CrtTime, _Thc_CrtTime, value);
    				this._Thc_CrtTime = value;
    			}
    		}
    		
    		public String Thc_Device {
    			get {
    				return this._Thc_Device;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thc_Device, _Thc_Device, value);
    				this._Thc_Device = value;
    			}
    		}
    		
    		public String Thc_IP {
    			get {
    				return this._Thc_IP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thc_IP, _Thc_IP, value);
    				this._Thc_IP = value;
    			}
    		}
    		
    		public Boolean Thc_IsUse {
    			get {
    				return this._Thc_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thc_IsUse, _Thc_IsUse, value);
    				this._Thc_IsUse = value;
    			}
    		}
    		
    		public Boolean Thc_IsShow {
    			get {
    				return this._Thc_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thc_IsShow, _Thc_IsShow, value);
    				this._Thc_IsShow = value;
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
    			return new WeiSha.Data.Table<TeacherComment>("TeacherComment");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Thc_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Thc_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Thc_ID,
    					_.Ac_ID,
    					_.Th_ID,
    					_.Th_Name,
    					_.Thc_Score,
    					_.Thc_Comment,
    					_.Thc_Reply,
    					_.Thc_CrtTime,
    					_.Thc_Device,
    					_.Thc_IP,
    					_.Thc_IsUse,
    					_.Thc_IsShow,
    					_.Org_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Thc_ID,
    					this._Ac_ID,
    					this._Th_ID,
    					this._Th_Name,
    					this._Thc_Score,
    					this._Thc_Comment,
    					this._Thc_Reply,
    					this._Thc_CrtTime,
    					this._Thc_Device,
    					this._Thc_IP,
    					this._Thc_IsUse,
    					this._Thc_IsShow,
    					this._Org_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Thc_ID))) {
    				this._Thc_ID = reader.GetInt32(_.Thc_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Th_ID))) {
    				this._Th_ID = reader.GetInt32(_.Th_ID);
    			}
    			if ((false == reader.IsDBNull(_.Th_Name))) {
    				this._Th_Name = reader.GetString(_.Th_Name);
    			}
    			if ((false == reader.IsDBNull(_.Thc_Score))) {
    				this._Thc_Score = reader.GetDouble(_.Thc_Score);
    			}
    			if ((false == reader.IsDBNull(_.Thc_Comment))) {
    				this._Thc_Comment = reader.GetString(_.Thc_Comment);
    			}
    			if ((false == reader.IsDBNull(_.Thc_Reply))) {
    				this._Thc_Reply = reader.GetString(_.Thc_Reply);
    			}
    			if ((false == reader.IsDBNull(_.Thc_CrtTime))) {
    				this._Thc_CrtTime = reader.GetDateTime(_.Thc_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Thc_Device))) {
    				this._Thc_Device = reader.GetString(_.Thc_Device);
    			}
    			if ((false == reader.IsDBNull(_.Thc_IP))) {
    				this._Thc_IP = reader.GetString(_.Thc_IP);
    			}
    			if ((false == reader.IsDBNull(_.Thc_IsUse))) {
    				this._Thc_IsUse = reader.GetBoolean(_.Thc_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Thc_IsShow))) {
    				this._Thc_IsShow = reader.GetBoolean(_.Thc_IsShow);
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
    			if ((false == typeof(TeacherComment).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<TeacherComment>();
    			
    			/// <summary>
    			/// 字段名：Thc_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Thc_ID = new WeiSha.Data.Field<TeacherComment>("Thc_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<TeacherComment>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Th_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_ID = new WeiSha.Data.Field<TeacherComment>("Th_ID");
    			
    			/// <summary>
    			/// 字段名：Th_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Name = new WeiSha.Data.Field<TeacherComment>("Th_Name");
    			
    			/// <summary>
    			/// 字段名：Thc_Score - 数据类型：Double
    			/// </summary>
    			public static WeiSha.Data.Field Thc_Score = new WeiSha.Data.Field<TeacherComment>("Thc_Score");
    			
    			/// <summary>
    			/// 字段名：Thc_Comment - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Thc_Comment = new WeiSha.Data.Field<TeacherComment>("Thc_Comment");
    			
    			/// <summary>
    			/// 字段名：Thc_Reply - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Thc_Reply = new WeiSha.Data.Field<TeacherComment>("Thc_Reply");
    			
    			/// <summary>
    			/// 字段名：Thc_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Thc_CrtTime = new WeiSha.Data.Field<TeacherComment>("Thc_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Thc_Device - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Thc_Device = new WeiSha.Data.Field<TeacherComment>("Thc_Device");
    			
    			/// <summary>
    			/// 字段名：Thc_IP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Thc_IP = new WeiSha.Data.Field<TeacherComment>("Thc_IP");
    			
    			/// <summary>
    			/// 字段名：Thc_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Thc_IsUse = new WeiSha.Data.Field<TeacherComment>("Thc_IsUse");
    			
    			/// <summary>
    			/// 字段名：Thc_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Thc_IsShow = new WeiSha.Data.Field<TeacherComment>("Thc_IsShow");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<TeacherComment>("Org_ID");
    		}
    	}
    }
    