namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ProductPackage 主键列：PPK_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ProductPackage : WeiSha.Data.Entity {
    		
    		protected Int32 _PPK_Id;
    		
    		protected String _PPK_Name;
    		
    		protected Boolean _PPK_IsUse;
    		
    		protected String _PPK_Logo;
    		
    		protected Int32? _PPK_Price;
    		
    		protected Int32? _PPK_DiscountPrice;
    		
    		protected DateTime? _PPK_StartTime;
    		
    		protected DateTime? _PPK_EndTime;
    		
    		protected String _PPK_Intro;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 PPK_Id {
    			get {
    				return this._PPK_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.PPK_Id, _PPK_Id, value);
    				this._PPK_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String PPK_Name {
    			get {
    				return this._PPK_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.PPK_Name, _PPK_Name, value);
    				this._PPK_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean PPK_IsUse {
    			get {
    				return this._PPK_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.PPK_IsUse, _PPK_IsUse, value);
    				this._PPK_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String PPK_Logo {
    			get {
    				return this._PPK_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.PPK_Logo, _PPK_Logo, value);
    				this._PPK_Logo = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? PPK_Price {
    			get {
    				return this._PPK_Price;
    			}
    			set {
    				this.OnPropertyValueChange(_.PPK_Price, _PPK_Price, value);
    				this._PPK_Price = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? PPK_DiscountPrice {
    			get {
    				return this._PPK_DiscountPrice;
    			}
    			set {
    				this.OnPropertyValueChange(_.PPK_DiscountPrice, _PPK_DiscountPrice, value);
    				this._PPK_DiscountPrice = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? PPK_StartTime {
    			get {
    				return this._PPK_StartTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.PPK_StartTime, _PPK_StartTime, value);
    				this._PPK_StartTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? PPK_EndTime {
    			get {
    				return this._PPK_EndTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.PPK_EndTime, _PPK_EndTime, value);
    				this._PPK_EndTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String PPK_Intro {
    			get {
    				return this._PPK_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.PPK_Intro, _PPK_Intro, value);
    				this._PPK_Intro = value;
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
    			return new WeiSha.Data.Table<ProductPackage>("ProductPackage");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.PPK_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.PPK_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.PPK_Id,
    					_.PPK_Name,
    					_.PPK_IsUse,
    					_.PPK_Logo,
    					_.PPK_Price,
    					_.PPK_DiscountPrice,
    					_.PPK_StartTime,
    					_.PPK_EndTime,
    					_.PPK_Intro,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._PPK_Id,
    					this._PPK_Name,
    					this._PPK_IsUse,
    					this._PPK_Logo,
    					this._PPK_Price,
    					this._PPK_DiscountPrice,
    					this._PPK_StartTime,
    					this._PPK_EndTime,
    					this._PPK_Intro,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.PPK_Id))) {
    				this._PPK_Id = reader.GetInt32(_.PPK_Id);
    			}
    			if ((false == reader.IsDBNull(_.PPK_Name))) {
    				this._PPK_Name = reader.GetString(_.PPK_Name);
    			}
    			if ((false == reader.IsDBNull(_.PPK_IsUse))) {
    				this._PPK_IsUse = reader.GetBoolean(_.PPK_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.PPK_Logo))) {
    				this._PPK_Logo = reader.GetString(_.PPK_Logo);
    			}
    			if ((false == reader.IsDBNull(_.PPK_Price))) {
    				this._PPK_Price = reader.GetInt32(_.PPK_Price);
    			}
    			if ((false == reader.IsDBNull(_.PPK_DiscountPrice))) {
    				this._PPK_DiscountPrice = reader.GetInt32(_.PPK_DiscountPrice);
    			}
    			if ((false == reader.IsDBNull(_.PPK_StartTime))) {
    				this._PPK_StartTime = reader.GetDateTime(_.PPK_StartTime);
    			}
    			if ((false == reader.IsDBNull(_.PPK_EndTime))) {
    				this._PPK_EndTime = reader.GetDateTime(_.PPK_EndTime);
    			}
    			if ((false == reader.IsDBNull(_.PPK_Intro))) {
    				this._PPK_Intro = reader.GetString(_.PPK_Intro);
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
    			if ((false == typeof(ProductPackage).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<ProductPackage>();
    			
    			/// <summary>
    			/// -1 - 字段名：PPK_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field PPK_Id = new WeiSha.Data.Field<ProductPackage>("PPK_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：PPK_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field PPK_Name = new WeiSha.Data.Field<ProductPackage>("PPK_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：PPK_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field PPK_IsUse = new WeiSha.Data.Field<ProductPackage>("PPK_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：PPK_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field PPK_Logo = new WeiSha.Data.Field<ProductPackage>("PPK_Logo");
    			
    			/// <summary>
    			/// -1 - 字段名：PPK_Price - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field PPK_Price = new WeiSha.Data.Field<ProductPackage>("PPK_Price");
    			
    			/// <summary>
    			/// -1 - 字段名：PPK_DiscountPrice - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field PPK_DiscountPrice = new WeiSha.Data.Field<ProductPackage>("PPK_DiscountPrice");
    			
    			/// <summary>
    			/// -1 - 字段名：PPK_StartTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field PPK_StartTime = new WeiSha.Data.Field<ProductPackage>("PPK_StartTime");
    			
    			/// <summary>
    			/// -1 - 字段名：PPK_EndTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field PPK_EndTime = new WeiSha.Data.Field<ProductPackage>("PPK_EndTime");
    			
    			/// <summary>
    			/// -1 - 字段名：PPK_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field PPK_Intro = new WeiSha.Data.Field<ProductPackage>("PPK_Intro");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<ProductPackage>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<ProductPackage>("Org_Name");
    		}
    	}
    }
    