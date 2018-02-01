namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：GuideColumns 主键列：Gc_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class GuideColumns : WeiSha.Data.Entity {
    		
    		protected Int32 _Gc_ID;
    		
    		protected Int32 _Gc_PID;
    		
    		protected Int32 _Cou_ID;
    		
    		protected String _Cou_Name;
    		
    		protected String _Gc_ByName;
    		
    		protected String _Gc_Title;
    		
    		protected String _Gc_Keywords;
    		
    		protected String _Gc_Descr;
    		
    		protected String _Gc_Intro;
    		
    		protected String _Gc_Type;
    		
    		protected Int32 _Gc_Tax;
    		
    		protected Boolean _Gc_IsUse;
    		
    		protected Boolean _Gc_IsNote;
    		
    		protected DateTime _Gc_CrtTime;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		public Int32 Gc_ID {
    			get {
    				return this._Gc_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gc_ID, _Gc_ID, value);
    				this._Gc_ID = value;
    			}
    		}
    		
    		public Int32 Gc_PID {
    			get {
    				return this._Gc_PID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gc_PID, _Gc_PID, value);
    				this._Gc_PID = value;
    			}
    		}
    		
    		public Int32 Cou_ID {
    			get {
    				return this._Cou_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ID, _Cou_ID, value);
    				this._Cou_ID = value;
    			}
    		}
    		
    		public String Cou_Name {
    			get {
    				return this._Cou_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_Name, _Cou_Name, value);
    				this._Cou_Name = value;
    			}
    		}
    		
    		public String Gc_ByName {
    			get {
    				return this._Gc_ByName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gc_ByName, _Gc_ByName, value);
    				this._Gc_ByName = value;
    			}
    		}
    		
    		public String Gc_Title {
    			get {
    				return this._Gc_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gc_Title, _Gc_Title, value);
    				this._Gc_Title = value;
    			}
    		}
    		
    		public String Gc_Keywords {
    			get {
    				return this._Gc_Keywords;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gc_Keywords, _Gc_Keywords, value);
    				this._Gc_Keywords = value;
    			}
    		}
    		
    		public String Gc_Descr {
    			get {
    				return this._Gc_Descr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gc_Descr, _Gc_Descr, value);
    				this._Gc_Descr = value;
    			}
    		}
    		
    		public String Gc_Intro {
    			get {
    				return this._Gc_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gc_Intro, _Gc_Intro, value);
    				this._Gc_Intro = value;
    			}
    		}
    		
    		public String Gc_Type {
    			get {
    				return this._Gc_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gc_Type, _Gc_Type, value);
    				this._Gc_Type = value;
    			}
    		}
    		
    		public Int32 Gc_Tax {
    			get {
    				return this._Gc_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gc_Tax, _Gc_Tax, value);
    				this._Gc_Tax = value;
    			}
    		}
    		
    		public Boolean Gc_IsUse {
    			get {
    				return this._Gc_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gc_IsUse, _Gc_IsUse, value);
    				this._Gc_IsUse = value;
    			}
    		}
    		
    		public Boolean Gc_IsNote {
    			get {
    				return this._Gc_IsNote;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gc_IsNote, _Gc_IsNote, value);
    				this._Gc_IsNote = value;
    			}
    		}
    		
    		public DateTime Gc_CrtTime {
    			get {
    				return this._Gc_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gc_CrtTime, _Gc_CrtTime, value);
    				this._Gc_CrtTime = value;
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
    			return new WeiSha.Data.Table<GuideColumns>("GuideColumns");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Gc_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Gc_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Gc_ID,
    					_.Gc_PID,
    					_.Cou_ID,
    					_.Cou_Name,
    					_.Gc_ByName,
    					_.Gc_Title,
    					_.Gc_Keywords,
    					_.Gc_Descr,
    					_.Gc_Intro,
    					_.Gc_Type,
    					_.Gc_Tax,
    					_.Gc_IsUse,
    					_.Gc_IsNote,
    					_.Gc_CrtTime,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Gc_ID,
    					this._Gc_PID,
    					this._Cou_ID,
    					this._Cou_Name,
    					this._Gc_ByName,
    					this._Gc_Title,
    					this._Gc_Keywords,
    					this._Gc_Descr,
    					this._Gc_Intro,
    					this._Gc_Type,
    					this._Gc_Tax,
    					this._Gc_IsUse,
    					this._Gc_IsNote,
    					this._Gc_CrtTime,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Gc_ID))) {
    				this._Gc_ID = reader.GetInt32(_.Gc_ID);
    			}
    			if ((false == reader.IsDBNull(_.Gc_PID))) {
    				this._Gc_PID = reader.GetInt32(_.Gc_PID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Name))) {
    				this._Cou_Name = reader.GetString(_.Cou_Name);
    			}
    			if ((false == reader.IsDBNull(_.Gc_ByName))) {
    				this._Gc_ByName = reader.GetString(_.Gc_ByName);
    			}
    			if ((false == reader.IsDBNull(_.Gc_Title))) {
    				this._Gc_Title = reader.GetString(_.Gc_Title);
    			}
    			if ((false == reader.IsDBNull(_.Gc_Keywords))) {
    				this._Gc_Keywords = reader.GetString(_.Gc_Keywords);
    			}
    			if ((false == reader.IsDBNull(_.Gc_Descr))) {
    				this._Gc_Descr = reader.GetString(_.Gc_Descr);
    			}
    			if ((false == reader.IsDBNull(_.Gc_Intro))) {
    				this._Gc_Intro = reader.GetString(_.Gc_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Gc_Type))) {
    				this._Gc_Type = reader.GetString(_.Gc_Type);
    			}
    			if ((false == reader.IsDBNull(_.Gc_Tax))) {
    				this._Gc_Tax = reader.GetInt32(_.Gc_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Gc_IsUse))) {
    				this._Gc_IsUse = reader.GetBoolean(_.Gc_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Gc_IsNote))) {
    				this._Gc_IsNote = reader.GetBoolean(_.Gc_IsNote);
    			}
    			if ((false == reader.IsDBNull(_.Gc_CrtTime))) {
    				this._Gc_CrtTime = reader.GetDateTime(_.Gc_CrtTime);
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
    			if ((false == typeof(GuideColumns).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<GuideColumns>();
    			
    			/// <summary>
    			/// 字段名：Gc_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Gc_ID = new WeiSha.Data.Field<GuideColumns>("Gc_ID");
    			
    			/// <summary>
    			/// 字段名：Gc_PID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Gc_PID = new WeiSha.Data.Field<GuideColumns>("Gc_PID");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<GuideColumns>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Cou_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Name = new WeiSha.Data.Field<GuideColumns>("Cou_Name");
    			
    			/// <summary>
    			/// 字段名：Gc_ByName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gc_ByName = new WeiSha.Data.Field<GuideColumns>("Gc_ByName");
    			
    			/// <summary>
    			/// 字段名：Gc_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gc_Title = new WeiSha.Data.Field<GuideColumns>("Gc_Title");
    			
    			/// <summary>
    			/// 字段名：Gc_Keywords - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gc_Keywords = new WeiSha.Data.Field<GuideColumns>("Gc_Keywords");
    			
    			/// <summary>
    			/// 字段名：Gc_Descr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gc_Descr = new WeiSha.Data.Field<GuideColumns>("Gc_Descr");
    			
    			/// <summary>
    			/// 字段名：Gc_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gc_Intro = new WeiSha.Data.Field<GuideColumns>("Gc_Intro");
    			
    			/// <summary>
    			/// 字段名：Gc_Type - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gc_Type = new WeiSha.Data.Field<GuideColumns>("Gc_Type");
    			
    			/// <summary>
    			/// 字段名：Gc_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Gc_Tax = new WeiSha.Data.Field<GuideColumns>("Gc_Tax");
    			
    			/// <summary>
    			/// 字段名：Gc_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Gc_IsUse = new WeiSha.Data.Field<GuideColumns>("Gc_IsUse");
    			
    			/// <summary>
    			/// 字段名：Gc_IsNote - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Gc_IsNote = new WeiSha.Data.Field<GuideColumns>("Gc_IsNote");
    			
    			/// <summary>
    			/// 字段名：Gc_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Gc_CrtTime = new WeiSha.Data.Field<GuideColumns>("Gc_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<GuideColumns>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<GuideColumns>("Org_Name");
    		}
    	}
    }
    