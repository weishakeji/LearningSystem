namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：SystemPara 主键列：Sys_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class SystemPara : WeiSha.Data.Entity {
    		
    		protected Int32 _Sys_Id;
    		
    		protected String _Sys_Key;
    		
    		protected String _Sys_Value;
    		
    		protected String _Sys_Default;
    		
    		protected String _Sys_ParaIntro;
    		
    		protected String _Sys_Unit;
    		
    		protected String _Sys_SelectUnit;
    		
    		protected Int32? _Org_Id;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Sys_Id {
    			get {
    				return this._Sys_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sys_Id, _Sys_Id, value);
    				this._Sys_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sys_Key {
    			get {
    				return this._Sys_Key;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sys_Key, _Sys_Key, value);
    				this._Sys_Key = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sys_Value {
    			get {
    				return this._Sys_Value;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sys_Value, _Sys_Value, value);
    				this._Sys_Value = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sys_Default {
    			get {
    				return this._Sys_Default;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sys_Default, _Sys_Default, value);
    				this._Sys_Default = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sys_ParaIntro {
    			get {
    				return this._Sys_ParaIntro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sys_ParaIntro, _Sys_ParaIntro, value);
    				this._Sys_ParaIntro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sys_Unit {
    			get {
    				return this._Sys_Unit;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sys_Unit, _Sys_Unit, value);
    				this._Sys_Unit = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Sys_SelectUnit {
    			get {
    				return this._Sys_SelectUnit;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sys_SelectUnit, _Sys_SelectUnit, value);
    				this._Sys_SelectUnit = value;
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
    			return new WeiSha.Data.Table<SystemPara>("SystemPara");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Sys_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Sys_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Sys_Id,
    					_.Sys_Key,
    					_.Sys_Value,
    					_.Sys_Default,
    					_.Sys_ParaIntro,
    					_.Sys_Unit,
    					_.Sys_SelectUnit,
    					_.Org_Id,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Sys_Id,
    					this._Sys_Key,
    					this._Sys_Value,
    					this._Sys_Default,
    					this._Sys_ParaIntro,
    					this._Sys_Unit,
    					this._Sys_SelectUnit,
    					this._Org_Id,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Sys_Id))) {
    				this._Sys_Id = reader.GetInt32(_.Sys_Id);
    			}
    			if ((false == reader.IsDBNull(_.Sys_Key))) {
    				this._Sys_Key = reader.GetString(_.Sys_Key);
    			}
    			if ((false == reader.IsDBNull(_.Sys_Value))) {
    				this._Sys_Value = reader.GetString(_.Sys_Value);
    			}
    			if ((false == reader.IsDBNull(_.Sys_Default))) {
    				this._Sys_Default = reader.GetString(_.Sys_Default);
    			}
    			if ((false == reader.IsDBNull(_.Sys_ParaIntro))) {
    				this._Sys_ParaIntro = reader.GetString(_.Sys_ParaIntro);
    			}
    			if ((false == reader.IsDBNull(_.Sys_Unit))) {
    				this._Sys_Unit = reader.GetString(_.Sys_Unit);
    			}
    			if ((false == reader.IsDBNull(_.Sys_SelectUnit))) {
    				this._Sys_SelectUnit = reader.GetString(_.Sys_SelectUnit);
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
    			if ((false == typeof(SystemPara).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<SystemPara>();
    			
    			/// <summary>
    			/// -1 - 字段名：Sys_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sys_Id = new WeiSha.Data.Field<SystemPara>("Sys_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Sys_Key - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sys_Key = new WeiSha.Data.Field<SystemPara>("Sys_Key");
    			
    			/// <summary>
    			/// -1 - 字段名：Sys_Value - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sys_Value = new WeiSha.Data.Field<SystemPara>("Sys_Value");
    			
    			/// <summary>
    			/// -1 - 字段名：Sys_Default - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sys_Default = new WeiSha.Data.Field<SystemPara>("Sys_Default");
    			
    			/// <summary>
    			/// -1 - 字段名：Sys_ParaIntro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sys_ParaIntro = new WeiSha.Data.Field<SystemPara>("Sys_ParaIntro");
    			
    			/// <summary>
    			/// -1 - 字段名：Sys_Unit - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sys_Unit = new WeiSha.Data.Field<SystemPara>("Sys_Unit");
    			
    			/// <summary>
    			/// -1 - 字段名：Sys_SelectUnit - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Sys_SelectUnit = new WeiSha.Data.Field<SystemPara>("Sys_SelectUnit");
    			
    			/// <summary>
    			/// 字段名：Org_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Org_Id = new WeiSha.Data.Field<SystemPara>("Org_Id");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<SystemPara>("Org_Name");
    		}
    	}
    }
    