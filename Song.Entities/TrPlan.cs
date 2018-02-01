namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：TrPlan 主键列：TrP_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class TrPlan : WeiSha.Data.Entity {
    		
    		protected Int32 _TrP_Id;
    		
    		protected DateTime _TrP_Time;
    		
    		protected DateTime _TrP_Start;
    		
    		protected DateTime _TrP_End;
    		
    		protected Int32 _Dep_Id;
    		
    		protected String _Dep_CnName;
    		
    		protected Int32 _Sbj_ID;
    		
    		protected String _Sbj_Name;
    		
    		protected Int32 _TrP_GroupType;
    		
    		protected Int32 _TrP_Num;
    		
    		protected String _TrP_Content;
    		
    		protected String _TrP_Location;
    		
    		protected String _TrP_Teacher;
    		
    		protected String _TrP_Way;
    		
    		protected Int32 _TrP_Result;
    		
    		protected String _TrP_UID;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		public Int32 TrP_Id {
    			get {
    				return this._TrP_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.TrP_Id, _TrP_Id, value);
    				this._TrP_Id = value;
    			}
    		}
    		
    		public DateTime TrP_Time {
    			get {
    				return this._TrP_Time;
    			}
    			set {
    				this.OnPropertyValueChange(_.TrP_Time, _TrP_Time, value);
    				this._TrP_Time = value;
    			}
    		}
    		
    		public DateTime TrP_Start {
    			get {
    				return this._TrP_Start;
    			}
    			set {
    				this.OnPropertyValueChange(_.TrP_Start, _TrP_Start, value);
    				this._TrP_Start = value;
    			}
    		}
    		
    		public DateTime TrP_End {
    			get {
    				return this._TrP_End;
    			}
    			set {
    				this.OnPropertyValueChange(_.TrP_End, _TrP_End, value);
    				this._TrP_End = value;
    			}
    		}
    		
    		public Int32 Dep_Id {
    			get {
    				return this._Dep_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Id, _Dep_Id, value);
    				this._Dep_Id = value;
    			}
    		}
    		
    		public String Dep_CnName {
    			get {
    				return this._Dep_CnName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_CnName, _Dep_CnName, value);
    				this._Dep_CnName = value;
    			}
    		}
    		
    		public Int32 Sbj_ID {
    			get {
    				return this._Sbj_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_ID, _Sbj_ID, value);
    				this._Sbj_ID = value;
    			}
    		}
    		
    		public String Sbj_Name {
    			get {
    				return this._Sbj_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_Name, _Sbj_Name, value);
    				this._Sbj_Name = value;
    			}
    		}
    		
    		public Int32 TrP_GroupType {
    			get {
    				return this._TrP_GroupType;
    			}
    			set {
    				this.OnPropertyValueChange(_.TrP_GroupType, _TrP_GroupType, value);
    				this._TrP_GroupType = value;
    			}
    		}
    		
    		public Int32 TrP_Num {
    			get {
    				return this._TrP_Num;
    			}
    			set {
    				this.OnPropertyValueChange(_.TrP_Num, _TrP_Num, value);
    				this._TrP_Num = value;
    			}
    		}
    		
    		public String TrP_Content {
    			get {
    				return this._TrP_Content;
    			}
    			set {
    				this.OnPropertyValueChange(_.TrP_Content, _TrP_Content, value);
    				this._TrP_Content = value;
    			}
    		}
    		
    		public String TrP_Location {
    			get {
    				return this._TrP_Location;
    			}
    			set {
    				this.OnPropertyValueChange(_.TrP_Location, _TrP_Location, value);
    				this._TrP_Location = value;
    			}
    		}
    		
    		public String TrP_Teacher {
    			get {
    				return this._TrP_Teacher;
    			}
    			set {
    				this.OnPropertyValueChange(_.TrP_Teacher, _TrP_Teacher, value);
    				this._TrP_Teacher = value;
    			}
    		}
    		
    		public String TrP_Way {
    			get {
    				return this._TrP_Way;
    			}
    			set {
    				this.OnPropertyValueChange(_.TrP_Way, _TrP_Way, value);
    				this._TrP_Way = value;
    			}
    		}
    		
    		public Int32 TrP_Result {
    			get {
    				return this._TrP_Result;
    			}
    			set {
    				this.OnPropertyValueChange(_.TrP_Result, _TrP_Result, value);
    				this._TrP_Result = value;
    			}
    		}
    		
    		public String TrP_UID {
    			get {
    				return this._TrP_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.TrP_UID, _TrP_UID, value);
    				this._TrP_UID = value;
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
    			return new WeiSha.Data.Table<TrPlan>("TrPlan");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.TrP_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.TrP_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.TrP_Id,
    					_.TrP_Time,
    					_.TrP_Start,
    					_.TrP_End,
    					_.Dep_Id,
    					_.Dep_CnName,
    					_.Sbj_ID,
    					_.Sbj_Name,
    					_.TrP_GroupType,
    					_.TrP_Num,
    					_.TrP_Content,
    					_.TrP_Location,
    					_.TrP_Teacher,
    					_.TrP_Way,
    					_.TrP_Result,
    					_.TrP_UID,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._TrP_Id,
    					this._TrP_Time,
    					this._TrP_Start,
    					this._TrP_End,
    					this._Dep_Id,
    					this._Dep_CnName,
    					this._Sbj_ID,
    					this._Sbj_Name,
    					this._TrP_GroupType,
    					this._TrP_Num,
    					this._TrP_Content,
    					this._TrP_Location,
    					this._TrP_Teacher,
    					this._TrP_Way,
    					this._TrP_Result,
    					this._TrP_UID,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.TrP_Id))) {
    				this._TrP_Id = reader.GetInt32(_.TrP_Id);
    			}
    			if ((false == reader.IsDBNull(_.TrP_Time))) {
    				this._TrP_Time = reader.GetDateTime(_.TrP_Time);
    			}
    			if ((false == reader.IsDBNull(_.TrP_Start))) {
    				this._TrP_Start = reader.GetDateTime(_.TrP_Start);
    			}
    			if ((false == reader.IsDBNull(_.TrP_End))) {
    				this._TrP_End = reader.GetDateTime(_.TrP_End);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Id))) {
    				this._Dep_Id = reader.GetInt32(_.Dep_Id);
    			}
    			if ((false == reader.IsDBNull(_.Dep_CnName))) {
    				this._Dep_CnName = reader.GetString(_.Dep_CnName);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt32(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Name))) {
    				this._Sbj_Name = reader.GetString(_.Sbj_Name);
    			}
    			if ((false == reader.IsDBNull(_.TrP_GroupType))) {
    				this._TrP_GroupType = reader.GetInt32(_.TrP_GroupType);
    			}
    			if ((false == reader.IsDBNull(_.TrP_Num))) {
    				this._TrP_Num = reader.GetInt32(_.TrP_Num);
    			}
    			if ((false == reader.IsDBNull(_.TrP_Content))) {
    				this._TrP_Content = reader.GetString(_.TrP_Content);
    			}
    			if ((false == reader.IsDBNull(_.TrP_Location))) {
    				this._TrP_Location = reader.GetString(_.TrP_Location);
    			}
    			if ((false == reader.IsDBNull(_.TrP_Teacher))) {
    				this._TrP_Teacher = reader.GetString(_.TrP_Teacher);
    			}
    			if ((false == reader.IsDBNull(_.TrP_Way))) {
    				this._TrP_Way = reader.GetString(_.TrP_Way);
    			}
    			if ((false == reader.IsDBNull(_.TrP_Result))) {
    				this._TrP_Result = reader.GetInt32(_.TrP_Result);
    			}
    			if ((false == reader.IsDBNull(_.TrP_UID))) {
    				this._TrP_UID = reader.GetString(_.TrP_UID);
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
    			if ((false == typeof(TrPlan).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<TrPlan>();
    			
    			/// <summary>
    			/// 字段名：TrP_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field TrP_Id = new WeiSha.Data.Field<TrPlan>("TrP_Id");
    			
    			/// <summary>
    			/// 字段名：TrP_Time - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field TrP_Time = new WeiSha.Data.Field<TrPlan>("TrP_Time");
    			
    			/// <summary>
    			/// 字段名：TrP_Start - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field TrP_Start = new WeiSha.Data.Field<TrPlan>("TrP_Start");
    			
    			/// <summary>
    			/// 字段名：TrP_End - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field TrP_End = new WeiSha.Data.Field<TrPlan>("TrP_End");
    			
    			/// <summary>
    			/// 字段名：Dep_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Id = new WeiSha.Data.Field<TrPlan>("Dep_Id");
    			
    			/// <summary>
    			/// 字段名：Dep_CnName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Dep_CnName = new WeiSha.Data.Field<TrPlan>("Dep_CnName");
    			
    			/// <summary>
    			/// 字段名：Sbj_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<TrPlan>("Sbj_ID");
    			
    			/// <summary>
    			/// 字段名：Sbj_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Name = new WeiSha.Data.Field<TrPlan>("Sbj_Name");
    			
    			/// <summary>
    			/// 字段名：TrP_GroupType - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field TrP_GroupType = new WeiSha.Data.Field<TrPlan>("TrP_GroupType");
    			
    			/// <summary>
    			/// 字段名：TrP_Num - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field TrP_Num = new WeiSha.Data.Field<TrPlan>("TrP_Num");
    			
    			/// <summary>
    			/// 字段名：TrP_Content - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field TrP_Content = new WeiSha.Data.Field<TrPlan>("TrP_Content");
    			
    			/// <summary>
    			/// 字段名：TrP_Location - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field TrP_Location = new WeiSha.Data.Field<TrPlan>("TrP_Location");
    			
    			/// <summary>
    			/// 字段名：TrP_Teacher - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field TrP_Teacher = new WeiSha.Data.Field<TrPlan>("TrP_Teacher");
    			
    			/// <summary>
    			/// 字段名：TrP_Way - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field TrP_Way = new WeiSha.Data.Field<TrPlan>("TrP_Way");
    			
    			/// <summary>
    			/// 字段名：TrP_Result - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field TrP_Result = new WeiSha.Data.Field<TrPlan>("TrP_Result");
    			
    			/// <summary>
    			/// 字段名：TrP_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field TrP_UID = new WeiSha.Data.Field<TrPlan>("TrP_UID");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<TrPlan>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<TrPlan>("Org_Name");
    		}
    	}
    }
    