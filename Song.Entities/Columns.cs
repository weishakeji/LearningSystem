namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Columns 主键列：Col_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Columns : WeiSha.Data.Entity {
    		
    		protected Int32 _Col_ID;
    		
    		protected Int32 _Col_PID;
    		
    		protected String _Col_Name;
    		
    		protected String _Col_ByName;
    		
    		protected String _Col_Title;
    		
    		protected String _Col_Keywords;
    		
    		protected String _Col_Descr;
    		
    		protected String _Col_Intro;
    		
    		protected String _Col_Type;
    		
    		protected Int32 _Col_Tax;
    		
    		protected Boolean _Col_IsUse;
    		
    		protected Boolean _Col_IsNote;
    		
    		protected DateTime _Col_CrtTime;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Col_ID {
    			get {
    				return this._Col_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_ID, _Col_ID, value);
    				this._Col_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Col_PID {
    			get {
    				return this._Col_PID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_PID, _Col_PID, value);
    				this._Col_PID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Col_Name {
    			get {
    				return this._Col_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_Name, _Col_Name, value);
    				this._Col_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Col_ByName {
    			get {
    				return this._Col_ByName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_ByName, _Col_ByName, value);
    				this._Col_ByName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Col_Title {
    			get {
    				return this._Col_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_Title, _Col_Title, value);
    				this._Col_Title = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Col_Keywords {
    			get {
    				return this._Col_Keywords;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_Keywords, _Col_Keywords, value);
    				this._Col_Keywords = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Col_Descr {
    			get {
    				return this._Col_Descr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_Descr, _Col_Descr, value);
    				this._Col_Descr = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Col_Intro {
    			get {
    				return this._Col_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_Intro, _Col_Intro, value);
    				this._Col_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Col_Type {
    			get {
    				return this._Col_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_Type, _Col_Type, value);
    				this._Col_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Col_Tax {
    			get {
    				return this._Col_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_Tax, _Col_Tax, value);
    				this._Col_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Col_IsUse {
    			get {
    				return this._Col_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_IsUse, _Col_IsUse, value);
    				this._Col_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Col_IsNote {
    			get {
    				return this._Col_IsNote;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_IsNote, _Col_IsNote, value);
    				this._Col_IsNote = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime Col_CrtTime {
    			get {
    				return this._Col_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_CrtTime, _Col_CrtTime, value);
    				this._Col_CrtTime = value;
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
    			return new WeiSha.Data.Table<Columns>("Columns");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Col_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Col_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Col_ID,
    					_.Col_PID,
    					_.Col_Name,
    					_.Col_ByName,
    					_.Col_Title,
    					_.Col_Keywords,
    					_.Col_Descr,
    					_.Col_Intro,
    					_.Col_Type,
    					_.Col_Tax,
    					_.Col_IsUse,
    					_.Col_IsNote,
    					_.Col_CrtTime,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Col_ID,
    					this._Col_PID,
    					this._Col_Name,
    					this._Col_ByName,
    					this._Col_Title,
    					this._Col_Keywords,
    					this._Col_Descr,
    					this._Col_Intro,
    					this._Col_Type,
    					this._Col_Tax,
    					this._Col_IsUse,
    					this._Col_IsNote,
    					this._Col_CrtTime,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Col_ID))) {
    				this._Col_ID = reader.GetInt32(_.Col_ID);
    			}
    			if ((false == reader.IsDBNull(_.Col_PID))) {
    				this._Col_PID = reader.GetInt32(_.Col_PID);
    			}
    			if ((false == reader.IsDBNull(_.Col_Name))) {
    				this._Col_Name = reader.GetString(_.Col_Name);
    			}
    			if ((false == reader.IsDBNull(_.Col_ByName))) {
    				this._Col_ByName = reader.GetString(_.Col_ByName);
    			}
    			if ((false == reader.IsDBNull(_.Col_Title))) {
    				this._Col_Title = reader.GetString(_.Col_Title);
    			}
    			if ((false == reader.IsDBNull(_.Col_Keywords))) {
    				this._Col_Keywords = reader.GetString(_.Col_Keywords);
    			}
    			if ((false == reader.IsDBNull(_.Col_Descr))) {
    				this._Col_Descr = reader.GetString(_.Col_Descr);
    			}
    			if ((false == reader.IsDBNull(_.Col_Intro))) {
    				this._Col_Intro = reader.GetString(_.Col_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Col_Type))) {
    				this._Col_Type = reader.GetString(_.Col_Type);
    			}
    			if ((false == reader.IsDBNull(_.Col_Tax))) {
    				this._Col_Tax = reader.GetInt32(_.Col_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Col_IsUse))) {
    				this._Col_IsUse = reader.GetBoolean(_.Col_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Col_IsNote))) {
    				this._Col_IsNote = reader.GetBoolean(_.Col_IsNote);
    			}
    			if ((false == reader.IsDBNull(_.Col_CrtTime))) {
    				this._Col_CrtTime = reader.GetDateTime(_.Col_CrtTime);
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
    			if ((false == typeof(Columns).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Columns>();
    			
    			/// <summary>
    			/// -1 - 字段名：Col_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Col_ID = new WeiSha.Data.Field<Columns>("Col_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_PID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Col_PID = new WeiSha.Data.Field<Columns>("Col_PID");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Col_Name = new WeiSha.Data.Field<Columns>("Col_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_ByName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Col_ByName = new WeiSha.Data.Field<Columns>("Col_ByName");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Col_Title = new WeiSha.Data.Field<Columns>("Col_Title");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Keywords - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Col_Keywords = new WeiSha.Data.Field<Columns>("Col_Keywords");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Descr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Col_Descr = new WeiSha.Data.Field<Columns>("Col_Descr");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Col_Intro = new WeiSha.Data.Field<Columns>("Col_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Type - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Col_Type = new WeiSha.Data.Field<Columns>("Col_Type");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Col_Tax = new WeiSha.Data.Field<Columns>("Col_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Col_IsUse = new WeiSha.Data.Field<Columns>("Col_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_IsNote - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Col_IsNote = new WeiSha.Data.Field<Columns>("Col_IsNote");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Col_CrtTime = new WeiSha.Data.Field<Columns>("Col_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Columns>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Columns>("Org_Name");
    		}
    	}
    }
    