namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：StudentSort_Course 主键列：Ssc_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class StudentSort_Course : WeiSha.Data.Entity {
    		
    		protected Int32 _Ssc_ID;
    		
    		protected Int64 _Cou_ID;
    		
    		protected DateTime _Ssc_EndTime;
    		
    		protected Boolean _Ssc_IsEnable;
    		
    		protected DateTime _Ssc_StartTime;
    		
    		protected Int64 _Sts_ID;
    		
    		public Int32 Ssc_ID {
    			get {
    				return this._Ssc_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ssc_ID, _Ssc_ID, value);
    				this._Ssc_ID = value;
    			}
    		}
    		
    		public Int64 Cou_ID {
    			get {
    				return this._Cou_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ID, _Cou_ID, value);
    				this._Cou_ID = value;
    			}
    		}
    		
    		public DateTime Ssc_EndTime {
    			get {
    				return this._Ssc_EndTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ssc_EndTime, _Ssc_EndTime, value);
    				this._Ssc_EndTime = value;
    			}
    		}
    		
    		public Boolean Ssc_IsEnable {
    			get {
    				return this._Ssc_IsEnable;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ssc_IsEnable, _Ssc_IsEnable, value);
    				this._Ssc_IsEnable = value;
    			}
    		}
    		
    		public DateTime Ssc_StartTime {
    			get {
    				return this._Ssc_StartTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ssc_StartTime, _Ssc_StartTime, value);
    				this._Ssc_StartTime = value;
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
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<StudentSort_Course>("StudentSort_Course");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Ssc_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ssc_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ssc_ID,
    					_.Cou_ID,
    					_.Ssc_EndTime,
    					_.Ssc_IsEnable,
    					_.Ssc_StartTime,
    					_.Sts_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Ssc_ID,
    					this._Cou_ID,
    					this._Ssc_EndTime,
    					this._Ssc_IsEnable,
    					this._Ssc_StartTime,
    					this._Sts_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Ssc_ID))) {
    				this._Ssc_ID = reader.GetInt32(_.Ssc_ID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt64(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ssc_EndTime))) {
    				this._Ssc_EndTime = reader.GetDateTime(_.Ssc_EndTime);
    			}
    			if ((false == reader.IsDBNull(_.Ssc_IsEnable))) {
    				this._Ssc_IsEnable = reader.GetBoolean(_.Ssc_IsEnable);
    			}
    			if ((false == reader.IsDBNull(_.Ssc_StartTime))) {
    				this._Ssc_StartTime = reader.GetDateTime(_.Ssc_StartTime);
    			}
    			if ((false == reader.IsDBNull(_.Sts_ID))) {
    				this._Sts_ID = reader.GetInt64(_.Sts_ID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(StudentSort_Course).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<StudentSort_Course>();
    			
    			/// <summary>
    			/// 字段名：Ssc_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ssc_ID = new WeiSha.Data.Field<StudentSort_Course>("Ssc_ID");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<StudentSort_Course>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Ssc_EndTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Ssc_EndTime = new WeiSha.Data.Field<StudentSort_Course>("Ssc_EndTime");
    			
    			/// <summary>
    			/// 字段名：Ssc_IsEnable - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ssc_IsEnable = new WeiSha.Data.Field<StudentSort_Course>("Ssc_IsEnable");
    			
    			/// <summary>
    			/// 字段名：Ssc_StartTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Ssc_StartTime = new WeiSha.Data.Field<StudentSort_Course>("Ssc_StartTime");
    			
    			/// <summary>
    			/// 字段名：Sts_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Sts_ID = new WeiSha.Data.Field<StudentSort_Course>("Sts_ID");
    		}
    	}
    }
    