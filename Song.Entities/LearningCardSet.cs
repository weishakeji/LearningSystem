namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：LearningCardSet 主键列：Lcs_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class LearningCardSet : WeiSha.Data.Entity {
    		
    		protected Int32 _Lcs_ID;
    		
    		protected String _Lcs_RelatedCourses;
    		
    		protected Int32 _Lcs_CoursesCount;
    		
    		protected Int32 _Lcs_Count;
    		
    		protected Int32 _Lcs_MaxCount;
    		
    		protected Int32 _Lcs_BuildCount;
    		
    		protected Boolean _Lcs_IsFixed;
    		
    		protected Single _Lcs_Price;
    		
    		protected Int32 _Lcs_Coupon;
    		
    		protected Int32 _Lcs_Span;
    		
    		protected String _Lcs_Unit;
    		
    		protected DateTime _Lcs_CrtTime;
    		
    		protected String _Lcs_Theme;
    		
    		protected String _Lcs_Intro;
    		
    		protected Int32 _Lsc_UsedCount;
    		
    		protected Int32 _Org_ID;
    		
    		protected DateTime _Lcs_LimitStart;
    		
    		protected DateTime _Lcs_LimitEnd;
    		
    		protected Boolean _Lcs_IsEnable;
    		
    		protected Int32 _Lcs_CodeLength;
    		
    		protected Int32 _Lcs_PwLength;
    		
    		protected String _Lcs_SecretKey;
    		
    		public Int32 Lcs_ID {
    			get {
    				return this._Lcs_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_ID, _Lcs_ID, value);
    				this._Lcs_ID = value;
    			}
    		}
    		
    		public String Lcs_RelatedCourses {
    			get {
    				return this._Lcs_RelatedCourses;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_RelatedCourses, _Lcs_RelatedCourses, value);
    				this._Lcs_RelatedCourses = value;
    			}
    		}
    		
    		public Int32 Lcs_CoursesCount {
    			get {
    				return this._Lcs_CoursesCount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_CoursesCount, _Lcs_CoursesCount, value);
    				this._Lcs_CoursesCount = value;
    			}
    		}
    		
    		public Int32 Lcs_Count {
    			get {
    				return this._Lcs_Count;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_Count, _Lcs_Count, value);
    				this._Lcs_Count = value;
    			}
    		}
    		
    		public Int32 Lcs_MaxCount {
    			get {
    				return this._Lcs_MaxCount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_MaxCount, _Lcs_MaxCount, value);
    				this._Lcs_MaxCount = value;
    			}
    		}
    		
    		public Int32 Lcs_BuildCount {
    			get {
    				return this._Lcs_BuildCount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_BuildCount, _Lcs_BuildCount, value);
    				this._Lcs_BuildCount = value;
    			}
    		}
    		
    		public Boolean Lcs_IsFixed {
    			get {
    				return this._Lcs_IsFixed;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_IsFixed, _Lcs_IsFixed, value);
    				this._Lcs_IsFixed = value;
    			}
    		}
    		
    		public Single Lcs_Price {
    			get {
    				return this._Lcs_Price;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_Price, _Lcs_Price, value);
    				this._Lcs_Price = value;
    			}
    		}
    		
    		public Int32 Lcs_Coupon {
    			get {
    				return this._Lcs_Coupon;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_Coupon, _Lcs_Coupon, value);
    				this._Lcs_Coupon = value;
    			}
    		}
    		
    		public Int32 Lcs_Span {
    			get {
    				return this._Lcs_Span;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_Span, _Lcs_Span, value);
    				this._Lcs_Span = value;
    			}
    		}
    		
    		public String Lcs_Unit {
    			get {
    				return this._Lcs_Unit;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_Unit, _Lcs_Unit, value);
    				this._Lcs_Unit = value;
    			}
    		}
    		
    		public DateTime Lcs_CrtTime {
    			get {
    				return this._Lcs_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_CrtTime, _Lcs_CrtTime, value);
    				this._Lcs_CrtTime = value;
    			}
    		}
    		
    		public String Lcs_Theme {
    			get {
    				return this._Lcs_Theme;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_Theme, _Lcs_Theme, value);
    				this._Lcs_Theme = value;
    			}
    		}
    		
    		public String Lcs_Intro {
    			get {
    				return this._Lcs_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_Intro, _Lcs_Intro, value);
    				this._Lcs_Intro = value;
    			}
    		}
    		
    		public Int32 Lsc_UsedCount {
    			get {
    				return this._Lsc_UsedCount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lsc_UsedCount, _Lsc_UsedCount, value);
    				this._Lsc_UsedCount = value;
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
    		
    		public DateTime Lcs_LimitStart {
    			get {
    				return this._Lcs_LimitStart;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_LimitStart, _Lcs_LimitStart, value);
    				this._Lcs_LimitStart = value;
    			}
    		}
    		
    		public DateTime Lcs_LimitEnd {
    			get {
    				return this._Lcs_LimitEnd;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_LimitEnd, _Lcs_LimitEnd, value);
    				this._Lcs_LimitEnd = value;
    			}
    		}
    		
    		public Boolean Lcs_IsEnable {
    			get {
    				return this._Lcs_IsEnable;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_IsEnable, _Lcs_IsEnable, value);
    				this._Lcs_IsEnable = value;
    			}
    		}
    		
    		public Int32 Lcs_CodeLength {
    			get {
    				return this._Lcs_CodeLength;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_CodeLength, _Lcs_CodeLength, value);
    				this._Lcs_CodeLength = value;
    			}
    		}
    		
    		public Int32 Lcs_PwLength {
    			get {
    				return this._Lcs_PwLength;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_PwLength, _Lcs_PwLength, value);
    				this._Lcs_PwLength = value;
    			}
    		}
    		
    		public String Lcs_SecretKey {
    			get {
    				return this._Lcs_SecretKey;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcs_SecretKey, _Lcs_SecretKey, value);
    				this._Lcs_SecretKey = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<LearningCardSet>("LearningCardSet");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Lcs_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lcs_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lcs_ID,
    					_.Lcs_RelatedCourses,
    					_.Lcs_CoursesCount,
    					_.Lcs_Count,
    					_.Lcs_MaxCount,
    					_.Lcs_BuildCount,
    					_.Lcs_IsFixed,
    					_.Lcs_Price,
    					_.Lcs_Coupon,
    					_.Lcs_Span,
    					_.Lcs_Unit,
    					_.Lcs_CrtTime,
    					_.Lcs_Theme,
    					_.Lcs_Intro,
    					_.Lsc_UsedCount,
    					_.Org_ID,
    					_.Lcs_LimitStart,
    					_.Lcs_LimitEnd,
    					_.Lcs_IsEnable,
    					_.Lcs_CodeLength,
    					_.Lcs_PwLength,
    					_.Lcs_SecretKey};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Lcs_ID,
    					this._Lcs_RelatedCourses,
    					this._Lcs_CoursesCount,
    					this._Lcs_Count,
    					this._Lcs_MaxCount,
    					this._Lcs_BuildCount,
    					this._Lcs_IsFixed,
    					this._Lcs_Price,
    					this._Lcs_Coupon,
    					this._Lcs_Span,
    					this._Lcs_Unit,
    					this._Lcs_CrtTime,
    					this._Lcs_Theme,
    					this._Lcs_Intro,
    					this._Lsc_UsedCount,
    					this._Org_ID,
    					this._Lcs_LimitStart,
    					this._Lcs_LimitEnd,
    					this._Lcs_IsEnable,
    					this._Lcs_CodeLength,
    					this._Lcs_PwLength,
    					this._Lcs_SecretKey};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Lcs_ID))) {
    				this._Lcs_ID = reader.GetInt32(_.Lcs_ID);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_RelatedCourses))) {
    				this._Lcs_RelatedCourses = reader.GetString(_.Lcs_RelatedCourses);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_CoursesCount))) {
    				this._Lcs_CoursesCount = reader.GetInt32(_.Lcs_CoursesCount);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_Count))) {
    				this._Lcs_Count = reader.GetInt32(_.Lcs_Count);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_MaxCount))) {
    				this._Lcs_MaxCount = reader.GetInt32(_.Lcs_MaxCount);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_BuildCount))) {
    				this._Lcs_BuildCount = reader.GetInt32(_.Lcs_BuildCount);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_IsFixed))) {
    				this._Lcs_IsFixed = reader.GetBoolean(_.Lcs_IsFixed);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_Price))) {
    				this._Lcs_Price = reader.GetFloat(_.Lcs_Price);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_Coupon))) {
    				this._Lcs_Coupon = reader.GetInt32(_.Lcs_Coupon);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_Span))) {
    				this._Lcs_Span = reader.GetInt32(_.Lcs_Span);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_Unit))) {
    				this._Lcs_Unit = reader.GetString(_.Lcs_Unit);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_CrtTime))) {
    				this._Lcs_CrtTime = reader.GetDateTime(_.Lcs_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_Theme))) {
    				this._Lcs_Theme = reader.GetString(_.Lcs_Theme);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_Intro))) {
    				this._Lcs_Intro = reader.GetString(_.Lcs_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Lsc_UsedCount))) {
    				this._Lsc_UsedCount = reader.GetInt32(_.Lsc_UsedCount);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_LimitStart))) {
    				this._Lcs_LimitStart = reader.GetDateTime(_.Lcs_LimitStart);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_LimitEnd))) {
    				this._Lcs_LimitEnd = reader.GetDateTime(_.Lcs_LimitEnd);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_IsEnable))) {
    				this._Lcs_IsEnable = reader.GetBoolean(_.Lcs_IsEnable);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_CodeLength))) {
    				this._Lcs_CodeLength = reader.GetInt32(_.Lcs_CodeLength);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_PwLength))) {
    				this._Lcs_PwLength = reader.GetInt32(_.Lcs_PwLength);
    			}
    			if ((false == reader.IsDBNull(_.Lcs_SecretKey))) {
    				this._Lcs_SecretKey = reader.GetString(_.Lcs_SecretKey);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(LearningCardSet).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<LearningCardSet>();
    			
    			/// <summary>
    			/// 字段名：Lcs_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_ID = new WeiSha.Data.Field<LearningCardSet>("Lcs_ID");
    			
    			/// <summary>
    			/// 字段名：Lcs_RelatedCourses - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_RelatedCourses = new WeiSha.Data.Field<LearningCardSet>("Lcs_RelatedCourses");
    			
    			/// <summary>
    			/// 字段名：Lcs_CoursesCount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_CoursesCount = new WeiSha.Data.Field<LearningCardSet>("Lcs_CoursesCount");
    			
    			/// <summary>
    			/// 字段名：Lcs_Count - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_Count = new WeiSha.Data.Field<LearningCardSet>("Lcs_Count");
    			
    			/// <summary>
    			/// 字段名：Lcs_MaxCount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_MaxCount = new WeiSha.Data.Field<LearningCardSet>("Lcs_MaxCount");
    			
    			/// <summary>
    			/// 字段名：Lcs_BuildCount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_BuildCount = new WeiSha.Data.Field<LearningCardSet>("Lcs_BuildCount");
    			
    			/// <summary>
    			/// 字段名：Lcs_IsFixed - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_IsFixed = new WeiSha.Data.Field<LearningCardSet>("Lcs_IsFixed");
    			
    			/// <summary>
    			/// 字段名：Lcs_Price - 数据类型：Single
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_Price = new WeiSha.Data.Field<LearningCardSet>("Lcs_Price");
    			
    			/// <summary>
    			/// 字段名：Lcs_Coupon - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_Coupon = new WeiSha.Data.Field<LearningCardSet>("Lcs_Coupon");
    			
    			/// <summary>
    			/// 字段名：Lcs_Span - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_Span = new WeiSha.Data.Field<LearningCardSet>("Lcs_Span");
    			
    			/// <summary>
    			/// 字段名：Lcs_Unit - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_Unit = new WeiSha.Data.Field<LearningCardSet>("Lcs_Unit");
    			
    			/// <summary>
    			/// 字段名：Lcs_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_CrtTime = new WeiSha.Data.Field<LearningCardSet>("Lcs_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Lcs_Theme - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_Theme = new WeiSha.Data.Field<LearningCardSet>("Lcs_Theme");
    			
    			/// <summary>
    			/// 字段名：Lcs_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_Intro = new WeiSha.Data.Field<LearningCardSet>("Lcs_Intro");
    			
    			/// <summary>
    			/// 字段名：Lsc_UsedCount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lsc_UsedCount = new WeiSha.Data.Field<LearningCardSet>("Lsc_UsedCount");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<LearningCardSet>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Lcs_LimitStart - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_LimitStart = new WeiSha.Data.Field<LearningCardSet>("Lcs_LimitStart");
    			
    			/// <summary>
    			/// 字段名：Lcs_LimitEnd - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_LimitEnd = new WeiSha.Data.Field<LearningCardSet>("Lcs_LimitEnd");
    			
    			/// <summary>
    			/// 字段名：Lcs_IsEnable - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_IsEnable = new WeiSha.Data.Field<LearningCardSet>("Lcs_IsEnable");
    			
    			/// <summary>
    			/// 字段名：Lcs_CodeLength - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_CodeLength = new WeiSha.Data.Field<LearningCardSet>("Lcs_CodeLength");
    			
    			/// <summary>
    			/// 字段名：Lcs_PwLength - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_PwLength = new WeiSha.Data.Field<LearningCardSet>("Lcs_PwLength");
    			
    			/// <summary>
    			/// 字段名：Lcs_SecretKey - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lcs_SecretKey = new WeiSha.Data.Field<LearningCardSet>("Lcs_SecretKey");
    		}
    	}
    }
    