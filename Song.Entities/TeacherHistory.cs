namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：TeacherHistory 主键列：Thh_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class TeacherHistory : WeiSha.Data.Entity {
    		
    		protected Int32 _Thh_ID;
    		
    		protected String _Thh_Theme;
    		
    		protected DateTime _Thh_StartTime;
    		
    		protected DateTime? _Thh_EndTime;
    		
    		protected DateTime? _Thh_CrtTime;
    		
    		protected String _Thh_Type;
    		
    		protected String _Thh_Intro;
    		
    		protected String _Thh_Success;
    		
    		protected String _Thh_School;
    		
    		protected String _Thh_Major;
    		
    		protected String _Thh_Education;
    		
    		protected String _Thh_Compay;
    		
    		protected String _Thh_Job;
    		
    		protected String _Thh_Post;
    		
    		protected Int32 _Th_ID;
    		
    		protected String _Th_Name;
    		
    		public Int32 Thh_ID {
    			get {
    				return this._Thh_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thh_ID, _Thh_ID, value);
    				this._Thh_ID = value;
    			}
    		}
    		
    		public String Thh_Theme {
    			get {
    				return this._Thh_Theme;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thh_Theme, _Thh_Theme, value);
    				this._Thh_Theme = value;
    			}
    		}
    		
    		public DateTime Thh_StartTime {
    			get {
    				return this._Thh_StartTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thh_StartTime, _Thh_StartTime, value);
    				this._Thh_StartTime = value;
    			}
    		}
    		
    		public DateTime? Thh_EndTime {
    			get {
    				return this._Thh_EndTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thh_EndTime, _Thh_EndTime, value);
    				this._Thh_EndTime = value;
    			}
    		}
    		
    		public DateTime? Thh_CrtTime {
    			get {
    				return this._Thh_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thh_CrtTime, _Thh_CrtTime, value);
    				this._Thh_CrtTime = value;
    			}
    		}
    		
    		public String Thh_Type {
    			get {
    				return this._Thh_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thh_Type, _Thh_Type, value);
    				this._Thh_Type = value;
    			}
    		}
    		
    		public String Thh_Intro {
    			get {
    				return this._Thh_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thh_Intro, _Thh_Intro, value);
    				this._Thh_Intro = value;
    			}
    		}
    		
    		public String Thh_Success {
    			get {
    				return this._Thh_Success;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thh_Success, _Thh_Success, value);
    				this._Thh_Success = value;
    			}
    		}
    		
    		public String Thh_School {
    			get {
    				return this._Thh_School;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thh_School, _Thh_School, value);
    				this._Thh_School = value;
    			}
    		}
    		
    		public String Thh_Major {
    			get {
    				return this._Thh_Major;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thh_Major, _Thh_Major, value);
    				this._Thh_Major = value;
    			}
    		}
    		
    		public String Thh_Education {
    			get {
    				return this._Thh_Education;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thh_Education, _Thh_Education, value);
    				this._Thh_Education = value;
    			}
    		}
    		
    		public String Thh_Compay {
    			get {
    				return this._Thh_Compay;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thh_Compay, _Thh_Compay, value);
    				this._Thh_Compay = value;
    			}
    		}
    		
    		public String Thh_Job {
    			get {
    				return this._Thh_Job;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thh_Job, _Thh_Job, value);
    				this._Thh_Job = value;
    			}
    		}
    		
    		public String Thh_Post {
    			get {
    				return this._Thh_Post;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thh_Post, _Thh_Post, value);
    				this._Thh_Post = value;
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
    			return new WeiSha.Data.Table<TeacherHistory>("TeacherHistory");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Thh_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Thh_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Thh_ID,
    					_.Thh_Theme,
    					_.Thh_StartTime,
    					_.Thh_EndTime,
    					_.Thh_CrtTime,
    					_.Thh_Type,
    					_.Thh_Intro,
    					_.Thh_Success,
    					_.Thh_School,
    					_.Thh_Major,
    					_.Thh_Education,
    					_.Thh_Compay,
    					_.Thh_Job,
    					_.Thh_Post,
    					_.Th_ID,
    					_.Th_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Thh_ID,
    					this._Thh_Theme,
    					this._Thh_StartTime,
    					this._Thh_EndTime,
    					this._Thh_CrtTime,
    					this._Thh_Type,
    					this._Thh_Intro,
    					this._Thh_Success,
    					this._Thh_School,
    					this._Thh_Major,
    					this._Thh_Education,
    					this._Thh_Compay,
    					this._Thh_Job,
    					this._Thh_Post,
    					this._Th_ID,
    					this._Th_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Thh_ID))) {
    				this._Thh_ID = reader.GetInt32(_.Thh_ID);
    			}
    			if ((false == reader.IsDBNull(_.Thh_Theme))) {
    				this._Thh_Theme = reader.GetString(_.Thh_Theme);
    			}
    			if ((false == reader.IsDBNull(_.Thh_StartTime))) {
    				this._Thh_StartTime = reader.GetDateTime(_.Thh_StartTime);
    			}
    			if ((false == reader.IsDBNull(_.Thh_EndTime))) {
    				this._Thh_EndTime = reader.GetDateTime(_.Thh_EndTime);
    			}
    			if ((false == reader.IsDBNull(_.Thh_CrtTime))) {
    				this._Thh_CrtTime = reader.GetDateTime(_.Thh_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Thh_Type))) {
    				this._Thh_Type = reader.GetString(_.Thh_Type);
    			}
    			if ((false == reader.IsDBNull(_.Thh_Intro))) {
    				this._Thh_Intro = reader.GetString(_.Thh_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Thh_Success))) {
    				this._Thh_Success = reader.GetString(_.Thh_Success);
    			}
    			if ((false == reader.IsDBNull(_.Thh_School))) {
    				this._Thh_School = reader.GetString(_.Thh_School);
    			}
    			if ((false == reader.IsDBNull(_.Thh_Major))) {
    				this._Thh_Major = reader.GetString(_.Thh_Major);
    			}
    			if ((false == reader.IsDBNull(_.Thh_Education))) {
    				this._Thh_Education = reader.GetString(_.Thh_Education);
    			}
    			if ((false == reader.IsDBNull(_.Thh_Compay))) {
    				this._Thh_Compay = reader.GetString(_.Thh_Compay);
    			}
    			if ((false == reader.IsDBNull(_.Thh_Job))) {
    				this._Thh_Job = reader.GetString(_.Thh_Job);
    			}
    			if ((false == reader.IsDBNull(_.Thh_Post))) {
    				this._Thh_Post = reader.GetString(_.Thh_Post);
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
    			if ((false == typeof(TeacherHistory).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<TeacherHistory>();
    			
    			/// <summary>
    			/// 字段名：Thh_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Thh_ID = new WeiSha.Data.Field<TeacherHistory>("Thh_ID");
    			
    			/// <summary>
    			/// 字段名：Thh_Theme - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Thh_Theme = new WeiSha.Data.Field<TeacherHistory>("Thh_Theme");
    			
    			/// <summary>
    			/// 字段名：Thh_StartTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Thh_StartTime = new WeiSha.Data.Field<TeacherHistory>("Thh_StartTime");
    			
    			/// <summary>
    			/// 字段名：Thh_EndTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Thh_EndTime = new WeiSha.Data.Field<TeacherHistory>("Thh_EndTime");
    			
    			/// <summary>
    			/// 字段名：Thh_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Thh_CrtTime = new WeiSha.Data.Field<TeacherHistory>("Thh_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Thh_Type - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Thh_Type = new WeiSha.Data.Field<TeacherHistory>("Thh_Type");
    			
    			/// <summary>
    			/// 字段名：Thh_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Thh_Intro = new WeiSha.Data.Field<TeacherHistory>("Thh_Intro");
    			
    			/// <summary>
    			/// 字段名：Thh_Success - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Thh_Success = new WeiSha.Data.Field<TeacherHistory>("Thh_Success");
    			
    			/// <summary>
    			/// 字段名：Thh_School - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Thh_School = new WeiSha.Data.Field<TeacherHistory>("Thh_School");
    			
    			/// <summary>
    			/// 字段名：Thh_Major - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Thh_Major = new WeiSha.Data.Field<TeacherHistory>("Thh_Major");
    			
    			/// <summary>
    			/// 字段名：Thh_Education - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Thh_Education = new WeiSha.Data.Field<TeacherHistory>("Thh_Education");
    			
    			/// <summary>
    			/// 字段名：Thh_Compay - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Thh_Compay = new WeiSha.Data.Field<TeacherHistory>("Thh_Compay");
    			
    			/// <summary>
    			/// 字段名：Thh_Job - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Thh_Job = new WeiSha.Data.Field<TeacherHistory>("Thh_Job");
    			
    			/// <summary>
    			/// 字段名：Thh_Post - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Thh_Post = new WeiSha.Data.Field<TeacherHistory>("Thh_Post");
    			
    			/// <summary>
    			/// 字段名：Th_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_ID = new WeiSha.Data.Field<TeacherHistory>("Th_ID");
    			
    			/// <summary>
    			/// 字段名：Th_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Name = new WeiSha.Data.Field<TeacherHistory>("Th_Name");
    		}
    	}
    }
    