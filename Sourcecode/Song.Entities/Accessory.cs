namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Accessory 主键列：As_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Accessory : WeiSha.Data.Entity {
    		
    		protected Int32 _As_Id;
    		
    		protected String _As_Name;
    		
    		protected String _As_FileName;
    		
    		protected String _As_Type;
    		
    		protected DateTime _As_CrtTime;
    		
    		protected Int32 _As_Size;
    		
    		protected String _As_Uid;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Int32 _As_Width;
    		
    		protected Int32 _As_Height;
    		
    		protected String _As_Extension;
    		
    		protected Int32 _As_Duration;
    		
    		protected Boolean _As_IsOuter;
    		
    		protected Boolean _As_IsOther;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 As_Id {
    			get {
    				return this._As_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.As_Id, _As_Id, value);
    				this._As_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String As_Name {
    			get {
    				return this._As_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.As_Name, _As_Name, value);
    				this._As_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String As_FileName {
    			get {
    				return this._As_FileName;
    			}
    			set {
    				this.OnPropertyValueChange(_.As_FileName, _As_FileName, value);
    				this._As_FileName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String As_Type {
    			get {
    				return this._As_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.As_Type, _As_Type, value);
    				this._As_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime As_CrtTime {
    			get {
    				return this._As_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.As_CrtTime, _As_CrtTime, value);
    				this._As_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 As_Size {
    			get {
    				return this._As_Size;
    			}
    			set {
    				this.OnPropertyValueChange(_.As_Size, _As_Size, value);
    				this._As_Size = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String As_Uid {
    			get {
    				return this._As_Uid;
    			}
    			set {
    				this.OnPropertyValueChange(_.As_Uid, _As_Uid, value);
    				this._As_Uid = value;
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
    		
    		public Int32 As_Width {
    			get {
    				return this._As_Width;
    			}
    			set {
    				this.OnPropertyValueChange(_.As_Width, _As_Width, value);
    				this._As_Width = value;
    			}
    		}
    		
    		public Int32 As_Height {
    			get {
    				return this._As_Height;
    			}
    			set {
    				this.OnPropertyValueChange(_.As_Height, _As_Height, value);
    				this._As_Height = value;
    			}
    		}
    		
    		public String As_Extension {
    			get {
    				return this._As_Extension;
    			}
    			set {
    				this.OnPropertyValueChange(_.As_Extension, _As_Extension, value);
    				this._As_Extension = value;
    			}
    		}
    		
    		public Int32 As_Duration {
    			get {
    				return this._As_Duration;
    			}
    			set {
    				this.OnPropertyValueChange(_.As_Duration, _As_Duration, value);
    				this._As_Duration = value;
    			}
    		}
    		
    		public Boolean As_IsOuter {
    			get {
    				return this._As_IsOuter;
    			}
    			set {
    				this.OnPropertyValueChange(_.As_IsOuter, _As_IsOuter, value);
    				this._As_IsOuter = value;
    			}
    		}
    		
    		public Boolean As_IsOther {
    			get {
    				return this._As_IsOther;
    			}
    			set {
    				this.OnPropertyValueChange(_.As_IsOther, _As_IsOther, value);
    				this._As_IsOther = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Accessory>("Accessory");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.As_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.As_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.As_Id,
    					_.As_Name,
    					_.As_FileName,
    					_.As_Type,
    					_.As_CrtTime,
    					_.As_Size,
    					_.As_Uid,
    					_.Org_ID,
    					_.Org_Name,
    					_.As_Width,
    					_.As_Height,
    					_.As_Extension,
    					_.As_Duration,
    					_.As_IsOuter,
    					_.As_IsOther};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._As_Id,
    					this._As_Name,
    					this._As_FileName,
    					this._As_Type,
    					this._As_CrtTime,
    					this._As_Size,
    					this._As_Uid,
    					this._Org_ID,
    					this._Org_Name,
    					this._As_Width,
    					this._As_Height,
    					this._As_Extension,
    					this._As_Duration,
    					this._As_IsOuter,
    					this._As_IsOther};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.As_Id))) {
    				this._As_Id = reader.GetInt32(_.As_Id);
    			}
    			if ((false == reader.IsDBNull(_.As_Name))) {
    				this._As_Name = reader.GetString(_.As_Name);
    			}
    			if ((false == reader.IsDBNull(_.As_FileName))) {
    				this._As_FileName = reader.GetString(_.As_FileName);
    			}
    			if ((false == reader.IsDBNull(_.As_Type))) {
    				this._As_Type = reader.GetString(_.As_Type);
    			}
    			if ((false == reader.IsDBNull(_.As_CrtTime))) {
    				this._As_CrtTime = reader.GetDateTime(_.As_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.As_Size))) {
    				this._As_Size = reader.GetInt32(_.As_Size);
    			}
    			if ((false == reader.IsDBNull(_.As_Uid))) {
    				this._As_Uid = reader.GetString(_.As_Uid);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.As_Width))) {
    				this._As_Width = reader.GetInt32(_.As_Width);
    			}
    			if ((false == reader.IsDBNull(_.As_Height))) {
    				this._As_Height = reader.GetInt32(_.As_Height);
    			}
    			if ((false == reader.IsDBNull(_.As_Extension))) {
    				this._As_Extension = reader.GetString(_.As_Extension);
    			}
    			if ((false == reader.IsDBNull(_.As_Duration))) {
    				this._As_Duration = reader.GetInt32(_.As_Duration);
    			}
    			if ((false == reader.IsDBNull(_.As_IsOuter))) {
    				this._As_IsOuter = reader.GetBoolean(_.As_IsOuter);
    			}
    			if ((false == reader.IsDBNull(_.As_IsOther))) {
    				this._As_IsOther = reader.GetBoolean(_.As_IsOther);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Accessory).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Accessory>();
    			
    			/// <summary>
    			/// -1 - 字段名：As_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field As_Id = new WeiSha.Data.Field<Accessory>("As_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：As_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field As_Name = new WeiSha.Data.Field<Accessory>("As_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：As_FileName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field As_FileName = new WeiSha.Data.Field<Accessory>("As_FileName");
    			
    			/// <summary>
    			/// -1 - 字段名：As_Type - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field As_Type = new WeiSha.Data.Field<Accessory>("As_Type");
    			
    			/// <summary>
    			/// -1 - 字段名：As_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field As_CrtTime = new WeiSha.Data.Field<Accessory>("As_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：As_Size - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field As_Size = new WeiSha.Data.Field<Accessory>("As_Size");
    			
    			/// <summary>
    			/// -1 - 字段名：As_Uid - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field As_Uid = new WeiSha.Data.Field<Accessory>("As_Uid");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Accessory>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Accessory>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：As_Width - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field As_Width = new WeiSha.Data.Field<Accessory>("As_Width");
    			
    			/// <summary>
    			/// 字段名：As_Height - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field As_Height = new WeiSha.Data.Field<Accessory>("As_Height");
    			
    			/// <summary>
    			/// 字段名：As_Extension - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field As_Extension = new WeiSha.Data.Field<Accessory>("As_Extension");
    			
    			/// <summary>
    			/// 字段名：As_Duration - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field As_Duration = new WeiSha.Data.Field<Accessory>("As_Duration");
    			
    			/// <summary>
    			/// 字段名：As_IsOuter - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field As_IsOuter = new WeiSha.Data.Field<Accessory>("As_IsOuter");
    			
    			/// <summary>
    			/// 字段名：As_IsOther - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field As_IsOther = new WeiSha.Data.Field<Accessory>("As_IsOther");
    		}
    	}
    }
    