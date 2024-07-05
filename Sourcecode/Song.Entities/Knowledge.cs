namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Knowledge 主键列：Kn_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Knowledge : WeiSha.Data.Entity {
    		
    		protected Int64 _Kn_ID;
    		
    		protected Int64 _Cou_ID;
    		
    		protected String _Kn_Author;
    		
    		protected DateTime? _Kn_CrtTime;
    		
    		protected String _Kn_Descr;
    		
    		protected String _Kn_Details;
    		
    		protected String _Kn_Intro;
    		
    		protected Boolean _Kn_IsDel;
    		
    		protected Boolean _Kn_IsHot;
    		
    		protected Boolean _Kn_IsNote;
    		
    		protected Boolean _Kn_IsRec;
    		
    		protected Boolean _Kn_IsTop;
    		
    		protected Boolean _Kn_IsUse;
    		
    		protected String _Kn_Keywords;
    		
    		protected String _Kn_Label;
    		
    		protected DateTime? _Kn_LastTime;
    		
    		protected String _Kn_Logo;
    		
    		protected Int32 _Kn_Number;
    		
    		protected String _Kn_Source;
    		
    		protected String _Kn_Title;
    		
    		protected String _Kn_TitleFull;
    		
    		protected String _Kn_TitleSub;
    		
    		protected String _Kn_Uid;
    		
    		protected Int64 _Kns_ID;
    		
    		protected String _Kns_Name;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected String _OtherData;
    		
    		protected Int32 _Th_ID;
    		
    		protected String _Th_Name;
    		
    		public Int64 Kn_ID {
    			get {
    				return this._Kn_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_ID, _Kn_ID, value);
    				this._Kn_ID = value;
    			}
    		}
    		
    		public Int64 Cou_ID {
    			get {
    				return this._Cou_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ID, _Cou_ID, value);
    				this._Cou_ID = value;
    			}
    		}
    		
    		public String Kn_Author {
    			get {
    				return this._Kn_Author;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_Author, _Kn_Author, value);
    				this._Kn_Author = value;
    			}
    		}
    		
    		public DateTime? Kn_CrtTime {
    			get {
    				return this._Kn_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_CrtTime, _Kn_CrtTime, value);
    				this._Kn_CrtTime = value;
    			}
    		}
    		
    		public String Kn_Descr {
    			get {
    				return this._Kn_Descr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_Descr, _Kn_Descr, value);
    				this._Kn_Descr = value;
    			}
    		}
    		
    		public String Kn_Details {
    			get {
    				return this._Kn_Details;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_Details, _Kn_Details, value);
    				this._Kn_Details = value;
    			}
    		}
    		
    		public String Kn_Intro {
    			get {
    				return this._Kn_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_Intro, _Kn_Intro, value);
    				this._Kn_Intro = value;
    			}
    		}
    		
    		public Boolean Kn_IsDel {
    			get {
    				return this._Kn_IsDel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_IsDel, _Kn_IsDel, value);
    				this._Kn_IsDel = value;
    			}
    		}
    		
    		public Boolean Kn_IsHot {
    			get {
    				return this._Kn_IsHot;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_IsHot, _Kn_IsHot, value);
    				this._Kn_IsHot = value;
    			}
    		}
    		
    		public Boolean Kn_IsNote {
    			get {
    				return this._Kn_IsNote;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_IsNote, _Kn_IsNote, value);
    				this._Kn_IsNote = value;
    			}
    		}
    		
    		public Boolean Kn_IsRec {
    			get {
    				return this._Kn_IsRec;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_IsRec, _Kn_IsRec, value);
    				this._Kn_IsRec = value;
    			}
    		}
    		
    		public Boolean Kn_IsTop {
    			get {
    				return this._Kn_IsTop;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_IsTop, _Kn_IsTop, value);
    				this._Kn_IsTop = value;
    			}
    		}
    		
    		public Boolean Kn_IsUse {
    			get {
    				return this._Kn_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_IsUse, _Kn_IsUse, value);
    				this._Kn_IsUse = value;
    			}
    		}
    		
    		public String Kn_Keywords {
    			get {
    				return this._Kn_Keywords;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_Keywords, _Kn_Keywords, value);
    				this._Kn_Keywords = value;
    			}
    		}
    		
    		public String Kn_Label {
    			get {
    				return this._Kn_Label;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_Label, _Kn_Label, value);
    				this._Kn_Label = value;
    			}
    		}
    		
    		public DateTime? Kn_LastTime {
    			get {
    				return this._Kn_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_LastTime, _Kn_LastTime, value);
    				this._Kn_LastTime = value;
    			}
    		}
    		
    		public String Kn_Logo {
    			get {
    				return this._Kn_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_Logo, _Kn_Logo, value);
    				this._Kn_Logo = value;
    			}
    		}
    		
    		public Int32 Kn_Number {
    			get {
    				return this._Kn_Number;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_Number, _Kn_Number, value);
    				this._Kn_Number = value;
    			}
    		}
    		
    		public String Kn_Source {
    			get {
    				return this._Kn_Source;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_Source, _Kn_Source, value);
    				this._Kn_Source = value;
    			}
    		}
    		
    		public String Kn_Title {
    			get {
    				return this._Kn_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_Title, _Kn_Title, value);
    				this._Kn_Title = value;
    			}
    		}
    		
    		public String Kn_TitleFull {
    			get {
    				return this._Kn_TitleFull;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_TitleFull, _Kn_TitleFull, value);
    				this._Kn_TitleFull = value;
    			}
    		}
    		
    		public String Kn_TitleSub {
    			get {
    				return this._Kn_TitleSub;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_TitleSub, _Kn_TitleSub, value);
    				this._Kn_TitleSub = value;
    			}
    		}
    		
    		public String Kn_Uid {
    			get {
    				return this._Kn_Uid;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kn_Uid, _Kn_Uid, value);
    				this._Kn_Uid = value;
    			}
    		}
    		
    		public Int64 Kns_ID {
    			get {
    				return this._Kns_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kns_ID, _Kns_ID, value);
    				this._Kns_ID = value;
    			}
    		}
    		
    		public String Kns_Name {
    			get {
    				return this._Kns_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Kns_Name, _Kns_Name, value);
    				this._Kns_Name = value;
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
    		
    		public Int32 Th_ID {
    			get {
    				return this._Th_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_ID, _Th_ID, value);
    				this._Th_ID = value;
    			}
    		}
    		
    		public String Th_Name {
    			get {
    				return this._Th_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_Name, _Th_Name, value);
    				this._Th_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Knowledge>("Knowledge");
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Kn_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Kn_ID,
    					_.Cou_ID,
    					_.Kn_Author,
    					_.Kn_CrtTime,
    					_.Kn_Descr,
    					_.Kn_Details,
    					_.Kn_Intro,
    					_.Kn_IsDel,
    					_.Kn_IsHot,
    					_.Kn_IsNote,
    					_.Kn_IsRec,
    					_.Kn_IsTop,
    					_.Kn_IsUse,
    					_.Kn_Keywords,
    					_.Kn_Label,
    					_.Kn_LastTime,
    					_.Kn_Logo,
    					_.Kn_Number,
    					_.Kn_Source,
    					_.Kn_Title,
    					_.Kn_TitleFull,
    					_.Kn_TitleSub,
    					_.Kn_Uid,
    					_.Kns_ID,
    					_.Kns_Name,
    					_.Org_ID,
    					_.Org_Name,
    					_.OtherData,
    					_.Th_ID,
    					_.Th_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Kn_ID,
    					this._Cou_ID,
    					this._Kn_Author,
    					this._Kn_CrtTime,
    					this._Kn_Descr,
    					this._Kn_Details,
    					this._Kn_Intro,
    					this._Kn_IsDel,
    					this._Kn_IsHot,
    					this._Kn_IsNote,
    					this._Kn_IsRec,
    					this._Kn_IsTop,
    					this._Kn_IsUse,
    					this._Kn_Keywords,
    					this._Kn_Label,
    					this._Kn_LastTime,
    					this._Kn_Logo,
    					this._Kn_Number,
    					this._Kn_Source,
    					this._Kn_Title,
    					this._Kn_TitleFull,
    					this._Kn_TitleSub,
    					this._Kn_Uid,
    					this._Kns_ID,
    					this._Kns_Name,
    					this._Org_ID,
    					this._Org_Name,
    					this._OtherData,
    					this._Th_ID,
    					this._Th_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Kn_ID))) {
    				this._Kn_ID = reader.GetInt64(_.Kn_ID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt64(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Kn_Author))) {
    				this._Kn_Author = reader.GetString(_.Kn_Author);
    			}
    			if ((false == reader.IsDBNull(_.Kn_CrtTime))) {
    				this._Kn_CrtTime = reader.GetDateTime(_.Kn_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Kn_Descr))) {
    				this._Kn_Descr = reader.GetString(_.Kn_Descr);
    			}
    			if ((false == reader.IsDBNull(_.Kn_Details))) {
    				this._Kn_Details = reader.GetString(_.Kn_Details);
    			}
    			if ((false == reader.IsDBNull(_.Kn_Intro))) {
    				this._Kn_Intro = reader.GetString(_.Kn_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Kn_IsDel))) {
    				this._Kn_IsDel = reader.GetBoolean(_.Kn_IsDel);
    			}
    			if ((false == reader.IsDBNull(_.Kn_IsHot))) {
    				this._Kn_IsHot = reader.GetBoolean(_.Kn_IsHot);
    			}
    			if ((false == reader.IsDBNull(_.Kn_IsNote))) {
    				this._Kn_IsNote = reader.GetBoolean(_.Kn_IsNote);
    			}
    			if ((false == reader.IsDBNull(_.Kn_IsRec))) {
    				this._Kn_IsRec = reader.GetBoolean(_.Kn_IsRec);
    			}
    			if ((false == reader.IsDBNull(_.Kn_IsTop))) {
    				this._Kn_IsTop = reader.GetBoolean(_.Kn_IsTop);
    			}
    			if ((false == reader.IsDBNull(_.Kn_IsUse))) {
    				this._Kn_IsUse = reader.GetBoolean(_.Kn_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Kn_Keywords))) {
    				this._Kn_Keywords = reader.GetString(_.Kn_Keywords);
    			}
    			if ((false == reader.IsDBNull(_.Kn_Label))) {
    				this._Kn_Label = reader.GetString(_.Kn_Label);
    			}
    			if ((false == reader.IsDBNull(_.Kn_LastTime))) {
    				this._Kn_LastTime = reader.GetDateTime(_.Kn_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Kn_Logo))) {
    				this._Kn_Logo = reader.GetString(_.Kn_Logo);
    			}
    			if ((false == reader.IsDBNull(_.Kn_Number))) {
    				this._Kn_Number = reader.GetInt32(_.Kn_Number);
    			}
    			if ((false == reader.IsDBNull(_.Kn_Source))) {
    				this._Kn_Source = reader.GetString(_.Kn_Source);
    			}
    			if ((false == reader.IsDBNull(_.Kn_Title))) {
    				this._Kn_Title = reader.GetString(_.Kn_Title);
    			}
    			if ((false == reader.IsDBNull(_.Kn_TitleFull))) {
    				this._Kn_TitleFull = reader.GetString(_.Kn_TitleFull);
    			}
    			if ((false == reader.IsDBNull(_.Kn_TitleSub))) {
    				this._Kn_TitleSub = reader.GetString(_.Kn_TitleSub);
    			}
    			if ((false == reader.IsDBNull(_.Kn_Uid))) {
    				this._Kn_Uid = reader.GetString(_.Kn_Uid);
    			}
    			if ((false == reader.IsDBNull(_.Kns_ID))) {
    				this._Kns_ID = reader.GetInt64(_.Kns_ID);
    			}
    			if ((false == reader.IsDBNull(_.Kns_Name))) {
    				this._Kns_Name = reader.GetString(_.Kns_Name);
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
    			if ((false == reader.IsDBNull(_.Th_ID))) {
    				this._Th_ID = reader.GetInt32(_.Th_ID);
    			}
    			if ((false == reader.IsDBNull(_.Th_Name))) {
    				this._Th_Name = reader.GetString(_.Th_Name);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Knowledge).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Knowledge>();
    			
    			/// <summary>
    			/// 字段名：Kn_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Kn_ID = new WeiSha.Data.Field<Knowledge>("Kn_ID");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<Knowledge>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Kn_Author - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kn_Author = new WeiSha.Data.Field<Knowledge>("Kn_Author");
    			
    			/// <summary>
    			/// 字段名：Kn_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Kn_CrtTime = new WeiSha.Data.Field<Knowledge>("Kn_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Kn_Descr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kn_Descr = new WeiSha.Data.Field<Knowledge>("Kn_Descr");
    			
    			/// <summary>
    			/// 字段名：Kn_Details - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kn_Details = new WeiSha.Data.Field<Knowledge>("Kn_Details");
    			
    			/// <summary>
    			/// 字段名：Kn_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kn_Intro = new WeiSha.Data.Field<Knowledge>("Kn_Intro");
    			
    			/// <summary>
    			/// 字段名：Kn_IsDel - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Kn_IsDel = new WeiSha.Data.Field<Knowledge>("Kn_IsDel");
    			
    			/// <summary>
    			/// 字段名：Kn_IsHot - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Kn_IsHot = new WeiSha.Data.Field<Knowledge>("Kn_IsHot");
    			
    			/// <summary>
    			/// 字段名：Kn_IsNote - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Kn_IsNote = new WeiSha.Data.Field<Knowledge>("Kn_IsNote");
    			
    			/// <summary>
    			/// 字段名：Kn_IsRec - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Kn_IsRec = new WeiSha.Data.Field<Knowledge>("Kn_IsRec");
    			
    			/// <summary>
    			/// 字段名：Kn_IsTop - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Kn_IsTop = new WeiSha.Data.Field<Knowledge>("Kn_IsTop");
    			
    			/// <summary>
    			/// 字段名：Kn_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Kn_IsUse = new WeiSha.Data.Field<Knowledge>("Kn_IsUse");
    			
    			/// <summary>
    			/// 字段名：Kn_Keywords - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kn_Keywords = new WeiSha.Data.Field<Knowledge>("Kn_Keywords");
    			
    			/// <summary>
    			/// 字段名：Kn_Label - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kn_Label = new WeiSha.Data.Field<Knowledge>("Kn_Label");
    			
    			/// <summary>
    			/// 字段名：Kn_LastTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Kn_LastTime = new WeiSha.Data.Field<Knowledge>("Kn_LastTime");
    			
    			/// <summary>
    			/// 字段名：Kn_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kn_Logo = new WeiSha.Data.Field<Knowledge>("Kn_Logo");
    			
    			/// <summary>
    			/// 字段名：Kn_Number - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Kn_Number = new WeiSha.Data.Field<Knowledge>("Kn_Number");
    			
    			/// <summary>
    			/// 字段名：Kn_Source - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kn_Source = new WeiSha.Data.Field<Knowledge>("Kn_Source");
    			
    			/// <summary>
    			/// 字段名：Kn_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kn_Title = new WeiSha.Data.Field<Knowledge>("Kn_Title");
    			
    			/// <summary>
    			/// 字段名：Kn_TitleFull - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kn_TitleFull = new WeiSha.Data.Field<Knowledge>("Kn_TitleFull");
    			
    			/// <summary>
    			/// 字段名：Kn_TitleSub - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kn_TitleSub = new WeiSha.Data.Field<Knowledge>("Kn_TitleSub");
    			
    			/// <summary>
    			/// 字段名：Kn_Uid - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kn_Uid = new WeiSha.Data.Field<Knowledge>("Kn_Uid");
    			
    			/// <summary>
    			/// 字段名：Kns_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Kns_ID = new WeiSha.Data.Field<Knowledge>("Kns_ID");
    			
    			/// <summary>
    			/// 字段名：Kns_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Kns_Name = new WeiSha.Data.Field<Knowledge>("Kns_Name");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Knowledge>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Knowledge>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：OtherData - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field OtherData = new WeiSha.Data.Field<Knowledge>("OtherData");
    			
    			/// <summary>
    			/// 字段名：Th_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_ID = new WeiSha.Data.Field<Knowledge>("Th_ID");
    			
    			/// <summary>
    			/// 字段名：Th_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Th_Name = new WeiSha.Data.Field<Knowledge>("Th_Name");
    		}
    	}
    }
    