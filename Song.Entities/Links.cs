namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Links 主键列：Lk_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Links : WeiSha.Data.Entity {
    		
    		protected Int32 _Lk_Id;
    		
    		protected Int32? _Ls_Id;
    		
    		protected String _Ls_Name;
    		
    		protected String _Lk_Name;
    		
    		protected String _Lk_Url;
    		
    		protected Int32? _Lk_Tax;
    		
    		protected Boolean _Lk_IsUse;
    		
    		protected String _Lk_Logo;
    		
    		protected String _Lk_LogoSmall;
    		
    		protected Boolean _Lk_IsShow;
    		
    		protected String _Lk_Tootip;
    		
    		protected String _Lk_Email;
    		
    		protected Boolean _Lk_IsApply;
    		
    		protected Boolean _Lk_IsVerify;
    		
    		protected String _Lk_SiteMaster;
    		
    		protected String _Lk_QQ;
    		
    		protected String _Lk_Mobile;
    		
    		protected String _Lk_Explain;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Lk_Id {
    			get {
    				return this._Lk_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_Id, _Lk_Id, value);
    				this._Lk_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Ls_Id {
    			get {
    				return this._Ls_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ls_Id, _Ls_Id, value);
    				this._Ls_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Ls_Name {
    			get {
    				return this._Ls_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ls_Name, _Ls_Name, value);
    				this._Ls_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Lk_Name {
    			get {
    				return this._Lk_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_Name, _Lk_Name, value);
    				this._Lk_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Lk_Url {
    			get {
    				return this._Lk_Url;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_Url, _Lk_Url, value);
    				this._Lk_Url = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Lk_Tax {
    			get {
    				return this._Lk_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_Tax, _Lk_Tax, value);
    				this._Lk_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Lk_IsUse {
    			get {
    				return this._Lk_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_IsUse, _Lk_IsUse, value);
    				this._Lk_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Lk_Logo {
    			get {
    				return this._Lk_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_Logo, _Lk_Logo, value);
    				this._Lk_Logo = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Lk_LogoSmall {
    			get {
    				return this._Lk_LogoSmall;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_LogoSmall, _Lk_LogoSmall, value);
    				this._Lk_LogoSmall = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Lk_IsShow {
    			get {
    				return this._Lk_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_IsShow, _Lk_IsShow, value);
    				this._Lk_IsShow = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Lk_Tootip {
    			get {
    				return this._Lk_Tootip;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_Tootip, _Lk_Tootip, value);
    				this._Lk_Tootip = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Lk_Email {
    			get {
    				return this._Lk_Email;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_Email, _Lk_Email, value);
    				this._Lk_Email = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Lk_IsApply {
    			get {
    				return this._Lk_IsApply;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_IsApply, _Lk_IsApply, value);
    				this._Lk_IsApply = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Lk_IsVerify {
    			get {
    				return this._Lk_IsVerify;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_IsVerify, _Lk_IsVerify, value);
    				this._Lk_IsVerify = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Lk_SiteMaster {
    			get {
    				return this._Lk_SiteMaster;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_SiteMaster, _Lk_SiteMaster, value);
    				this._Lk_SiteMaster = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Lk_QQ {
    			get {
    				return this._Lk_QQ;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_QQ, _Lk_QQ, value);
    				this._Lk_QQ = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Lk_Mobile {
    			get {
    				return this._Lk_Mobile;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_Mobile, _Lk_Mobile, value);
    				this._Lk_Mobile = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Lk_Explain {
    			get {
    				return this._Lk_Explain;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lk_Explain, _Lk_Explain, value);
    				this._Lk_Explain = value;
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
    			return new WeiSha.Data.Table<Links>("Links");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Lk_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lk_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lk_Id,
    					_.Ls_Id,
    					_.Ls_Name,
    					_.Lk_Name,
    					_.Lk_Url,
    					_.Lk_Tax,
    					_.Lk_IsUse,
    					_.Lk_Logo,
    					_.Lk_LogoSmall,
    					_.Lk_IsShow,
    					_.Lk_Tootip,
    					_.Lk_Email,
    					_.Lk_IsApply,
    					_.Lk_IsVerify,
    					_.Lk_SiteMaster,
    					_.Lk_QQ,
    					_.Lk_Mobile,
    					_.Lk_Explain,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Lk_Id,
    					this._Ls_Id,
    					this._Ls_Name,
    					this._Lk_Name,
    					this._Lk_Url,
    					this._Lk_Tax,
    					this._Lk_IsUse,
    					this._Lk_Logo,
    					this._Lk_LogoSmall,
    					this._Lk_IsShow,
    					this._Lk_Tootip,
    					this._Lk_Email,
    					this._Lk_IsApply,
    					this._Lk_IsVerify,
    					this._Lk_SiteMaster,
    					this._Lk_QQ,
    					this._Lk_Mobile,
    					this._Lk_Explain,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Lk_Id))) {
    				this._Lk_Id = reader.GetInt32(_.Lk_Id);
    			}
    			if ((false == reader.IsDBNull(_.Ls_Id))) {
    				this._Ls_Id = reader.GetInt32(_.Ls_Id);
    			}
    			if ((false == reader.IsDBNull(_.Ls_Name))) {
    				this._Ls_Name = reader.GetString(_.Ls_Name);
    			}
    			if ((false == reader.IsDBNull(_.Lk_Name))) {
    				this._Lk_Name = reader.GetString(_.Lk_Name);
    			}
    			if ((false == reader.IsDBNull(_.Lk_Url))) {
    				this._Lk_Url = reader.GetString(_.Lk_Url);
    			}
    			if ((false == reader.IsDBNull(_.Lk_Tax))) {
    				this._Lk_Tax = reader.GetInt32(_.Lk_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Lk_IsUse))) {
    				this._Lk_IsUse = reader.GetBoolean(_.Lk_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Lk_Logo))) {
    				this._Lk_Logo = reader.GetString(_.Lk_Logo);
    			}
    			if ((false == reader.IsDBNull(_.Lk_LogoSmall))) {
    				this._Lk_LogoSmall = reader.GetString(_.Lk_LogoSmall);
    			}
    			if ((false == reader.IsDBNull(_.Lk_IsShow))) {
    				this._Lk_IsShow = reader.GetBoolean(_.Lk_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Lk_Tootip))) {
    				this._Lk_Tootip = reader.GetString(_.Lk_Tootip);
    			}
    			if ((false == reader.IsDBNull(_.Lk_Email))) {
    				this._Lk_Email = reader.GetString(_.Lk_Email);
    			}
    			if ((false == reader.IsDBNull(_.Lk_IsApply))) {
    				this._Lk_IsApply = reader.GetBoolean(_.Lk_IsApply);
    			}
    			if ((false == reader.IsDBNull(_.Lk_IsVerify))) {
    				this._Lk_IsVerify = reader.GetBoolean(_.Lk_IsVerify);
    			}
    			if ((false == reader.IsDBNull(_.Lk_SiteMaster))) {
    				this._Lk_SiteMaster = reader.GetString(_.Lk_SiteMaster);
    			}
    			if ((false == reader.IsDBNull(_.Lk_QQ))) {
    				this._Lk_QQ = reader.GetString(_.Lk_QQ);
    			}
    			if ((false == reader.IsDBNull(_.Lk_Mobile))) {
    				this._Lk_Mobile = reader.GetString(_.Lk_Mobile);
    			}
    			if ((false == reader.IsDBNull(_.Lk_Explain))) {
    				this._Lk_Explain = reader.GetString(_.Lk_Explain);
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
    			if ((false == typeof(Links).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Links>();
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lk_Id = new WeiSha.Data.Field<Links>("Lk_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Ls_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Ls_Id = new WeiSha.Data.Field<Links>("Ls_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Ls_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ls_Name = new WeiSha.Data.Field<Links>("Ls_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lk_Name = new WeiSha.Data.Field<Links>("Lk_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_Url - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lk_Url = new WeiSha.Data.Field<Links>("Lk_Url");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_Tax - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Lk_Tax = new WeiSha.Data.Field<Links>("Lk_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Lk_IsUse = new WeiSha.Data.Field<Links>("Lk_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lk_Logo = new WeiSha.Data.Field<Links>("Lk_Logo");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_LogoSmall - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lk_LogoSmall = new WeiSha.Data.Field<Links>("Lk_LogoSmall");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Lk_IsShow = new WeiSha.Data.Field<Links>("Lk_IsShow");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_Tootip - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lk_Tootip = new WeiSha.Data.Field<Links>("Lk_Tootip");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_Email - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lk_Email = new WeiSha.Data.Field<Links>("Lk_Email");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_IsApply - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Lk_IsApply = new WeiSha.Data.Field<Links>("Lk_IsApply");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_IsVerify - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Lk_IsVerify = new WeiSha.Data.Field<Links>("Lk_IsVerify");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_SiteMaster - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lk_SiteMaster = new WeiSha.Data.Field<Links>("Lk_SiteMaster");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_QQ - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lk_QQ = new WeiSha.Data.Field<Links>("Lk_QQ");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_Mobile - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lk_Mobile = new WeiSha.Data.Field<Links>("Lk_Mobile");
    			
    			/// <summary>
    			/// -1 - 字段名：Lk_Explain - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Lk_Explain = new WeiSha.Data.Field<Links>("Lk_Explain");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Links>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Links>("Org_Name");
    		}
    	}
    }
    