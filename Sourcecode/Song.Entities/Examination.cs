namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Examination 主键列：Exam_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Examination : WeiSha.Data.Entity {
    		
    		protected Int32 _Exam_ID;
    		
    		protected String _Exam_Title;
    		
    		protected DateTime _Exam_CrtTime;
    		
    		protected String _Exam_Intro;
    		
    		protected Boolean _Exam_IsUse;
    		
    		protected Boolean _Exam_IsTheme;
    		
    		protected Int32 _Exam_GroupType;
    		
    		protected String _Exam_Monitor;
    		
    		protected DateTime _Exam_Date;
    		
    		protected Int32 _Exam_Span;
    		
    		protected Int32 _Sbj_ID;
    		
    		protected String _Sbj_Name;
    		
    		protected String _Exam_UID;
    		
    		protected Int32 _Th_ID;
    		
    		protected String _Th_Name;
    		
    		protected Int32 _Tp_Id;
    		
    		protected Int32 _Exam_PassScore;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected String _Exam_Name;
    		
    		protected Int32 _Exam_Total;
    		
    		protected Int32 _Exam_Tax;
    		
    		protected DateTime _Exam_DateOver;
    		
    		protected Int32 _Exam_DateType;
    		
    		protected Boolean _Exam_IsToggle;
    		
    		protected Boolean _Exam_IsShowBtn;
    		
    		protected Boolean _Exam_IsRightClick;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Exam_ID {
    			get {
    				return this._Exam_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_ID, _Exam_ID, value);
    				this._Exam_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Exam_Title {
    			get {
    				return this._Exam_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_Title, _Exam_Title, value);
    				this._Exam_Title = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime Exam_CrtTime {
    			get {
    				return this._Exam_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_CrtTime, _Exam_CrtTime, value);
    				this._Exam_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Exam_Intro {
    			get {
    				return this._Exam_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_Intro, _Exam_Intro, value);
    				this._Exam_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Exam_IsUse {
    			get {
    				return this._Exam_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_IsUse, _Exam_IsUse, value);
    				this._Exam_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Exam_IsTheme {
    			get {
    				return this._Exam_IsTheme;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_IsTheme, _Exam_IsTheme, value);
    				this._Exam_IsTheme = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Exam_GroupType {
    			get {
    				return this._Exam_GroupType;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_GroupType, _Exam_GroupType, value);
    				this._Exam_GroupType = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Exam_Monitor {
    			get {
    				return this._Exam_Monitor;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_Monitor, _Exam_Monitor, value);
    				this._Exam_Monitor = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime Exam_Date {
    			get {
    				return this._Exam_Date;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_Date, _Exam_Date, value);
    				this._Exam_Date = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Exam_Span {
    			get {
    				return this._Exam_Span;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_Span, _Exam_Span, value);
    				this._Exam_Span = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Sbj_ID {
    			get {
    				return this._Sbj_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_ID, _Sbj_ID, value);
    				this._Sbj_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sbj_Name {
    			get {
    				return this._Sbj_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_Name, _Sbj_Name, value);
    				this._Sbj_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Exam_UID {
    			get {
    				return this._Exam_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_UID, _Exam_UID, value);
    				this._Exam_UID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Th_ID {
    			get {
    				return this._Th_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_ID, _Th_ID, value);
    				this._Th_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Th_Name {
    			get {
    				return this._Th_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Name, _Th_Name, value);
    				this._Th_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Tp_Id {
    			get {
    				return this._Tp_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Id, _Tp_Id, value);
    				this._Tp_Id = value;
    			}
    		}
    		
    		public Int32 Exam_PassScore {
    			get {
    				return this._Exam_PassScore;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_PassScore, _Exam_PassScore, value);
    				this._Exam_PassScore = value;
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
    		/// -1
    		/// </summary>
    		public String Exam_Name {
    			get {
    				return this._Exam_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_Name, _Exam_Name, value);
    				this._Exam_Name = value;
    			}
    		}
    		
    		public Int32 Exam_Total {
    			get {
    				return this._Exam_Total;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_Total, _Exam_Total, value);
    				this._Exam_Total = value;
    			}
    		}
    		
    		public Int32 Exam_Tax {
    			get {
    				return this._Exam_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_Tax, _Exam_Tax, value);
    				this._Exam_Tax = value;
    			}
    		}
    		
    		public DateTime Exam_DateOver {
    			get {
    				return this._Exam_DateOver;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_DateOver, _Exam_DateOver, value);
    				this._Exam_DateOver = value;
    			}
    		}
    		
    		public Int32 Exam_DateType {
    			get {
    				return this._Exam_DateType;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_DateType, _Exam_DateType, value);
    				this._Exam_DateType = value;
    			}
    		}
    		
    		public Boolean Exam_IsToggle {
    			get {
    				return this._Exam_IsToggle;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_IsToggle, _Exam_IsToggle, value);
    				this._Exam_IsToggle = value;
    			}
    		}
    		
    		public Boolean Exam_IsShowBtn {
    			get {
    				return this._Exam_IsShowBtn;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_IsShowBtn, _Exam_IsShowBtn, value);
    				this._Exam_IsShowBtn = value;
    			}
    		}
    		
    		public Boolean Exam_IsRightClick {
    			get {
    				return this._Exam_IsRightClick;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_IsRightClick, _Exam_IsRightClick, value);
    				this._Exam_IsRightClick = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Examination>("Examination");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Exam_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Exam_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Exam_ID,
    					_.Exam_Title,
    					_.Exam_CrtTime,
    					_.Exam_Intro,
    					_.Exam_IsUse,
    					_.Exam_IsTheme,
    					_.Exam_GroupType,
    					_.Exam_Monitor,
    					_.Exam_Date,
    					_.Exam_Span,
    					_.Sbj_ID,
    					_.Sbj_Name,
    					_.Exam_UID,
    					_.Th_ID,
    					_.Th_Name,
    					_.Tp_Id,
    					_.Exam_PassScore,
    					_.Org_ID,
    					_.Org_Name,
    					_.Exam_Name,
    					_.Exam_Total,
    					_.Exam_Tax,
    					_.Exam_DateOver,
    					_.Exam_DateType,
    					_.Exam_IsToggle,
    					_.Exam_IsShowBtn,
    					_.Exam_IsRightClick};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Exam_ID,
    					this._Exam_Title,
    					this._Exam_CrtTime,
    					this._Exam_Intro,
    					this._Exam_IsUse,
    					this._Exam_IsTheme,
    					this._Exam_GroupType,
    					this._Exam_Monitor,
    					this._Exam_Date,
    					this._Exam_Span,
    					this._Sbj_ID,
    					this._Sbj_Name,
    					this._Exam_UID,
    					this._Th_ID,
    					this._Th_Name,
    					this._Tp_Id,
    					this._Exam_PassScore,
    					this._Org_ID,
    					this._Org_Name,
    					this._Exam_Name,
    					this._Exam_Total,
    					this._Exam_Tax,
    					this._Exam_DateOver,
    					this._Exam_DateType,
    					this._Exam_IsToggle,
    					this._Exam_IsShowBtn,
    					this._Exam_IsRightClick};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Exam_ID))) {
    				this._Exam_ID = reader.GetInt32(_.Exam_ID);
    			}
    			if ((false == reader.IsDBNull(_.Exam_Title))) {
    				this._Exam_Title = reader.GetString(_.Exam_Title);
    			}
    			if ((false == reader.IsDBNull(_.Exam_CrtTime))) {
    				this._Exam_CrtTime = reader.GetDateTime(_.Exam_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Exam_Intro))) {
    				this._Exam_Intro = reader.GetString(_.Exam_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Exam_IsUse))) {
    				this._Exam_IsUse = reader.GetBoolean(_.Exam_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Exam_IsTheme))) {
    				this._Exam_IsTheme = reader.GetBoolean(_.Exam_IsTheme);
    			}
    			if ((false == reader.IsDBNull(_.Exam_GroupType))) {
    				this._Exam_GroupType = reader.GetInt32(_.Exam_GroupType);
    			}
    			if ((false == reader.IsDBNull(_.Exam_Monitor))) {
    				this._Exam_Monitor = reader.GetString(_.Exam_Monitor);
    			}
    			if ((false == reader.IsDBNull(_.Exam_Date))) {
    				this._Exam_Date = reader.GetDateTime(_.Exam_Date);
    			}
    			if ((false == reader.IsDBNull(_.Exam_Span))) {
    				this._Exam_Span = reader.GetInt32(_.Exam_Span);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt32(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Name))) {
    				this._Sbj_Name = reader.GetString(_.Sbj_Name);
    			}
    			if ((false == reader.IsDBNull(_.Exam_UID))) {
    				this._Exam_UID = reader.GetString(_.Exam_UID);
    			}
    			if ((false == reader.IsDBNull(_.Th_ID))) {
    				this._Th_ID = reader.GetInt32(_.Th_ID);
    			}
    			if ((false == reader.IsDBNull(_.Th_Name))) {
    				this._Th_Name = reader.GetString(_.Th_Name);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Id))) {
    				this._Tp_Id = reader.GetInt32(_.Tp_Id);
    			}
    			if ((false == reader.IsDBNull(_.Exam_PassScore))) {
    				this._Exam_PassScore = reader.GetInt32(_.Exam_PassScore);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Exam_Name))) {
    				this._Exam_Name = reader.GetString(_.Exam_Name);
    			}
    			if ((false == reader.IsDBNull(_.Exam_Total))) {
    				this._Exam_Total = reader.GetInt32(_.Exam_Total);
    			}
    			if ((false == reader.IsDBNull(_.Exam_Tax))) {
    				this._Exam_Tax = reader.GetInt32(_.Exam_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Exam_DateOver))) {
    				this._Exam_DateOver = reader.GetDateTime(_.Exam_DateOver);
    			}
    			if ((false == reader.IsDBNull(_.Exam_DateType))) {
    				this._Exam_DateType = reader.GetInt32(_.Exam_DateType);
    			}
    			if ((false == reader.IsDBNull(_.Exam_IsToggle))) {
    				this._Exam_IsToggle = reader.GetBoolean(_.Exam_IsToggle);
    			}
    			if ((false == reader.IsDBNull(_.Exam_IsShowBtn))) {
    				this._Exam_IsShowBtn = reader.GetBoolean(_.Exam_IsShowBtn);
    			}
    			if ((false == reader.IsDBNull(_.Exam_IsRightClick))) {
    				this._Exam_IsRightClick = reader.GetBoolean(_.Exam_IsRightClick);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Examination).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Examination>();
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Exam_ID = new WeiSha.Data.Field<Examination>("Exam_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exam_Title = new WeiSha.Data.Field<Examination>("Exam_Title");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Exam_CrtTime = new WeiSha.Data.Field<Examination>("Exam_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exam_Intro = new WeiSha.Data.Field<Examination>("Exam_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Exam_IsUse = new WeiSha.Data.Field<Examination>("Exam_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_IsTheme - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Exam_IsTheme = new WeiSha.Data.Field<Examination>("Exam_IsTheme");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_GroupType - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Exam_GroupType = new WeiSha.Data.Field<Examination>("Exam_GroupType");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_Monitor - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exam_Monitor = new WeiSha.Data.Field<Examination>("Exam_Monitor");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_Date - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Exam_Date = new WeiSha.Data.Field<Examination>("Exam_Date");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_Span - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Exam_Span = new WeiSha.Data.Field<Examination>("Exam_Span");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<Examination>("Sbj_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Name = new WeiSha.Data.Field<Examination>("Sbj_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exam_UID = new WeiSha.Data.Field<Examination>("Exam_UID");
    			
    			/// <summary>
    			/// -1 - 字段名：Th_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_ID = new WeiSha.Data.Field<Examination>("Th_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Th_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Name = new WeiSha.Data.Field<Examination>("Th_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Id = new WeiSha.Data.Field<Examination>("Tp_Id");
    			
    			/// <summary>
    			/// 字段名：Exam_PassScore - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Exam_PassScore = new WeiSha.Data.Field<Examination>("Exam_PassScore");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Examination>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Examination>("Org_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exam_Name = new WeiSha.Data.Field<Examination>("Exam_Name");
    			
    			/// <summary>
    			/// 字段名：Exam_Total - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Exam_Total = new WeiSha.Data.Field<Examination>("Exam_Total");
    			
    			/// <summary>
    			/// 字段名：Exam_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Exam_Tax = new WeiSha.Data.Field<Examination>("Exam_Tax");
    			
    			/// <summary>
    			/// 字段名：Exam_DateOver - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Exam_DateOver = new WeiSha.Data.Field<Examination>("Exam_DateOver");
    			
    			/// <summary>
    			/// 字段名：Exam_DateType - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Exam_DateType = new WeiSha.Data.Field<Examination>("Exam_DateType");
    			
    			/// <summary>
    			/// 字段名：Exam_IsToggle - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Exam_IsToggle = new WeiSha.Data.Field<Examination>("Exam_IsToggle");
    			
    			/// <summary>
    			/// 字段名：Exam_IsShowBtn - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Exam_IsShowBtn = new WeiSha.Data.Field<Examination>("Exam_IsShowBtn");
    			
    			/// <summary>
    			/// 字段名：Exam_IsRightClick - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Exam_IsRightClick = new WeiSha.Data.Field<Examination>("Exam_IsRightClick");
    		}
    	}
    }
    