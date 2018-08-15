namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：LearningCardCode 主键列：Lcc_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class LearningCardCode : WeiSha.Data.Entity {
    		
    		protected Int32 _Lcc_ID;
    		
    		protected String _Lcc_Code;
    		
    		protected String _Lcc_Pw;
    		
    		protected Single _Lcc_Price;
    		
    		protected Int32 _Lcc_Coupon;
    		
    		protected DateTime _Lcc_CrtTime;
    		
    		protected DateTime _Lcc_UsedTime;
    		
    		protected Boolean _Lcc_IsEnable;
    		
    		protected Boolean _Lcc_IsUsed;
    		
    		protected Int32 _Lcs_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected String _Ac_AccName;
    		
    		protected Int32 _Org_ID;
    		
    		protected DateTime _Lcc_LimitStart;
    		
    		protected DateTime _Lcc_LimitEnd;
    		
    		protected Int32 _Lcc_State;
    		
    		public Int32 Lcc_ID {
    			get {
    				return this._Lcc_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcc_ID, _Lcc_ID, value);
    				this._Lcc_ID = value;
    			}
    		}
    		
    		public String Lcc_Code {
    			get {
    				return this._Lcc_Code;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcc_Code, _Lcc_Code, value);
    				this._Lcc_Code = value;
    			}
    		}
    		
    		public String Lcc_Pw {
    			get {
    				return this._Lcc_Pw;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcc_Pw, _Lcc_Pw, value);
    				this._Lcc_Pw = value;
    			}
    		}
    		
    		public Single Lcc_Price {
    			get {
    				return this._Lcc_Price;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcc_Price, _Lcc_Price, value);
    				this._Lcc_Price = value;
    			}
    		}
    		
    		public Int32 Lcc_Coupon {
    			get {
    				return this._Lcc_Coupon;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcc_Coupon, _Lcc_Coupon, value);
    				this._Lcc_Coupon = value;
    			}
    		}
    		
    		public DateTime Lcc_CrtTime {
    			get {
    				return this._Lcc_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcc_CrtTime, _Lcc_CrtTime, value);
    				this._Lcc_CrtTime = value;
    			}
    		}
    		
    		public DateTime Lcc_UsedTime {
    			get {
    				return this._Lcc_UsedTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcc_UsedTime, _Lcc_UsedTime, value);
    				this._Lcc_UsedTime = value;
    			}
    		}
    		
    		public Boolean Lcc_IsEnable {
    			get {
    				return this._Lcc_IsEnable;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcc_IsEnable, _Lcc_IsEnable, value);
    				this._Lcc_IsEnable = value;
    			}
    		}
    		
    		public Boolean Lcc_IsUsed {
    			get {
    				return this._Lcc_IsUsed;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcc_IsUsed, _Lcc_IsUsed, value);
    				this._Lcc_IsUsed = value;
    			}
    		}
    		
    		public Int32 Lcs_ID {
    			get {
    				return this._Lcs_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_ID, _Lcs_ID, value);
    				this._Lcs_ID = value;
    			}
    		}
    		
    		public Int32 Ac_ID {
    			get {
    				return this._Ac_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_ID, _Ac_ID, value);
    				this._Ac_ID = value;
    			}
    		}
    		
    		public String Ac_AccName {
    			get {
    				return this._Ac_AccName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_AccName, _Ac_AccName, value);
    				this._Ac_AccName = value;
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
    		
    		public DateTime Lcc_LimitStart {
    			get {
    				return this._Lcc_LimitStart;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcc_LimitStart, _Lcc_LimitStart, value);
    				this._Lcc_LimitStart = value;
    			}
    		}
    		
    		public DateTime Lcc_LimitEnd {
    			get {
    				return this._Lcc_LimitEnd;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcc_LimitEnd, _Lcc_LimitEnd, value);
    				this._Lcc_LimitEnd = value;
    			}
    		}
    		
    		public Int32 Lcc_State {
    			get {
    				return this._Lcc_State;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcc_State, _Lcc_State, value);
    				this._Lcc_State = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<LearningCardCode>("LearningCardCode");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Lcc_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lcc_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lcc_ID,
    					_.Lcc_Code,
    					_.Lcc_Pw,
    					_.Lcc_Price,
    					_.Lcc_Coupon,
    					_.Lcc_CrtTime,
    					_.Lcc_UsedTime,
    					_.Lcc_IsEnable,
    					_.Lcc_IsUsed,
    					_.Lcs_ID,
    					_.Ac_ID,
    					_.Ac_AccName,
    					_.Org_ID,
    					_.Lcc_LimitStart,
    					_.Lcc_LimitEnd,
    					_.Lcc_State};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Lcc_ID,
    					this._Lcc_Code,
    					this._Lcc_Pw,
    					this._Lcc_Price,
    					this._Lcc_Coupon,
    					this._Lcc_CrtTime,
    					this._Lcc_UsedTime,
    					this._Lcc_IsEnable,
    					this._Lcc_IsUsed,
    					this._Lcs_ID,
    					this._Ac_ID,
    					this._Ac_AccName,
    					this._Org_ID,
    					this._Lcc_LimitStart,
    					this._Lcc_LimitEnd,
    					this._Lcc_State};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Lcc_ID))) {
    				this._Lcc_ID = reader.GetInt32(_.Lcc_ID);
    			}
    			if ((false == reader.IsDBNull(_.Lcc_Code))) {
    				this._Lcc_Code = reader.GetString(_.Lcc_Code);
    			}
    			if ((false == reader.IsDBNull(_.Lcc_Pw))) {
    				this._Lcc_Pw = reader.GetString(_.Lcc_Pw);
    			}
    			if ((false == reader.IsDBNull(_.Lcc_Price))) {
    				this._Lcc_Price = reader.GetFloat(_.Lcc_Price);
    			}
    			if ((false == reader.IsDBNull(_.Lcc_Coupon))) {
    				this._Lcc_Coupon = reader.GetInt32(_.Lcc_Coupon);
    			}
    			if ((false == reader.IsDBNull(_.Lcc_CrtTime))) {
    				this._Lcc_CrtTime = reader.GetDateTime(_.Lcc_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Lcc_UsedTime))) {
    				this._Lcc_UsedTime = reader.GetDateTime(_.Lcc_UsedTime);
    			}
    			if ((false == reader.IsDBNull(_.Lcc_IsEnable))) {
    				this._Lcc_IsEnable = reader.GetBoolean(_.Lcc_IsEnable);
    			}
    			if ((false == reader.IsDBNull(_.Lcc_IsUsed))) {
    				this._Lcc_IsUsed = reader.GetBoolean(_.Lcc_IsUsed);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_ID))) {
    				this._Lcs_ID = reader.GetInt32(_.Lcs_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_AccName))) {
    				this._Ac_AccName = reader.GetString(_.Ac_AccName);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Lcc_LimitStart))) {
    				this._Lcc_LimitStart = reader.GetDateTime(_.Lcc_LimitStart);
    			}
    			if ((false == reader.IsDBNull(_.Lcc_LimitEnd))) {
    				this._Lcc_LimitEnd = reader.GetDateTime(_.Lcc_LimitEnd);
    			}
    			if ((false == reader.IsDBNull(_.Lcc_State))) {
    				this._Lcc_State = reader.GetInt32(_.Lcc_State);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(LearningCardCode).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<LearningCardCode>();
    			
    			/// <summary>
    			/// 字段名：Lcc_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcc_ID = new WeiSha.Data.Field<LearningCardCode>("Lcc_ID");
    			
    			/// <summary>
    			/// 字段名：Lcc_Code - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lcc_Code = new WeiSha.Data.Field<LearningCardCode>("Lcc_Code");
    			
    			/// <summary>
    			/// 字段名：Lcc_Pw - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lcc_Pw = new WeiSha.Data.Field<LearningCardCode>("Lcc_Pw");
    			
    			/// <summary>
    			/// 字段名：Lcc_Price - 数据类型：Single
    			/// </summary>
    			public static WeiSha.Data.Field Lcc_Price = new WeiSha.Data.Field<LearningCardCode>("Lcc_Price");
    			
    			/// <summary>
    			/// 字段名：Lcc_Coupon - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcc_Coupon = new WeiSha.Data.Field<LearningCardCode>("Lcc_Coupon");
    			
    			/// <summary>
    			/// 字段名：Lcc_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lcc_CrtTime = new WeiSha.Data.Field<LearningCardCode>("Lcc_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Lcc_UsedTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lcc_UsedTime = new WeiSha.Data.Field<LearningCardCode>("Lcc_UsedTime");
    			
    			/// <summary>
    			/// 字段名：Lcc_IsEnable - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Lcc_IsEnable = new WeiSha.Data.Field<LearningCardCode>("Lcc_IsEnable");
    			
    			/// <summary>
    			/// 字段名：Lcc_IsUsed - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Lcc_IsUsed = new WeiSha.Data.Field<LearningCardCode>("Lcc_IsUsed");
    			
    			/// <summary>
    			/// 字段名：Lcs_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_ID = new WeiSha.Data.Field<LearningCardCode>("Lcs_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<LearningCardCode>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_AccName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_AccName = new WeiSha.Data.Field<LearningCardCode>("Ac_AccName");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<LearningCardCode>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Lcc_LimitStart - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lcc_LimitStart = new WeiSha.Data.Field<LearningCardCode>("Lcc_LimitStart");
    			
    			/// <summary>
    			/// 字段名：Lcc_LimitEnd - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lcc_LimitEnd = new WeiSha.Data.Field<LearningCardCode>("Lcc_LimitEnd");
    			
    			/// <summary>
    			/// 字段名：Lcc_State - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcc_State = new WeiSha.Data.Field<LearningCardCode>("Lcc_State");
    		}
    	}
    }
    