namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：QuesTypes 主键列：Qt_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class QuesTypes : WeiSha.Data.Entity {
    		
    		protected Int32 _Qt_ID;
    		
    		protected String _Qt_Name;
    		
    		protected String _Qt_Intro;
    		
    		protected Boolean _Qt_IsUse;
    		
    		protected Int32 _Qt_Type;
    		
    		protected String _Qt_TypeName;
    		
    		protected Int32 _Qt_Tax;
    		
    		protected Int32 _Qt_Count;
    		
    		protected Int32 _Cou_ID;
    		
    		protected Int32 _Org_ID;
    		
    		public Int32 Qt_ID {
    			get {
    				return this._Qt_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qt_ID, _Qt_ID, value);
    				this._Qt_ID = value;
    			}
    		}
    		
    		public String Qt_Name {
    			get {
    				return this._Qt_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qt_Name, _Qt_Name, value);
    				this._Qt_Name = value;
    			}
    		}
    		
    		public String Qt_Intro {
    			get {
    				return this._Qt_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qt_Intro, _Qt_Intro, value);
    				this._Qt_Intro = value;
    			}
    		}
    		
    		public Boolean Qt_IsUse {
    			get {
    				return this._Qt_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qt_IsUse, _Qt_IsUse, value);
    				this._Qt_IsUse = value;
    			}
    		}
    		
    		public Int32 Qt_Type {
    			get {
    				return this._Qt_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qt_Type, _Qt_Type, value);
    				this._Qt_Type = value;
    			}
    		}
    		
    		public String Qt_TypeName {
    			get {
    				return this._Qt_TypeName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qt_TypeName, _Qt_TypeName, value);
    				this._Qt_TypeName = value;
    			}
    		}
    		
    		public Int32 Qt_Tax {
    			get {
    				return this._Qt_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qt_Tax, _Qt_Tax, value);
    				this._Qt_Tax = value;
    			}
    		}
    		
    		public Int32 Qt_Count {
    			get {
    				return this._Qt_Count;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qt_Count, _Qt_Count, value);
    				this._Qt_Count = value;
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
    		
    		public Int32 Org_ID {
    			get {
    				return this._Org_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_ID, _Org_ID, value);
    				this._Org_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<QuesTypes>("QuesTypes");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Qt_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Qt_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Qt_ID,
    					_.Qt_Name,
    					_.Qt_Intro,
    					_.Qt_IsUse,
    					_.Qt_Type,
    					_.Qt_TypeName,
    					_.Qt_Tax,
    					_.Qt_Count,
    					_.Cou_ID,
    					_.Org_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Qt_ID,
    					this._Qt_Name,
    					this._Qt_Intro,
    					this._Qt_IsUse,
    					this._Qt_Type,
    					this._Qt_TypeName,
    					this._Qt_Tax,
    					this._Qt_Count,
    					this._Cou_ID,
    					this._Org_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Qt_ID))) {
    				this._Qt_ID = reader.GetInt32(_.Qt_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qt_Name))) {
    				this._Qt_Name = reader.GetString(_.Qt_Name);
    			}
    			if ((false == reader.IsDBNull(_.Qt_Intro))) {
    				this._Qt_Intro = reader.GetString(_.Qt_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Qt_IsUse))) {
    				this._Qt_IsUse = reader.GetBoolean(_.Qt_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Qt_Type))) {
    				this._Qt_Type = reader.GetInt32(_.Qt_Type);
    			}
    			if ((false == reader.IsDBNull(_.Qt_TypeName))) {
    				this._Qt_TypeName = reader.GetString(_.Qt_TypeName);
    			}
    			if ((false == reader.IsDBNull(_.Qt_Tax))) {
    				this._Qt_Tax = reader.GetInt32(_.Qt_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Qt_Count))) {
    				this._Qt_Count = reader.GetInt32(_.Qt_Count);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(QuesTypes).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<QuesTypes>();
    			
    			/// <summary>
    			/// 字段名：Qt_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qt_ID = new WeiSha.Data.Field<QuesTypes>("Qt_ID");
    			
    			/// <summary>
    			/// 字段名：Qt_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Qt_Name = new WeiSha.Data.Field<QuesTypes>("Qt_Name");
    			
    			/// <summary>
    			/// 字段名：Qt_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Qt_Intro = new WeiSha.Data.Field<QuesTypes>("Qt_Intro");
    			
    			/// <summary>
    			/// 字段名：Qt_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Qt_IsUse = new WeiSha.Data.Field<QuesTypes>("Qt_IsUse");
    			
    			/// <summary>
    			/// 字段名：Qt_Type - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qt_Type = new WeiSha.Data.Field<QuesTypes>("Qt_Type");
    			
    			/// <summary>
    			/// 字段名：Qt_TypeName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Qt_TypeName = new WeiSha.Data.Field<QuesTypes>("Qt_TypeName");
    			
    			/// <summary>
    			/// 字段名：Qt_Tax - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qt_Tax = new WeiSha.Data.Field<QuesTypes>("Qt_Tax");
    			
    			/// <summary>
    			/// 字段名：Qt_Count - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qt_Count = new WeiSha.Data.Field<QuesTypes>("Qt_Count");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<QuesTypes>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<QuesTypes>("Org_ID");
    		}
    	}
    }
    