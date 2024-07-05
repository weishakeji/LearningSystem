namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：QuesAnswer 主键列：
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class QuesAnswer : WeiSha.Data.Entity {
    		
    		protected String _Ans_Context;
    		
    		protected Int64 _Ans_ID;
    		
    		protected Boolean _Ans_IsCorrect;
    		
    		protected Int64 _Qus_ID;
    		
    		protected String _Qus_UID;
    		
    		public String Ans_Context {
    			get {
    				return this._Ans_Context;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ans_Context, _Ans_Context, value);
    				this._Ans_Context = value;
    			}
    		}
    		
    		public Int64 Ans_ID {
    			get {
    				return this._Ans_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ans_ID, _Ans_ID, value);
    				this._Ans_ID = value;
    			}
    		}
    		
    		public Boolean Ans_IsCorrect {
    			get {
    				return this._Ans_IsCorrect;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ans_IsCorrect, _Ans_IsCorrect, value);
    				this._Ans_IsCorrect = value;
    			}
    		}
    		
    		public Int64 Qus_ID {
    			get {
    				return this._Qus_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_ID, _Qus_ID, value);
    				this._Qus_ID = value;
    			}
    		}
    		
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
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<QuesAnswer>("QuesAnswer");
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ans_Context,
    					_.Ans_ID,
    					_.Ans_IsCorrect,
    					_.Qus_ID,
    					_.Qus_UID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Ans_Context,
    					this._Ans_ID,
    					this._Ans_IsCorrect,
    					this._Qus_ID,
    					this._Qus_UID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Ans_Context))) {
    				this._Ans_Context = reader.GetString(_.Ans_Context);
    			}
    			if ((false == reader.IsDBNull(_.Ans_ID))) {
    				this._Ans_ID = reader.GetInt64(_.Ans_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ans_IsCorrect))) {
    				this._Ans_IsCorrect = reader.GetBoolean(_.Ans_IsCorrect);
    			}
    			if ((false == reader.IsDBNull(_.Qus_ID))) {
    				this._Qus_ID = reader.GetInt64(_.Qus_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qus_UID))) {
    				this._Qus_UID = reader.GetString(_.Qus_UID);
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
    			/// 字段名：Ans_Context - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ans_Context = new WeiSha.Data.Field<QuesAnswer>("Ans_Context");
    			
    			/// <summary>
    			/// 字段名：Ans_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Ans_ID = new WeiSha.Data.Field<QuesAnswer>("Ans_ID");
    			
    			/// <summary>
    			/// 字段名：Ans_IsCorrect - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Ans_IsCorrect = new WeiSha.Data.Field<QuesAnswer>("Ans_IsCorrect");
    			
    			/// <summary>
    			/// 字段名：Qus_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Qus_ID = new WeiSha.Data.Field<QuesAnswer>("Qus_ID");
    			
    			/// <summary>
    			/// 字段名：Qus_UID - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Qus_UID = new WeiSha.Data.Field<QuesAnswer>("Qus_UID");
    		}
    	}
    }
