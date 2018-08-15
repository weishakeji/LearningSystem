namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：MoneyAccount 主键列：Ma_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class MoneyAccount : WeiSha.Data.Entity {
    		
    		protected Int32 _Ma_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected Decimal _Ma_Total;
    		
    		protected Decimal _Ma_Money;
    		
    		protected String _Ma_Source;
    		
    		protected Int32 _Ma_Type;
    		
    		protected String _Ma_Info;
    		
    		protected String _Ma_Remark;
    		
    		protected DateTime _Ma_CrtTime;
    		
    		protected String _Rc_Code;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Ma_Serial;
    		
    		protected Boolean _Ma_IsSuccess;
    		
    		protected Int32 _Ma_From;
    		
    		protected Int32 _Pai_ID;
    		
    		protected Int32 _Ma_Status;
    		
    		protected String _Ma_Buyer;
    		
    		protected String _Ma_Seller;
    		
    		public Int32 Ma_ID {
    			get {
    				return this._Ma_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ma_ID, _Ma_ID, value);
    				this._Ma_ID = value;
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
    		
    		public Decimal Ma_Total {
    			get {
    				return this._Ma_Total;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ma_Total, _Ma_Total, value);
    				this._Ma_Total = value;
    			}
    		}
    		
    		public Decimal Ma_Money {
    			get {
    				return this._Ma_Money;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ma_Money, _Ma_Money, value);
    				this._Ma_Money = value;
    			}
    		}
    		
    		public String Ma_Source {
    			get {
    				return this._Ma_Source;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ma_Source, _Ma_Source, value);
    				this._Ma_Source = value;
    			}
    		}
    		
    		public Int32 Ma_Type {
    			get {
    				return this._Ma_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ma_Type, _Ma_Type, value);
    				this._Ma_Type = value;
    			}
    		}
    		
    		public String Ma_Info {
    			get {
    				return this._Ma_Info;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ma_Info, _Ma_Info, value);
    				this._Ma_Info = value;
    			}
    		}
    		
    		public String Ma_Remark {
    			get {
    				return this._Ma_Remark;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ma_Remark, _Ma_Remark, value);
    				this._Ma_Remark = value;
    			}
    		}
    		
    		public DateTime Ma_CrtTime {
    			get {
    				return this._Ma_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ma_CrtTime, _Ma_CrtTime, value);
    				this._Ma_CrtTime = value;
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
    		
    		public Int32 Org_ID {
    			get {
    				return this._Org_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_ID, _Org_ID, value);
    				this._Org_ID = value;
    			}
    		}
    		
    		public String Ma_Serial {
    			get {
    				return this._Ma_Serial;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ma_Serial, _Ma_Serial, value);
    				this._Ma_Serial = value;
    			}
    		}
    		
    		public Boolean Ma_IsSuccess {
    			get {
    				return this._Ma_IsSuccess;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ma_IsSuccess, _Ma_IsSuccess, value);
    				this._Ma_IsSuccess = value;
    			}
    		}
    		
    		public Int32 Ma_From {
    			get {
    				return this._Ma_From;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ma_From, _Ma_From, value);
    				this._Ma_From = value;
    			}
    		}
    		
    		public Int32 Pai_ID {
    			get {
    				return this._Pai_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pai_ID, _Pai_ID, value);
    				this._Pai_ID = value;
    			}
    		}
    		
    		public Int32 Ma_Status {
    			get {
    				return this._Ma_Status;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ma_Status, _Ma_Status, value);
    				this._Ma_Status = value;
    			}
    		}
    		
    		public String Ma_Buyer {
    			get {
    				return this._Ma_Buyer;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ma_Buyer, _Ma_Buyer, value);
    				this._Ma_Buyer = value;
    			}
    		}
    		
    		public String Ma_Seller {
    			get {
    				return this._Ma_Seller;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ma_Seller, _Ma_Seller, value);
    				this._Ma_Seller = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<MoneyAccount>("MoneyAccount");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Ma_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ma_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ma_ID,
    					_.Ac_ID,
    					_.Ma_Total,
    					_.Ma_Money,
    					_.Ma_Source,
    					_.Ma_Type,
    					_.Ma_Info,
    					_.Ma_Remark,
    					_.Ma_CrtTime,
    					_.Rc_Code,
    					_.Org_ID,
    					_.Ma_Serial,
    					_.Ma_IsSuccess,
    					_.Ma_From,
    					_.Pai_ID,
    					_.Ma_Status,
    					_.Ma_Buyer,
    					_.Ma_Seller};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Ma_ID,
    					this._Ac_ID,
    					this._Ma_Total,
    					this._Ma_Money,
    					this._Ma_Source,
    					this._Ma_Type,
    					this._Ma_Info,
    					this._Ma_Remark,
    					this._Ma_CrtTime,
    					this._Rc_Code,
    					this._Org_ID,
    					this._Ma_Serial,
    					this._Ma_IsSuccess,
    					this._Ma_From,
    					this._Pai_ID,
    					this._Ma_Status,
    					this._Ma_Buyer,
    					this._Ma_Seller};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Ma_ID))) {
    				this._Ma_ID = reader.GetInt32(_.Ma_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ma_Total))) {
    				this._Ma_Total = reader.GetDecimal(_.Ma_Total);
    			}
    			if ((false == reader.IsDBNull(_.Ma_Money))) {
    				this._Ma_Money = reader.GetDecimal(_.Ma_Money);
    			}
    			if ((false == reader.IsDBNull(_.Ma_Source))) {
    				this._Ma_Source = reader.GetString(_.Ma_Source);
    			}
    			if ((false == reader.IsDBNull(_.Ma_Type))) {
    				this._Ma_Type = reader.GetInt32(_.Ma_Type);
    			}
    			if ((false == reader.IsDBNull(_.Ma_Info))) {
    				this._Ma_Info = reader.GetString(_.Ma_Info);
    			}
    			if ((false == reader.IsDBNull(_.Ma_Remark))) {
    				this._Ma_Remark = reader.GetString(_.Ma_Remark);
    			}
    			if ((false == reader.IsDBNull(_.Ma_CrtTime))) {
    				this._Ma_CrtTime = reader.GetDateTime(_.Ma_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Rc_Code))) {
    				this._Rc_Code = reader.GetString(_.Rc_Code);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ma_Serial))) {
    				this._Ma_Serial = reader.GetString(_.Ma_Serial);
    			}
    			if ((false == reader.IsDBNull(_.Ma_IsSuccess))) {
    				this._Ma_IsSuccess = reader.GetBoolean(_.Ma_IsSuccess);
    			}
    			if ((false == reader.IsDBNull(_.Ma_From))) {
    				this._Ma_From = reader.GetInt32(_.Ma_From);
    			}
    			if ((false == reader.IsDBNull(_.Pai_ID))) {
    				this._Pai_ID = reader.GetInt32(_.Pai_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ma_Status))) {
    				this._Ma_Status = reader.GetInt32(_.Ma_Status);
    			}
    			if ((false == reader.IsDBNull(_.Ma_Buyer))) {
    				this._Ma_Buyer = reader.GetString(_.Ma_Buyer);
    			}
    			if ((false == reader.IsDBNull(_.Ma_Seller))) {
    				this._Ma_Seller = reader.GetString(_.Ma_Seller);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(MoneyAccount).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<MoneyAccount>();
    			
    			/// <summary>
    			/// 字段名：Ma_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ma_ID = new WeiSha.Data.Field<MoneyAccount>("Ma_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<MoneyAccount>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ma_Total - 数据类型：Decimal
    			/// </summary>
    			public static WeiSha.Data.Field Ma_Total = new WeiSha.Data.Field<MoneyAccount>("Ma_Total");
    			
    			/// <summary>
    			/// 字段名：Ma_Money - 数据类型：Decimal
    			/// </summary>
    			public static WeiSha.Data.Field Ma_Money = new WeiSha.Data.Field<MoneyAccount>("Ma_Money");
    			
    			/// <summary>
    			/// 字段名：Ma_Source - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ma_Source = new WeiSha.Data.Field<MoneyAccount>("Ma_Source");
    			
    			/// <summary>
    			/// 字段名：Ma_Type - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ma_Type = new WeiSha.Data.Field<MoneyAccount>("Ma_Type");
    			
    			/// <summary>
    			/// 字段名：Ma_Info - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ma_Info = new WeiSha.Data.Field<MoneyAccount>("Ma_Info");
    			
    			/// <summary>
    			/// 字段名：Ma_Remark - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ma_Remark = new WeiSha.Data.Field<MoneyAccount>("Ma_Remark");
    			
    			/// <summary>
    			/// 字段名：Ma_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Ma_CrtTime = new WeiSha.Data.Field<MoneyAccount>("Ma_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Rc_Code - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Rc_Code = new WeiSha.Data.Field<MoneyAccount>("Rc_Code");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<MoneyAccount>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Ma_Serial - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ma_Serial = new WeiSha.Data.Field<MoneyAccount>("Ma_Serial");
    			
    			/// <summary>
    			/// 字段名：Ma_IsSuccess - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ma_IsSuccess = new WeiSha.Data.Field<MoneyAccount>("Ma_IsSuccess");
    			
    			/// <summary>
    			/// 字段名：Ma_From - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ma_From = new WeiSha.Data.Field<MoneyAccount>("Ma_From");
    			
    			/// <summary>
    			/// 字段名：Pai_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pai_ID = new WeiSha.Data.Field<MoneyAccount>("Pai_ID");
    			
    			/// <summary>
    			/// 字段名：Ma_Status - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ma_Status = new WeiSha.Data.Field<MoneyAccount>("Ma_Status");
    			
    			/// <summary>
    			/// 字段名：Ma_Buyer - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ma_Buyer = new WeiSha.Data.Field<MoneyAccount>("Ma_Buyer");
    			
    			/// <summary>
    			/// 字段名：Ma_Seller - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ma_Seller = new WeiSha.Data.Field<MoneyAccount>("Ma_Seller");
    		}
    	}
    }
    