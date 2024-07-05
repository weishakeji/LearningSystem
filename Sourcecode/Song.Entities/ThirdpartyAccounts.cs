namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：ThirdpartyAccounts 主键列：Ta_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class ThirdpartyAccounts : WeiSha.Data.Entity {
    		
    		protected Int32 _Ta_ID;
    		
    		protected Int64 _Ac_ID;
    		
    		protected String _Ta_Headimgurl;
    		
    		protected String _Ta_NickName;
    		
    		protected String _Ta_Openid;
    		
    		protected String _Ta_Tag;
    		
    		public Int32 Ta_ID {
    			get {
    				return this._Ta_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ta_ID, _Ta_ID, value);
    				this._Ta_ID = value;
    			}
    		}
    		
    		public Int64 Ac_ID {
    			get {
    				return this._Ac_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ac_ID, _Ac_ID, value);
    				this._Ac_ID = value;
    			}
    		}
    		
    		public String Ta_Headimgurl {
    			get {
    				return this._Ta_Headimgurl;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ta_Headimgurl, _Ta_Headimgurl, value);
    				this._Ta_Headimgurl = value;
    			}
    		}
    		
    		public String Ta_NickName {
    			get {
    				return this._Ta_NickName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ta_NickName, _Ta_NickName, value);
    				this._Ta_NickName = value;
    			}
    		}
    		
    		public String Ta_Openid {
    			get {
    				return this._Ta_Openid;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ta_Openid, _Ta_Openid, value);
    				this._Ta_Openid = value;
    			}
    		}
    		
    		public String Ta_Tag {
    			get {
    				return this._Ta_Tag;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ta_Tag, _Ta_Tag, value);
    				this._Ta_Tag = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<ThirdpartyAccounts>("ThirdpartyAccounts");
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ta_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Ta_ID,
    					_.Ac_ID,
    					_.Ta_Headimgurl,
    					_.Ta_NickName,
    					_.Ta_Openid,
    					_.Ta_Tag};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Ta_ID,
    					this._Ac_ID,
    					this._Ta_Headimgurl,
    					this._Ta_NickName,
    					this._Ta_Openid,
    					this._Ta_Tag};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Ta_ID))) {
    				this._Ta_ID = reader.GetInt32(_.Ta_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ac_ID))) {
    				this._Ac_ID = reader.GetInt64(_.Ac_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ta_Headimgurl))) {
    				this._Ta_Headimgurl = reader.GetString(_.Ta_Headimgurl);
    			}
    			if ((false == reader.IsDBNull(_.Ta_NickName))) {
    				this._Ta_NickName = reader.GetString(_.Ta_NickName);
    			}
    			if ((false == reader.IsDBNull(_.Ta_Openid))) {
    				this._Ta_Openid = reader.GetString(_.Ta_Openid);
    			}
    			if ((false == reader.IsDBNull(_.Ta_Tag))) {
    				this._Ta_Tag = reader.GetString(_.Ta_Tag);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(ThirdpartyAccounts).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<ThirdpartyAccounts>();
    			
    			/// <summary>
    			/// 字段名：Ta_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Ta_ID = new WeiSha.Data.Field<ThirdpartyAccounts>("Ta_ID");
    			
    			/// <summary>
    			/// 字段名：Ac_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Ac_ID = new WeiSha.Data.Field<ThirdpartyAccounts>("Ac_ID");
    			
    			/// <summary>
    			/// 字段名：Ta_Headimgurl - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ta_Headimgurl = new WeiSha.Data.Field<ThirdpartyAccounts>("Ta_Headimgurl");
    			
    			/// <summary>
    			/// 字段名：Ta_NickName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ta_NickName = new WeiSha.Data.Field<ThirdpartyAccounts>("Ta_NickName");
    			
    			/// <summary>
    			/// 字段名：Ta_Openid - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ta_Openid = new WeiSha.Data.Field<ThirdpartyAccounts>("Ta_Openid");
    			
    			/// <summary>
    			/// 字段名：Ta_Tag - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Ta_Tag = new WeiSha.Data.Field<ThirdpartyAccounts>("Ta_Tag");
    		}
    	}
    }
    