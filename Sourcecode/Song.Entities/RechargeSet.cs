namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：RechargeSet 主键列：Rs_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class RechargeSet : WeiSha.Data.Entity {
    		
    		protected Int32 _Rs_ID;
    		
    		protected Int32 _Rs_Count;
    		
    		protected Int32 _Rs_Price;
    		
    		protected DateTime _Rs_CrtTime;
    		
    		protected String _Rs_Theme;
    		
    		protected String _Rs_Intro;
    		
    		protected String _Rs_Pw;
    		
    		protected Int32 _Rs_UsedCount;
    		
    		protected Int32 _Org_ID;
    		
    		protected DateTime _Rs_LimitStart;
    		
    		protected DateTime _Rs_LimitEnd;
    		
    		protected Boolean _Rs_IsEnable;
    		
    		protected Int32 _Rs_CodeLength;
    		
    		protected Int32 _Rs_PwLength;
    		
    		public Int32 Rs_ID {
    			get {
    				return this._Rs_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rs_ID, _Rs_ID, value);
    				this._Rs_ID = value;
    			}
    		}
    		
    		public Int32 Rs_Count {
    			get {
    				return this._Rs_Count;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rs_Count, _Rs_Count, value);
    				this._Rs_Count = value;
    			}
    		}
    		
    		public Int32 Rs_Price {
    			get {
    				return this._Rs_Price;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rs_Price, _Rs_Price, value);
    				this._Rs_Price = value;
    			}
    		}
    		
    		public DateTime Rs_CrtTime {
    			get {
    				return this._Rs_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rs_CrtTime, _Rs_CrtTime, value);
    				this._Rs_CrtTime = value;
    			}
    		}
    		
    		public String Rs_Theme {
    			get {
    				return this._Rs_Theme;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rs_Theme, _Rs_Theme, value);
    				this._Rs_Theme = value;
    			}
    		}
    		
    		public String Rs_Intro {
    			get {
    				return this._Rs_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rs_Intro, _Rs_Intro, value);
    				this._Rs_Intro = value;
    			}
    		}
    		
    		public String Rs_Pw {
    			get {
    				return this._Rs_Pw;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rs_Pw, _Rs_Pw, value);
    				this._Rs_Pw = value;
    			}
    		}
    		
    		public Int32 Rs_UsedCount {
    			get {
    				return this._Rs_UsedCount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rs_UsedCount, _Rs_UsedCount, value);
    				this._Rs_UsedCount = value;
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
    		
    		public DateTime Rs_LimitStart {
    			get {
    				return this._Rs_LimitStart;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rs_LimitStart, _Rs_LimitStart, value);
    				this._Rs_LimitStart = value;
    			}
    		}
    		
    		public DateTime Rs_LimitEnd {
    			get {
    				return this._Rs_LimitEnd;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rs_LimitEnd, _Rs_LimitEnd, value);
    				this._Rs_LimitEnd = value;
    			}
    		}
    		
    		public Boolean Rs_IsEnable {
    			get {
    				return this._Rs_IsEnable;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rs_IsEnable, _Rs_IsEnable, value);
    				this._Rs_IsEnable = value;
    			}
    		}
    		
    		public Int32 Rs_CodeLength {
    			get {
    				return this._Rs_CodeLength;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rs_CodeLength, _Rs_CodeLength, value);
    				this._Rs_CodeLength = value;
    			}
    		}
    		
    		public Int32 Rs_PwLength {
    			get {
    				return this._Rs_PwLength;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rs_PwLength, _Rs_PwLength, value);
    				this._Rs_PwLength = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<RechargeSet>("RechargeSet");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Rs_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Rs_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Rs_ID,
    					_.Rs_Count,
    					_.Rs_Price,
    					_.Rs_CrtTime,
    					_.Rs_Theme,
    					_.Rs_Intro,
    					_.Rs_Pw,
    					_.Rs_UsedCount,
    					_.Org_ID,
    					_.Rs_LimitStart,
    					_.Rs_LimitEnd,
    					_.Rs_IsEnable,
    					_.Rs_CodeLength,
    					_.Rs_PwLength};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Rs_ID,
    					this._Rs_Count,
    					this._Rs_Price,
    					this._Rs_CrtTime,
    					this._Rs_Theme,
    					this._Rs_Intro,
    					this._Rs_Pw,
    					this._Rs_UsedCount,
    					this._Org_ID,
    					this._Rs_LimitStart,
    					this._Rs_LimitEnd,
    					this._Rs_IsEnable,
    					this._Rs_CodeLength,
    					this._Rs_PwLength};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Rs_ID))) {
    				this._Rs_ID = reader.GetInt32(_.Rs_ID);
    			}
    			if ((false == reader.IsDBNull(_.Rs_Count))) {
    				this._Rs_Count = reader.GetInt32(_.Rs_Count);
    			}
    			if ((false == reader.IsDBNull(_.Rs_Price))) {
    				this._Rs_Price = reader.GetInt32(_.Rs_Price);
    			}
    			if ((false == reader.IsDBNull(_.Rs_CrtTime))) {
    				this._Rs_CrtTime = reader.GetDateTime(_.Rs_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Rs_Theme))) {
    				this._Rs_Theme = reader.GetString(_.Rs_Theme);
    			}
    			if ((false == reader.IsDBNull(_.Rs_Intro))) {
    				this._Rs_Intro = reader.GetString(_.Rs_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Rs_Pw))) {
    				this._Rs_Pw = reader.GetString(_.Rs_Pw);
    			}
    			if ((false == reader.IsDBNull(_.Rs_UsedCount))) {
    				this._Rs_UsedCount = reader.GetInt32(_.Rs_UsedCount);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Rs_LimitStart))) {
    				this._Rs_LimitStart = reader.GetDateTime(_.Rs_LimitStart);
    			}
    			if ((false == reader.IsDBNull(_.Rs_LimitEnd))) {
    				this._Rs_LimitEnd = reader.GetDateTime(_.Rs_LimitEnd);
    			}
    			if ((false == reader.IsDBNull(_.Rs_IsEnable))) {
    				this._Rs_IsEnable = reader.GetBoolean(_.Rs_IsEnable);
    			}
    			if ((false == reader.IsDBNull(_.Rs_CodeLength))) {
    				this._Rs_CodeLength = reader.GetInt32(_.Rs_CodeLength);
    			}
    			if ((false == reader.IsDBNull(_.Rs_PwLength))) {
    				this._Rs_PwLength = reader.GetInt32(_.Rs_PwLength);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(RechargeSet).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<RechargeSet>();
    			
    			/// <summary>
    			/// 字段名：Rs_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Rs_ID = new WeiSha.Data.Field<RechargeSet>("Rs_ID");
    			
    			/// <summary>
    			/// 字段名：Rs_Count - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Rs_Count = new WeiSha.Data.Field<RechargeSet>("Rs_Count");
    			
    			/// <summary>
    			/// 字段名：Rs_Price - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Rs_Price = new WeiSha.Data.Field<RechargeSet>("Rs_Price");
    			
    			/// <summary>
    			/// 字段名：Rs_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Rs_CrtTime = new WeiSha.Data.Field<RechargeSet>("Rs_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Rs_Theme - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Rs_Theme = new WeiSha.Data.Field<RechargeSet>("Rs_Theme");
    			
    			/// <summary>
    			/// 字段名：Rs_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Rs_Intro = new WeiSha.Data.Field<RechargeSet>("Rs_Intro");
    			
    			/// <summary>
    			/// 字段名：Rs_Pw - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Rs_Pw = new WeiSha.Data.Field<RechargeSet>("Rs_Pw");
    			
    			/// <summary>
    			/// 字段名：Rs_UsedCount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Rs_UsedCount = new WeiSha.Data.Field<RechargeSet>("Rs_UsedCount");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<RechargeSet>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Rs_LimitStart - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Rs_LimitStart = new WeiSha.Data.Field<RechargeSet>("Rs_LimitStart");
    			
    			/// <summary>
    			/// 字段名：Rs_LimitEnd - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Rs_LimitEnd = new WeiSha.Data.Field<RechargeSet>("Rs_LimitEnd");
    			
    			/// <summary>
    			/// 字段名：Rs_IsEnable - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Rs_IsEnable = new WeiSha.Data.Field<RechargeSet>("Rs_IsEnable");
    			
    			/// <summary>
    			/// 字段名：Rs_CodeLength - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Rs_CodeLength = new WeiSha.Data.Field<RechargeSet>("Rs_CodeLength");
    			
    			/// <summary>
    			/// 字段名：Rs_PwLength - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Rs_PwLength = new WeiSha.Data.Field<RechargeSet>("Rs_PwLength");
    		}
    	}
    }
    