namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：NewsNote 主键列：Nn_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class NewsNote : WeiSha.Data.Entity {
    		
    		protected Int32 _Nn_Id;
    		
    		protected Int32? _Art_Id;
    		
    		protected String _Nn_Name;
    		
    		protected String _Nn_Title;
    		
    		protected String _Nn_Details;
    		
    		protected DateTime? _Nn_CrtTime;
    		
    		protected String _Nn_IP;
    		
    		protected Boolean _Nn_IsShow;
    		
    		protected String _Nn_Email;
    		
    		protected String _Nn_Province;
    		
    		protected String _Nn_City;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Nn_Id {
    			get {
    				return this._Nn_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nn_Id, _Nn_Id, value);
    				this._Nn_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Art_Id {
    			get {
    				return this._Art_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Id, _Art_Id, value);
    				this._Art_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nn_Name {
    			get {
    				return this._Nn_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nn_Name, _Nn_Name, value);
    				this._Nn_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nn_Title {
    			get {
    				return this._Nn_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nn_Title, _Nn_Title, value);
    				this._Nn_Title = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nn_Details {
    			get {
    				return this._Nn_Details;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nn_Details, _Nn_Details, value);
    				this._Nn_Details = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Nn_CrtTime {
    			get {
    				return this._Nn_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nn_CrtTime, _Nn_CrtTime, value);
    				this._Nn_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nn_IP {
    			get {
    				return this._Nn_IP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nn_IP, _Nn_IP, value);
    				this._Nn_IP = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Nn_IsShow {
    			get {
    				return this._Nn_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nn_IsShow, _Nn_IsShow, value);
    				this._Nn_IsShow = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nn_Email {
    			get {
    				return this._Nn_Email;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nn_Email, _Nn_Email, value);
    				this._Nn_Email = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nn_Province {
    			get {
    				return this._Nn_Province;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nn_Province, _Nn_Province, value);
    				this._Nn_Province = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Nn_City {
    			get {
    				return this._Nn_City;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nn_City, _Nn_City, value);
    				this._Nn_City = value;
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
    			return new WeiSha.Data.Table<NewsNote>("NewsNote");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Nn_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Nn_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Nn_Id,
    					_.Art_Id,
    					_.Nn_Name,
    					_.Nn_Title,
    					_.Nn_Details,
    					_.Nn_CrtTime,
    					_.Nn_IP,
    					_.Nn_IsShow,
    					_.Nn_Email,
    					_.Nn_Province,
    					_.Nn_City,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Nn_Id,
    					this._Art_Id,
    					this._Nn_Name,
    					this._Nn_Title,
    					this._Nn_Details,
    					this._Nn_CrtTime,
    					this._Nn_IP,
    					this._Nn_IsShow,
    					this._Nn_Email,
    					this._Nn_Province,
    					this._Nn_City,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Nn_Id))) {
    				this._Nn_Id = reader.GetInt32(_.Nn_Id);
    			}
    			if ((false == reader.IsDBNull(_.Art_Id))) {
    				this._Art_Id = reader.GetInt32(_.Art_Id);
    			}
    			if ((false == reader.IsDBNull(_.Nn_Name))) {
    				this._Nn_Name = reader.GetString(_.Nn_Name);
    			}
    			if ((false == reader.IsDBNull(_.Nn_Title))) {
    				this._Nn_Title = reader.GetString(_.Nn_Title);
    			}
    			if ((false == reader.IsDBNull(_.Nn_Details))) {
    				this._Nn_Details = reader.GetString(_.Nn_Details);
    			}
    			if ((false == reader.IsDBNull(_.Nn_CrtTime))) {
    				this._Nn_CrtTime = reader.GetDateTime(_.Nn_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Nn_IP))) {
    				this._Nn_IP = reader.GetString(_.Nn_IP);
    			}
    			if ((false == reader.IsDBNull(_.Nn_IsShow))) {
    				this._Nn_IsShow = reader.GetBoolean(_.Nn_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Nn_Email))) {
    				this._Nn_Email = reader.GetString(_.Nn_Email);
    			}
    			if ((false == reader.IsDBNull(_.Nn_Province))) {
    				this._Nn_Province = reader.GetString(_.Nn_Province);
    			}
    			if ((false == reader.IsDBNull(_.Nn_City))) {
    				this._Nn_City = reader.GetString(_.Nn_City);
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
    			if ((false == typeof(NewsNote).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<NewsNote>();
    			
    			/// <summary>
    			/// -1 - 字段名：Nn_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Nn_Id = new WeiSha.Data.Field<NewsNote>("Nn_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Art_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Art_Id = new WeiSha.Data.Field<NewsNote>("Art_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Nn_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nn_Name = new WeiSha.Data.Field<NewsNote>("Nn_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Nn_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nn_Title = new WeiSha.Data.Field<NewsNote>("Nn_Title");
    			
    			/// <summary>
    			/// -1 - 字段名：Nn_Details - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nn_Details = new WeiSha.Data.Field<NewsNote>("Nn_Details");
    			
    			/// <summary>
    			/// -1 - 字段名：Nn_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Nn_CrtTime = new WeiSha.Data.Field<NewsNote>("Nn_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Nn_IP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nn_IP = new WeiSha.Data.Field<NewsNote>("Nn_IP");
    			
    			/// <summary>
    			/// -1 - 字段名：Nn_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Nn_IsShow = new WeiSha.Data.Field<NewsNote>("Nn_IsShow");
    			
    			/// <summary>
    			/// -1 - 字段名：Nn_Email - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nn_Email = new WeiSha.Data.Field<NewsNote>("Nn_Email");
    			
    			/// <summary>
    			/// -1 - 字段名：Nn_Province - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nn_Province = new WeiSha.Data.Field<NewsNote>("Nn_Province");
    			
    			/// <summary>
    			/// -1 - 字段名：Nn_City - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Nn_City = new WeiSha.Data.Field<NewsNote>("Nn_City");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<NewsNote>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<NewsNote>("Org_Name");
    		}
    	}
    }
    