namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Student_Collect 主键列：Stc_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Student_Collect : WeiSha.Data.Entity {
    		
    		protected Int32 _Stc_ID;
    		
    		protected Int32 _Ac_ID;
    		
    		protected Int32 _Cou_ID;
    		
    		protected Int32 _Qus_ID;
    		
    		protected Int32 _Qus_Type;
    		
    		protected Int32 _Qus_Diff;
    		
    		protected String _Qus_Title;
    		
    		protected Int32 _Sbj_ID;
    		
    		protected Int32 _Stc_Level;
    		
    		protected Int32 _Stc_Strange;
    		
    		protected DateTime _Stc_CrtTime;
    		
    		public Int32 Stc_ID {
    			get {
    				return this._Stc_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stc_ID, _Stc_ID, value);
    				this._Stc_ID = value;
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
    		
    		public Int32 Cou_ID {
    			get {
    				return this._Cou_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Cou_ID, _Cou_ID, value);
    				this._Cou_ID = value;
    			}
    		}
    		
    		public Int32 Qus_ID {
    			get {
    				return this._Qus_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_ID, _Qus_ID, value);
    				this._Qus_ID = value;
    			}
    		}
    		
    		public Int32 Qus_Type {
    			get {
    				return this._Qus_Type;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Type, _Qus_Type, value);
    				this._Qus_Type = value;
    			}
    		}
    		
    		public Int32 Qus_Diff {
    			get {
    				return this._Qus_Diff;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Diff, _Qus_Diff, value);
    				this._Qus_Diff = value;
    			}
    		}
    		
    		public String Qus_Title {
    			get {
    				return this._Qus_Title;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qus_Title, _Qus_Title, value);
    				this._Qus_Title = value;
    			}
    		}
    		
    		public Int32 Sbj_ID {
    			get {
    				return this._Sbj_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Sbj_ID, _Sbj_ID, value);
    				this._Sbj_ID = value;
    			}
    		}
    		
    		public Int32 Stc_Level {
    			get {
    				return this._Stc_Level;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stc_Level, _Stc_Level, value);
    				this._Stc_Level = value;
    			}
    		}
    		
    		public Int32 Stc_Strange {
    			get {
    				return this._Stc_Strange;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stc_Strange, _Stc_Strange, value);
    				this._Stc_Strange = value;
    			}
    		}
    		
    		public DateTime Stc_CrtTime {
    			get {
    				return this._Stc_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Stc_CrtTime, _Stc_CrtTime, value);
    				this._Stc_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Student_Collect>("Student_Collect");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Stc_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Stc_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Stc_ID,
    					_.Ac_ID,
    					_.Cou_ID,
    					_.Qus_ID,
    					_.Qus_Type,
    					_.Qus_Diff,
    					_.Qus_Title,
    					_.Sbj_ID,
    					_.Stc_Level,
    					_.Stc_Strange,
    					_.Stc_CrtTime};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Stc_ID,
    					this._Ac_ID,
    					this._Cou_ID,
    					this._Qus_ID,
    					this._Qus_Type,
    					this._Qus_Diff,
    					this._Qus_Title,
    					this._Sbj_ID,
    					this._Stc_Level,
    					this._Stc_Strange,
    					this._Stc_CrtTime};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Stc_ID))) {
    				this._Stc_ID = reader.GetInt32(_.Stc_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt32(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Cou_ID))) {
    				this._Cou_ID = reader.GetInt32(_.Cou_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qus_ID))) {
    				this._Qus_ID = reader.GetInt32(_.Qus_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Type))) {
    				this._Qus_Type = reader.GetInt32(_.Qus_Type);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Diff))) {
    				this._Qus_Diff = reader.GetInt32(_.Qus_Diff);
    			}
    			if ((false == reader.IsDBNull(_.Qus_Title))) {
    				this._Qus_Title = reader.GetString(_.Qus_Title);
    			}
    			if ((false == reader.IsDBNull(_.Sbj_ID))) {
    				this._Sbj_ID = reader.GetInt32(_.Sbj_ID);
    			}
    			if ((false == reader.IsDBNull(_.Stc_Level))) {
    				this._Stc_Level = reader.GetInt32(_.Stc_Level);
    			}
    			if ((false == reader.IsDBNull(_.Stc_Strange))) {
    				this._Stc_Strange = reader.GetInt32(_.Stc_Strange);
    			}
    			if ((false == reader.IsDBNull(_.Stc_CrtTime))) {
    				this._Stc_CrtTime = reader.GetDateTime(_.Stc_CrtTime);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Student_Collect).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Student_Collect>();
    			
    			/// <summary>
    			/// 字段名：Stc_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Stc_ID = new WeiSha.Data.Field<Student_Collect>("Stc_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<Student_Collect>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Cou_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Cou_ID = new WeiSha.Data.Field<Student_Collect>("Cou_ID");
    			
    			/// <summary>
    			/// 字段名：Qus_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_ID = new WeiSha.Data.Field<Student_Collect>("Qus_ID");
    			
    			/// <summary>
    			/// 字段名：Qus_Type - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Type = new WeiSha.Data.Field<Student_Collect>("Qus_Type");
    			
    			/// <summary>
    			/// 字段名：Qus_Diff - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Diff = new WeiSha.Data.Field<Student_Collect>("Qus_Diff");
    			
    			/// <summary>
    			/// 字段名：Qus_Title - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Qus_Title = new WeiSha.Data.Field<Student_Collect>("Qus_Title");
    			
    			/// <summary>
    			/// 字段名：Sbj_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Sbj_ID = new WeiSha.Data.Field<Student_Collect>("Sbj_ID");
    			
    			/// <summary>
    			/// 字段名：Stc_Level - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Stc_Level = new WeiSha.Data.Field<Student_Collect>("Stc_Level");
    			
    			/// <summary>
    			/// 字段名：Stc_Strange - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Stc_Strange = new WeiSha.Data.Field<Student_Collect>("Stc_Strange");
    			
    			/// <summary>
    			/// 字段名：Stc_CrtTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Stc_CrtTime = new WeiSha.Data.Field<Student_Collect>("Stc_CrtTime");
    		}
    	}
    }
    