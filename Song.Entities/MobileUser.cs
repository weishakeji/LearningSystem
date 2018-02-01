namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：MobileUser 主键列：Mu_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class MobileUser : WeiSha.Data.Entity {
    		
    		protected Int32 _Mu_Id;
    		
    		protected String _Mu_Phone;
    		
    		protected DateTime? _Mu_LastTime;
    		
    		protected Int32? _Mu_Sex;
    		
    		protected Int32? _Mu_Age;
    		
    		protected String _Mu_Post;
    		
    		protected String _Mu_Depart;
    		
    		protected Int32? _Mu_Number;
    		
    		protected String _Mu_Version;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Mu_Id {
    			get {
    				return this._Mu_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mu_Id, _Mu_Id, value);
    				this._Mu_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Mu_Phone {
    			get {
    				return this._Mu_Phone;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mu_Phone, _Mu_Phone, value);
    				this._Mu_Phone = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Mu_LastTime {
    			get {
    				return this._Mu_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mu_LastTime, _Mu_LastTime, value);
    				this._Mu_LastTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Mu_Sex {
    			get {
    				return this._Mu_Sex;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mu_Sex, _Mu_Sex, value);
    				this._Mu_Sex = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Mu_Age {
    			get {
    				return this._Mu_Age;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mu_Age, _Mu_Age, value);
    				this._Mu_Age = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Mu_Post {
    			get {
    				return this._Mu_Post;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mu_Post, _Mu_Post, value);
    				this._Mu_Post = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Mu_Depart {
    			get {
    				return this._Mu_Depart;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mu_Depart, _Mu_Depart, value);
    				this._Mu_Depart = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Mu_Number {
    			get {
    				return this._Mu_Number;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mu_Number, _Mu_Number, value);
    				this._Mu_Number = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Mu_Version {
    			get {
    				return this._Mu_Version;
    			}
    			set {
    				this.OnPropertyValueChange(_.Mu_Version, _Mu_Version, value);
    				this._Mu_Version = value;
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
    			return new WeiSha.Data.Table<MobileUser>("MobileUser");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Mu_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Mu_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Mu_Id,
    					_.Mu_Phone,
    					_.Mu_LastTime,
    					_.Mu_Sex,
    					_.Mu_Age,
    					_.Mu_Post,
    					_.Mu_Depart,
    					_.Mu_Number,
    					_.Mu_Version,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Mu_Id,
    					this._Mu_Phone,
    					this._Mu_LastTime,
    					this._Mu_Sex,
    					this._Mu_Age,
    					this._Mu_Post,
    					this._Mu_Depart,
    					this._Mu_Number,
    					this._Mu_Version,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Mu_Id))) {
    				this._Mu_Id = reader.GetInt32(_.Mu_Id);
    			}
    			if ((false == reader.IsDBNull(_.Mu_Phone))) {
    				this._Mu_Phone = reader.GetString(_.Mu_Phone);
    			}
    			if ((false == reader.IsDBNull(_.Mu_LastTime))) {
    				this._Mu_LastTime = reader.GetDateTime(_.Mu_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Mu_Sex))) {
    				this._Mu_Sex = reader.GetInt32(_.Mu_Sex);
    			}
    			if ((false == reader.IsDBNull(_.Mu_Age))) {
    				this._Mu_Age = reader.GetInt32(_.Mu_Age);
    			}
    			if ((false == reader.IsDBNull(_.Mu_Post))) {
    				this._Mu_Post = reader.GetString(_.Mu_Post);
    			}
    			if ((false == reader.IsDBNull(_.Mu_Depart))) {
    				this._Mu_Depart = reader.GetString(_.Mu_Depart);
    			}
    			if ((false == reader.IsDBNull(_.Mu_Number))) {
    				this._Mu_Number = reader.GetInt32(_.Mu_Number);
    			}
    			if ((false == reader.IsDBNull(_.Mu_Version))) {
    				this._Mu_Version = reader.GetString(_.Mu_Version);
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
    			if ((false == typeof(MobileUser).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<MobileUser>();
    			
    			/// <summary>
    			/// -1 - 字段名：Mu_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Mu_Id = new WeiSha.Data.Field<MobileUser>("Mu_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Mu_Phone - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Mu_Phone = new WeiSha.Data.Field<MobileUser>("Mu_Phone");
    			
    			/// <summary>
    			/// -1 - 字段名：Mu_LastTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Mu_LastTime = new WeiSha.Data.Field<MobileUser>("Mu_LastTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Mu_Sex - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Mu_Sex = new WeiSha.Data.Field<MobileUser>("Mu_Sex");
    			
    			/// <summary>
    			/// -1 - 字段名：Mu_Age - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Mu_Age = new WeiSha.Data.Field<MobileUser>("Mu_Age");
    			
    			/// <summary>
    			/// -1 - 字段名：Mu_Post - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Mu_Post = new WeiSha.Data.Field<MobileUser>("Mu_Post");
    			
    			/// <summary>
    			/// -1 - 字段名：Mu_Depart - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Mu_Depart = new WeiSha.Data.Field<MobileUser>("Mu_Depart");
    			
    			/// <summary>
    			/// -1 - 字段名：Mu_Number - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Mu_Number = new WeiSha.Data.Field<MobileUser>("Mu_Number");
    			
    			/// <summary>
    			/// -1 - 字段名：Mu_Version - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Mu_Version = new WeiSha.Data.Field<MobileUser>("Mu_Version");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<MobileUser>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<MobileUser>("Org_Name");
    		}
    	}
    }
    