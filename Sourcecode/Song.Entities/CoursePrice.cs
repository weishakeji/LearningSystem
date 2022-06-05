namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：CoursePrice 主键列：CP_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class CoursePrice : WeiSha.Data.Entity {
    		
    		protected Int32 _CP_ID;
    		
    		protected Int32 _CP_Tax;
    		
    		protected Int32 _CP_Price;
    		
    		protected Int32 _CP_Span;
    		
    		protected String _CP_Unit;
    		
    		protected Boolean _CP_IsUse;
    		
    		protected String _CP_Group;
    		
    		protected Int32 _Cou_ID;
    		
    		protected String _Cou_UID;
    		
    		protected Int32 _Org_ID;
    		
    		protected Int32 _CP_Coupon;
    		
    		public Int32 CP_ID {
    			get {
    				return this._CP_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.CP_ID, _CP_ID, value);
    				this._CP_ID = value;
    			}
    		}
    		
    		public Int32 CP_Tax {
    			get {
    				return this._CP_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.CP_Tax, _CP_Tax, value);
    				this._CP_Tax = value;
    			}
    		}
    		
    		public Int32 CP_Price {
    			get {
    				return this._CP_Price;
    			}
    			set {
    				this.OnPropertyValueChange(_.CP_Price, _CP_Price, value);
    				this._CP_Price = value;
    			}
    		}
    		
    		public Int32 CP_Span {
    			get {
    				return this._CP_Span;
    			}
    			set {
    				this.OnPropertyValueChange(_.CP_Span, _CP_Span, value);
    				this._CP_Span = value;
    			}
    		}
    		
    		public String CP_Unit {
    			get {
    				return this._CP_Unit;
    			}
    			set {
    				this.OnPropertyValueChange(_.CP_Unit, _CP_Unit, value);
    				this._CP_Unit = value;
    			}
    		}
    		
    		public Boolean CP_IsUse {
    			get {
    				return this._CP_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.CP_IsUse, _CP_IsUse, value);
    				this._CP_IsUse = value;
    			}
    		}
    		
    		public String CP_Group {
    			get {
    				return this._CP_Group;
    			}
    			set {
    				this.OnPropertyValueChange(_.CP_Group, _CP_Group, value);
    				this._CP_Group = value;
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
    		
    		public String Cou_UID {
    			get {
    				return this._Cou_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_UID, _Cou_UID, value);
    				this._Cou_UID = value;
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
    		
    		public Int32 CP_Coupon {
    			get {
    				return this._CP_Coupon;
    			}
    			set {
    				this.OnPropertyValueChange(_.CP_Coupon, _CP_Coupon, value);
    				this._CP_Coupon = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<CoursePrice>("CoursePrice");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.CP_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.CP_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.CP_ID,
    					_.CP_Tax,
    					_.CP_Price,
    					_.CP_Span,
    					_.CP_Unit,
    					_.CP_IsUse,
    					_.CP_Group,
    					_.Cou_ID,
    					_.Cou_UID,
    					_.Org_ID,
    					_.CP_Coupon};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._CP_ID,
    					this._CP_Tax,
    					this._CP_Price,
    					this._CP_Span,
    					this._CP_Unit,
    					this._CP_IsUse,
    					this._CP_Group,
    					this._Cou_ID,
    					this._Cou_UID,
    					this._Org_ID,
    					this._CP_Coupon};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.CP_ID))) {
    				this._CP_ID = reader.GetInt32(_.CP_ID);
    			}
    			if ((false == reader.IsDBNull(_.CP_Tax))) {
    				this._CP_Tax = reader.GetInt32(_.CP_Tax);
    			}
    			if ((false == reader.IsDBNull(_.CP_Price))) {
    				this._CP_Price = reader.GetInt32(_.CP_Price);
    			}
    			if ((false == reader.IsDBNull(_.CP_Span))) {
    				this._CP_Span = reader.GetInt32(_.CP_Span);
    			}
    			if ((false == reader.IsDBNull(_.CP_Unit))) {
    				this._CP_Unit = reader.GetString(_.CP_Unit);
    			}
    			if ((false == reader.IsDBNull(_.CP_IsUse))) {
    				this._CP_IsUse = reader.GetBoolean(_.CP_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.CP_Group))) {
    				this._CP_Group = reader.GetString(_.CP_Group);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_UID))) {
    				this._Cou_UID = reader.GetString(_.Cou_UID);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.CP_Coupon))) {
    				this._CP_Coupon = reader.GetInt32(_.CP_Coupon);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(CoursePrice).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<CoursePrice>();
    			
    			/// <summary>
    			/// 字段名：CP_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field CP_ID = new WeiSha.Data.Field<CoursePrice>("CP_ID");
    			
    			/// <summary>
    			/// 字段名：CP_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field CP_Tax = new WeiSha.Data.Field<CoursePrice>("CP_Tax");
    			
    			/// <summary>
    			/// 字段名：CP_Price - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field CP_Price = new WeiSha.Data.Field<CoursePrice>("CP_Price");
    			
    			/// <summary>
    			/// 字段名：CP_Span - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field CP_Span = new WeiSha.Data.Field<CoursePrice>("CP_Span");
    			
    			/// <summary>
    			/// 字段名：CP_Unit - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field CP_Unit = new WeiSha.Data.Field<CoursePrice>("CP_Unit");
    			
    			/// <summary>
    			/// 字段名：CP_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field CP_IsUse = new WeiSha.Data.Field<CoursePrice>("CP_IsUse");
    			
    			/// <summary>
    			/// 字段名：CP_Group - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field CP_Group = new WeiSha.Data.Field<CoursePrice>("CP_Group");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<CoursePrice>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Cou_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Cou_UID = new WeiSha.Data.Field<CoursePrice>("Cou_UID");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<CoursePrice>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：CP_Coupon - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field CP_Coupon = new WeiSha.Data.Field<CoursePrice>("CP_Coupon");
    		}
    	}
    }
    