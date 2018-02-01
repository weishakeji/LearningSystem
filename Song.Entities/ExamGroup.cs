namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ExamGroup 主键列：Eg_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ExamGroup : WeiSha.Data.Entity {
    		
    		protected Int32 _Eg_ID;
    		
    		protected Int32? _Eg_Type;
    		
    		protected Int32 _Sts_ID;
    		
    		protected String _Exam_UID;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Eg_ID {
    			get {
    				return this._Eg_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Eg_ID, _Eg_ID, value);
    				this._Eg_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Eg_Type {
    			get {
    				return this._Eg_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Eg_Type, _Eg_Type, value);
    				this._Eg_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Sts_ID {
    			get {
    				return this._Sts_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sts_ID, _Sts_ID, value);
    				this._Sts_ID = value;
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
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<ExamGroup>("ExamGroup");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Eg_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Eg_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Eg_ID,
    					_.Eg_Type,
    					_.Sts_ID,
    					_.Exam_UID,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Eg_ID,
    					this._Eg_Type,
    					this._Sts_ID,
    					this._Exam_UID,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Eg_ID))) {
    				this._Eg_ID = reader.GetInt32(_.Eg_ID);
    			}
    			if ((false == reader.IsDBNull(_.Eg_Type))) {
    				this._Eg_Type = reader.GetInt32(_.Eg_Type);
    			}
    			if ((false == reader.IsDBNull(_.Sts_ID))) {
    				this._Sts_ID = reader.GetInt32(_.Sts_ID);
    			}
    			if ((false == reader.IsDBNull(_.Exam_UID))) {
    				this._Exam_UID = reader.GetString(_.Exam_UID);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(ExamGroup).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<ExamGroup>();
    			
    			/// <summary>
    			/// -1 - 字段名：Eg_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Eg_ID = new WeiSha.Data.Field<ExamGroup>("Eg_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Eg_Type - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Eg_Type = new WeiSha.Data.Field<ExamGroup>("Eg_Type");
    			
    			/// <summary>
    			/// -1 - 字段名：Sts_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sts_ID = new WeiSha.Data.Field<ExamGroup>("Sts_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Exam_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Exam_UID = new WeiSha.Data.Field<ExamGroup>("Exam_UID");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<ExamGroup>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<ExamGroup>("Org_Name");
    		}
    	}
    }
    