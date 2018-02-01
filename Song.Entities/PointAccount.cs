namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：PointAccount 主键列：Pa_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class PointAccount : WeiSha.Data.Entity {
    		
    		protected Int32 _Pa_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected Int32 _Pa_Total;
    		
    		protected Int32 _Pa_TotalAmount;
    		
    		protected Int32 _Pa_Value;
    		
    		protected String _Pa_Source;
    		
    		protected Int32 _Pa_Type;
    		
    		protected String _Pa_Info;
    		
    		protected String _Pa_Remark;
    		
    		protected DateTime _Pa_CrtTime;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Pa_Serial;
    		
    		protected Int32 _Pa_From;
    		
    		public Int32 Pa_ID {
    			get {
    				return this._Pa_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pa_ID, _Pa_ID, value);
    				this._Pa_ID = value;
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
    		
    		public Int32 Pa_Total {
    			get {
    				return this._Pa_Total;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pa_Total, _Pa_Total, value);
    				this._Pa_Total = value;
    			}
    		}
    		
    		public Int32 Pa_TotalAmount {
    			get {
    				return this._Pa_TotalAmount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pa_TotalAmount, _Pa_TotalAmount, value);
    				this._Pa_TotalAmount = value;
    			}
    		}
    		
    		public Int32 Pa_Value {
    			get {
    				return this._Pa_Value;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pa_Value, _Pa_Value, value);
    				this._Pa_Value = value;
    			}
    		}
    		
    		public String Pa_Source {
    			get {
    				return this._Pa_Source;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pa_Source, _Pa_Source, value);
    				this._Pa_Source = value;
    			}
    		}
    		
    		public Int32 Pa_Type {
    			get {
    				return this._Pa_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pa_Type, _Pa_Type, value);
    				this._Pa_Type = value;
    			}
    		}
    		
    		public String Pa_Info {
    			get {
    				return this._Pa_Info;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pa_Info, _Pa_Info, value);
    				this._Pa_Info = value;
    			}
    		}
    		
    		public String Pa_Remark {
    			get {
    				return this._Pa_Remark;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pa_Remark, _Pa_Remark, value);
    				this._Pa_Remark = value;
    			}
    		}
    		
    		public DateTime Pa_CrtTime {
    			get {
    				return this._Pa_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pa_CrtTime, _Pa_CrtTime, value);
    				this._Pa_CrtTime = value;
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
    		
    		public String Pa_Serial {
    			get {
    				return this._Pa_Serial;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pa_Serial, _Pa_Serial, value);
    				this._Pa_Serial = value;
    			}
    		}
    		
    		public Int32 Pa_From {
    			get {
    				return this._Pa_From;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pa_From, _Pa_From, value);
    				this._Pa_From = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<PointAccount>("PointAccount");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Pa_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pa_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pa_ID,
    					_.Ac_ID,
    					_.Pa_Total,
    					_.Pa_TotalAmount,
    					_.Pa_Value,
    					_.Pa_Source,
    					_.Pa_Type,
    					_.Pa_Info,
    					_.Pa_Remark,
    					_.Pa_CrtTime,
    					_.Org_ID,
    					_.Pa_Serial,
    					_.Pa_From};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Pa_ID,
    					this._Ac_ID,
    					this._Pa_Total,
    					this._Pa_TotalAmount,
    					this._Pa_Value,
    					this._Pa_Source,
    					this._Pa_Type,
    					this._Pa_Info,
    					this._Pa_Remark,
    					this._Pa_CrtTime,
    					this._Org_ID,
    					this._Pa_Serial,
    					this._Pa_From};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Pa_ID))) {
    				this._Pa_ID = reader.GetInt32(_.Pa_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Pa_Total))) {
    				this._Pa_Total = reader.GetInt32(_.Pa_Total);
    			}
    			if ((false == reader.IsDBNull(_.Pa_TotalAmount))) {
    				this._Pa_TotalAmount = reader.GetInt32(_.Pa_TotalAmount);
    			}
    			if ((false == reader.IsDBNull(_.Pa_Value))) {
    				this._Pa_Value = reader.GetInt32(_.Pa_Value);
    			}
    			if ((false == reader.IsDBNull(_.Pa_Source))) {
    				this._Pa_Source = reader.GetString(_.Pa_Source);
    			}
    			if ((false == reader.IsDBNull(_.Pa_Type))) {
    				this._Pa_Type = reader.GetInt32(_.Pa_Type);
    			}
    			if ((false == reader.IsDBNull(_.Pa_Info))) {
    				this._Pa_Info = reader.GetString(_.Pa_Info);
    			}
    			if ((false == reader.IsDBNull(_.Pa_Remark))) {
    				this._Pa_Remark = reader.GetString(_.Pa_Remark);
    			}
    			if ((false == reader.IsDBNull(_.Pa_CrtTime))) {
    				this._Pa_CrtTime = reader.GetDateTime(_.Pa_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Pa_Serial))) {
    				this._Pa_Serial = reader.GetString(_.Pa_Serial);
    			}
    			if ((false == reader.IsDBNull(_.Pa_From))) {
    				this._Pa_From = reader.GetInt32(_.Pa_From);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(PointAccount).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<PointAccount>();
    			
    			/// <summary>
    			/// 字段名：Pa_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pa_ID = new WeiSha.Data.Field<PointAccount>("Pa_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<PointAccount>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Pa_Total - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pa_Total = new WeiSha.Data.Field<PointAccount>("Pa_Total");
    			
    			/// <summary>
    			/// 字段名：Pa_TotalAmount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pa_TotalAmount = new WeiSha.Data.Field<PointAccount>("Pa_TotalAmount");
    			
    			/// <summary>
    			/// 字段名：Pa_Value - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pa_Value = new WeiSha.Data.Field<PointAccount>("Pa_Value");
    			
    			/// <summary>
    			/// 字段名：Pa_Source - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pa_Source = new WeiSha.Data.Field<PointAccount>("Pa_Source");
    			
    			/// <summary>
    			/// 字段名：Pa_Type - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pa_Type = new WeiSha.Data.Field<PointAccount>("Pa_Type");
    			
    			/// <summary>
    			/// 字段名：Pa_Info - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pa_Info = new WeiSha.Data.Field<PointAccount>("Pa_Info");
    			
    			/// <summary>
    			/// 字段名：Pa_Remark - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pa_Remark = new WeiSha.Data.Field<PointAccount>("Pa_Remark");
    			
    			/// <summary>
    			/// 字段名：Pa_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Pa_CrtTime = new WeiSha.Data.Field<PointAccount>("Pa_CrtTime");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<PointAccount>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Pa_Serial - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pa_Serial = new WeiSha.Data.Field<PointAccount>("Pa_Serial");
    			
    			/// <summary>
    			/// 字段名：Pa_From - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pa_From = new WeiSha.Data.Field<PointAccount>("Pa_From");
    		}
    	}
    }
    