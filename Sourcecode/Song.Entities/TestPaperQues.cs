namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：TestPaperQues 主键列：Tq_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class TestPaperQues : WeiSha.Data.Entity {
    		
    		protected Int32 _Tq_Id;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Int32 _Qk_Id;
    		
    		protected Int64 _Tp_Id;
    		
    		protected Double _Tq_Number;
    		
    		protected Int32 _Tq_Percent;
    		
    		protected Int32 _Tq_Type;
    		
    		public Int32 Tq_Id {
    			get {
    				return this._Tq_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tq_Id, _Tq_Id, value);
    				this._Tq_Id = value;
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
    		
    		public Int32 Qk_Id {
    			get {
    				return this._Qk_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qk_Id, _Qk_Id, value);
    				this._Qk_Id = value;
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
    		
    		public Double Tq_Number {
    			get {
    				return this._Tq_Number;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tq_Number, _Tq_Number, value);
    				this._Tq_Number = value;
    			}
    		}
    		
    		public Int32 Tq_Percent {
    			get {
    				return this._Tq_Percent;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tq_Percent, _Tq_Percent, value);
    				this._Tq_Percent = value;
    			}
    		}
    		
    		public Int32 Tq_Type {
    			get {
    				return this._Tq_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tq_Type, _Tq_Type, value);
    				this._Tq_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<TestPaperQues>("TestPaperQues");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Tq_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Tq_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Tq_Id,
    					_.Org_ID,
    					_.Org_Name,
    					_.Qk_Id,
    					_.Tp_Id,
    					_.Tq_Number,
    					_.Tq_Percent,
    					_.Tq_Type};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Tq_Id,
    					this._Org_ID,
    					this._Org_Name,
    					this._Qk_Id,
    					this._Tp_Id,
    					this._Tq_Number,
    					this._Tq_Percent,
    					this._Tq_Type};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Tq_Id))) {
    				this._Tq_Id = reader.GetInt32(_.Tq_Id);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Qk_Id))) {
    				this._Qk_Id = reader.GetInt32(_.Qk_Id);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Id))) {
    				this._Tp_Id = reader.GetInt64(_.Tp_Id);
    			}
    			if ((false == reader.IsDBNull(_.Tq_Number))) {
    				this._Tq_Number = reader.GetDouble(_.Tq_Number);
    			}
    			if ((false == reader.IsDBNull(_.Tq_Percent))) {
    				this._Tq_Percent = reader.GetInt32(_.Tq_Percent);
    			}
    			if ((false == reader.IsDBNull(_.Tq_Type))) {
    				this._Tq_Type = reader.GetInt32(_.Tq_Type);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(TestPaperQues).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<TestPaperQues>();
    			
    			/// <summary>
    			/// 字段名：Tq_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tq_Id = new WeiSha.Data.Field<TestPaperQues>("Tq_Id");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<TestPaperQues>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<TestPaperQues>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Qk_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qk_Id = new WeiSha.Data.Field<TestPaperQues>("Qk_Id");
    			
    			/// <summary>
    			/// 字段名：Tp_Id - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Id = new WeiSha.Data.Field<TestPaperQues>("Tp_Id");
    			
    			/// <summary>
    			/// 字段名：Tq_Number - 数据类型：Double
    			/// </summary>
    			public static WeiSha.Data.Field Tq_Number = new WeiSha.Data.Field<TestPaperQues>("Tq_Number");
    			
    			/// <summary>
    			/// 字段名：Tq_Percent - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tq_Percent = new WeiSha.Data.Field<TestPaperQues>("Tq_Percent");
    			
    			/// <summary>
    			/// 字段名：Tq_Type - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tq_Type = new WeiSha.Data.Field<TestPaperQues>("Tq_Type");
    		}
    	}
    }
    