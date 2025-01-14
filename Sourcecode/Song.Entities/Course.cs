namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Course 主键列：Cou_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Course : WeiSha.Data.Entity {
    		
    		protected Int64 _Cou_ID;
    		
    		protected Boolean _Cou_Allowedit;
    		
    		protected String _Cou_Content;
    		
    		protected DateTime _Cou_CrtTime;
    		
    		protected Boolean _Cou_ExistExam;
    		
    		protected Boolean _Cou_ExistLive;
    		
    		protected DateTime _Cou_FreeEnd;
    		
    		protected DateTime _Cou_FreeStart;
    		
    		protected String _Cou_Intro;
    		
    		protected Boolean _Cou_IsFree;
    		
    		protected Boolean _Cou_IsLimitFree;
    		
    		protected Boolean _Cou_IsRec;
    		
    		protected Boolean _Cou_IsStudy;
    		
    		protected Boolean _Cou_IsTry;
    		
    		protected Boolean _Cou_IsUse;
    		
    		protected Int32 _Cou_KnlCount;
    		
    		protected Int32 _Cou_Level;
    		
    		protected String _Cou_Logo;
    		
    		protected String _Cou_LogoSmall;
    		
    		protected String _Cou_Name;
    		
    		protected Int32 _Cou_OutlineCount;
    		
    		protected Int64 _Cou_PID;
    		
    		protected Int32 _Cou_Price;
    		
    		protected Int32 _Cou_PriceSpan;
    		
    		protected String _Cou_PriceUnit;
    		
    		protected String _Cou_Prices;
    		
    		protected Int32 _Cou_QuesCount;
    		
    		protected Int32 _Cou_Score;
    		
    		protected Int32 _Cou_StudentSum;
    		
    		protected String _Cou_Target;
    		
    		protected Int32 _Cou_Tax;
    		
    		protected Int32 _Cou_TestCount;
    		
    		protected Int32 _Cou_TryNum;
    		
    		protected Int32 _Cou_Type;
    		
    		protected String _Cou_UID;
    		
    		protected Int32 _Cou_VideoCount;
    		
    		protected Int32 _Cou_ViewNum;
    		
    		protected String _Cou_XPath;
    		
    		protected String _Dep_CnName;
    		
    		protected Int32 _Dep_Id;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Int64 _Sbj_ID;
    		
    		protected String _Sbj_Name;
    		
    		protected Int32 _Th_ID;
    		
    		protected String _Th_Name;
    		
    		public Int64 Cou_ID {
    			get {
    				return this._Cou_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ID, _Cou_ID, value);
    				this._Cou_ID = value;
    			}
    		}
    		
    		public Boolean Cou_Allowedit {
    			get {
    				return this._Cou_Allowedit;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_Allowedit, _Cou_Allowedit, value);
    				this._Cou_Allowedit = value;
    			}
    		}
    		
    		public String Cou_Content {
    			get {
    				return this._Cou_Content;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_Content, _Cou_Content, value);
    				this._Cou_Content = value;
    			}
    		}
    		
    		public DateTime Cou_CrtTime {
    			get {
    				return this._Cou_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_CrtTime, _Cou_CrtTime, value);
    				this._Cou_CrtTime = value;
    			}
    		}
    		
    		public Boolean Cou_ExistExam {
    			get {
    				return this._Cou_ExistExam;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ExistExam, _Cou_ExistExam, value);
    				this._Cou_ExistExam = value;
    			}
    		}
    		
    		public Boolean Cou_ExistLive {
    			get {
    				return this._Cou_ExistLive;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ExistLive, _Cou_ExistLive, value);
    				this._Cou_ExistLive = value;
    			}
    		}
    		
    		public DateTime Cou_FreeEnd {
    			get {
    				return this._Cou_FreeEnd;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_FreeEnd, _Cou_FreeEnd, value);
    				this._Cou_FreeEnd = value;
    			}
    		}
    		
    		public DateTime Cou_FreeStart {
    			get {
    				return this._Cou_FreeStart;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_FreeStart, _Cou_FreeStart, value);
    				this._Cou_FreeStart = value;
    			}
    		}
    		
    		public String Cou_Intro {
    			get {
    				return this._Cou_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_Intro, _Cou_Intro, value);
    				this._Cou_Intro = value;
    			}
    		}
    		
    		public Boolean Cou_IsFree {
    			get {
    				return this._Cou_IsFree;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_IsFree, _Cou_IsFree, value);
    				this._Cou_IsFree = value;
    			}
    		}
    		
    		public Boolean Cou_IsLimitFree {
    			get {
    				return this._Cou_IsLimitFree;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_IsLimitFree, _Cou_IsLimitFree, value);
    				this._Cou_IsLimitFree = value;
    			}
    		}
    		
    		public Boolean Cou_IsRec {
    			get {
    				return this._Cou_IsRec;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_IsRec, _Cou_IsRec, value);
    				this._Cou_IsRec = value;
    			}
    		}
    		
    		public Boolean Cou_IsStudy {
    			get {
    				return this._Cou_IsStudy;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_IsStudy, _Cou_IsStudy, value);
    				this._Cou_IsStudy = value;
    			}
    		}
    		
    		public Boolean Cou_IsTry {
    			get {
    				return this._Cou_IsTry;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_IsTry, _Cou_IsTry, value);
    				this._Cou_IsTry = value;
    			}
    		}
    		
    		public Boolean Cou_IsUse {
    			get {
    				return this._Cou_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_IsUse, _Cou_IsUse, value);
    				this._Cou_IsUse = value;
    			}
    		}
    		
    		public Int32 Cou_KnlCount {
    			get {
    				return this._Cou_KnlCount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_KnlCount, _Cou_KnlCount, value);
    				this._Cou_KnlCount = value;
    			}
    		}
    		
    		public Int32 Cou_Level {
    			get {
    				return this._Cou_Level;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_Level, _Cou_Level, value);
    				this._Cou_Level = value;
    			}
    		}
    		
    		public String Cou_Logo {
    			get {
    				return this._Cou_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_Logo, _Cou_Logo, value);
    				this._Cou_Logo = value;
    			}
    		}
    		
    		public String Cou_LogoSmall {
    			get {
    				return this._Cou_LogoSmall;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_LogoSmall, _Cou_LogoSmall, value);
    				this._Cou_LogoSmall = value;
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
    		
    		public Int32 Cou_OutlineCount {
    			get {
    				return this._Cou_OutlineCount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_OutlineCount, _Cou_OutlineCount, value);
    				this._Cou_OutlineCount = value;
    			}
    		}
    		
    		public Int64 Cou_PID {
    			get {
    				return this._Cou_PID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_PID, _Cou_PID, value);
    				this._Cou_PID = value;
    			}
    		}
    		
    		public Int32 Cou_Price {
    			get {
    				return this._Cou_Price;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_Price, _Cou_Price, value);
    				this._Cou_Price = value;
    			}
    		}
    		
    		public Int32 Cou_PriceSpan {
    			get {
    				return this._Cou_PriceSpan;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_PriceSpan, _Cou_PriceSpan, value);
    				this._Cou_PriceSpan = value;
    			}
    		}
    		
    		public String Cou_PriceUnit {
    			get {
    				return this._Cou_PriceUnit;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_PriceUnit, _Cou_PriceUnit, value);
    				this._Cou_PriceUnit = value;
    			}
    		}
    		
    		public String Cou_Prices {
    			get {
    				return this._Cou_Prices;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_Prices, _Cou_Prices, value);
    				this._Cou_Prices = value;
    			}
    		}
    		
    		public Int32 Cou_QuesCount {
    			get {
    				return this._Cou_QuesCount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_QuesCount, _Cou_QuesCount, value);
    				this._Cou_QuesCount = value;
    			}
    		}
    		
    		public Int32 Cou_Score {
    			get {
    				return this._Cou_Score;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_Score, _Cou_Score, value);
    				this._Cou_Score = value;
    			}
    		}
    		
    		public Int32 Cou_StudentSum {
    			get {
    				return this._Cou_StudentSum;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_StudentSum, _Cou_StudentSum, value);
    				this._Cou_StudentSum = value;
    			}
    		}
    		
    		public String Cou_Target {
    			get {
    				return this._Cou_Target;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_Target, _Cou_Target, value);
    				this._Cou_Target = value;
    			}
    		}
    		
    		public Int32 Cou_Tax {
    			get {
    				return this._Cou_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_Tax, _Cou_Tax, value);
    				this._Cou_Tax = value;
    			}
    		}
    		
    		public Int32 Cou_TestCount {
    			get {
    				return this._Cou_TestCount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_TestCount, _Cou_TestCount, value);
    				this._Cou_TestCount = value;
    			}
    		}
    		
    		public Int32 Cou_TryNum {
    			get {
    				return this._Cou_TryNum;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_TryNum, _Cou_TryNum, value);
    				this._Cou_TryNum = value;
    			}
    		}
    		
    		public Int32 Cou_Type {
    			get {
    				return this._Cou_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_Type, _Cou_Type, value);
    				this._Cou_Type = value;
    			}
    		}
    		
    		public String Cou_UID {
    			get {
    				return this._Cou_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_UID, _Cou_UID, value);
    				this._Cou_UID = value;
    			}
    		}
    		
    		public Int32 Cou_VideoCount {
    			get {
    				return this._Cou_VideoCount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_VideoCount, _Cou_VideoCount, value);
    				this._Cou_VideoCount = value;
    			}
    		}
    		
    		public Int32 Cou_ViewNum {
    			get {
    				return this._Cou_ViewNum;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ViewNum, _Cou_ViewNum, value);
    				this._Cou_ViewNum = value;
    			}
    		}
    		
    		public String Cou_XPath {
    			get {
    				return this._Cou_XPath;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_XPath, _Cou_XPath, value);
    				this._Cou_XPath = value;
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
    		
    		public Int32 Dep_Id {
    			get {
    				return this._Dep_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Id, _Dep_Id, value);
    				this._Dep_Id = value;
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
    		
    		public Int64 Sbj_ID {
    			get {
    				return this._Sbj_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_ID, _Sbj_ID, value);
    				this._Sbj_ID = value;
    			}
    		}
    		
    		public String Sbj_Name {
    			get {
    				return this._Sbj_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_Name, _Sbj_Name, value);
    				this._Sbj_Name = value;
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
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Course>("Course");
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Cou_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Cou_ID,
    					_.Cou_Allowedit,
    					_.Cou_Content,
    					_.Cou_CrtTime,
    					_.Cou_ExistExam,
    					_.Cou_ExistLive,
    					_.Cou_FreeEnd,
    					_.Cou_FreeStart,
    					_.Cou_Intro,
    					_.Cou_IsFree,
    					_.Cou_IsLimitFree,
    					_.Cou_IsRec,
    					_.Cou_IsStudy,
    					_.Cou_IsTry,
    					_.Cou_IsUse,
    					_.Cou_KnlCount,
    					_.Cou_Level,
    					_.Cou_Logo,
    					_.Cou_LogoSmall,
    					_.Cou_Name,
    					_.Cou_OutlineCount,
    					_.Cou_PID,
    					_.Cou_Price,
    					_.Cou_PriceSpan,
    					_.Cou_PriceUnit,
    					_.Cou_Prices,
    					_.Cou_QuesCount,
    					_.Cou_Score,
    					_.Cou_StudentSum,
    					_.Cou_Target,
    					_.Cou_Tax,
    					_.Cou_TestCount,
    					_.Cou_TryNum,
    					_.Cou_Type,
    					_.Cou_UID,
    					_.Cou_VideoCount,
    					_.Cou_ViewNum,
    					_.Cou_XPath,
    					_.Dep_CnName,
    					_.Dep_Id,
    					_.Org_ID,
    					_.Org_Name,
    					_.Sbj_ID,
    					_.Sbj_Name,
    					_.Th_ID,
    					_.Th_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Cou_ID,
    					this._Cou_Allowedit,
    					this._Cou_Content,
    					this._Cou_CrtTime,
    					this._Cou_ExistExam,
    					this._Cou_ExistLive,
    					this._Cou_FreeEnd,
    					this._Cou_FreeStart,
    					this._Cou_Intro,
    					this._Cou_IsFree,
    					this._Cou_IsLimitFree,
    					this._Cou_IsRec,
    					this._Cou_IsStudy,
    					this._Cou_IsTry,
    					this._Cou_IsUse,
    					this._Cou_KnlCount,
    					this._Cou_Level,
    					this._Cou_Logo,
    					this._Cou_LogoSmall,
    					this._Cou_Name,
    					this._Cou_OutlineCount,
    					this._Cou_PID,
    					this._Cou_Price,
    					this._Cou_PriceSpan,
    					this._Cou_PriceUnit,
    					this._Cou_Prices,
    					this._Cou_QuesCount,
    					this._Cou_Score,
    					this._Cou_StudentSum,
    					this._Cou_Target,
    					this._Cou_Tax,
    					this._Cou_TestCount,
    					this._Cou_TryNum,
    					this._Cou_Type,
    					this._Cou_UID,
    					this._Cou_VideoCount,
    					this._Cou_ViewNum,
    					this._Cou_XPath,
    					this._Dep_CnName,
    					this._Dep_Id,
    					this._Org_ID,
    					this._Org_Name,
    					this._Sbj_ID,
    					this._Sbj_Name,
    					this._Th_ID,
    					this._Th_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt64(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Allowedit))) {
    				this._Cou_Allowedit = reader.GetBoolean(_.Cou_Allowedit);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Content))) {
    				this._Cou_Content = reader.GetString(_.Cou_Content);
    			}
    			if ((false == reader.IsDBNull(_.Cou_CrtTime))) {
    				this._Cou_CrtTime = reader.GetDateTime(_.Cou_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ExistExam))) {
    				this._Cou_ExistExam = reader.GetBoolean(_.Cou_ExistExam);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ExistLive))) {
    				this._Cou_ExistLive = reader.GetBoolean(_.Cou_ExistLive);
    			}
    			if ((false == reader.IsDBNull(_.Cou_FreeEnd))) {
    				this._Cou_FreeEnd = reader.GetDateTime(_.Cou_FreeEnd);
    			}
    			if ((false == reader.IsDBNull(_.Cou_FreeStart))) {
    				this._Cou_FreeStart = reader.GetDateTime(_.Cou_FreeStart);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Intro))) {
    				this._Cou_Intro = reader.GetString(_.Cou_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Cou_IsFree))) {
    				this._Cou_IsFree = reader.GetBoolean(_.Cou_IsFree);
    			}
    			if ((false == reader.IsDBNull(_.Cou_IsLimitFree))) {
    				this._Cou_IsLimitFree = reader.GetBoolean(_.Cou_IsLimitFree);
    			}
    			if ((false == reader.IsDBNull(_.Cou_IsRec))) {
    				this._Cou_IsRec = reader.GetBoolean(_.Cou_IsRec);
    			}
    			if ((false == reader.IsDBNull(_.Cou_IsStudy))) {
    				this._Cou_IsStudy = reader.GetBoolean(_.Cou_IsStudy);
    			}
    			if ((false == reader.IsDBNull(_.Cou_IsTry))) {
    				this._Cou_IsTry = reader.GetBoolean(_.Cou_IsTry);
    			}
    			if ((false == reader.IsDBNull(_.Cou_IsUse))) {
    				this._Cou_IsUse = reader.GetBoolean(_.Cou_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Cou_KnlCount))) {
    				this._Cou_KnlCount = reader.GetInt32(_.Cou_KnlCount);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Level))) {
    				this._Cou_Level = reader.GetInt32(_.Cou_Level);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Logo))) {
    				this._Cou_Logo = reader.GetString(_.Cou_Logo);
    			}
    			if ((false == reader.IsDBNull(_.Cou_LogoSmall))) {
    				this._Cou_LogoSmall = reader.GetString(_.Cou_LogoSmall);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Name))) {
    				this._Cou_Name = reader.GetString(_.Cou_Name);
    			}
    			if ((false == reader.IsDBNull(_.Cou_OutlineCount))) {
    				this._Cou_OutlineCount = reader.GetInt32(_.Cou_OutlineCount);
    			}
    			if ((false == reader.IsDBNull(_.Cou_PID))) {
    				this._Cou_PID = reader.GetInt64(_.Cou_PID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Price))) {
    				this._Cou_Price = reader.GetInt32(_.Cou_Price);
    			}
    			if ((false == reader.IsDBNull(_.Cou_PriceSpan))) {
    				this._Cou_PriceSpan = reader.GetInt32(_.Cou_PriceSpan);
    			}
    			if ((false == reader.IsDBNull(_.Cou_PriceUnit))) {
    				this._Cou_PriceUnit = reader.GetString(_.Cou_PriceUnit);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Prices))) {
    				this._Cou_Prices = reader.GetString(_.Cou_Prices);
    			}
    			if ((false == reader.IsDBNull(_.Cou_QuesCount))) {
    				this._Cou_QuesCount = reader.GetInt32(_.Cou_QuesCount);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Score))) {
    				this._Cou_Score = reader.GetInt32(_.Cou_Score);
    			}
    			if ((false == reader.IsDBNull(_.Cou_StudentSum))) {
    				this._Cou_StudentSum = reader.GetInt32(_.Cou_StudentSum);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Target))) {
    				this._Cou_Target = reader.GetString(_.Cou_Target);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Tax))) {
    				this._Cou_Tax = reader.GetInt32(_.Cou_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Cou_TestCount))) {
    				this._Cou_TestCount = reader.GetInt32(_.Cou_TestCount);
    			}
    			if ((false == reader.IsDBNull(_.Cou_TryNum))) {
    				this._Cou_TryNum = reader.GetInt32(_.Cou_TryNum);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Type))) {
    				this._Cou_Type = reader.GetInt32(_.Cou_Type);
    			}
    			if ((false == reader.IsDBNull(_.Cou_UID))) {
    				this._Cou_UID = reader.GetString(_.Cou_UID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_VideoCount))) {
    				this._Cou_VideoCount = reader.GetInt32(_.Cou_VideoCount);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ViewNum))) {
    				this._Cou_ViewNum = reader.GetInt32(_.Cou_ViewNum);
    			}
    			if ((false == reader.IsDBNull(_.Cou_XPath))) {
    				this._Cou_XPath = reader.GetString(_.Cou_XPath);
    			}
    			if ((false == reader.IsDBNull(_.Dep_CnName))) {
    				this._Dep_CnName = reader.GetString(_.Dep_CnName);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Id))) {
    				this._Dep_Id = reader.GetInt32(_.Dep_Id);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt64(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Name))) {
    				this._Sbj_Name = reader.GetString(_.Sbj_Name);
    			}
    			if ((false == reader.IsDBNull(_.Th_ID))) {
    				this._Th_ID = reader.GetInt32(_.Th_ID);
    			}
    			if ((false == reader.IsDBNull(_.Th_Name))) {
    				this._Th_Name = reader.GetString(_.Th_Name);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Course).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Course>();
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<Course>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Cou_Allowedit - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Allowedit = new WeiSha.Data.Field<Course>("Cou_Allowedit");
    			
    			/// <summary>
    			/// 字段名：Cou_Content - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Content = new WeiSha.Data.Field<Course>("Cou_Content");
    			
    			/// <summary>
    			/// 字段名：Cou_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Cou_CrtTime = new WeiSha.Data.Field<Course>("Cou_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Cou_ExistExam - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ExistExam = new WeiSha.Data.Field<Course>("Cou_ExistExam");
    			
    			/// <summary>
    			/// 字段名：Cou_ExistLive - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ExistLive = new WeiSha.Data.Field<Course>("Cou_ExistLive");
    			
    			/// <summary>
    			/// 字段名：Cou_FreeEnd - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Cou_FreeEnd = new WeiSha.Data.Field<Course>("Cou_FreeEnd");
    			
    			/// <summary>
    			/// 字段名：Cou_FreeStart - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Cou_FreeStart = new WeiSha.Data.Field<Course>("Cou_FreeStart");
    			
    			/// <summary>
    			/// 字段名：Cou_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Intro = new WeiSha.Data.Field<Course>("Cou_Intro");
    			
    			/// <summary>
    			/// 字段名：Cou_IsFree - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Cou_IsFree = new WeiSha.Data.Field<Course>("Cou_IsFree");
    			
    			/// <summary>
    			/// 字段名：Cou_IsLimitFree - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Cou_IsLimitFree = new WeiSha.Data.Field<Course>("Cou_IsLimitFree");
    			
    			/// <summary>
    			/// 字段名：Cou_IsRec - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Cou_IsRec = new WeiSha.Data.Field<Course>("Cou_IsRec");
    			
    			/// <summary>
    			/// 字段名：Cou_IsStudy - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Cou_IsStudy = new WeiSha.Data.Field<Course>("Cou_IsStudy");
    			
    			/// <summary>
    			/// 字段名：Cou_IsTry - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Cou_IsTry = new WeiSha.Data.Field<Course>("Cou_IsTry");
    			
    			/// <summary>
    			/// 字段名：Cou_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Cou_IsUse = new WeiSha.Data.Field<Course>("Cou_IsUse");
    			
    			/// <summary>
    			/// 字段名：Cou_KnlCount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_KnlCount = new WeiSha.Data.Field<Course>("Cou_KnlCount");
    			
    			/// <summary>
    			/// 字段名：Cou_Level - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Level = new WeiSha.Data.Field<Course>("Cou_Level");
    			
    			/// <summary>
    			/// 字段名：Cou_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Logo = new WeiSha.Data.Field<Course>("Cou_Logo");
    			
    			/// <summary>
    			/// 字段名：Cou_LogoSmall - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Cou_LogoSmall = new WeiSha.Data.Field<Course>("Cou_LogoSmall");
    			
    			/// <summary>
    			/// 字段名：Cou_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Name = new WeiSha.Data.Field<Course>("Cou_Name");
    			
    			/// <summary>
    			/// 字段名：Cou_OutlineCount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_OutlineCount = new WeiSha.Data.Field<Course>("Cou_OutlineCount");
    			
    			/// <summary>
    			/// 字段名：Cou_PID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Cou_PID = new WeiSha.Data.Field<Course>("Cou_PID");
    			
    			/// <summary>
    			/// 字段名：Cou_Price - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Price = new WeiSha.Data.Field<Course>("Cou_Price");
    			
    			/// <summary>
    			/// 字段名：Cou_PriceSpan - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_PriceSpan = new WeiSha.Data.Field<Course>("Cou_PriceSpan");
    			
    			/// <summary>
    			/// 字段名：Cou_PriceUnit - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Cou_PriceUnit = new WeiSha.Data.Field<Course>("Cou_PriceUnit");
    			
    			/// <summary>
    			/// 字段名：Cou_Prices - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Prices = new WeiSha.Data.Field<Course>("Cou_Prices");
    			
    			/// <summary>
    			/// 字段名：Cou_QuesCount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_QuesCount = new WeiSha.Data.Field<Course>("Cou_QuesCount");
    			
    			/// <summary>
    			/// 字段名：Cou_Score - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Score = new WeiSha.Data.Field<Course>("Cou_Score");
    			
    			/// <summary>
    			/// 字段名：Cou_StudentSum - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_StudentSum = new WeiSha.Data.Field<Course>("Cou_StudentSum");
    			
    			/// <summary>
    			/// 字段名：Cou_Target - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Target = new WeiSha.Data.Field<Course>("Cou_Target");
    			
    			/// <summary>
    			/// 字段名：Cou_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Tax = new WeiSha.Data.Field<Course>("Cou_Tax");
    			
    			/// <summary>
    			/// 字段名：Cou_TestCount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_TestCount = new WeiSha.Data.Field<Course>("Cou_TestCount");
    			
    			/// <summary>
    			/// 字段名：Cou_TryNum - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_TryNum = new WeiSha.Data.Field<Course>("Cou_TryNum");
    			
    			/// <summary>
    			/// 字段名：Cou_Type - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Type = new WeiSha.Data.Field<Course>("Cou_Type");
    			
    			/// <summary>
    			/// 字段名：Cou_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Cou_UID = new WeiSha.Data.Field<Course>("Cou_UID");
    			
    			/// <summary>
    			/// 字段名：Cou_VideoCount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_VideoCount = new WeiSha.Data.Field<Course>("Cou_VideoCount");
    			
    			/// <summary>
    			/// 字段名：Cou_ViewNum - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ViewNum = new WeiSha.Data.Field<Course>("Cou_ViewNum");
    			
    			/// <summary>
    			/// 字段名：Cou_XPath - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Cou_XPath = new WeiSha.Data.Field<Course>("Cou_XPath");
    			
    			/// <summary>
    			/// 字段名：Dep_CnName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_CnName = new WeiSha.Data.Field<Course>("Dep_CnName");
    			
    			/// <summary>
    			/// 字段名：Dep_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Id = new WeiSha.Data.Field<Course>("Dep_Id");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Course>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Course>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Sbj_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<Course>("Sbj_ID");
    			
    			/// <summary>
    			/// 字段名：Sbj_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Name = new WeiSha.Data.Field<Course>("Sbj_Name");
    			
    			/// <summary>
    			/// 字段名：Th_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_ID = new WeiSha.Data.Field<Course>("Th_ID");
    			
    			/// <summary>
    			/// 字段名：Th_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Name = new WeiSha.Data.Field<Course>("Th_Name");
    		}
    	}
    }
    