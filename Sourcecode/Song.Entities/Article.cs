namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Article 主键列：Art_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Article : WeiSha.Data.Entity {
    		
    		protected Int64 _Art_ID;
    		
    		protected Int32 _Acc_Id;
    		
    		protected String _Acc_Name;
    		
    		protected String _Art_Author;
    		
    		protected String _Art_Color;
    		
    		protected DateTime _Art_CrtTime;
    		
    		protected String _Art_Descr;
    		
    		protected String _Art_Details;
    		
    		protected String _Art_Endnote;
    		
    		protected String _Art_ErrInfo;
    		
    		protected String _Art_Font;
    		
    		protected String _Art_Intro;
    		
    		protected Boolean _Art_IsDel;
    		
    		protected Boolean _Art_IsError;
    		
    		protected Boolean _Art_IsHot;
    		
    		protected Boolean _Art_IsImg;
    		
    		protected Boolean _Art_IsNote;
    		
    		protected Boolean _Art_IsOut;
    		
    		protected Boolean _Art_IsRec;
    		
    		protected Boolean _Art_IsShow;
    		
    		protected Boolean _Art_IsStatic;
    		
    		protected Boolean _Art_IsTop;
    		
    		protected Boolean _Art_IsUse;
    		
    		protected Boolean _Art_IsVerify;
    		
    		protected String _Art_Keywords;
    		
    		protected String _Art_Label;
    		
    		protected DateTime _Art_LastTime;
    		
    		protected String _Art_Logo;
    		
    		protected Int32 _Art_Number;
    		
    		protected Int32 _Art_Order;
    		
    		protected String _Art_OutUrl;
    		
    		protected DateTime _Art_PushTime;
    		
    		protected String _Art_Source;
    		
    		protected String _Art_Title;
    		
    		protected String _Art_TitleAbbr;
    		
    		protected String _Art_TitleFull;
    		
    		protected String _Art_TitleSub;
    		
    		protected String _Art_Uid;
    		
    		protected String _Art_VerifyMan;
    		
    		protected DateTime _Art_VerifyTime;
    		
    		protected String _Col_Name;
    		
    		protected String _Col_UID;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected String _OtherData;
    		
    		public Int64 Art_ID {
    			get {
    				return this._Art_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_ID, _Art_ID, value);
    				this._Art_ID = value;
    			}
    		}
    		
    		public Int32 Acc_Id {
    			get {
    				return this._Acc_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Id, _Acc_Id, value);
    				this._Acc_Id = value;
    			}
    		}
    		
    		public String Acc_Name {
    			get {
    				return this._Acc_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Name, _Acc_Name, value);
    				this._Acc_Name = value;
    			}
    		}
    		
    		public String Art_Author {
    			get {
    				return this._Art_Author;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Author, _Art_Author, value);
    				this._Art_Author = value;
    			}
    		}
    		
    		public String Art_Color {
    			get {
    				return this._Art_Color;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Color, _Art_Color, value);
    				this._Art_Color = value;
    			}
    		}
    		
    		public DateTime Art_CrtTime {
    			get {
    				return this._Art_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_CrtTime, _Art_CrtTime, value);
    				this._Art_CrtTime = value;
    			}
    		}
    		
    		public String Art_Descr {
    			get {
    				return this._Art_Descr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Descr, _Art_Descr, value);
    				this._Art_Descr = value;
    			}
    		}
    		
    		public String Art_Details {
    			get {
    				return this._Art_Details;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Details, _Art_Details, value);
    				this._Art_Details = value;
    			}
    		}
    		
    		public String Art_Endnote {
    			get {
    				return this._Art_Endnote;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Endnote, _Art_Endnote, value);
    				this._Art_Endnote = value;
    			}
    		}
    		
    		public String Art_ErrInfo {
    			get {
    				return this._Art_ErrInfo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_ErrInfo, _Art_ErrInfo, value);
    				this._Art_ErrInfo = value;
    			}
    		}
    		
    		public String Art_Font {
    			get {
    				return this._Art_Font;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Font, _Art_Font, value);
    				this._Art_Font = value;
    			}
    		}
    		
    		public String Art_Intro {
    			get {
    				return this._Art_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Intro, _Art_Intro, value);
    				this._Art_Intro = value;
    			}
    		}
    		
    		public Boolean Art_IsDel {
    			get {
    				return this._Art_IsDel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_IsDel, _Art_IsDel, value);
    				this._Art_IsDel = value;
    			}
    		}
    		
    		public Boolean Art_IsError {
    			get {
    				return this._Art_IsError;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_IsError, _Art_IsError, value);
    				this._Art_IsError = value;
    			}
    		}
    		
    		public Boolean Art_IsHot {
    			get {
    				return this._Art_IsHot;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_IsHot, _Art_IsHot, value);
    				this._Art_IsHot = value;
    			}
    		}
    		
    		public Boolean Art_IsImg {
    			get {
    				return this._Art_IsImg;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_IsImg, _Art_IsImg, value);
    				this._Art_IsImg = value;
    			}
    		}
    		
    		public Boolean Art_IsNote {
    			get {
    				return this._Art_IsNote;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_IsNote, _Art_IsNote, value);
    				this._Art_IsNote = value;
    			}
    		}
    		
    		public Boolean Art_IsOut {
    			get {
    				return this._Art_IsOut;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_IsOut, _Art_IsOut, value);
    				this._Art_IsOut = value;
    			}
    		}
    		
    		public Boolean Art_IsRec {
    			get {
    				return this._Art_IsRec;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_IsRec, _Art_IsRec, value);
    				this._Art_IsRec = value;
    			}
    		}
    		
    		public Boolean Art_IsShow {
    			get {
    				return this._Art_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_IsShow, _Art_IsShow, value);
    				this._Art_IsShow = value;
    			}
    		}
    		
    		public Boolean Art_IsStatic {
    			get {
    				return this._Art_IsStatic;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_IsStatic, _Art_IsStatic, value);
    				this._Art_IsStatic = value;
    			}
    		}
    		
    		public Boolean Art_IsTop {
    			get {
    				return this._Art_IsTop;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_IsTop, _Art_IsTop, value);
    				this._Art_IsTop = value;
    			}
    		}
    		
    		public Boolean Art_IsUse {
    			get {
    				return this._Art_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_IsUse, _Art_IsUse, value);
    				this._Art_IsUse = value;
    			}
    		}
    		
    		public Boolean Art_IsVerify {
    			get {
    				return this._Art_IsVerify;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_IsVerify, _Art_IsVerify, value);
    				this._Art_IsVerify = value;
    			}
    		}
    		
    		public String Art_Keywords {
    			get {
    				return this._Art_Keywords;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Keywords, _Art_Keywords, value);
    				this._Art_Keywords = value;
    			}
    		}
    		
    		public String Art_Label {
    			get {
    				return this._Art_Label;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Label, _Art_Label, value);
    				this._Art_Label = value;
    			}
    		}
    		
    		public DateTime Art_LastTime {
    			get {
    				return this._Art_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_LastTime, _Art_LastTime, value);
    				this._Art_LastTime = value;
    			}
    		}
    		
    		public String Art_Logo {
    			get {
    				return this._Art_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Logo, _Art_Logo, value);
    				this._Art_Logo = value;
    			}
    		}
    		
    		public Int32 Art_Number {
    			get {
    				return this._Art_Number;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Number, _Art_Number, value);
    				this._Art_Number = value;
    			}
    		}
    		
    		public Int32 Art_Order {
    			get {
    				return this._Art_Order;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Order, _Art_Order, value);
    				this._Art_Order = value;
    			}
    		}
    		
    		public String Art_OutUrl {
    			get {
    				return this._Art_OutUrl;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_OutUrl, _Art_OutUrl, value);
    				this._Art_OutUrl = value;
    			}
    		}
    		
    		public DateTime Art_PushTime {
    			get {
    				return this._Art_PushTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_PushTime, _Art_PushTime, value);
    				this._Art_PushTime = value;
    			}
    		}
    		
    		public String Art_Source {
    			get {
    				return this._Art_Source;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Source, _Art_Source, value);
    				this._Art_Source = value;
    			}
    		}
    		
    		public String Art_Title {
    			get {
    				return this._Art_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Title, _Art_Title, value);
    				this._Art_Title = value;
    			}
    		}
    		
    		public String Art_TitleAbbr {
    			get {
    				return this._Art_TitleAbbr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_TitleAbbr, _Art_TitleAbbr, value);
    				this._Art_TitleAbbr = value;
    			}
    		}
    		
    		public String Art_TitleFull {
    			get {
    				return this._Art_TitleFull;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_TitleFull, _Art_TitleFull, value);
    				this._Art_TitleFull = value;
    			}
    		}
    		
    		public String Art_TitleSub {
    			get {
    				return this._Art_TitleSub;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_TitleSub, _Art_TitleSub, value);
    				this._Art_TitleSub = value;
    			}
    		}
    		
    		public String Art_Uid {
    			get {
    				return this._Art_Uid;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_Uid, _Art_Uid, value);
    				this._Art_Uid = value;
    			}
    		}
    		
    		public String Art_VerifyMan {
    			get {
    				return this._Art_VerifyMan;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_VerifyMan, _Art_VerifyMan, value);
    				this._Art_VerifyMan = value;
    			}
    		}
    		
    		public DateTime Art_VerifyTime {
    			get {
    				return this._Art_VerifyTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Art_VerifyTime, _Art_VerifyTime, value);
    				this._Art_VerifyTime = value;
    			}
    		}
    		
    		public String Col_Name {
    			get {
    				return this._Col_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_Name, _Col_Name, value);
    				this._Col_Name = value;
    			}
    		}
    		
    		public String Col_UID {
    			get {
    				return this._Col_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_UID, _Col_UID, value);
    				this._Col_UID = value;
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
    		
    		public String OtherData {
    			get {
    				return this._OtherData;
    			}
    			set {
    				this.OnPropertyValueChange(_.OtherData, _OtherData, value);
    				this._OtherData = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Article>("Article");
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Art_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Art_ID,
    					_.Acc_Id,
    					_.Acc_Name,
    					_.Art_Author,
    					_.Art_Color,
    					_.Art_CrtTime,
    					_.Art_Descr,
    					_.Art_Details,
    					_.Art_Endnote,
    					_.Art_ErrInfo,
    					_.Art_Font,
    					_.Art_Intro,
    					_.Art_IsDel,
    					_.Art_IsError,
    					_.Art_IsHot,
    					_.Art_IsImg,
    					_.Art_IsNote,
    					_.Art_IsOut,
    					_.Art_IsRec,
    					_.Art_IsShow,
    					_.Art_IsStatic,
    					_.Art_IsTop,
    					_.Art_IsUse,
    					_.Art_IsVerify,
    					_.Art_Keywords,
    					_.Art_Label,
    					_.Art_LastTime,
    					_.Art_Logo,
    					_.Art_Number,
    					_.Art_Order,
    					_.Art_OutUrl,
    					_.Art_PushTime,
    					_.Art_Source,
    					_.Art_Title,
    					_.Art_TitleAbbr,
    					_.Art_TitleFull,
    					_.Art_TitleSub,
    					_.Art_Uid,
    					_.Art_VerifyMan,
    					_.Art_VerifyTime,
    					_.Col_Name,
    					_.Col_UID,
    					_.Org_ID,
    					_.Org_Name,
    					_.OtherData};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Art_ID,
    					this._Acc_Id,
    					this._Acc_Name,
    					this._Art_Author,
    					this._Art_Color,
    					this._Art_CrtTime,
    					this._Art_Descr,
    					this._Art_Details,
    					this._Art_Endnote,
    					this._Art_ErrInfo,
    					this._Art_Font,
    					this._Art_Intro,
    					this._Art_IsDel,
    					this._Art_IsError,
    					this._Art_IsHot,
    					this._Art_IsImg,
    					this._Art_IsNote,
    					this._Art_IsOut,
    					this._Art_IsRec,
    					this._Art_IsShow,
    					this._Art_IsStatic,
    					this._Art_IsTop,
    					this._Art_IsUse,
    					this._Art_IsVerify,
    					this._Art_Keywords,
    					this._Art_Label,
    					this._Art_LastTime,
    					this._Art_Logo,
    					this._Art_Number,
    					this._Art_Order,
    					this._Art_OutUrl,
    					this._Art_PushTime,
    					this._Art_Source,
    					this._Art_Title,
    					this._Art_TitleAbbr,
    					this._Art_TitleFull,
    					this._Art_TitleSub,
    					this._Art_Uid,
    					this._Art_VerifyMan,
    					this._Art_VerifyTime,
    					this._Col_Name,
    					this._Col_UID,
    					this._Org_ID,
    					this._Org_Name,
    					this._OtherData};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Art_ID))) {
    				this._Art_ID = reader.GetInt64(_.Art_ID);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Name))) {
    				this._Acc_Name = reader.GetString(_.Acc_Name);
    			}
    			if ((false == reader.IsDBNull(_.Art_Author))) {
    				this._Art_Author = reader.GetString(_.Art_Author);
    			}
    			if ((false == reader.IsDBNull(_.Art_Color))) {
    				this._Art_Color = reader.GetString(_.Art_Color);
    			}
    			if ((false == reader.IsDBNull(_.Art_CrtTime))) {
    				this._Art_CrtTime = reader.GetDateTime(_.Art_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Art_Descr))) {
    				this._Art_Descr = reader.GetString(_.Art_Descr);
    			}
    			if ((false == reader.IsDBNull(_.Art_Details))) {
    				this._Art_Details = reader.GetString(_.Art_Details);
    			}
    			if ((false == reader.IsDBNull(_.Art_Endnote))) {
    				this._Art_Endnote = reader.GetString(_.Art_Endnote);
    			}
    			if ((false == reader.IsDBNull(_.Art_ErrInfo))) {
    				this._Art_ErrInfo = reader.GetString(_.Art_ErrInfo);
    			}
    			if ((false == reader.IsDBNull(_.Art_Font))) {
    				this._Art_Font = reader.GetString(_.Art_Font);
    			}
    			if ((false == reader.IsDBNull(_.Art_Intro))) {
    				this._Art_Intro = reader.GetString(_.Art_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Art_IsDel))) {
    				this._Art_IsDel = reader.GetBoolean(_.Art_IsDel);
    			}
    			if ((false == reader.IsDBNull(_.Art_IsError))) {
    				this._Art_IsError = reader.GetBoolean(_.Art_IsError);
    			}
    			if ((false == reader.IsDBNull(_.Art_IsHot))) {
    				this._Art_IsHot = reader.GetBoolean(_.Art_IsHot);
    			}
    			if ((false == reader.IsDBNull(_.Art_IsImg))) {
    				this._Art_IsImg = reader.GetBoolean(_.Art_IsImg);
    			}
    			if ((false == reader.IsDBNull(_.Art_IsNote))) {
    				this._Art_IsNote = reader.GetBoolean(_.Art_IsNote);
    			}
    			if ((false == reader.IsDBNull(_.Art_IsOut))) {
    				this._Art_IsOut = reader.GetBoolean(_.Art_IsOut);
    			}
    			if ((false == reader.IsDBNull(_.Art_IsRec))) {
    				this._Art_IsRec = reader.GetBoolean(_.Art_IsRec);
    			}
    			if ((false == reader.IsDBNull(_.Art_IsShow))) {
    				this._Art_IsShow = reader.GetBoolean(_.Art_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Art_IsStatic))) {
    				this._Art_IsStatic = reader.GetBoolean(_.Art_IsStatic);
    			}
    			if ((false == reader.IsDBNull(_.Art_IsTop))) {
    				this._Art_IsTop = reader.GetBoolean(_.Art_IsTop);
    			}
    			if ((false == reader.IsDBNull(_.Art_IsUse))) {
    				this._Art_IsUse = reader.GetBoolean(_.Art_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Art_IsVerify))) {
    				this._Art_IsVerify = reader.GetBoolean(_.Art_IsVerify);
    			}
    			if ((false == reader.IsDBNull(_.Art_Keywords))) {
    				this._Art_Keywords = reader.GetString(_.Art_Keywords);
    			}
    			if ((false == reader.IsDBNull(_.Art_Label))) {
    				this._Art_Label = reader.GetString(_.Art_Label);
    			}
    			if ((false == reader.IsDBNull(_.Art_LastTime))) {
    				this._Art_LastTime = reader.GetDateTime(_.Art_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Art_Logo))) {
    				this._Art_Logo = reader.GetString(_.Art_Logo);
    			}
    			if ((false == reader.IsDBNull(_.Art_Number))) {
    				this._Art_Number = reader.GetInt32(_.Art_Number);
    			}
    			if ((false == reader.IsDBNull(_.Art_Order))) {
    				this._Art_Order = reader.GetInt32(_.Art_Order);
    			}
    			if ((false == reader.IsDBNull(_.Art_OutUrl))) {
    				this._Art_OutUrl = reader.GetString(_.Art_OutUrl);
    			}
    			if ((false == reader.IsDBNull(_.Art_PushTime))) {
    				this._Art_PushTime = reader.GetDateTime(_.Art_PushTime);
    			}
    			if ((false == reader.IsDBNull(_.Art_Source))) {
    				this._Art_Source = reader.GetString(_.Art_Source);
    			}
    			if ((false == reader.IsDBNull(_.Art_Title))) {
    				this._Art_Title = reader.GetString(_.Art_Title);
    			}
    			if ((false == reader.IsDBNull(_.Art_TitleAbbr))) {
    				this._Art_TitleAbbr = reader.GetString(_.Art_TitleAbbr);
    			}
    			if ((false == reader.IsDBNull(_.Art_TitleFull))) {
    				this._Art_TitleFull = reader.GetString(_.Art_TitleFull);
    			}
    			if ((false == reader.IsDBNull(_.Art_TitleSub))) {
    				this._Art_TitleSub = reader.GetString(_.Art_TitleSub);
    			}
    			if ((false == reader.IsDBNull(_.Art_Uid))) {
    				this._Art_Uid = reader.GetString(_.Art_Uid);
    			}
    			if ((false == reader.IsDBNull(_.Art_VerifyMan))) {
    				this._Art_VerifyMan = reader.GetString(_.Art_VerifyMan);
    			}
    			if ((false == reader.IsDBNull(_.Art_VerifyTime))) {
    				this._Art_VerifyTime = reader.GetDateTime(_.Art_VerifyTime);
    			}
    			if ((false == reader.IsDBNull(_.Col_Name))) {
    				this._Col_Name = reader.GetString(_.Col_Name);
    			}
    			if ((false == reader.IsDBNull(_.Col_UID))) {
    				this._Col_UID = reader.GetString(_.Col_UID);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.OtherData))) {
    				this._OtherData = reader.GetString(_.OtherData);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Article).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Article>();
    			
    			/// <summary>
    			/// 字段名：Art_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Art_ID = new WeiSha.Data.Field<Article>("Art_ID");
    			
    			/// <summary>
    			/// 字段名：Acc_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<Article>("Acc_Id");
    			
    			/// <summary>
    			/// 字段名：Acc_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Name = new WeiSha.Data.Field<Article>("Acc_Name");
    			
    			/// <summary>
    			/// 字段名：Art_Author - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_Author = new WeiSha.Data.Field<Article>("Art_Author");
    			
    			/// <summary>
    			/// 字段名：Art_Color - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_Color = new WeiSha.Data.Field<Article>("Art_Color");
    			
    			/// <summary>
    			/// 字段名：Art_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Art_CrtTime = new WeiSha.Data.Field<Article>("Art_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Art_Descr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_Descr = new WeiSha.Data.Field<Article>("Art_Descr");
    			
    			/// <summary>
    			/// 字段名：Art_Details - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_Details = new WeiSha.Data.Field<Article>("Art_Details");
    			
    			/// <summary>
    			/// 字段名：Art_Endnote - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_Endnote = new WeiSha.Data.Field<Article>("Art_Endnote");
    			
    			/// <summary>
    			/// 字段名：Art_ErrInfo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_ErrInfo = new WeiSha.Data.Field<Article>("Art_ErrInfo");
    			
    			/// <summary>
    			/// 字段名：Art_Font - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_Font = new WeiSha.Data.Field<Article>("Art_Font");
    			
    			/// <summary>
    			/// 字段名：Art_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_Intro = new WeiSha.Data.Field<Article>("Art_Intro");
    			
    			/// <summary>
    			/// 字段名：Art_IsDel - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Art_IsDel = new WeiSha.Data.Field<Article>("Art_IsDel");
    			
    			/// <summary>
    			/// 字段名：Art_IsError - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Art_IsError = new WeiSha.Data.Field<Article>("Art_IsError");
    			
    			/// <summary>
    			/// 字段名：Art_IsHot - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Art_IsHot = new WeiSha.Data.Field<Article>("Art_IsHot");
    			
    			/// <summary>
    			/// 字段名：Art_IsImg - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Art_IsImg = new WeiSha.Data.Field<Article>("Art_IsImg");
    			
    			/// <summary>
    			/// 字段名：Art_IsNote - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Art_IsNote = new WeiSha.Data.Field<Article>("Art_IsNote");
    			
    			/// <summary>
    			/// 字段名：Art_IsOut - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Art_IsOut = new WeiSha.Data.Field<Article>("Art_IsOut");
    			
    			/// <summary>
    			/// 字段名：Art_IsRec - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Art_IsRec = new WeiSha.Data.Field<Article>("Art_IsRec");
    			
    			/// <summary>
    			/// 字段名：Art_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Art_IsShow = new WeiSha.Data.Field<Article>("Art_IsShow");
    			
    			/// <summary>
    			/// 字段名：Art_IsStatic - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Art_IsStatic = new WeiSha.Data.Field<Article>("Art_IsStatic");
    			
    			/// <summary>
    			/// 字段名：Art_IsTop - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Art_IsTop = new WeiSha.Data.Field<Article>("Art_IsTop");
    			
    			/// <summary>
    			/// 字段名：Art_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Art_IsUse = new WeiSha.Data.Field<Article>("Art_IsUse");
    			
    			/// <summary>
    			/// 字段名：Art_IsVerify - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Art_IsVerify = new WeiSha.Data.Field<Article>("Art_IsVerify");
    			
    			/// <summary>
    			/// 字段名：Art_Keywords - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_Keywords = new WeiSha.Data.Field<Article>("Art_Keywords");
    			
    			/// <summary>
    			/// 字段名：Art_Label - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_Label = new WeiSha.Data.Field<Article>("Art_Label");
    			
    			/// <summary>
    			/// 字段名：Art_LastTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Art_LastTime = new WeiSha.Data.Field<Article>("Art_LastTime");
    			
    			/// <summary>
    			/// 字段名：Art_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_Logo = new WeiSha.Data.Field<Article>("Art_Logo");
    			
    			/// <summary>
    			/// 字段名：Art_Number - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Art_Number = new WeiSha.Data.Field<Article>("Art_Number");
    			
    			/// <summary>
    			/// 字段名：Art_Order - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Art_Order = new WeiSha.Data.Field<Article>("Art_Order");
    			
    			/// <summary>
    			/// 字段名：Art_OutUrl - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_OutUrl = new WeiSha.Data.Field<Article>("Art_OutUrl");
    			
    			/// <summary>
    			/// 字段名：Art_PushTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Art_PushTime = new WeiSha.Data.Field<Article>("Art_PushTime");
    			
    			/// <summary>
    			/// 字段名：Art_Source - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_Source = new WeiSha.Data.Field<Article>("Art_Source");
    			
    			/// <summary>
    			/// 字段名：Art_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_Title = new WeiSha.Data.Field<Article>("Art_Title");
    			
    			/// <summary>
    			/// 字段名：Art_TitleAbbr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_TitleAbbr = new WeiSha.Data.Field<Article>("Art_TitleAbbr");
    			
    			/// <summary>
    			/// 字段名：Art_TitleFull - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_TitleFull = new WeiSha.Data.Field<Article>("Art_TitleFull");
    			
    			/// <summary>
    			/// 字段名：Art_TitleSub - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_TitleSub = new WeiSha.Data.Field<Article>("Art_TitleSub");
    			
    			/// <summary>
    			/// 字段名：Art_Uid - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_Uid = new WeiSha.Data.Field<Article>("Art_Uid");
    			
    			/// <summary>
    			/// 字段名：Art_VerifyMan - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Art_VerifyMan = new WeiSha.Data.Field<Article>("Art_VerifyMan");
    			
    			/// <summary>
    			/// 字段名：Art_VerifyTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Art_VerifyTime = new WeiSha.Data.Field<Article>("Art_VerifyTime");
    			
    			/// <summary>
    			/// 字段名：Col_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Col_Name = new WeiSha.Data.Field<Article>("Col_Name");
    			
    			/// <summary>
    			/// 字段名：Col_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Col_UID = new WeiSha.Data.Field<Article>("Col_UID");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Article>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Article>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：OtherData - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field OtherData = new WeiSha.Data.Field<Article>("OtherData");
    		}
    	}
    }
    