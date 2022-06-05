namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ManageMenu_Point 主键列：MMP_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ManageMenu_Point : WeiSha.Data.Entity {
    		
    		protected Int32 _MMP_Id;
    		
    		protected String _MMP_FileName;
    		
    		protected Boolean _MMP_IsUse;
    		
    		protected Boolean _MMP_IsShow;
    		
    		protected Int16? _MM_Id;
    		
    		protected Int16? _FPI_Id;
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32 MMP_Id {
    			get {
    				return this._MMP_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.MMP_Id, _MMP_Id, value);
    				this._MMP_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// True
    		/// </summary>
    		public String MMP_FileName {
    			get {
    				return this._MMP_FileName;
    			}
    			set {
    				this.OnPropertyValueChange(_.MMP_FileName, _MMP_FileName, value);
    				this._MMP_FileName = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Boolean MMP_IsUse {
    			get {
    				return this._MMP_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.MMP_IsUse, _MMP_IsUse, value);
    				this._MMP_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Boolean MMP_IsShow {
    			get {
    				return this._MMP_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.MMP_IsShow, _MMP_IsShow, value);
    				this._MMP_IsShow = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int16? MM_Id {
    			get {
    				return this._MM_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.MM_Id, _MM_Id, value);
    				this._MM_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int16? FPI_Id {
    			get {
    				return this._FPI_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.FPI_Id, _FPI_Id, value);
    				this._FPI_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<ManageMenu_Point>("ManageMenu_Point");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.MMP_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.MMP_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.MMP_Id,
    					_.MMP_FileName,
    					_.MMP_IsUse,
    					_.MMP_IsShow,
    					_.MM_Id,
    					_.FPI_Id};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._MMP_Id,
    					this._MMP_FileName,
    					this._MMP_IsUse,
    					this._MMP_IsShow,
    					this._MM_Id,
    					this._FPI_Id};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.MMP_Id))) {
    				this._MMP_Id = reader.GetInt32(_.MMP_Id);
    			}
    			if ((false == reader.IsDBNull(_.MMP_FileName))) {
    				this._MMP_FileName = reader.GetString(_.MMP_FileName);
    			}
    			if ((false == reader.IsDBNull(_.MMP_IsUse))) {
    				this._MMP_IsUse = reader.GetBoolean(_.MMP_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.MMP_IsShow))) {
    				this._MMP_IsShow = reader.GetBoolean(_.MMP_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.MM_Id))) {
    				this._MM_Id = reader.GetInt16(_.MM_Id);
    			}
    			if ((false == reader.IsDBNull(_.FPI_Id))) {
    				this._FPI_Id = reader.GetInt16(_.FPI_Id);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(ManageMenu_Point).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<ManageMenu_Point>();
    			
    			/// <summary>
    			/// False - 字段名：MMP_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field MMP_Id = new WeiSha.Data.Field<ManageMenu_Point>("MMP_Id");
    			
    			/// <summary>
    			/// True - 字段名：MMP_FileName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field MMP_FileName = new WeiSha.Data.Field<ManageMenu_Point>("MMP_FileName");
    			
    			/// <summary>
    			/// False - 字段名：MMP_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MMP_IsUse = new WeiSha.Data.Field<ManageMenu_Point>("MMP_IsUse");
    			
    			/// <summary>
    			/// False - 字段名：MMP_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field MMP_IsShow = new WeiSha.Data.Field<ManageMenu_Point>("MMP_IsShow");
    			
    			/// <summary>
    			/// False - 字段名：MM_Id - 数据类型：Int16(可空)
    			/// </summary>
    			public static WeiSha.Data.Field MM_Id = new WeiSha.Data.Field<ManageMenu_Point>("MM_Id");
    			
    			/// <summary>
    			/// False - 字段名：FPI_Id - 数据类型：Int16(可空)
    			/// </summary>
    			public static WeiSha.Data.Field FPI_Id = new WeiSha.Data.Field<ManageMenu_Point>("FPI_Id");
    		}
    	}
    }
    