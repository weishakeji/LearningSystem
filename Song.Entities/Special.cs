namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Special 主键列：Sp_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Special : WeiSha.Data.Entity {
    		
    		protected Int32 _Sp_Id;
    		
    		protected Int32? _Sp_PatId;
    		
    		protected Int32? _Sp_Tax;
    		
    		protected String _Sp_Name;
    		
    		protected Boolean _Sp_IsUse;
    		
    		protected Boolean _Sp_IsShow;
    		
    		protected Boolean _Sp_IsOut;
    		
    		protected String _Sp_OutUrl;
    		
    		protected String _Sp_Intro;
    		
    		protected String _Sp_Details;
    		
    		protected String _Sp_Keywords;
    		
    		protected String _Sp_Descr;
    		
    		protected DateTime? _Sp_PushTime;
    		
    		protected String _Sp_Label;
    		
    		protected String _Sp_Tootip;
    		
    		protected String _Sp_Banner;
    		
    		protected String _Sp_Uid;
    		
    		protected String _Sp_Logo;
    		
    		protected String _Sp_QrCode;
    		
    		protected String _OtherData;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Sp_Id {
    			get {
    				return this._Sp_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_Id, _Sp_Id, value);
    				this._Sp_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Sp_PatId {
    			get {
    				return this._Sp_PatId;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_PatId, _Sp_PatId, value);
    				this._Sp_PatId = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Sp_Tax {
    			get {
    				return this._Sp_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_Tax, _Sp_Tax, value);
    				this._Sp_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sp_Name {
    			get {
    				return this._Sp_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_Name, _Sp_Name, value);
    				this._Sp_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Sp_IsUse {
    			get {
    				return this._Sp_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_IsUse, _Sp_IsUse, value);
    				this._Sp_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Sp_IsShow {
    			get {
    				return this._Sp_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_IsShow, _Sp_IsShow, value);
    				this._Sp_IsShow = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Sp_IsOut {
    			get {
    				return this._Sp_IsOut;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_IsOut, _Sp_IsOut, value);
    				this._Sp_IsOut = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sp_OutUrl {
    			get {
    				return this._Sp_OutUrl;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_OutUrl, _Sp_OutUrl, value);
    				this._Sp_OutUrl = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sp_Intro {
    			get {
    				return this._Sp_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_Intro, _Sp_Intro, value);
    				this._Sp_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sp_Details {
    			get {
    				return this._Sp_Details;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_Details, _Sp_Details, value);
    				this._Sp_Details = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sp_Keywords {
    			get {
    				return this._Sp_Keywords;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_Keywords, _Sp_Keywords, value);
    				this._Sp_Keywords = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sp_Descr {
    			get {
    				return this._Sp_Descr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_Descr, _Sp_Descr, value);
    				this._Sp_Descr = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Sp_PushTime {
    			get {
    				return this._Sp_PushTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_PushTime, _Sp_PushTime, value);
    				this._Sp_PushTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sp_Label {
    			get {
    				return this._Sp_Label;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_Label, _Sp_Label, value);
    				this._Sp_Label = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sp_Tootip {
    			get {
    				return this._Sp_Tootip;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_Tootip, _Sp_Tootip, value);
    				this._Sp_Tootip = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sp_Banner {
    			get {
    				return this._Sp_Banner;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_Banner, _Sp_Banner, value);
    				this._Sp_Banner = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sp_Uid {
    			get {
    				return this._Sp_Uid;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_Uid, _Sp_Uid, value);
    				this._Sp_Uid = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sp_Logo {
    			get {
    				return this._Sp_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_Logo, _Sp_Logo, value);
    				this._Sp_Logo = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sp_QrCode {
    			get {
    				return this._Sp_QrCode;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sp_QrCode, _Sp_QrCode, value);
    				this._Sp_QrCode = value;
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
    			return new WeiSha.Data.Table<Special>("Special");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Sp_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Sp_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Sp_Id,
    					_.Sp_PatId,
    					_.Sp_Tax,
    					_.Sp_Name,
    					_.Sp_IsUse,
    					_.Sp_IsShow,
    					_.Sp_IsOut,
    					_.Sp_OutUrl,
    					_.Sp_Intro,
    					_.Sp_Details,
    					_.Sp_Keywords,
    					_.Sp_Descr,
    					_.Sp_PushTime,
    					_.Sp_Label,
    					_.Sp_Tootip,
    					_.Sp_Banner,
    					_.Sp_Uid,
    					_.Sp_Logo,
    					_.Sp_QrCode,
    					_.OtherData,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Sp_Id,
    					this._Sp_PatId,
    					this._Sp_Tax,
    					this._Sp_Name,
    					this._Sp_IsUse,
    					this._Sp_IsShow,
    					this._Sp_IsOut,
    					this._Sp_OutUrl,
    					this._Sp_Intro,
    					this._Sp_Details,
    					this._Sp_Keywords,
    					this._Sp_Descr,
    					this._Sp_PushTime,
    					this._Sp_Label,
    					this._Sp_Tootip,
    					this._Sp_Banner,
    					this._Sp_Uid,
    					this._Sp_Logo,
    					this._Sp_QrCode,
    					this._OtherData,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Sp_Id))) {
    				this._Sp_Id = reader.GetInt32(_.Sp_Id);
    			}
    			if ((false == reader.IsDBNull(_.Sp_PatId))) {
    				this._Sp_PatId = reader.GetInt32(_.Sp_PatId);
    			}
    			if ((false == reader.IsDBNull(_.Sp_Tax))) {
    				this._Sp_Tax = reader.GetInt32(_.Sp_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Sp_Name))) {
    				this._Sp_Name = reader.GetString(_.Sp_Name);
    			}
    			if ((false == reader.IsDBNull(_.Sp_IsUse))) {
    				this._Sp_IsUse = reader.GetBoolean(_.Sp_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Sp_IsShow))) {
    				this._Sp_IsShow = reader.GetBoolean(_.Sp_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Sp_IsOut))) {
    				this._Sp_IsOut = reader.GetBoolean(_.Sp_IsOut);
    			}
    			if ((false == reader.IsDBNull(_.Sp_OutUrl))) {
    				this._Sp_OutUrl = reader.GetString(_.Sp_OutUrl);
    			}
    			if ((false == reader.IsDBNull(_.Sp_Intro))) {
    				this._Sp_Intro = reader.GetString(_.Sp_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Sp_Details))) {
    				this._Sp_Details = reader.GetString(_.Sp_Details);
    			}
    			if ((false == reader.IsDBNull(_.Sp_Keywords))) {
    				this._Sp_Keywords = reader.GetString(_.Sp_Keywords);
    			}
    			if ((false == reader.IsDBNull(_.Sp_Descr))) {
    				this._Sp_Descr = reader.GetString(_.Sp_Descr);
    			}
    			if ((false == reader.IsDBNull(_.Sp_PushTime))) {
    				this._Sp_PushTime = reader.GetDateTime(_.Sp_PushTime);
    			}
    			if ((false == reader.IsDBNull(_.Sp_Label))) {
    				this._Sp_Label = reader.GetString(_.Sp_Label);
    			}
    			if ((false == reader.IsDBNull(_.Sp_Tootip))) {
    				this._Sp_Tootip = reader.GetString(_.Sp_Tootip);
    			}
    			if ((false == reader.IsDBNull(_.Sp_Banner))) {
    				this._Sp_Banner = reader.GetString(_.Sp_Banner);
    			}
    			if ((false == reader.IsDBNull(_.Sp_Uid))) {
    				this._Sp_Uid = reader.GetString(_.Sp_Uid);
    			}
    			if ((false == reader.IsDBNull(_.Sp_Logo))) {
    				this._Sp_Logo = reader.GetString(_.Sp_Logo);
    			}
    			if ((false == reader.IsDBNull(_.Sp_QrCode))) {
    				this._Sp_QrCode = reader.GetString(_.Sp_QrCode);
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
    			if ((false == typeof(Special).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Special>();
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sp_Id = new WeiSha.Data.Field<Special>("Sp_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_PatId - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Sp_PatId = new WeiSha.Data.Field<Special>("Sp_PatId");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_Tax - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Sp_Tax = new WeiSha.Data.Field<Special>("Sp_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sp_Name = new WeiSha.Data.Field<Special>("Sp_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Sp_IsUse = new WeiSha.Data.Field<Special>("Sp_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Sp_IsShow = new WeiSha.Data.Field<Special>("Sp_IsShow");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_IsOut - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Sp_IsOut = new WeiSha.Data.Field<Special>("Sp_IsOut");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_OutUrl - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sp_OutUrl = new WeiSha.Data.Field<Special>("Sp_OutUrl");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sp_Intro = new WeiSha.Data.Field<Special>("Sp_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_Details - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sp_Details = new WeiSha.Data.Field<Special>("Sp_Details");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_Keywords - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sp_Keywords = new WeiSha.Data.Field<Special>("Sp_Keywords");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_Descr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sp_Descr = new WeiSha.Data.Field<Special>("Sp_Descr");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_PushTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Sp_PushTime = new WeiSha.Data.Field<Special>("Sp_PushTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_Label - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sp_Label = new WeiSha.Data.Field<Special>("Sp_Label");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_Tootip - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sp_Tootip = new WeiSha.Data.Field<Special>("Sp_Tootip");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_Banner - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sp_Banner = new WeiSha.Data.Field<Special>("Sp_Banner");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_Uid - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sp_Uid = new WeiSha.Data.Field<Special>("Sp_Uid");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sp_Logo = new WeiSha.Data.Field<Special>("Sp_Logo");
    			
    			/// <summary>
    			/// -1 - 字段名：Sp_QrCode - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sp_QrCode = new WeiSha.Data.Field<Special>("Sp_QrCode");
    			
    			/// <summary>
    			/// -1 - 字段名：OtherData - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field OtherData = new WeiSha.Data.Field<Special>("OtherData");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Special>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Special>("Org_Name");
    		}
    	}
    }
    