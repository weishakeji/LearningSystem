namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：LogForStudentQuestions 主键列：Lsq_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class LogForStudentQuestions : WeiSha.Data.Entity {
    		
    		protected Int32 _Lsq_ID;
    		
    		protected Int32 _Org_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected String _Ac_AccName;
    		
    		protected Int32 _Cou_ID;
    		
    		protected Int32 _Ol_ID;
    		
    		protected DateTime _Lsq_CrtTime;
    		
    		protected DateTime _Lsq_LastTime;
    		
    		protected Int32 _Qus_ID;
    		
    		protected Int32 _Lsq_Index;
    		
    		public Int32 Lsq_ID {
    			get {
    				return this._Lsq_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lsq_ID, _Lsq_ID, value);
    				this._Lsq_ID = value;
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
    		
    		public Int32 Ac_ID {
    			get {
    				return this._Ac_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_ID, _Ac_ID, value);
    				this._Ac_ID = value;
    			}
    		}
    		
    		public String Ac_AccName {
    			get {
    				return this._Ac_AccName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_AccName, _Ac_AccName, value);
    				this._Ac_AccName = value;
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
    		
    		public DateTime Lsq_CrtTime {
    			get {
    				return this._Lsq_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lsq_CrtTime, _Lsq_CrtTime, value);
    				this._Lsq_CrtTime = value;
    			}
    		}
    		
    		public DateTime Lsq_LastTime {
    			get {
    				return this._Lsq_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lsq_LastTime, _Lsq_LastTime, value);
    				this._Lsq_LastTime = value;
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
    		
    		public Int32 Lsq_Index {
    			get {
    				return this._Lsq_Index;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lsq_Index, _Lsq_Index, value);
    				this._Lsq_Index = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<LogForStudentQuestions>("LogForStudentQuestions");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Lsq_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lsq_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lsq_ID,
    					_.Org_ID,
    					_.Ac_ID,
    					_.Ac_AccName,
    					_.Cou_ID,
    					_.Ol_ID,
    					_.Lsq_CrtTime,
    					_.Lsq_LastTime,
    					_.Qus_ID,
    					_.Lsq_Index};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Lsq_ID,
    					this._Org_ID,
    					this._Ac_ID,
    					this._Ac_AccName,
    					this._Cou_ID,
    					this._Ol_ID,
    					this._Lsq_CrtTime,
    					this._Lsq_LastTime,
    					this._Qus_ID,
    					this._Lsq_Index};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Lsq_ID))) {
    				this._Lsq_ID = reader.GetInt32(_.Lsq_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_AccName))) {
    				this._Ac_AccName = reader.GetString(_.Ac_AccName);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ol_ID))) {
    				this._Ol_ID = reader.GetInt32(_.Ol_ID);
    			}
    			if ((false == reader.IsDBNull(_.Lsq_CrtTime))) {
    				this._Lsq_CrtTime = reader.GetDateTime(_.Lsq_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Lsq_LastTime))) {
    				this._Lsq_LastTime = reader.GetDateTime(_.Lsq_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Qus_ID))) {
    				this._Qus_ID = reader.GetInt32(_.Qus_ID);
    			}
    			if ((false == reader.IsDBNull(_.Lsq_Index))) {
    				this._Lsq_Index = reader.GetInt32(_.Lsq_Index);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(LogForStudentQuestions).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<LogForStudentQuestions>();
    			
    			/// <summary>
    			/// 字段名：Lsq_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lsq_ID = new WeiSha.Data.Field<LogForStudentQuestions>("Lsq_ID");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<LogForStudentQuestions>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<LogForStudentQuestions>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_AccName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_AccName = new WeiSha.Data.Field<LogForStudentQuestions>("Ac_AccName");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<LogForStudentQuestions>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Ol_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ol_ID = new WeiSha.Data.Field<LogForStudentQuestions>("Ol_ID");
    			
    			/// <summary>
    			/// 字段名：Lsq_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lsq_CrtTime = new WeiSha.Data.Field<LogForStudentQuestions>("Lsq_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Lsq_LastTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Lsq_LastTime = new WeiSha.Data.Field<LogForStudentQuestions>("Lsq_LastTime");
    			
    			/// <summary>
    			/// 字段名：Qus_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_ID = new WeiSha.Data.Field<LogForStudentQuestions>("Qus_ID");
    			
    			/// <summary>
    			/// 字段名：Lsq_Index - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lsq_Index = new WeiSha.Data.Field<LogForStudentQuestions>("Lsq_Index");
    		}
    	}
    }
    