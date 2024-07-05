namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Position 主键列：Posi_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Position : WeiSha.Data.Entity {
    		
    		protected Int32 _Posi_Id;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected String _Posi_Intro;
    		
    		protected Boolean _Posi_IsAdmin;
    		
    		protected Boolean _Posi_IsUse;
    		
    		protected String _Posi_Name;
    		
    		protected Int32 _Posi_Tax;
    		
    		public Int32 Posi_Id {
    			get {
    				return this._Posi_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Posi_Id, _Posi_Id, value);
    				this._Posi_Id = value;
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
    		
    		public String Posi_Intro {
    			get {
    				return this._Posi_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Posi_Intro, _Posi_Intro, value);
    				this._Posi_Intro = value;
    			}
    		}
    		
    		public Boolean Posi_IsAdmin {
    			get {
    				return this._Posi_IsAdmin;
    			}
    			set {
    				this.OnPropertyValueChange(_.Posi_IsAdmin, _Posi_IsAdmin, value);
    				this._Posi_IsAdmin = value;
    			}
    		}
    		
    		public Boolean Posi_IsUse {
    			get {
    				return this._Posi_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Posi_IsUse, _Posi_IsUse, value);
    				this._Posi_IsUse = value;
    			}
    		}
    		
    		public String Posi_Name {
    			get {
    				return this._Posi_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Posi_Name, _Posi_Name, value);
    				this._Posi_Name = value;
    			}
    		}
    		
    		public Int32 Posi_Tax {
    			get {
    				return this._Posi_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Posi_Tax, _Posi_Tax, value);
    				this._Posi_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Position>("Position");
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Posi_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Posi_Id,
    					_.Org_ID,
    					_.Org_Name,
    					_.Posi_Intro,
    					_.Posi_IsAdmin,
    					_.Posi_IsUse,
    					_.Posi_Name,
    					_.Posi_Tax};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Posi_Id,
    					this._Org_ID,
    					this._Org_Name,
    					this._Posi_Intro,
    					this._Posi_IsAdmin,
    					this._Posi_IsUse,
    					this._Posi_Name,
    					this._Posi_Tax};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Posi_Id))) {
    				this._Posi_Id = reader.GetInt32(_.Posi_Id);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Posi_Intro))) {
    				this._Posi_Intro = reader.GetString(_.Posi_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Posi_IsAdmin))) {
    				this._Posi_IsAdmin = reader.GetBoolean(_.Posi_IsAdmin);
    			}
    			if ((false == reader.IsDBNull(_.Posi_IsUse))) {
    				this._Posi_IsUse = reader.GetBoolean(_.Posi_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Posi_Name))) {
    				this._Posi_Name = reader.GetString(_.Posi_Name);
    			}
    			if ((false == reader.IsDBNull(_.Posi_Tax))) {
    				this._Posi_Tax = reader.GetInt32(_.Posi_Tax);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Position).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Position>();
    			
    			/// <summary>
    			/// 字段名：Posi_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Posi_Id = new WeiSha.Data.Field<Position>("Posi_Id");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Position>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Position>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Posi_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Posi_Intro = new WeiSha.Data.Field<Position>("Posi_Intro");
    			
    			/// <summary>
    			/// 字段名：Posi_IsAdmin - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Posi_IsAdmin = new WeiSha.Data.Field<Position>("Posi_IsAdmin");
    			
    			/// <summary>
    			/// 字段名：Posi_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Posi_IsUse = new WeiSha.Data.Field<Position>("Posi_IsUse");
    			
    			/// <summary>
    			/// 字段名：Posi_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Posi_Name = new WeiSha.Data.Field<Position>("Posi_Name");
    			
    			/// <summary>
    			/// 字段名：Posi_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Posi_Tax = new WeiSha.Data.Field<Position>("Posi_Tax");
    		}
    	}
    }
    