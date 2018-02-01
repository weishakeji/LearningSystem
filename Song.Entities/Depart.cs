namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Depart 主键列：Dep_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Depart : WeiSha.Data.Entity {
    		
    		protected Int32 _Dep_Id;
    		
    		protected String _Dep_CnName;
    		
    		protected String _Dep_Code;
    		
    		protected Int32 _Dep_Tax;
    		
    		protected Boolean _Dep_IsUse;
    		
    		protected Boolean _Dep_IsShow;
    		
    		protected Boolean _Dep_IsAdmin;
    		
    		protected Int32 _Dep_PatId;
    		
    		protected Int32 _Dep_Level;
    		
    		protected String _Dep_Func;
    		
    		protected String _Dep_Phone;
    		
    		protected String _Dep_Msn;
    		
    		protected String _Dep_Fax;
    		
    		protected String _Dep_Email;
    		
    		protected String _Dep_WorkAddr;
    		
    		protected String _Dep_CnAbbr;
    		
    		protected String _Dep_EnAbbr;
    		
    		protected String _Dep_EnName;
    		
    		protected Boolean _Dep_State;
    		
    		protected Int32 _Dep_Count;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Dep_Id {
    			get {
    				return this._Dep_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Id, _Dep_Id, value);
    				this._Dep_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dep_CnName {
    			get {
    				return this._Dep_CnName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_CnName, _Dep_CnName, value);
    				this._Dep_CnName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dep_Code {
    			get {
    				return this._Dep_Code;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Code, _Dep_Code, value);
    				this._Dep_Code = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Dep_Tax {
    			get {
    				return this._Dep_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Tax, _Dep_Tax, value);
    				this._Dep_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Dep_IsUse {
    			get {
    				return this._Dep_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_IsUse, _Dep_IsUse, value);
    				this._Dep_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Dep_IsShow {
    			get {
    				return this._Dep_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_IsShow, _Dep_IsShow, value);
    				this._Dep_IsShow = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Dep_IsAdmin {
    			get {
    				return this._Dep_IsAdmin;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_IsAdmin, _Dep_IsAdmin, value);
    				this._Dep_IsAdmin = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Dep_PatId {
    			get {
    				return this._Dep_PatId;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_PatId, _Dep_PatId, value);
    				this._Dep_PatId = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Dep_Level {
    			get {
    				return this._Dep_Level;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Level, _Dep_Level, value);
    				this._Dep_Level = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dep_Func {
    			get {
    				return this._Dep_Func;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Func, _Dep_Func, value);
    				this._Dep_Func = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dep_Phone {
    			get {
    				return this._Dep_Phone;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Phone, _Dep_Phone, value);
    				this._Dep_Phone = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dep_Msn {
    			get {
    				return this._Dep_Msn;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Msn, _Dep_Msn, value);
    				this._Dep_Msn = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dep_Fax {
    			get {
    				return this._Dep_Fax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Fax, _Dep_Fax, value);
    				this._Dep_Fax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dep_Email {
    			get {
    				return this._Dep_Email;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Email, _Dep_Email, value);
    				this._Dep_Email = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dep_WorkAddr {
    			get {
    				return this._Dep_WorkAddr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_WorkAddr, _Dep_WorkAddr, value);
    				this._Dep_WorkAddr = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dep_CnAbbr {
    			get {
    				return this._Dep_CnAbbr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_CnAbbr, _Dep_CnAbbr, value);
    				this._Dep_CnAbbr = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dep_EnAbbr {
    			get {
    				return this._Dep_EnAbbr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_EnAbbr, _Dep_EnAbbr, value);
    				this._Dep_EnAbbr = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Dep_EnName {
    			get {
    				return this._Dep_EnName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_EnName, _Dep_EnName, value);
    				this._Dep_EnName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Dep_State {
    			get {
    				return this._Dep_State;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_State, _Dep_State, value);
    				this._Dep_State = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Dep_Count {
    			get {
    				return this._Dep_Count;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Count, _Dep_Count, value);
    				this._Dep_Count = value;
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
    			return new WeiSha.Data.Table<Depart>("Depart");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Dep_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Dep_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Dep_Id,
    					_.Dep_CnName,
    					_.Dep_Code,
    					_.Dep_Tax,
    					_.Dep_IsUse,
    					_.Dep_IsShow,
    					_.Dep_IsAdmin,
    					_.Dep_PatId,
    					_.Dep_Level,
    					_.Dep_Func,
    					_.Dep_Phone,
    					_.Dep_Msn,
    					_.Dep_Fax,
    					_.Dep_Email,
    					_.Dep_WorkAddr,
    					_.Dep_CnAbbr,
    					_.Dep_EnAbbr,
    					_.Dep_EnName,
    					_.Dep_State,
    					_.Dep_Count,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Dep_Id,
    					this._Dep_CnName,
    					this._Dep_Code,
    					this._Dep_Tax,
    					this._Dep_IsUse,
    					this._Dep_IsShow,
    					this._Dep_IsAdmin,
    					this._Dep_PatId,
    					this._Dep_Level,
    					this._Dep_Func,
    					this._Dep_Phone,
    					this._Dep_Msn,
    					this._Dep_Fax,
    					this._Dep_Email,
    					this._Dep_WorkAddr,
    					this._Dep_CnAbbr,
    					this._Dep_EnAbbr,
    					this._Dep_EnName,
    					this._Dep_State,
    					this._Dep_Count,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Dep_Id))) {
    				this._Dep_Id = reader.GetInt32(_.Dep_Id);
    			}
    			if ((false == reader.IsDBNull(_.Dep_CnName))) {
    				this._Dep_CnName = reader.GetString(_.Dep_CnName);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Code))) {
    				this._Dep_Code = reader.GetString(_.Dep_Code);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Tax))) {
    				this._Dep_Tax = reader.GetInt32(_.Dep_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Dep_IsUse))) {
    				this._Dep_IsUse = reader.GetBoolean(_.Dep_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Dep_IsShow))) {
    				this._Dep_IsShow = reader.GetBoolean(_.Dep_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Dep_IsAdmin))) {
    				this._Dep_IsAdmin = reader.GetBoolean(_.Dep_IsAdmin);
    			}
    			if ((false == reader.IsDBNull(_.Dep_PatId))) {
    				this._Dep_PatId = reader.GetInt32(_.Dep_PatId);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Level))) {
    				this._Dep_Level = reader.GetInt32(_.Dep_Level);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Func))) {
    				this._Dep_Func = reader.GetString(_.Dep_Func);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Phone))) {
    				this._Dep_Phone = reader.GetString(_.Dep_Phone);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Msn))) {
    				this._Dep_Msn = reader.GetString(_.Dep_Msn);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Fax))) {
    				this._Dep_Fax = reader.GetString(_.Dep_Fax);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Email))) {
    				this._Dep_Email = reader.GetString(_.Dep_Email);
    			}
    			if ((false == reader.IsDBNull(_.Dep_WorkAddr))) {
    				this._Dep_WorkAddr = reader.GetString(_.Dep_WorkAddr);
    			}
    			if ((false == reader.IsDBNull(_.Dep_CnAbbr))) {
    				this._Dep_CnAbbr = reader.GetString(_.Dep_CnAbbr);
    			}
    			if ((false == reader.IsDBNull(_.Dep_EnAbbr))) {
    				this._Dep_EnAbbr = reader.GetString(_.Dep_EnAbbr);
    			}
    			if ((false == reader.IsDBNull(_.Dep_EnName))) {
    				this._Dep_EnName = reader.GetString(_.Dep_EnName);
    			}
    			if ((false == reader.IsDBNull(_.Dep_State))) {
    				this._Dep_State = reader.GetBoolean(_.Dep_State);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Count))) {
    				this._Dep_Count = reader.GetInt32(_.Dep_Count);
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
    			if ((false == typeof(Depart).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Depart>();
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Id = new WeiSha.Data.Field<Depart>("Dep_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_CnName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_CnName = new WeiSha.Data.Field<Depart>("Dep_CnName");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_Code - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Code = new WeiSha.Data.Field<Depart>("Dep_Code");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Tax = new WeiSha.Data.Field<Depart>("Dep_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Dep_IsUse = new WeiSha.Data.Field<Depart>("Dep_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Dep_IsShow = new WeiSha.Data.Field<Depart>("Dep_IsShow");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_IsAdmin - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Dep_IsAdmin = new WeiSha.Data.Field<Depart>("Dep_IsAdmin");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_PatId - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_PatId = new WeiSha.Data.Field<Depart>("Dep_PatId");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_Level - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Level = new WeiSha.Data.Field<Depart>("Dep_Level");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_Func - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Func = new WeiSha.Data.Field<Depart>("Dep_Func");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_Phone - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Phone = new WeiSha.Data.Field<Depart>("Dep_Phone");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_Msn - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Msn = new WeiSha.Data.Field<Depart>("Dep_Msn");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_Fax - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Fax = new WeiSha.Data.Field<Depart>("Dep_Fax");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_Email - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Email = new WeiSha.Data.Field<Depart>("Dep_Email");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_WorkAddr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_WorkAddr = new WeiSha.Data.Field<Depart>("Dep_WorkAddr");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_CnAbbr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_CnAbbr = new WeiSha.Data.Field<Depart>("Dep_CnAbbr");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_EnAbbr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_EnAbbr = new WeiSha.Data.Field<Depart>("Dep_EnAbbr");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_EnName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_EnName = new WeiSha.Data.Field<Depart>("Dep_EnName");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_State - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Dep_State = new WeiSha.Data.Field<Depart>("Dep_State");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_Count - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Count = new WeiSha.Data.Field<Depart>("Dep_Count");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Depart>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Depart>("Org_Name");
    		}
    	}
    }
    