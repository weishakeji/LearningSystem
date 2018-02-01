namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Navigation 主键列：Nav_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Navigation : WeiSha.Data.Entity {
    		
    		protected Int32 _Nav_ID;
    		
    		protected Int32 _Nav_PID;
    		
    		protected String _Nav_Name;
    		
    		protected String _Nav_EnName;
    		
    		protected String _Nav_Url;
    		
    		protected String _Nav_Target;
    		
    		protected String _Nav_Event;
    		
    		protected String _Nav_Image;
    		
    		protected String _Nav_Color;
    		
    		protected String _Nav_Font;
    		
    		protected Int32 _Nav_Child;
    		
    		protected Int32 _Nav_Tax;
    		
    		protected String _Nav_Intro;
    		
    		protected String _Nav_Title;
    		
    		protected String _Nav_Type;
    		
    		protected String _Nav_Site;
    		
    		protected DateTime _Nav_CrtTime;
    		
    		protected Boolean _Nav_IsShow;
    		
    		protected Boolean _Nav_IsBold;
    		
    		protected String _Nav_Logo;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Nav_ID {
    			get {
    				return this._Nav_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_ID, _Nav_ID, value);
    				this._Nav_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Nav_PID {
    			get {
    				return this._Nav_PID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_PID, _Nav_PID, value);
    				this._Nav_PID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nav_Name {
    			get {
    				return this._Nav_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_Name, _Nav_Name, value);
    				this._Nav_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nav_EnName {
    			get {
    				return this._Nav_EnName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_EnName, _Nav_EnName, value);
    				this._Nav_EnName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nav_Url {
    			get {
    				return this._Nav_Url;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_Url, _Nav_Url, value);
    				this._Nav_Url = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nav_Target {
    			get {
    				return this._Nav_Target;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_Target, _Nav_Target, value);
    				this._Nav_Target = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nav_Event {
    			get {
    				return this._Nav_Event;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_Event, _Nav_Event, value);
    				this._Nav_Event = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nav_Image {
    			get {
    				return this._Nav_Image;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_Image, _Nav_Image, value);
    				this._Nav_Image = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nav_Color {
    			get {
    				return this._Nav_Color;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_Color, _Nav_Color, value);
    				this._Nav_Color = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nav_Font {
    			get {
    				return this._Nav_Font;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_Font, _Nav_Font, value);
    				this._Nav_Font = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Nav_Child {
    			get {
    				return this._Nav_Child;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_Child, _Nav_Child, value);
    				this._Nav_Child = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Nav_Tax {
    			get {
    				return this._Nav_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_Tax, _Nav_Tax, value);
    				this._Nav_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nav_Intro {
    			get {
    				return this._Nav_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_Intro, _Nav_Intro, value);
    				this._Nav_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nav_Title {
    			get {
    				return this._Nav_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_Title, _Nav_Title, value);
    				this._Nav_Title = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nav_Type {
    			get {
    				return this._Nav_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_Type, _Nav_Type, value);
    				this._Nav_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nav_Site {
    			get {
    				return this._Nav_Site;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_Site, _Nav_Site, value);
    				this._Nav_Site = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime Nav_CrtTime {
    			get {
    				return this._Nav_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_CrtTime, _Nav_CrtTime, value);
    				this._Nav_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Nav_IsShow {
    			get {
    				return this._Nav_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_IsShow, _Nav_IsShow, value);
    				this._Nav_IsShow = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Nav_IsBold {
    			get {
    				return this._Nav_IsBold;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_IsBold, _Nav_IsBold, value);
    				this._Nav_IsBold = value;
    			}
    		}
    		
    		public String Nav_Logo {
    			get {
    				return this._Nav_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nav_Logo, _Nav_Logo, value);
    				this._Nav_Logo = value;
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
    			return new WeiSha.Data.Table<Navigation>("Navigation");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Nav_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Nav_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Nav_ID,
    					_.Nav_PID,
    					_.Nav_Name,
    					_.Nav_EnName,
    					_.Nav_Url,
    					_.Nav_Target,
    					_.Nav_Event,
    					_.Nav_Image,
    					_.Nav_Color,
    					_.Nav_Font,
    					_.Nav_Child,
    					_.Nav_Tax,
    					_.Nav_Intro,
    					_.Nav_Title,
    					_.Nav_Type,
    					_.Nav_Site,
    					_.Nav_CrtTime,
    					_.Nav_IsShow,
    					_.Nav_IsBold,
    					_.Nav_Logo,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Nav_ID,
    					this._Nav_PID,
    					this._Nav_Name,
    					this._Nav_EnName,
    					this._Nav_Url,
    					this._Nav_Target,
    					this._Nav_Event,
    					this._Nav_Image,
    					this._Nav_Color,
    					this._Nav_Font,
    					this._Nav_Child,
    					this._Nav_Tax,
    					this._Nav_Intro,
    					this._Nav_Title,
    					this._Nav_Type,
    					this._Nav_Site,
    					this._Nav_CrtTime,
    					this._Nav_IsShow,
    					this._Nav_IsBold,
    					this._Nav_Logo,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Nav_ID))) {
    				this._Nav_ID = reader.GetInt32(_.Nav_ID);
    			}
    			if ((false == reader.IsDBNull(_.Nav_PID))) {
    				this._Nav_PID = reader.GetInt32(_.Nav_PID);
    			}
    			if ((false == reader.IsDBNull(_.Nav_Name))) {
    				this._Nav_Name = reader.GetString(_.Nav_Name);
    			}
    			if ((false == reader.IsDBNull(_.Nav_EnName))) {
    				this._Nav_EnName = reader.GetString(_.Nav_EnName);
    			}
    			if ((false == reader.IsDBNull(_.Nav_Url))) {
    				this._Nav_Url = reader.GetString(_.Nav_Url);
    			}
    			if ((false == reader.IsDBNull(_.Nav_Target))) {
    				this._Nav_Target = reader.GetString(_.Nav_Target);
    			}
    			if ((false == reader.IsDBNull(_.Nav_Event))) {
    				this._Nav_Event = reader.GetString(_.Nav_Event);
    			}
    			if ((false == reader.IsDBNull(_.Nav_Image))) {
    				this._Nav_Image = reader.GetString(_.Nav_Image);
    			}
    			if ((false == reader.IsDBNull(_.Nav_Color))) {
    				this._Nav_Color = reader.GetString(_.Nav_Color);
    			}
    			if ((false == reader.IsDBNull(_.Nav_Font))) {
    				this._Nav_Font = reader.GetString(_.Nav_Font);
    			}
    			if ((false == reader.IsDBNull(_.Nav_Child))) {
    				this._Nav_Child = reader.GetInt32(_.Nav_Child);
    			}
    			if ((false == reader.IsDBNull(_.Nav_Tax))) {
    				this._Nav_Tax = reader.GetInt32(_.Nav_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Nav_Intro))) {
    				this._Nav_Intro = reader.GetString(_.Nav_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Nav_Title))) {
    				this._Nav_Title = reader.GetString(_.Nav_Title);
    			}
    			if ((false == reader.IsDBNull(_.Nav_Type))) {
    				this._Nav_Type = reader.GetString(_.Nav_Type);
    			}
    			if ((false == reader.IsDBNull(_.Nav_Site))) {
    				this._Nav_Site = reader.GetString(_.Nav_Site);
    			}
    			if ((false == reader.IsDBNull(_.Nav_CrtTime))) {
    				this._Nav_CrtTime = reader.GetDateTime(_.Nav_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Nav_IsShow))) {
    				this._Nav_IsShow = reader.GetBoolean(_.Nav_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Nav_IsBold))) {
    				this._Nav_IsBold = reader.GetBoolean(_.Nav_IsBold);
    			}
    			if ((false == reader.IsDBNull(_.Nav_Logo))) {
    				this._Nav_Logo = reader.GetString(_.Nav_Logo);
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
    			if ((false == typeof(Navigation).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Navigation>();
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Nav_ID = new WeiSha.Data.Field<Navigation>("Nav_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_PID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Nav_PID = new WeiSha.Data.Field<Navigation>("Nav_PID");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nav_Name = new WeiSha.Data.Field<Navigation>("Nav_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_EnName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nav_EnName = new WeiSha.Data.Field<Navigation>("Nav_EnName");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_Url - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nav_Url = new WeiSha.Data.Field<Navigation>("Nav_Url");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_Target - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nav_Target = new WeiSha.Data.Field<Navigation>("Nav_Target");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_Event - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nav_Event = new WeiSha.Data.Field<Navigation>("Nav_Event");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_Image - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nav_Image = new WeiSha.Data.Field<Navigation>("Nav_Image");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_Color - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nav_Color = new WeiSha.Data.Field<Navigation>("Nav_Color");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_Font - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nav_Font = new WeiSha.Data.Field<Navigation>("Nav_Font");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_Child - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Nav_Child = new WeiSha.Data.Field<Navigation>("Nav_Child");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Nav_Tax = new WeiSha.Data.Field<Navigation>("Nav_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nav_Intro = new WeiSha.Data.Field<Navigation>("Nav_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nav_Title = new WeiSha.Data.Field<Navigation>("Nav_Title");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_Type - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nav_Type = new WeiSha.Data.Field<Navigation>("Nav_Type");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_Site - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nav_Site = new WeiSha.Data.Field<Navigation>("Nav_Site");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Nav_CrtTime = new WeiSha.Data.Field<Navigation>("Nav_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Nav_IsShow = new WeiSha.Data.Field<Navigation>("Nav_IsShow");
    			
    			/// <summary>
    			/// -1 - 字段名：Nav_IsBold - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Nav_IsBold = new WeiSha.Data.Field<Navigation>("Nav_IsBold");
    			
    			/// <summary>
    			/// 字段名：Nav_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nav_Logo = new WeiSha.Data.Field<Navigation>("Nav_Logo");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Navigation>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Navigation>("Org_Name");
    		}
    	}
    }
    