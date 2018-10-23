namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：LearningCard 主键列：Lc_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class LearningCard : WeiSha.Data.Entity {
    		
    		protected Int32 _Lc_ID;
    		
    		protected String _Lc_Code;
    		
    		protected String _Lc_Pw;
    		
    		protected Single _Lc_Price;
    		
    		protected Int32 _Lc_Coupon;
    		
    		protected DateTime _Lc_CrtTime;
    		
    		protected DateTime _Lc_UsedTime;
    		
    		protected Boolean _Lc_IsEnable;
    		
    		protected Boolean _Lc_IsUsed;
    		
    		protected Int32 _Lcs_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected String _Ac_AccName;
    		
    		protected Int32 _Org_ID;
    		
    		protected DateTime _Lc_LimitStart;
    		
    		protected DateTime _Lc_LimitEnd;
    		
    		protected Int32 _Lc_State;
    		
    		protected String _Lc_QrcodeBase64;
    		
    		protected Int32 _Lc_Span;
    		
    		public Int32 Lc_ID {
    			get {
    				return this._Lc_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_ID, _Lc_ID, value);
    				this._Lc_ID = value;
    			}
    		}
    		
    		public String Lc_Code {
    			get {
    				return this._Lc_Code;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_Code, _Lc_Code, value);
    				this._Lc_Code = value;
    			}
    		}
    		
    		public String Lc_Pw {
    			get {
    				return this._Lc_Pw;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_Pw, _Lc_Pw, value);
    				this._Lc_Pw = value;
    			}
    		}
    		
    		public Single Lc_Price {
    			get {
    				return this._Lc_Price;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_Price, _Lc_Price, value);
    				this._Lc_Price = value;
    			}
    		}
    		
    		public Int32 Lc_Coupon {
    			get {
    				return this._Lc_Coupon;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_Coupon, _Lc_Coupon, value);
    				this._Lc_Coupon = value;
    			}
    		}
    		
    		public DateTime Lc_CrtTime {
    			get {
    				return this._Lc_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_CrtTime, _Lc_CrtTime, value);
    				this._Lc_CrtTime = value;
    			}
    		}
    		
    		public DateTime Lc_UsedTime {
    			get {
    				return this._Lc_UsedTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_UsedTime, _Lc_UsedTime, value);
    				this._Lc_UsedTime = value;
    			}
    		}
    		
    		public Boolean Lc_IsEnable {
    			get {
    				return this._Lc_IsEnable;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_IsEnable, _Lc_IsEnable, value);
    				this._Lc_IsEnable = value;
    			}
    		}
    		
    		public Boolean Lc_IsUsed {
    			get {
    				return this._Lc_IsUsed;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_IsUsed, _Lc_IsUsed, value);
    				this._Lc_IsUsed = value;
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
    		
    		public DateTime Lc_LimitStart {
    			get {
    				return this._Lc_LimitStart;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_LimitStart, _Lc_LimitStart, value);
    				this._Lc_LimitStart = value;
    			}
    		}
    		
    		public DateTime Lc_LimitEnd {
    			get {
    				return this._Lc_LimitEnd;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_LimitEnd, _Lc_LimitEnd, value);
    				this._Lc_LimitEnd = value;
    			}
    		}
    		
    		public Int32 Lc_State {
    			get {
    				return this._Lc_State;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_State, _Lc_State, value);
    				this._Lc_State = value;
    			}
    		}
    		
    		public String Lc_QrcodeBase64 {
    			get {
    				return this._Lc_QrcodeBase64;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_QrcodeBase64, _Lc_QrcodeBase64, value);
    				this._Lc_QrcodeBase64 = value;
    			}
    		}
    		
    		public Int32 Lc_Span {
    			get {
    				return this._Lc_Span;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_Span, _Lc_Span, value);
    				this._Lc_Span = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<LearningCard>("LearningCard");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Lc_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lc_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lc_ID,
    					_.Lc_Code,
    					_.Lc_Pw,
    					_.Lc_Price,
    					_.Lc_Coupon,
    					_.Lc_CrtTime,
    					_.Lc_UsedTime,
    					_.Lc_IsEnable,
    					_.Lc_IsUsed,
    					_.Lcs_ID,
    					_.Ac_ID,
    					_.Ac_AccName,
    					_.Org_ID,
    					_.Lc_LimitStart,
    					_.Lc_LimitEnd,
    					_.Lc_State,
    					_.Lc_QrcodeBase64,
    					_.Lc_Span};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Lc_ID,
    					this._Lc_Code,
    					this._Lc_Pw,
    					this._Lc_Price,
    					this._Lc_Coupon,
    					this._Lc_CrtTime,
    					this._Lc_UsedTime,
    					this._Lc_IsEnable,
    					this._Lc_IsUsed,
    					this._Lcs_ID,
    					this._Ac_ID,
    					this._Ac_AccName,
    					this._Org_ID,
    					this._Lc_LimitStart,
    					this._Lc_LimitEnd,
    					this._Lc_State,
    					this._Lc_QrcodeBase64,
    					this._Lc_Span};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Lc_ID))) {
    				this._Lc_ID = reader.GetInt32(_.Lc_ID);
    			}
    			if ((false == reader.IsDBNull(_.Lc_Code))) {
    				this._Lc_Code = reader.GetString(_.Lc_Code);
    			}
    			if ((false == reader.IsDBNull(_.Lc_Pw))) {
    				this._Lc_Pw = reader.GetString(_.Lc_Pw);
    			}
    			if ((false == reader.IsDBNull(_.Lc_Price))) {
    				this._Lc_Price = reader.GetFloat(_.Lc_Price);
    			}
    			if ((false == reader.IsDBNull(_.Lc_Coupon))) {
    				this._Lc_Coupon = reader.GetInt32(_.Lc_Coupon);
    			}
    			if ((false == reader.IsDBNull(_.Lc_CrtTime))) {
    				this._Lc_CrtTime = reader.GetDateTime(_.Lc_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Lc_UsedTime))) {
    				this._Lc_UsedTime = reader.GetDateTime(_.Lc_UsedTime);
    			}
    			if ((false == reader.IsDBNull(_.Lc_IsEnable))) {
    				this._Lc_IsEnable = reader.GetBoolean(_.Lc_IsEnable);
    			}
    			if ((false == reader.IsDBNull(_.Lc_IsUsed))) {
    				this._Lc_IsUsed = reader.GetBoolean(_.Lc_IsUsed);
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
    			if ((false == reader.IsDBNull(_.Lc_LimitStart))) {
    				this._Lc_LimitStart = reader.GetDateTime(_.Lc_LimitStart);
    			}
    			if ((false == reader.IsDBNull(_.Lc_LimitEnd))) {
    				this._Lc_LimitEnd = reader.GetDateTime(_.Lc_LimitEnd);
    			}
    			if ((false == reader.IsDBNull(_.Lc_State))) {
    				this._Lc_State = reader.GetInt32(_.Lc_State);
    			}
    			if ((false == reader.IsDBNull(_.Lc_QrcodeBase64))) {
    				this._Lc_QrcodeBase64 = reader.GetString(_.Lc_QrcodeBase64);
    			}
    			if ((false == reader.IsDBNull(_.Lc_Span))) {
    				this._Lc_Span = reader.GetInt32(_.Lc_Span);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(LearningCard).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<LearningCard>();
    			
    			/// <summary>
    			/// 字段名：Lc_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lc_ID = new WeiSha.Data.Field<LearningCard>("Lc_ID");
    			
    			/// <summary>
    			/// 字段名：Lc_Code - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lc_Code = new WeiSha.Data.Field<LearningCard>("Lc_Code");
    			
    			/// <summary>
    			/// 字段名：Lc_Pw - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lc_Pw = new WeiSha.Data.Field<LearningCard>("Lc_Pw");
    			
    			/// <summary>
    			/// 字段名：Lc_Price - 数据类型：Single
    			/// </summary>
    			public static WeiSha.Data.Field Lc_Price = new WeiSha.Data.Field<LearningCard>("Lc_Price");
    			
    			/// <summary>
    			/// 字段名：Lc_Coupon - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lc_Coupon = new WeiSha.Data.Field<LearningCard>("Lc_Coupon");
    			
    			/// <summary>
    			/// 字段名：Lc_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lc_CrtTime = new WeiSha.Data.Field<LearningCard>("Lc_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Lc_UsedTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lc_UsedTime = new WeiSha.Data.Field<LearningCard>("Lc_UsedTime");
    			
    			/// <summary>
    			/// 字段名：Lc_IsEnable - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Lc_IsEnable = new WeiSha.Data.Field<LearningCard>("Lc_IsEnable");
    			
    			/// <summary>
    			/// 字段名：Lc_IsUsed - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Lc_IsUsed = new WeiSha.Data.Field<LearningCard>("Lc_IsUsed");
    			
    			/// <summary>
    			/// 字段名：Lcs_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_ID = new WeiSha.Data.Field<LearningCard>("Lcs_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<LearningCard>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_AccName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_AccName = new WeiSha.Data.Field<LearningCard>("Ac_AccName");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<LearningCard>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Lc_LimitStart - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lc_LimitStart = new WeiSha.Data.Field<LearningCard>("Lc_LimitStart");
    			
    			/// <summary>
    			/// 字段名：Lc_LimitEnd - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lc_LimitEnd = new WeiSha.Data.Field<LearningCard>("Lc_LimitEnd");
    			
    			/// <summary>
    			/// 字段名：Lc_State - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lc_State = new WeiSha.Data.Field<LearningCard>("Lc_State");
    			
    			/// <summary>
    			/// 字段名：Lc_QrcodeBase64 - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lc_QrcodeBase64 = new WeiSha.Data.Field<LearningCard>("Lc_QrcodeBase64");
    			
    			/// <summary>
    			/// 字段名：Lc_Span - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lc_Span = new WeiSha.Data.Field<LearningCard>("Lc_Span");
    		}
    	}
    }
    