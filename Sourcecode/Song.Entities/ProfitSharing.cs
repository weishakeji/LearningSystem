namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ProfitSharing 主键列：Ps_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ProfitSharing : WeiSha.Data.Entity {
    		
    		protected Int32 _Ps_ID;
    		
    		protected Boolean _Ps_IsTheme;
    		
    		protected Boolean _Ps_IsUse;
    		
    		protected Int32 _Ps_Level;
    		
    		protected String _Ps_Intro;
    		
    		protected Int32 _Ps_Moneyratio;
    		
    		protected Int32 _Ps_Couponratio;
    		
    		protected Decimal _Ps_MoneyValue;
    		
    		protected Int32 _Ps_CouponValue;
    		
    		protected Int32 _Ps_PID;
    		
    		protected String _Ps_Name;
    		
    		public Int32 Ps_ID {
    			get {
    				return this._Ps_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ps_ID, _Ps_ID, value);
    				this._Ps_ID = value;
    			}
    		}
    		
    		public Boolean Ps_IsTheme {
    			get {
    				return this._Ps_IsTheme;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ps_IsTheme, _Ps_IsTheme, value);
    				this._Ps_IsTheme = value;
    			}
    		}
    		
    		public Boolean Ps_IsUse {
    			get {
    				return this._Ps_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ps_IsUse, _Ps_IsUse, value);
    				this._Ps_IsUse = value;
    			}
    		}
    		
    		public Int32 Ps_Level {
    			get {
    				return this._Ps_Level;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ps_Level, _Ps_Level, value);
    				this._Ps_Level = value;
    			}
    		}
    		
    		public String Ps_Intro {
    			get {
    				return this._Ps_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ps_Intro, _Ps_Intro, value);
    				this._Ps_Intro = value;
    			}
    		}
    		
    		public Int32 Ps_Moneyratio {
    			get {
    				return this._Ps_Moneyratio;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ps_Moneyratio, _Ps_Moneyratio, value);
    				this._Ps_Moneyratio = value;
    			}
    		}
    		
    		public Int32 Ps_Couponratio {
    			get {
    				return this._Ps_Couponratio;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ps_Couponratio, _Ps_Couponratio, value);
    				this._Ps_Couponratio = value;
    			}
    		}
    		
    		public Decimal Ps_MoneyValue {
    			get {
    				return this._Ps_MoneyValue;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ps_MoneyValue, _Ps_MoneyValue, value);
    				this._Ps_MoneyValue = value;
    			}
    		}
    		
    		public Int32 Ps_CouponValue {
    			get {
    				return this._Ps_CouponValue;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ps_CouponValue, _Ps_CouponValue, value);
    				this._Ps_CouponValue = value;
    			}
    		}
    		
    		public Int32 Ps_PID {
    			get {
    				return this._Ps_PID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ps_PID, _Ps_PID, value);
    				this._Ps_PID = value;
    			}
    		}
    		
    		public String Ps_Name {
    			get {
    				return this._Ps_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ps_Name, _Ps_Name, value);
    				this._Ps_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<ProfitSharing>("ProfitSharing");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Ps_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ps_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ps_ID,
    					_.Ps_IsTheme,
    					_.Ps_IsUse,
    					_.Ps_Level,
    					_.Ps_Intro,
    					_.Ps_Moneyratio,
    					_.Ps_Couponratio,
    					_.Ps_MoneyValue,
    					_.Ps_CouponValue,
    					_.Ps_PID,
    					_.Ps_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Ps_ID,
    					this._Ps_IsTheme,
    					this._Ps_IsUse,
    					this._Ps_Level,
    					this._Ps_Intro,
    					this._Ps_Moneyratio,
    					this._Ps_Couponratio,
    					this._Ps_MoneyValue,
    					this._Ps_CouponValue,
    					this._Ps_PID,
    					this._Ps_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Ps_ID))) {
    				this._Ps_ID = reader.GetInt32(_.Ps_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ps_IsTheme))) {
    				this._Ps_IsTheme = reader.GetBoolean(_.Ps_IsTheme);
    			}
    			if ((false == reader.IsDBNull(_.Ps_IsUse))) {
    				this._Ps_IsUse = reader.GetBoolean(_.Ps_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Ps_Level))) {
    				this._Ps_Level = reader.GetInt32(_.Ps_Level);
    			}
    			if ((false == reader.IsDBNull(_.Ps_Intro))) {
    				this._Ps_Intro = reader.GetString(_.Ps_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Ps_Moneyratio))) {
    				this._Ps_Moneyratio = reader.GetInt32(_.Ps_Moneyratio);
    			}
    			if ((false == reader.IsDBNull(_.Ps_Couponratio))) {
    				this._Ps_Couponratio = reader.GetInt32(_.Ps_Couponratio);
    			}
    			if ((false == reader.IsDBNull(_.Ps_MoneyValue))) {
    				this._Ps_MoneyValue = reader.GetDecimal(_.Ps_MoneyValue);
    			}
    			if ((false == reader.IsDBNull(_.Ps_CouponValue))) {
    				this._Ps_CouponValue = reader.GetInt32(_.Ps_CouponValue);
    			}
    			if ((false == reader.IsDBNull(_.Ps_PID))) {
    				this._Ps_PID = reader.GetInt32(_.Ps_PID);
    			}
    			if ((false == reader.IsDBNull(_.Ps_Name))) {
    				this._Ps_Name = reader.GetString(_.Ps_Name);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(ProfitSharing).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<ProfitSharing>();
    			
    			/// <summary>
    			/// 字段名：Ps_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ps_ID = new WeiSha.Data.Field<ProfitSharing>("Ps_ID");
    			
    			/// <summary>
    			/// 字段名：Ps_IsTheme - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ps_IsTheme = new WeiSha.Data.Field<ProfitSharing>("Ps_IsTheme");
    			
    			/// <summary>
    			/// 字段名：Ps_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ps_IsUse = new WeiSha.Data.Field<ProfitSharing>("Ps_IsUse");
    			
    			/// <summary>
    			/// 字段名：Ps_Level - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ps_Level = new WeiSha.Data.Field<ProfitSharing>("Ps_Level");
    			
    			/// <summary>
    			/// 字段名：Ps_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ps_Intro = new WeiSha.Data.Field<ProfitSharing>("Ps_Intro");
    			
    			/// <summary>
    			/// 字段名：Ps_Moneyratio - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ps_Moneyratio = new WeiSha.Data.Field<ProfitSharing>("Ps_Moneyratio");
    			
    			/// <summary>
    			/// 字段名：Ps_Couponratio - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ps_Couponratio = new WeiSha.Data.Field<ProfitSharing>("Ps_Couponratio");
    			
    			/// <summary>
    			/// 字段名：Ps_MoneyValue - 数据类型：Decimal
    			/// </summary>
    			public static WeiSha.Data.Field Ps_MoneyValue = new WeiSha.Data.Field<ProfitSharing>("Ps_MoneyValue");
    			
    			/// <summary>
    			/// 字段名：Ps_CouponValue - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ps_CouponValue = new WeiSha.Data.Field<ProfitSharing>("Ps_CouponValue");
    			
    			/// <summary>
    			/// 字段名：Ps_PID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ps_PID = new WeiSha.Data.Field<ProfitSharing>("Ps_PID");
    			
    			/// <summary>
    			/// 字段名：Ps_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ps_Name = new WeiSha.Data.Field<ProfitSharing>("Ps_Name");
    		}
    	}
    }
    