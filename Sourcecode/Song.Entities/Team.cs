namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Team 主键列：Team_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Team : WeiSha.Data.Entity {
    		
    		protected Int32 _Team_ID;
    		
    		protected Int32? _Sbj_ID;
    		
    		protected String _Sbj_Name;
    		
    		protected String _Team_Name;
    		
    		protected String _Team_ByName;
    		
    		protected String _Team_Intro;
    		
    		protected Int32? _Team_Tax;
    		
    		protected Boolean _Team_IsUse;
    		
    		protected Int32? _Team_Count;
    		
    		protected Int32? _Dep_ID;
    		
    		protected String _Dep_Name;
    		
    		protected DateTime? _Team_CrtTime;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Team_ID {
    			get {
    				return this._Team_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Team_ID, _Team_ID, value);
    				this._Team_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Sbj_ID {
    			get {
    				return this._Sbj_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_ID, _Sbj_ID, value);
    				this._Sbj_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sbj_Name {
    			get {
    				return this._Sbj_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_Name, _Sbj_Name, value);
    				this._Sbj_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Team_Name {
    			get {
    				return this._Team_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Team_Name, _Team_Name, value);
    				this._Team_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Team_ByName {
    			get {
    				return this._Team_ByName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Team_ByName, _Team_ByName, value);
    				this._Team_ByName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Team_Intro {
    			get {
    				return this._Team_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Team_Intro, _Team_Intro, value);
    				this._Team_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Team_Tax {
    			get {
    				return this._Team_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Team_Tax, _Team_Tax, value);
    				this._Team_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Team_IsUse {
    			get {
    				return this._Team_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Team_IsUse, _Team_IsUse, value);
    				this._Team_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Team_Count {
    			get {
    				return this._Team_Count;
    			}
    			set {
    				this.OnPropertyValueChange(_.Team_Count, _Team_Count, value);
    				this._Team_Count = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Dep_ID {
    			get {
    				return this._Dep_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_ID, _Dep_ID, value);
    				this._Dep_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dep_Name {
    			get {
    				return this._Dep_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Name, _Dep_Name, value);
    				this._Dep_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Team_CrtTime {
    			get {
    				return this._Team_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Team_CrtTime, _Team_CrtTime, value);
    				this._Team_CrtTime = value;
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
    			return new WeiSha.Data.Table<Team>("Team");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Team_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Team_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Team_ID,
    					_.Sbj_ID,
    					_.Sbj_Name,
    					_.Team_Name,
    					_.Team_ByName,
    					_.Team_Intro,
    					_.Team_Tax,
    					_.Team_IsUse,
    					_.Team_Count,
    					_.Dep_ID,
    					_.Dep_Name,
    					_.Team_CrtTime,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Team_ID,
    					this._Sbj_ID,
    					this._Sbj_Name,
    					this._Team_Name,
    					this._Team_ByName,
    					this._Team_Intro,
    					this._Team_Tax,
    					this._Team_IsUse,
    					this._Team_Count,
    					this._Dep_ID,
    					this._Dep_Name,
    					this._Team_CrtTime,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Team_ID))) {
    				this._Team_ID = reader.GetInt32(_.Team_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt32(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Name))) {
    				this._Sbj_Name = reader.GetString(_.Sbj_Name);
    			}
    			if ((false == reader.IsDBNull(_.Team_Name))) {
    				this._Team_Name = reader.GetString(_.Team_Name);
    			}
    			if ((false == reader.IsDBNull(_.Team_ByName))) {
    				this._Team_ByName = reader.GetString(_.Team_ByName);
    			}
    			if ((false == reader.IsDBNull(_.Team_Intro))) {
    				this._Team_Intro = reader.GetString(_.Team_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Team_Tax))) {
    				this._Team_Tax = reader.GetInt32(_.Team_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Team_IsUse))) {
    				this._Team_IsUse = reader.GetBoolean(_.Team_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Team_Count))) {
    				this._Team_Count = reader.GetInt32(_.Team_Count);
    			}
    			if ((false == reader.IsDBNull(_.Dep_ID))) {
    				this._Dep_ID = reader.GetInt32(_.Dep_ID);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Name))) {
    				this._Dep_Name = reader.GetString(_.Dep_Name);
    			}
    			if ((false == reader.IsDBNull(_.Team_CrtTime))) {
    				this._Team_CrtTime = reader.GetDateTime(_.Team_CrtTime);
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
    			if ((false == typeof(Team).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Team>();
    			
    			/// <summary>
    			/// -1 - 字段名：Team_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Team_ID = new WeiSha.Data.Field<Team>("Team_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_ID - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<Team>("Sbj_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Name = new WeiSha.Data.Field<Team>("Sbj_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Team_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Team_Name = new WeiSha.Data.Field<Team>("Team_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Team_ByName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Team_ByName = new WeiSha.Data.Field<Team>("Team_ByName");
    			
    			/// <summary>
    			/// -1 - 字段名：Team_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Team_Intro = new WeiSha.Data.Field<Team>("Team_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Team_Tax - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Team_Tax = new WeiSha.Data.Field<Team>("Team_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Team_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Team_IsUse = new WeiSha.Data.Field<Team>("Team_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Team_Count - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Team_Count = new WeiSha.Data.Field<Team>("Team_Count");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_ID - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Dep_ID = new WeiSha.Data.Field<Team>("Dep_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Name = new WeiSha.Data.Field<Team>("Dep_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Team_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Team_CrtTime = new WeiSha.Data.Field<Team>("Team_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Team>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Team>("Org_Name");
    		}
    	}
    }
    