namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：EmpAcc_Group 主键列：Emgr_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class EmpAcc_Group : WeiSha.Data.Entity {
    		
    		protected Int32 _Emgr_Id;
    		
    		protected Int32? _Acc_Id;
    		
    		protected Int32? _EGrp_Id;
    		
    		protected Int32? _Org_Id;
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32 Emgr_Id {
    			get {
    				return this._Emgr_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Emgr_Id, _Emgr_Id, value);
    				this._Emgr_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Acc_Id {
    			get {
    				return this._Acc_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Id, _Acc_Id, value);
    				this._Acc_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? EGrp_Id {
    			get {
    				return this._EGrp_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.EGrp_Id, _EGrp_Id, value);
    				this._EGrp_Id = value;
    			}
    		}
    		
    		public Int32? Org_Id {
    			get {
    				return this._Org_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Id, _Org_Id, value);
    				this._Org_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<EmpAcc_Group>("EmpAcc_Group");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Emgr_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Emgr_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Emgr_Id,
    					_.Acc_Id,
    					_.EGrp_Id,
    					_.Org_Id};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Emgr_Id,
    					this._Acc_Id,
    					this._EGrp_Id,
    					this._Org_Id};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Emgr_Id))) {
    				this._Emgr_Id = reader.GetInt32(_.Emgr_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
    			}
    			if ((false == reader.IsDBNull(_.EGrp_Id))) {
    				this._EGrp_Id = reader.GetInt32(_.EGrp_Id);
    			}
    			if ((false == reader.IsDBNull(_.Org_Id))) {
    				this._Org_Id = reader.GetInt32(_.Org_Id);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(EmpAcc_Group).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<EmpAcc_Group>();
    			
    			/// <summary>
    			/// False - 字段名：Emgr_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Emgr_Id = new WeiSha.Data.Field<EmpAcc_Group>("Emgr_Id");
    			
    			/// <summary>
    			/// False - 字段名：Acc_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<EmpAcc_Group>("Acc_Id");
    			
    			/// <summary>
    			/// False - 字段名：EGrp_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field EGrp_Id = new WeiSha.Data.Field<EmpAcc_Group>("EGrp_Id");
    			
    			/// <summary>
    			/// 字段名：Org_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Org_Id = new WeiSha.Data.Field<EmpAcc_Group>("Org_Id");
    		}
    	}
    }
    