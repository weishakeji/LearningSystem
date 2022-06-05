namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Download 主键列：Dl_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Download : WeiSha.Data.Entity {
    		
    		protected Int32 _Dl_Id;
    		
    		protected String _Dl_Name;
    		
    		protected String _Dl_Intro;
    		
    		protected String _Dl_Details;
    		
    		protected String _Dl_Version;
    		
    		protected String _Dl_FilePath;
    		
    		protected String _Dl_Logo;
    		
    		protected String _Dl_LogoSmall;
    		
    		protected Boolean _Dl_IsShow;
    		
    		protected String _Dl_Keywords;
    		
    		protected String _Dl_Descr;
    		
    		protected Boolean _Dl_IsRec;
    		
    		protected Boolean _Dl_IsTop;
    		
    		protected Boolean _Dl_IsHot;
    		
    		protected Boolean _Dl_IsDel;
    		
    		protected DateTime? _Dl_CrtTime;
    		
    		protected DateTime? _Dl_UpdateTime;
    		
    		protected Int32? _Dl_Size;
    		
    		protected String _Dl_OS;
    		
    		protected String _Dl_Author;
    		
    		protected Int32? _Dl_LookNumber;
    		
    		protected Int32? _Dl_DownNumber;
    		
    		protected Int32? _Col_Id;
    		
    		protected String _Col_Name;
    		
    		protected Int32? _Acc_Id;
    		
    		protected String _Acc_Name;
    		
    		protected String _Dl_QrCode;
    		
    		protected Boolean _Dl_IsStatic;
    		
    		protected DateTime? _Dl_PushTime;
    		
    		protected String _Dl_Uid;
    		
    		protected Int32? _Dty_Id;
    		
    		protected String _Dty_Type;
    		
    		protected String _Dl_Label;
    		
    		protected String _OtherData;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Dl_Id {
    			get {
    				return this._Dl_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_Id, _Dl_Id, value);
    				this._Dl_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dl_Name {
    			get {
    				return this._Dl_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_Name, _Dl_Name, value);
    				this._Dl_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dl_Intro {
    			get {
    				return this._Dl_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_Intro, _Dl_Intro, value);
    				this._Dl_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dl_Details {
    			get {
    				return this._Dl_Details;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_Details, _Dl_Details, value);
    				this._Dl_Details = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dl_Version {
    			get {
    				return this._Dl_Version;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_Version, _Dl_Version, value);
    				this._Dl_Version = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dl_FilePath {
    			get {
    				return this._Dl_FilePath;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_FilePath, _Dl_FilePath, value);
    				this._Dl_FilePath = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dl_Logo {
    			get {
    				return this._Dl_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_Logo, _Dl_Logo, value);
    				this._Dl_Logo = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dl_LogoSmall {
    			get {
    				return this._Dl_LogoSmall;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_LogoSmall, _Dl_LogoSmall, value);
    				this._Dl_LogoSmall = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Dl_IsShow {
    			get {
    				return this._Dl_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_IsShow, _Dl_IsShow, value);
    				this._Dl_IsShow = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dl_Keywords {
    			get {
    				return this._Dl_Keywords;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_Keywords, _Dl_Keywords, value);
    				this._Dl_Keywords = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dl_Descr {
    			get {
    				return this._Dl_Descr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_Descr, _Dl_Descr, value);
    				this._Dl_Descr = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Dl_IsRec {
    			get {
    				return this._Dl_IsRec;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_IsRec, _Dl_IsRec, value);
    				this._Dl_IsRec = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Dl_IsTop {
    			get {
    				return this._Dl_IsTop;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_IsTop, _Dl_IsTop, value);
    				this._Dl_IsTop = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Dl_IsHot {
    			get {
    				return this._Dl_IsHot;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_IsHot, _Dl_IsHot, value);
    				this._Dl_IsHot = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Dl_IsDel {
    			get {
    				return this._Dl_IsDel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_IsDel, _Dl_IsDel, value);
    				this._Dl_IsDel = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Dl_CrtTime {
    			get {
    				return this._Dl_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_CrtTime, _Dl_CrtTime, value);
    				this._Dl_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Dl_UpdateTime {
    			get {
    				return this._Dl_UpdateTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_UpdateTime, _Dl_UpdateTime, value);
    				this._Dl_UpdateTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Dl_Size {
    			get {
    				return this._Dl_Size;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_Size, _Dl_Size, value);
    				this._Dl_Size = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dl_OS {
    			get {
    				return this._Dl_OS;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_OS, _Dl_OS, value);
    				this._Dl_OS = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dl_Author {
    			get {
    				return this._Dl_Author;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_Author, _Dl_Author, value);
    				this._Dl_Author = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Dl_LookNumber {
    			get {
    				return this._Dl_LookNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_LookNumber, _Dl_LookNumber, value);
    				this._Dl_LookNumber = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Dl_DownNumber {
    			get {
    				return this._Dl_DownNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_DownNumber, _Dl_DownNumber, value);
    				this._Dl_DownNumber = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Col_Id {
    			get {
    				return this._Col_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_Id, _Col_Id, value);
    				this._Col_Id = value;
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
    		public Int32? Acc_Id {
    			get {
    				return this._Acc_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Id, _Acc_Id, value);
    				this._Acc_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Acc_Name {
    			get {
    				return this._Acc_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Name, _Acc_Name, value);
    				this._Acc_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dl_QrCode {
    			get {
    				return this._Dl_QrCode;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_QrCode, _Dl_QrCode, value);
    				this._Dl_QrCode = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Dl_IsStatic {
    			get {
    				return this._Dl_IsStatic;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_IsStatic, _Dl_IsStatic, value);
    				this._Dl_IsStatic = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Dl_PushTime {
    			get {
    				return this._Dl_PushTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_PushTime, _Dl_PushTime, value);
    				this._Dl_PushTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dl_Uid {
    			get {
    				return this._Dl_Uid;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_Uid, _Dl_Uid, value);
    				this._Dl_Uid = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Dty_Id {
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
    		public String Dty_Type {
    			get {
    				return this._Dty_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dty_Type, _Dty_Type, value);
    				this._Dty_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dl_Label {
    			get {
    				return this._Dl_Label;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dl_Label, _Dl_Label, value);
    				this._Dl_Label = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String OtherData {
    			get {
    				return this._OtherData;
    			}
    			set {
    				this.OnPropertyValueChange(_.OtherData, _OtherData, value);
    				this._OtherData = value;
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
    			return new WeiSha.Data.Table<Download>("Download");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Dl_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Dl_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Dl_Id,
    					_.Dl_Name,
    					_.Dl_Intro,
    					_.Dl_Details,
    					_.Dl_Version,
    					_.Dl_FilePath,
    					_.Dl_Logo,
    					_.Dl_LogoSmall,
    					_.Dl_IsShow,
    					_.Dl_Keywords,
    					_.Dl_Descr,
    					_.Dl_IsRec,
    					_.Dl_IsTop,
    					_.Dl_IsHot,
    					_.Dl_IsDel,
    					_.Dl_CrtTime,
    					_.Dl_UpdateTime,
    					_.Dl_Size,
    					_.Dl_OS,
    					_.Dl_Author,
    					_.Dl_LookNumber,
    					_.Dl_DownNumber,
    					_.Col_Id,
    					_.Col_Name,
    					_.Acc_Id,
    					_.Acc_Name,
    					_.Dl_QrCode,
    					_.Dl_IsStatic,
    					_.Dl_PushTime,
    					_.Dl_Uid,
    					_.Dty_Id,
    					_.Dty_Type,
    					_.Dl_Label,
    					_.OtherData,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Dl_Id,
    					this._Dl_Name,
    					this._Dl_Intro,
    					this._Dl_Details,
    					this._Dl_Version,
    					this._Dl_FilePath,
    					this._Dl_Logo,
    					this._Dl_LogoSmall,
    					this._Dl_IsShow,
    					this._Dl_Keywords,
    					this._Dl_Descr,
    					this._Dl_IsRec,
    					this._Dl_IsTop,
    					this._Dl_IsHot,
    					this._Dl_IsDel,
    					this._Dl_CrtTime,
    					this._Dl_UpdateTime,
    					this._Dl_Size,
    					this._Dl_OS,
    					this._Dl_Author,
    					this._Dl_LookNumber,
    					this._Dl_DownNumber,
    					this._Col_Id,
    					this._Col_Name,
    					this._Acc_Id,
    					this._Acc_Name,
    					this._Dl_QrCode,
    					this._Dl_IsStatic,
    					this._Dl_PushTime,
    					this._Dl_Uid,
    					this._Dty_Id,
    					this._Dty_Type,
    					this._Dl_Label,
    					this._OtherData,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Dl_Id))) {
    				this._Dl_Id = reader.GetInt32(_.Dl_Id);
    			}
    			if ((false == reader.IsDBNull(_.Dl_Name))) {
    				this._Dl_Name = reader.GetString(_.Dl_Name);
    			}
    			if ((false == reader.IsDBNull(_.Dl_Intro))) {
    				this._Dl_Intro = reader.GetString(_.Dl_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Dl_Details))) {
    				this._Dl_Details = reader.GetString(_.Dl_Details);
    			}
    			if ((false == reader.IsDBNull(_.Dl_Version))) {
    				this._Dl_Version = reader.GetString(_.Dl_Version);
    			}
    			if ((false == reader.IsDBNull(_.Dl_FilePath))) {
    				this._Dl_FilePath = reader.GetString(_.Dl_FilePath);
    			}
    			if ((false == reader.IsDBNull(_.Dl_Logo))) {
    				this._Dl_Logo = reader.GetString(_.Dl_Logo);
    			}
    			if ((false == reader.IsDBNull(_.Dl_LogoSmall))) {
    				this._Dl_LogoSmall = reader.GetString(_.Dl_LogoSmall);
    			}
    			if ((false == reader.IsDBNull(_.Dl_IsShow))) {
    				this._Dl_IsShow = reader.GetBoolean(_.Dl_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Dl_Keywords))) {
    				this._Dl_Keywords = reader.GetString(_.Dl_Keywords);
    			}
    			if ((false == reader.IsDBNull(_.Dl_Descr))) {
    				this._Dl_Descr = reader.GetString(_.Dl_Descr);
    			}
    			if ((false == reader.IsDBNull(_.Dl_IsRec))) {
    				this._Dl_IsRec = reader.GetBoolean(_.Dl_IsRec);
    			}
    			if ((false == reader.IsDBNull(_.Dl_IsTop))) {
    				this._Dl_IsTop = reader.GetBoolean(_.Dl_IsTop);
    			}
    			if ((false == reader.IsDBNull(_.Dl_IsHot))) {
    				this._Dl_IsHot = reader.GetBoolean(_.Dl_IsHot);
    			}
    			if ((false == reader.IsDBNull(_.Dl_IsDel))) {
    				this._Dl_IsDel = reader.GetBoolean(_.Dl_IsDel);
    			}
    			if ((false == reader.IsDBNull(_.Dl_CrtTime))) {
    				this._Dl_CrtTime = reader.GetDateTime(_.Dl_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Dl_UpdateTime))) {
    				this._Dl_UpdateTime = reader.GetDateTime(_.Dl_UpdateTime);
    			}
    			if ((false == reader.IsDBNull(_.Dl_Size))) {
    				this._Dl_Size = reader.GetInt32(_.Dl_Size);
    			}
    			if ((false == reader.IsDBNull(_.Dl_OS))) {
    				this._Dl_OS = reader.GetString(_.Dl_OS);
    			}
    			if ((false == reader.IsDBNull(_.Dl_Author))) {
    				this._Dl_Author = reader.GetString(_.Dl_Author);
    			}
    			if ((false == reader.IsDBNull(_.Dl_LookNumber))) {
    				this._Dl_LookNumber = reader.GetInt32(_.Dl_LookNumber);
    			}
    			if ((false == reader.IsDBNull(_.Dl_DownNumber))) {
    				this._Dl_DownNumber = reader.GetInt32(_.Dl_DownNumber);
    			}
    			if ((false == reader.IsDBNull(_.Col_Id))) {
    				this._Col_Id = reader.GetInt32(_.Col_Id);
    			}
    			if ((false == reader.IsDBNull(_.Col_Name))) {
    				this._Col_Name = reader.GetString(_.Col_Name);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Name))) {
    				this._Acc_Name = reader.GetString(_.Acc_Name);
    			}
    			if ((false == reader.IsDBNull(_.Dl_QrCode))) {
    				this._Dl_QrCode = reader.GetString(_.Dl_QrCode);
    			}
    			if ((false == reader.IsDBNull(_.Dl_IsStatic))) {
    				this._Dl_IsStatic = reader.GetBoolean(_.Dl_IsStatic);
    			}
    			if ((false == reader.IsDBNull(_.Dl_PushTime))) {
    				this._Dl_PushTime = reader.GetDateTime(_.Dl_PushTime);
    			}
    			if ((false == reader.IsDBNull(_.Dl_Uid))) {
    				this._Dl_Uid = reader.GetString(_.Dl_Uid);
    			}
    			if ((false == reader.IsDBNull(_.Dty_Id))) {
    				this._Dty_Id = reader.GetInt32(_.Dty_Id);
    			}
    			if ((false == reader.IsDBNull(_.Dty_Type))) {
    				this._Dty_Type = reader.GetString(_.Dty_Type);
    			}
    			if ((false == reader.IsDBNull(_.Dl_Label))) {
    				this._Dl_Label = reader.GetString(_.Dl_Label);
    			}
    			if ((false == reader.IsDBNull(_.OtherData))) {
    				this._OtherData = reader.GetString(_.OtherData);
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
    			if ((false == typeof(Download).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Download>();
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dl_Id = new WeiSha.Data.Field<Download>("Dl_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dl_Name = new WeiSha.Data.Field<Download>("Dl_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dl_Intro = new WeiSha.Data.Field<Download>("Dl_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_Details - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dl_Details = new WeiSha.Data.Field<Download>("Dl_Details");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_Version - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dl_Version = new WeiSha.Data.Field<Download>("Dl_Version");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_FilePath - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dl_FilePath = new WeiSha.Data.Field<Download>("Dl_FilePath");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dl_Logo = new WeiSha.Data.Field<Download>("Dl_Logo");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_LogoSmall - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dl_LogoSmall = new WeiSha.Data.Field<Download>("Dl_LogoSmall");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Dl_IsShow = new WeiSha.Data.Field<Download>("Dl_IsShow");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_Keywords - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dl_Keywords = new WeiSha.Data.Field<Download>("Dl_Keywords");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_Descr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dl_Descr = new WeiSha.Data.Field<Download>("Dl_Descr");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_IsRec - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Dl_IsRec = new WeiSha.Data.Field<Download>("Dl_IsRec");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_IsTop - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Dl_IsTop = new WeiSha.Data.Field<Download>("Dl_IsTop");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_IsHot - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Dl_IsHot = new WeiSha.Data.Field<Download>("Dl_IsHot");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_IsDel - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Dl_IsDel = new WeiSha.Data.Field<Download>("Dl_IsDel");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Dl_CrtTime = new WeiSha.Data.Field<Download>("Dl_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_UpdateTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Dl_UpdateTime = new WeiSha.Data.Field<Download>("Dl_UpdateTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_Size - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Dl_Size = new WeiSha.Data.Field<Download>("Dl_Size");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_OS - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dl_OS = new WeiSha.Data.Field<Download>("Dl_OS");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_Author - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dl_Author = new WeiSha.Data.Field<Download>("Dl_Author");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_LookNumber - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Dl_LookNumber = new WeiSha.Data.Field<Download>("Dl_LookNumber");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_DownNumber - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Dl_DownNumber = new WeiSha.Data.Field<Download>("Dl_DownNumber");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Col_Id = new WeiSha.Data.Field<Download>("Col_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Col_Name = new WeiSha.Data.Field<Download>("Col_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<Download>("Acc_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Name = new WeiSha.Data.Field<Download>("Acc_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_QrCode - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dl_QrCode = new WeiSha.Data.Field<Download>("Dl_QrCode");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_IsStatic - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Dl_IsStatic = new WeiSha.Data.Field<Download>("Dl_IsStatic");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_PushTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Dl_PushTime = new WeiSha.Data.Field<Download>("Dl_PushTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_Uid - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dl_Uid = new WeiSha.Data.Field<Download>("Dl_Uid");
    			
    			/// <summary>
    			/// -1 - 字段名：Dty_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Dty_Id = new WeiSha.Data.Field<Download>("Dty_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Dty_Type - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dty_Type = new WeiSha.Data.Field<Download>("Dty_Type");
    			
    			/// <summary>
    			/// -1 - 字段名：Dl_Label - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dl_Label = new WeiSha.Data.Field<Download>("Dl_Label");
    			
    			/// <summary>
    			/// -1 - 字段名：OtherData - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field OtherData = new WeiSha.Data.Field<Download>("OtherData");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Download>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Download>("Org_Name");
    		}
    	}
    }
    