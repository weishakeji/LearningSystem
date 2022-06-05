namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Student_Course 主键列：Stc_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Student_Course : WeiSha.Data.Entity {
    		
    		protected Int32 _Stc_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected Int32 _Cou_ID;
    		
    		protected DateTime _Stc_CrtTime;
    		
    		protected Single _Stc_Money;
    		
    		protected DateTime _Stc_StartTime;
    		
    		protected DateTime _Stc_EndTime;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Rc_Code;
    		
    		protected Boolean _Stc_IsFree;
    		
    		protected Boolean _Stc_IsTry;
    		
    		protected Double _Stc_QuesScore;
    		
    		protected Double _Stc_StudyScore;
    		
    		protected Double _Stc_ExamScore;
    		
    		public Int32 Stc_ID {
    			get {
    				return this._Stc_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stc_ID, _Stc_ID, value);
    				this._Stc_ID = value;
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
    		
    		public Int32 Cou_ID {
    			get {
    				return this._Cou_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ID, _Cou_ID, value);
    				this._Cou_ID = value;
    			}
    		}
    		
    		public DateTime Stc_CrtTime {
    			get {
    				return this._Stc_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stc_CrtTime, _Stc_CrtTime, value);
    				this._Stc_CrtTime = value;
    			}
    		}
    		
    		public Single Stc_Money {
    			get {
    				return this._Stc_Money;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stc_Money, _Stc_Money, value);
    				this._Stc_Money = value;
    			}
    		}
    		
    		public DateTime Stc_StartTime {
    			get {
    				return this._Stc_StartTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stc_StartTime, _Stc_StartTime, value);
    				this._Stc_StartTime = value;
    			}
    		}
    		
    		public DateTime Stc_EndTime {
    			get {
    				return this._Stc_EndTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stc_EndTime, _Stc_EndTime, value);
    				this._Stc_EndTime = value;
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
    		
    		public String Rc_Code {
    			get {
    				return this._Rc_Code;
    			}
    			set {
    				this.OnPropertyValueChange(_.Rc_Code, _Rc_Code, value);
    				this._Rc_Code = value;
    			}
    		}
    		
    		public Boolean Stc_IsFree {
    			get {
    				return this._Stc_IsFree;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stc_IsFree, _Stc_IsFree, value);
    				this._Stc_IsFree = value;
    			}
    		}
    		
    		public Boolean Stc_IsTry {
    			get {
    				return this._Stc_IsTry;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stc_IsTry, _Stc_IsTry, value);
    				this._Stc_IsTry = value;
    			}
    		}
    		
    		public Double Stc_QuesScore {
    			get {
    				return this._Stc_QuesScore;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stc_QuesScore, _Stc_QuesScore, value);
    				this._Stc_QuesScore = value;
    			}
    		}
    		
    		public Double Stc_StudyScore {
    			get {
    				return this._Stc_StudyScore;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stc_StudyScore, _Stc_StudyScore, value);
    				this._Stc_StudyScore = value;
    			}
    		}
    		
    		public Double Stc_ExamScore {
    			get {
    				return this._Stc_ExamScore;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stc_ExamScore, _Stc_ExamScore, value);
    				this._Stc_ExamScore = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Student_Course>("Student_Course");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Stc_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Stc_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Stc_ID,
    					_.Ac_ID,
    					_.Cou_ID,
    					_.Stc_CrtTime,
    					_.Stc_Money,
    					_.Stc_StartTime,
    					_.Stc_EndTime,
    					_.Org_ID,
    					_.Rc_Code,
    					_.Stc_IsFree,
    					_.Stc_IsTry,
    					_.Stc_QuesScore,
    					_.Stc_StudyScore,
    					_.Stc_ExamScore};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Stc_ID,
    					this._Ac_ID,
    					this._Cou_ID,
    					this._Stc_CrtTime,
    					this._Stc_Money,
    					this._Stc_StartTime,
    					this._Stc_EndTime,
    					this._Org_ID,
    					this._Rc_Code,
    					this._Stc_IsFree,
    					this._Stc_IsTry,
    					this._Stc_QuesScore,
    					this._Stc_StudyScore,
    					this._Stc_ExamScore};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Stc_ID))) {
    				this._Stc_ID = reader.GetInt32(_.Stc_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Stc_CrtTime))) {
    				this._Stc_CrtTime = reader.GetDateTime(_.Stc_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Stc_Money))) {
    				this._Stc_Money = reader.GetFloat(_.Stc_Money);
    			}
    			if ((false == reader.IsDBNull(_.Stc_StartTime))) {
    				this._Stc_StartTime = reader.GetDateTime(_.Stc_StartTime);
    			}
    			if ((false == reader.IsDBNull(_.Stc_EndTime))) {
    				this._Stc_EndTime = reader.GetDateTime(_.Stc_EndTime);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Rc_Code))) {
    				this._Rc_Code = reader.GetString(_.Rc_Code);
    			}
    			if ((false == reader.IsDBNull(_.Stc_IsFree))) {
    				this._Stc_IsFree = reader.GetBoolean(_.Stc_IsFree);
    			}
    			if ((false == reader.IsDBNull(_.Stc_IsTry))) {
    				this._Stc_IsTry = reader.GetBoolean(_.Stc_IsTry);
    			}
    			if ((false == reader.IsDBNull(_.Stc_QuesScore))) {
    				this._Stc_QuesScore = reader.GetDouble(_.Stc_QuesScore);
    			}
    			if ((false == reader.IsDBNull(_.Stc_StudyScore))) {
    				this._Stc_StudyScore = reader.GetDouble(_.Stc_StudyScore);
    			}
    			if ((false == reader.IsDBNull(_.Stc_ExamScore))) {
    				this._Stc_ExamScore = reader.GetDouble(_.Stc_ExamScore);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Student_Course).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Student_Course>();
    			
    			/// <summary>
    			/// 字段名：Stc_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Stc_ID = new WeiSha.Data.Field<Student_Course>("Stc_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<Student_Course>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<Student_Course>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Stc_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Stc_CrtTime = new WeiSha.Data.Field<Student_Course>("Stc_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Stc_Money - 数据类型：Single
    			/// </summary>
    			public static WeiSha.Data.Field Stc_Money = new WeiSha.Data.Field<Student_Course>("Stc_Money");
    			
    			/// <summary>
    			/// 字段名：Stc_StartTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Stc_StartTime = new WeiSha.Data.Field<Student_Course>("Stc_StartTime");
    			
    			/// <summary>
    			/// 字段名：Stc_EndTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Stc_EndTime = new WeiSha.Data.Field<Student_Course>("Stc_EndTime");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Student_Course>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Rc_Code - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Rc_Code = new WeiSha.Data.Field<Student_Course>("Rc_Code");
    			
    			/// <summary>
    			/// 字段名：Stc_IsFree - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Stc_IsFree = new WeiSha.Data.Field<Student_Course>("Stc_IsFree");
    			
    			/// <summary>
    			/// 字段名：Stc_IsTry - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Stc_IsTry = new WeiSha.Data.Field<Student_Course>("Stc_IsTry");
    			
    			/// <summary>
    			/// 字段名：Stc_QuesScore - 数据类型：Double
    			/// </summary>
    			public static WeiSha.Data.Field Stc_QuesScore = new WeiSha.Data.Field<Student_Course>("Stc_QuesScore");
    			
    			/// <summary>
    			/// 字段名：Stc_StudyScore - 数据类型：Double
    			/// </summary>
    			public static WeiSha.Data.Field Stc_StudyScore = new WeiSha.Data.Field<Student_Course>("Stc_StudyScore");
    			
    			/// <summary>
    			/// 字段名：Stc_ExamScore - 数据类型：Double
    			/// </summary>
    			public static WeiSha.Data.Field Stc_ExamScore = new WeiSha.Data.Field<Student_Course>("Stc_ExamScore");
    		}
    	}
    }
    