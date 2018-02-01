namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ShowPicture 主键列：Shp_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ShowPicture : WeiSha.Data.Entity {
    		
    		protected Int32 _Shp_ID;
    		
    		protected String _Shp_File;
    		
    		protected String _Shp_Url;
    		
    		protected String _Shp_Target;
    		
    		protected Boolean _Shp_IsShow;
    		
    		protected String _Shp_Site;
    		
    		protected String _Shp_BgColor;
    		
    		protected Int32 _Shp_Tax;
    		
    		protected String _Shp_Intro;
    		
    		protected Int32 _Org_ID;
    		
    		public Int32 Shp_ID {
    			get {
    				return this._Shp_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Shp_ID, _Shp_ID, value);
    				this._Shp_ID = value;
    			}
    		}
    		
    		public String Shp_File {
    			get {
    				return this._Shp_File;
    			}
    			set {
    				this.OnPropertyValueChange(_.Shp_File, _Shp_File, value);
    				this._Shp_File = value;
    			}
    		}
    		
    		public String Shp_Url {
    			get {
    				return this._Shp_Url;
    			}
    			set {
    				this.OnPropertyValueChange(_.Shp_Url, _Shp_Url, value);
    				this._Shp_Url = value;
    			}
    		}
    		
    		public String Shp_Target {
    			get {
    				return this._Shp_Target;
    			}
    			set {
    				this.OnPropertyValueChange(_.Shp_Target, _Shp_Target, value);
    				this._Shp_Target = value;
    			}
    		}
    		
    		public Boolean Shp_IsShow {
    			get {
    				return this._Shp_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Shp_IsShow, _Shp_IsShow, value);
    				this._Shp_IsShow = value;
    			}
    		}
    		
    		public String Shp_Site {
    			get {
    				return this._Shp_Site;
    			}
    			set {
    				this.OnPropertyValueChange(_.Shp_Site, _Shp_Site, value);
    				this._Shp_Site = value;
    			}
    		}
    		
    		public String Shp_BgColor {
    			get {
    				return this._Shp_BgColor;
    			}
    			set {
    				this.OnPropertyValueChange(_.Shp_BgColor, _Shp_BgColor, value);
    				this._Shp_BgColor = value;
    			}
    		}
    		
    		public Int32 Shp_Tax {
    			get {
    				return this._Shp_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Shp_Tax, _Shp_Tax, value);
    				this._Shp_Tax = value;
    			}
    		}
    		
    		public String Shp_Intro {
    			get {
    				return this._Shp_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Shp_Intro, _Shp_Intro, value);
    				this._Shp_Intro = value;
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
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<ShowPicture>("ShowPicture");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Shp_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Shp_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Shp_ID,
    					_.Shp_File,
    					_.Shp_Url,
    					_.Shp_Target,
    					_.Shp_IsShow,
    					_.Shp_Site,
    					_.Shp_BgColor,
    					_.Shp_Tax,
    					_.Shp_Intro,
    					_.Org_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Shp_ID,
    					this._Shp_File,
    					this._Shp_Url,
    					this._Shp_Target,
    					this._Shp_IsShow,
    					this._Shp_Site,
    					this._Shp_BgColor,
    					this._Shp_Tax,
    					this._Shp_Intro,
    					this._Org_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Shp_ID))) {
    				this._Shp_ID = reader.GetInt32(_.Shp_ID);
    			}
    			if ((false == reader.IsDBNull(_.Shp_File))) {
    				this._Shp_File = reader.GetString(_.Shp_File);
    			}
    			if ((false == reader.IsDBNull(_.Shp_Url))) {
    				this._Shp_Url = reader.GetString(_.Shp_Url);
    			}
    			if ((false == reader.IsDBNull(_.Shp_Target))) {
    				this._Shp_Target = reader.GetString(_.Shp_Target);
    			}
    			if ((false == reader.IsDBNull(_.Shp_IsShow))) {
    				this._Shp_IsShow = reader.GetBoolean(_.Shp_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Shp_Site))) {
    				this._Shp_Site = reader.GetString(_.Shp_Site);
    			}
    			if ((false == reader.IsDBNull(_.Shp_BgColor))) {
    				this._Shp_BgColor = reader.GetString(_.Shp_BgColor);
    			}
    			if ((false == reader.IsDBNull(_.Shp_Tax))) {
    				this._Shp_Tax = reader.GetInt32(_.Shp_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Shp_Intro))) {
    				this._Shp_Intro = reader.GetString(_.Shp_Intro);
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
    			if ((false == typeof(ShowPicture).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<ShowPicture>();
    			
    			/// <summary>
    			/// 字段名：Shp_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Shp_ID = new WeiSha.Data.Field<ShowPicture>("Shp_ID");
    			
    			/// <summary>
    			/// 字段名：Shp_File - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Shp_File = new WeiSha.Data.Field<ShowPicture>("Shp_File");
    			
    			/// <summary>
    			/// 字段名：Shp_Url - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Shp_Url = new WeiSha.Data.Field<ShowPicture>("Shp_Url");
    			
    			/// <summary>
    			/// 字段名：Shp_Target - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Shp_Target = new WeiSha.Data.Field<ShowPicture>("Shp_Target");
    			
    			/// <summary>
    			/// 字段名：Shp_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Shp_IsShow = new WeiSha.Data.Field<ShowPicture>("Shp_IsShow");
    			
    			/// <summary>
    			/// 字段名：Shp_Site - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Shp_Site = new WeiSha.Data.Field<ShowPicture>("Shp_Site");
    			
    			/// <summary>
    			/// 字段名：Shp_BgColor - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Shp_BgColor = new WeiSha.Data.Field<ShowPicture>("Shp_BgColor");
    			
    			/// <summary>
    			/// 字段名：Shp_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Shp_Tax = new WeiSha.Data.Field<ShowPicture>("Shp_Tax");
    			
    			/// <summary>
    			/// 字段名：Shp_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Shp_Intro = new WeiSha.Data.Field<ShowPicture>("Shp_Intro");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<ShowPicture>("Org_ID");
    		}
    	}
    }
    