namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：LearningCardSet_Courses 主键列：Lcsc_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class LearningCardSet_Courses : WeiSha.Data.Entity {
    		
    		protected Int32 _Lcsc_ID;
    		
    		protected Int32 _Cou_ID;
    		
    		protected Int32 _Lc_ID;
    		
    		public Int32 Lcsc_ID {
    			get {
    				return this._Lcsc_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lcsc_ID, _Lcsc_ID, value);
    				this._Lcsc_ID = value;
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
    		
    		public Int32 Lc_ID {
    			get {
    				return this._Lc_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Lc_ID, _Lc_ID, value);
    				this._Lc_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<LearningCardSet_Courses>("LearningCardSet_Courses");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Lc_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lcsc_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Lcsc_ID,
    					_.Cou_ID,
    					_.Lc_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Lcsc_ID,
    					this._Cou_ID,
    					this._Lc_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Lcsc_ID))) {
    				this._Lcsc_ID = reader.GetInt32(_.Lcsc_ID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Lc_ID))) {
    				this._Lc_ID = reader.GetInt32(_.Lc_ID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(LearningCardSet_Courses).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<LearningCardSet_Courses>();
    			
    			/// <summary>
    			/// 字段名：Lcsc_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lcsc_ID = new WeiSha.Data.Field<LearningCardSet_Courses>("Lcsc_ID");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<LearningCardSet_Courses>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Lc_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Lc_ID = new WeiSha.Data.Field<LearningCardSet_Courses>("Lc_ID");
    		}
    	}
    }
    