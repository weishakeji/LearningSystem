namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Notice 主键列：No_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Notice : WeiSha.Data.Entity {
    		
    		protected Int32 _No_Id;
    		
    		protected String _No_Ttl;
    		
    		protected String _No_Context;
    		
    		protected Boolean _No_IsShow;
    		
    		protected Boolean _No_IsTop;
    		
    		protected DateTime? _No_CrtTime;
    		
    		protected DateTime? _No_StartTime;
    		
    		protected DateTime? _No_EndTime;
    		
    		protected Int32? _Acc_Id;
    		
    		protected String _Acc_Name;
    		
    		protected String _No_Organ;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected String _No_Page;
    		
    		protected String _No_Interval;
    		
    		protected Int32 _No_Range;
    		
    		protected Int32 _No_OpenCount;
    		
    		protected Int32 _No_Width;
    		
    		protected Int32 _No_Height;
    		
    		protected String _No_BgImage;
    		
    		protected String _No_Linkurl;
    		
    		protected Int32 _No_Timespan;
    		
    		protected String _No_StudentSort;
    		
    		protected Int32 _No_Type;
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32 No_Id {
    			get {
    				return this._No_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_Id, _No_Id, value);
    				this._No_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String No_Ttl {
    			get {
    				return this._No_Ttl;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_Ttl, _No_Ttl, value);
    				this._No_Ttl = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String No_Context {
    			get {
    				return this._No_Context;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_Context, _No_Context, value);
    				this._No_Context = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Boolean No_IsShow {
    			get {
    				return this._No_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_IsShow, _No_IsShow, value);
    				this._No_IsShow = value;
    			}
    		}
    		
    		public Boolean No_IsTop {
    			get {
    				return this._No_IsTop;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_IsTop, _No_IsTop, value);
    				this._No_IsTop = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public DateTime? No_CrtTime {
    			get {
    				return this._No_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_CrtTime, _No_CrtTime, value);
    				this._No_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public DateTime? No_StartTime {
    			get {
    				return this._No_StartTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_StartTime, _No_StartTime, value);
    				this._No_StartTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public DateTime? No_EndTime {
    			get {
    				return this._No_EndTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_EndTime, _No_EndTime, value);
    				this._No_EndTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
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
    		/// True
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
    		/// True
    		/// </summary>
    		public String No_Organ {
    			get {
    				return this._No_Organ;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_Organ, _No_Organ, value);
    				this._No_Organ = value;
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
    		
    		public String No_Page {
    			get {
    				return this._No_Page;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_Page, _No_Page, value);
    				this._No_Page = value;
    			}
    		}
    		
    		public String No_Interval {
    			get {
    				return this._No_Interval;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_Interval, _No_Interval, value);
    				this._No_Interval = value;
    			}
    		}
    		
    		public Int32 No_Range {
    			get {
    				return this._No_Range;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_Range, _No_Range, value);
    				this._No_Range = value;
    			}
    		}
    		
    		public Int32 No_OpenCount {
    			get {
    				return this._No_OpenCount;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_OpenCount, _No_OpenCount, value);
    				this._No_OpenCount = value;
    			}
    		}
    		
    		public Int32 No_Width {
    			get {
    				return this._No_Width;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_Width, _No_Width, value);
    				this._No_Width = value;
    			}
    		}
    		
    		public Int32 No_Height {
    			get {
    				return this._No_Height;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_Height, _No_Height, value);
    				this._No_Height = value;
    			}
    		}
    		
    		public String No_BgImage {
    			get {
    				return this._No_BgImage;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_BgImage, _No_BgImage, value);
    				this._No_BgImage = value;
    			}
    		}
    		
    		public String No_Linkurl {
    			get {
    				return this._No_Linkurl;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_Linkurl, _No_Linkurl, value);
    				this._No_Linkurl = value;
    			}
    		}
    		
    		public Int32 No_Timespan {
    			get {
    				return this._No_Timespan;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_Timespan, _No_Timespan, value);
    				this._No_Timespan = value;
    			}
    		}
    		
    		public String No_StudentSort {
    			get {
    				return this._No_StudentSort;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_StudentSort, _No_StudentSort, value);
    				this._No_StudentSort = value;
    			}
    		}
    		
    		public Int32 No_Type {
    			get {
    				return this._No_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_Type, _No_Type, value);
    				this._No_Type = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Notice>("Notice");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.No_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.No_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.No_Id,
    					_.No_Ttl,
    					_.No_Context,
    					_.No_IsShow,
    					_.No_IsTop,
    					_.No_CrtTime,
    					_.No_StartTime,
    					_.No_EndTime,
    					_.Acc_Id,
    					_.Acc_Name,
    					_.No_Organ,
    					_.Org_ID,
    					_.Org_Name,
    					_.No_Page,
    					_.No_Interval,
    					_.No_Range,
    					_.No_OpenCount,
    					_.No_Width,
    					_.No_Height,
    					_.No_BgImage,
    					_.No_Linkurl,
    					_.No_Timespan,
    					_.No_StudentSort,
    					_.No_Type};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._No_Id,
    					this._No_Ttl,
    					this._No_Context,
    					this._No_IsShow,
    					this._No_IsTop,
    					this._No_CrtTime,
    					this._No_StartTime,
    					this._No_EndTime,
    					this._Acc_Id,
    					this._Acc_Name,
    					this._No_Organ,
    					this._Org_ID,
    					this._Org_Name,
    					this._No_Page,
    					this._No_Interval,
    					this._No_Range,
    					this._No_OpenCount,
    					this._No_Width,
    					this._No_Height,
    					this._No_BgImage,
    					this._No_Linkurl,
    					this._No_Timespan,
    					this._No_StudentSort,
    					this._No_Type};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.No_Id))) {
    				this._No_Id = reader.GetInt32(_.No_Id);
    			}
    			if ((false == reader.IsDBNull(_.No_Ttl))) {
    				this._No_Ttl = reader.GetString(_.No_Ttl);
    			}
    			if ((false == reader.IsDBNull(_.No_Context))) {
    				this._No_Context = reader.GetString(_.No_Context);
    			}
    			if ((false == reader.IsDBNull(_.No_IsShow))) {
    				this._No_IsShow = reader.GetBoolean(_.No_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.No_IsTop))) {
    				this._No_IsTop = reader.GetBoolean(_.No_IsTop);
    			}
    			if ((false == reader.IsDBNull(_.No_CrtTime))) {
    				this._No_CrtTime = reader.GetDateTime(_.No_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.No_StartTime))) {
    				this._No_StartTime = reader.GetDateTime(_.No_StartTime);
    			}
    			if ((false == reader.IsDBNull(_.No_EndTime))) {
    				this._No_EndTime = reader.GetDateTime(_.No_EndTime);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Name))) {
    				this._Acc_Name = reader.GetString(_.Acc_Name);
    			}
    			if ((false == reader.IsDBNull(_.No_Organ))) {
    				this._No_Organ = reader.GetString(_.No_Organ);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.No_Page))) {
    				this._No_Page = reader.GetString(_.No_Page);
    			}
    			if ((false == reader.IsDBNull(_.No_Interval))) {
    				this._No_Interval = reader.GetString(_.No_Interval);
    			}
    			if ((false == reader.IsDBNull(_.No_Range))) {
    				this._No_Range = reader.GetInt32(_.No_Range);
    			}
    			if ((false == reader.IsDBNull(_.No_OpenCount))) {
    				this._No_OpenCount = reader.GetInt32(_.No_OpenCount);
    			}
    			if ((false == reader.IsDBNull(_.No_Width))) {
    				this._No_Width = reader.GetInt32(_.No_Width);
    			}
    			if ((false == reader.IsDBNull(_.No_Height))) {
    				this._No_Height = reader.GetInt32(_.No_Height);
    			}
    			if ((false == reader.IsDBNull(_.No_BgImage))) {
    				this._No_BgImage = reader.GetString(_.No_BgImage);
    			}
    			if ((false == reader.IsDBNull(_.No_Linkurl))) {
    				this._No_Linkurl = reader.GetString(_.No_Linkurl);
    			}
    			if ((false == reader.IsDBNull(_.No_Timespan))) {
    				this._No_Timespan = reader.GetInt32(_.No_Timespan);
    			}
    			if ((false == reader.IsDBNull(_.No_StudentSort))) {
    				this._No_StudentSort = reader.GetString(_.No_StudentSort);
    			}
    			if ((false == reader.IsDBNull(_.No_Type))) {
    				this._No_Type = reader.GetInt32(_.No_Type);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Notice).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Notice>();
    			
    			/// <summary>
    			/// False - 字段名：No_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field No_Id = new WeiSha.Data.Field<Notice>("No_Id");
    			
    			/// <summary>
    			/// True - 字段名：No_Ttl - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field No_Ttl = new WeiSha.Data.Field<Notice>("No_Ttl");
    			
    			/// <summary>
    			/// True - 字段名：No_Context - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field No_Context = new WeiSha.Data.Field<Notice>("No_Context");
    			
    			/// <summary>
    			/// False - 字段名：No_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field No_IsShow = new WeiSha.Data.Field<Notice>("No_IsShow");
    			
    			/// <summary>
    			/// 字段名：No_IsTop - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field No_IsTop = new WeiSha.Data.Field<Notice>("No_IsTop");
    			
    			/// <summary>
    			/// False - 字段名：No_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field No_CrtTime = new WeiSha.Data.Field<Notice>("No_CrtTime");
    			
    			/// <summary>
    			/// False - 字段名：No_StartTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field No_StartTime = new WeiSha.Data.Field<Notice>("No_StartTime");
    			
    			/// <summary>
    			/// False - 字段名：No_EndTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field No_EndTime = new WeiSha.Data.Field<Notice>("No_EndTime");
    			
    			/// <summary>
    			/// False - 字段名：Acc_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<Notice>("Acc_Id");
    			
    			/// <summary>
    			/// True - 字段名：Acc_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Name = new WeiSha.Data.Field<Notice>("Acc_Name");
    			
    			/// <summary>
    			/// True - 字段名：No_Organ - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field No_Organ = new WeiSha.Data.Field<Notice>("No_Organ");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Notice>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Notice>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：No_Page - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field No_Page = new WeiSha.Data.Field<Notice>("No_Page");
    			
    			/// <summary>
    			/// 字段名：No_Interval - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field No_Interval = new WeiSha.Data.Field<Notice>("No_Interval");
    			
    			/// <summary>
    			/// 字段名：No_Range - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field No_Range = new WeiSha.Data.Field<Notice>("No_Range");
    			
    			/// <summary>
    			/// 字段名：No_OpenCount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field No_OpenCount = new WeiSha.Data.Field<Notice>("No_OpenCount");
    			
    			/// <summary>
    			/// 字段名：No_Width - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field No_Width = new WeiSha.Data.Field<Notice>("No_Width");
    			
    			/// <summary>
    			/// 字段名：No_Height - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field No_Height = new WeiSha.Data.Field<Notice>("No_Height");
    			
    			/// <summary>
    			/// 字段名：No_BgImage - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field No_BgImage = new WeiSha.Data.Field<Notice>("No_BgImage");
    			
    			/// <summary>
    			/// 字段名：No_Linkurl - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field No_Linkurl = new WeiSha.Data.Field<Notice>("No_Linkurl");
    			
    			/// <summary>
    			/// 字段名：No_Timespan - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field No_Timespan = new WeiSha.Data.Field<Notice>("No_Timespan");
    			
    			/// <summary>
    			/// 字段名：No_StudentSort - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field No_StudentSort = new WeiSha.Data.Field<Notice>("No_StudentSort");
    			
    			/// <summary>
    			/// 字段名：No_Type - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field No_Type = new WeiSha.Data.Field<Notice>("No_Type");
    		}
    	}
    }
