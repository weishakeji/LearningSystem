namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Message 主键列：Msg_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Message : WeiSha.Data.Entity {
    		
    		protected Int32 _Msg_Id;
    		
    		protected String _Msg_Title;
    		
    		protected String _Msg_Context;
    		
    		protected DateTime? _Msg_CrtTime;
    		
    		protected DateTime? _Msg_ToTime;
    		
    		protected DateTime? _Msg_ReadTime;
    		
    		protected Int32? _Msg_State;
    		
    		protected Int32? _Msg_ToId;
    		
    		protected Int32? _Msg_OfBox;
    		
    		protected String _Msg_ToAllId;
    		
    		protected String _Msg_ToAllName;
    		
    		protected Int32? _Msg_OwnerId;
    		
    		protected Boolean _Msg_Del;
    		
    		protected Int32? _Org_Id;
    		
    		protected String _Org_Name;
    		
    		protected Int32 _Ac_ID;
    		
    		protected String _Msg_QQ;
    		
    		protected String _Msg_Phone;
    		
    		protected String _Msg_ReContext;
    		
    		protected Boolean _Msg_IsReply;
    		
    		protected Int64 _Cou_ID;
    		
    		protected Int64 _Ol_ID;
    		
    		protected String _Msg_IP;
    		
    		protected String _Ac_AccName;
    		
    		protected Int32 _Msg_PlayTime;
    		
    		protected Int32 _Msg_Likenum;
    		
    		protected String _Ac_Name;
    		
    		protected String _Ac_Photo;
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32 Msg_Id {
    			get {
    				return this._Msg_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_Id, _Msg_Id, value);
    				this._Msg_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Msg_Title {
    			get {
    				return this._Msg_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_Title, _Msg_Title, value);
    				this._Msg_Title = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Msg_Context {
    			get {
    				return this._Msg_Context;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_Context, _Msg_Context, value);
    				this._Msg_Context = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public DateTime? Msg_CrtTime {
    			get {
    				return this._Msg_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_CrtTime, _Msg_CrtTime, value);
    				this._Msg_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public DateTime? Msg_ToTime {
    			get {
    				return this._Msg_ToTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_ToTime, _Msg_ToTime, value);
    				this._Msg_ToTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public DateTime? Msg_ReadTime {
    			get {
    				return this._Msg_ReadTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_ReadTime, _Msg_ReadTime, value);
    				this._Msg_ReadTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Msg_State {
    			get {
    				return this._Msg_State;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_State, _Msg_State, value);
    				this._Msg_State = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Msg_ToId {
    			get {
    				return this._Msg_ToId;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_ToId, _Msg_ToId, value);
    				this._Msg_ToId = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Msg_OfBox {
    			get {
    				return this._Msg_OfBox;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_OfBox, _Msg_OfBox, value);
    				this._Msg_OfBox = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Msg_ToAllId {
    			get {
    				return this._Msg_ToAllId;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_ToAllId, _Msg_ToAllId, value);
    				this._Msg_ToAllId = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Msg_ToAllName {
    			get {
    				return this._Msg_ToAllName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_ToAllName, _Msg_ToAllName, value);
    				this._Msg_ToAllName = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Msg_OwnerId {
    			get {
    				return this._Msg_OwnerId;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_OwnerId, _Msg_OwnerId, value);
    				this._Msg_OwnerId = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Boolean Msg_Del {
    			get {
    				return this._Msg_Del;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_Del, _Msg_Del, value);
    				this._Msg_Del = value;
    			}
    		}
    		
    		public Int32? Org_Id {
    			get {
    				return this._Org_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Id, _Org_Id, value);
    				this._Org_Id = value;
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
    		
    		public Int32 Ac_ID {
    			get {
    				return this._Ac_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_ID, _Ac_ID, value);
    				this._Ac_ID = value;
    			}
    		}
    		
    		public String Msg_QQ {
    			get {
    				return this._Msg_QQ;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_QQ, _Msg_QQ, value);
    				this._Msg_QQ = value;
    			}
    		}
    		
    		public String Msg_Phone {
    			get {
    				return this._Msg_Phone;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_Phone, _Msg_Phone, value);
    				this._Msg_Phone = value;
    			}
    		}
    		
    		public String Msg_ReContext {
    			get {
    				return this._Msg_ReContext;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_ReContext, _Msg_ReContext, value);
    				this._Msg_ReContext = value;
    			}
    		}
    		
    		public Boolean Msg_IsReply {
    			get {
    				return this._Msg_IsReply;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_IsReply, _Msg_IsReply, value);
    				this._Msg_IsReply = value;
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
    		
    		public String Msg_IP {
    			get {
    				return this._Msg_IP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_IP, _Msg_IP, value);
    				this._Msg_IP = value;
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
    		
    		public Int32 Msg_PlayTime {
    			get {
    				return this._Msg_PlayTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_PlayTime, _Msg_PlayTime, value);
    				this._Msg_PlayTime = value;
    			}
    		}
    		
    		public Int32 Msg_Likenum {
    			get {
    				return this._Msg_Likenum;
    			}
    			set {
    				this.OnPropertyValueChange(_.Msg_Likenum, _Msg_Likenum, value);
    				this._Msg_Likenum = value;
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
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Message>("Message");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Msg_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Msg_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Msg_Id,
    					_.Msg_Title,
    					_.Msg_Context,
    					_.Msg_CrtTime,
    					_.Msg_ToTime,
    					_.Msg_ReadTime,
    					_.Msg_State,
    					_.Msg_ToId,
    					_.Msg_OfBox,
    					_.Msg_ToAllId,
    					_.Msg_ToAllName,
    					_.Msg_OwnerId,
    					_.Msg_Del,
    					_.Org_Id,
    					_.Org_Name,
    					_.Ac_ID,
    					_.Msg_QQ,
    					_.Msg_Phone,
    					_.Msg_ReContext,
    					_.Msg_IsReply,
    					_.Cou_ID,
    					_.Ol_ID,
    					_.Msg_IP,
    					_.Ac_AccName,
    					_.Msg_PlayTime,
    					_.Msg_Likenum,
    					_.Ac_Name,
    					_.Ac_Photo};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Msg_Id,
    					this._Msg_Title,
    					this._Msg_Context,
    					this._Msg_CrtTime,
    					this._Msg_ToTime,
    					this._Msg_ReadTime,
    					this._Msg_State,
    					this._Msg_ToId,
    					this._Msg_OfBox,
    					this._Msg_ToAllId,
    					this._Msg_ToAllName,
    					this._Msg_OwnerId,
    					this._Msg_Del,
    					this._Org_Id,
    					this._Org_Name,
    					this._Ac_ID,
    					this._Msg_QQ,
    					this._Msg_Phone,
    					this._Msg_ReContext,
    					this._Msg_IsReply,
    					this._Cou_ID,
    					this._Ol_ID,
    					this._Msg_IP,
    					this._Ac_AccName,
    					this._Msg_PlayTime,
    					this._Msg_Likenum,
    					this._Ac_Name,
    					this._Ac_Photo};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Msg_Id))) {
    				this._Msg_Id = reader.GetInt32(_.Msg_Id);
    			}
    			if ((false == reader.IsDBNull(_.Msg_Title))) {
    				this._Msg_Title = reader.GetString(_.Msg_Title);
    			}
    			if ((false == reader.IsDBNull(_.Msg_Context))) {
    				this._Msg_Context = reader.GetString(_.Msg_Context);
    			}
    			if ((false == reader.IsDBNull(_.Msg_CrtTime))) {
    				this._Msg_CrtTime = reader.GetDateTime(_.Msg_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Msg_ToTime))) {
    				this._Msg_ToTime = reader.GetDateTime(_.Msg_ToTime);
    			}
    			if ((false == reader.IsDBNull(_.Msg_ReadTime))) {
    				this._Msg_ReadTime = reader.GetDateTime(_.Msg_ReadTime);
    			}
    			if ((false == reader.IsDBNull(_.Msg_State))) {
    				this._Msg_State = reader.GetInt32(_.Msg_State);
    			}
    			if ((false == reader.IsDBNull(_.Msg_ToId))) {
    				this._Msg_ToId = reader.GetInt32(_.Msg_ToId);
    			}
    			if ((false == reader.IsDBNull(_.Msg_OfBox))) {
    				this._Msg_OfBox = reader.GetInt32(_.Msg_OfBox);
    			}
    			if ((false == reader.IsDBNull(_.Msg_ToAllId))) {
    				this._Msg_ToAllId = reader.GetString(_.Msg_ToAllId);
    			}
    			if ((false == reader.IsDBNull(_.Msg_ToAllName))) {
    				this._Msg_ToAllName = reader.GetString(_.Msg_ToAllName);
    			}
    			if ((false == reader.IsDBNull(_.Msg_OwnerId))) {
    				this._Msg_OwnerId = reader.GetInt32(_.Msg_OwnerId);
    			}
    			if ((false == reader.IsDBNull(_.Msg_Del))) {
    				this._Msg_Del = reader.GetBoolean(_.Msg_Del);
    			}
    			if ((false == reader.IsDBNull(_.Org_Id))) {
    				this._Org_Id = reader.GetInt32(_.Org_Id);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Msg_QQ))) {
    				this._Msg_QQ = reader.GetString(_.Msg_QQ);
    			}
    			if ((false == reader.IsDBNull(_.Msg_Phone))) {
    				this._Msg_Phone = reader.GetString(_.Msg_Phone);
    			}
    			if ((false == reader.IsDBNull(_.Msg_ReContext))) {
    				this._Msg_ReContext = reader.GetString(_.Msg_ReContext);
    			}
    			if ((false == reader.IsDBNull(_.Msg_IsReply))) {
    				this._Msg_IsReply = reader.GetBoolean(_.Msg_IsReply);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt64(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ol_ID))) {
    				this._Ol_ID = reader.GetInt64(_.Ol_ID);
    			}
    			if ((false == reader.IsDBNull(_.Msg_IP))) {
    				this._Msg_IP = reader.GetString(_.Msg_IP);
    			}
    			if ((false == reader.IsDBNull(_.Ac_AccName))) {
    				this._Ac_AccName = reader.GetString(_.Ac_AccName);
    			}
    			if ((false == reader.IsDBNull(_.Msg_PlayTime))) {
    				this._Msg_PlayTime = reader.GetInt32(_.Msg_PlayTime);
    			}
    			if ((false == reader.IsDBNull(_.Msg_Likenum))) {
    				this._Msg_Likenum = reader.GetInt32(_.Msg_Likenum);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Name))) {
    				this._Ac_Name = reader.GetString(_.Ac_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Photo))) {
    				this._Ac_Photo = reader.GetString(_.Ac_Photo);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Message).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Message>();
    			
    			/// <summary>
    			/// False - 字段名：Msg_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Msg_Id = new WeiSha.Data.Field<Message>("Msg_Id");
    			
    			/// <summary>
    			/// True - 字段名：Msg_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Msg_Title = new WeiSha.Data.Field<Message>("Msg_Title");
    			
    			/// <summary>
    			/// True - 字段名：Msg_Context - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Msg_Context = new WeiSha.Data.Field<Message>("Msg_Context");
    			
    			/// <summary>
    			/// False - 字段名：Msg_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Msg_CrtTime = new WeiSha.Data.Field<Message>("Msg_CrtTime");
    			
    			/// <summary>
    			/// False - 字段名：Msg_ToTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Msg_ToTime = new WeiSha.Data.Field<Message>("Msg_ToTime");
    			
    			/// <summary>
    			/// False - 字段名：Msg_ReadTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Msg_ReadTime = new WeiSha.Data.Field<Message>("Msg_ReadTime");
    			
    			/// <summary>
    			/// False - 字段名：Msg_State - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Msg_State = new WeiSha.Data.Field<Message>("Msg_State");
    			
    			/// <summary>
    			/// False - 字段名：Msg_ToId - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Msg_ToId = new WeiSha.Data.Field<Message>("Msg_ToId");
    			
    			/// <summary>
    			/// False - 字段名：Msg_OfBox - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Msg_OfBox = new WeiSha.Data.Field<Message>("Msg_OfBox");
    			
    			/// <summary>
    			/// True - 字段名：Msg_ToAllId - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Msg_ToAllId = new WeiSha.Data.Field<Message>("Msg_ToAllId");
    			
    			/// <summary>
    			/// True - 字段名：Msg_ToAllName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Msg_ToAllName = new WeiSha.Data.Field<Message>("Msg_ToAllName");
    			
    			/// <summary>
    			/// False - 字段名：Msg_OwnerId - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Msg_OwnerId = new WeiSha.Data.Field<Message>("Msg_OwnerId");
    			
    			/// <summary>
    			/// False - 字段名：Msg_Del - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Msg_Del = new WeiSha.Data.Field<Message>("Msg_Del");
    			
    			/// <summary>
    			/// 字段名：Org_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Org_Id = new WeiSha.Data.Field<Message>("Org_Id");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Message>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<Message>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Msg_QQ - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Msg_QQ = new WeiSha.Data.Field<Message>("Msg_QQ");
    			
    			/// <summary>
    			/// 字段名：Msg_Phone - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Msg_Phone = new WeiSha.Data.Field<Message>("Msg_Phone");
    			
    			/// <summary>
    			/// 字段名：Msg_ReContext - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Msg_ReContext = new WeiSha.Data.Field<Message>("Msg_ReContext");
    			
    			/// <summary>
    			/// 字段名：Msg_IsReply - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Msg_IsReply = new WeiSha.Data.Field<Message>("Msg_IsReply");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<Message>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Ol_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Ol_ID = new WeiSha.Data.Field<Message>("Ol_ID");
    			
    			/// <summary>
    			/// 字段名：Msg_IP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Msg_IP = new WeiSha.Data.Field<Message>("Msg_IP");
    			
    			/// <summary>
    			/// 字段名：Ac_AccName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_AccName = new WeiSha.Data.Field<Message>("Ac_AccName");
    			
    			/// <summary>
    			/// 字段名：Msg_PlayTime - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Msg_PlayTime = new WeiSha.Data.Field<Message>("Msg_PlayTime");
    			
    			/// <summary>
    			/// 字段名：Msg_Likenum - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Msg_Likenum = new WeiSha.Data.Field<Message>("Msg_Likenum");
    			
    			/// <summary>
    			/// 字段名：Ac_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Name = new WeiSha.Data.Field<Message>("Ac_Name");
    			
    			/// <summary>
    			/// 字段名：Ac_Photo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Photo = new WeiSha.Data.Field<Message>("Ac_Photo");
    		}
    	}
    }
    