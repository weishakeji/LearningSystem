namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：TestPaperItem 主键列：TPI_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class TestPaperItem : WeiSha.Data.Entity {
    		
    		protected Int32 _TPI_ID;
    		
    		protected String _Tp_UID;
    		
    		protected Int32 _TPI_Type;
    		
    		protected Int32 _TPI_Percent;
    		
    		protected Int32 _TPI_Number;
    		
    		protected Int32 _TPI_Count;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Int32 _Ol_ID;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 TPI_ID {
    			get {
    				return this._TPI_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.TPI_ID, _TPI_ID, value);
    				this._TPI_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Tp_UID {
    			get {
    				return this._Tp_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_UID, _Tp_UID, value);
    				this._Tp_UID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 TPI_Type {
    			get {
    				return this._TPI_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.TPI_Type, _TPI_Type, value);
    				this._TPI_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 TPI_Percent {
    			get {
    				return this._TPI_Percent;
    			}
    			set {
    				this.OnPropertyValueChange(_.TPI_Percent, _TPI_Percent, value);
    				this._TPI_Percent = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 TPI_Number {
    			get {
    				return this._TPI_Number;
    			}
    			set {
    				this.OnPropertyValueChange(_.TPI_Number, _TPI_Number, value);
    				this._TPI_Number = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 TPI_Count {
    			get {
    				return this._TPI_Count;
    			}
    			set {
    				this.OnPropertyValueChange(_.TPI_Count, _TPI_Count, value);
    				this._TPI_Count = value;
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
    		
    		public Int32 Ol_ID {
    			get {
    				return this._Ol_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ol_ID, _Ol_ID, value);
    				this._Ol_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<TestPaperItem>("TestPaperItem");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.TPI_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.TPI_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.TPI_ID,
    					_.Tp_UID,
    					_.TPI_Type,
    					_.TPI_Percent,
    					_.TPI_Number,
    					_.TPI_Count,
    					_.Org_ID,
    					_.Org_Name,
    					_.Ol_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._TPI_ID,
    					this._Tp_UID,
    					this._TPI_Type,
    					this._TPI_Percent,
    					this._TPI_Number,
    					this._TPI_Count,
    					this._Org_ID,
    					this._Org_Name,
    					this._Ol_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.TPI_ID))) {
    				this._TPI_ID = reader.GetInt32(_.TPI_ID);
    			}
    			if ((false == reader.IsDBNull(_.Tp_UID))) {
    				this._Tp_UID = reader.GetString(_.Tp_UID);
    			}
    			if ((false == reader.IsDBNull(_.TPI_Type))) {
    				this._TPI_Type = reader.GetInt32(_.TPI_Type);
    			}
    			if ((false == reader.IsDBNull(_.TPI_Percent))) {
    				this._TPI_Percent = reader.GetInt32(_.TPI_Percent);
    			}
    			if ((false == reader.IsDBNull(_.TPI_Number))) {
    				this._TPI_Number = reader.GetInt32(_.TPI_Number);
    			}
    			if ((false == reader.IsDBNull(_.TPI_Count))) {
    				this._TPI_Count = reader.GetInt32(_.TPI_Count);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ol_ID))) {
    				this._Ol_ID = reader.GetInt32(_.Ol_ID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(TestPaperItem).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<TestPaperItem>();
    			
    			/// <summary>
    			/// -1 - 字段名：TPI_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field TPI_ID = new WeiSha.Data.Field<TestPaperItem>("TPI_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tp_UID = new WeiSha.Data.Field<TestPaperItem>("Tp_UID");
    			
    			/// <summary>
    			/// -1 - 字段名：TPI_Type - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field TPI_Type = new WeiSha.Data.Field<TestPaperItem>("TPI_Type");
    			
    			/// <summary>
    			/// -1 - 字段名：TPI_Percent - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field TPI_Percent = new WeiSha.Data.Field<TestPaperItem>("TPI_Percent");
    			
    			/// <summary>
    			/// -1 - 字段名：TPI_Number - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field TPI_Number = new WeiSha.Data.Field<TestPaperItem>("TPI_Number");
    			
    			/// <summary>
    			/// -1 - 字段名：TPI_Count - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field TPI_Count = new WeiSha.Data.Field<TestPaperItem>("TPI_Count");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<TestPaperItem>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<TestPaperItem>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Ol_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ol_ID = new WeiSha.Data.Field<TestPaperItem>("Ol_ID");
    		}
    	}
    }
    