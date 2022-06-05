namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：LimitDomain 主键列：LD_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class LimitDomain : WeiSha.Data.Entity {
    		
    		protected Int32 _LD_ID;
    		
    		protected String _LD_Name;
    		
    		protected Boolean _LD_IsUse;
    		
    		protected String _LD_Intro;
    		
    		public Int32 LD_ID {
    			get {
    				return this._LD_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.LD_ID, _LD_ID, value);
    				this._LD_ID = value;
    			}
    		}
    		
    		public String LD_Name {
    			get {
    				return this._LD_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.LD_Name, _LD_Name, value);
    				this._LD_Name = value;
    			}
    		}
    		
    		public Boolean LD_IsUse {
    			get {
    				return this._LD_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.LD_IsUse, _LD_IsUse, value);
    				this._LD_IsUse = value;
    			}
    		}
    		
    		public String LD_Intro {
    			get {
    				return this._LD_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.LD_Intro, _LD_Intro, value);
    				this._LD_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<LimitDomain>("LimitDomain");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.LD_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.LD_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.LD_ID,
    					_.LD_Name,
    					_.LD_IsUse,
    					_.LD_Intro};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._LD_ID,
    					this._LD_Name,
    					this._LD_IsUse,
    					this._LD_Intro};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.LD_ID))) {
    				this._LD_ID = reader.GetInt32(_.LD_ID);
    			}
    			if ((false == reader.IsDBNull(_.LD_Name))) {
    				this._LD_Name = reader.GetString(_.LD_Name);
    			}
    			if ((false == reader.IsDBNull(_.LD_IsUse))) {
    				this._LD_IsUse = reader.GetBoolean(_.LD_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.LD_Intro))) {
    				this._LD_Intro = reader.GetString(_.LD_Intro);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(LimitDomain).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<LimitDomain>();
    			
    			/// <summary>
    			/// 字段名：LD_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field LD_ID = new WeiSha.Data.Field<LimitDomain>("LD_ID");
    			
    			/// <summary>
    			/// 字段名：LD_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field LD_Name = new WeiSha.Data.Field<LimitDomain>("LD_Name");
    			
    			/// <summary>
    			/// 字段名：LD_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field LD_IsUse = new WeiSha.Data.Field<LimitDomain>("LD_IsUse");
    			
    			/// <summary>
    			/// 字段名：LD_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field LD_Intro = new WeiSha.Data.Field<LimitDomain>("LD_Intro");
    		}
    	}
    }
    