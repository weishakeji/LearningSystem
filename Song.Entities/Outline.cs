namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Outline 主键列：Ol_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Outline : WeiSha.Data.Entity {
    		
    		protected Int32 _Ol_ID;
    		
    		protected String _Ol_Name;
    		
    		protected String _Ol_Intro;
    		
    		protected Int32 _Ol_PID;
    		
    		protected Int32 _Ol_Tax;
    		
    		protected Int32 _Ol_Level;
    		
    		protected Boolean _Ol_IsUse;
    		
    		protected Boolean _Ol_IsFree;
    		
    		protected String _Ol_Courseware;
    		
    		protected String _Ol_Video;
    		
    		protected String _Ol_LessonPlan;
    		
    		protected Int32 _Cou_ID;
    		
    		protected String _Ol_UID;
    		
    		protected String _Ol_XPath;
    		
    		protected Int32 _Ol_QusNumber;
    		
    		protected Int32 _Org_ID;
    		
    		protected Int32 _Sbj_ID;
    		
    		protected Int32 _Ol_QuesCount;
    		
    		protected Boolean _Ol_IsFinish;
    		
    		protected Boolean _Ol_IsNode;
    		
    		protected Boolean _Ol_IsVideo;
    		
    		public Int32 Ol_ID {
    			get {
    				return this._Ol_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_ID, _Ol_ID, value);
    				this._Ol_ID = value;
    			}
    		}
    		
    		public String Ol_Name {
    			get {
    				return this._Ol_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_Name, _Ol_Name, value);
    				this._Ol_Name = value;
    			}
    		}
    		
    		public String Ol_Intro {
    			get {
    				return this._Ol_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_Intro, _Ol_Intro, value);
    				this._Ol_Intro = value;
    			}
    		}
    		
    		public Int32 Ol_PID {
    			get {
    				return this._Ol_PID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_PID, _Ol_PID, value);
    				this._Ol_PID = value;
    			}
    		}
    		
    		public Int32 Ol_Tax {
    			get {
    				return this._Ol_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_Tax, _Ol_Tax, value);
    				this._Ol_Tax = value;
    			}
    		}
    		
    		public Int32 Ol_Level {
    			get {
    				return this._Ol_Level;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_Level, _Ol_Level, value);
    				this._Ol_Level = value;
    			}
    		}
    		
    		public Boolean Ol_IsUse {
    			get {
    				return this._Ol_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_IsUse, _Ol_IsUse, value);
    				this._Ol_IsUse = value;
    			}
    		}
    		
    		public Boolean Ol_IsFree {
    			get {
    				return this._Ol_IsFree;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_IsFree, _Ol_IsFree, value);
    				this._Ol_IsFree = value;
    			}
    		}
    		
    		public String Ol_Courseware {
    			get {
    				return this._Ol_Courseware;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_Courseware, _Ol_Courseware, value);
    				this._Ol_Courseware = value;
    			}
    		}
    		
    		public String Ol_Video {
    			get {
    				return this._Ol_Video;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_Video, _Ol_Video, value);
    				this._Ol_Video = value;
    			}
    		}
    		
    		public String Ol_LessonPlan {
    			get {
    				return this._Ol_LessonPlan;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_LessonPlan, _Ol_LessonPlan, value);
    				this._Ol_LessonPlan = value;
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
    		
    		public String Ol_UID {
    			get {
    				return this._Ol_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_UID, _Ol_UID, value);
    				this._Ol_UID = value;
    			}
    		}
    		
    		public String Ol_XPath {
    			get {
    				return this._Ol_XPath;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_XPath, _Ol_XPath, value);
    				this._Ol_XPath = value;
    			}
    		}
    		
    		public Int32 Ol_QusNumber {
    			get {
    				return this._Ol_QusNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_QusNumber, _Ol_QusNumber, value);
    				this._Ol_QusNumber = value;
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
    		
    		public Int32 Sbj_ID {
    			get {
    				return this._Sbj_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_ID, _Sbj_ID, value);
    				this._Sbj_ID = value;
    			}
    		}
    		
    		public Int32 Ol_QuesCount {
    			get {
    				return this._Ol_QuesCount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_QuesCount, _Ol_QuesCount, value);
    				this._Ol_QuesCount = value;
    			}
    		}
    		
    		public Boolean Ol_IsFinish {
    			get {
    				return this._Ol_IsFinish;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_IsFinish, _Ol_IsFinish, value);
    				this._Ol_IsFinish = value;
    			}
    		}
    		
    		public Boolean Ol_IsNode {
    			get {
    				return this._Ol_IsNode;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_IsNode, _Ol_IsNode, value);
    				this._Ol_IsNode = value;
    			}
    		}
    		
    		public Boolean Ol_IsVideo {
    			get {
    				return this._Ol_IsVideo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_IsVideo, _Ol_IsVideo, value);
    				this._Ol_IsVideo = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Outline>("Outline");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Ol_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ol_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ol_ID,
    					_.Ol_Name,
    					_.Ol_Intro,
    					_.Ol_PID,
    					_.Ol_Tax,
    					_.Ol_Level,
    					_.Ol_IsUse,
    					_.Ol_IsFree,
    					_.Ol_Courseware,
    					_.Ol_Video,
    					_.Ol_LessonPlan,
    					_.Cou_ID,
    					_.Ol_UID,
    					_.Ol_XPath,
    					_.Ol_QusNumber,
    					_.Org_ID,
    					_.Sbj_ID,
    					_.Ol_QuesCount,
    					_.Ol_IsFinish,
    					_.Ol_IsNode,
    					_.Ol_IsVideo};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Ol_ID,
    					this._Ol_Name,
    					this._Ol_Intro,
    					this._Ol_PID,
    					this._Ol_Tax,
    					this._Ol_Level,
    					this._Ol_IsUse,
    					this._Ol_IsFree,
    					this._Ol_Courseware,
    					this._Ol_Video,
    					this._Ol_LessonPlan,
    					this._Cou_ID,
    					this._Ol_UID,
    					this._Ol_XPath,
    					this._Ol_QusNumber,
    					this._Org_ID,
    					this._Sbj_ID,
    					this._Ol_QuesCount,
    					this._Ol_IsFinish,
    					this._Ol_IsNode,
    					this._Ol_IsVideo};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Ol_ID))) {
    				this._Ol_ID = reader.GetInt32(_.Ol_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ol_Name))) {
    				this._Ol_Name = reader.GetString(_.Ol_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ol_Intro))) {
    				this._Ol_Intro = reader.GetString(_.Ol_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Ol_PID))) {
    				this._Ol_PID = reader.GetInt32(_.Ol_PID);
    			}
    			if ((false == reader.IsDBNull(_.Ol_Tax))) {
    				this._Ol_Tax = reader.GetInt32(_.Ol_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Ol_Level))) {
    				this._Ol_Level = reader.GetInt32(_.Ol_Level);
    			}
    			if ((false == reader.IsDBNull(_.Ol_IsUse))) {
    				this._Ol_IsUse = reader.GetBoolean(_.Ol_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Ol_IsFree))) {
    				this._Ol_IsFree = reader.GetBoolean(_.Ol_IsFree);
    			}
    			if ((false == reader.IsDBNull(_.Ol_Courseware))) {
    				this._Ol_Courseware = reader.GetString(_.Ol_Courseware);
    			}
    			if ((false == reader.IsDBNull(_.Ol_Video))) {
    				this._Ol_Video = reader.GetString(_.Ol_Video);
    			}
    			if ((false == reader.IsDBNull(_.Ol_LessonPlan))) {
    				this._Ol_LessonPlan = reader.GetString(_.Ol_LessonPlan);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ol_UID))) {
    				this._Ol_UID = reader.GetString(_.Ol_UID);
    			}
    			if ((false == reader.IsDBNull(_.Ol_XPath))) {
    				this._Ol_XPath = reader.GetString(_.Ol_XPath);
    			}
    			if ((false == reader.IsDBNull(_.Ol_QusNumber))) {
    				this._Ol_QusNumber = reader.GetInt32(_.Ol_QusNumber);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt32(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ol_QuesCount))) {
    				this._Ol_QuesCount = reader.GetInt32(_.Ol_QuesCount);
    			}
    			if ((false == reader.IsDBNull(_.Ol_IsFinish))) {
    				this._Ol_IsFinish = reader.GetBoolean(_.Ol_IsFinish);
    			}
    			if ((false == reader.IsDBNull(_.Ol_IsNode))) {
    				this._Ol_IsNode = reader.GetBoolean(_.Ol_IsNode);
    			}
    			if ((false == reader.IsDBNull(_.Ol_IsVideo))) {
    				this._Ol_IsVideo = reader.GetBoolean(_.Ol_IsVideo);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Outline).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Outline>();
    			
    			/// <summary>
    			/// 字段名：Ol_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ol_ID = new WeiSha.Data.Field<Outline>("Ol_ID");
    			
    			/// <summary>
    			/// 字段名：Ol_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ol_Name = new WeiSha.Data.Field<Outline>("Ol_Name");
    			
    			/// <summary>
    			/// 字段名：Ol_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ol_Intro = new WeiSha.Data.Field<Outline>("Ol_Intro");
    			
    			/// <summary>
    			/// 字段名：Ol_PID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ol_PID = new WeiSha.Data.Field<Outline>("Ol_PID");
    			
    			/// <summary>
    			/// 字段名：Ol_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ol_Tax = new WeiSha.Data.Field<Outline>("Ol_Tax");
    			
    			/// <summary>
    			/// 字段名：Ol_Level - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ol_Level = new WeiSha.Data.Field<Outline>("Ol_Level");
    			
    			/// <summary>
    			/// 字段名：Ol_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ol_IsUse = new WeiSha.Data.Field<Outline>("Ol_IsUse");
    			
    			/// <summary>
    			/// 字段名：Ol_IsFree - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ol_IsFree = new WeiSha.Data.Field<Outline>("Ol_IsFree");
    			
    			/// <summary>
    			/// 字段名：Ol_Courseware - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ol_Courseware = new WeiSha.Data.Field<Outline>("Ol_Courseware");
    			
    			/// <summary>
    			/// 字段名：Ol_Video - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ol_Video = new WeiSha.Data.Field<Outline>("Ol_Video");
    			
    			/// <summary>
    			/// 字段名：Ol_LessonPlan - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ol_LessonPlan = new WeiSha.Data.Field<Outline>("Ol_LessonPlan");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<Outline>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Ol_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ol_UID = new WeiSha.Data.Field<Outline>("Ol_UID");
    			
    			/// <summary>
    			/// 字段名：Ol_XPath - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ol_XPath = new WeiSha.Data.Field<Outline>("Ol_XPath");
    			
    			/// <summary>
    			/// 字段名：Ol_QusNumber - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ol_QusNumber = new WeiSha.Data.Field<Outline>("Ol_QusNumber");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Outline>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Sbj_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<Outline>("Sbj_ID");
    			
    			/// <summary>
    			/// 字段名：Ol_QuesCount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ol_QuesCount = new WeiSha.Data.Field<Outline>("Ol_QuesCount");
    			
    			/// <summary>
    			/// 字段名：Ol_IsFinish - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ol_IsFinish = new WeiSha.Data.Field<Outline>("Ol_IsFinish");
    			
    			/// <summary>
    			/// 字段名：Ol_IsNode - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ol_IsNode = new WeiSha.Data.Field<Outline>("Ol_IsNode");
    			
    			/// <summary>
    			/// 字段名：Ol_IsVideo - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ol_IsVideo = new WeiSha.Data.Field<Outline>("Ol_IsVideo");
    		}
    	}
    }
    