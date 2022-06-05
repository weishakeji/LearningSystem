namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：InnerEmail 主键列：Ine_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class InnerEmail : WeiSha.Data.Entity {
    		
    		protected Int32 _Ine_Id;
    		
    		protected String _Ine_Title;
    		
    		protected String _Ine_Context;
    		
    		protected Int32? _Acc_Id;
    		
    		protected String _Acc_Name;
    		
    		protected DateTime? _Ine_CrtTime;
    		
    		protected DateTime? _Ine_ToTime;
    		
    		protected DateTime? _Ine_ReadTime;
    		
    		protected Int32? _Ine_State;
    		
    		protected String _Ine_ToName;
    		
    		protected Int32? _Ine_ToId;
    		
    		protected Int32? _Ine_OfBox;
    		
    		protected String _Ine_ToAllId;
    		
    		protected String _Ine_ToAllName;
    		
    		protected Int32? _Ine_OwnerId;
    		
    		protected Boolean _Ine_Del;
    		
    		protected String _Ine_UniqueId;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32 Ine_Id {
    			get {
    				return this._Ine_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_Id, _Ine_Id, value);
    				this._Ine_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Ine_Title {
    			get {
    				return this._Ine_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_Title, _Ine_Title, value);
    				this._Ine_Title = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Ine_Context {
    			get {
    				return this._Ine_Context;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_Context, _Ine_Context, value);
    				this._Ine_Context = value;
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
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public DateTime? Ine_CrtTime {
    			get {
    				return this._Ine_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_CrtTime, _Ine_CrtTime, value);
    				this._Ine_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public DateTime? Ine_ToTime {
    			get {
    				return this._Ine_ToTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_ToTime, _Ine_ToTime, value);
    				this._Ine_ToTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public DateTime? Ine_ReadTime {
    			get {
    				return this._Ine_ReadTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_ReadTime, _Ine_ReadTime, value);
    				this._Ine_ReadTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Ine_State {
    			get {
    				return this._Ine_State;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_State, _Ine_State, value);
    				this._Ine_State = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Ine_ToName {
    			get {
    				return this._Ine_ToName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_ToName, _Ine_ToName, value);
    				this._Ine_ToName = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Ine_ToId {
    			get {
    				return this._Ine_ToId;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_ToId, _Ine_ToId, value);
    				this._Ine_ToId = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Ine_OfBox {
    			get {
    				return this._Ine_OfBox;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_OfBox, _Ine_OfBox, value);
    				this._Ine_OfBox = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Ine_ToAllId {
    			get {
    				return this._Ine_ToAllId;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_ToAllId, _Ine_ToAllId, value);
    				this._Ine_ToAllId = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Ine_ToAllName {
    			get {
    				return this._Ine_ToAllName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_ToAllName, _Ine_ToAllName, value);
    				this._Ine_ToAllName = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Ine_OwnerId {
    			get {
    				return this._Ine_OwnerId;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_OwnerId, _Ine_OwnerId, value);
    				this._Ine_OwnerId = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Boolean Ine_Del {
    			get {
    				return this._Ine_Del;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_Del, _Ine_Del, value);
    				this._Ine_Del = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Ine_UniqueId {
    			get {
    				return this._Ine_UniqueId;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ine_UniqueId, _Ine_UniqueId, value);
    				this._Ine_UniqueId = value;
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
    			return new WeiSha.Data.Table<InnerEmail>("InnerEmail");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Ine_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ine_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ine_Id,
    					_.Ine_Title,
    					_.Ine_Context,
    					_.Acc_Id,
    					_.Acc_Name,
    					_.Ine_CrtTime,
    					_.Ine_ToTime,
    					_.Ine_ReadTime,
    					_.Ine_State,
    					_.Ine_ToName,
    					_.Ine_ToId,
    					_.Ine_OfBox,
    					_.Ine_ToAllId,
    					_.Ine_ToAllName,
    					_.Ine_OwnerId,
    					_.Ine_Del,
    					_.Ine_UniqueId,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Ine_Id,
    					this._Ine_Title,
    					this._Ine_Context,
    					this._Acc_Id,
    					this._Acc_Name,
    					this._Ine_CrtTime,
    					this._Ine_ToTime,
    					this._Ine_ReadTime,
    					this._Ine_State,
    					this._Ine_ToName,
    					this._Ine_ToId,
    					this._Ine_OfBox,
    					this._Ine_ToAllId,
    					this._Ine_ToAllName,
    					this._Ine_OwnerId,
    					this._Ine_Del,
    					this._Ine_UniqueId,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Ine_Id))) {
    				this._Ine_Id = reader.GetInt32(_.Ine_Id);
    			}
    			if ((false == reader.IsDBNull(_.Ine_Title))) {
    				this._Ine_Title = reader.GetString(_.Ine_Title);
    			}
    			if ((false == reader.IsDBNull(_.Ine_Context))) {
    				this._Ine_Context = reader.GetString(_.Ine_Context);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Name))) {
    				this._Acc_Name = reader.GetString(_.Acc_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ine_CrtTime))) {
    				this._Ine_CrtTime = reader.GetDateTime(_.Ine_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Ine_ToTime))) {
    				this._Ine_ToTime = reader.GetDateTime(_.Ine_ToTime);
    			}
    			if ((false == reader.IsDBNull(_.Ine_ReadTime))) {
    				this._Ine_ReadTime = reader.GetDateTime(_.Ine_ReadTime);
    			}
    			if ((false == reader.IsDBNull(_.Ine_State))) {
    				this._Ine_State = reader.GetInt32(_.Ine_State);
    			}
    			if ((false == reader.IsDBNull(_.Ine_ToName))) {
    				this._Ine_ToName = reader.GetString(_.Ine_ToName);
    			}
    			if ((false == reader.IsDBNull(_.Ine_ToId))) {
    				this._Ine_ToId = reader.GetInt32(_.Ine_ToId);
    			}
    			if ((false == reader.IsDBNull(_.Ine_OfBox))) {
    				this._Ine_OfBox = reader.GetInt32(_.Ine_OfBox);
    			}
    			if ((false == reader.IsDBNull(_.Ine_ToAllId))) {
    				this._Ine_ToAllId = reader.GetString(_.Ine_ToAllId);
    			}
    			if ((false == reader.IsDBNull(_.Ine_ToAllName))) {
    				this._Ine_ToAllName = reader.GetString(_.Ine_ToAllName);
    			}
    			if ((false == reader.IsDBNull(_.Ine_OwnerId))) {
    				this._Ine_OwnerId = reader.GetInt32(_.Ine_OwnerId);
    			}
    			if ((false == reader.IsDBNull(_.Ine_Del))) {
    				this._Ine_Del = reader.GetBoolean(_.Ine_Del);
    			}
    			if ((false == reader.IsDBNull(_.Ine_UniqueId))) {
    				this._Ine_UniqueId = reader.GetString(_.Ine_UniqueId);
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
    			if ((false == typeof(InnerEmail).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<InnerEmail>();
    			
    			/// <summary>
    			/// False - 字段名：Ine_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ine_Id = new WeiSha.Data.Field<InnerEmail>("Ine_Id");
    			
    			/// <summary>
    			/// True - 字段名：Ine_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ine_Title = new WeiSha.Data.Field<InnerEmail>("Ine_Title");
    			
    			/// <summary>
    			/// True - 字段名：Ine_Context - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ine_Context = new WeiSha.Data.Field<InnerEmail>("Ine_Context");
    			
    			/// <summary>
    			/// False - 字段名：Acc_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<InnerEmail>("Acc_Id");
    			
    			/// <summary>
    			/// True - 字段名：Acc_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Name = new WeiSha.Data.Field<InnerEmail>("Acc_Name");
    			
    			/// <summary>
    			/// False - 字段名：Ine_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Ine_CrtTime = new WeiSha.Data.Field<InnerEmail>("Ine_CrtTime");
    			
    			/// <summary>
    			/// False - 字段名：Ine_ToTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Ine_ToTime = new WeiSha.Data.Field<InnerEmail>("Ine_ToTime");
    			
    			/// <summary>
    			/// False - 字段名：Ine_ReadTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Ine_ReadTime = new WeiSha.Data.Field<InnerEmail>("Ine_ReadTime");
    			
    			/// <summary>
    			/// False - 字段名：Ine_State - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Ine_State = new WeiSha.Data.Field<InnerEmail>("Ine_State");
    			
    			/// <summary>
    			/// True - 字段名：Ine_ToName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ine_ToName = new WeiSha.Data.Field<InnerEmail>("Ine_ToName");
    			
    			/// <summary>
    			/// False - 字段名：Ine_ToId - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Ine_ToId = new WeiSha.Data.Field<InnerEmail>("Ine_ToId");
    			
    			/// <summary>
    			/// False - 字段名：Ine_OfBox - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Ine_OfBox = new WeiSha.Data.Field<InnerEmail>("Ine_OfBox");
    			
    			/// <summary>
    			/// True - 字段名：Ine_ToAllId - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ine_ToAllId = new WeiSha.Data.Field<InnerEmail>("Ine_ToAllId");
    			
    			/// <summary>
    			/// True - 字段名：Ine_ToAllName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ine_ToAllName = new WeiSha.Data.Field<InnerEmail>("Ine_ToAllName");
    			
    			/// <summary>
    			/// False - 字段名：Ine_OwnerId - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Ine_OwnerId = new WeiSha.Data.Field<InnerEmail>("Ine_OwnerId");
    			
    			/// <summary>
    			/// False - 字段名：Ine_Del - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ine_Del = new WeiSha.Data.Field<InnerEmail>("Ine_Del");
    			
    			/// <summary>
    			/// True - 字段名：Ine_UniqueId - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ine_UniqueId = new WeiSha.Data.Field<InnerEmail>("Ine_UniqueId");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<InnerEmail>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<InnerEmail>("Org_Name");
    		}
    	}
    }
    