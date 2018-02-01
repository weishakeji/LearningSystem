namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ManageMenu 主键列：MM_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ManageMenu : WeiSha.Data.Entity {
    		
    		protected Int32 _MM_Id;
    		
    		protected String _MM_Name;
    		
    		protected String _MM_Type;
    		
    		protected Int32 _MM_Root;
    		
    		protected String _MM_Link;
    		
    		protected String _MM_Marker;
    		
    		protected Int32 _MM_Tax;
    		
    		protected Int32 _MM_PatId;
    		
    		protected String _MM_Color;
    		
    		protected String _MM_Font;
    		
    		protected Boolean _MM_IsBold;
    		
    		protected Boolean _MM_IsItalic;
    		
    		protected String _MM_IcoS;
    		
    		protected String _MM_IcoB;
    		
    		protected Boolean _MM_IsUse;
    		
    		protected Boolean _MM_IsShow;
    		
    		protected String _MM_Intro;
    		
    		protected Boolean _MM_State;
    		
    		protected String _MM_Func;
    		
    		protected Int32 _MM_WinWidth;
    		
    		protected Int32 _MM_WinHeight;
    		
    		protected Int32 _MM_IcoX;
    		
    		protected Int32 _MM_IcoY;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 MM_Id {
    			get {
    				return this._MM_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Id, _MM_Id, value);
    				this._MM_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String MM_Name {
    			get {
    				return this._MM_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Name, _MM_Name, value);
    				this._MM_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String MM_Type {
    			get {
    				return this._MM_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Type, _MM_Type, value);
    				this._MM_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 MM_Root {
    			get {
    				return this._MM_Root;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Root, _MM_Root, value);
    				this._MM_Root = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String MM_Link {
    			get {
    				return this._MM_Link;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Link, _MM_Link, value);
    				this._MM_Link = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String MM_Marker {
    			get {
    				return this._MM_Marker;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Marker, _MM_Marker, value);
    				this._MM_Marker = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 MM_Tax {
    			get {
    				return this._MM_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Tax, _MM_Tax, value);
    				this._MM_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 MM_PatId {
    			get {
    				return this._MM_PatId;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_PatId, _MM_PatId, value);
    				this._MM_PatId = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String MM_Color {
    			get {
    				return this._MM_Color;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Color, _MM_Color, value);
    				this._MM_Color = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String MM_Font {
    			get {
    				return this._MM_Font;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Font, _MM_Font, value);
    				this._MM_Font = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean MM_IsBold {
    			get {
    				return this._MM_IsBold;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IsBold, _MM_IsBold, value);
    				this._MM_IsBold = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean MM_IsItalic {
    			get {
    				return this._MM_IsItalic;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IsItalic, _MM_IsItalic, value);
    				this._MM_IsItalic = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String MM_IcoS {
    			get {
    				return this._MM_IcoS;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IcoS, _MM_IcoS, value);
    				this._MM_IcoS = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String MM_IcoB {
    			get {
    				return this._MM_IcoB;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IcoB, _MM_IcoB, value);
    				this._MM_IcoB = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean MM_IsUse {
    			get {
    				return this._MM_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IsUse, _MM_IsUse, value);
    				this._MM_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean MM_IsShow {
    			get {
    				return this._MM_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IsShow, _MM_IsShow, value);
    				this._MM_IsShow = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String MM_Intro {
    			get {
    				return this._MM_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Intro, _MM_Intro, value);
    				this._MM_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean MM_State {
    			get {
    				return this._MM_State;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_State, _MM_State, value);
    				this._MM_State = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String MM_Func {
    			get {
    				return this._MM_Func;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Func, _MM_Func, value);
    				this._MM_Func = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 MM_WinWidth {
    			get {
    				return this._MM_WinWidth;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_WinWidth, _MM_WinWidth, value);
    				this._MM_WinWidth = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 MM_WinHeight {
    			get {
    				return this._MM_WinHeight;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_WinHeight, _MM_WinHeight, value);
    				this._MM_WinHeight = value;
    			}
    		}
    		
    		public Int32 MM_IcoX {
    			get {
    				return this._MM_IcoX;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IcoX, _MM_IcoX, value);
    				this._MM_IcoX = value;
    			}
    		}
    		
    		public Int32 MM_IcoY {
    			get {
    				return this._MM_IcoY;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IcoY, _MM_IcoY, value);
    				this._MM_IcoY = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<ManageMenu>("ManageMenu");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.MM_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.MM_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.MM_Id,
    					_.MM_Name,
    					_.MM_Type,
    					_.MM_Root,
    					_.MM_Link,
    					_.MM_Marker,
    					_.MM_Tax,
    					_.MM_PatId,
    					_.MM_Color,
    					_.MM_Font,
    					_.MM_IsBold,
    					_.MM_IsItalic,
    					_.MM_IcoS,
    					_.MM_IcoB,
    					_.MM_IsUse,
    					_.MM_IsShow,
    					_.MM_Intro,
    					_.MM_State,
    					_.MM_Func,
    					_.MM_WinWidth,
    					_.MM_WinHeight,
    					_.MM_IcoX,
    					_.MM_IcoY};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._MM_Id,
    					this._MM_Name,
    					this._MM_Type,
    					this._MM_Root,
    					this._MM_Link,
    					this._MM_Marker,
    					this._MM_Tax,
    					this._MM_PatId,
    					this._MM_Color,
    					this._MM_Font,
    					this._MM_IsBold,
    					this._MM_IsItalic,
    					this._MM_IcoS,
    					this._MM_IcoB,
    					this._MM_IsUse,
    					this._MM_IsShow,
    					this._MM_Intro,
    					this._MM_State,
    					this._MM_Func,
    					this._MM_WinWidth,
    					this._MM_WinHeight,
    					this._MM_IcoX,
    					this._MM_IcoY};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.MM_Id))) {
    				this._MM_Id = reader.GetInt32(_.MM_Id);
    			}
    			if ((false == reader.IsDBNull(_.MM_Name))) {
    				this._MM_Name = reader.GetString(_.MM_Name);
    			}
    			if ((false == reader.IsDBNull(_.MM_Type))) {
    				this._MM_Type = reader.GetString(_.MM_Type);
    			}
    			if ((false == reader.IsDBNull(_.MM_Root))) {
    				this._MM_Root = reader.GetInt32(_.MM_Root);
    			}
    			if ((false == reader.IsDBNull(_.MM_Link))) {
    				this._MM_Link = reader.GetString(_.MM_Link);
    			}
    			if ((false == reader.IsDBNull(_.MM_Marker))) {
    				this._MM_Marker = reader.GetString(_.MM_Marker);
    			}
    			if ((false == reader.IsDBNull(_.MM_Tax))) {
    				this._MM_Tax = reader.GetInt32(_.MM_Tax);
    			}
    			if ((false == reader.IsDBNull(_.MM_PatId))) {
    				this._MM_PatId = reader.GetInt32(_.MM_PatId);
    			}
    			if ((false == reader.IsDBNull(_.MM_Color))) {
    				this._MM_Color = reader.GetString(_.MM_Color);
    			}
    			if ((false == reader.IsDBNull(_.MM_Font))) {
    				this._MM_Font = reader.GetString(_.MM_Font);
    			}
    			if ((false == reader.IsDBNull(_.MM_IsBold))) {
    				this._MM_IsBold = reader.GetBoolean(_.MM_IsBold);
    			}
    			if ((false == reader.IsDBNull(_.MM_IsItalic))) {
    				this._MM_IsItalic = reader.GetBoolean(_.MM_IsItalic);
    			}
    			if ((false == reader.IsDBNull(_.MM_IcoS))) {
    				this._MM_IcoS = reader.GetString(_.MM_IcoS);
    			}
    			if ((false == reader.IsDBNull(_.MM_IcoB))) {
    				this._MM_IcoB = reader.GetString(_.MM_IcoB);
    			}
    			if ((false == reader.IsDBNull(_.MM_IsUse))) {
    				this._MM_IsUse = reader.GetBoolean(_.MM_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.MM_IsShow))) {
    				this._MM_IsShow = reader.GetBoolean(_.MM_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.MM_Intro))) {
    				this._MM_Intro = reader.GetString(_.MM_Intro);
    			}
    			if ((false == reader.IsDBNull(_.MM_State))) {
    				this._MM_State = reader.GetBoolean(_.MM_State);
    			}
    			if ((false == reader.IsDBNull(_.MM_Func))) {
    				this._MM_Func = reader.GetString(_.MM_Func);
    			}
    			if ((false == reader.IsDBNull(_.MM_WinWidth))) {
    				this._MM_WinWidth = reader.GetInt32(_.MM_WinWidth);
    			}
    			if ((false == reader.IsDBNull(_.MM_WinHeight))) {
    				this._MM_WinHeight = reader.GetInt32(_.MM_WinHeight);
    			}
    			if ((false == reader.IsDBNull(_.MM_IcoX))) {
    				this._MM_IcoX = reader.GetInt32(_.MM_IcoX);
    			}
    			if ((false == reader.IsDBNull(_.MM_IcoY))) {
    				this._MM_IcoY = reader.GetInt32(_.MM_IcoY);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(ManageMenu).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<ManageMenu>();
    			
    			/// <summary>
    			/// -1 - 字段名：MM_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_Id = new WeiSha.Data.Field<ManageMenu>("MM_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Name = new WeiSha.Data.Field<ManageMenu>("MM_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_Type - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Type = new WeiSha.Data.Field<ManageMenu>("MM_Type");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_Root - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_Root = new WeiSha.Data.Field<ManageMenu>("MM_Root");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_Link - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Link = new WeiSha.Data.Field<ManageMenu>("MM_Link");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_Marker - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Marker = new WeiSha.Data.Field<ManageMenu>("MM_Marker");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_Tax = new WeiSha.Data.Field<ManageMenu>("MM_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_PatId - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_PatId = new WeiSha.Data.Field<ManageMenu>("MM_PatId");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_Color - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Color = new WeiSha.Data.Field<ManageMenu>("MM_Color");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_Font - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Font = new WeiSha.Data.Field<ManageMenu>("MM_Font");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_IsBold - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_IsBold = new WeiSha.Data.Field<ManageMenu>("MM_IsBold");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_IsItalic - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_IsItalic = new WeiSha.Data.Field<ManageMenu>("MM_IsItalic");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_IcoS - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_IcoS = new WeiSha.Data.Field<ManageMenu>("MM_IcoS");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_IcoB - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_IcoB = new WeiSha.Data.Field<ManageMenu>("MM_IcoB");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_IsUse = new WeiSha.Data.Field<ManageMenu>("MM_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_IsShow = new WeiSha.Data.Field<ManageMenu>("MM_IsShow");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Intro = new WeiSha.Data.Field<ManageMenu>("MM_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_State - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_State = new WeiSha.Data.Field<ManageMenu>("MM_State");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_Func - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Func = new WeiSha.Data.Field<ManageMenu>("MM_Func");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_WinWidth - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_WinWidth = new WeiSha.Data.Field<ManageMenu>("MM_WinWidth");
    			
    			/// <summary>
    			/// -1 - 字段名：MM_WinHeight - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_WinHeight = new WeiSha.Data.Field<ManageMenu>("MM_WinHeight");
    			
    			/// <summary>
    			/// 字段名：MM_IcoX - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_IcoX = new WeiSha.Data.Field<ManageMenu>("MM_IcoX");
    			
    			/// <summary>
    			/// 字段名：MM_IcoY - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_IcoY = new WeiSha.Data.Field<ManageMenu>("MM_IcoY");
    		}
    	}
    }
    