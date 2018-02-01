namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Student_Ques 主键列：Squs_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Student_Ques : WeiSha.Data.Entity {
    		
    		protected Int32 _Squs_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected Int32 _Cou_ID;
    		
    		protected Int32 _Qus_ID;
    		
    		protected Int32 _Qus_Type;
    		
    		protected Int32 _Qus_Diff;
    		
    		protected Int32 _Sbj_ID;
    		
    		protected Int32 _Squs_Level;
    		
    		protected DateTime _Squs_CrtTime;
    		
    		public Int32 Squs_ID {
    			get {
    				return this._Squs_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Squs_ID, _Squs_ID, value);
    				this._Squs_ID = value;
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
    		
    		public Int32 Qus_ID {
    			get {
    				return this._Qus_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_ID, _Qus_ID, value);
    				this._Qus_ID = value;
    			}
    		}
    		
    		public Int32 Qus_Type {
    			get {
    				return this._Qus_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Type, _Qus_Type, value);
    				this._Qus_Type = value;
    			}
    		}
    		
    		public Int32 Qus_Diff {
    			get {
    				return this._Qus_Diff;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Diff, _Qus_Diff, value);
    				this._Qus_Diff = value;
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
    		
    		public Int32 Squs_Level {
    			get {
    				return this._Squs_Level;
    			}
    			set {
    				this.OnPropertyValueChange(_.Squs_Level, _Squs_Level, value);
    				this._Squs_Level = value;
    			}
    		}
    		
    		public DateTime Squs_CrtTime {
    			get {
    				return this._Squs_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Squs_CrtTime, _Squs_CrtTime, value);
    				this._Squs_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Student_Ques>("Student_Ques");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Squs_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Squs_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Squs_ID,
    					_.Ac_ID,
    					_.Cou_ID,
    					_.Qus_ID,
    					_.Qus_Type,
    					_.Qus_Diff,
    					_.Sbj_ID,
    					_.Squs_Level,
    					_.Squs_CrtTime};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Squs_ID,
    					this._Ac_ID,
    					this._Cou_ID,
    					this._Qus_ID,
    					this._Qus_Type,
    					this._Qus_Diff,
    					this._Sbj_ID,
    					this._Squs_Level,
    					this._Squs_CrtTime};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Squs_ID))) {
    				this._Squs_ID = reader.GetInt32(_.Squs_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qus_ID))) {
    				this._Qus_ID = reader.GetInt32(_.Qus_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Type))) {
    				this._Qus_Type = reader.GetInt32(_.Qus_Type);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Diff))) {
    				this._Qus_Diff = reader.GetInt32(_.Qus_Diff);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt32(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Squs_Level))) {
    				this._Squs_Level = reader.GetInt32(_.Squs_Level);
    			}
    			if ((false == reader.IsDBNull(_.Squs_CrtTime))) {
    				this._Squs_CrtTime = reader.GetDateTime(_.Squs_CrtTime);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Student_Ques).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Student_Ques>();
    			
    			/// <summary>
    			/// 字段名：Squs_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Squs_ID = new WeiSha.Data.Field<Student_Ques>("Squs_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<Student_Ques>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<Student_Ques>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Qus_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_ID = new WeiSha.Data.Field<Student_Ques>("Qus_ID");
    			
    			/// <summary>
    			/// 字段名：Qus_Type - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Type = new WeiSha.Data.Field<Student_Ques>("Qus_Type");
    			
    			/// <summary>
    			/// 字段名：Qus_Diff - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Diff = new WeiSha.Data.Field<Student_Ques>("Qus_Diff");
    			
    			/// <summary>
    			/// 字段名：Sbj_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<Student_Ques>("Sbj_ID");
    			
    			/// <summary>
    			/// 字段名：Squs_Level - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Squs_Level = new WeiSha.Data.Field<Student_Ques>("Squs_Level");
    			
    			/// <summary>
    			/// 字段名：Squs_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Squs_CrtTime = new WeiSha.Data.Field<Student_Ques>("Squs_CrtTime");
    		}
    	}
    }
    