namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：DownloadType 主键列：Dty_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class DownloadType : WeiSha.Data.Entity {
    		
    		protected Int32 _Dty_Id;
    		
    		protected String _Dty_Name;
    		
    		protected String _Dty_Intro;
    		
    		protected String _Dty_Details;
    		
    		protected Boolean _Dty_IsUse;
    		
    		protected Int32? _Dty_Tax;
    		
    		protected Int32? _Org_Id;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Dty_Id {
    			get {
    				return this._Dty_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dty_Id, _Dty_Id, value);
    				this._Dty_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dty_Name {
    			get {
    				return this._Dty_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dty_Name, _Dty_Name, value);
    				this._Dty_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dty_Intro {
    			get {
    				return this._Dty_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dty_Intro, _Dty_Intro, value);
    				this._Dty_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dty_Details {
    			get {
    				return this._Dty_Details;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dty_Details, _Dty_Details, value);
    				this._Dty_Details = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Dty_IsUse {
    			get {
    				return this._Dty_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dty_IsUse, _Dty_IsUse, value);
    				this._Dty_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Dty_Tax {
    			get {
    				return this._Dty_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dty_Tax, _Dty_Tax, value);
    				this._Dty_Tax = value;
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
    			return new WeiSha.Data.Table<DownloadType>("DownloadType");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Dty_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Dty_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Dty_Id,
    					_.Dty_Name,
    					_.Dty_Intro,
    					_.Dty_Details,
    					_.Dty_IsUse,
    					_.Dty_Tax,
    					_.Org_Id,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Dty_Id,
    					this._Dty_Name,
    					this._Dty_Intro,
    					this._Dty_Details,
    					this._Dty_IsUse,
    					this._Dty_Tax,
    					this._Org_Id,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Dty_Id))) {
    				this._Dty_Id = reader.GetInt32(_.Dty_Id);
    			}
    			if ((false == reader.IsDBNull(_.Dty_Name))) {
    				this._Dty_Name = reader.GetString(_.Dty_Name);
    			}
    			if ((false == reader.IsDBNull(_.Dty_Intro))) {
    				this._Dty_Intro = reader.GetString(_.Dty_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Dty_Details))) {
    				this._Dty_Details = reader.GetString(_.Dty_Details);
    			}
    			if ((false == reader.IsDBNull(_.Dty_IsUse))) {
    				this._Dty_IsUse = reader.GetBoolean(_.Dty_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Dty_Tax))) {
    				this._Dty_Tax = reader.GetInt32(_.Dty_Tax);
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
    			if ((false == typeof(DownloadType).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<DownloadType>();
    			
    			/// <summary>
    			/// -1 - 字段名：Dty_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dty_Id = new WeiSha.Data.Field<DownloadType>("Dty_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Dty_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dty_Name = new WeiSha.Data.Field<DownloadType>("Dty_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Dty_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dty_Intro = new WeiSha.Data.Field<DownloadType>("Dty_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Dty_Details - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dty_Details = new WeiSha.Data.Field<DownloadType>("Dty_Details");
    			
    			/// <summary>
    			/// -1 - 字段名：Dty_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Dty_IsUse = new WeiSha.Data.Field<DownloadType>("Dty_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Dty_Tax - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Dty_Tax = new WeiSha.Data.Field<DownloadType>("Dty_Tax");
    			
    			/// <summary>
    			/// 字段名：Org_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Org_Id = new WeiSha.Data.Field<DownloadType>("Org_Id");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<DownloadType>("Org_Name");
    		}
    	}
    }
    