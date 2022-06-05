namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：OrganLevel 主键列：Olv_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class OrganLevel : WeiSha.Data.Entity {
    		
    		protected Int32 _Olv_ID;
    		
    		protected Int32 _Olv_Level;
    		
    		protected String _Olv_Name;
    		
    		protected String _Olv_Intro;
    		
    		protected Boolean _Olv_IsUse;
    		
    		protected String _Olv_Tag;
    		
    		protected Int32 _Olv_Tax;
    		
    		protected Boolean _Olv_IsDefault;
    		
    		protected Int32 _Ps_ID;
    		
    		public Int32 Olv_ID {
    			get {
    				return this._Olv_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Olv_ID, _Olv_ID, value);
    				this._Olv_ID = value;
    			}
    		}
    		
    		public Int32 Olv_Level {
    			get {
    				return this._Olv_Level;
    			}
    			set {
    				this.OnPropertyValueChange(_.Olv_Level, _Olv_Level, value);
    				this._Olv_Level = value;
    			}
    		}
    		
    		public String Olv_Name {
    			get {
    				return this._Olv_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Olv_Name, _Olv_Name, value);
    				this._Olv_Name = value;
    			}
    		}
    		
    		public String Olv_Intro {
    			get {
    				return this._Olv_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Olv_Intro, _Olv_Intro, value);
    				this._Olv_Intro = value;
    			}
    		}
    		
    		public Boolean Olv_IsUse {
    			get {
    				return this._Olv_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Olv_IsUse, _Olv_IsUse, value);
    				this._Olv_IsUse = value;
    			}
    		}
    		
    		public String Olv_Tag {
    			get {
    				return this._Olv_Tag;
    			}
    			set {
    				this.OnPropertyValueChange(_.Olv_Tag, _Olv_Tag, value);
    				this._Olv_Tag = value;
    			}
    		}
    		
    		public Int32 Olv_Tax {
    			get {
    				return this._Olv_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Olv_Tax, _Olv_Tax, value);
    				this._Olv_Tax = value;
    			}
    		}
    		
    		public Boolean Olv_IsDefault {
    			get {
    				return this._Olv_IsDefault;
    			}
    			set {
    				this.OnPropertyValueChange(_.Olv_IsDefault, _Olv_IsDefault, value);
    				this._Olv_IsDefault = value;
    			}
    		}
    		
    		public Int32 Ps_ID {
    			get {
    				return this._Ps_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ps_ID, _Ps_ID, value);
    				this._Ps_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<OrganLevel>("OrganLevel");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Olv_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Olv_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Olv_ID,
    					_.Olv_Level,
    					_.Olv_Name,
    					_.Olv_Intro,
    					_.Olv_IsUse,
    					_.Olv_Tag,
    					_.Olv_Tax,
    					_.Olv_IsDefault,
    					_.Ps_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Olv_ID,
    					this._Olv_Level,
    					this._Olv_Name,
    					this._Olv_Intro,
    					this._Olv_IsUse,
    					this._Olv_Tag,
    					this._Olv_Tax,
    					this._Olv_IsDefault,
    					this._Ps_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Olv_ID))) {
    				this._Olv_ID = reader.GetInt32(_.Olv_ID);
    			}
    			if ((false == reader.IsDBNull(_.Olv_Level))) {
    				this._Olv_Level = reader.GetInt32(_.Olv_Level);
    			}
    			if ((false == reader.IsDBNull(_.Olv_Name))) {
    				this._Olv_Name = reader.GetString(_.Olv_Name);
    			}
    			if ((false == reader.IsDBNull(_.Olv_Intro))) {
    				this._Olv_Intro = reader.GetString(_.Olv_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Olv_IsUse))) {
    				this._Olv_IsUse = reader.GetBoolean(_.Olv_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Olv_Tag))) {
    				this._Olv_Tag = reader.GetString(_.Olv_Tag);
    			}
    			if ((false == reader.IsDBNull(_.Olv_Tax))) {
    				this._Olv_Tax = reader.GetInt32(_.Olv_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Olv_IsDefault))) {
    				this._Olv_IsDefault = reader.GetBoolean(_.Olv_IsDefault);
    			}
    			if ((false == reader.IsDBNull(_.Ps_ID))) {
    				this._Ps_ID = reader.GetInt32(_.Ps_ID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(OrganLevel).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<OrganLevel>();
    			
    			/// <summary>
    			/// 字段名：Olv_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Olv_ID = new WeiSha.Data.Field<OrganLevel>("Olv_ID");
    			
    			/// <summary>
    			/// 字段名：Olv_Level - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Olv_Level = new WeiSha.Data.Field<OrganLevel>("Olv_Level");
    			
    			/// <summary>
    			/// 字段名：Olv_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Olv_Name = new WeiSha.Data.Field<OrganLevel>("Olv_Name");
    			
    			/// <summary>
    			/// 字段名：Olv_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Olv_Intro = new WeiSha.Data.Field<OrganLevel>("Olv_Intro");
    			
    			/// <summary>
    			/// 字段名：Olv_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Olv_IsUse = new WeiSha.Data.Field<OrganLevel>("Olv_IsUse");
    			
    			/// <summary>
    			/// 字段名：Olv_Tag - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Olv_Tag = new WeiSha.Data.Field<OrganLevel>("Olv_Tag");
    			
    			/// <summary>
    			/// 字段名：Olv_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Olv_Tax = new WeiSha.Data.Field<OrganLevel>("Olv_Tax");
    			
    			/// <summary>
    			/// 字段名：Olv_IsDefault - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Olv_IsDefault = new WeiSha.Data.Field<OrganLevel>("Olv_IsDefault");
    			
    			/// <summary>
    			/// 字段名：Ps_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ps_ID = new WeiSha.Data.Field<OrganLevel>("Ps_ID");
    		}
    	}
    }
    