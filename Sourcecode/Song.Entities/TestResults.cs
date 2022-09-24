namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：TestResults 主键列：Tr_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class TestResults : WeiSha.Data.Entity {
    		
    		protected Int32 _Tr_ID;
    		
    		protected Single? _Tr_Score;
    		
    		protected Single? _Tr_ScoreFinal;
    		
    		protected Single? _Tr_Draw;
    		
    		protected Single? _Tr_Colligate;
    		
    		protected String _Tr_Results;
    		
    		protected DateTime? _Tr_CrtTime;
    		
    		protected String _Tr_IP;
    		
    		protected String _Tr_Name;
    		
    		protected String _Tr_Mac;
    		
    		protected Boolean _Tr_IsSubmit;
    		
    		protected String _Tr_UID;
    		
    		protected Int32? _Sbj_ID;
    		
    		protected String _Sbj_Name;
    		
    		protected Int64 _Tp_Id;
    		
    		protected String _Tp_Name;
    		
    		protected Int32 _Ac_ID;
    		
    		protected String _Ac_Name;
    		
    		protected Int32 _Sts_ID;
    		
    		protected String _Sts_Name;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		protected Int32 _St_Sex;
    		
    		protected String _St_IDCardNumber;
    		
    		protected Int64 _Cou_ID;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Tr_ID {
    			get {
    				return this._Tr_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tr_ID, _Tr_ID, value);
    				this._Tr_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Single? Tr_Score {
    			get {
    				return this._Tr_Score;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tr_Score, _Tr_Score, value);
    				this._Tr_Score = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Single? Tr_ScoreFinal {
    			get {
    				return this._Tr_ScoreFinal;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tr_ScoreFinal, _Tr_ScoreFinal, value);
    				this._Tr_ScoreFinal = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Single? Tr_Draw {
    			get {
    				return this._Tr_Draw;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tr_Draw, _Tr_Draw, value);
    				this._Tr_Draw = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Single? Tr_Colligate {
    			get {
    				return this._Tr_Colligate;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tr_Colligate, _Tr_Colligate, value);
    				this._Tr_Colligate = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Tr_Results {
    			get {
    				return this._Tr_Results;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tr_Results, _Tr_Results, value);
    				this._Tr_Results = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Tr_CrtTime {
    			get {
    				return this._Tr_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tr_CrtTime, _Tr_CrtTime, value);
    				this._Tr_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Tr_IP {
    			get {
    				return this._Tr_IP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tr_IP, _Tr_IP, value);
    				this._Tr_IP = value;
    			}
    		}
    		
    		public String Tr_Name {
    			get {
    				return this._Tr_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tr_Name, _Tr_Name, value);
    				this._Tr_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Tr_Mac {
    			get {
    				return this._Tr_Mac;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tr_Mac, _Tr_Mac, value);
    				this._Tr_Mac = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Tr_IsSubmit {
    			get {
    				return this._Tr_IsSubmit;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tr_IsSubmit, _Tr_IsSubmit, value);
    				this._Tr_IsSubmit = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Tr_UID {
    			get {
    				return this._Tr_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tr_UID, _Tr_UID, value);
    				this._Tr_UID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Sbj_ID {
    			get {
    				return this._Sbj_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_ID, _Sbj_ID, value);
    				this._Sbj_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sbj_Name {
    			get {
    				return this._Sbj_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_Name, _Sbj_Name, value);
    				this._Sbj_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int64 Tp_Id {
    			get {
    				return this._Tp_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Id, _Tp_Id, value);
    				this._Tp_Id = value;
    			}
    		}
    		
    		public String Tp_Name {
    			get {
    				return this._Tp_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Tp_Name, _Tp_Name, value);
    				this._Tp_Name = value;
    			}
    		}
    		
    		public Int32 Ac_ID {
    			get {
    				return this._Ac_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_ID, _Ac_ID, value);
    				this._Ac_ID = value;
    			}
    		}
    		
    		public String Ac_Name {
    			get {
    				return this._Ac_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_Name, _Ac_Name, value);
    				this._Ac_Name = value;
    			}
    		}
    		
    		public Int32 Sts_ID {
    			get {
    				return this._Sts_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sts_ID, _Sts_ID, value);
    				this._Sts_ID = value;
    			}
    		}
    		
    		public String Sts_Name {
    			get {
    				return this._Sts_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sts_Name, _Sts_Name, value);
    				this._Sts_Name = value;
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
    		
    		public Int32 St_Sex {
    			get {
    				return this._St_Sex;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_Sex, _St_Sex, value);
    				this._St_Sex = value;
    			}
    		}
    		
    		public String St_IDCardNumber {
    			get {
    				return this._St_IDCardNumber;
    			}
    			set {
    				this.OnPropertyValueChange(_.St_IDCardNumber, _St_IDCardNumber, value);
    				this._St_IDCardNumber = value;
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
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<TestResults>("TestResults");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Tr_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Tr_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Tr_ID,
    					_.Tr_Score,
    					_.Tr_ScoreFinal,
    					_.Tr_Draw,
    					_.Tr_Colligate,
    					_.Tr_Results,
    					_.Tr_CrtTime,
    					_.Tr_IP,
    					_.Tr_Name,
    					_.Tr_Mac,
    					_.Tr_IsSubmit,
    					_.Tr_UID,
    					_.Sbj_ID,
    					_.Sbj_Name,
    					_.Tp_Id,
    					_.Tp_Name,
    					_.Ac_ID,
    					_.Ac_Name,
    					_.Sts_ID,
    					_.Sts_Name,
    					_.Org_ID,
    					_.Org_Name,
    					_.St_Sex,
    					_.St_IDCardNumber,
    					_.Cou_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Tr_ID,
    					this._Tr_Score,
    					this._Tr_ScoreFinal,
    					this._Tr_Draw,
    					this._Tr_Colligate,
    					this._Tr_Results,
    					this._Tr_CrtTime,
    					this._Tr_IP,
    					this._Tr_Name,
    					this._Tr_Mac,
    					this._Tr_IsSubmit,
    					this._Tr_UID,
    					this._Sbj_ID,
    					this._Sbj_Name,
    					this._Tp_Id,
    					this._Tp_Name,
    					this._Ac_ID,
    					this._Ac_Name,
    					this._Sts_ID,
    					this._Sts_Name,
    					this._Org_ID,
    					this._Org_Name,
    					this._St_Sex,
    					this._St_IDCardNumber,
    					this._Cou_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Tr_ID))) {
    				this._Tr_ID = reader.GetInt32(_.Tr_ID);
    			}
    			if ((false == reader.IsDBNull(_.Tr_Score))) {
    				this._Tr_Score = reader.GetFloat(_.Tr_Score);
    			}
    			if ((false == reader.IsDBNull(_.Tr_ScoreFinal))) {
    				this._Tr_ScoreFinal = reader.GetFloat(_.Tr_ScoreFinal);
    			}
    			if ((false == reader.IsDBNull(_.Tr_Draw))) {
    				this._Tr_Draw = reader.GetFloat(_.Tr_Draw);
    			}
    			if ((false == reader.IsDBNull(_.Tr_Colligate))) {
    				this._Tr_Colligate = reader.GetFloat(_.Tr_Colligate);
    			}
    			if ((false == reader.IsDBNull(_.Tr_Results))) {
    				this._Tr_Results = reader.GetString(_.Tr_Results);
    			}
    			if ((false == reader.IsDBNull(_.Tr_CrtTime))) {
    				this._Tr_CrtTime = reader.GetDateTime(_.Tr_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Tr_IP))) {
    				this._Tr_IP = reader.GetString(_.Tr_IP);
    			}
    			if ((false == reader.IsDBNull(_.Tr_Name))) {
    				this._Tr_Name = reader.GetString(_.Tr_Name);
    			}
    			if ((false == reader.IsDBNull(_.Tr_Mac))) {
    				this._Tr_Mac = reader.GetString(_.Tr_Mac);
    			}
    			if ((false == reader.IsDBNull(_.Tr_IsSubmit))) {
    				this._Tr_IsSubmit = reader.GetBoolean(_.Tr_IsSubmit);
    			}
    			if ((false == reader.IsDBNull(_.Tr_UID))) {
    				this._Tr_UID = reader.GetString(_.Tr_UID);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt32(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_Name))) {
    				this._Sbj_Name = reader.GetString(_.Sbj_Name);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Id))) {
    				this._Tp_Id = reader.GetInt64(_.Tp_Id);
    			}
    			if ((false == reader.IsDBNull(_.Tp_Name))) {
    				this._Tp_Name = reader.GetString(_.Tp_Name);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_Name))) {
    				this._Ac_Name = reader.GetString(_.Ac_Name);
    			}
    			if ((false == reader.IsDBNull(_.Sts_ID))) {
    				this._Sts_ID = reader.GetInt32(_.Sts_ID);
    			}
    			if ((false == reader.IsDBNull(_.Sts_Name))) {
    				this._Sts_Name = reader.GetString(_.Sts_Name);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.St_Sex))) {
    				this._St_Sex = reader.GetInt32(_.St_Sex);
    			}
    			if ((false == reader.IsDBNull(_.St_IDCardNumber))) {
    				this._St_IDCardNumber = reader.GetString(_.St_IDCardNumber);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt64(_.Cou_ID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(TestResults).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<TestResults>();
    			
    			/// <summary>
    			/// -1 - 字段名：Tr_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Tr_ID = new WeiSha.Data.Field<TestResults>("Tr_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Tr_Score - 数据类型：Single(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Tr_Score = new WeiSha.Data.Field<TestResults>("Tr_Score");
    			
    			/// <summary>
    			/// -1 - 字段名：Tr_ScoreFinal - 数据类型：Single(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Tr_ScoreFinal = new WeiSha.Data.Field<TestResults>("Tr_ScoreFinal");
    			
    			/// <summary>
    			/// -1 - 字段名：Tr_Draw - 数据类型：Single(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Tr_Draw = new WeiSha.Data.Field<TestResults>("Tr_Draw");
    			
    			/// <summary>
    			/// -1 - 字段名：Tr_Colligate - 数据类型：Single(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Tr_Colligate = new WeiSha.Data.Field<TestResults>("Tr_Colligate");
    			
    			/// <summary>
    			/// -1 - 字段名：Tr_Results - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tr_Results = new WeiSha.Data.Field<TestResults>("Tr_Results");
    			
    			/// <summary>
    			/// -1 - 字段名：Tr_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Tr_CrtTime = new WeiSha.Data.Field<TestResults>("Tr_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Tr_IP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tr_IP = new WeiSha.Data.Field<TestResults>("Tr_IP");
    			
    			/// <summary>
    			/// 字段名：Tr_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tr_Name = new WeiSha.Data.Field<TestResults>("Tr_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Tr_Mac - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tr_Mac = new WeiSha.Data.Field<TestResults>("Tr_Mac");
    			
    			/// <summary>
    			/// -1 - 字段名：Tr_IsSubmit - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Tr_IsSubmit = new WeiSha.Data.Field<TestResults>("Tr_IsSubmit");
    			
    			/// <summary>
    			/// -1 - 字段名：Tr_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tr_UID = new WeiSha.Data.Field<TestResults>("Tr_UID");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_ID - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<TestResults>("Sbj_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_Name = new WeiSha.Data.Field<TestResults>("Sbj_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Tp_Id - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Id = new WeiSha.Data.Field<TestResults>("Tp_Id");
    			
    			/// <summary>
    			/// 字段名：Tp_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Tp_Name = new WeiSha.Data.Field<TestResults>("Tp_Name");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<TestResults>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ac_Name = new WeiSha.Data.Field<TestResults>("Ac_Name");
    			
    			/// <summary>
    			/// 字段名：Sts_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sts_ID = new WeiSha.Data.Field<TestResults>("Sts_ID");
    			
    			/// <summary>
    			/// 字段名：Sts_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sts_Name = new WeiSha.Data.Field<TestResults>("Sts_Name");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<TestResults>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<TestResults>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：St_Sex - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field St_Sex = new WeiSha.Data.Field<TestResults>("St_Sex");
    			
    			/// <summary>
    			/// 字段名：St_IDCardNumber - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field St_IDCardNumber = new WeiSha.Data.Field<TestResults>("St_IDCardNumber");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<TestResults>("Cou_ID");
    		}
    	}
    }
    