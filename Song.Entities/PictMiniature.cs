namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：PictMiniature 主键列：Pmin_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class PictMiniature : WeiSha.Data.Entity {
    		
    		protected Int32 _Pmin_Id;
    		
    		protected Int32? _Pmin_Tax;
    		
    		protected Int32? _Pmin_Width;
    		
    		protected Int32? _Pmin_Height;
    		
    		protected String _Pmin_Unit;
    		
    		protected Boolean _Pmin_IsRestrain;
    		
    		protected String _Pmin_RestrainObj;
    		
    		protected Boolean _Pmin_IsUse;
    		
    		protected String _Pmin_File;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Pmin_Id {
    			get {
    				return this._Pmin_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pmin_Id, _Pmin_Id, value);
    				this._Pmin_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pmin_Tax {
    			get {
    				return this._Pmin_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pmin_Tax, _Pmin_Tax, value);
    				this._Pmin_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pmin_Width {
    			get {
    				return this._Pmin_Width;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pmin_Width, _Pmin_Width, value);
    				this._Pmin_Width = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pmin_Height {
    			get {
    				return this._Pmin_Height;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pmin_Height, _Pmin_Height, value);
    				this._Pmin_Height = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pmin_Unit {
    			get {
    				return this._Pmin_Unit;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pmin_Unit, _Pmin_Unit, value);
    				this._Pmin_Unit = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pmin_IsRestrain {
    			get {
    				return this._Pmin_IsRestrain;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pmin_IsRestrain, _Pmin_IsRestrain, value);
    				this._Pmin_IsRestrain = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pmin_RestrainObj {
    			get {
    				return this._Pmin_RestrainObj;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pmin_RestrainObj, _Pmin_RestrainObj, value);
    				this._Pmin_RestrainObj = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pmin_IsUse {
    			get {
    				return this._Pmin_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pmin_IsUse, _Pmin_IsUse, value);
    				this._Pmin_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pmin_File {
    			get {
    				return this._Pmin_File;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pmin_File, _Pmin_File, value);
    				this._Pmin_File = value;
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
    			return new WeiSha.Data.Table<PictMiniature>("PictMiniature");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Pmin_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pmin_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pmin_Id,
    					_.Pmin_Tax,
    					_.Pmin_Width,
    					_.Pmin_Height,
    					_.Pmin_Unit,
    					_.Pmin_IsRestrain,
    					_.Pmin_RestrainObj,
    					_.Pmin_IsUse,
    					_.Pmin_File,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Pmin_Id,
    					this._Pmin_Tax,
    					this._Pmin_Width,
    					this._Pmin_Height,
    					this._Pmin_Unit,
    					this._Pmin_IsRestrain,
    					this._Pmin_RestrainObj,
    					this._Pmin_IsUse,
    					this._Pmin_File,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Pmin_Id))) {
    				this._Pmin_Id = reader.GetInt32(_.Pmin_Id);
    			}
    			if ((false == reader.IsDBNull(_.Pmin_Tax))) {
    				this._Pmin_Tax = reader.GetInt32(_.Pmin_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Pmin_Width))) {
    				this._Pmin_Width = reader.GetInt32(_.Pmin_Width);
    			}
    			if ((false == reader.IsDBNull(_.Pmin_Height))) {
    				this._Pmin_Height = reader.GetInt32(_.Pmin_Height);
    			}
    			if ((false == reader.IsDBNull(_.Pmin_Unit))) {
    				this._Pmin_Unit = reader.GetString(_.Pmin_Unit);
    			}
    			if ((false == reader.IsDBNull(_.Pmin_IsRestrain))) {
    				this._Pmin_IsRestrain = reader.GetBoolean(_.Pmin_IsRestrain);
    			}
    			if ((false == reader.IsDBNull(_.Pmin_RestrainObj))) {
    				this._Pmin_RestrainObj = reader.GetString(_.Pmin_RestrainObj);
    			}
    			if ((false == reader.IsDBNull(_.Pmin_IsUse))) {
    				this._Pmin_IsUse = reader.GetBoolean(_.Pmin_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Pmin_File))) {
    				this._Pmin_File = reader.GetString(_.Pmin_File);
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
    			if ((false == typeof(PictMiniature).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<PictMiniature>();
    			
    			/// <summary>
    			/// -1 - 字段名：Pmin_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pmin_Id = new WeiSha.Data.Field<PictMiniature>("Pmin_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Pmin_Tax - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pmin_Tax = new WeiSha.Data.Field<PictMiniature>("Pmin_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Pmin_Width - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pmin_Width = new WeiSha.Data.Field<PictMiniature>("Pmin_Width");
    			
    			/// <summary>
    			/// -1 - 字段名：Pmin_Height - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pmin_Height = new WeiSha.Data.Field<PictMiniature>("Pmin_Height");
    			
    			/// <summary>
    			/// -1 - 字段名：Pmin_Unit - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pmin_Unit = new WeiSha.Data.Field<PictMiniature>("Pmin_Unit");
    			
    			/// <summary>
    			/// -1 - 字段名：Pmin_IsRestrain - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pmin_IsRestrain = new WeiSha.Data.Field<PictMiniature>("Pmin_IsRestrain");
    			
    			/// <summary>
    			/// -1 - 字段名：Pmin_RestrainObj - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pmin_RestrainObj = new WeiSha.Data.Field<PictMiniature>("Pmin_RestrainObj");
    			
    			/// <summary>
    			/// -1 - 字段名：Pmin_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pmin_IsUse = new WeiSha.Data.Field<PictMiniature>("Pmin_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Pmin_File - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pmin_File = new WeiSha.Data.Field<PictMiniature>("Pmin_File");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<PictMiniature>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<PictMiniature>("Org_Name");
    		}
    	}
    }
    