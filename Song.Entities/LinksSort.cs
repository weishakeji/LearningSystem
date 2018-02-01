namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：LinksSort 主键列：Ls_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class LinksSort : WeiSha.Data.Entity {
    		
    		protected Int32 _Ls_Id;
    		
    		protected Int32? _Ls_PatId;
    		
    		protected String _Ls_Name;
    		
    		protected Int32? _Ls_Tax;
    		
    		protected Boolean _Ls_IsUse;
    		
    		protected String _Ls_Logo;
    		
    		protected Boolean _Ls_IsShow;
    		
    		protected String _Ls_Tootip;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Boolean _Ls_IsImg;
    		
    		protected Boolean _Ls_IsText;
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32 Ls_Id {
    			get {
    				return this._Ls_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ls_Id, _Ls_Id, value);
    				this._Ls_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Ls_PatId {
    			get {
    				return this._Ls_PatId;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ls_PatId, _Ls_PatId, value);
    				this._Ls_PatId = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public String Ls_Name {
    			get {
    				return this._Ls_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ls_Name, _Ls_Name, value);
    				this._Ls_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Ls_Tax {
    			get {
    				return this._Ls_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ls_Tax, _Ls_Tax, value);
    				this._Ls_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Boolean Ls_IsUse {
    			get {
    				return this._Ls_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ls_IsUse, _Ls_IsUse, value);
    				this._Ls_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Ls_Logo {
    			get {
    				return this._Ls_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ls_Logo, _Ls_Logo, value);
    				this._Ls_Logo = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Boolean Ls_IsShow {
    			get {
    				return this._Ls_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ls_IsShow, _Ls_IsShow, value);
    				this._Ls_IsShow = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String Ls_Tootip {
    			get {
    				return this._Ls_Tootip;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ls_Tootip, _Ls_Tootip, value);
    				this._Ls_Tootip = value;
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
    		
    		public Boolean Ls_IsImg {
    			get {
    				return this._Ls_IsImg;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ls_IsImg, _Ls_IsImg, value);
    				this._Ls_IsImg = value;
    			}
    		}
    		
    		public Boolean Ls_IsText {
    			get {
    				return this._Ls_IsText;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ls_IsText, _Ls_IsText, value);
    				this._Ls_IsText = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<LinksSort>("LinksSort");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Ls_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ls_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ls_Id,
    					_.Ls_PatId,
    					_.Ls_Name,
    					_.Ls_Tax,
    					_.Ls_IsUse,
    					_.Ls_Logo,
    					_.Ls_IsShow,
    					_.Ls_Tootip,
    					_.Org_ID,
    					_.Org_Name,
    					_.Ls_IsImg,
    					_.Ls_IsText};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Ls_Id,
    					this._Ls_PatId,
    					this._Ls_Name,
    					this._Ls_Tax,
    					this._Ls_IsUse,
    					this._Ls_Logo,
    					this._Ls_IsShow,
    					this._Ls_Tootip,
    					this._Org_ID,
    					this._Org_Name,
    					this._Ls_IsImg,
    					this._Ls_IsText};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Ls_Id))) {
    				this._Ls_Id = reader.GetInt32(_.Ls_Id);
    			}
    			if ((false == reader.IsDBNull(_.Ls_PatId))) {
    				this._Ls_PatId = reader.GetInt32(_.Ls_PatId);
    			}
    			if ((false == reader.IsDBNull(_.Ls_Name))) {
    				this._Ls_Name = reader.GetString(_.Ls_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ls_Tax))) {
    				this._Ls_Tax = reader.GetInt32(_.Ls_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Ls_IsUse))) {
    				this._Ls_IsUse = reader.GetBoolean(_.Ls_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Ls_Logo))) {
    				this._Ls_Logo = reader.GetString(_.Ls_Logo);
    			}
    			if ((false == reader.IsDBNull(_.Ls_IsShow))) {
    				this._Ls_IsShow = reader.GetBoolean(_.Ls_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Ls_Tootip))) {
    				this._Ls_Tootip = reader.GetString(_.Ls_Tootip);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ls_IsImg))) {
    				this._Ls_IsImg = reader.GetBoolean(_.Ls_IsImg);
    			}
    			if ((false == reader.IsDBNull(_.Ls_IsText))) {
    				this._Ls_IsText = reader.GetBoolean(_.Ls_IsText);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(LinksSort).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<LinksSort>();
    			
    			/// <summary>
    			/// False - 字段名：Ls_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ls_Id = new WeiSha.Data.Field<LinksSort>("Ls_Id");
    			
    			/// <summary>
    			/// False - 字段名：Ls_PatId - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Ls_PatId = new WeiSha.Data.Field<LinksSort>("Ls_PatId");
    			
    			/// <summary>
    			/// False - 字段名：Ls_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ls_Name = new WeiSha.Data.Field<LinksSort>("Ls_Name");
    			
    			/// <summary>
    			/// False - 字段名：Ls_Tax - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Ls_Tax = new WeiSha.Data.Field<LinksSort>("Ls_Tax");
    			
    			/// <summary>
    			/// False - 字段名：Ls_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ls_IsUse = new WeiSha.Data.Field<LinksSort>("Ls_IsUse");
    			
    			/// <summary>
    			/// True - 字段名：Ls_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ls_Logo = new WeiSha.Data.Field<LinksSort>("Ls_Logo");
    			
    			/// <summary>
    			/// False - 字段名：Ls_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ls_IsShow = new WeiSha.Data.Field<LinksSort>("Ls_IsShow");
    			
    			/// <summary>
    			/// True - 字段名：Ls_Tootip - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ls_Tootip = new WeiSha.Data.Field<LinksSort>("Ls_Tootip");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<LinksSort>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<LinksSort>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Ls_IsImg - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ls_IsImg = new WeiSha.Data.Field<LinksSort>("Ls_IsImg");
    			
    			/// <summary>
    			/// 字段名：Ls_IsText - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ls_IsText = new WeiSha.Data.Field<LinksSort>("Ls_IsText");
    		}
    	}
    }
    