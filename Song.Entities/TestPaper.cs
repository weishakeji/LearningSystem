namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：TestPaper 主键列：Tp_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class TestPaper : WeiSha.Data.Entity {
    		
    		protected Int32 _Tp_Id;
    		
    		protected String _Tp_Name;
    		
    		protected Int32 _Tp_Type;
    		
    		protected String _Tp_Intro;
    		
    		protected String _Tp_Logo;
    		
    		protected String _Tp_Author;
    		
    		protected Int32 _Tp_Total;
    		
    		protected Int32 _Tp_Count;
    		
    		protected DateTime _Tp_CrtTime;
    		
    		protected DateTime _Tp_Lasttime;
    		
    		protected Int32 _Sbj_ID;
    		
    		protected String _Sbj_Name;
    		
    		protected Int32 _Tp_Diff;
    		
    		protected Boolean _Tp_IsRec;
    		
    		protected Boolean _Tp_IsBuild;
    		
    		protected Boolean _Tp_IsUse;
    		
    		protected Int32 _Tp_Span;
    		
    		protected String _Tp_UID;
    		
    		protected Int32 _Tp_Diff2;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Int32 _Th_ID;
    		
    		protected String _Th_Name;
    		
    		protected Int32 _Tp_PassScore;
    		
    		protected Int32 _Cou_ID;
    		
    		protected String _Tp_Remind;
    		
    		protected String _Tp_SubName;
    		
    		protected Int32 _Tp_FromType;
    		
    		protected String _Tp_FromConfig;
    		
    		protected String _Cou_Name;
    		
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
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Tp_Name {
    			get {
    				return this._Tp_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Name, _Tp_Name, value);
    				this._Tp_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Tp_Type {
    			get {
    				return this._Tp_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Type, _Tp_Type, value);
    				this._Tp_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Tp_Intro {
    			get {
    				return this._Tp_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Intro, _Tp_Intro, value);
    				this._Tp_Intro = value;
    			}
    		}
    		
    		public String Tp_Logo {
    			get {
    				return this._Tp_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Logo, _Tp_Logo, value);
    				this._Tp_Logo = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Tp_Author {
    			get {
    				return this._Tp_Author;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Author, _Tp_Author, value);
    				this._Tp_Author = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Tp_Total {
    			get {
    				return this._Tp_Total;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Total, _Tp_Total, value);
    				this._Tp_Total = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Tp_Count {
    			get {
    				return this._Tp_Count;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Count, _Tp_Count, value);
    				this._Tp_Count = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime Tp_CrtTime {
    			get {
    				return this._Tp_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_CrtTime, _Tp_CrtTime, value);
    				this._Tp_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime Tp_Lasttime {
    			get {
    				return this._Tp_Lasttime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Lasttime, _Tp_Lasttime, value);
    				this._Tp_Lasttime = value;
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
    		public Int32 Tp_Diff {
    			get {
    				return this._Tp_Diff;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Diff, _Tp_Diff, value);
    				this._Tp_Diff = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Tp_IsRec {
    			get {
    				return this._Tp_IsRec;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_IsRec, _Tp_IsRec, value);
    				this._Tp_IsRec = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Tp_IsBuild {
    			get {
    				return this._Tp_IsBuild;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_IsBuild, _Tp_IsBuild, value);
    				this._Tp_IsBuild = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Tp_IsUse {
    			get {
    				return this._Tp_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_IsUse, _Tp_IsUse, value);
    				this._Tp_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Tp_Span {
    			get {
    				return this._Tp_Span;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Span, _Tp_Span, value);
    				this._Tp_Span = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Tp_UID {
    			get {
    				return this._Tp_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_UID, _Tp_UID, value);
    				this._Tp_UID = value;
    			}
    		}
    		
    		public Int32 Tp_Diff2 {
    			get {
    				return this._Tp_Diff2;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Diff2, _Tp_Diff2, value);
    				this._Tp_Diff2 = value;
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
    		
    		public Int32 Th_ID {
    			get {
    				return this._Th_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_ID, _Th_ID, value);
    				this._Th_ID = value;
    			}
    		}
    		
    		public String Th_Name {
    			get {
    				return this._Th_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Name, _Th_Name, value);
    				this._Th_Name = value;
    			}
    		}
    		
    		public Int32 Tp_PassScore {
    			get {
    				return this._Tp_PassScore;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_PassScore, _Tp_PassScore, value);
    				this._Tp_PassScore = value;
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
    		
    		public String Tp_Remind {
    			get {
    				return this._Tp_Remind;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Remind, _Tp_Remind, value);
    				this._Tp_Remind = value;
    			}
    		}
    		
    		public String Tp_SubName {
    			get {
    				return this._Tp_SubName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_SubName, _Tp_SubName, value);
    				this._Tp_SubName = value;
    			}
    		}
    		
    		public Int32 Tp_FromType {
    			get {
    				return this._Tp_FromType;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_FromType, _Tp_FromType, value);
    				this._Tp_FromType = value;
    			}
    		}
    		
    		public String Tp_FromConfig {
    			get {
    				return this._Tp_FromConfig;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_FromConfig, _Tp_FromConfig, value);
    				this._Tp_FromConfig = value;
    			}
    		}
    		
    		public String Cou_Name {
    			get {
    				return this._Cou_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_Name, _Cou_Name, value);
    				this._Cou_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<TestPaper>("TestPaper");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Tp_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Tp_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Tp_Id,
    					_.Tp_Name,
    					_.Tp_Type,
    					_.Tp_Intro,
    					_.Tp_Logo,
    					_.Tp_Author,
    					_.Tp_Total,
    					_.Tp_Count,
    					_.Tp_CrtTime,
    					_.Tp_Lasttime,
    					_.Sbj_ID,
    					_.Sbj_Name,
    					_.Tp_Diff,
    					_.Tp_IsRec,
    					_.Tp_IsBuild,
    					_.Tp_IsUse,
    					_.Tp_Span,
    					_.Tp_UID,
    					_.Tp_Diff2,
    					_.Org_ID,
    					_.Org_Name,
    					_.Th_ID,
    					_.Th_Name,
    					_.Tp_PassScore,
    					_.Cou_ID,
    					_.Tp_Remind,
    					_.Tp_SubName,
    					_.Tp_FromType,
    					_.Tp_FromConfig,
    					_.Cou_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Tp_Id,
    					this._Tp_Name,
    					this._Tp_Type,
    					this._Tp_Intro,
    					this._Tp_Logo,
    					this._Tp_Author,
    					this._Tp_Total,
    					this._Tp_Count,
    					this._Tp_CrtTime,
    					this._Tp_Lasttime,
    					this._Sbj_ID,
    					this._Sbj_Name,
    					this._Tp_Diff,
    					this._Tp_IsRec,
    					this._Tp_IsBuild,
    					this._Tp_IsUse,
    					this._Tp_Span,
    					this._Tp_UID,
    					this._Tp_Diff2,
    					this._Org_ID,
    					this._Org_Name,
    					this._Th_ID,
    					this._Th_Name,
    					this._Tp_PassScore,
    					this._Cou_ID,
    					this._Tp_Remind,
    					this._Tp_SubName,
    					this._Tp_FromType,
    					this._Tp_FromConfig,
    					this._Cou_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Tp_Id))) {
    				this._Tp_Id = reader.GetInt32(_.Tp_Id);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Name))) {
    				this._Tp_Name = reader.GetString(_.Tp_Name);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Type))) {
    				this._Tp_Type = reader.GetInt32(_.Tp_Type);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Intro))) {
    				this._Tp_Intro = reader.GetString(_.Tp_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Logo))) {
    				this._Tp_Logo = reader.GetString(_.Tp_Logo);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Author))) {
    				this._Tp_Author = reader.GetString(_.Tp_Author);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Total))) {
    				this._Tp_Total = reader.GetInt32(_.Tp_Total);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Count))) {
    				this._Tp_Count = reader.GetInt32(_.Tp_Count);
    			}
    			if ((false == reader.IsDBNull(_.Tp_CrtTime))) {
    				this._Tp_CrtTime = reader.GetDateTime(_.Tp_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Lasttime))) {
    				this._Tp_Lasttime = reader.GetDateTime(_.Tp_Lasttime);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt32(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Name))) {
    				this._Sbj_Name = reader.GetString(_.Sbj_Name);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Diff))) {
    				this._Tp_Diff = reader.GetInt32(_.Tp_Diff);
    			}
    			if ((false == reader.IsDBNull(_.Tp_IsRec))) {
    				this._Tp_IsRec = reader.GetBoolean(_.Tp_IsRec);
    			}
    			if ((false == reader.IsDBNull(_.Tp_IsBuild))) {
    				this._Tp_IsBuild = reader.GetBoolean(_.Tp_IsBuild);
    			}
    			if ((false == reader.IsDBNull(_.Tp_IsUse))) {
    				this._Tp_IsUse = reader.GetBoolean(_.Tp_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Span))) {
    				this._Tp_Span = reader.GetInt32(_.Tp_Span);
    			}
    			if ((false == reader.IsDBNull(_.Tp_UID))) {
    				this._Tp_UID = reader.GetString(_.Tp_UID);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Diff2))) {
    				this._Tp_Diff2 = reader.GetInt32(_.Tp_Diff2);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Th_ID))) {
    				this._Th_ID = reader.GetInt32(_.Th_ID);
    			}
    			if ((false == reader.IsDBNull(_.Th_Name))) {
    				this._Th_Name = reader.GetString(_.Th_Name);
    			}
    			if ((false == reader.IsDBNull(_.Tp_PassScore))) {
    				this._Tp_PassScore = reader.GetInt32(_.Tp_PassScore);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Remind))) {
    				this._Tp_Remind = reader.GetString(_.Tp_Remind);
    			}
    			if ((false == reader.IsDBNull(_.Tp_SubName))) {
    				this._Tp_SubName = reader.GetString(_.Tp_SubName);
    			}
    			if ((false == reader.IsDBNull(_.Tp_FromType))) {
    				this._Tp_FromType = reader.GetInt32(_.Tp_FromType);
    			}
    			if ((false == reader.IsDBNull(_.Tp_FromConfig))) {
    				this._Tp_FromConfig = reader.GetString(_.Tp_FromConfig);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Name))) {
    				this._Cou_Name = reader.GetString(_.Cou_Name);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(TestPaper).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<TestPaper>();
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Id = new WeiSha.Data.Field<TestPaper>("Tp_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Name = new WeiSha.Data.Field<TestPaper>("Tp_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Type - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Type = new WeiSha.Data.Field<TestPaper>("Tp_Type");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Intro = new WeiSha.Data.Field<TestPaper>("Tp_Intro");
    			
    			/// <summary>
    			/// 字段名：Tp_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Logo = new WeiSha.Data.Field<TestPaper>("Tp_Logo");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Author - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Author = new WeiSha.Data.Field<TestPaper>("Tp_Author");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Total - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Total = new WeiSha.Data.Field<TestPaper>("Tp_Total");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Count - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Count = new WeiSha.Data.Field<TestPaper>("Tp_Count");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Tp_CrtTime = new WeiSha.Data.Field<TestPaper>("Tp_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Lasttime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Lasttime = new WeiSha.Data.Field<TestPaper>("Tp_Lasttime");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<TestPaper>("Sbj_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Name = new WeiSha.Data.Field<TestPaper>("Sbj_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Diff - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Diff = new WeiSha.Data.Field<TestPaper>("Tp_Diff");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_IsRec - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Tp_IsRec = new WeiSha.Data.Field<TestPaper>("Tp_IsRec");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_IsBuild - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Tp_IsBuild = new WeiSha.Data.Field<TestPaper>("Tp_IsBuild");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Tp_IsUse = new WeiSha.Data.Field<TestPaper>("Tp_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Span - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Span = new WeiSha.Data.Field<TestPaper>("Tp_Span");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tp_UID = new WeiSha.Data.Field<TestPaper>("Tp_UID");
    			
    			/// <summary>
    			/// 字段名：Tp_Diff2 - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Diff2 = new WeiSha.Data.Field<TestPaper>("Tp_Diff2");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<TestPaper>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<TestPaper>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Th_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_ID = new WeiSha.Data.Field<TestPaper>("Th_ID");
    			
    			/// <summary>
    			/// 字段名：Th_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Name = new WeiSha.Data.Field<TestPaper>("Th_Name");
    			
    			/// <summary>
    			/// 字段名：Tp_PassScore - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tp_PassScore = new WeiSha.Data.Field<TestPaper>("Tp_PassScore");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<TestPaper>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Tp_Remind - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Remind = new WeiSha.Data.Field<TestPaper>("Tp_Remind");
    			
    			/// <summary>
    			/// 字段名：Tp_SubName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tp_SubName = new WeiSha.Data.Field<TestPaper>("Tp_SubName");
    			
    			/// <summary>
    			/// 字段名：Tp_FromType - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tp_FromType = new WeiSha.Data.Field<TestPaper>("Tp_FromType");
    			
    			/// <summary>
    			/// 字段名：Tp_FromConfig - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tp_FromConfig = new WeiSha.Data.Field<TestPaper>("Tp_FromConfig");
    			
    			/// <summary>
    			/// 字段名：Cou_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Name = new WeiSha.Data.Field<TestPaper>("Cou_Name");
    		}
    	}
    }
    