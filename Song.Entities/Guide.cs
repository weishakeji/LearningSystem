namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Guide 主键列：Gu_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Guide : WeiSha.Data.Entity {
    		
    		protected Int32 _Gu_Id;
    		
    		protected String _Gc_Title;
    		
    		protected Int32? _Gc_ID;
    		
    		protected Int32 _Cou_ID;
    		
    		protected String _Cou_Name;
    		
    		protected String _Gu_Title;
    		
    		protected String _Gu_TitleAbbr;
    		
    		protected String _Gu_TitleFull;
    		
    		protected String _Gu_TitleSub;
    		
    		protected String _Gu_Color;
    		
    		protected String _Gu_Font;
    		
    		protected Boolean _Gu_IsError;
    		
    		protected String _Gu_ErrInfo;
    		
    		protected Boolean _Gu_IsUse;
    		
    		protected Boolean _Gu_IsShow;
    		
    		protected Boolean _Gu_IsImg;
    		
    		protected Boolean _Gu_IsHot;
    		
    		protected Boolean _Gu_IsTop;
    		
    		protected Boolean _Gu_IsRec;
    		
    		protected Boolean _Gu_IsDel;
    		
    		protected Boolean _Gu_IsVerify;
    		
    		protected String _Gu_VerifyMan;
    		
    		protected Boolean _Gu_IsOut;
    		
    		protected String _Gu_OutUrl;
    		
    		protected String _Gu_Keywords;
    		
    		protected String _Gu_Descr;
    		
    		protected String _Gu_Author;
    		
    		protected Int32? _Acc_Id;
    		
    		protected String _Acc_Name;
    		
    		protected String _Gu_Source;
    		
    		protected String _Gu_Intro;
    		
    		protected String _Gu_Details;
    		
    		protected String _Gu_Endnote;
    		
    		protected DateTime? _Gu_CrtTime;
    		
    		protected DateTime? _Gu_LastTime;
    		
    		protected DateTime? _Gu_VerifyTime;
    		
    		protected Int32 _Gu_Number;
    		
    		protected Boolean _Gu_IsNote;
    		
    		protected String _Gu_Logo;
    		
    		protected Boolean _Gu_IsStatic;
    		
    		protected DateTime? _Gu_PushTime;
    		
    		protected String _Gu_Label;
    		
    		protected String _Gu_Uid;
    		
    		protected String _OtherData;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		public Int32 Gu_Id {
    			get {
    				return this._Gu_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Id, _Gu_Id, value);
    				this._Gu_Id = value;
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
    		
    		public Int32? Gc_ID {
    			get {
    				return this._Gc_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gc_ID, _Gc_ID, value);
    				this._Gc_ID = value;
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
    		
    		public String Gu_Title {
    			get {
    				return this._Gu_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Title, _Gu_Title, value);
    				this._Gu_Title = value;
    			}
    		}
    		
    		public String Gu_TitleAbbr {
    			get {
    				return this._Gu_TitleAbbr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_TitleAbbr, _Gu_TitleAbbr, value);
    				this._Gu_TitleAbbr = value;
    			}
    		}
    		
    		public String Gu_TitleFull {
    			get {
    				return this._Gu_TitleFull;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_TitleFull, _Gu_TitleFull, value);
    				this._Gu_TitleFull = value;
    			}
    		}
    		
    		public String Gu_TitleSub {
    			get {
    				return this._Gu_TitleSub;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_TitleSub, _Gu_TitleSub, value);
    				this._Gu_TitleSub = value;
    			}
    		}
    		
    		public String Gu_Color {
    			get {
    				return this._Gu_Color;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Color, _Gu_Color, value);
    				this._Gu_Color = value;
    			}
    		}
    		
    		public String Gu_Font {
    			get {
    				return this._Gu_Font;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Font, _Gu_Font, value);
    				this._Gu_Font = value;
    			}
    		}
    		
    		public Boolean Gu_IsError {
    			get {
    				return this._Gu_IsError;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_IsError, _Gu_IsError, value);
    				this._Gu_IsError = value;
    			}
    		}
    		
    		public String Gu_ErrInfo {
    			get {
    				return this._Gu_ErrInfo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_ErrInfo, _Gu_ErrInfo, value);
    				this._Gu_ErrInfo = value;
    			}
    		}
    		
    		public Boolean Gu_IsUse {
    			get {
    				return this._Gu_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_IsUse, _Gu_IsUse, value);
    				this._Gu_IsUse = value;
    			}
    		}
    		
    		public Boolean Gu_IsShow {
    			get {
    				return this._Gu_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_IsShow, _Gu_IsShow, value);
    				this._Gu_IsShow = value;
    			}
    		}
    		
    		public Boolean Gu_IsImg {
    			get {
    				return this._Gu_IsImg;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_IsImg, _Gu_IsImg, value);
    				this._Gu_IsImg = value;
    			}
    		}
    		
    		public Boolean Gu_IsHot {
    			get {
    				return this._Gu_IsHot;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_IsHot, _Gu_IsHot, value);
    				this._Gu_IsHot = value;
    			}
    		}
    		
    		public Boolean Gu_IsTop {
    			get {
    				return this._Gu_IsTop;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_IsTop, _Gu_IsTop, value);
    				this._Gu_IsTop = value;
    			}
    		}
    		
    		public Boolean Gu_IsRec {
    			get {
    				return this._Gu_IsRec;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_IsRec, _Gu_IsRec, value);
    				this._Gu_IsRec = value;
    			}
    		}
    		
    		public Boolean Gu_IsDel {
    			get {
    				return this._Gu_IsDel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_IsDel, _Gu_IsDel, value);
    				this._Gu_IsDel = value;
    			}
    		}
    		
    		public Boolean Gu_IsVerify {
    			get {
    				return this._Gu_IsVerify;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_IsVerify, _Gu_IsVerify, value);
    				this._Gu_IsVerify = value;
    			}
    		}
    		
    		public String Gu_VerifyMan {
    			get {
    				return this._Gu_VerifyMan;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_VerifyMan, _Gu_VerifyMan, value);
    				this._Gu_VerifyMan = value;
    			}
    		}
    		
    		public Boolean Gu_IsOut {
    			get {
    				return this._Gu_IsOut;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_IsOut, _Gu_IsOut, value);
    				this._Gu_IsOut = value;
    			}
    		}
    		
    		public String Gu_OutUrl {
    			get {
    				return this._Gu_OutUrl;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_OutUrl, _Gu_OutUrl, value);
    				this._Gu_OutUrl = value;
    			}
    		}
    		
    		public String Gu_Keywords {
    			get {
    				return this._Gu_Keywords;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Keywords, _Gu_Keywords, value);
    				this._Gu_Keywords = value;
    			}
    		}
    		
    		public String Gu_Descr {
    			get {
    				return this._Gu_Descr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Descr, _Gu_Descr, value);
    				this._Gu_Descr = value;
    			}
    		}
    		
    		public String Gu_Author {
    			get {
    				return this._Gu_Author;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Author, _Gu_Author, value);
    				this._Gu_Author = value;
    			}
    		}
    		
    		public Int32? Acc_Id {
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
    		
    		public String Gu_Source {
    			get {
    				return this._Gu_Source;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Source, _Gu_Source, value);
    				this._Gu_Source = value;
    			}
    		}
    		
    		public String Gu_Intro {
    			get {
    				return this._Gu_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Intro, _Gu_Intro, value);
    				this._Gu_Intro = value;
    			}
    		}
    		
    		public String Gu_Details {
    			get {
    				return this._Gu_Details;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Details, _Gu_Details, value);
    				this._Gu_Details = value;
    			}
    		}
    		
    		public String Gu_Endnote {
    			get {
    				return this._Gu_Endnote;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Endnote, _Gu_Endnote, value);
    				this._Gu_Endnote = value;
    			}
    		}
    		
    		public DateTime? Gu_CrtTime {
    			get {
    				return this._Gu_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_CrtTime, _Gu_CrtTime, value);
    				this._Gu_CrtTime = value;
    			}
    		}
    		
    		public DateTime? Gu_LastTime {
    			get {
    				return this._Gu_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_LastTime, _Gu_LastTime, value);
    				this._Gu_LastTime = value;
    			}
    		}
    		
    		public DateTime? Gu_VerifyTime {
    			get {
    				return this._Gu_VerifyTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_VerifyTime, _Gu_VerifyTime, value);
    				this._Gu_VerifyTime = value;
    			}
    		}
    		
    		public Int32 Gu_Number {
    			get {
    				return this._Gu_Number;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Number, _Gu_Number, value);
    				this._Gu_Number = value;
    			}
    		}
    		
    		public Boolean Gu_IsNote {
    			get {
    				return this._Gu_IsNote;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_IsNote, _Gu_IsNote, value);
    				this._Gu_IsNote = value;
    			}
    		}
    		
    		public String Gu_Logo {
    			get {
    				return this._Gu_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Logo, _Gu_Logo, value);
    				this._Gu_Logo = value;
    			}
    		}
    		
    		public Boolean Gu_IsStatic {
    			get {
    				return this._Gu_IsStatic;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_IsStatic, _Gu_IsStatic, value);
    				this._Gu_IsStatic = value;
    			}
    		}
    		
    		public DateTime? Gu_PushTime {
    			get {
    				return this._Gu_PushTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_PushTime, _Gu_PushTime, value);
    				this._Gu_PushTime = value;
    			}
    		}
    		
    		public String Gu_Label {
    			get {
    				return this._Gu_Label;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Label, _Gu_Label, value);
    				this._Gu_Label = value;
    			}
    		}
    		
    		public String Gu_Uid {
    			get {
    				return this._Gu_Uid;
    			}
    			set {
    				this.OnPropertyValueChange(_.Gu_Uid, _Gu_Uid, value);
    				this._Gu_Uid = value;
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
    			return new WeiSha.Data.Table<Guide>("Guide");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Gu_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Gu_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Gu_Id,
    					_.Gc_Title,
    					_.Gc_ID,
    					_.Cou_ID,
    					_.Cou_Name,
    					_.Gu_Title,
    					_.Gu_TitleAbbr,
    					_.Gu_TitleFull,
    					_.Gu_TitleSub,
    					_.Gu_Color,
    					_.Gu_Font,
    					_.Gu_IsError,
    					_.Gu_ErrInfo,
    					_.Gu_IsUse,
    					_.Gu_IsShow,
    					_.Gu_IsImg,
    					_.Gu_IsHot,
    					_.Gu_IsTop,
    					_.Gu_IsRec,
    					_.Gu_IsDel,
    					_.Gu_IsVerify,
    					_.Gu_VerifyMan,
    					_.Gu_IsOut,
    					_.Gu_OutUrl,
    					_.Gu_Keywords,
    					_.Gu_Descr,
    					_.Gu_Author,
    					_.Acc_Id,
    					_.Acc_Name,
    					_.Gu_Source,
    					_.Gu_Intro,
    					_.Gu_Details,
    					_.Gu_Endnote,
    					_.Gu_CrtTime,
    					_.Gu_LastTime,
    					_.Gu_VerifyTime,
    					_.Gu_Number,
    					_.Gu_IsNote,
    					_.Gu_Logo,
    					_.Gu_IsStatic,
    					_.Gu_PushTime,
    					_.Gu_Label,
    					_.Gu_Uid,
    					_.OtherData,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Gu_Id,
    					this._Gc_Title,
    					this._Gc_ID,
    					this._Cou_ID,
    					this._Cou_Name,
    					this._Gu_Title,
    					this._Gu_TitleAbbr,
    					this._Gu_TitleFull,
    					this._Gu_TitleSub,
    					this._Gu_Color,
    					this._Gu_Font,
    					this._Gu_IsError,
    					this._Gu_ErrInfo,
    					this._Gu_IsUse,
    					this._Gu_IsShow,
    					this._Gu_IsImg,
    					this._Gu_IsHot,
    					this._Gu_IsTop,
    					this._Gu_IsRec,
    					this._Gu_IsDel,
    					this._Gu_IsVerify,
    					this._Gu_VerifyMan,
    					this._Gu_IsOut,
    					this._Gu_OutUrl,
    					this._Gu_Keywords,
    					this._Gu_Descr,
    					this._Gu_Author,
    					this._Acc_Id,
    					this._Acc_Name,
    					this._Gu_Source,
    					this._Gu_Intro,
    					this._Gu_Details,
    					this._Gu_Endnote,
    					this._Gu_CrtTime,
    					this._Gu_LastTime,
    					this._Gu_VerifyTime,
    					this._Gu_Number,
    					this._Gu_IsNote,
    					this._Gu_Logo,
    					this._Gu_IsStatic,
    					this._Gu_PushTime,
    					this._Gu_Label,
    					this._Gu_Uid,
    					this._OtherData,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Gu_Id))) {
    				this._Gu_Id = reader.GetInt32(_.Gu_Id);
    			}
    			if ((false == reader.IsDBNull(_.Gc_Title))) {
    				this._Gc_Title = reader.GetString(_.Gc_Title);
    			}
    			if ((false == reader.IsDBNull(_.Gc_ID))) {
    				this._Gc_ID = reader.GetInt32(_.Gc_ID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_Name))) {
    				this._Cou_Name = reader.GetString(_.Cou_Name);
    			}
    			if ((false == reader.IsDBNull(_.Gu_Title))) {
    				this._Gu_Title = reader.GetString(_.Gu_Title);
    			}
    			if ((false == reader.IsDBNull(_.Gu_TitleAbbr))) {
    				this._Gu_TitleAbbr = reader.GetString(_.Gu_TitleAbbr);
    			}
    			if ((false == reader.IsDBNull(_.Gu_TitleFull))) {
    				this._Gu_TitleFull = reader.GetString(_.Gu_TitleFull);
    			}
    			if ((false == reader.IsDBNull(_.Gu_TitleSub))) {
    				this._Gu_TitleSub = reader.GetString(_.Gu_TitleSub);
    			}
    			if ((false == reader.IsDBNull(_.Gu_Color))) {
    				this._Gu_Color = reader.GetString(_.Gu_Color);
    			}
    			if ((false == reader.IsDBNull(_.Gu_Font))) {
    				this._Gu_Font = reader.GetString(_.Gu_Font);
    			}
    			if ((false == reader.IsDBNull(_.Gu_IsError))) {
    				this._Gu_IsError = reader.GetBoolean(_.Gu_IsError);
    			}
    			if ((false == reader.IsDBNull(_.Gu_ErrInfo))) {
    				this._Gu_ErrInfo = reader.GetString(_.Gu_ErrInfo);
    			}
    			if ((false == reader.IsDBNull(_.Gu_IsUse))) {
    				this._Gu_IsUse = reader.GetBoolean(_.Gu_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Gu_IsShow))) {
    				this._Gu_IsShow = reader.GetBoolean(_.Gu_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Gu_IsImg))) {
    				this._Gu_IsImg = reader.GetBoolean(_.Gu_IsImg);
    			}
    			if ((false == reader.IsDBNull(_.Gu_IsHot))) {
    				this._Gu_IsHot = reader.GetBoolean(_.Gu_IsHot);
    			}
    			if ((false == reader.IsDBNull(_.Gu_IsTop))) {
    				this._Gu_IsTop = reader.GetBoolean(_.Gu_IsTop);
    			}
    			if ((false == reader.IsDBNull(_.Gu_IsRec))) {
    				this._Gu_IsRec = reader.GetBoolean(_.Gu_IsRec);
    			}
    			if ((false == reader.IsDBNull(_.Gu_IsDel))) {
    				this._Gu_IsDel = reader.GetBoolean(_.Gu_IsDel);
    			}
    			if ((false == reader.IsDBNull(_.Gu_IsVerify))) {
    				this._Gu_IsVerify = reader.GetBoolean(_.Gu_IsVerify);
    			}
    			if ((false == reader.IsDBNull(_.Gu_VerifyMan))) {
    				this._Gu_VerifyMan = reader.GetString(_.Gu_VerifyMan);
    			}
    			if ((false == reader.IsDBNull(_.Gu_IsOut))) {
    				this._Gu_IsOut = reader.GetBoolean(_.Gu_IsOut);
    			}
    			if ((false == reader.IsDBNull(_.Gu_OutUrl))) {
    				this._Gu_OutUrl = reader.GetString(_.Gu_OutUrl);
    			}
    			if ((false == reader.IsDBNull(_.Gu_Keywords))) {
    				this._Gu_Keywords = reader.GetString(_.Gu_Keywords);
    			}
    			if ((false == reader.IsDBNull(_.Gu_Descr))) {
    				this._Gu_Descr = reader.GetString(_.Gu_Descr);
    			}
    			if ((false == reader.IsDBNull(_.Gu_Author))) {
    				this._Gu_Author = reader.GetString(_.Gu_Author);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Name))) {
    				this._Acc_Name = reader.GetString(_.Acc_Name);
    			}
    			if ((false == reader.IsDBNull(_.Gu_Source))) {
    				this._Gu_Source = reader.GetString(_.Gu_Source);
    			}
    			if ((false == reader.IsDBNull(_.Gu_Intro))) {
    				this._Gu_Intro = reader.GetString(_.Gu_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Gu_Details))) {
    				this._Gu_Details = reader.GetString(_.Gu_Details);
    			}
    			if ((false == reader.IsDBNull(_.Gu_Endnote))) {
    				this._Gu_Endnote = reader.GetString(_.Gu_Endnote);
    			}
    			if ((false == reader.IsDBNull(_.Gu_CrtTime))) {
    				this._Gu_CrtTime = reader.GetDateTime(_.Gu_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Gu_LastTime))) {
    				this._Gu_LastTime = reader.GetDateTime(_.Gu_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Gu_VerifyTime))) {
    				this._Gu_VerifyTime = reader.GetDateTime(_.Gu_VerifyTime);
    			}
    			if ((false == reader.IsDBNull(_.Gu_Number))) {
    				this._Gu_Number = reader.GetInt32(_.Gu_Number);
    			}
    			if ((false == reader.IsDBNull(_.Gu_IsNote))) {
    				this._Gu_IsNote = reader.GetBoolean(_.Gu_IsNote);
    			}
    			if ((false == reader.IsDBNull(_.Gu_Logo))) {
    				this._Gu_Logo = reader.GetString(_.Gu_Logo);
    			}
    			if ((false == reader.IsDBNull(_.Gu_IsStatic))) {
    				this._Gu_IsStatic = reader.GetBoolean(_.Gu_IsStatic);
    			}
    			if ((false == reader.IsDBNull(_.Gu_PushTime))) {
    				this._Gu_PushTime = reader.GetDateTime(_.Gu_PushTime);
    			}
    			if ((false == reader.IsDBNull(_.Gu_Label))) {
    				this._Gu_Label = reader.GetString(_.Gu_Label);
    			}
    			if ((false == reader.IsDBNull(_.Gu_Uid))) {
    				this._Gu_Uid = reader.GetString(_.Gu_Uid);
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
    			if ((false == typeof(Guide).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Guide>();
    			
    			/// <summary>
    			/// 字段名：Gu_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Id = new WeiSha.Data.Field<Guide>("Gu_Id");
    			
    			/// <summary>
    			/// 字段名：Gc_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gc_Title = new WeiSha.Data.Field<Guide>("Gc_Title");
    			
    			/// <summary>
    			/// 字段名：Gc_ID - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Gc_ID = new WeiSha.Data.Field<Guide>("Gc_ID");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<Guide>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Cou_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Cou_Name = new WeiSha.Data.Field<Guide>("Cou_Name");
    			
    			/// <summary>
    			/// 字段名：Gu_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Title = new WeiSha.Data.Field<Guide>("Gu_Title");
    			
    			/// <summary>
    			/// 字段名：Gu_TitleAbbr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_TitleAbbr = new WeiSha.Data.Field<Guide>("Gu_TitleAbbr");
    			
    			/// <summary>
    			/// 字段名：Gu_TitleFull - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_TitleFull = new WeiSha.Data.Field<Guide>("Gu_TitleFull");
    			
    			/// <summary>
    			/// 字段名：Gu_TitleSub - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_TitleSub = new WeiSha.Data.Field<Guide>("Gu_TitleSub");
    			
    			/// <summary>
    			/// 字段名：Gu_Color - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Color = new WeiSha.Data.Field<Guide>("Gu_Color");
    			
    			/// <summary>
    			/// 字段名：Gu_Font - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Font = new WeiSha.Data.Field<Guide>("Gu_Font");
    			
    			/// <summary>
    			/// 字段名：Gu_IsError - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Gu_IsError = new WeiSha.Data.Field<Guide>("Gu_IsError");
    			
    			/// <summary>
    			/// 字段名：Gu_ErrInfo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_ErrInfo = new WeiSha.Data.Field<Guide>("Gu_ErrInfo");
    			
    			/// <summary>
    			/// 字段名：Gu_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Gu_IsUse = new WeiSha.Data.Field<Guide>("Gu_IsUse");
    			
    			/// <summary>
    			/// 字段名：Gu_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Gu_IsShow = new WeiSha.Data.Field<Guide>("Gu_IsShow");
    			
    			/// <summary>
    			/// 字段名：Gu_IsImg - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Gu_IsImg = new WeiSha.Data.Field<Guide>("Gu_IsImg");
    			
    			/// <summary>
    			/// 字段名：Gu_IsHot - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Gu_IsHot = new WeiSha.Data.Field<Guide>("Gu_IsHot");
    			
    			/// <summary>
    			/// 字段名：Gu_IsTop - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Gu_IsTop = new WeiSha.Data.Field<Guide>("Gu_IsTop");
    			
    			/// <summary>
    			/// 字段名：Gu_IsRec - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Gu_IsRec = new WeiSha.Data.Field<Guide>("Gu_IsRec");
    			
    			/// <summary>
    			/// 字段名：Gu_IsDel - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Gu_IsDel = new WeiSha.Data.Field<Guide>("Gu_IsDel");
    			
    			/// <summary>
    			/// 字段名：Gu_IsVerify - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Gu_IsVerify = new WeiSha.Data.Field<Guide>("Gu_IsVerify");
    			
    			/// <summary>
    			/// 字段名：Gu_VerifyMan - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_VerifyMan = new WeiSha.Data.Field<Guide>("Gu_VerifyMan");
    			
    			/// <summary>
    			/// 字段名：Gu_IsOut - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Gu_IsOut = new WeiSha.Data.Field<Guide>("Gu_IsOut");
    			
    			/// <summary>
    			/// 字段名：Gu_OutUrl - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_OutUrl = new WeiSha.Data.Field<Guide>("Gu_OutUrl");
    			
    			/// <summary>
    			/// 字段名：Gu_Keywords - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Keywords = new WeiSha.Data.Field<Guide>("Gu_Keywords");
    			
    			/// <summary>
    			/// 字段名：Gu_Descr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Descr = new WeiSha.Data.Field<Guide>("Gu_Descr");
    			
    			/// <summary>
    			/// 字段名：Gu_Author - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Author = new WeiSha.Data.Field<Guide>("Gu_Author");
    			
    			/// <summary>
    			/// 字段名：Acc_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<Guide>("Acc_Id");
    			
    			/// <summary>
    			/// 字段名：Acc_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Name = new WeiSha.Data.Field<Guide>("Acc_Name");
    			
    			/// <summary>
    			/// 字段名：Gu_Source - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Source = new WeiSha.Data.Field<Guide>("Gu_Source");
    			
    			/// <summary>
    			/// 字段名：Gu_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Intro = new WeiSha.Data.Field<Guide>("Gu_Intro");
    			
    			/// <summary>
    			/// 字段名：Gu_Details - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Details = new WeiSha.Data.Field<Guide>("Gu_Details");
    			
    			/// <summary>
    			/// 字段名：Gu_Endnote - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Endnote = new WeiSha.Data.Field<Guide>("Gu_Endnote");
    			
    			/// <summary>
    			/// 字段名：Gu_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Gu_CrtTime = new WeiSha.Data.Field<Guide>("Gu_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Gu_LastTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Gu_LastTime = new WeiSha.Data.Field<Guide>("Gu_LastTime");
    			
    			/// <summary>
    			/// 字段名：Gu_VerifyTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Gu_VerifyTime = new WeiSha.Data.Field<Guide>("Gu_VerifyTime");
    			
    			/// <summary>
    			/// 字段名：Gu_Number - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Number = new WeiSha.Data.Field<Guide>("Gu_Number");
    			
    			/// <summary>
    			/// 字段名：Gu_IsNote - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Gu_IsNote = new WeiSha.Data.Field<Guide>("Gu_IsNote");
    			
    			/// <summary>
    			/// 字段名：Gu_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Logo = new WeiSha.Data.Field<Guide>("Gu_Logo");
    			
    			/// <summary>
    			/// 字段名：Gu_IsStatic - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Gu_IsStatic = new WeiSha.Data.Field<Guide>("Gu_IsStatic");
    			
    			/// <summary>
    			/// 字段名：Gu_PushTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Gu_PushTime = new WeiSha.Data.Field<Guide>("Gu_PushTime");
    			
    			/// <summary>
    			/// 字段名：Gu_Label - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Label = new WeiSha.Data.Field<Guide>("Gu_Label");
    			
    			/// <summary>
    			/// 字段名：Gu_Uid - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Gu_Uid = new WeiSha.Data.Field<Guide>("Gu_Uid");
    			
    			/// <summary>
    			/// 字段名：OtherData - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field OtherData = new WeiSha.Data.Field<Guide>("OtherData");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Guide>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Guide>("Org_Name");
    		}
    	}
    }
    