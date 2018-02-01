namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Video 主键列：Vi_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Video : WeiSha.Data.Entity {
    		
    		protected Int32 _Vi_Id;
    		
    		protected String _Vi_Name;
    		
    		protected String _Vi_Intro;
    		
    		protected String _Vi_Details;
    		
    		protected String _Vi_VideoFile;
    		
    		protected String _Vi_FilePath;
    		
    		protected String _Vi_FilePathSmall;
    		
    		protected Boolean _Vi_IsShow;
    		
    		protected String _Vi_Keywords;
    		
    		protected String _Vi_Descr;
    		
    		protected Boolean _Vi_IsRec;
    		
    		protected Boolean _Vi_IsTop;
    		
    		protected Boolean _Vi_IsHot;
    		
    		protected Boolean _Vi_IsDel;
    		
    		protected Boolean _Vi_IsCover;
    		
    		protected DateTime? _Vi_CrtTime;
    		
    		protected Int32? _Vi_Size;
    		
    		protected Int32? _Vi_Width;
    		
    		protected Int32? _Vi_Height;
    		
    		protected Int32? _Vi_Number;
    		
    		protected Int32? _Vi_Tax;
    		
    		protected Int32? _Col_Id;
    		
    		protected String _Col_Name;
    		
    		protected Int32? _Acc_Id;
    		
    		protected String _Acc_Name;
    		
    		protected Boolean _Vi_IsStatic;
    		
    		protected DateTime? _Vi_PushTime;
    		
    		protected String _Vi_Label;
    		
    		protected String _Vi_Uid;
    		
    		protected String _OtherData;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Vi_Id {
    			get {
    				return this._Vi_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_Id, _Vi_Id, value);
    				this._Vi_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vi_Name {
    			get {
    				return this._Vi_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_Name, _Vi_Name, value);
    				this._Vi_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vi_Intro {
    			get {
    				return this._Vi_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_Intro, _Vi_Intro, value);
    				this._Vi_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vi_Details {
    			get {
    				return this._Vi_Details;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_Details, _Vi_Details, value);
    				this._Vi_Details = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vi_VideoFile {
    			get {
    				return this._Vi_VideoFile;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_VideoFile, _Vi_VideoFile, value);
    				this._Vi_VideoFile = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vi_FilePath {
    			get {
    				return this._Vi_FilePath;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_FilePath, _Vi_FilePath, value);
    				this._Vi_FilePath = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vi_FilePathSmall {
    			get {
    				return this._Vi_FilePathSmall;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_FilePathSmall, _Vi_FilePathSmall, value);
    				this._Vi_FilePathSmall = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Vi_IsShow {
    			get {
    				return this._Vi_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_IsShow, _Vi_IsShow, value);
    				this._Vi_IsShow = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vi_Keywords {
    			get {
    				return this._Vi_Keywords;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_Keywords, _Vi_Keywords, value);
    				this._Vi_Keywords = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vi_Descr {
    			get {
    				return this._Vi_Descr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_Descr, _Vi_Descr, value);
    				this._Vi_Descr = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Vi_IsRec {
    			get {
    				return this._Vi_IsRec;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_IsRec, _Vi_IsRec, value);
    				this._Vi_IsRec = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Vi_IsTop {
    			get {
    				return this._Vi_IsTop;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_IsTop, _Vi_IsTop, value);
    				this._Vi_IsTop = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Vi_IsHot {
    			get {
    				return this._Vi_IsHot;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_IsHot, _Vi_IsHot, value);
    				this._Vi_IsHot = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Vi_IsDel {
    			get {
    				return this._Vi_IsDel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_IsDel, _Vi_IsDel, value);
    				this._Vi_IsDel = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Vi_IsCover {
    			get {
    				return this._Vi_IsCover;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_IsCover, _Vi_IsCover, value);
    				this._Vi_IsCover = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Vi_CrtTime {
    			get {
    				return this._Vi_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_CrtTime, _Vi_CrtTime, value);
    				this._Vi_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Vi_Size {
    			get {
    				return this._Vi_Size;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_Size, _Vi_Size, value);
    				this._Vi_Size = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Vi_Width {
    			get {
    				return this._Vi_Width;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_Width, _Vi_Width, value);
    				this._Vi_Width = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Vi_Height {
    			get {
    				return this._Vi_Height;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_Height, _Vi_Height, value);
    				this._Vi_Height = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Vi_Number {
    			get {
    				return this._Vi_Number;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_Number, _Vi_Number, value);
    				this._Vi_Number = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Vi_Tax {
    			get {
    				return this._Vi_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_Tax, _Vi_Tax, value);
    				this._Vi_Tax = value;
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
    		public Boolean Vi_IsStatic {
    			get {
    				return this._Vi_IsStatic;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_IsStatic, _Vi_IsStatic, value);
    				this._Vi_IsStatic = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Vi_PushTime {
    			get {
    				return this._Vi_PushTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_PushTime, _Vi_PushTime, value);
    				this._Vi_PushTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vi_Label {
    			get {
    				return this._Vi_Label;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_Label, _Vi_Label, value);
    				this._Vi_Label = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Vi_Uid {
    			get {
    				return this._Vi_Uid;
    			}
    			set {
    				this.OnPropertyValueChange(_.Vi_Uid, _Vi_Uid, value);
    				this._Vi_Uid = value;
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
    			return new WeiSha.Data.Table<Video>("Video");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Vi_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Vi_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Vi_Id,
    					_.Vi_Name,
    					_.Vi_Intro,
    					_.Vi_Details,
    					_.Vi_VideoFile,
    					_.Vi_FilePath,
    					_.Vi_FilePathSmall,
    					_.Vi_IsShow,
    					_.Vi_Keywords,
    					_.Vi_Descr,
    					_.Vi_IsRec,
    					_.Vi_IsTop,
    					_.Vi_IsHot,
    					_.Vi_IsDel,
    					_.Vi_IsCover,
    					_.Vi_CrtTime,
    					_.Vi_Size,
    					_.Vi_Width,
    					_.Vi_Height,
    					_.Vi_Number,
    					_.Vi_Tax,
    					_.Col_Id,
    					_.Col_Name,
    					_.Acc_Id,
    					_.Acc_Name,
    					_.Vi_IsStatic,
    					_.Vi_PushTime,
    					_.Vi_Label,
    					_.Vi_Uid,
    					_.OtherData,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Vi_Id,
    					this._Vi_Name,
    					this._Vi_Intro,
    					this._Vi_Details,
    					this._Vi_VideoFile,
    					this._Vi_FilePath,
    					this._Vi_FilePathSmall,
    					this._Vi_IsShow,
    					this._Vi_Keywords,
    					this._Vi_Descr,
    					this._Vi_IsRec,
    					this._Vi_IsTop,
    					this._Vi_IsHot,
    					this._Vi_IsDel,
    					this._Vi_IsCover,
    					this._Vi_CrtTime,
    					this._Vi_Size,
    					this._Vi_Width,
    					this._Vi_Height,
    					this._Vi_Number,
    					this._Vi_Tax,
    					this._Col_Id,
    					this._Col_Name,
    					this._Acc_Id,
    					this._Acc_Name,
    					this._Vi_IsStatic,
    					this._Vi_PushTime,
    					this._Vi_Label,
    					this._Vi_Uid,
    					this._OtherData,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Vi_Id))) {
    				this._Vi_Id = reader.GetInt32(_.Vi_Id);
    			}
    			if ((false == reader.IsDBNull(_.Vi_Name))) {
    				this._Vi_Name = reader.GetString(_.Vi_Name);
    			}
    			if ((false == reader.IsDBNull(_.Vi_Intro))) {
    				this._Vi_Intro = reader.GetString(_.Vi_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Vi_Details))) {
    				this._Vi_Details = reader.GetString(_.Vi_Details);
    			}
    			if ((false == reader.IsDBNull(_.Vi_VideoFile))) {
    				this._Vi_VideoFile = reader.GetString(_.Vi_VideoFile);
    			}
    			if ((false == reader.IsDBNull(_.Vi_FilePath))) {
    				this._Vi_FilePath = reader.GetString(_.Vi_FilePath);
    			}
    			if ((false == reader.IsDBNull(_.Vi_FilePathSmall))) {
    				this._Vi_FilePathSmall = reader.GetString(_.Vi_FilePathSmall);
    			}
    			if ((false == reader.IsDBNull(_.Vi_IsShow))) {
    				this._Vi_IsShow = reader.GetBoolean(_.Vi_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Vi_Keywords))) {
    				this._Vi_Keywords = reader.GetString(_.Vi_Keywords);
    			}
    			if ((false == reader.IsDBNull(_.Vi_Descr))) {
    				this._Vi_Descr = reader.GetString(_.Vi_Descr);
    			}
    			if ((false == reader.IsDBNull(_.Vi_IsRec))) {
    				this._Vi_IsRec = reader.GetBoolean(_.Vi_IsRec);
    			}
    			if ((false == reader.IsDBNull(_.Vi_IsTop))) {
    				this._Vi_IsTop = reader.GetBoolean(_.Vi_IsTop);
    			}
    			if ((false == reader.IsDBNull(_.Vi_IsHot))) {
    				this._Vi_IsHot = reader.GetBoolean(_.Vi_IsHot);
    			}
    			if ((false == reader.IsDBNull(_.Vi_IsDel))) {
    				this._Vi_IsDel = reader.GetBoolean(_.Vi_IsDel);
    			}
    			if ((false == reader.IsDBNull(_.Vi_IsCover))) {
    				this._Vi_IsCover = reader.GetBoolean(_.Vi_IsCover);
    			}
    			if ((false == reader.IsDBNull(_.Vi_CrtTime))) {
    				this._Vi_CrtTime = reader.GetDateTime(_.Vi_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Vi_Size))) {
    				this._Vi_Size = reader.GetInt32(_.Vi_Size);
    			}
    			if ((false == reader.IsDBNull(_.Vi_Width))) {
    				this._Vi_Width = reader.GetInt32(_.Vi_Width);
    			}
    			if ((false == reader.IsDBNull(_.Vi_Height))) {
    				this._Vi_Height = reader.GetInt32(_.Vi_Height);
    			}
    			if ((false == reader.IsDBNull(_.Vi_Number))) {
    				this._Vi_Number = reader.GetInt32(_.Vi_Number);
    			}
    			if ((false == reader.IsDBNull(_.Vi_Tax))) {
    				this._Vi_Tax = reader.GetInt32(_.Vi_Tax);
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
    			if ((false == reader.IsDBNull(_.Vi_IsStatic))) {
    				this._Vi_IsStatic = reader.GetBoolean(_.Vi_IsStatic);
    			}
    			if ((false == reader.IsDBNull(_.Vi_PushTime))) {
    				this._Vi_PushTime = reader.GetDateTime(_.Vi_PushTime);
    			}
    			if ((false == reader.IsDBNull(_.Vi_Label))) {
    				this._Vi_Label = reader.GetString(_.Vi_Label);
    			}
    			if ((false == reader.IsDBNull(_.Vi_Uid))) {
    				this._Vi_Uid = reader.GetString(_.Vi_Uid);
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
    			if ((false == typeof(Video).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Video>();
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Vi_Id = new WeiSha.Data.Field<Video>("Vi_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vi_Name = new WeiSha.Data.Field<Video>("Vi_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vi_Intro = new WeiSha.Data.Field<Video>("Vi_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_Details - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vi_Details = new WeiSha.Data.Field<Video>("Vi_Details");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_VideoFile - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vi_VideoFile = new WeiSha.Data.Field<Video>("Vi_VideoFile");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_FilePath - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vi_FilePath = new WeiSha.Data.Field<Video>("Vi_FilePath");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_FilePathSmall - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vi_FilePathSmall = new WeiSha.Data.Field<Video>("Vi_FilePathSmall");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Vi_IsShow = new WeiSha.Data.Field<Video>("Vi_IsShow");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_Keywords - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vi_Keywords = new WeiSha.Data.Field<Video>("Vi_Keywords");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_Descr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vi_Descr = new WeiSha.Data.Field<Video>("Vi_Descr");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_IsRec - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Vi_IsRec = new WeiSha.Data.Field<Video>("Vi_IsRec");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_IsTop - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Vi_IsTop = new WeiSha.Data.Field<Video>("Vi_IsTop");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_IsHot - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Vi_IsHot = new WeiSha.Data.Field<Video>("Vi_IsHot");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_IsDel - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Vi_IsDel = new WeiSha.Data.Field<Video>("Vi_IsDel");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_IsCover - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Vi_IsCover = new WeiSha.Data.Field<Video>("Vi_IsCover");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Vi_CrtTime = new WeiSha.Data.Field<Video>("Vi_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_Size - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Vi_Size = new WeiSha.Data.Field<Video>("Vi_Size");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_Width - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Vi_Width = new WeiSha.Data.Field<Video>("Vi_Width");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_Height - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Vi_Height = new WeiSha.Data.Field<Video>("Vi_Height");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_Number - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Vi_Number = new WeiSha.Data.Field<Video>("Vi_Number");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_Tax - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Vi_Tax = new WeiSha.Data.Field<Video>("Vi_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Col_Id = new WeiSha.Data.Field<Video>("Col_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Col_Name = new WeiSha.Data.Field<Video>("Col_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<Video>("Acc_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Name = new WeiSha.Data.Field<Video>("Acc_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_IsStatic - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Vi_IsStatic = new WeiSha.Data.Field<Video>("Vi_IsStatic");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_PushTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Vi_PushTime = new WeiSha.Data.Field<Video>("Vi_PushTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_Label - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vi_Label = new WeiSha.Data.Field<Video>("Vi_Label");
    			
    			/// <summary>
    			/// -1 - 字段名：Vi_Uid - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Vi_Uid = new WeiSha.Data.Field<Video>("Vi_Uid");
    			
    			/// <summary>
    			/// -1 - 字段名：OtherData - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field OtherData = new WeiSha.Data.Field<Video>("OtherData");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Video>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Video>("Org_Name");
    		}
    	}
    }
    