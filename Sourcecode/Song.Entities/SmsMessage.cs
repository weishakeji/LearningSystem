namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：SmsMessage 主键列：SMS_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class SmsMessage : WeiSha.Data.Entity {
    		
    		protected Int32 _SMS_Id;
    		
    		protected String _Sms_Context;
    		
    		protected DateTime? _Sms_SendTime;
    		
    		protected DateTime? _Sms_CrtTime;
    		
    		protected Int32? _Sms_Type;
    		
    		protected Int32? _Sms_SendId;
    		
    		protected String _Sms_SendName;
    		
    		protected Int32? _Sms_MailBox;
    		
    		protected Int32? _Sms_State;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 SMS_Id {
    			get {
    				return this._SMS_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.SMS_Id, _SMS_Id, value);
    				this._SMS_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sms_Context {
    			get {
    				return this._Sms_Context;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sms_Context, _Sms_Context, value);
    				this._Sms_Context = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Sms_SendTime {
    			get {
    				return this._Sms_SendTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sms_SendTime, _Sms_SendTime, value);
    				this._Sms_SendTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Sms_CrtTime {
    			get {
    				return this._Sms_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sms_CrtTime, _Sms_CrtTime, value);
    				this._Sms_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Sms_Type {
    			get {
    				return this._Sms_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sms_Type, _Sms_Type, value);
    				this._Sms_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Sms_SendId {
    			get {
    				return this._Sms_SendId;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sms_SendId, _Sms_SendId, value);
    				this._Sms_SendId = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sms_SendName {
    			get {
    				return this._Sms_SendName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sms_SendName, _Sms_SendName, value);
    				this._Sms_SendName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Sms_MailBox {
    			get {
    				return this._Sms_MailBox;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sms_MailBox, _Sms_MailBox, value);
    				this._Sms_MailBox = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Sms_State {
    			get {
    				return this._Sms_State;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sms_State, _Sms_State, value);
    				this._Sms_State = value;
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
    			return new WeiSha.Data.Table<SmsMessage>("SmsMessage");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.SMS_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.SMS_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.SMS_Id,
    					_.Sms_Context,
    					_.Sms_SendTime,
    					_.Sms_CrtTime,
    					_.Sms_Type,
    					_.Sms_SendId,
    					_.Sms_SendName,
    					_.Sms_MailBox,
    					_.Sms_State,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._SMS_Id,
    					this._Sms_Context,
    					this._Sms_SendTime,
    					this._Sms_CrtTime,
    					this._Sms_Type,
    					this._Sms_SendId,
    					this._Sms_SendName,
    					this._Sms_MailBox,
    					this._Sms_State,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.SMS_Id))) {
    				this._SMS_Id = reader.GetInt32(_.SMS_Id);
    			}
    			if ((false == reader.IsDBNull(_.Sms_Context))) {
    				this._Sms_Context = reader.GetString(_.Sms_Context);
    			}
    			if ((false == reader.IsDBNull(_.Sms_SendTime))) {
    				this._Sms_SendTime = reader.GetDateTime(_.Sms_SendTime);
    			}
    			if ((false == reader.IsDBNull(_.Sms_CrtTime))) {
    				this._Sms_CrtTime = reader.GetDateTime(_.Sms_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Sms_Type))) {
    				this._Sms_Type = reader.GetInt32(_.Sms_Type);
    			}
    			if ((false == reader.IsDBNull(_.Sms_SendId))) {
    				this._Sms_SendId = reader.GetInt32(_.Sms_SendId);
    			}
    			if ((false == reader.IsDBNull(_.Sms_SendName))) {
    				this._Sms_SendName = reader.GetString(_.Sms_SendName);
    			}
    			if ((false == reader.IsDBNull(_.Sms_MailBox))) {
    				this._Sms_MailBox = reader.GetInt32(_.Sms_MailBox);
    			}
    			if ((false == reader.IsDBNull(_.Sms_State))) {
    				this._Sms_State = reader.GetInt32(_.Sms_State);
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
    			if ((false == typeof(SmsMessage).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<SmsMessage>();
    			
    			/// <summary>
    			/// -1 - 字段名：SMS_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field SMS_Id = new WeiSha.Data.Field<SmsMessage>("SMS_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Sms_Context - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sms_Context = new WeiSha.Data.Field<SmsMessage>("Sms_Context");
    			
    			/// <summary>
    			/// -1 - 字段名：Sms_SendTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Sms_SendTime = new WeiSha.Data.Field<SmsMessage>("Sms_SendTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Sms_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Sms_CrtTime = new WeiSha.Data.Field<SmsMessage>("Sms_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Sms_Type - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Sms_Type = new WeiSha.Data.Field<SmsMessage>("Sms_Type");
    			
    			/// <summary>
    			/// -1 - 字段名：Sms_SendId - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Sms_SendId = new WeiSha.Data.Field<SmsMessage>("Sms_SendId");
    			
    			/// <summary>
    			/// -1 - 字段名：Sms_SendName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sms_SendName = new WeiSha.Data.Field<SmsMessage>("Sms_SendName");
    			
    			/// <summary>
    			/// -1 - 字段名：Sms_MailBox - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Sms_MailBox = new WeiSha.Data.Field<SmsMessage>("Sms_MailBox");
    			
    			/// <summary>
    			/// -1 - 字段名：Sms_State - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Sms_State = new WeiSha.Data.Field<SmsMessage>("Sms_State");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<SmsMessage>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<SmsMessage>("Org_Name");
    		}
    	}
    }
    