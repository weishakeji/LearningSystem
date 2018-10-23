namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：RechargeCode 主键列：Rc_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class RechargeCode : WeiSha.Data.Entity {
    		
    		protected Int32 _Rc_ID;
    		
    		protected String _Rc_Code;
    		
    		protected String _Rc_Pw;
    		
    		protected Int32 _Rc_Price;
    		
    		protected DateTime _Rc_CrtTime;
    		
    		protected DateTime _Rc_UsedTime;
    		
    		protected Boolean? _Rc_IsEnable;
    		
    		protected Boolean _Rc_IsUsed;
    		
    		protected Int32 _Rc_Type;
    		
    		protected Int32 _Rs_ID;
    		
    		protected Int32 _Org_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected String _Ac_AccName;
    		
    		protected DateTime _Rc_LimitStart;
    		
    		protected DateTime _Rc_LimitEnd;
    		
    		protected String _Rc_QrcodeBase64;
    		
    		public Int32 Rc_ID {
    			get {
    				return this._Rc_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rc_ID, _Rc_ID, value);
    				this._Rc_ID = value;
    			}
    		}
    		
    		public String Rc_Code {
    			get {
    				return this._Rc_Code;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rc_Code, _Rc_Code, value);
    				this._Rc_Code = value;
    			}
    		}
    		
    		public String Rc_Pw {
    			get {
    				return this._Rc_Pw;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rc_Pw, _Rc_Pw, value);
    				this._Rc_Pw = value;
    			}
    		}
    		
    		public Int32 Rc_Price {
    			get {
    				return this._Rc_Price;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rc_Price, _Rc_Price, value);
    				this._Rc_Price = value;
    			}
    		}
    		
    		public DateTime Rc_CrtTime {
    			get {
    				return this._Rc_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rc_CrtTime, _Rc_CrtTime, value);
    				this._Rc_CrtTime = value;
    			}
    		}
    		
    		public DateTime Rc_UsedTime {
    			get {
    				return this._Rc_UsedTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rc_UsedTime, _Rc_UsedTime, value);
    				this._Rc_UsedTime = value;
    			}
    		}
    		
    		public Boolean? Rc_IsEnable {
    			get {
    				return this._Rc_IsEnable;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rc_IsEnable, _Rc_IsEnable, value);
    				this._Rc_IsEnable = value;
    			}
    		}
    		
    		public Boolean Rc_IsUsed {
    			get {
    				return this._Rc_IsUsed;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rc_IsUsed, _Rc_IsUsed, value);
    				this._Rc_IsUsed = value;
    			}
    		}
    		
    		public Int32 Rc_Type {
    			get {
    				return this._Rc_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rc_Type, _Rc_Type, value);
    				this._Rc_Type = value;
    			}
    		}
    		
    		public Int32 Rs_ID {
    			get {
    				return this._Rs_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rs_ID, _Rs_ID, value);
    				this._Rs_ID = value;
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
    		
    		public DateTime Rc_LimitStart {
    			get {
    				return this._Rc_LimitStart;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rc_LimitStart, _Rc_LimitStart, value);
    				this._Rc_LimitStart = value;
    			}
    		}
    		
    		public DateTime Rc_LimitEnd {
    			get {
    				return this._Rc_LimitEnd;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rc_LimitEnd, _Rc_LimitEnd, value);
    				this._Rc_LimitEnd = value;
    			}
    		}
    		
    		public String Rc_QrcodeBase64 {
    			get {
    				return this._Rc_QrcodeBase64;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rc_QrcodeBase64, _Rc_QrcodeBase64, value);
    				this._Rc_QrcodeBase64 = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<RechargeCode>("RechargeCode");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Rc_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Rc_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Rc_ID,
    					_.Rc_Code,
    					_.Rc_Pw,
    					_.Rc_Price,
    					_.Rc_CrtTime,
    					_.Rc_UsedTime,
    					_.Rc_IsEnable,
    					_.Rc_IsUsed,
    					_.Rc_Type,
    					_.Rs_ID,
    					_.Org_ID,
    					_.Ac_ID,
    					_.Ac_AccName,
    					_.Rc_LimitStart,
    					_.Rc_LimitEnd,
    					_.Rc_QrcodeBase64};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Rc_ID,
    					this._Rc_Code,
    					this._Rc_Pw,
    					this._Rc_Price,
    					this._Rc_CrtTime,
    					this._Rc_UsedTime,
    					this._Rc_IsEnable,
    					this._Rc_IsUsed,
    					this._Rc_Type,
    					this._Rs_ID,
    					this._Org_ID,
    					this._Ac_ID,
    					this._Ac_AccName,
    					this._Rc_LimitStart,
    					this._Rc_LimitEnd,
    					this._Rc_QrcodeBase64};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Rc_ID))) {
    				this._Rc_ID = reader.GetInt32(_.Rc_ID);
    			}
    			if ((false == reader.IsDBNull(_.Rc_Code))) {
    				this._Rc_Code = reader.GetString(_.Rc_Code);
    			}
    			if ((false == reader.IsDBNull(_.Rc_Pw))) {
    				this._Rc_Pw = reader.GetString(_.Rc_Pw);
    			}
    			if ((false == reader.IsDBNull(_.Rc_Price))) {
    				this._Rc_Price = reader.GetInt32(_.Rc_Price);
    			}
    			if ((false == reader.IsDBNull(_.Rc_CrtTime))) {
    				this._Rc_CrtTime = reader.GetDateTime(_.Rc_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Rc_UsedTime))) {
    				this._Rc_UsedTime = reader.GetDateTime(_.Rc_UsedTime);
    			}
    			if ((false == reader.IsDBNull(_.Rc_IsEnable))) {
    				this._Rc_IsEnable = reader.GetBoolean(_.Rc_IsEnable);
    			}
    			if ((false == reader.IsDBNull(_.Rc_IsUsed))) {
    				this._Rc_IsUsed = reader.GetBoolean(_.Rc_IsUsed);
    			}
    			if ((false == reader.IsDBNull(_.Rc_Type))) {
    				this._Rc_Type = reader.GetInt32(_.Rc_Type);
    			}
    			if ((false == reader.IsDBNull(_.Rs_ID))) {
    				this._Rs_ID = reader.GetInt32(_.Rs_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_AccName))) {
    				this._Ac_AccName = reader.GetString(_.Ac_AccName);
    			}
    			if ((false == reader.IsDBNull(_.Rc_LimitStart))) {
    				this._Rc_LimitStart = reader.GetDateTime(_.Rc_LimitStart);
    			}
    			if ((false == reader.IsDBNull(_.Rc_LimitEnd))) {
    				this._Rc_LimitEnd = reader.GetDateTime(_.Rc_LimitEnd);
    			}
    			if ((false == reader.IsDBNull(_.Rc_QrcodeBase64))) {
    				this._Rc_QrcodeBase64 = reader.GetString(_.Rc_QrcodeBase64);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(RechargeCode).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<RechargeCode>();
    			
    			/// <summary>
    			/// 字段名：Rc_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Rc_ID = new WeiSha.Data.Field<RechargeCode>("Rc_ID");
    			
    			/// <summary>
    			/// 字段名：Rc_Code - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Rc_Code = new WeiSha.Data.Field<RechargeCode>("Rc_Code");
    			
    			/// <summary>
    			/// 字段名：Rc_Pw - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Rc_Pw = new WeiSha.Data.Field<RechargeCode>("Rc_Pw");
    			
    			/// <summary>
    			/// 字段名：Rc_Price - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Rc_Price = new WeiSha.Data.Field<RechargeCode>("Rc_Price");
    			
    			/// <summary>
    			/// 字段名：Rc_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Rc_CrtTime = new WeiSha.Data.Field<RechargeCode>("Rc_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Rc_UsedTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Rc_UsedTime = new WeiSha.Data.Field<RechargeCode>("Rc_UsedTime");
    			
    			/// <summary>
    			/// 字段名：Rc_IsEnable - 数据类型：Boolean(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Rc_IsEnable = new WeiSha.Data.Field<RechargeCode>("Rc_IsEnable");
    			
    			/// <summary>
    			/// 字段名：Rc_IsUsed - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Rc_IsUsed = new WeiSha.Data.Field<RechargeCode>("Rc_IsUsed");
    			
    			/// <summary>
    			/// 字段名：Rc_Type - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Rc_Type = new WeiSha.Data.Field<RechargeCode>("Rc_Type");
    			
    			/// <summary>
    			/// 字段名：Rs_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Rs_ID = new WeiSha.Data.Field<RechargeCode>("Rs_ID");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<RechargeCode>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<RechargeCode>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_AccName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_AccName = new WeiSha.Data.Field<RechargeCode>("Ac_AccName");
    			
    			/// <summary>
    			/// 字段名：Rc_LimitStart - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Rc_LimitStart = new WeiSha.Data.Field<RechargeCode>("Rc_LimitStart");
    			
    			/// <summary>
    			/// 字段名：Rc_LimitEnd - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Rc_LimitEnd = new WeiSha.Data.Field<RechargeCode>("Rc_LimitEnd");
    			
    			/// <summary>
    			/// 字段名：Rc_QrcodeBase64 - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Rc_QrcodeBase64 = new WeiSha.Data.Field<RechargeCode>("Rc_QrcodeBase64");
    		}
    	}
    }
    