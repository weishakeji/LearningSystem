namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：QuesAnswer 主键列：Ans_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class QuesAnswer : WeiSha.Data.Entity {
    		
    		protected Int32 _Ans_ID;
    		
    		protected Int32 _Qus_ID;
    		
    		protected String _Qus_UID;
    		
    		protected String _Ans_Context;
    		
    		protected Boolean _Ans_IsCorrect;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Ans_ID {
    			get {
    				return this._Ans_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ans_ID, _Ans_ID, value);
    				this._Ans_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Qus_ID {
    			get {
    				return this._Qus_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_ID, _Qus_ID, value);
    				this._Qus_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Qus_UID {
    			get {
    				return this._Qus_UID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_UID, _Qus_UID, value);
    				this._Qus_UID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Ans_Context {
    			get {
    				return this._Ans_Context;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ans_Context, _Ans_Context, value);
    				this._Ans_Context = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Ans_IsCorrect {
    			get {
    				return this._Ans_IsCorrect;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ans_IsCorrect, _Ans_IsCorrect, value);
    				this._Ans_IsCorrect = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<QuesAnswer>("QuesAnswer");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Ans_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ans_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ans_ID,
    					_.Qus_ID,
    					_.Qus_UID,
    					_.Ans_Context,
    					_.Ans_IsCorrect};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Ans_ID,
    					this._Qus_ID,
    					this._Qus_UID,
    					this._Ans_Context,
    					this._Ans_IsCorrect};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Ans_ID))) {
    				this._Ans_ID = reader.GetInt32(_.Ans_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qus_ID))) {
    				this._Qus_ID = reader.GetInt32(_.Qus_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qus_UID))) {
    				this._Qus_UID = reader.GetString(_.Qus_UID);
    			}
    			if ((false == reader.IsDBNull(_.Ans_Context))) {
    				this._Ans_Context = reader.GetString(_.Ans_Context);
    			}
    			if ((false == reader.IsDBNull(_.Ans_IsCorrect))) {
    				this._Ans_IsCorrect = reader.GetBoolean(_.Ans_IsCorrect);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(QuesAnswer).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<QuesAnswer>();
    			
    			/// <summary>
    			/// -1 - 字段名：Ans_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ans_ID = new WeiSha.Data.Field<QuesAnswer>("Ans_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_ID = new WeiSha.Data.Field<QuesAnswer>("Qus_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Qus_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Qus_UID = new WeiSha.Data.Field<QuesAnswer>("Qus_UID");
    			
    			/// <summary>
    			/// -1 - 字段名：Ans_Context - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ans_Context = new WeiSha.Data.Field<QuesAnswer>("Ans_Context");
    			
    			/// <summary>
    			/// -1 - 字段名：Ans_IsCorrect - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ans_IsCorrect = new WeiSha.Data.Field<QuesAnswer>("Ans_IsCorrect");
    		}
    	}
    }
    