namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Teacher_Course 主键列：Thc_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Teacher_Course : WeiSha.Data.Entity {
    		
    		protected Int32 _Thc_ID;
    		
    		protected Int32 _Th_ID;
    		
    		protected Int32 _Cou_ID;
    		
    		public Int32 Thc_ID {
    			get {
    				return this._Thc_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Thc_ID, _Thc_ID, value);
    				this._Thc_ID = value;
    			}
    		}
    		
    		public Int32 Th_ID {
    			get {
    				return this._Th_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Th_ID, _Th_ID, value);
    				this._Th_ID = value;
    			}
    		}
    		
    		public Int32 Cou_ID {
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
    			return new WeiSha.Data.Table<Teacher_Course>("Teacher_Course");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Thc_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Thc_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Thc_ID,
    					_.Th_ID,
    					_.Cou_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Thc_ID,
    					this._Th_ID,
    					this._Cou_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Thc_ID))) {
    				this._Thc_ID = reader.GetInt32(_.Thc_ID);
    			}
    			if ((false == reader.IsDBNull(_.Th_ID))) {
    				this._Th_ID = reader.GetInt32(_.Th_ID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Teacher_Course).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Teacher_Course>();
    			
    			/// <summary>
    			/// 字段名：Thc_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Thc_ID = new WeiSha.Data.Field<Teacher_Course>("Thc_ID");
    			
    			/// <summary>
    			/// 字段名：Th_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Th_ID = new WeiSha.Data.Field<Teacher_Course>("Th_ID");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<Teacher_Course>("Cou_ID");
    		}
    	}
    }
    