namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Forum 主键列：Fm_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Forum : WeiSha.Data.Entity {
    		
    		protected Int32 _Fm_ID;
    		
    		protected String _Fm_Title;
    		
    		protected Boolean _Fm_IsTheme;
    		
    		protected String _Fm_Content;
    		
    		protected Int32 _Fm_PID;
    		
    		protected String _Fm_UID;
    		
    		protected Int32 _Fm_At;
    		
    		protected DateTime _Fm_CrtTime;
    		
    		protected DateTime? _Fm_LastTime;
    		
    		protected String _Fm_IP;
    		
    		protected Int32 _Fm_ViewNumber;
    		
    		protected Int32 _Fm_ReplyNumber;
    		
    		protected Boolean _Fm_IsDel;
    		
    		protected Boolean _Fm_IsTop;
    		
    		protected Boolean _Fm_IsRec;
    		
    		protected Int32? _Ac_ID;
    		
    		protected String _Ac_Name;
    		
    		protected Int32? _Th_ID;
    		
    		protected String _Th_Name;
    		
    		protected Int32? _Org_ID;
    		
    		public Int32 Fm_ID {
    			get {
    				return this._Fm_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_ID, _Fm_ID, value);
    				this._Fm_ID = value;
    			}
    		}
    		
    		public String Fm_Title {
    			get {
    				return this._Fm_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_Title, _Fm_Title, value);
    				this._Fm_Title = value;
    			}
    		}
    		
    		public Boolean Fm_IsTheme {
    			get {
    				return this._Fm_IsTheme;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_IsTheme, _Fm_IsTheme, value);
    				this._Fm_IsTheme = value;
    			}
    		}
    		
    		public String Fm_Content {
    			get {
    				return this._Fm_Content;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_Content, _Fm_Content, value);
    				this._Fm_Content = value;
    			}
    		}
    		
    		public Int32 Fm_PID {
    			get {
    				return this._Fm_PID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_PID, _Fm_PID, value);
    				this._Fm_PID = value;
    			}
    		}
    		
    		public String Fm_UID {
    			get {
    				return this._Fm_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_UID, _Fm_UID, value);
    				this._Fm_UID = value;
    			}
    		}
    		
    		public Int32 Fm_At {
    			get {
    				return this._Fm_At;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_At, _Fm_At, value);
    				this._Fm_At = value;
    			}
    		}
    		
    		public DateTime Fm_CrtTime {
    			get {
    				return this._Fm_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_CrtTime, _Fm_CrtTime, value);
    				this._Fm_CrtTime = value;
    			}
    		}
    		
    		public DateTime? Fm_LastTime {
    			get {
    				return this._Fm_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_LastTime, _Fm_LastTime, value);
    				this._Fm_LastTime = value;
    			}
    		}
    		
    		public String Fm_IP {
    			get {
    				return this._Fm_IP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_IP, _Fm_IP, value);
    				this._Fm_IP = value;
    			}
    		}
    		
    		public Int32 Fm_ViewNumber {
    			get {
    				return this._Fm_ViewNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_ViewNumber, _Fm_ViewNumber, value);
    				this._Fm_ViewNumber = value;
    			}
    		}
    		
    		public Int32 Fm_ReplyNumber {
    			get {
    				return this._Fm_ReplyNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_ReplyNumber, _Fm_ReplyNumber, value);
    				this._Fm_ReplyNumber = value;
    			}
    		}
    		
    		public Boolean Fm_IsDel {
    			get {
    				return this._Fm_IsDel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_IsDel, _Fm_IsDel, value);
    				this._Fm_IsDel = value;
    			}
    		}
    		
    		public Boolean Fm_IsTop {
    			get {
    				return this._Fm_IsTop;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_IsTop, _Fm_IsTop, value);
    				this._Fm_IsTop = value;
    			}
    		}
    		
    		public Boolean Fm_IsRec {
    			get {
    				return this._Fm_IsRec;
    			}
    			set {
    				this.OnPropertyValueChange(_.Fm_IsRec, _Fm_IsRec, value);
    				this._Fm_IsRec = value;
    			}
    		}
    		
    		public Int32? Ac_ID {
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
    		
    		public Int32? Th_ID {
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
    		
    		public Int32? Org_ID {
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
    			return new WeiSha.Data.Table<Forum>("Forum");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Fm_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Fm_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Fm_ID,
    					_.Fm_Title,
    					_.Fm_IsTheme,
    					_.Fm_Content,
    					_.Fm_PID,
    					_.Fm_UID,
    					_.Fm_At,
    					_.Fm_CrtTime,
    					_.Fm_LastTime,
    					_.Fm_IP,
    					_.Fm_ViewNumber,
    					_.Fm_ReplyNumber,
    					_.Fm_IsDel,
    					_.Fm_IsTop,
    					_.Fm_IsRec,
    					_.Ac_ID,
    					_.Ac_Name,
    					_.Th_ID,
    					_.Th_Name,
    					_.Org_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Fm_ID,
    					this._Fm_Title,
    					this._Fm_IsTheme,
    					this._Fm_Content,
    					this._Fm_PID,
    					this._Fm_UID,
    					this._Fm_At,
    					this._Fm_CrtTime,
    					this._Fm_LastTime,
    					this._Fm_IP,
    					this._Fm_ViewNumber,
    					this._Fm_ReplyNumber,
    					this._Fm_IsDel,
    					this._Fm_IsTop,
    					this._Fm_IsRec,
    					this._Ac_ID,
    					this._Ac_Name,
    					this._Th_ID,
    					this._Th_Name,
    					this._Org_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Fm_ID))) {
    				this._Fm_ID = reader.GetInt32(_.Fm_ID);
    			}
    			if ((false == reader.IsDBNull(_.Fm_Title))) {
    				this._Fm_Title = reader.GetString(_.Fm_Title);
    			}
    			if ((false == reader.IsDBNull(_.Fm_IsTheme))) {
    				this._Fm_IsTheme = reader.GetBoolean(_.Fm_IsTheme);
    			}
    			if ((false == reader.IsDBNull(_.Fm_Content))) {
    				this._Fm_Content = reader.GetString(_.Fm_Content);
    			}
    			if ((false == reader.IsDBNull(_.Fm_PID))) {
    				this._Fm_PID = reader.GetInt32(_.Fm_PID);
    			}
    			if ((false == reader.IsDBNull(_.Fm_UID))) {
    				this._Fm_UID = reader.GetString(_.Fm_UID);
    			}
    			if ((false == reader.IsDBNull(_.Fm_At))) {
    				this._Fm_At = reader.GetInt32(_.Fm_At);
    			}
    			if ((false == reader.IsDBNull(_.Fm_CrtTime))) {
    				this._Fm_CrtTime = reader.GetDateTime(_.Fm_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Fm_LastTime))) {
    				this._Fm_LastTime = reader.GetDateTime(_.Fm_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Fm_IP))) {
    				this._Fm_IP = reader.GetString(_.Fm_IP);
    			}
    			if ((false == reader.IsDBNull(_.Fm_ViewNumber))) {
    				this._Fm_ViewNumber = reader.GetInt32(_.Fm_ViewNumber);
    			}
    			if ((false == reader.IsDBNull(_.Fm_ReplyNumber))) {
    				this._Fm_ReplyNumber = reader.GetInt32(_.Fm_ReplyNumber);
    			}
    			if ((false == reader.IsDBNull(_.Fm_IsDel))) {
    				this._Fm_IsDel = reader.GetBoolean(_.Fm_IsDel);
    			}
    			if ((false == reader.IsDBNull(_.Fm_IsTop))) {
    				this._Fm_IsTop = reader.GetBoolean(_.Fm_IsTop);
    			}
    			if ((false == reader.IsDBNull(_.Fm_IsRec))) {
    				this._Fm_IsRec = reader.GetBoolean(_.Fm_IsRec);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Name))) {
    				this._Ac_Name = reader.GetString(_.Ac_Name);
    			}
    			if ((false == reader.IsDBNull(_.Th_ID))) {
    				this._Th_ID = reader.GetInt32(_.Th_ID);
    			}
    			if ((false == reader.IsDBNull(_.Th_Name))) {
    				this._Th_Name = reader.GetString(_.Th_Name);
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
    			if ((false == typeof(Forum).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Forum>();
    			
    			/// <summary>
    			/// 字段名：Fm_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Fm_ID = new WeiSha.Data.Field<Forum>("Fm_ID");
    			
    			/// <summary>
    			/// 字段名：Fm_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Fm_Title = new WeiSha.Data.Field<Forum>("Fm_Title");
    			
    			/// <summary>
    			/// 字段名：Fm_IsTheme - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Fm_IsTheme = new WeiSha.Data.Field<Forum>("Fm_IsTheme");
    			
    			/// <summary>
    			/// 字段名：Fm_Content - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Fm_Content = new WeiSha.Data.Field<Forum>("Fm_Content");
    			
    			/// <summary>
    			/// 字段名：Fm_PID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Fm_PID = new WeiSha.Data.Field<Forum>("Fm_PID");
    			
    			/// <summary>
    			/// 字段名：Fm_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Fm_UID = new WeiSha.Data.Field<Forum>("Fm_UID");
    			
    			/// <summary>
    			/// 字段名：Fm_At - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Fm_At = new WeiSha.Data.Field<Forum>("Fm_At");
    			
    			/// <summary>
    			/// 字段名：Fm_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Fm_CrtTime = new WeiSha.Data.Field<Forum>("Fm_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Fm_LastTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Fm_LastTime = new WeiSha.Data.Field<Forum>("Fm_LastTime");
    			
    			/// <summary>
    			/// 字段名：Fm_IP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Fm_IP = new WeiSha.Data.Field<Forum>("Fm_IP");
    			
    			/// <summary>
    			/// 字段名：Fm_ViewNumber - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Fm_ViewNumber = new WeiSha.Data.Field<Forum>("Fm_ViewNumber");
    			
    			/// <summary>
    			/// 字段名：Fm_ReplyNumber - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Fm_ReplyNumber = new WeiSha.Data.Field<Forum>("Fm_ReplyNumber");
    			
    			/// <summary>
    			/// 字段名：Fm_IsDel - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Fm_IsDel = new WeiSha.Data.Field<Forum>("Fm_IsDel");
    			
    			/// <summary>
    			/// 字段名：Fm_IsTop - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Fm_IsTop = new WeiSha.Data.Field<Forum>("Fm_IsTop");
    			
    			/// <summary>
    			/// 字段名：Fm_IsRec - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Fm_IsRec = new WeiSha.Data.Field<Forum>("Fm_IsRec");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<Forum>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Name = new WeiSha.Data.Field<Forum>("Ac_Name");
    			
    			/// <summary>
    			/// 字段名：Th_ID - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Th_ID = new WeiSha.Data.Field<Forum>("Th_ID");
    			
    			/// <summary>
    			/// 字段名：Th_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Name = new WeiSha.Data.Field<Forum>("Th_Name");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Forum>("Org_ID");
    		}
    	}
    }
    