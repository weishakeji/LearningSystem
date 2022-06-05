namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：SmsFault 主键列：Smf_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class SmsFault : WeiSha.Data.Entity {
    		
    		protected Int32 _Smf_Id;
    		
    		protected String _Smf_Context;
    		
    		protected DateTime? _Smf_SendTime;
    		
    		protected DateTime? _Smf_CrtTime;
    		
    		protected String _Smf_MobileTel;
    		
    		protected String _Smf_SendName;
    		
    		protected String _Smf_Company;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Smf_Id {
    			get {
    				return this._Smf_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Smf_Id, _Smf_Id, value);
    				this._Smf_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Smf_Context {
    			get {
    				return this._Smf_Context;
    			}
    			set {
    				this.OnPropertyValueChange(_.Smf_Context, _Smf_Context, value);
    				this._Smf_Context = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Smf_SendTime {
    			get {
    				return this._Smf_SendTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Smf_SendTime, _Smf_SendTime, value);
    				this._Smf_SendTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Smf_CrtTime {
    			get {
    				return this._Smf_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Smf_CrtTime, _Smf_CrtTime, value);
    				this._Smf_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Smf_MobileTel {
    			get {
    				return this._Smf_MobileTel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Smf_MobileTel, _Smf_MobileTel, value);
    				this._Smf_MobileTel = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Smf_SendName {
    			get {
    				return this._Smf_SendName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Smf_SendName, _Smf_SendName, value);
    				this._Smf_SendName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Smf_Company {
    			get {
    				return this._Smf_Company;
    			}
    			set {
    				this.OnPropertyValueChange(_.Smf_Company, _Smf_Company, value);
    				this._Smf_Company = value;
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
    			return new WeiSha.Data.Table<SmsFault>("SmsFault");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Smf_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Smf_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Smf_Id,
    					_.Smf_Context,
    					_.Smf_SendTime,
    					_.Smf_CrtTime,
    					_.Smf_MobileTel,
    					_.Smf_SendName,
    					_.Smf_Company,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Smf_Id,
    					this._Smf_Context,
    					this._Smf_SendTime,
    					this._Smf_CrtTime,
    					this._Smf_MobileTel,
    					this._Smf_SendName,
    					this._Smf_Company,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Smf_Id))) {
    				this._Smf_Id = reader.GetInt32(_.Smf_Id);
    			}
    			if ((false == reader.IsDBNull(_.Smf_Context))) {
    				this._Smf_Context = reader.GetString(_.Smf_Context);
    			}
    			if ((false == reader.IsDBNull(_.Smf_SendTime))) {
    				this._Smf_SendTime = reader.GetDateTime(_.Smf_SendTime);
    			}
    			if ((false == reader.IsDBNull(_.Smf_CrtTime))) {
    				this._Smf_CrtTime = reader.GetDateTime(_.Smf_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Smf_MobileTel))) {
    				this._Smf_MobileTel = reader.GetString(_.Smf_MobileTel);
    			}
    			if ((false == reader.IsDBNull(_.Smf_SendName))) {
    				this._Smf_SendName = reader.GetString(_.Smf_SendName);
    			}
    			if ((false == reader.IsDBNull(_.Smf_Company))) {
    				this._Smf_Company = reader.GetString(_.Smf_Company);
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
    			if ((false == typeof(SmsFault).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<SmsFault>();
    			
    			/// <summary>
    			/// -1 - 字段名：Smf_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Smf_Id = new WeiSha.Data.Field<SmsFault>("Smf_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Smf_Context - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Smf_Context = new WeiSha.Data.Field<SmsFault>("Smf_Context");
    			
    			/// <summary>
    			/// -1 - 字段名：Smf_SendTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Smf_SendTime = new WeiSha.Data.Field<SmsFault>("Smf_SendTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Smf_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Smf_CrtTime = new WeiSha.Data.Field<SmsFault>("Smf_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Smf_MobileTel - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Smf_MobileTel = new WeiSha.Data.Field<SmsFault>("Smf_MobileTel");
    			
    			/// <summary>
    			/// -1 - 字段名：Smf_SendName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Smf_SendName = new WeiSha.Data.Field<SmsFault>("Smf_SendName");
    			
    			/// <summary>
    			/// -1 - 字段名：Smf_Company - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Smf_Company = new WeiSha.Data.Field<SmsFault>("Smf_Company");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<SmsFault>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<SmsFault>("Org_Name");
    		}
    	}
    }
    