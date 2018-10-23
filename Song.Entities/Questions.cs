namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Questions 主键列：Qus_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Questions : WeiSha.Data.Entity {
    		
    		protected Int32 _Qus_ID;
    		
    		protected String _Qus_Title;
    		
    		protected String _Qus_Answer;
    		
    		protected Int32 _Qus_Diff;
    		
    		protected Int32 _Qus_Type;
    		
    		protected String _Qus_Explain;
    		
    		protected Boolean _Qus_IsUse;
    		
    		protected Boolean _Qus_IsError;
    		
    		protected String _Qus_UID;
    		
    		protected Single _Qus_Number;
    		
    		protected DateTime _Qus_CrtTime;
    		
    		protected DateTime _Qus_LastTime;
    		
    		protected Boolean _Qus_IsCorrect;
    		
    		protected Int32 _Kn_ID;
    		
    		protected Int32 _Sbj_ID;
    		
    		protected String _Qus_ErrorInfo;
    		
    		protected Int32 _Org_ID;
    		
    		protected Int32 _Cou_ID;
    		
    		protected Int32 _Ol_ID;
    		
    		protected Boolean _Qus_IsWrong;
    		
    		protected Int32 _Qt_ID;
    		
    		protected String _Qus_WrongInfo;
    		
    		protected String _Qus_Items;
    		
    		protected Boolean _Qus_IsTitle;
    		
    		protected String _Ol_Name;
    		
    		protected String _Sbj_Name;
    		
    		protected Int32 _Qus_Tax;
    		
    		protected Int32 _Qus_Errornum;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Qus_ID {
    			get {
    				return this._Qus_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_ID, _Qus_ID, value);
    				this._Qus_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Qus_Title {
    			get {
    				return this._Qus_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Title, _Qus_Title, value);
    				this._Qus_Title = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Qus_Answer {
    			get {
    				return this._Qus_Answer;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Answer, _Qus_Answer, value);
    				this._Qus_Answer = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Qus_Diff {
    			get {
    				return this._Qus_Diff;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Diff, _Qus_Diff, value);
    				this._Qus_Diff = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Qus_Type {
    			get {
    				return this._Qus_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Type, _Qus_Type, value);
    				this._Qus_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Qus_Explain {
    			get {
    				return this._Qus_Explain;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Explain, _Qus_Explain, value);
    				this._Qus_Explain = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Qus_IsUse {
    			get {
    				return this._Qus_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_IsUse, _Qus_IsUse, value);
    				this._Qus_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Qus_IsError {
    			get {
    				return this._Qus_IsError;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_IsError, _Qus_IsError, value);
    				this._Qus_IsError = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Qus_UID {
    			get {
    				return this._Qus_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_UID, _Qus_UID, value);
    				this._Qus_UID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Single Qus_Number {
    			get {
    				return this._Qus_Number;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Number, _Qus_Number, value);
    				this._Qus_Number = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime Qus_CrtTime {
    			get {
    				return this._Qus_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_CrtTime, _Qus_CrtTime, value);
    				this._Qus_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime Qus_LastTime {
    			get {
    				return this._Qus_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_LastTime, _Qus_LastTime, value);
    				this._Qus_LastTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Qus_IsCorrect {
    			get {
    				return this._Qus_IsCorrect;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_IsCorrect, _Qus_IsCorrect, value);
    				this._Qus_IsCorrect = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Kn_ID {
    			get {
    				return this._Kn_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_ID, _Kn_ID, value);
    				this._Kn_ID = value;
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
    		
    		public String Qus_ErrorInfo {
    			get {
    				return this._Qus_ErrorInfo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_ErrorInfo, _Qus_ErrorInfo, value);
    				this._Qus_ErrorInfo = value;
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
    		
    		public Int32 Cou_ID {
    			get {
    				return this._Cou_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ID, _Cou_ID, value);
    				this._Cou_ID = value;
    			}
    		}
    		
    		public Int32 Ol_ID {
    			get {
    				return this._Ol_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_ID, _Ol_ID, value);
    				this._Ol_ID = value;
    			}
    		}
    		
    		public Boolean Qus_IsWrong {
    			get {
    				return this._Qus_IsWrong;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_IsWrong, _Qus_IsWrong, value);
    				this._Qus_IsWrong = value;
    			}
    		}
    		
    		public Int32 Qt_ID {
    			get {
    				return this._Qt_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qt_ID, _Qt_ID, value);
    				this._Qt_ID = value;
    			}
    		}
    		
    		public String Qus_WrongInfo {
    			get {
    				return this._Qus_WrongInfo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_WrongInfo, _Qus_WrongInfo, value);
    				this._Qus_WrongInfo = value;
    			}
    		}
    		
    		public String Qus_Items {
    			get {
    				return this._Qus_Items;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Items, _Qus_Items, value);
    				this._Qus_Items = value;
    			}
    		}
    		
    		public Boolean Qus_IsTitle {
    			get {
    				return this._Qus_IsTitle;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_IsTitle, _Qus_IsTitle, value);
    				this._Qus_IsTitle = value;
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
    		
    		public String Sbj_Name {
    			get {
    				return this._Sbj_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_Name, _Sbj_Name, value);
    				this._Sbj_Name = value;
    			}
    		}
    		
    		public Int32 Qus_Tax {
    			get {
    				return this._Qus_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Tax, _Qus_Tax, value);
    				this._Qus_Tax = value;
    			}
    		}
    		
    		public Int32 Qus_Errornum {
    			get {
    				return this._Qus_Errornum;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Errornum, _Qus_Errornum, value);
    				this._Qus_Errornum = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Questions>("Questions");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Qus_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Qus_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Qus_ID,
    					_.Qus_Title,
    					_.Qus_Answer,
    					_.Qus_Diff,
    					_.Qus_Type,
    					_.Qus_Explain,
    					_.Qus_IsUse,
    					_.Qus_IsError,
    					_.Qus_UID,
    					_.Qus_Number,
    					_.Qus_CrtTime,
    					_.Qus_LastTime,
    					_.Qus_IsCorrect,
    					_.Kn_ID,
    					_.Sbj_ID,
    					_.Qus_ErrorInfo,
    					_.Org_ID,
    					_.Cou_ID,
    					_.Ol_ID,
    					_.Qus_IsWrong,
    					_.Qt_ID,
    					_.Qus_WrongInfo,
    					_.Qus_Items,
    					_.Qus_IsTitle,
    					_.Ol_Name,
    					_.Sbj_Name,
    					_.Qus_Tax,
    					_.Qus_Errornum};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Qus_ID,
    					this._Qus_Title,
    					this._Qus_Answer,
    					this._Qus_Diff,
    					this._Qus_Type,
    					this._Qus_Explain,
    					this._Qus_IsUse,
    					this._Qus_IsError,
    					this._Qus_UID,
    					this._Qus_Number,
    					this._Qus_CrtTime,
    					this._Qus_LastTime,
    					this._Qus_IsCorrect,
    					this._Kn_ID,
    					this._Sbj_ID,
    					this._Qus_ErrorInfo,
    					this._Org_ID,
    					this._Cou_ID,
    					this._Ol_ID,
    					this._Qus_IsWrong,
    					this._Qt_ID,
    					this._Qus_WrongInfo,
    					this._Qus_Items,
    					this._Qus_IsTitle,
    					this._Ol_Name,
    					this._Sbj_Name,
    					this._Qus_Tax,
    					this._Qus_Errornum};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Qus_ID))) {
    				this._Qus_ID = reader.GetInt32(_.Qus_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Title))) {
    				this._Qus_Title = reader.GetString(_.Qus_Title);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Answer))) {
    				this._Qus_Answer = reader.GetString(_.Qus_Answer);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Diff))) {
    				this._Qus_Diff = reader.GetInt32(_.Qus_Diff);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Type))) {
    				this._Qus_Type = reader.GetInt32(_.Qus_Type);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Explain))) {
    				this._Qus_Explain = reader.GetString(_.Qus_Explain);
    			}
    			if ((false == reader.IsDBNull(_.Qus_IsUse))) {
    				this._Qus_IsUse = reader.GetBoolean(_.Qus_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Qus_IsError))) {
    				this._Qus_IsError = reader.GetBoolean(_.Qus_IsError);
    			}
    			if ((false == reader.IsDBNull(_.Qus_UID))) {
    				this._Qus_UID = reader.GetString(_.Qus_UID);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Number))) {
    				this._Qus_Number = reader.GetFloat(_.Qus_Number);
    			}
    			if ((false == reader.IsDBNull(_.Qus_CrtTime))) {
    				this._Qus_CrtTime = reader.GetDateTime(_.Qus_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Qus_LastTime))) {
    				this._Qus_LastTime = reader.GetDateTime(_.Qus_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Qus_IsCorrect))) {
    				this._Qus_IsCorrect = reader.GetBoolean(_.Qus_IsCorrect);
    			}
    			if ((false == reader.IsDBNull(_.Kn_ID))) {
    				this._Kn_ID = reader.GetInt32(_.Kn_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt32(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qus_ErrorInfo))) {
    				this._Qus_ErrorInfo = reader.GetString(_.Qus_ErrorInfo);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ol_ID))) {
    				this._Ol_ID = reader.GetInt32(_.Ol_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qus_IsWrong))) {
    				this._Qus_IsWrong = reader.GetBoolean(_.Qus_IsWrong);
    			}
    			if ((false == reader.IsDBNull(_.Qt_ID))) {
    				this._Qt_ID = reader.GetInt32(_.Qt_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qus_WrongInfo))) {
    				this._Qus_WrongInfo = reader.GetString(_.Qus_WrongInfo);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Items))) {
    				this._Qus_Items = reader.GetString(_.Qus_Items);
    			}
    			if ((false == reader.IsDBNull(_.Qus_IsTitle))) {
    				this._Qus_IsTitle = reader.GetBoolean(_.Qus_IsTitle);
    			}
    			if ((false == reader.IsDBNull(_.Ol_Name))) {
    				this._Ol_Name = reader.GetString(_.Ol_Name);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Name))) {
    				this._Sbj_Name = reader.GetString(_.Sbj_Name);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Tax))) {
    				this._Qus_Tax = reader.GetInt32(_.Qus_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Errornum))) {
    				this._Qus_Errornum = reader.GetInt32(_.Qus_Errornum);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Questions).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Questions>();
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_ID = new WeiSha.Data.Field<Questions>("Qus_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Title = new WeiSha.Data.Field<Questions>("Qus_Title");
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_Answer - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Answer = new WeiSha.Data.Field<Questions>("Qus_Answer");
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_Diff - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Diff = new WeiSha.Data.Field<Questions>("Qus_Diff");
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_Type - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Type = new WeiSha.Data.Field<Questions>("Qus_Type");
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_Explain - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Explain = new WeiSha.Data.Field<Questions>("Qus_Explain");
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Qus_IsUse = new WeiSha.Data.Field<Questions>("Qus_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_IsError - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Qus_IsError = new WeiSha.Data.Field<Questions>("Qus_IsError");
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Qus_UID = new WeiSha.Data.Field<Questions>("Qus_UID");
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_Number - 数据类型：Single
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Number = new WeiSha.Data.Field<Questions>("Qus_Number");
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Qus_CrtTime = new WeiSha.Data.Field<Questions>("Qus_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_LastTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Qus_LastTime = new WeiSha.Data.Field<Questions>("Qus_LastTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_IsCorrect - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Qus_IsCorrect = new WeiSha.Data.Field<Questions>("Qus_IsCorrect");
    			
    			/// <summary>
    			/// -1 - 字段名：Kn_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Kn_ID = new WeiSha.Data.Field<Questions>("Kn_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<Questions>("Sbj_ID");
    			
    			/// <summary>
    			/// 字段名：Qus_ErrorInfo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Qus_ErrorInfo = new WeiSha.Data.Field<Questions>("Qus_ErrorInfo");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Questions>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<Questions>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Ol_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ol_ID = new WeiSha.Data.Field<Questions>("Ol_ID");
    			
    			/// <summary>
    			/// 字段名：Qus_IsWrong - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Qus_IsWrong = new WeiSha.Data.Field<Questions>("Qus_IsWrong");
    			
    			/// <summary>
    			/// 字段名：Qt_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qt_ID = new WeiSha.Data.Field<Questions>("Qt_ID");
    			
    			/// <summary>
    			/// 字段名：Qus_WrongInfo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Qus_WrongInfo = new WeiSha.Data.Field<Questions>("Qus_WrongInfo");
    			
    			/// <summary>
    			/// 字段名：Qus_Items - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Items = new WeiSha.Data.Field<Questions>("Qus_Items");
    			
    			/// <summary>
    			/// 字段名：Qus_IsTitle - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Qus_IsTitle = new WeiSha.Data.Field<Questions>("Qus_IsTitle");
    			
    			/// <summary>
    			/// 字段名：Ol_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ol_Name = new WeiSha.Data.Field<Questions>("Ol_Name");
    			
    			/// <summary>
    			/// 字段名：Sbj_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Name = new WeiSha.Data.Field<Questions>("Sbj_Name");
    			
    			/// <summary>
    			/// 字段名：Qus_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Tax = new WeiSha.Data.Field<Questions>("Qus_Tax");
    			
    			/// <summary>
    			/// 字段名：Qus_Errornum - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Errornum = new WeiSha.Data.Field<Questions>("Qus_Errornum");
    		}
    	}
    }
    