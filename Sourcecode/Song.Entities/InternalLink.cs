namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：InternalLink 主键列：IL_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class InternalLink : WeiSha.Data.Entity {
    		
    		protected Int32 _IL_ID;
    		
    		protected String _IL_Name;
    		
    		protected String _IL_Url;
    		
    		protected String _IL_Title;
    		
    		protected String _IL_Target;
    		
    		protected Boolean _IL_IsUse;
    		
    		protected DateTime? _IL_CrtTime;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 IL_ID {
    			get {
    				return this._IL_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.IL_ID, _IL_ID, value);
    				this._IL_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String IL_Name {
    			get {
    				return this._IL_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.IL_Name, _IL_Name, value);
    				this._IL_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String IL_Url {
    			get {
    				return this._IL_Url;
    			}
    			set {
    				this.OnPropertyValueChange(_.IL_Url, _IL_Url, value);
    				this._IL_Url = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String IL_Title {
    			get {
    				return this._IL_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.IL_Title, _IL_Title, value);
    				this._IL_Title = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String IL_Target {
    			get {
    				return this._IL_Target;
    			}
    			set {
    				this.OnPropertyValueChange(_.IL_Target, _IL_Target, value);
    				this._IL_Target = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean IL_IsUse {
    			get {
    				return this._IL_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.IL_IsUse, _IL_IsUse, value);
    				this._IL_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? IL_CrtTime {
    			get {
    				return this._IL_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.IL_CrtTime, _IL_CrtTime, value);
    				this._IL_CrtTime = value;
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
    			return new WeiSha.Data.Table<InternalLink>("InternalLink");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.IL_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.IL_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.IL_ID,
    					_.IL_Name,
    					_.IL_Url,
    					_.IL_Title,
    					_.IL_Target,
    					_.IL_IsUse,
    					_.IL_CrtTime,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._IL_ID,
    					this._IL_Name,
    					this._IL_Url,
    					this._IL_Title,
    					this._IL_Target,
    					this._IL_IsUse,
    					this._IL_CrtTime,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.IL_ID))) {
    				this._IL_ID = reader.GetInt32(_.IL_ID);
    			}
    			if ((false == reader.IsDBNull(_.IL_Name))) {
    				this._IL_Name = reader.GetString(_.IL_Name);
    			}
    			if ((false == reader.IsDBNull(_.IL_Url))) {
    				this._IL_Url = reader.GetString(_.IL_Url);
    			}
    			if ((false == reader.IsDBNull(_.IL_Title))) {
    				this._IL_Title = reader.GetString(_.IL_Title);
    			}
    			if ((false == reader.IsDBNull(_.IL_Target))) {
    				this._IL_Target = reader.GetString(_.IL_Target);
    			}
    			if ((false == reader.IsDBNull(_.IL_IsUse))) {
    				this._IL_IsUse = reader.GetBoolean(_.IL_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.IL_CrtTime))) {
    				this._IL_CrtTime = reader.GetDateTime(_.IL_CrtTime);
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
    			if ((false == typeof(InternalLink).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<InternalLink>();
    			
    			/// <summary>
    			/// -1 - 字段名：IL_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field IL_ID = new WeiSha.Data.Field<InternalLink>("IL_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：IL_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field IL_Name = new WeiSha.Data.Field<InternalLink>("IL_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：IL_Url - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field IL_Url = new WeiSha.Data.Field<InternalLink>("IL_Url");
    			
    			/// <summary>
    			/// -1 - 字段名：IL_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field IL_Title = new WeiSha.Data.Field<InternalLink>("IL_Title");
    			
    			/// <summary>
    			/// -1 - 字段名：IL_Target - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field IL_Target = new WeiSha.Data.Field<InternalLink>("IL_Target");
    			
    			/// <summary>
    			/// -1 - 字段名：IL_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field IL_IsUse = new WeiSha.Data.Field<InternalLink>("IL_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：IL_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field IL_CrtTime = new WeiSha.Data.Field<InternalLink>("IL_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<InternalLink>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<InternalLink>("Org_Name");
    		}
    	}
    }
    