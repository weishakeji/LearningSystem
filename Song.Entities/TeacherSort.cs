namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：TeacherSort 主键列：Ths_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class TeacherSort : WeiSha.Data.Entity {
    		
    		protected Int32 _Ths_ID;
    		
    		protected String _Ths_Name;
    		
    		protected Int32 _Ths_Tax;
    		
    		protected String _Ths_Intro;
    		
    		protected Boolean _Ths_IsUse;
    		
    		protected Boolean _Ths_IsDefault;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		public Int32 Ths_ID {
    			get {
    				return this._Ths_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ths_ID, _Ths_ID, value);
    				this._Ths_ID = value;
    			}
    		}
    		
    		public String Ths_Name {
    			get {
    				return this._Ths_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ths_Name, _Ths_Name, value);
    				this._Ths_Name = value;
    			}
    		}
    		
    		public Int32 Ths_Tax {
    			get {
    				return this._Ths_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ths_Tax, _Ths_Tax, value);
    				this._Ths_Tax = value;
    			}
    		}
    		
    		public String Ths_Intro {
    			get {
    				return this._Ths_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ths_Intro, _Ths_Intro, value);
    				this._Ths_Intro = value;
    			}
    		}
    		
    		public Boolean Ths_IsUse {
    			get {
    				return this._Ths_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ths_IsUse, _Ths_IsUse, value);
    				this._Ths_IsUse = value;
    			}
    		}
    		
    		public Boolean Ths_IsDefault {
    			get {
    				return this._Ths_IsDefault;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ths_IsDefault, _Ths_IsDefault, value);
    				this._Ths_IsDefault = value;
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
    			return new WeiSha.Data.Table<TeacherSort>("TeacherSort");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Ths_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ths_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ths_ID,
    					_.Ths_Name,
    					_.Ths_Tax,
    					_.Ths_Intro,
    					_.Ths_IsUse,
    					_.Ths_IsDefault,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Ths_ID,
    					this._Ths_Name,
    					this._Ths_Tax,
    					this._Ths_Intro,
    					this._Ths_IsUse,
    					this._Ths_IsDefault,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Ths_ID))) {
    				this._Ths_ID = reader.GetInt32(_.Ths_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ths_Name))) {
    				this._Ths_Name = reader.GetString(_.Ths_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ths_Tax))) {
    				this._Ths_Tax = reader.GetInt32(_.Ths_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Ths_Intro))) {
    				this._Ths_Intro = reader.GetString(_.Ths_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Ths_IsUse))) {
    				this._Ths_IsUse = reader.GetBoolean(_.Ths_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Ths_IsDefault))) {
    				this._Ths_IsDefault = reader.GetBoolean(_.Ths_IsDefault);
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
    			if ((false == typeof(TeacherSort).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<TeacherSort>();
    			
    			/// <summary>
    			/// 字段名：Ths_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ths_ID = new WeiSha.Data.Field<TeacherSort>("Ths_ID");
    			
    			/// <summary>
    			/// 字段名：Ths_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ths_Name = new WeiSha.Data.Field<TeacherSort>("Ths_Name");
    			
    			/// <summary>
    			/// 字段名：Ths_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ths_Tax = new WeiSha.Data.Field<TeacherSort>("Ths_Tax");
    			
    			/// <summary>
    			/// 字段名：Ths_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ths_Intro = new WeiSha.Data.Field<TeacherSort>("Ths_Intro");
    			
    			/// <summary>
    			/// 字段名：Ths_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ths_IsUse = new WeiSha.Data.Field<TeacherSort>("Ths_IsUse");
    			
    			/// <summary>
    			/// 字段名：Ths_IsDefault - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ths_IsDefault = new WeiSha.Data.Field<TeacherSort>("Ths_IsDefault");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<TeacherSort>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<TeacherSort>("Org_Name");
    		}
    	}
    }
    