namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：UserGroup 主键列：UGrp_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class UserGroup : WeiSha.Data.Entity {
    		
    		protected Int32 _UGrp_Id;
    		
    		protected String _UGrp_Name;
    		
    		protected Boolean _UGrp_IsUse;
    		
    		protected Int32? _UGrp_Tax;
    		
    		protected String _UGrp_Intro;
    		
    		protected Boolean _UGrp_IsDefault;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 UGrp_Id {
    			get {
    				return this._UGrp_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.UGrp_Id, _UGrp_Id, value);
    				this._UGrp_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String UGrp_Name {
    			get {
    				return this._UGrp_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.UGrp_Name, _UGrp_Name, value);
    				this._UGrp_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean UGrp_IsUse {
    			get {
    				return this._UGrp_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.UGrp_IsUse, _UGrp_IsUse, value);
    				this._UGrp_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? UGrp_Tax {
    			get {
    				return this._UGrp_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.UGrp_Tax, _UGrp_Tax, value);
    				this._UGrp_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String UGrp_Intro {
    			get {
    				return this._UGrp_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.UGrp_Intro, _UGrp_Intro, value);
    				this._UGrp_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean UGrp_IsDefault {
    			get {
    				return this._UGrp_IsDefault;
    			}
    			set {
    				this.OnPropertyValueChange(_.UGrp_IsDefault, _UGrp_IsDefault, value);
    				this._UGrp_IsDefault = value;
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
    			return new WeiSha.Data.Table<UserGroup>("UserGroup");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.UGrp_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.UGrp_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.UGrp_Id,
    					_.UGrp_Name,
    					_.UGrp_IsUse,
    					_.UGrp_Tax,
    					_.UGrp_Intro,
    					_.UGrp_IsDefault,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._UGrp_Id,
    					this._UGrp_Name,
    					this._UGrp_IsUse,
    					this._UGrp_Tax,
    					this._UGrp_Intro,
    					this._UGrp_IsDefault,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.UGrp_Id))) {
    				this._UGrp_Id = reader.GetInt32(_.UGrp_Id);
    			}
    			if ((false == reader.IsDBNull(_.UGrp_Name))) {
    				this._UGrp_Name = reader.GetString(_.UGrp_Name);
    			}
    			if ((false == reader.IsDBNull(_.UGrp_IsUse))) {
    				this._UGrp_IsUse = reader.GetBoolean(_.UGrp_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.UGrp_Tax))) {
    				this._UGrp_Tax = reader.GetInt32(_.UGrp_Tax);
    			}
    			if ((false == reader.IsDBNull(_.UGrp_Intro))) {
    				this._UGrp_Intro = reader.GetString(_.UGrp_Intro);
    			}
    			if ((false == reader.IsDBNull(_.UGrp_IsDefault))) {
    				this._UGrp_IsDefault = reader.GetBoolean(_.UGrp_IsDefault);
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
    			if ((false == typeof(UserGroup).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<UserGroup>();
    			
    			/// <summary>
    			/// -1 - 字段名：UGrp_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field UGrp_Id = new WeiSha.Data.Field<UserGroup>("UGrp_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：UGrp_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field UGrp_Name = new WeiSha.Data.Field<UserGroup>("UGrp_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：UGrp_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field UGrp_IsUse = new WeiSha.Data.Field<UserGroup>("UGrp_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：UGrp_Tax - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field UGrp_Tax = new WeiSha.Data.Field<UserGroup>("UGrp_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：UGrp_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field UGrp_Intro = new WeiSha.Data.Field<UserGroup>("UGrp_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：UGrp_IsDefault - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field UGrp_IsDefault = new WeiSha.Data.Field<UserGroup>("UGrp_IsDefault");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<UserGroup>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<UserGroup>("Org_Name");
    		}
    	}
    }
    