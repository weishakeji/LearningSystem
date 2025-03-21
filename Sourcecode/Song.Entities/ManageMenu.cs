namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ManageMenu 主键列：MM_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ManageMenu : WeiSha.Data.Entity {
    		
    		protected Int32 _MM_Id;
    		
    		protected String _MM_AbbrName;
    		
    		protected String _MM_Color;
    		
    		protected Int32 _MM_Complete;
    		
    		protected String _MM_Font;
    		
    		protected String _MM_Func;
    		
    		protected String _MM_Help;
    		
    		protected String _MM_IcoCode;
    		
    		protected String _MM_IcoColor;
    		
    		protected Int32 _MM_IcoSize;
    		
    		protected Int32 _MM_IcoX;
    		
    		protected Int32 _MM_IcoY;
    		
    		protected String _MM_Intro;
    		
    		protected Boolean _MM_IsBold;
    		
    		protected Boolean _MM_IsChilds;
    		
    		protected Boolean _MM_IsFixed;
    		
    		protected Boolean _MM_IsItalic;
    		
    		protected Boolean _MM_IsShow;
    		
    		protected Boolean _MM_IsUse;
    		
    		protected String _MM_Link;
    		
    		protected String _MM_Marker;
    		
    		protected String _MM_Name;
    		
    		protected String _MM_PatId;
    		
    		protected Int32 _MM_Root;
    		
    		protected Int32 _MM_Tax;
    		
    		protected String _MM_Type;
    		
    		protected String _MM_UID;
    		
    		protected Int32 _MM_WinHeight;
    		
    		protected String _MM_WinID;
    		
    		protected Boolean _MM_WinMax;
    		
    		protected Boolean _MM_WinMin;
    		
    		protected Boolean _MM_WinMove;
    		
    		protected Boolean _MM_WinResize;
    		
    		protected Int32 _MM_WinWidth;
    		
    		public Int32 MM_Id {
    			get {
    				return this._MM_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Id, _MM_Id, value);
    				this._MM_Id = value;
    			}
    		}
    		
    		public String MM_AbbrName {
    			get {
    				return this._MM_AbbrName;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_AbbrName, _MM_AbbrName, value);
    				this._MM_AbbrName = value;
    			}
    		}
    		
    		public String MM_Color {
    			get {
    				return this._MM_Color;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Color, _MM_Color, value);
    				this._MM_Color = value;
    			}
    		}
    		
    		public Int32 MM_Complete {
    			get {
    				return this._MM_Complete;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Complete, _MM_Complete, value);
    				this._MM_Complete = value;
    			}
    		}
    		
    		public String MM_Font {
    			get {
    				return this._MM_Font;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Font, _MM_Font, value);
    				this._MM_Font = value;
    			}
    		}
    		
    		public String MM_Func {
    			get {
    				return this._MM_Func;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Func, _MM_Func, value);
    				this._MM_Func = value;
    			}
    		}
    		
    		public String MM_Help {
    			get {
    				return this._MM_Help;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Help, _MM_Help, value);
    				this._MM_Help = value;
    			}
    		}
    		
    		public String MM_IcoCode {
    			get {
    				return this._MM_IcoCode;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IcoCode, _MM_IcoCode, value);
    				this._MM_IcoCode = value;
    			}
    		}
    		
    		public String MM_IcoColor {
    			get {
    				return this._MM_IcoColor;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IcoColor, _MM_IcoColor, value);
    				this._MM_IcoColor = value;
    			}
    		}
    		
    		public Int32 MM_IcoSize {
    			get {
    				return this._MM_IcoSize;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IcoSize, _MM_IcoSize, value);
    				this._MM_IcoSize = value;
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
    		
    		public String MM_Intro {
    			get {
    				return this._MM_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Intro, _MM_Intro, value);
    				this._MM_Intro = value;
    			}
    		}
    		
    		public Boolean MM_IsBold {
    			get {
    				return this._MM_IsBold;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IsBold, _MM_IsBold, value);
    				this._MM_IsBold = value;
    			}
    		}
    		
    		public Boolean MM_IsChilds {
    			get {
    				return this._MM_IsChilds;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IsChilds, _MM_IsChilds, value);
    				this._MM_IsChilds = value;
    			}
    		}
    		
    		public Boolean MM_IsFixed {
    			get {
    				return this._MM_IsFixed;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IsFixed, _MM_IsFixed, value);
    				this._MM_IsFixed = value;
    			}
    		}
    		
    		public Boolean MM_IsItalic {
    			get {
    				return this._MM_IsItalic;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IsItalic, _MM_IsItalic, value);
    				this._MM_IsItalic = value;
    			}
    		}
    		
    		public Boolean MM_IsShow {
    			get {
    				return this._MM_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IsShow, _MM_IsShow, value);
    				this._MM_IsShow = value;
    			}
    		}
    		
    		public Boolean MM_IsUse {
    			get {
    				return this._MM_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_IsUse, _MM_IsUse, value);
    				this._MM_IsUse = value;
    			}
    		}
    		
    		public String MM_Link {
    			get {
    				return this._MM_Link;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Link, _MM_Link, value);
    				this._MM_Link = value;
    			}
    		}
    		
    		public String MM_Marker {
    			get {
    				return this._MM_Marker;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Marker, _MM_Marker, value);
    				this._MM_Marker = value;
    			}
    		}
    		
    		public String MM_Name {
    			get {
    				return this._MM_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Name, _MM_Name, value);
    				this._MM_Name = value;
    			}
    		}
    		
    		public String MM_PatId {
    			get {
    				return this._MM_PatId;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_PatId, _MM_PatId, value);
    				this._MM_PatId = value;
    			}
    		}
    		
    		public Int32 MM_Root {
    			get {
    				return this._MM_Root;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Root, _MM_Root, value);
    				this._MM_Root = value;
    			}
    		}
    		
    		public Int32 MM_Tax {
    			get {
    				return this._MM_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Tax, _MM_Tax, value);
    				this._MM_Tax = value;
    			}
    		}
    		
    		public String MM_Type {
    			get {
    				return this._MM_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Type, _MM_Type, value);
    				this._MM_Type = value;
    			}
    		}
    		
    		public String MM_UID {
    			get {
    				return this._MM_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_UID, _MM_UID, value);
    				this._MM_UID = value;
    			}
    		}
    		
    		public Int32 MM_WinHeight {
    			get {
    				return this._MM_WinHeight;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_WinHeight, _MM_WinHeight, value);
    				this._MM_WinHeight = value;
    			}
    		}
    		
    		public String MM_WinID {
    			get {
    				return this._MM_WinID;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_WinID, _MM_WinID, value);
    				this._MM_WinID = value;
    			}
    		}
    		
    		public Boolean MM_WinMax {
    			get {
    				return this._MM_WinMax;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_WinMax, _MM_WinMax, value);
    				this._MM_WinMax = value;
    			}
    		}
    		
    		public Boolean MM_WinMin {
    			get {
    				return this._MM_WinMin;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_WinMin, _MM_WinMin, value);
    				this._MM_WinMin = value;
    			}
    		}
    		
    		public Boolean MM_WinMove {
    			get {
    				return this._MM_WinMove;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_WinMove, _MM_WinMove, value);
    				this._MM_WinMove = value;
    			}
    		}
    		
    		public Boolean MM_WinResize {
    			get {
    				return this._MM_WinResize;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_WinResize, _MM_WinResize, value);
    				this._MM_WinResize = value;
    			}
    		}
    		
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
    					_.MM_AbbrName,
    					_.MM_Color,
    					_.MM_Complete,
    					_.MM_Font,
    					_.MM_Func,
    					_.MM_Help,
    					_.MM_IcoCode,
    					_.MM_IcoColor,
    					_.MM_IcoSize,
    					_.MM_IcoX,
    					_.MM_IcoY,
    					_.MM_Intro,
    					_.MM_IsBold,
    					_.MM_IsChilds,
    					_.MM_IsFixed,
    					_.MM_IsItalic,
    					_.MM_IsShow,
    					_.MM_IsUse,
    					_.MM_Link,
    					_.MM_Marker,
    					_.MM_Name,
    					_.MM_PatId,
    					_.MM_Root,
    					_.MM_Tax,
    					_.MM_Type,
    					_.MM_UID,
    					_.MM_WinHeight,
    					_.MM_WinID,
    					_.MM_WinMax,
    					_.MM_WinMin,
    					_.MM_WinMove,
    					_.MM_WinResize,
    					_.MM_WinWidth};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._MM_Id,
    					this._MM_AbbrName,
    					this._MM_Color,
    					this._MM_Complete,
    					this._MM_Font,
    					this._MM_Func,
    					this._MM_Help,
    					this._MM_IcoCode,
    					this._MM_IcoColor,
    					this._MM_IcoSize,
    					this._MM_IcoX,
    					this._MM_IcoY,
    					this._MM_Intro,
    					this._MM_IsBold,
    					this._MM_IsChilds,
    					this._MM_IsFixed,
    					this._MM_IsItalic,
    					this._MM_IsShow,
    					this._MM_IsUse,
    					this._MM_Link,
    					this._MM_Marker,
    					this._MM_Name,
    					this._MM_PatId,
    					this._MM_Root,
    					this._MM_Tax,
    					this._MM_Type,
    					this._MM_UID,
    					this._MM_WinHeight,
    					this._MM_WinID,
    					this._MM_WinMax,
    					this._MM_WinMin,
    					this._MM_WinMove,
    					this._MM_WinResize,
    					this._MM_WinWidth};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.MM_Id))) {
    				this._MM_Id = reader.GetInt32(_.MM_Id);
    			}
    			if ((false == reader.IsDBNull(_.MM_AbbrName))) {
    				this._MM_AbbrName = reader.GetString(_.MM_AbbrName);
    			}
    			if ((false == reader.IsDBNull(_.MM_Color))) {
    				this._MM_Color = reader.GetString(_.MM_Color);
    			}
    			if ((false == reader.IsDBNull(_.MM_Complete))) {
    				this._MM_Complete = reader.GetInt32(_.MM_Complete);
    			}
    			if ((false == reader.IsDBNull(_.MM_Font))) {
    				this._MM_Font = reader.GetString(_.MM_Font);
    			}
    			if ((false == reader.IsDBNull(_.MM_Func))) {
    				this._MM_Func = reader.GetString(_.MM_Func);
    			}
    			if ((false == reader.IsDBNull(_.MM_Help))) {
    				this._MM_Help = reader.GetString(_.MM_Help);
    			}
    			if ((false == reader.IsDBNull(_.MM_IcoCode))) {
    				this._MM_IcoCode = reader.GetString(_.MM_IcoCode);
    			}
    			if ((false == reader.IsDBNull(_.MM_IcoColor))) {
    				this._MM_IcoColor = reader.GetString(_.MM_IcoColor);
    			}
    			if ((false == reader.IsDBNull(_.MM_IcoSize))) {
    				this._MM_IcoSize = reader.GetInt32(_.MM_IcoSize);
    			}
    			if ((false == reader.IsDBNull(_.MM_IcoX))) {
    				this._MM_IcoX = reader.GetInt32(_.MM_IcoX);
    			}
    			if ((false == reader.IsDBNull(_.MM_IcoY))) {
    				this._MM_IcoY = reader.GetInt32(_.MM_IcoY);
    			}
    			if ((false == reader.IsDBNull(_.MM_Intro))) {
    				this._MM_Intro = reader.GetString(_.MM_Intro);
    			}
    			if ((false == reader.IsDBNull(_.MM_IsBold))) {
    				this._MM_IsBold = reader.GetBoolean(_.MM_IsBold);
    			}
    			if ((false == reader.IsDBNull(_.MM_IsChilds))) {
    				this._MM_IsChilds = reader.GetBoolean(_.MM_IsChilds);
    			}
    			if ((false == reader.IsDBNull(_.MM_IsFixed))) {
    				this._MM_IsFixed = reader.GetBoolean(_.MM_IsFixed);
    			}
    			if ((false == reader.IsDBNull(_.MM_IsItalic))) {
    				this._MM_IsItalic = reader.GetBoolean(_.MM_IsItalic);
    			}
    			if ((false == reader.IsDBNull(_.MM_IsShow))) {
    				this._MM_IsShow = reader.GetBoolean(_.MM_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.MM_IsUse))) {
    				this._MM_IsUse = reader.GetBoolean(_.MM_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.MM_Link))) {
    				this._MM_Link = reader.GetString(_.MM_Link);
    			}
    			if ((false == reader.IsDBNull(_.MM_Marker))) {
    				this._MM_Marker = reader.GetString(_.MM_Marker);
    			}
    			if ((false == reader.IsDBNull(_.MM_Name))) {
    				this._MM_Name = reader.GetString(_.MM_Name);
    			}
    			if ((false == reader.IsDBNull(_.MM_PatId))) {
    				this._MM_PatId = reader.GetString(_.MM_PatId);
    			}
    			if ((false == reader.IsDBNull(_.MM_Root))) {
    				this._MM_Root = reader.GetInt32(_.MM_Root);
    			}
    			if ((false == reader.IsDBNull(_.MM_Tax))) {
    				this._MM_Tax = reader.GetInt32(_.MM_Tax);
    			}
    			if ((false == reader.IsDBNull(_.MM_Type))) {
    				this._MM_Type = reader.GetString(_.MM_Type);
    			}
    			if ((false == reader.IsDBNull(_.MM_UID))) {
    				this._MM_UID = reader.GetString(_.MM_UID);
    			}
    			if ((false == reader.IsDBNull(_.MM_WinHeight))) {
    				this._MM_WinHeight = reader.GetInt32(_.MM_WinHeight);
    			}
    			if ((false == reader.IsDBNull(_.MM_WinID))) {
    				this._MM_WinID = reader.GetString(_.MM_WinID);
    			}
    			if ((false == reader.IsDBNull(_.MM_WinMax))) {
    				this._MM_WinMax = reader.GetBoolean(_.MM_WinMax);
    			}
    			if ((false == reader.IsDBNull(_.MM_WinMin))) {
    				this._MM_WinMin = reader.GetBoolean(_.MM_WinMin);
    			}
    			if ((false == reader.IsDBNull(_.MM_WinMove))) {
    				this._MM_WinMove = reader.GetBoolean(_.MM_WinMove);
    			}
    			if ((false == reader.IsDBNull(_.MM_WinResize))) {
    				this._MM_WinResize = reader.GetBoolean(_.MM_WinResize);
    			}
    			if ((false == reader.IsDBNull(_.MM_WinWidth))) {
    				this._MM_WinWidth = reader.GetInt32(_.MM_WinWidth);
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
    			/// 字段名：MM_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_Id = new WeiSha.Data.Field<ManageMenu>("MM_Id");
    			
    			/// <summary>
    			/// 字段名：MM_AbbrName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_AbbrName = new WeiSha.Data.Field<ManageMenu>("MM_AbbrName");
    			
    			/// <summary>
    			/// 字段名：MM_Color - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Color = new WeiSha.Data.Field<ManageMenu>("MM_Color");
    			
    			/// <summary>
    			/// 字段名：MM_Complete - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_Complete = new WeiSha.Data.Field<ManageMenu>("MM_Complete");
    			
    			/// <summary>
    			/// 字段名：MM_Font - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Font = new WeiSha.Data.Field<ManageMenu>("MM_Font");
    			
    			/// <summary>
    			/// 字段名：MM_Func - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Func = new WeiSha.Data.Field<ManageMenu>("MM_Func");
    			
    			/// <summary>
    			/// 字段名：MM_Help - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Help = new WeiSha.Data.Field<ManageMenu>("MM_Help");
    			
    			/// <summary>
    			/// 字段名：MM_IcoCode - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_IcoCode = new WeiSha.Data.Field<ManageMenu>("MM_IcoCode");
    			
    			/// <summary>
    			/// 字段名：MM_IcoColor - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_IcoColor = new WeiSha.Data.Field<ManageMenu>("MM_IcoColor");
    			
    			/// <summary>
    			/// 字段名：MM_IcoSize - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_IcoSize = new WeiSha.Data.Field<ManageMenu>("MM_IcoSize");
    			
    			/// <summary>
    			/// 字段名：MM_IcoX - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_IcoX = new WeiSha.Data.Field<ManageMenu>("MM_IcoX");
    			
    			/// <summary>
    			/// 字段名：MM_IcoY - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_IcoY = new WeiSha.Data.Field<ManageMenu>("MM_IcoY");
    			
    			/// <summary>
    			/// 字段名：MM_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Intro = new WeiSha.Data.Field<ManageMenu>("MM_Intro");
    			
    			/// <summary>
    			/// 字段名：MM_IsBold - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_IsBold = new WeiSha.Data.Field<ManageMenu>("MM_IsBold");
    			
    			/// <summary>
    			/// 字段名：MM_IsChilds - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_IsChilds = new WeiSha.Data.Field<ManageMenu>("MM_IsChilds");
    			
    			/// <summary>
    			/// 字段名：MM_IsFixed - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_IsFixed = new WeiSha.Data.Field<ManageMenu>("MM_IsFixed");
    			
    			/// <summary>
    			/// 字段名：MM_IsItalic - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_IsItalic = new WeiSha.Data.Field<ManageMenu>("MM_IsItalic");
    			
    			/// <summary>
    			/// 字段名：MM_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_IsShow = new WeiSha.Data.Field<ManageMenu>("MM_IsShow");
    			
    			/// <summary>
    			/// 字段名：MM_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_IsUse = new WeiSha.Data.Field<ManageMenu>("MM_IsUse");
    			
    			/// <summary>
    			/// 字段名：MM_Link - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Link = new WeiSha.Data.Field<ManageMenu>("MM_Link");
    			
    			/// <summary>
    			/// 字段名：MM_Marker - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Marker = new WeiSha.Data.Field<ManageMenu>("MM_Marker");
    			
    			/// <summary>
    			/// 字段名：MM_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Name = new WeiSha.Data.Field<ManageMenu>("MM_Name");
    			
    			/// <summary>
    			/// 字段名：MM_PatId - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_PatId = new WeiSha.Data.Field<ManageMenu>("MM_PatId");
    			
    			/// <summary>
    			/// 字段名：MM_Root - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_Root = new WeiSha.Data.Field<ManageMenu>("MM_Root");
    			
    			/// <summary>
    			/// 字段名：MM_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_Tax = new WeiSha.Data.Field<ManageMenu>("MM_Tax");
    			
    			/// <summary>
    			/// 字段名：MM_Type - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_Type = new WeiSha.Data.Field<ManageMenu>("MM_Type");
    			
    			/// <summary>
    			/// 字段名：MM_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_UID = new WeiSha.Data.Field<ManageMenu>("MM_UID");
    			
    			/// <summary>
    			/// 字段名：MM_WinHeight - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_WinHeight = new WeiSha.Data.Field<ManageMenu>("MM_WinHeight");
    			
    			/// <summary>
    			/// 字段名：MM_WinID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MM_WinID = new WeiSha.Data.Field<ManageMenu>("MM_WinID");
    			
    			/// <summary>
    			/// 字段名：MM_WinMax - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_WinMax = new WeiSha.Data.Field<ManageMenu>("MM_WinMax");
    			
    			/// <summary>
    			/// 字段名：MM_WinMin - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_WinMin = new WeiSha.Data.Field<ManageMenu>("MM_WinMin");
    			
    			/// <summary>
    			/// 字段名：MM_WinMove - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_WinMove = new WeiSha.Data.Field<ManageMenu>("MM_WinMove");
    			
    			/// <summary>
    			/// 字段名：MM_WinResize - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MM_WinResize = new WeiSha.Data.Field<ManageMenu>("MM_WinResize");
    			
    			/// <summary>
    			/// 字段名：MM_WinWidth - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MM_WinWidth = new WeiSha.Data.Field<ManageMenu>("MM_WinWidth");
    		}
    	}
    }
    