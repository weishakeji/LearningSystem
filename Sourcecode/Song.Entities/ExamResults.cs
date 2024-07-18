namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ExamResults 主键列：Exr_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ExamResults : WeiSha.Data.Entity {
    		
    		protected Int32 _Exr_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected String _Ac_IDCardNumber;
    		
    		protected String _Ac_Name;
    		
    		protected Int32 _Ac_Sex;
    		
    		protected Int32 _Dep_Id;
    		
    		protected Int32 _Exam_ID;
    		
    		protected String _Exam_Name;
    		
    		protected String _Exam_Title;
    		
    		protected String _Exam_UID;
    		
    		protected DateTime _Exr_CalcTime;
    		
    		protected Double _Exr_Colligate;
    		
    		protected DateTime _Exr_CrtTime;
    		
    		protected Double _Exr_Draw;
    		
    		protected String _Exr_IP;
    		
    		protected Boolean _Exr_IsCalc;
    		
    		protected Boolean _Exr_IsManual;
    		
    		protected Boolean _Exr_IsSubmit;
    		
    		protected DateTime _Exr_LastTime;
    		
    		protected String _Exr_Mac;
    		
    		protected DateTime _Exr_OverTime;
    		
    		protected String _Exr_Results;
    		
    		protected Double _Exr_Score;
    		
    		protected Double _Exr_ScoreFinal;
    		
    		protected DateTime _Exr_SubmitTime;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Int64 _Sbj_ID;
    		
    		protected String _Sbj_Name;
    		
    		protected Int64 _Sts_ID;
    		
    		protected Int64 _Tp_Id;
    		
    		public Int32 Exr_ID {
    			get {
    				return this._Exr_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_ID, _Exr_ID, value);
    				this._Exr_ID = value;
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
    		
    		public String Ac_IDCardNumber {
    			get {
    				return this._Ac_IDCardNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_IDCardNumber, _Ac_IDCardNumber, value);
    				this._Ac_IDCardNumber = value;
    			}
    		}
    		
    		public String Ac_Name {
    			get {
    				return this._Ac_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Name, _Ac_Name, value);
    				this._Ac_Name = value;
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
    		
    		public Int32 Dep_Id {
    			get {
    				return this._Dep_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Id, _Dep_Id, value);
    				this._Dep_Id = value;
    			}
    		}
    		
    		public Int32 Exam_ID {
    			get {
    				return this._Exam_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_ID, _Exam_ID, value);
    				this._Exam_ID = value;
    			}
    		}
    		
    		public String Exam_Name {
    			get {
    				return this._Exam_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_Name, _Exam_Name, value);
    				this._Exam_Name = value;
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
    		
    		public String Exam_UID {
    			get {
    				return this._Exam_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exam_UID, _Exam_UID, value);
    				this._Exam_UID = value;
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
    		
    		public Double Exr_Colligate {
    			get {
    				return this._Exr_Colligate;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_Colligate, _Exr_Colligate, value);
    				this._Exr_Colligate = value;
    			}
    		}
    		
    		public DateTime Exr_CrtTime {
    			get {
    				return this._Exr_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_CrtTime, _Exr_CrtTime, value);
    				this._Exr_CrtTime = value;
    			}
    		}
    		
    		public Double Exr_Draw {
    			get {
    				return this._Exr_Draw;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_Draw, _Exr_Draw, value);
    				this._Exr_Draw = value;
    			}
    		}
    		
    		public String Exr_IP {
    			get {
    				return this._Exr_IP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_IP, _Exr_IP, value);
    				this._Exr_IP = value;
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
    		
    		public Boolean Exr_IsManual {
    			get {
    				return this._Exr_IsManual;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_IsManual, _Exr_IsManual, value);
    				this._Exr_IsManual = value;
    			}
    		}
    		
    		public Boolean Exr_IsSubmit {
    			get {
    				return this._Exr_IsSubmit;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_IsSubmit, _Exr_IsSubmit, value);
    				this._Exr_IsSubmit = value;
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
    		
    		public String Exr_Mac {
    			get {
    				return this._Exr_Mac;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_Mac, _Exr_Mac, value);
    				this._Exr_Mac = value;
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
    		
    		public String Exr_Results {
    			get {
    				return this._Exr_Results;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_Results, _Exr_Results, value);
    				this._Exr_Results = value;
    			}
    		}
    		
    		public Double Exr_Score {
    			get {
    				return this._Exr_Score;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_Score, _Exr_Score, value);
    				this._Exr_Score = value;
    			}
    		}
    		
    		public Double Exr_ScoreFinal {
    			get {
    				return this._Exr_ScoreFinal;
    			}
    			set {
    				this.OnPropertyValueChange(_.Exr_ScoreFinal, _Exr_ScoreFinal, value);
    				this._Exr_ScoreFinal = value;
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
    		
    		public Int64 Sts_ID {
    			get {
    				return this._Sts_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sts_ID, _Sts_ID, value);
    				this._Sts_ID = value;
    			}
    		}
    		
    		public Int64 Tp_Id {
    			get {
    				return this._Tp_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Id, _Tp_Id, value);
    				this._Tp_Id = value;
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
    					_.Ac_ID,
    					_.Ac_IDCardNumber,
    					_.Ac_Name,
    					_.Ac_Sex,
    					_.Dep_Id,
    					_.Exam_ID,
    					_.Exam_Name,
    					_.Exam_Title,
    					_.Exam_UID,
    					_.Exr_CalcTime,
    					_.Exr_Colligate,
    					_.Exr_CrtTime,
    					_.Exr_Draw,
    					_.Exr_IP,
    					_.Exr_IsCalc,
    					_.Exr_IsManual,
    					_.Exr_IsSubmit,
    					_.Exr_LastTime,
    					_.Exr_Mac,
    					_.Exr_OverTime,
    					_.Exr_Results,
    					_.Exr_Score,
    					_.Exr_ScoreFinal,
    					_.Exr_SubmitTime,
    					_.Org_ID,
    					_.Org_Name,
    					_.Sbj_ID,
    					_.Sbj_Name,
    					_.Sts_ID,
    					_.Tp_Id};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Exr_ID,
    					this._Ac_ID,
    					this._Ac_IDCardNumber,
    					this._Ac_Name,
    					this._Ac_Sex,
    					this._Dep_Id,
    					this._Exam_ID,
    					this._Exam_Name,
    					this._Exam_Title,
    					this._Exam_UID,
    					this._Exr_CalcTime,
    					this._Exr_Colligate,
    					this._Exr_CrtTime,
    					this._Exr_Draw,
    					this._Exr_IP,
    					this._Exr_IsCalc,
    					this._Exr_IsManual,
    					this._Exr_IsSubmit,
    					this._Exr_LastTime,
    					this._Exr_Mac,
    					this._Exr_OverTime,
    					this._Exr_Results,
    					this._Exr_Score,
    					this._Exr_ScoreFinal,
    					this._Exr_SubmitTime,
    					this._Org_ID,
    					this._Org_Name,
    					this._Sbj_ID,
    					this._Sbj_Name,
    					this._Sts_ID,
    					this._Tp_Id};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Exr_ID))) {
    				this._Exr_ID = reader.GetInt32(_.Exr_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_IDCardNumber))) {
    				this._Ac_IDCardNumber = reader.GetString(_.Ac_IDCardNumber);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Name))) {
    				this._Ac_Name = reader.GetString(_.Ac_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Sex))) {
    				this._Ac_Sex = reader.GetInt32(_.Ac_Sex);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Id))) {
    				this._Dep_Id = reader.GetInt32(_.Dep_Id);
    			}
    			if ((false == reader.IsDBNull(_.Exam_ID))) {
    				this._Exam_ID = reader.GetInt32(_.Exam_ID);
    			}
    			if ((false == reader.IsDBNull(_.Exam_Name))) {
    				this._Exam_Name = reader.GetString(_.Exam_Name);
    			}
    			if ((false == reader.IsDBNull(_.Exam_Title))) {
    				this._Exam_Title = reader.GetString(_.Exam_Title);
    			}
    			if ((false == reader.IsDBNull(_.Exam_UID))) {
    				this._Exam_UID = reader.GetString(_.Exam_UID);
    			}
    			if ((false == reader.IsDBNull(_.Exr_CalcTime))) {
    				this._Exr_CalcTime = reader.GetDateTime(_.Exr_CalcTime);
    			}
    			if ((false == reader.IsDBNull(_.Exr_Colligate))) {
    				this._Exr_Colligate = reader.GetDouble(_.Exr_Colligate);
    			}
    			if ((false == reader.IsDBNull(_.Exr_CrtTime))) {
    				this._Exr_CrtTime = reader.GetDateTime(_.Exr_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Exr_Draw))) {
    				this._Exr_Draw = reader.GetDouble(_.Exr_Draw);
    			}
    			if ((false == reader.IsDBNull(_.Exr_IP))) {
    				this._Exr_IP = reader.GetString(_.Exr_IP);
    			}
    			if ((false == reader.IsDBNull(_.Exr_IsCalc))) {
    				this._Exr_IsCalc = reader.GetBoolean(_.Exr_IsCalc);
    			}
    			if ((false == reader.IsDBNull(_.Exr_IsManual))) {
    				this._Exr_IsManual = reader.GetBoolean(_.Exr_IsManual);
    			}
    			if ((false == reader.IsDBNull(_.Exr_IsSubmit))) {
    				this._Exr_IsSubmit = reader.GetBoolean(_.Exr_IsSubmit);
    			}
    			if ((false == reader.IsDBNull(_.Exr_LastTime))) {
    				this._Exr_LastTime = reader.GetDateTime(_.Exr_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Exr_Mac))) {
    				this._Exr_Mac = reader.GetString(_.Exr_Mac);
    			}
    			if ((false == reader.IsDBNull(_.Exr_OverTime))) {
    				this._Exr_OverTime = reader.GetDateTime(_.Exr_OverTime);
    			}
    			if ((false == reader.IsDBNull(_.Exr_Results))) {
    				this._Exr_Results = reader.GetString(_.Exr_Results);
    			}
    			if ((false == reader.IsDBNull(_.Exr_Score))) {
    				this._Exr_Score = reader.GetDouble(_.Exr_Score);
    			}
    			if ((false == reader.IsDBNull(_.Exr_ScoreFinal))) {
    				this._Exr_ScoreFinal = reader.GetDouble(_.Exr_ScoreFinal);
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
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt64(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Name))) {
    				this._Sbj_Name = reader.GetString(_.Sbj_Name);
    			}
    			if ((false == reader.IsDBNull(_.Sts_ID))) {
    				this._Sts_ID = reader.GetInt64(_.Sts_ID);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Id))) {
    				this._Tp_Id = reader.GetInt64(_.Tp_Id);
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
    			/// 字段名：Exr_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Exr_ID = new WeiSha.Data.Field<ExamResults>("Exr_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<ExamResults>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_IDCardNumber - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_IDCardNumber = new WeiSha.Data.Field<ExamResults>("Ac_IDCardNumber");
    			
    			/// <summary>
    			/// 字段名：Ac_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Name = new WeiSha.Data.Field<ExamResults>("Ac_Name");
    			
    			/// <summary>
    			/// 字段名：Ac_Sex - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Sex = new WeiSha.Data.Field<ExamResults>("Ac_Sex");
    			
    			/// <summary>
    			/// 字段名：Dep_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Id = new WeiSha.Data.Field<ExamResults>("Dep_Id");
    			
    			/// <summary>
    			/// 字段名：Exam_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Exam_ID = new WeiSha.Data.Field<ExamResults>("Exam_ID");
    			
    			/// <summary>
    			/// 字段名：Exam_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exam_Name = new WeiSha.Data.Field<ExamResults>("Exam_Name");
    			
    			/// <summary>
    			/// 字段名：Exam_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exam_Title = new WeiSha.Data.Field<ExamResults>("Exam_Title");
    			
    			/// <summary>
    			/// 字段名：Exam_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exam_UID = new WeiSha.Data.Field<ExamResults>("Exam_UID");
    			
    			/// <summary>
    			/// 字段名：Exr_CalcTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Exr_CalcTime = new WeiSha.Data.Field<ExamResults>("Exr_CalcTime");
    			
    			/// <summary>
    			/// 字段名：Exr_Colligate - 数据类型：Double
    			/// </summary>
    			public static WeiSha.Data.Field Exr_Colligate = new WeiSha.Data.Field<ExamResults>("Exr_Colligate");
    			
    			/// <summary>
    			/// 字段名：Exr_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Exr_CrtTime = new WeiSha.Data.Field<ExamResults>("Exr_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Exr_Draw - 数据类型：Double
    			/// </summary>
    			public static WeiSha.Data.Field Exr_Draw = new WeiSha.Data.Field<ExamResults>("Exr_Draw");
    			
    			/// <summary>
    			/// 字段名：Exr_IP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exr_IP = new WeiSha.Data.Field<ExamResults>("Exr_IP");
    			
    			/// <summary>
    			/// 字段名：Exr_IsCalc - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Exr_IsCalc = new WeiSha.Data.Field<ExamResults>("Exr_IsCalc");
    			
    			/// <summary>
    			/// 字段名：Exr_IsManual - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Exr_IsManual = new WeiSha.Data.Field<ExamResults>("Exr_IsManual");
    			
    			/// <summary>
    			/// 字段名：Exr_IsSubmit - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Exr_IsSubmit = new WeiSha.Data.Field<ExamResults>("Exr_IsSubmit");
    			
    			/// <summary>
    			/// 字段名：Exr_LastTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Exr_LastTime = new WeiSha.Data.Field<ExamResults>("Exr_LastTime");
    			
    			/// <summary>
    			/// 字段名：Exr_Mac - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exr_Mac = new WeiSha.Data.Field<ExamResults>("Exr_Mac");
    			
    			/// <summary>
    			/// 字段名：Exr_OverTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Exr_OverTime = new WeiSha.Data.Field<ExamResults>("Exr_OverTime");
    			
    			/// <summary>
    			/// 字段名：Exr_Results - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exr_Results = new WeiSha.Data.Field<ExamResults>("Exr_Results");
    			
    			/// <summary>
    			/// 字段名：Exr_Score - 数据类型：Double
    			/// </summary>
    			public static WeiSha.Data.Field Exr_Score = new WeiSha.Data.Field<ExamResults>("Exr_Score");
    			
    			/// <summary>
    			/// 字段名：Exr_ScoreFinal - 数据类型：Double
    			/// </summary>
    			public static WeiSha.Data.Field Exr_ScoreFinal = new WeiSha.Data.Field<ExamResults>("Exr_ScoreFinal");
    			
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
    			/// 字段名：Sbj_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<ExamResults>("Sbj_ID");
    			
    			/// <summary>
    			/// 字段名：Sbj_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Name = new WeiSha.Data.Field<ExamResults>("Sbj_Name");
    			
    			/// <summary>
    			/// 字段名：Sts_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Sts_ID = new WeiSha.Data.Field<ExamResults>("Sts_ID");
    			
    			/// <summary>
    			/// 字段名：Tp_Id - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Id = new WeiSha.Data.Field<ExamResults>("Tp_Id");
    		}
    	}
    }
    