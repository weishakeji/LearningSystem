namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ExamResults 主键列：Exr_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ExamResults : WeiSha.Data.Entity {
    		
    		protected Int32 _Exr_ID;
    		
    		protected Single _Exr_Score;
    		
    		protected Single _Exr_ScoreFinal;
    		
    		protected Single _Exr_Draw;
    		
    		protected Single _Exr_Colligate;
    		
    		protected String _Exr_Results;
    		
    		protected DateTime _Exr_CrtTime;
    		
    		protected String _Exr_IP;
    		
    		protected String _Exr_Mac;
    		
    		protected Boolean _Exr_IsSubmit;
    		
    		protected Int32 _Exam_ID;
    		
    		protected String _Exam_UID;
    		
    		protected String _Exam_Name;
    		
    		protected Int32 _Sbj_ID;
    		
    		protected String _Sbj_Name;
    		
    		protected Int32 _Tp_Id;
    		
    		protected Int32 _Ac_ID;
    		
    		protected String _Ac_Name;
    		
    		protected Int32 _Dep_Id;
    		
    		protected Int32 _Team_ID;
    		
    		protected String _Exam_Title;
    		
    		protected DateTime _Exr_SubmitTime;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Int32 _Sts_ID;
    		
    		protected Int32 _Ac_Sex;
    		
    		protected String _Ac_IDCardNumber;
    		
    		protected Boolean _Exr_IsCalc;
    		
    		protected DateTime _Exr_OverTime;
    		
    		protected DateTime _Exr_CalcTime;
    		
    		protected DateTime _Exr_LastTime;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Exr_ID {
    			get {
    				return this._Exr_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_ID, _Exr_ID, value);
    				this._Exr_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Single Exr_Score {
    			get {
    				return this._Exr_Score;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_Score, _Exr_Score, value);
    				this._Exr_Score = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Single Exr_ScoreFinal {
    			get {
    				return this._Exr_ScoreFinal;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_ScoreFinal, _Exr_ScoreFinal, value);
    				this._Exr_ScoreFinal = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Single Exr_Draw {
    			get {
    				return this._Exr_Draw;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_Draw, _Exr_Draw, value);
    				this._Exr_Draw = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Single Exr_Colligate {
    			get {
    				return this._Exr_Colligate;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_Colligate, _Exr_Colligate, value);
    				this._Exr_Colligate = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Exr_Results {
    			get {
    				return this._Exr_Results;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_Results, _Exr_Results, value);
    				this._Exr_Results = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime Exr_CrtTime {
    			get {
    				return this._Exr_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_CrtTime, _Exr_CrtTime, value);
    				this._Exr_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Exr_IP {
    			get {
    				return this._Exr_IP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_IP, _Exr_IP, value);
    				this._Exr_IP = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Exr_Mac {
    			get {
    				return this._Exr_Mac;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_Mac, _Exr_Mac, value);
    				this._Exr_Mac = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Exr_IsSubmit {
    			get {
    				return this._Exr_IsSubmit;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_IsSubmit, _Exr_IsSubmit, value);
    				this._Exr_IsSubmit = value;
    			}
    		}
    		
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
    		public String Exam_Name {
    			get {
    				return this._Exam_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_Name, _Exam_Name, value);
    				this._Exam_Name = value;
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
    		public Int32 Ac_ID {
    			get {
    				return this._Ac_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_ID, _Ac_ID, value);
    				this._Ac_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Ac_Name {
    			get {
    				return this._Ac_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Name, _Ac_Name, value);
    				this._Ac_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Dep_Id {
    			get {
    				return this._Dep_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Id, _Dep_Id, value);
    				this._Dep_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Team_ID {
    			get {
    				return this._Team_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Team_ID, _Team_ID, value);
    				this._Team_ID = value;
    			}
    		}
    		
    		public String Exam_Title {
    			get {
    				return this._Exam_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_Title, _Exam_Title, value);
    				this._Exam_Title = value;
    			}
    		}
    		
    		public DateTime Exr_SubmitTime {
    			get {
    				return this._Exr_SubmitTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_SubmitTime, _Exr_SubmitTime, value);
    				this._Exr_SubmitTime = value;
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
    		
    		public Int32 Sts_ID {
    			get {
    				return this._Sts_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sts_ID, _Sts_ID, value);
    				this._Sts_ID = value;
    			}
    		}
    		
    		public Int32 Ac_Sex {
    			get {
    				return this._Ac_Sex;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Sex, _Ac_Sex, value);
    				this._Ac_Sex = value;
    			}
    		}
    		
    		public String Ac_IDCardNumber {
    			get {
    				return this._Ac_IDCardNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_IDCardNumber, _Ac_IDCardNumber, value);
    				this._Ac_IDCardNumber = value;
    			}
    		}
    		
    		public Boolean Exr_IsCalc {
    			get {
    				return this._Exr_IsCalc;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_IsCalc, _Exr_IsCalc, value);
    				this._Exr_IsCalc = value;
    			}
    		}
    		
    		public DateTime Exr_OverTime {
    			get {
    				return this._Exr_OverTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_OverTime, _Exr_OverTime, value);
    				this._Exr_OverTime = value;
    			}
    		}
    		
    		public DateTime Exr_CalcTime {
    			get {
    				return this._Exr_CalcTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_CalcTime, _Exr_CalcTime, value);
    				this._Exr_CalcTime = value;
    			}
    		}
    		
    		public DateTime Exr_LastTime {
    			get {
    				return this._Exr_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_LastTime, _Exr_LastTime, value);
    				this._Exr_LastTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<ExamResults>("ExamResults");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Exr_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Exr_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Exr_ID,
    					_.Exr_Score,
    					_.Exr_ScoreFinal,
    					_.Exr_Draw,
    					_.Exr_Colligate,
    					_.Exr_Results,
    					_.Exr_CrtTime,
    					_.Exr_IP,
    					_.Exr_Mac,
    					_.Exr_IsSubmit,
    					_.Exam_ID,
    					_.Exam_UID,
    					_.Exam_Name,
    					_.Sbj_ID,
    					_.Sbj_Name,
    					_.Tp_Id,
    					_.Ac_ID,
    					_.Ac_Name,
    					_.Dep_Id,
    					_.Team_ID,
    					_.Exam_Title,
    					_.Exr_SubmitTime,
    					_.Org_ID,
    					_.Org_Name,
    					_.Sts_ID,
    					_.Ac_Sex,
    					_.Ac_IDCardNumber,
    					_.Exr_IsCalc,
    					_.Exr_OverTime,
    					_.Exr_CalcTime,
    					_.Exr_LastTime};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Exr_ID,
    					this._Exr_Score,
    					this._Exr_ScoreFinal,
    					this._Exr_Draw,
    					this._Exr_Colligate,
    					this._Exr_Results,
    					this._Exr_CrtTime,
    					this._Exr_IP,
    					this._Exr_Mac,
    					this._Exr_IsSubmit,
    					this._Exam_ID,
    					this._Exam_UID,
    					this._Exam_Name,
    					this._Sbj_ID,
    					this._Sbj_Name,
    					this._Tp_Id,
    					this._Ac_ID,
    					this._Ac_Name,
    					this._Dep_Id,
    					this._Team_ID,
    					this._Exam_Title,
    					this._Exr_SubmitTime,
    					this._Org_ID,
    					this._Org_Name,
    					this._Sts_ID,
    					this._Ac_Sex,
    					this._Ac_IDCardNumber,
    					this._Exr_IsCalc,
    					this._Exr_OverTime,
    					this._Exr_CalcTime,
    					this._Exr_LastTime};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Exr_ID))) {
    				this._Exr_ID = reader.GetInt32(_.Exr_ID);
    			}
    			if ((false == reader.IsDBNull(_.Exr_Score))) {
    				this._Exr_Score = reader.GetFloat(_.Exr_Score);
    			}
    			if ((false == reader.IsDBNull(_.Exr_ScoreFinal))) {
    				this._Exr_ScoreFinal = reader.GetFloat(_.Exr_ScoreFinal);
    			}
    			if ((false == reader.IsDBNull(_.Exr_Draw))) {
    				this._Exr_Draw = reader.GetFloat(_.Exr_Draw);
    			}
    			if ((false == reader.IsDBNull(_.Exr_Colligate))) {
    				this._Exr_Colligate = reader.GetFloat(_.Exr_Colligate);
    			}
    			if ((false == reader.IsDBNull(_.Exr_Results))) {
    				this._Exr_Results = reader.GetString(_.Exr_Results);
    			}
    			if ((false == reader.IsDBNull(_.Exr_CrtTime))) {
    				this._Exr_CrtTime = reader.GetDateTime(_.Exr_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Exr_IP))) {
    				this._Exr_IP = reader.GetString(_.Exr_IP);
    			}
    			if ((false == reader.IsDBNull(_.Exr_Mac))) {
    				this._Exr_Mac = reader.GetString(_.Exr_Mac);
    			}
    			if ((false == reader.IsDBNull(_.Exr_IsSubmit))) {
    				this._Exr_IsSubmit = reader.GetBoolean(_.Exr_IsSubmit);
    			}
    			if ((false == reader.IsDBNull(_.Exam_ID))) {
    				this._Exam_ID = reader.GetInt32(_.Exam_ID);
    			}
    			if ((false == reader.IsDBNull(_.Exam_UID))) {
    				this._Exam_UID = reader.GetString(_.Exam_UID);
    			}
    			if ((false == reader.IsDBNull(_.Exam_Name))) {
    				this._Exam_Name = reader.GetString(_.Exam_Name);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt32(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Name))) {
    				this._Sbj_Name = reader.GetString(_.Sbj_Name);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Id))) {
    				this._Tp_Id = reader.GetInt32(_.Tp_Id);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Name))) {
    				this._Ac_Name = reader.GetString(_.Ac_Name);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Id))) {
    				this._Dep_Id = reader.GetInt32(_.Dep_Id);
    			}
    			if ((false == reader.IsDBNull(_.Team_ID))) {
    				this._Team_ID = reader.GetInt32(_.Team_ID);
    			}
    			if ((false == reader.IsDBNull(_.Exam_Title))) {
    				this._Exam_Title = reader.GetString(_.Exam_Title);
    			}
    			if ((false == reader.IsDBNull(_.Exr_SubmitTime))) {
    				this._Exr_SubmitTime = reader.GetDateTime(_.Exr_SubmitTime);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Sts_ID))) {
    				this._Sts_ID = reader.GetInt32(_.Sts_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Sex))) {
    				this._Ac_Sex = reader.GetInt32(_.Ac_Sex);
    			}
    			if ((false == reader.IsDBNull(_.Ac_IDCardNumber))) {
    				this._Ac_IDCardNumber = reader.GetString(_.Ac_IDCardNumber);
    			}
    			if ((false == reader.IsDBNull(_.Exr_IsCalc))) {
    				this._Exr_IsCalc = reader.GetBoolean(_.Exr_IsCalc);
    			}
    			if ((false == reader.IsDBNull(_.Exr_OverTime))) {
    				this._Exr_OverTime = reader.GetDateTime(_.Exr_OverTime);
    			}
    			if ((false == reader.IsDBNull(_.Exr_CalcTime))) {
    				this._Exr_CalcTime = reader.GetDateTime(_.Exr_CalcTime);
    			}
    			if ((false == reader.IsDBNull(_.Exr_LastTime))) {
    				this._Exr_LastTime = reader.GetDateTime(_.Exr_LastTime);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(ExamResults).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<ExamResults>();
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Exr_ID = new WeiSha.Data.Field<ExamResults>("Exr_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_Score - 数据类型：Single
    			/// </summary>
    			public static WeiSha.Data.Field Exr_Score = new WeiSha.Data.Field<ExamResults>("Exr_Score");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_ScoreFinal - 数据类型：Single
    			/// </summary>
    			public static WeiSha.Data.Field Exr_ScoreFinal = new WeiSha.Data.Field<ExamResults>("Exr_ScoreFinal");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_Draw - 数据类型：Single
    			/// </summary>
    			public static WeiSha.Data.Field Exr_Draw = new WeiSha.Data.Field<ExamResults>("Exr_Draw");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_Colligate - 数据类型：Single
    			/// </summary>
    			public static WeiSha.Data.Field Exr_Colligate = new WeiSha.Data.Field<ExamResults>("Exr_Colligate");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_Results - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exr_Results = new WeiSha.Data.Field<ExamResults>("Exr_Results");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Exr_CrtTime = new WeiSha.Data.Field<ExamResults>("Exr_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_IP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exr_IP = new WeiSha.Data.Field<ExamResults>("Exr_IP");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_Mac - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exr_Mac = new WeiSha.Data.Field<ExamResults>("Exr_Mac");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_IsSubmit - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Exr_IsSubmit = new WeiSha.Data.Field<ExamResults>("Exr_IsSubmit");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Exam_ID = new WeiSha.Data.Field<ExamResults>("Exam_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exam_UID = new WeiSha.Data.Field<ExamResults>("Exam_UID");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exam_Name = new WeiSha.Data.Field<ExamResults>("Exam_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<ExamResults>("Sbj_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Name = new WeiSha.Data.Field<ExamResults>("Sbj_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Id = new WeiSha.Data.Field<ExamResults>("Tp_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<ExamResults>("Ac_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Ac_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Name = new WeiSha.Data.Field<ExamResults>("Ac_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Id = new WeiSha.Data.Field<ExamResults>("Dep_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Team_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Team_ID = new WeiSha.Data.Field<ExamResults>("Team_ID");
    			
    			/// <summary>
    			/// 字段名：Exam_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exam_Title = new WeiSha.Data.Field<ExamResults>("Exam_Title");
    			
    			/// <summary>
    			/// 字段名：Exr_SubmitTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Exr_SubmitTime = new WeiSha.Data.Field<ExamResults>("Exr_SubmitTime");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<ExamResults>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<ExamResults>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Sts_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sts_ID = new WeiSha.Data.Field<ExamResults>("Sts_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_Sex - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Sex = new WeiSha.Data.Field<ExamResults>("Ac_Sex");
    			
    			/// <summary>
    			/// 字段名：Ac_IDCardNumber - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_IDCardNumber = new WeiSha.Data.Field<ExamResults>("Ac_IDCardNumber");
    			
    			/// <summary>
    			/// 字段名：Exr_IsCalc - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Exr_IsCalc = new WeiSha.Data.Field<ExamResults>("Exr_IsCalc");
    			
    			/// <summary>
    			/// 字段名：Exr_OverTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Exr_OverTime = new WeiSha.Data.Field<ExamResults>("Exr_OverTime");
    			
    			/// <summary>
    			/// 字段名：Exr_CalcTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Exr_CalcTime = new WeiSha.Data.Field<ExamResults>("Exr_CalcTime");
    			
    			/// <summary>
    			/// 字段名：Exr_LastTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Exr_LastTime = new WeiSha.Data.Field<ExamResults>("Exr_LastTime");
    		}
    	}
    }
    