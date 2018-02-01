namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Depart_Subject 主键列：DepSub_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Depart_Subject : WeiSha.Data.Entity {
    		
    		protected Int32 _DepSub_ID;
    		
    		protected Int32? _Dep_Id;
    		
    		protected Int32? _Sbj_ID;
    		
    		protected Int32? _Org_Id;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 DepSub_ID {
    			get {
    				return this._DepSub_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.DepSub_ID, _DepSub_ID, value);
    				this._DepSub_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Dep_Id {
    			get {
    				return this._Dep_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Dep_Id, _Dep_Id, value);
    				this._Dep_Id = value;
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
    		
    		public Int32? Org_Id {
    			get {
    				return this._Org_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Id, _Org_Id, value);
    				this._Org_Id = value;
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
    			return new WeiSha.Data.Table<Depart_Subject>("Depart_Subject");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.DepSub_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.DepSub_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.DepSub_ID,
    					_.Dep_Id,
    					_.Sbj_ID,
    					_.Org_Id,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._DepSub_ID,
    					this._Dep_Id,
    					this._Sbj_ID,
    					this._Org_Id,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.DepSub_ID))) {
    				this._DepSub_ID = reader.GetInt32(_.DepSub_ID);
    			}
    			if ((false == reader.IsDBNull(_.Dep_Id))) {
    				this._Dep_Id = reader.GetInt32(_.Dep_Id);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt32(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_Id))) {
    				this._Org_Id = reader.GetInt32(_.Org_Id);
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
    			if ((false == typeof(Depart_Subject).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Depart_Subject>();
    			
    			/// <summary>
    			/// -1 - 字段名：DepSub_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field DepSub_ID = new WeiSha.Data.Field<Depart_Subject>("DepSub_ID");
    			
    			/// <summary>
    			/// -1 - 字段名：Dep_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Dep_Id = new WeiSha.Data.Field<Depart_Subject>("Dep_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Sbj_ID - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<Depart_Subject>("Sbj_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Org_Id = new WeiSha.Data.Field<Depart_Subject>("Org_Id");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Depart_Subject>("Org_Name");
    		}
    	}
    }
    