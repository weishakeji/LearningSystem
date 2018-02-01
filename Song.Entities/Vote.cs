namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Vote 主键列：Vt_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Vote : WeiSha.Data.Entity {
    		
    		protected Int32 _Vt_Id;
    		
    		protected String _Vt_UniqueId;
    		
    		protected Boolean _Vt_IsTheme;
    		
    		protected String _Vt_Name;
    		
    		protected Int32? _Vt_Type;
    		
    		protected String _Vt_Intro;
    		
    		protected Int32? _Vt_Tax;
    		
    		protected Int32? _Vt_SelectType;
    		
    		protected Boolean _Vt_IsShow;
    		
    		protected Boolean _Vt_IsUse;
    		
    		protected Boolean _Vt_IsAllowSee;
    		
    		protected Boolean _Vt_IsImage;
    		
    		protected DateTime? _Vt_CrtTime;
    		
    		protected DateTime? _Vt_StartTime;
    		
    		protected DateTime? _Vt_OverTime;
    		
    		protected String _Vt_Logo;
    		
    		protected String _Vt_Image;
    		
    		protected String _Vt_ImageSmall;
    		
    		protected Int32? _Vt_Number;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Vt_Id {
    			get {
    				return this._Vt_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_Id, _Vt_Id, value);
    				this._Vt_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vt_UniqueId {
    			get {
    				return this._Vt_UniqueId;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_UniqueId, _Vt_UniqueId, value);
    				this._Vt_UniqueId = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Vt_IsTheme {
    			get {
    				return this._Vt_IsTheme;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_IsTheme, _Vt_IsTheme, value);
    				this._Vt_IsTheme = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vt_Name {
    			get {
    				return this._Vt_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_Name, _Vt_Name, value);
    				this._Vt_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Vt_Type {
    			get {
    				return this._Vt_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_Type, _Vt_Type, value);
    				this._Vt_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vt_Intro {
    			get {
    				return this._Vt_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_Intro, _Vt_Intro, value);
    				this._Vt_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Vt_Tax {
    			get {
    				return this._Vt_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_Tax, _Vt_Tax, value);
    				this._Vt_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Vt_SelectType {
    			get {
    				return this._Vt_SelectType;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_SelectType, _Vt_SelectType, value);
    				this._Vt_SelectType = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Vt_IsShow {
    			get {
    				return this._Vt_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_IsShow, _Vt_IsShow, value);
    				this._Vt_IsShow = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Vt_IsUse {
    			get {
    				return this._Vt_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_IsUse, _Vt_IsUse, value);
    				this._Vt_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Vt_IsAllowSee {
    			get {
    				return this._Vt_IsAllowSee;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_IsAllowSee, _Vt_IsAllowSee, value);
    				this._Vt_IsAllowSee = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Vt_IsImage {
    			get {
    				return this._Vt_IsImage;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_IsImage, _Vt_IsImage, value);
    				this._Vt_IsImage = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Vt_CrtTime {
    			get {
    				return this._Vt_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_CrtTime, _Vt_CrtTime, value);
    				this._Vt_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Vt_StartTime {
    			get {
    				return this._Vt_StartTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_StartTime, _Vt_StartTime, value);
    				this._Vt_StartTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Vt_OverTime {
    			get {
    				return this._Vt_OverTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_OverTime, _Vt_OverTime, value);
    				this._Vt_OverTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vt_Logo {
    			get {
    				return this._Vt_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_Logo, _Vt_Logo, value);
    				this._Vt_Logo = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vt_Image {
    			get {
    				return this._Vt_Image;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_Image, _Vt_Image, value);
    				this._Vt_Image = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vt_ImageSmall {
    			get {
    				return this._Vt_ImageSmall;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_ImageSmall, _Vt_ImageSmall, value);
    				this._Vt_ImageSmall = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Vt_Number {
    			get {
    				return this._Vt_Number;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vt_Number, _Vt_Number, value);
    				this._Vt_Number = value;
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
    			return new WeiSha.Data.Table<Vote>("Vote");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Vt_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Vt_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Vt_Id,
    					_.Vt_UniqueId,
    					_.Vt_IsTheme,
    					_.Vt_Name,
    					_.Vt_Type,
    					_.Vt_Intro,
    					_.Vt_Tax,
    					_.Vt_SelectType,
    					_.Vt_IsShow,
    					_.Vt_IsUse,
    					_.Vt_IsAllowSee,
    					_.Vt_IsImage,
    					_.Vt_CrtTime,
    					_.Vt_StartTime,
    					_.Vt_OverTime,
    					_.Vt_Logo,
    					_.Vt_Image,
    					_.Vt_ImageSmall,
    					_.Vt_Number,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Vt_Id,
    					this._Vt_UniqueId,
    					this._Vt_IsTheme,
    					this._Vt_Name,
    					this._Vt_Type,
    					this._Vt_Intro,
    					this._Vt_Tax,
    					this._Vt_SelectType,
    					this._Vt_IsShow,
    					this._Vt_IsUse,
    					this._Vt_IsAllowSee,
    					this._Vt_IsImage,
    					this._Vt_CrtTime,
    					this._Vt_StartTime,
    					this._Vt_OverTime,
    					this._Vt_Logo,
    					this._Vt_Image,
    					this._Vt_ImageSmall,
    					this._Vt_Number,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Vt_Id))) {
    				this._Vt_Id = reader.GetInt32(_.Vt_Id);
    			}
    			if ((false == reader.IsDBNull(_.Vt_UniqueId))) {
    				this._Vt_UniqueId = reader.GetString(_.Vt_UniqueId);
    			}
    			if ((false == reader.IsDBNull(_.Vt_IsTheme))) {
    				this._Vt_IsTheme = reader.GetBoolean(_.Vt_IsTheme);
    			}
    			if ((false == reader.IsDBNull(_.Vt_Name))) {
    				this._Vt_Name = reader.GetString(_.Vt_Name);
    			}
    			if ((false == reader.IsDBNull(_.Vt_Type))) {
    				this._Vt_Type = reader.GetInt32(_.Vt_Type);
    			}
    			if ((false == reader.IsDBNull(_.Vt_Intro))) {
    				this._Vt_Intro = reader.GetString(_.Vt_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Vt_Tax))) {
    				this._Vt_Tax = reader.GetInt32(_.Vt_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Vt_SelectType))) {
    				this._Vt_SelectType = reader.GetInt32(_.Vt_SelectType);
    			}
    			if ((false == reader.IsDBNull(_.Vt_IsShow))) {
    				this._Vt_IsShow = reader.GetBoolean(_.Vt_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Vt_IsUse))) {
    				this._Vt_IsUse = reader.GetBoolean(_.Vt_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Vt_IsAllowSee))) {
    				this._Vt_IsAllowSee = reader.GetBoolean(_.Vt_IsAllowSee);
    			}
    			if ((false == reader.IsDBNull(_.Vt_IsImage))) {
    				this._Vt_IsImage = reader.GetBoolean(_.Vt_IsImage);
    			}
    			if ((false == reader.IsDBNull(_.Vt_CrtTime))) {
    				this._Vt_CrtTime = reader.GetDateTime(_.Vt_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Vt_StartTime))) {
    				this._Vt_StartTime = reader.GetDateTime(_.Vt_StartTime);
    			}
    			if ((false == reader.IsDBNull(_.Vt_OverTime))) {
    				this._Vt_OverTime = reader.GetDateTime(_.Vt_OverTime);
    			}
    			if ((false == reader.IsDBNull(_.Vt_Logo))) {
    				this._Vt_Logo = reader.GetString(_.Vt_Logo);
    			}
    			if ((false == reader.IsDBNull(_.Vt_Image))) {
    				this._Vt_Image = reader.GetString(_.Vt_Image);
    			}
    			if ((false == reader.IsDBNull(_.Vt_ImageSmall))) {
    				this._Vt_ImageSmall = reader.GetString(_.Vt_ImageSmall);
    			}
    			if ((false == reader.IsDBNull(_.Vt_Number))) {
    				this._Vt_Number = reader.GetInt32(_.Vt_Number);
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
    			if ((false == typeof(Vote).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Vote>();
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Vt_Id = new WeiSha.Data.Field<Vote>("Vt_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_UniqueId - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vt_UniqueId = new WeiSha.Data.Field<Vote>("Vt_UniqueId");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_IsTheme - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Vt_IsTheme = new WeiSha.Data.Field<Vote>("Vt_IsTheme");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vt_Name = new WeiSha.Data.Field<Vote>("Vt_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_Type - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Vt_Type = new WeiSha.Data.Field<Vote>("Vt_Type");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vt_Intro = new WeiSha.Data.Field<Vote>("Vt_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_Tax - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Vt_Tax = new WeiSha.Data.Field<Vote>("Vt_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_SelectType - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Vt_SelectType = new WeiSha.Data.Field<Vote>("Vt_SelectType");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Vt_IsShow = new WeiSha.Data.Field<Vote>("Vt_IsShow");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Vt_IsUse = new WeiSha.Data.Field<Vote>("Vt_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_IsAllowSee - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Vt_IsAllowSee = new WeiSha.Data.Field<Vote>("Vt_IsAllowSee");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_IsImage - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Vt_IsImage = new WeiSha.Data.Field<Vote>("Vt_IsImage");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Vt_CrtTime = new WeiSha.Data.Field<Vote>("Vt_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_StartTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Vt_StartTime = new WeiSha.Data.Field<Vote>("Vt_StartTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_OverTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Vt_OverTime = new WeiSha.Data.Field<Vote>("Vt_OverTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vt_Logo = new WeiSha.Data.Field<Vote>("Vt_Logo");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_Image - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vt_Image = new WeiSha.Data.Field<Vote>("Vt_Image");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_ImageSmall - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vt_ImageSmall = new WeiSha.Data.Field<Vote>("Vt_ImageSmall");
    			
    			/// <summary>
    			/// -1 - 字段名：Vt_Number - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Vt_Number = new WeiSha.Data.Field<Vote>("Vt_Number");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Vote>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Vote>("Org_Name");
    		}
    	}
    }
