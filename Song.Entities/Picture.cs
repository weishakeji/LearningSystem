namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Picture 主键列：Pic_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Picture : WeiSha.Data.Entity {
    		
    		protected Int32 _Pic_Id;
    		
    		protected Int32? _Col_Id;
    		
    		protected String _Col_Name;
    		
    		protected String _Pic_Name;
    		
    		protected String _Pic_Intro;
    		
    		protected String _Pic_FilePath;
    		
    		protected String _Pic_FilePathSmall;
    		
    		protected Boolean _Pic_IsShow;
    		
    		protected String _Pic_Keywords;
    		
    		protected String _Pic_Descr;
    		
    		protected Boolean _Pic_IsRec;
    		
    		protected Boolean _Pic_IsTop;
    		
    		protected Boolean _Pic_IsHot;
    		
    		protected Boolean _Pic_IsDel;
    		
    		protected Boolean _Pic_IsCover;
    		
    		protected DateTime? _Pic_CrtTime;
    		
    		protected Int32? _Pic_Size;
    		
    		protected Int32? _Pic_Width;
    		
    		protected Int32? _Pic_Height;
    		
    		protected Int32? _Pic_Number;
    		
    		protected Int32? _Pic_Tax;
    		
    		protected String _Pic_Uid;
    		
    		protected Int32? _Acc_Id;
    		
    		protected String _Acc_Name;
    		
    		protected Boolean _Pic_IsStatic;
    		
    		protected DateTime? _Pic_PushTime;
    		
    		protected String _Pic_Label;
    		
    		protected String _Pic_Type;
    		
    		protected String _OtherData;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Pic_Id {
    			get {
    				return this._Pic_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_Id, _Pic_Id, value);
    				this._Pic_Id = value;
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
    		public String Pic_Name {
    			get {
    				return this._Pic_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_Name, _Pic_Name, value);
    				this._Pic_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pic_Intro {
    			get {
    				return this._Pic_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_Intro, _Pic_Intro, value);
    				this._Pic_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pic_FilePath {
    			get {
    				return this._Pic_FilePath;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_FilePath, _Pic_FilePath, value);
    				this._Pic_FilePath = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pic_FilePathSmall {
    			get {
    				return this._Pic_FilePathSmall;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_FilePathSmall, _Pic_FilePathSmall, value);
    				this._Pic_FilePathSmall = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pic_IsShow {
    			get {
    				return this._Pic_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_IsShow, _Pic_IsShow, value);
    				this._Pic_IsShow = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pic_Keywords {
    			get {
    				return this._Pic_Keywords;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_Keywords, _Pic_Keywords, value);
    				this._Pic_Keywords = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pic_Descr {
    			get {
    				return this._Pic_Descr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_Descr, _Pic_Descr, value);
    				this._Pic_Descr = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pic_IsRec {
    			get {
    				return this._Pic_IsRec;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_IsRec, _Pic_IsRec, value);
    				this._Pic_IsRec = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pic_IsTop {
    			get {
    				return this._Pic_IsTop;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_IsTop, _Pic_IsTop, value);
    				this._Pic_IsTop = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pic_IsHot {
    			get {
    				return this._Pic_IsHot;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_IsHot, _Pic_IsHot, value);
    				this._Pic_IsHot = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pic_IsDel {
    			get {
    				return this._Pic_IsDel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_IsDel, _Pic_IsDel, value);
    				this._Pic_IsDel = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pic_IsCover {
    			get {
    				return this._Pic_IsCover;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_IsCover, _Pic_IsCover, value);
    				this._Pic_IsCover = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Pic_CrtTime {
    			get {
    				return this._Pic_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_CrtTime, _Pic_CrtTime, value);
    				this._Pic_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pic_Size {
    			get {
    				return this._Pic_Size;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_Size, _Pic_Size, value);
    				this._Pic_Size = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pic_Width {
    			get {
    				return this._Pic_Width;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_Width, _Pic_Width, value);
    				this._Pic_Width = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pic_Height {
    			get {
    				return this._Pic_Height;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_Height, _Pic_Height, value);
    				this._Pic_Height = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pic_Number {
    			get {
    				return this._Pic_Number;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_Number, _Pic_Number, value);
    				this._Pic_Number = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pic_Tax {
    			get {
    				return this._Pic_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_Tax, _Pic_Tax, value);
    				this._Pic_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pic_Uid {
    			get {
    				return this._Pic_Uid;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_Uid, _Pic_Uid, value);
    				this._Pic_Uid = value;
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
    		public Boolean Pic_IsStatic {
    			get {
    				return this._Pic_IsStatic;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_IsStatic, _Pic_IsStatic, value);
    				this._Pic_IsStatic = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Pic_PushTime {
    			get {
    				return this._Pic_PushTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_PushTime, _Pic_PushTime, value);
    				this._Pic_PushTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pic_Label {
    			get {
    				return this._Pic_Label;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_Label, _Pic_Label, value);
    				this._Pic_Label = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pic_Type {
    			get {
    				return this._Pic_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pic_Type, _Pic_Type, value);
    				this._Pic_Type = value;
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
    			return new WeiSha.Data.Table<Picture>("Picture");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Pic_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pic_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pic_Id,
    					_.Col_Id,
    					_.Col_Name,
    					_.Pic_Name,
    					_.Pic_Intro,
    					_.Pic_FilePath,
    					_.Pic_FilePathSmall,
    					_.Pic_IsShow,
    					_.Pic_Keywords,
    					_.Pic_Descr,
    					_.Pic_IsRec,
    					_.Pic_IsTop,
    					_.Pic_IsHot,
    					_.Pic_IsDel,
    					_.Pic_IsCover,
    					_.Pic_CrtTime,
    					_.Pic_Size,
    					_.Pic_Width,
    					_.Pic_Height,
    					_.Pic_Number,
    					_.Pic_Tax,
    					_.Pic_Uid,
    					_.Acc_Id,
    					_.Acc_Name,
    					_.Pic_IsStatic,
    					_.Pic_PushTime,
    					_.Pic_Label,
    					_.Pic_Type,
    					_.OtherData,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Pic_Id,
    					this._Col_Id,
    					this._Col_Name,
    					this._Pic_Name,
    					this._Pic_Intro,
    					this._Pic_FilePath,
    					this._Pic_FilePathSmall,
    					this._Pic_IsShow,
    					this._Pic_Keywords,
    					this._Pic_Descr,
    					this._Pic_IsRec,
    					this._Pic_IsTop,
    					this._Pic_IsHot,
    					this._Pic_IsDel,
    					this._Pic_IsCover,
    					this._Pic_CrtTime,
    					this._Pic_Size,
    					this._Pic_Width,
    					this._Pic_Height,
    					this._Pic_Number,
    					this._Pic_Tax,
    					this._Pic_Uid,
    					this._Acc_Id,
    					this._Acc_Name,
    					this._Pic_IsStatic,
    					this._Pic_PushTime,
    					this._Pic_Label,
    					this._Pic_Type,
    					this._OtherData,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Pic_Id))) {
    				this._Pic_Id = reader.GetInt32(_.Pic_Id);
    			}
    			if ((false == reader.IsDBNull(_.Col_Id))) {
    				this._Col_Id = reader.GetInt32(_.Col_Id);
    			}
    			if ((false == reader.IsDBNull(_.Col_Name))) {
    				this._Col_Name = reader.GetString(_.Col_Name);
    			}
    			if ((false == reader.IsDBNull(_.Pic_Name))) {
    				this._Pic_Name = reader.GetString(_.Pic_Name);
    			}
    			if ((false == reader.IsDBNull(_.Pic_Intro))) {
    				this._Pic_Intro = reader.GetString(_.Pic_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Pic_FilePath))) {
    				this._Pic_FilePath = reader.GetString(_.Pic_FilePath);
    			}
    			if ((false == reader.IsDBNull(_.Pic_FilePathSmall))) {
    				this._Pic_FilePathSmall = reader.GetString(_.Pic_FilePathSmall);
    			}
    			if ((false == reader.IsDBNull(_.Pic_IsShow))) {
    				this._Pic_IsShow = reader.GetBoolean(_.Pic_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Pic_Keywords))) {
    				this._Pic_Keywords = reader.GetString(_.Pic_Keywords);
    			}
    			if ((false == reader.IsDBNull(_.Pic_Descr))) {
    				this._Pic_Descr = reader.GetString(_.Pic_Descr);
    			}
    			if ((false == reader.IsDBNull(_.Pic_IsRec))) {
    				this._Pic_IsRec = reader.GetBoolean(_.Pic_IsRec);
    			}
    			if ((false == reader.IsDBNull(_.Pic_IsTop))) {
    				this._Pic_IsTop = reader.GetBoolean(_.Pic_IsTop);
    			}
    			if ((false == reader.IsDBNull(_.Pic_IsHot))) {
    				this._Pic_IsHot = reader.GetBoolean(_.Pic_IsHot);
    			}
    			if ((false == reader.IsDBNull(_.Pic_IsDel))) {
    				this._Pic_IsDel = reader.GetBoolean(_.Pic_IsDel);
    			}
    			if ((false == reader.IsDBNull(_.Pic_IsCover))) {
    				this._Pic_IsCover = reader.GetBoolean(_.Pic_IsCover);
    			}
    			if ((false == reader.IsDBNull(_.Pic_CrtTime))) {
    				this._Pic_CrtTime = reader.GetDateTime(_.Pic_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Pic_Size))) {
    				this._Pic_Size = reader.GetInt32(_.Pic_Size);
    			}
    			if ((false == reader.IsDBNull(_.Pic_Width))) {
    				this._Pic_Width = reader.GetInt32(_.Pic_Width);
    			}
    			if ((false == reader.IsDBNull(_.Pic_Height))) {
    				this._Pic_Height = reader.GetInt32(_.Pic_Height);
    			}
    			if ((false == reader.IsDBNull(_.Pic_Number))) {
    				this._Pic_Number = reader.GetInt32(_.Pic_Number);
    			}
    			if ((false == reader.IsDBNull(_.Pic_Tax))) {
    				this._Pic_Tax = reader.GetInt32(_.Pic_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Pic_Uid))) {
    				this._Pic_Uid = reader.GetString(_.Pic_Uid);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Name))) {
    				this._Acc_Name = reader.GetString(_.Acc_Name);
    			}
    			if ((false == reader.IsDBNull(_.Pic_IsStatic))) {
    				this._Pic_IsStatic = reader.GetBoolean(_.Pic_IsStatic);
    			}
    			if ((false == reader.IsDBNull(_.Pic_PushTime))) {
    				this._Pic_PushTime = reader.GetDateTime(_.Pic_PushTime);
    			}
    			if ((false == reader.IsDBNull(_.Pic_Label))) {
    				this._Pic_Label = reader.GetString(_.Pic_Label);
    			}
    			if ((false == reader.IsDBNull(_.Pic_Type))) {
    				this._Pic_Type = reader.GetString(_.Pic_Type);
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
    			if ((false == typeof(Picture).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Picture>();
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pic_Id = new WeiSha.Data.Field<Picture>("Pic_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Col_Id = new WeiSha.Data.Field<Picture>("Col_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Col_Name = new WeiSha.Data.Field<Picture>("Col_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pic_Name = new WeiSha.Data.Field<Picture>("Pic_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pic_Intro = new WeiSha.Data.Field<Picture>("Pic_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_FilePath - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pic_FilePath = new WeiSha.Data.Field<Picture>("Pic_FilePath");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_FilePathSmall - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pic_FilePathSmall = new WeiSha.Data.Field<Picture>("Pic_FilePathSmall");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pic_IsShow = new WeiSha.Data.Field<Picture>("Pic_IsShow");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_Keywords - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pic_Keywords = new WeiSha.Data.Field<Picture>("Pic_Keywords");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_Descr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pic_Descr = new WeiSha.Data.Field<Picture>("Pic_Descr");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_IsRec - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pic_IsRec = new WeiSha.Data.Field<Picture>("Pic_IsRec");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_IsTop - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pic_IsTop = new WeiSha.Data.Field<Picture>("Pic_IsTop");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_IsHot - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pic_IsHot = new WeiSha.Data.Field<Picture>("Pic_IsHot");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_IsDel - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pic_IsDel = new WeiSha.Data.Field<Picture>("Pic_IsDel");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_IsCover - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pic_IsCover = new WeiSha.Data.Field<Picture>("Pic_IsCover");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pic_CrtTime = new WeiSha.Data.Field<Picture>("Pic_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_Size - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pic_Size = new WeiSha.Data.Field<Picture>("Pic_Size");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_Width - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pic_Width = new WeiSha.Data.Field<Picture>("Pic_Width");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_Height - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pic_Height = new WeiSha.Data.Field<Picture>("Pic_Height");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_Number - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pic_Number = new WeiSha.Data.Field<Picture>("Pic_Number");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_Tax - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pic_Tax = new WeiSha.Data.Field<Picture>("Pic_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_Uid - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pic_Uid = new WeiSha.Data.Field<Picture>("Pic_Uid");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<Picture>("Acc_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Name = new WeiSha.Data.Field<Picture>("Acc_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_IsStatic - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pic_IsStatic = new WeiSha.Data.Field<Picture>("Pic_IsStatic");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_PushTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pic_PushTime = new WeiSha.Data.Field<Picture>("Pic_PushTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_Label - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pic_Label = new WeiSha.Data.Field<Picture>("Pic_Label");
    			
    			/// <summary>
    			/// -1 - 字段名：Pic_Type - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pic_Type = new WeiSha.Data.Field<Picture>("Pic_Type");
    			
    			/// <summary>
    			/// -1 - 字段名：OtherData - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field OtherData = new WeiSha.Data.Field<Picture>("OtherData");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Picture>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Picture>("Org_Name");
    		}
    	}
    }
    