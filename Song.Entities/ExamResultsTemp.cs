namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ExamResultsTemp 主键列：Exr_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ExamResultsTemp : WeiSha.Data.Entity {
    		
    		protected Int32 _Exr_ID;
    		
    		protected Single? _Exr_Score;
    		
    		protected String _Exr_Results;
    		
    		protected DateTime? _Exr_CrtTime;
    		
    		protected String _Exr_IP;
    		
    		protected String _Exr_Mac;
    		
    		protected Boolean _Exr_IsSubmit;
    		
    		protected Int32? _Exam_ID;
    		
    		protected Int32? _Sbj_ID;
    		
    		protected Int32? _Tp_Id;
    		
    		protected Int32? _Acc_Id;
    		
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
    		public Single? Exr_Score {
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
    		public DateTime? Exr_CrtTime {
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
    		public Int32? Exam_ID {
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
    		public Int32? Sbj_ID {
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
    		public Int32? Tp_Id {
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
    		public Int32? Acc_Id {
    			get {
    				return this._Acc_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Id, _Acc_Id, value);
    				this._Acc_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<ExamResultsTemp>("ExamResultsTemp");
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
    					_.Exr_Results,
    					_.Exr_CrtTime,
    					_.Exr_IP,
    					_.Exr_Mac,
    					_.Exr_IsSubmit,
    					_.Exam_ID,
    					_.Sbj_ID,
    					_.Tp_Id,
    					_.Acc_Id};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Exr_ID,
    					this._Exr_Score,
    					this._Exr_Results,
    					this._Exr_CrtTime,
    					this._Exr_IP,
    					this._Exr_Mac,
    					this._Exr_IsSubmit,
    					this._Exam_ID,
    					this._Sbj_ID,
    					this._Tp_Id,
    					this._Acc_Id};
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
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt32(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Id))) {
    				this._Tp_Id = reader.GetInt32(_.Tp_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(ExamResultsTemp).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<ExamResultsTemp>();
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Exr_ID = new WeiSha.Data.Field<ExamResultsTemp>("Exr_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_Score - 数据类型：Single(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Exr_Score = new WeiSha.Data.Field<ExamResultsTemp>("Exr_Score");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_Results - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exr_Results = new WeiSha.Data.Field<ExamResultsTemp>("Exr_Results");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Exr_CrtTime = new WeiSha.Data.Field<ExamResultsTemp>("Exr_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_IP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exr_IP = new WeiSha.Data.Field<ExamResultsTemp>("Exr_IP");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_Mac - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exr_Mac = new WeiSha.Data.Field<ExamResultsTemp>("Exr_Mac");
    			
    			/// <summary>
    			/// -1 - 字段名：Exr_IsSubmit - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Exr_IsSubmit = new WeiSha.Data.Field<ExamResultsTemp>("Exr_IsSubmit");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_ID - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Exam_ID = new WeiSha.Data.Field<ExamResultsTemp>("Exam_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_ID - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<ExamResultsTemp>("Sbj_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Id = new WeiSha.Data.Field<ExamResultsTemp>("Tp_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<ExamResultsTemp>("Acc_Id");
    		}
    	}
    }
    