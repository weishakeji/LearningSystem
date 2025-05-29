namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：LlmRecords 主键列：Llr_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class LlmRecords : WeiSha.Data.Entity {
    		
    		protected Int32 _Llr_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected DateTime _Llr_CrtTime;
    		
    		protected DateTime _Llr_LastTime;
    		
    		protected String _Llr_Records;
    		
    		protected String _Llr_Topic;
    		
    		public Int32 Llr_ID {
    			get {
    				return this._Llr_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Llr_ID, _Llr_ID, value);
    				this._Llr_ID = value;
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
    		
    		public DateTime Llr_CrtTime {
    			get {
    				return this._Llr_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Llr_CrtTime, _Llr_CrtTime, value);
    				this._Llr_CrtTime = value;
    			}
    		}
    		
    		public DateTime Llr_LastTime {
    			get {
    				return this._Llr_LastTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Llr_LastTime, _Llr_LastTime, value);
    				this._Llr_LastTime = value;
    			}
    		}
    		
    		public String Llr_Records {
    			get {
    				return this._Llr_Records;
    			}
    			set {
    				this.OnPropertyValueChange(_.Llr_Records, _Llr_Records, value);
    				this._Llr_Records = value;
    			}
    		}
    		
    		public String Llr_Topic {
    			get {
    				return this._Llr_Topic;
    			}
    			set {
    				this.OnPropertyValueChange(_.Llr_Topic, _Llr_Topic, value);
    				this._Llr_Topic = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<LlmRecords>("LlmRecords");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Llr_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Llr_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Llr_ID,
    					_.Ac_ID,
    					_.Llr_CrtTime,
    					_.Llr_LastTime,
    					_.Llr_Records,
    					_.Llr_Topic};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Llr_ID,
    					this._Ac_ID,
    					this._Llr_CrtTime,
    					this._Llr_LastTime,
    					this._Llr_Records,
    					this._Llr_Topic};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Llr_ID))) {
    				this._Llr_ID = reader.GetInt32(_.Llr_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Llr_CrtTime))) {
    				this._Llr_CrtTime = reader.GetDateTime(_.Llr_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Llr_LastTime))) {
    				this._Llr_LastTime = reader.GetDateTime(_.Llr_LastTime);
    			}
    			if ((false == reader.IsDBNull(_.Llr_Records))) {
    				this._Llr_Records = reader.GetString(_.Llr_Records);
    			}
    			if ((false == reader.IsDBNull(_.Llr_Topic))) {
    				this._Llr_Topic = reader.GetString(_.Llr_Topic);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(LlmRecords).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<LlmRecords>();
    			
    			/// <summary>
    			/// 字段名：Llr_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Llr_ID = new WeiSha.Data.Field<LlmRecords>("Llr_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<LlmRecords>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Llr_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Llr_CrtTime = new WeiSha.Data.Field<LlmRecords>("Llr_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Llr_LastTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Llr_LastTime = new WeiSha.Data.Field<LlmRecords>("Llr_LastTime");
    			
    			/// <summary>
    			/// 字段名：Llr_Records - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Llr_Records = new WeiSha.Data.Field<LlmRecords>("Llr_Records");
    			
    			/// <summary>
    			/// 字段名：Llr_Topic - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Llr_Topic = new WeiSha.Data.Field<LlmRecords>("Llr_Topic");
    		}
    	}
    }
