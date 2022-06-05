namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：MessageBoard 主键列：Mb_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class MessageBoard : WeiSha.Data.Entity {
    		
    		protected Int32 _Mb_Id;
    		
    		protected String _Mb_Title;
    		
    		protected Boolean _Mb_IsTheme;
    		
    		protected String _Mb_Content;
    		
    		protected String _Mb_Answer;
    		
    		protected Int32? _Mb_PID;
    		
    		protected String _Mb_UID;
    		
    		protected Int32? _Mb_At;
    		
    		protected DateTime? _Mb_CrtTime;
    		
    		protected DateTime? _Mb_AnsTime;
    		
    		protected Boolean _Mb_IsAns;
    		
    		protected Boolean _Mb_IsShow;
    		
    		protected Boolean _Mb_IsDel;
    		
    		protected String _Mb_IP;
    		
    		protected String _Mb_Phone;
    		
    		protected String _Mb_Email;
    		
    		protected String _Mb_QQ;
    		
    		protected Int32? _Mb_FluxNumber;
    		
    		protected Int32? _Mb_ReplyNumber;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Int32 _Cou_ID;
    		
    		protected String _Ac_Name;
    		
    		protected String _Ac_Photo;
    		
    		protected Int32 _Ac_ID;
    		
    		protected Int32 _Th_ID;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Mb_Id {
    			get {
    				return this._Mb_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_Id, _Mb_Id, value);
    				this._Mb_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Mb_Title {
    			get {
    				return this._Mb_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_Title, _Mb_Title, value);
    				this._Mb_Title = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Mb_IsTheme {
    			get {
    				return this._Mb_IsTheme;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_IsTheme, _Mb_IsTheme, value);
    				this._Mb_IsTheme = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Mb_Content {
    			get {
    				return this._Mb_Content;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_Content, _Mb_Content, value);
    				this._Mb_Content = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Mb_Answer {
    			get {
    				return this._Mb_Answer;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_Answer, _Mb_Answer, value);
    				this._Mb_Answer = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Mb_PID {
    			get {
    				return this._Mb_PID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_PID, _Mb_PID, value);
    				this._Mb_PID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Mb_UID {
    			get {
    				return this._Mb_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_UID, _Mb_UID, value);
    				this._Mb_UID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Mb_At {
    			get {
    				return this._Mb_At;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_At, _Mb_At, value);
    				this._Mb_At = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Mb_CrtTime {
    			get {
    				return this._Mb_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_CrtTime, _Mb_CrtTime, value);
    				this._Mb_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Mb_AnsTime {
    			get {
    				return this._Mb_AnsTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_AnsTime, _Mb_AnsTime, value);
    				this._Mb_AnsTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Mb_IsAns {
    			get {
    				return this._Mb_IsAns;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_IsAns, _Mb_IsAns, value);
    				this._Mb_IsAns = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Mb_IsShow {
    			get {
    				return this._Mb_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_IsShow, _Mb_IsShow, value);
    				this._Mb_IsShow = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Mb_IsDel {
    			get {
    				return this._Mb_IsDel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_IsDel, _Mb_IsDel, value);
    				this._Mb_IsDel = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Mb_IP {
    			get {
    				return this._Mb_IP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_IP, _Mb_IP, value);
    				this._Mb_IP = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Mb_Phone {
    			get {
    				return this._Mb_Phone;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_Phone, _Mb_Phone, value);
    				this._Mb_Phone = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Mb_Email {
    			get {
    				return this._Mb_Email;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_Email, _Mb_Email, value);
    				this._Mb_Email = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Mb_QQ {
    			get {
    				return this._Mb_QQ;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_QQ, _Mb_QQ, value);
    				this._Mb_QQ = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Mb_FluxNumber {
    			get {
    				return this._Mb_FluxNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_FluxNumber, _Mb_FluxNumber, value);
    				this._Mb_FluxNumber = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Mb_ReplyNumber {
    			get {
    				return this._Mb_ReplyNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mb_ReplyNumber, _Mb_ReplyNumber, value);
    				this._Mb_ReplyNumber = value;
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
    		
    		public Int32 Cou_ID {
    			get {
    				return this._Cou_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ID, _Cou_ID, value);
    				this._Cou_ID = value;
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
    		
    		public String Ac_Photo {
    			get {
    				return this._Ac_Photo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Photo, _Ac_Photo, value);
    				this._Ac_Photo = value;
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
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<MessageBoard>("MessageBoard");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Mb_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Mb_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Mb_Id,
    					_.Mb_Title,
    					_.Mb_IsTheme,
    					_.Mb_Content,
    					_.Mb_Answer,
    					_.Mb_PID,
    					_.Mb_UID,
    					_.Mb_At,
    					_.Mb_CrtTime,
    					_.Mb_AnsTime,
    					_.Mb_IsAns,
    					_.Mb_IsShow,
    					_.Mb_IsDel,
    					_.Mb_IP,
    					_.Mb_Phone,
    					_.Mb_Email,
    					_.Mb_QQ,
    					_.Mb_FluxNumber,
    					_.Mb_ReplyNumber,
    					_.Org_ID,
    					_.Org_Name,
    					_.Cou_ID,
    					_.Ac_Name,
    					_.Ac_Photo,
    					_.Ac_ID,
    					_.Th_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Mb_Id,
    					this._Mb_Title,
    					this._Mb_IsTheme,
    					this._Mb_Content,
    					this._Mb_Answer,
    					this._Mb_PID,
    					this._Mb_UID,
    					this._Mb_At,
    					this._Mb_CrtTime,
    					this._Mb_AnsTime,
    					this._Mb_IsAns,
    					this._Mb_IsShow,
    					this._Mb_IsDel,
    					this._Mb_IP,
    					this._Mb_Phone,
    					this._Mb_Email,
    					this._Mb_QQ,
    					this._Mb_FluxNumber,
    					this._Mb_ReplyNumber,
    					this._Org_ID,
    					this._Org_Name,
    					this._Cou_ID,
    					this._Ac_Name,
    					this._Ac_Photo,
    					this._Ac_ID,
    					this._Th_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Mb_Id))) {
    				this._Mb_Id = reader.GetInt32(_.Mb_Id);
    			}
    			if ((false == reader.IsDBNull(_.Mb_Title))) {
    				this._Mb_Title = reader.GetString(_.Mb_Title);
    			}
    			if ((false == reader.IsDBNull(_.Mb_IsTheme))) {
    				this._Mb_IsTheme = reader.GetBoolean(_.Mb_IsTheme);
    			}
    			if ((false == reader.IsDBNull(_.Mb_Content))) {
    				this._Mb_Content = reader.GetString(_.Mb_Content);
    			}
    			if ((false == reader.IsDBNull(_.Mb_Answer))) {
    				this._Mb_Answer = reader.GetString(_.Mb_Answer);
    			}
    			if ((false == reader.IsDBNull(_.Mb_PID))) {
    				this._Mb_PID = reader.GetInt32(_.Mb_PID);
    			}
    			if ((false == reader.IsDBNull(_.Mb_UID))) {
    				this._Mb_UID = reader.GetString(_.Mb_UID);
    			}
    			if ((false == reader.IsDBNull(_.Mb_At))) {
    				this._Mb_At = reader.GetInt32(_.Mb_At);
    			}
    			if ((false == reader.IsDBNull(_.Mb_CrtTime))) {
    				this._Mb_CrtTime = reader.GetDateTime(_.Mb_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Mb_AnsTime))) {
    				this._Mb_AnsTime = reader.GetDateTime(_.Mb_AnsTime);
    			}
    			if ((false == reader.IsDBNull(_.Mb_IsAns))) {
    				this._Mb_IsAns = reader.GetBoolean(_.Mb_IsAns);
    			}
    			if ((false == reader.IsDBNull(_.Mb_IsShow))) {
    				this._Mb_IsShow = reader.GetBoolean(_.Mb_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Mb_IsDel))) {
    				this._Mb_IsDel = reader.GetBoolean(_.Mb_IsDel);
    			}
    			if ((false == reader.IsDBNull(_.Mb_IP))) {
    				this._Mb_IP = reader.GetString(_.Mb_IP);
    			}
    			if ((false == reader.IsDBNull(_.Mb_Phone))) {
    				this._Mb_Phone = reader.GetString(_.Mb_Phone);
    			}
    			if ((false == reader.IsDBNull(_.Mb_Email))) {
    				this._Mb_Email = reader.GetString(_.Mb_Email);
    			}
    			if ((false == reader.IsDBNull(_.Mb_QQ))) {
    				this._Mb_QQ = reader.GetString(_.Mb_QQ);
    			}
    			if ((false == reader.IsDBNull(_.Mb_FluxNumber))) {
    				this._Mb_FluxNumber = reader.GetInt32(_.Mb_FluxNumber);
    			}
    			if ((false == reader.IsDBNull(_.Mb_ReplyNumber))) {
    				this._Mb_ReplyNumber = reader.GetInt32(_.Mb_ReplyNumber);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Name))) {
    				this._Ac_Name = reader.GetString(_.Ac_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Photo))) {
    				this._Ac_Photo = reader.GetString(_.Ac_Photo);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Th_ID))) {
    				this._Th_ID = reader.GetInt32(_.Th_ID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(MessageBoard).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<MessageBoard>();
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Mb_Id = new WeiSha.Data.Field<MessageBoard>("Mb_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Mb_Title = new WeiSha.Data.Field<MessageBoard>("Mb_Title");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_IsTheme - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Mb_IsTheme = new WeiSha.Data.Field<MessageBoard>("Mb_IsTheme");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_Content - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Mb_Content = new WeiSha.Data.Field<MessageBoard>("Mb_Content");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_Answer - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Mb_Answer = new WeiSha.Data.Field<MessageBoard>("Mb_Answer");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_PID - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Mb_PID = new WeiSha.Data.Field<MessageBoard>("Mb_PID");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Mb_UID = new WeiSha.Data.Field<MessageBoard>("Mb_UID");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_At - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Mb_At = new WeiSha.Data.Field<MessageBoard>("Mb_At");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Mb_CrtTime = new WeiSha.Data.Field<MessageBoard>("Mb_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_AnsTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Mb_AnsTime = new WeiSha.Data.Field<MessageBoard>("Mb_AnsTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_IsAns - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Mb_IsAns = new WeiSha.Data.Field<MessageBoard>("Mb_IsAns");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Mb_IsShow = new WeiSha.Data.Field<MessageBoard>("Mb_IsShow");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_IsDel - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Mb_IsDel = new WeiSha.Data.Field<MessageBoard>("Mb_IsDel");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_IP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Mb_IP = new WeiSha.Data.Field<MessageBoard>("Mb_IP");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_Phone - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Mb_Phone = new WeiSha.Data.Field<MessageBoard>("Mb_Phone");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_Email - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Mb_Email = new WeiSha.Data.Field<MessageBoard>("Mb_Email");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_QQ - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Mb_QQ = new WeiSha.Data.Field<MessageBoard>("Mb_QQ");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_FluxNumber - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Mb_FluxNumber = new WeiSha.Data.Field<MessageBoard>("Mb_FluxNumber");
    			
    			/// <summary>
    			/// -1 - 字段名：Mb_ReplyNumber - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Mb_ReplyNumber = new WeiSha.Data.Field<MessageBoard>("Mb_ReplyNumber");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<MessageBoard>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<MessageBoard>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<MessageBoard>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Name = new WeiSha.Data.Field<MessageBoard>("Ac_Name");
    			
    			/// <summary>
    			/// 字段名：Ac_Photo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Photo = new WeiSha.Data.Field<MessageBoard>("Ac_Photo");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<MessageBoard>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Th_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_ID = new WeiSha.Data.Field<MessageBoard>("Th_ID");
    		}
    	}
    }
    