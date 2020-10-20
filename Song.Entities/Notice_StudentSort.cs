namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Notice_StudentSort 主键列：Nos_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Notice_StudentSort : WeiSha.Data.Entity {
    		
    		protected Int32 _Nos_ID;
    		
    		protected Int32 _No_Id;
    		
    		protected Int32 _Sts_ID;
    		
    		public Int32 Nos_ID {
    			get {
    				return this._Nos_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Nos_ID, _Nos_ID, value);
    				this._Nos_ID = value;
    			}
    		}
    		
    		public Int32 No_Id {
    			get {
    				return this._No_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.No_Id, _No_Id, value);
    				this._No_Id = value;
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
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Notice_StudentSort>("Notice_StudentSort");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Nos_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Nos_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Nos_ID,
    					_.No_Id,
    					_.Sts_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Nos_ID,
    					this._No_Id,
    					this._Sts_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Nos_ID))) {
    				this._Nos_ID = reader.GetInt32(_.Nos_ID);
    			}
    			if ((false == reader.IsDBNull(_.No_Id))) {
    				this._No_Id = reader.GetInt32(_.No_Id);
    			}
    			if ((false == reader.IsDBNull(_.Sts_ID))) {
    				this._Sts_ID = reader.GetInt32(_.Sts_ID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Notice_StudentSort).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Notice_StudentSort>();
    			
    			/// <summary>
    			/// 字段名：Nos_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Nos_ID = new WeiSha.Data.Field<Notice_StudentSort>("Nos_ID");
    			
    			/// <summary>
    			/// 字段名：No_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field No_Id = new WeiSha.Data.Field<Notice_StudentSort>("No_Id");
    			
    			/// <summary>
    			/// 字段名：Sts_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sts_ID = new WeiSha.Data.Field<Notice_StudentSort>("Sts_ID");
    		}
    	}
    }
