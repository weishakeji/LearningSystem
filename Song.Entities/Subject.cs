namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Subject 主键列：Sbj_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Subject : WeiSha.Data.Entity {
    		
    		protected Int32 _Sbj_ID;
    		
    		protected String _Sbj_Name;
    		
    		protected String _Sbj_ByName;
    		
    		protected String _Sbj_Intro;
    		
    		protected Int32 _Sbj_Tax;
    		
    		protected Boolean _Sbj_IsUse;
    		
    		protected DateTime _Sbj_CrtTime;
    		
    		protected Int32 _Sbj_PassScore;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Int32 _Sbj_PID;
    		
    		protected Int32 _Sbj_Level;
    		
    		protected String _Sbj_XPath;
    		
    		protected Int32 _Sbj_CouNumber;
    		
    		protected String _Sbj_Logo;
    		
    		protected String _Sbj_LogoSmall;
    		
    		protected Int32 _Dep_Id;
    		
    		protected String _Dep_CnName;
    		
    		protected Boolean _Sbj_IsRec;
    		
    		protected String _Sbj_Details;
    		
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
    		public String Sbj_ByName {
    			get {
    				return this._Sbj_ByName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_ByName, _Sbj_ByName, value);
    				this._Sbj_ByName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sbj_Intro {
    			get {
    				return this._Sbj_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_Intro, _Sbj_Intro, value);
    				this._Sbj_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Sbj_Tax {
    			get {
    				return this._Sbj_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_Tax, _Sbj_Tax, value);
    				this._Sbj_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Sbj_IsUse {
    			get {
    				return this._Sbj_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_IsUse, _Sbj_IsUse, value);
    				this._Sbj_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime Sbj_CrtTime {
    			get {
    				return this._Sbj_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_CrtTime, _Sbj_CrtTime, value);
    				this._Sbj_CrtTime = value;
    			}
    		}
    		
    		public Int32 Sbj_PassScore {
    			get {
    				return this._Sbj_PassScore;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_PassScore, _Sbj_PassScore, value);
    				this._Sbj_PassScore = value;
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
    		
    		public Int32 Sbj_PID {
    			get {
    				return this._Sbj_PID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_PID, _Sbj_PID, value);
    				this._Sbj_PID = value;
    			}
    		}
    		
    		public Int32 Sbj_Level {
    			get {
    				return this._Sbj_Level;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_Level, _Sbj_Level, value);
    				this._Sbj_Level = value;
    			}
    		}
    		
    		public String Sbj_XPath {
    			get {
    				return this._Sbj_XPath;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_XPath, _Sbj_XPath, value);
    				this._Sbj_XPath = value;
    			}
    		}
    		
    		public Int32 Sbj_CouNumber {
    			get {
    				return this._Sbj_CouNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_CouNumber, _Sbj_CouNumber, value);
    				this._Sbj_CouNumber = value;
    			}
    		}
    		
    		public String Sbj_Logo {
    			get {
    				return this._Sbj_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_Logo, _Sbj_Logo, value);
    				this._Sbj_Logo = value;
    			}
    		}
    		
    		public String Sbj_LogoSmall {
    			get {
    				return this._Sbj_LogoSmall;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_LogoSmall, _Sbj_LogoSmall, value);
    				this._Sbj_LogoSmall = value;
    			}
    		}
    		
    		public Int32 Dep_Id {
    			get {
    				return this._Dep_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Id, _Dep_Id, value);
    				this._Dep_Id = value;
    			}
    		}
    		
    		public String Dep_CnName {
    			get {
    				return this._Dep_CnName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_CnName, _Dep_CnName, value);
    				this._Dep_CnName = value;
    			}
    		}
    		
    		public Boolean Sbj_IsRec {
    			get {
    				return this._Sbj_IsRec;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_IsRec, _Sbj_IsRec, value);
    				this._Sbj_IsRec = value;
    			}
    		}
    		
    		public String Sbj_Details {
    			get {
    				return this._Sbj_Details;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_Details, _Sbj_Details, value);
    				this._Sbj_Details = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Subject>("Subject");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Sbj_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Sbj_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Sbj_ID,
    					_.Sbj_Name,
    					_.Sbj_ByName,
    					_.Sbj_Intro,
    					_.Sbj_Tax,
    					_.Sbj_IsUse,
    					_.Sbj_CrtTime,
    					_.Sbj_PassScore,
    					_.Org_ID,
    					_.Org_Name,
    					_.Sbj_PID,
    					_.Sbj_Level,
    					_.Sbj_XPath,
    					_.Sbj_CouNumber,
    					_.Sbj_Logo,
    					_.Sbj_LogoSmall,
    					_.Dep_Id,
    					_.Dep_CnName,
    					_.Sbj_IsRec,
    					_.Sbj_Details};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Sbj_ID,
    					this._Sbj_Name,
    					this._Sbj_ByName,
    					this._Sbj_Intro,
    					this._Sbj_Tax,
    					this._Sbj_IsUse,
    					this._Sbj_CrtTime,
    					this._Sbj_PassScore,
    					this._Org_ID,
    					this._Org_Name,
    					this._Sbj_PID,
    					this._Sbj_Level,
    					this._Sbj_XPath,
    					this._Sbj_CouNumber,
    					this._Sbj_Logo,
    					this._Sbj_LogoSmall,
    					this._Dep_Id,
    					this._Dep_CnName,
    					this._Sbj_IsRec,
    					this._Sbj_Details};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt32(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Name))) {
    				this._Sbj_Name = reader.GetString(_.Sbj_Name);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_ByName))) {
    				this._Sbj_ByName = reader.GetString(_.Sbj_ByName);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Intro))) {
    				this._Sbj_Intro = reader.GetString(_.Sbj_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Tax))) {
    				this._Sbj_Tax = reader.GetInt32(_.Sbj_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_IsUse))) {
    				this._Sbj_IsUse = reader.GetBoolean(_.Sbj_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_CrtTime))) {
    				this._Sbj_CrtTime = reader.GetDateTime(_.Sbj_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_PassScore))) {
    				this._Sbj_PassScore = reader.GetInt32(_.Sbj_PassScore);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_PID))) {
    				this._Sbj_PID = reader.GetInt32(_.Sbj_PID);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Level))) {
    				this._Sbj_Level = reader.GetInt32(_.Sbj_Level);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_XPath))) {
    				this._Sbj_XPath = reader.GetString(_.Sbj_XPath);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_CouNumber))) {
    				this._Sbj_CouNumber = reader.GetInt32(_.Sbj_CouNumber);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Logo))) {
    				this._Sbj_Logo = reader.GetString(_.Sbj_Logo);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_LogoSmall))) {
    				this._Sbj_LogoSmall = reader.GetString(_.Sbj_LogoSmall);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Id))) {
    				this._Dep_Id = reader.GetInt32(_.Dep_Id);
    			}
    			if ((false == reader.IsDBNull(_.Dep_CnName))) {
    				this._Dep_CnName = reader.GetString(_.Dep_CnName);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_IsRec))) {
    				this._Sbj_IsRec = reader.GetBoolean(_.Sbj_IsRec);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Details))) {
    				this._Sbj_Details = reader.GetString(_.Sbj_Details);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Subject).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Subject>();
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<Subject>("Sbj_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Name = new WeiSha.Data.Field<Subject>("Sbj_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_ByName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ByName = new WeiSha.Data.Field<Subject>("Sbj_ByName");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Intro = new WeiSha.Data.Field<Subject>("Sbj_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Tax = new WeiSha.Data.Field<Subject>("Sbj_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_IsUse = new WeiSha.Data.Field<Subject>("Sbj_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_CrtTime = new WeiSha.Data.Field<Subject>("Sbj_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Sbj_PassScore - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_PassScore = new WeiSha.Data.Field<Subject>("Sbj_PassScore");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Subject>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Subject>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Sbj_PID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_PID = new WeiSha.Data.Field<Subject>("Sbj_PID");
    			
    			/// <summary>
    			/// 字段名：Sbj_Level - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Level = new WeiSha.Data.Field<Subject>("Sbj_Level");
    			
    			/// <summary>
    			/// 字段名：Sbj_XPath - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_XPath = new WeiSha.Data.Field<Subject>("Sbj_XPath");
    			
    			/// <summary>
    			/// 字段名：Sbj_CouNumber - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_CouNumber = new WeiSha.Data.Field<Subject>("Sbj_CouNumber");
    			
    			/// <summary>
    			/// 字段名：Sbj_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Logo = new WeiSha.Data.Field<Subject>("Sbj_Logo");
    			
    			/// <summary>
    			/// 字段名：Sbj_LogoSmall - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_LogoSmall = new WeiSha.Data.Field<Subject>("Sbj_LogoSmall");
    			
    			/// <summary>
    			/// 字段名：Dep_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Id = new WeiSha.Data.Field<Subject>("Dep_Id");
    			
    			/// <summary>
    			/// 字段名：Dep_CnName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_CnName = new WeiSha.Data.Field<Subject>("Dep_CnName");
    			
    			/// <summary>
    			/// 字段名：Sbj_IsRec - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_IsRec = new WeiSha.Data.Field<Subject>("Sbj_IsRec");
    			
    			/// <summary>
    			/// 字段名：Sbj_Details - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Details = new WeiSha.Data.Field<Subject>("Sbj_Details");
    		}
    	}
    }
    