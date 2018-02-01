namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：OutlineEvent 主键列：Oe_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class OutlineEvent : WeiSha.Data.Entity {
    		
    		protected Int32 _Oe_ID;
    		
    		protected Int32 _Ol_ID;
    		
    		protected String _Ol_UID;
    		
    		protected String _Oe_Title;
    		
    		protected Int32 _Oe_Width;
    		
    		protected Int32 _Oe_Height;
    		
    		protected Boolean _Oe_IsUse;
    		
    		protected Int32 _Oe_EventType;
    		
    		protected Int32 _Oe_TriggerPoint;
    		
    		protected String _Oe_Context;
    		
    		protected String _Oe_Datatable;
    		
    		protected Int32 _Oe_Questype;
    		
    		protected String _Oe_Answer;
    		
    		protected Int32 _Cou_ID;
    		
    		protected Int32 _Org_ID;
    		
    		protected DateTime _Oe_CrtTime;
    		
    		public Int32 Oe_ID {
    			get {
    				return this._Oe_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Oe_ID, _Oe_ID, value);
    				this._Oe_ID = value;
    			}
    		}
    		
    		public Int32 Ol_ID {
    			get {
    				return this._Ol_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_ID, _Ol_ID, value);
    				this._Ol_ID = value;
    			}
    		}
    		
    		public String Ol_UID {
    			get {
    				return this._Ol_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_UID, _Ol_UID, value);
    				this._Ol_UID = value;
    			}
    		}
    		
    		public String Oe_Title {
    			get {
    				return this._Oe_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Oe_Title, _Oe_Title, value);
    				this._Oe_Title = value;
    			}
    		}
    		
    		public Int32 Oe_Width {
    			get {
    				return this._Oe_Width;
    			}
    			set {
    				this.OnPropertyValueChange(_.Oe_Width, _Oe_Width, value);
    				this._Oe_Width = value;
    			}
    		}
    		
    		public Int32 Oe_Height {
    			get {
    				return this._Oe_Height;
    			}
    			set {
    				this.OnPropertyValueChange(_.Oe_Height, _Oe_Height, value);
    				this._Oe_Height = value;
    			}
    		}
    		
    		public Boolean Oe_IsUse {
    			get {
    				return this._Oe_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Oe_IsUse, _Oe_IsUse, value);
    				this._Oe_IsUse = value;
    			}
    		}
    		
    		public Int32 Oe_EventType {
    			get {
    				return this._Oe_EventType;
    			}
    			set {
    				this.OnPropertyValueChange(_.Oe_EventType, _Oe_EventType, value);
    				this._Oe_EventType = value;
    			}
    		}
    		
    		public Int32 Oe_TriggerPoint {
    			get {
    				return this._Oe_TriggerPoint;
    			}
    			set {
    				this.OnPropertyValueChange(_.Oe_TriggerPoint, _Oe_TriggerPoint, value);
    				this._Oe_TriggerPoint = value;
    			}
    		}
    		
    		public String Oe_Context {
    			get {
    				return this._Oe_Context;
    			}
    			set {
    				this.OnPropertyValueChange(_.Oe_Context, _Oe_Context, value);
    				this._Oe_Context = value;
    			}
    		}
    		
    		public String Oe_Datatable {
    			get {
    				return this._Oe_Datatable;
    			}
    			set {
    				this.OnPropertyValueChange(_.Oe_Datatable, _Oe_Datatable, value);
    				this._Oe_Datatable = value;
    			}
    		}
    		
    		public Int32 Oe_Questype {
    			get {
    				return this._Oe_Questype;
    			}
    			set {
    				this.OnPropertyValueChange(_.Oe_Questype, _Oe_Questype, value);
    				this._Oe_Questype = value;
    			}
    		}
    		
    		public String Oe_Answer {
    			get {
    				return this._Oe_Answer;
    			}
    			set {
    				this.OnPropertyValueChange(_.Oe_Answer, _Oe_Answer, value);
    				this._Oe_Answer = value;
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
    		
    		public Int32 Org_ID {
    			get {
    				return this._Org_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_ID, _Org_ID, value);
    				this._Org_ID = value;
    			}
    		}
    		
    		public DateTime Oe_CrtTime {
    			get {
    				return this._Oe_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Oe_CrtTime, _Oe_CrtTime, value);
    				this._Oe_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<OutlineEvent>("OutlineEvent");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Oe_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Oe_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Oe_ID,
    					_.Ol_ID,
    					_.Ol_UID,
    					_.Oe_Title,
    					_.Oe_Width,
    					_.Oe_Height,
    					_.Oe_IsUse,
    					_.Oe_EventType,
    					_.Oe_TriggerPoint,
    					_.Oe_Context,
    					_.Oe_Datatable,
    					_.Oe_Questype,
    					_.Oe_Answer,
    					_.Cou_ID,
    					_.Org_ID,
    					_.Oe_CrtTime};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Oe_ID,
    					this._Ol_ID,
    					this._Ol_UID,
    					this._Oe_Title,
    					this._Oe_Width,
    					this._Oe_Height,
    					this._Oe_IsUse,
    					this._Oe_EventType,
    					this._Oe_TriggerPoint,
    					this._Oe_Context,
    					this._Oe_Datatable,
    					this._Oe_Questype,
    					this._Oe_Answer,
    					this._Cou_ID,
    					this._Org_ID,
    					this._Oe_CrtTime};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Oe_ID))) {
    				this._Oe_ID = reader.GetInt32(_.Oe_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ol_ID))) {
    				this._Ol_ID = reader.GetInt32(_.Ol_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ol_UID))) {
    				this._Ol_UID = reader.GetString(_.Ol_UID);
    			}
    			if ((false == reader.IsDBNull(_.Oe_Title))) {
    				this._Oe_Title = reader.GetString(_.Oe_Title);
    			}
    			if ((false == reader.IsDBNull(_.Oe_Width))) {
    				this._Oe_Width = reader.GetInt32(_.Oe_Width);
    			}
    			if ((false == reader.IsDBNull(_.Oe_Height))) {
    				this._Oe_Height = reader.GetInt32(_.Oe_Height);
    			}
    			if ((false == reader.IsDBNull(_.Oe_IsUse))) {
    				this._Oe_IsUse = reader.GetBoolean(_.Oe_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Oe_EventType))) {
    				this._Oe_EventType = reader.GetInt32(_.Oe_EventType);
    			}
    			if ((false == reader.IsDBNull(_.Oe_TriggerPoint))) {
    				this._Oe_TriggerPoint = reader.GetInt32(_.Oe_TriggerPoint);
    			}
    			if ((false == reader.IsDBNull(_.Oe_Context))) {
    				this._Oe_Context = reader.GetString(_.Oe_Context);
    			}
    			if ((false == reader.IsDBNull(_.Oe_Datatable))) {
    				this._Oe_Datatable = reader.GetString(_.Oe_Datatable);
    			}
    			if ((false == reader.IsDBNull(_.Oe_Questype))) {
    				this._Oe_Questype = reader.GetInt32(_.Oe_Questype);
    			}
    			if ((false == reader.IsDBNull(_.Oe_Answer))) {
    				this._Oe_Answer = reader.GetString(_.Oe_Answer);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Oe_CrtTime))) {
    				this._Oe_CrtTime = reader.GetDateTime(_.Oe_CrtTime);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(OutlineEvent).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<OutlineEvent>();
    			
    			/// <summary>
    			/// 字段名：Oe_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Oe_ID = new WeiSha.Data.Field<OutlineEvent>("Oe_ID");
    			
    			/// <summary>
    			/// 字段名：Ol_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ol_ID = new WeiSha.Data.Field<OutlineEvent>("Ol_ID");
    			
    			/// <summary>
    			/// 字段名：Ol_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ol_UID = new WeiSha.Data.Field<OutlineEvent>("Ol_UID");
    			
    			/// <summary>
    			/// 字段名：Oe_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Oe_Title = new WeiSha.Data.Field<OutlineEvent>("Oe_Title");
    			
    			/// <summary>
    			/// 字段名：Oe_Width - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Oe_Width = new WeiSha.Data.Field<OutlineEvent>("Oe_Width");
    			
    			/// <summary>
    			/// 字段名：Oe_Height - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Oe_Height = new WeiSha.Data.Field<OutlineEvent>("Oe_Height");
    			
    			/// <summary>
    			/// 字段名：Oe_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Oe_IsUse = new WeiSha.Data.Field<OutlineEvent>("Oe_IsUse");
    			
    			/// <summary>
    			/// 字段名：Oe_EventType - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Oe_EventType = new WeiSha.Data.Field<OutlineEvent>("Oe_EventType");
    			
    			/// <summary>
    			/// 字段名：Oe_TriggerPoint - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Oe_TriggerPoint = new WeiSha.Data.Field<OutlineEvent>("Oe_TriggerPoint");
    			
    			/// <summary>
    			/// 字段名：Oe_Context - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Oe_Context = new WeiSha.Data.Field<OutlineEvent>("Oe_Context");
    			
    			/// <summary>
    			/// 字段名：Oe_Datatable - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Oe_Datatable = new WeiSha.Data.Field<OutlineEvent>("Oe_Datatable");
    			
    			/// <summary>
    			/// 字段名：Oe_Questype - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Oe_Questype = new WeiSha.Data.Field<OutlineEvent>("Oe_Questype");
    			
    			/// <summary>
    			/// 字段名：Oe_Answer - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Oe_Answer = new WeiSha.Data.Field<OutlineEvent>("Oe_Answer");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<OutlineEvent>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<OutlineEvent>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Oe_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Oe_CrtTime = new WeiSha.Data.Field<OutlineEvent>("Oe_CrtTime");
    		}
    	}
    }
    