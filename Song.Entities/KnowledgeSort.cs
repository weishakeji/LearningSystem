namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：KnowledgeSort 主键列：Kns_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class KnowledgeSort : WeiSha.Data.Entity {
    		
    		protected Int32 _Kns_ID;
    		
    		protected String _Kns_Name;
    		
    		protected String _Kns_ByName;
    		
    		protected Int32 _Kns_PID;
    		
    		protected Int32 _Kns_Tax;
    		
    		protected Boolean _Kns_IsUse;
    		
    		protected String _Kns_Type;
    		
    		protected String _Kns_Intro;
    		
    		protected DateTime? _Kns_CrtTime;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Int32 _Cou_ID;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Kns_ID {
    			get {
    				return this._Kns_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kns_ID, _Kns_ID, value);
    				this._Kns_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Kns_Name {
    			get {
    				return this._Kns_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kns_Name, _Kns_Name, value);
    				this._Kns_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Kns_ByName {
    			get {
    				return this._Kns_ByName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kns_ByName, _Kns_ByName, value);
    				this._Kns_ByName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Kns_PID {
    			get {
    				return this._Kns_PID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kns_PID, _Kns_PID, value);
    				this._Kns_PID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Kns_Tax {
    			get {
    				return this._Kns_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kns_Tax, _Kns_Tax, value);
    				this._Kns_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Kns_IsUse {
    			get {
    				return this._Kns_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kns_IsUse, _Kns_IsUse, value);
    				this._Kns_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Kns_Type {
    			get {
    				return this._Kns_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kns_Type, _Kns_Type, value);
    				this._Kns_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Kns_Intro {
    			get {
    				return this._Kns_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kns_Intro, _Kns_Intro, value);
    				this._Kns_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Kns_CrtTime {
    			get {
    				return this._Kns_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kns_CrtTime, _Kns_CrtTime, value);
    				this._Kns_CrtTime = value;
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
    		
    		public Int32 Cou_ID {
    			get {
    				return this._Cou_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ID, _Cou_ID, value);
    				this._Cou_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<KnowledgeSort>("KnowledgeSort");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Kns_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Kns_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Kns_ID,
    					_.Kns_Name,
    					_.Kns_ByName,
    					_.Kns_PID,
    					_.Kns_Tax,
    					_.Kns_IsUse,
    					_.Kns_Type,
    					_.Kns_Intro,
    					_.Kns_CrtTime,
    					_.Org_ID,
    					_.Org_Name,
    					_.Cou_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Kns_ID,
    					this._Kns_Name,
    					this._Kns_ByName,
    					this._Kns_PID,
    					this._Kns_Tax,
    					this._Kns_IsUse,
    					this._Kns_Type,
    					this._Kns_Intro,
    					this._Kns_CrtTime,
    					this._Org_ID,
    					this._Org_Name,
    					this._Cou_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Kns_ID))) {
    				this._Kns_ID = reader.GetInt32(_.Kns_ID);
    			}
    			if ((false == reader.IsDBNull(_.Kns_Name))) {
    				this._Kns_Name = reader.GetString(_.Kns_Name);
    			}
    			if ((false == reader.IsDBNull(_.Kns_ByName))) {
    				this._Kns_ByName = reader.GetString(_.Kns_ByName);
    			}
    			if ((false == reader.IsDBNull(_.Kns_PID))) {
    				this._Kns_PID = reader.GetInt32(_.Kns_PID);
    			}
    			if ((false == reader.IsDBNull(_.Kns_Tax))) {
    				this._Kns_Tax = reader.GetInt32(_.Kns_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Kns_IsUse))) {
    				this._Kns_IsUse = reader.GetBoolean(_.Kns_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Kns_Type))) {
    				this._Kns_Type = reader.GetString(_.Kns_Type);
    			}
    			if ((false == reader.IsDBNull(_.Kns_Intro))) {
    				this._Kns_Intro = reader.GetString(_.Kns_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Kns_CrtTime))) {
    				this._Kns_CrtTime = reader.GetDateTime(_.Kns_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(KnowledgeSort).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<KnowledgeSort>();
    			
    			/// <summary>
    			/// -1 - 字段名：Kns_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Kns_ID = new WeiSha.Data.Field<KnowledgeSort>("Kns_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Kns_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kns_Name = new WeiSha.Data.Field<KnowledgeSort>("Kns_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Kns_ByName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kns_ByName = new WeiSha.Data.Field<KnowledgeSort>("Kns_ByName");
    			
    			/// <summary>
    			/// -1 - 字段名：Kns_PID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Kns_PID = new WeiSha.Data.Field<KnowledgeSort>("Kns_PID");
    			
    			/// <summary>
    			/// -1 - 字段名：Kns_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Kns_Tax = new WeiSha.Data.Field<KnowledgeSort>("Kns_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Kns_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Kns_IsUse = new WeiSha.Data.Field<KnowledgeSort>("Kns_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Kns_Type - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kns_Type = new WeiSha.Data.Field<KnowledgeSort>("Kns_Type");
    			
    			/// <summary>
    			/// -1 - 字段名：Kns_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kns_Intro = new WeiSha.Data.Field<KnowledgeSort>("Kns_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Kns_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Kns_CrtTime = new WeiSha.Data.Field<KnowledgeSort>("Kns_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<KnowledgeSort>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<KnowledgeSort>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<KnowledgeSort>("Cou_ID");
    		}
    	}
    }
    