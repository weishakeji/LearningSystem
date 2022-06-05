namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：CouponAccount 主键列：Ca_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class CouponAccount : WeiSha.Data.Entity {
    		
    		protected Int32 _Ca_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected Int32 _Ca_Total;
    		
    		protected Int32 _Ca_TotalAmount;
    		
    		protected Int32 _Ca_Value;
    		
    		protected String _Ca_Source;
    		
    		protected Int32 _Ca_Type;
    		
    		protected String _Ca_Info;
    		
    		protected String _Ca_Remark;
    		
    		protected DateTime _Ca_CrtTime;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Ca_Serial;
    		
    		protected Int32 _Ca_From;
    		
    		protected String _Rc_Code;
    		
    		public Int32 Ca_ID {
    			get {
    				return this._Ca_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ca_ID, _Ca_ID, value);
    				this._Ca_ID = value;
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
    		
    		public Int32 Ca_Total {
    			get {
    				return this._Ca_Total;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ca_Total, _Ca_Total, value);
    				this._Ca_Total = value;
    			}
    		}
    		
    		public Int32 Ca_TotalAmount {
    			get {
    				return this._Ca_TotalAmount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ca_TotalAmount, _Ca_TotalAmount, value);
    				this._Ca_TotalAmount = value;
    			}
    		}
    		
    		public Int32 Ca_Value {
    			get {
    				return this._Ca_Value;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ca_Value, _Ca_Value, value);
    				this._Ca_Value = value;
    			}
    		}
    		
    		public String Ca_Source {
    			get {
    				return this._Ca_Source;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ca_Source, _Ca_Source, value);
    				this._Ca_Source = value;
    			}
    		}
    		
    		public Int32 Ca_Type {
    			get {
    				return this._Ca_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ca_Type, _Ca_Type, value);
    				this._Ca_Type = value;
    			}
    		}
    		
    		public String Ca_Info {
    			get {
    				return this._Ca_Info;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ca_Info, _Ca_Info, value);
    				this._Ca_Info = value;
    			}
    		}
    		
    		public String Ca_Remark {
    			get {
    				return this._Ca_Remark;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ca_Remark, _Ca_Remark, value);
    				this._Ca_Remark = value;
    			}
    		}
    		
    		public DateTime Ca_CrtTime {
    			get {
    				return this._Ca_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ca_CrtTime, _Ca_CrtTime, value);
    				this._Ca_CrtTime = value;
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
    		
    		public String Ca_Serial {
    			get {
    				return this._Ca_Serial;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ca_Serial, _Ca_Serial, value);
    				this._Ca_Serial = value;
    			}
    		}
    		
    		public Int32 Ca_From {
    			get {
    				return this._Ca_From;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ca_From, _Ca_From, value);
    				this._Ca_From = value;
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
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<CouponAccount>("CouponAccount");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Ca_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ca_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ca_ID,
    					_.Ac_ID,
    					_.Ca_Total,
    					_.Ca_TotalAmount,
    					_.Ca_Value,
    					_.Ca_Source,
    					_.Ca_Type,
    					_.Ca_Info,
    					_.Ca_Remark,
    					_.Ca_CrtTime,
    					_.Org_ID,
    					_.Ca_Serial,
    					_.Ca_From,
    					_.Rc_Code};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Ca_ID,
    					this._Ac_ID,
    					this._Ca_Total,
    					this._Ca_TotalAmount,
    					this._Ca_Value,
    					this._Ca_Source,
    					this._Ca_Type,
    					this._Ca_Info,
    					this._Ca_Remark,
    					this._Ca_CrtTime,
    					this._Org_ID,
    					this._Ca_Serial,
    					this._Ca_From,
    					this._Rc_Code};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Ca_ID))) {
    				this._Ca_ID = reader.GetInt32(_.Ca_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ca_Total))) {
    				this._Ca_Total = reader.GetInt32(_.Ca_Total);
    			}
    			if ((false == reader.IsDBNull(_.Ca_TotalAmount))) {
    				this._Ca_TotalAmount = reader.GetInt32(_.Ca_TotalAmount);
    			}
    			if ((false == reader.IsDBNull(_.Ca_Value))) {
    				this._Ca_Value = reader.GetInt32(_.Ca_Value);
    			}
    			if ((false == reader.IsDBNull(_.Ca_Source))) {
    				this._Ca_Source = reader.GetString(_.Ca_Source);
    			}
    			if ((false == reader.IsDBNull(_.Ca_Type))) {
    				this._Ca_Type = reader.GetInt32(_.Ca_Type);
    			}
    			if ((false == reader.IsDBNull(_.Ca_Info))) {
    				this._Ca_Info = reader.GetString(_.Ca_Info);
    			}
    			if ((false == reader.IsDBNull(_.Ca_Remark))) {
    				this._Ca_Remark = reader.GetString(_.Ca_Remark);
    			}
    			if ((false == reader.IsDBNull(_.Ca_CrtTime))) {
    				this._Ca_CrtTime = reader.GetDateTime(_.Ca_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ca_Serial))) {
    				this._Ca_Serial = reader.GetString(_.Ca_Serial);
    			}
    			if ((false == reader.IsDBNull(_.Ca_From))) {
    				this._Ca_From = reader.GetInt32(_.Ca_From);
    			}
    			if ((false == reader.IsDBNull(_.Rc_Code))) {
    				this._Rc_Code = reader.GetString(_.Rc_Code);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(CouponAccount).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<CouponAccount>();
    			
    			/// <summary>
    			/// 字段名：Ca_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ca_ID = new WeiSha.Data.Field<CouponAccount>("Ca_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<CouponAccount>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ca_Total - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ca_Total = new WeiSha.Data.Field<CouponAccount>("Ca_Total");
    			
    			/// <summary>
    			/// 字段名：Ca_TotalAmount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ca_TotalAmount = new WeiSha.Data.Field<CouponAccount>("Ca_TotalAmount");
    			
    			/// <summary>
    			/// 字段名：Ca_Value - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ca_Value = new WeiSha.Data.Field<CouponAccount>("Ca_Value");
    			
    			/// <summary>
    			/// 字段名：Ca_Source - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ca_Source = new WeiSha.Data.Field<CouponAccount>("Ca_Source");
    			
    			/// <summary>
    			/// 字段名：Ca_Type - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ca_Type = new WeiSha.Data.Field<CouponAccount>("Ca_Type");
    			
    			/// <summary>
    			/// 字段名：Ca_Info - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ca_Info = new WeiSha.Data.Field<CouponAccount>("Ca_Info");
    			
    			/// <summary>
    			/// 字段名：Ca_Remark - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ca_Remark = new WeiSha.Data.Field<CouponAccount>("Ca_Remark");
    			
    			/// <summary>
    			/// 字段名：Ca_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Ca_CrtTime = new WeiSha.Data.Field<CouponAccount>("Ca_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<CouponAccount>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Ca_Serial - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ca_Serial = new WeiSha.Data.Field<CouponAccount>("Ca_Serial");
    			
    			/// <summary>
    			/// 字段名：Ca_From - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ca_From = new WeiSha.Data.Field<CouponAccount>("Ca_From");
    			
    			/// <summary>
    			/// 字段名：Rc_Code - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Rc_Code = new WeiSha.Data.Field<CouponAccount>("Rc_Code");
    		}
    	}
    }
    