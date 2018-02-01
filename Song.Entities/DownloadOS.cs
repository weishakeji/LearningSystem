namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：DownloadOS 主键列：Dos_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class DownloadOS : WeiSha.Data.Entity {
    		
    		protected Int32 _Dos_Id;
    		
    		protected String _Dos_Name;
    		
    		protected String _Dos_Intro;
    		
    		protected Boolean _Dos_IsUse;
    		
    		protected Int32? _Dos_Tax;
    		
    		protected Int32? _Org_Id;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Dos_Id {
    			get {
    				return this._Dos_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dos_Id, _Dos_Id, value);
    				this._Dos_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dos_Name {
    			get {
    				return this._Dos_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dos_Name, _Dos_Name, value);
    				this._Dos_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dos_Intro {
    			get {
    				return this._Dos_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dos_Intro, _Dos_Intro, value);
    				this._Dos_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Dos_IsUse {
    			get {
    				return this._Dos_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dos_IsUse, _Dos_IsUse, value);
    				this._Dos_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Dos_Tax {
    			get {
    				return this._Dos_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dos_Tax, _Dos_Tax, value);
    				this._Dos_Tax = value;
    			}
    		}
    		
    		public Int32? Org_Id {
    			get {
    				return this._Org_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Id, _Org_Id, value);
    				this._Org_Id = value;
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
    			return new WeiSha.Data.Table<DownloadOS>("DownloadOS");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Dos_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Dos_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Dos_Id,
    					_.Dos_Name,
    					_.Dos_Intro,
    					_.Dos_IsUse,
    					_.Dos_Tax,
    					_.Org_Id,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Dos_Id,
    					this._Dos_Name,
    					this._Dos_Intro,
    					this._Dos_IsUse,
    					this._Dos_Tax,
    					this._Org_Id,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Dos_Id))) {
    				this._Dos_Id = reader.GetInt32(_.Dos_Id);
    			}
    			if ((false == reader.IsDBNull(_.Dos_Name))) {
    				this._Dos_Name = reader.GetString(_.Dos_Name);
    			}
    			if ((false == reader.IsDBNull(_.Dos_Intro))) {
    				this._Dos_Intro = reader.GetString(_.Dos_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Dos_IsUse))) {
    				this._Dos_IsUse = reader.GetBoolean(_.Dos_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Dos_Tax))) {
    				this._Dos_Tax = reader.GetInt32(_.Dos_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Org_Id))) {
    				this._Org_Id = reader.GetInt32(_.Org_Id);
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
    			if ((false == typeof(DownloadOS).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<DownloadOS>();
    			
    			/// <summary>
    			/// -1 - 字段名：Dos_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dos_Id = new WeiSha.Data.Field<DownloadOS>("Dos_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Dos_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dos_Name = new WeiSha.Data.Field<DownloadOS>("Dos_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Dos_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dos_Intro = new WeiSha.Data.Field<DownloadOS>("Dos_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Dos_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Dos_IsUse = new WeiSha.Data.Field<DownloadOS>("Dos_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Dos_Tax - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Dos_Tax = new WeiSha.Data.Field<DownloadOS>("Dos_Tax");
    			
    			/// <summary>
    			/// 字段名：Org_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Org_Id = new WeiSha.Data.Field<DownloadOS>("Org_Id");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<DownloadOS>("Org_Name");
    		}
    	}
    }
    