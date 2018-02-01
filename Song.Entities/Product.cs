namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Product 主键列：Pd_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Product : WeiSha.Data.Entity {
    		
    		protected Int32 _Pd_Id;
    		
    		protected String _Pd_Name;
    		
    		protected Int32? _Pd_Tax;
    		
    		protected Boolean _Pd_IsUse;
    		
    		protected Boolean _Pd_IsNew;
    		
    		protected Boolean _Pd_IsRec;
    		
    		protected Int32? _Col_Id;
    		
    		protected String _Col_Name;
    		
    		protected String _Pd_Model;
    		
    		protected String _Pd_Code;
    		
    		protected Single? _Pd_Prise;
    		
    		protected Single? _Pd_Weight;
    		
    		protected String _Pd_ColorName;
    		
    		protected String _Pd_Color;
    		
    		protected String _Pd_KeyWords;
    		
    		protected String _Pd_Descr;
    		
    		protected String _Pd_Logo;
    		
    		protected String _Pd_LogoSmall;
    		
    		protected String _Pd_QrCode;
    		
    		protected Int32? _Pd_Number;
    		
    		protected Int32? _Pd_Stocks;
    		
    		protected String _Pd_Unit;
    		
    		protected DateTime? _Pd_SaleTime;
    		
    		protected DateTime? _Pd_CrtTime;
    		
    		protected Boolean _Pd_IsNote;
    		
    		protected String _Pd_Intro;
    		
    		protected String _Pd_Details;
    		
    		protected Boolean _Pd_IsDel;
    		
    		protected Boolean _Pd_IsStatic;
    		
    		protected DateTime? _Pd_PushTime;
    		
    		protected String _Pd_Label;
    		
    		protected String _Pd_Uid;
    		
    		protected Int32? _Acc_Id;
    		
    		protected String _Acc_Name;
    		
    		protected Int32? _Pfact_Id;
    		
    		protected Int32? _Pori_Id;
    		
    		protected Int32? _Pmat_Id;
    		
    		protected String _OtherData;
    		
    		protected Int32 _Org_ID;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32 Pd_Id {
    			get {
    				return this._Pd_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Id, _Pd_Id, value);
    				this._Pd_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_Name {
    			get {
    				return this._Pd_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Name, _Pd_Name, value);
    				this._Pd_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pd_Tax {
    			get {
    				return this._Pd_Tax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Tax, _Pd_Tax, value);
    				this._Pd_Tax = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pd_IsUse {
    			get {
    				return this._Pd_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_IsUse, _Pd_IsUse, value);
    				this._Pd_IsUse = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pd_IsNew {
    			get {
    				return this._Pd_IsNew;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_IsNew, _Pd_IsNew, value);
    				this._Pd_IsNew = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pd_IsRec {
    			get {
    				return this._Pd_IsRec;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_IsRec, _Pd_IsRec, value);
    				this._Pd_IsRec = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Col_Id {
    			get {
    				return this._Col_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_Id, _Col_Id, value);
    				this._Col_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Col_Name {
    			get {
    				return this._Col_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Col_Name, _Col_Name, value);
    				this._Col_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_Model {
    			get {
    				return this._Pd_Model;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Model, _Pd_Model, value);
    				this._Pd_Model = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_Code {
    			get {
    				return this._Pd_Code;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Code, _Pd_Code, value);
    				this._Pd_Code = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Single? Pd_Prise {
    			get {
    				return this._Pd_Prise;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Prise, _Pd_Prise, value);
    				this._Pd_Prise = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Single? Pd_Weight {
    			get {
    				return this._Pd_Weight;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Weight, _Pd_Weight, value);
    				this._Pd_Weight = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_ColorName {
    			get {
    				return this._Pd_ColorName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_ColorName, _Pd_ColorName, value);
    				this._Pd_ColorName = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_Color {
    			get {
    				return this._Pd_Color;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Color, _Pd_Color, value);
    				this._Pd_Color = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_KeyWords {
    			get {
    				return this._Pd_KeyWords;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_KeyWords, _Pd_KeyWords, value);
    				this._Pd_KeyWords = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_Descr {
    			get {
    				return this._Pd_Descr;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Descr, _Pd_Descr, value);
    				this._Pd_Descr = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_Logo {
    			get {
    				return this._Pd_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Logo, _Pd_Logo, value);
    				this._Pd_Logo = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_LogoSmall {
    			get {
    				return this._Pd_LogoSmall;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_LogoSmall, _Pd_LogoSmall, value);
    				this._Pd_LogoSmall = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_QrCode {
    			get {
    				return this._Pd_QrCode;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_QrCode, _Pd_QrCode, value);
    				this._Pd_QrCode = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pd_Number {
    			get {
    				return this._Pd_Number;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Number, _Pd_Number, value);
    				this._Pd_Number = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pd_Stocks {
    			get {
    				return this._Pd_Stocks;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Stocks, _Pd_Stocks, value);
    				this._Pd_Stocks = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_Unit {
    			get {
    				return this._Pd_Unit;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Unit, _Pd_Unit, value);
    				this._Pd_Unit = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Pd_SaleTime {
    			get {
    				return this._Pd_SaleTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_SaleTime, _Pd_SaleTime, value);
    				this._Pd_SaleTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Pd_CrtTime {
    			get {
    				return this._Pd_CrtTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_CrtTime, _Pd_CrtTime, value);
    				this._Pd_CrtTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pd_IsNote {
    			get {
    				return this._Pd_IsNote;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_IsNote, _Pd_IsNote, value);
    				this._Pd_IsNote = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_Intro {
    			get {
    				return this._Pd_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Intro, _Pd_Intro, value);
    				this._Pd_Intro = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_Details {
    			get {
    				return this._Pd_Details;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Details, _Pd_Details, value);
    				this._Pd_Details = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pd_IsDel {
    			get {
    				return this._Pd_IsDel;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_IsDel, _Pd_IsDel, value);
    				this._Pd_IsDel = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Boolean Pd_IsStatic {
    			get {
    				return this._Pd_IsStatic;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_IsStatic, _Pd_IsStatic, value);
    				this._Pd_IsStatic = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public DateTime? Pd_PushTime {
    			get {
    				return this._Pd_PushTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_PushTime, _Pd_PushTime, value);
    				this._Pd_PushTime = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_Label {
    			get {
    				return this._Pd_Label;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Label, _Pd_Label, value);
    				this._Pd_Label = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String Pd_Uid {
    			get {
    				return this._Pd_Uid;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pd_Uid, _Pd_Uid, value);
    				this._Pd_Uid = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
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
    		/// -1
    		/// </summary>
    		public String Acc_Name {
    			get {
    				return this._Acc_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Name, _Acc_Name, value);
    				this._Acc_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pfact_Id {
    			get {
    				return this._Pfact_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pfact_Id, _Pfact_Id, value);
    				this._Pfact_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pori_Id {
    			get {
    				return this._Pori_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pori_Id, _Pori_Id, value);
    				this._Pori_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public Int32? Pmat_Id {
    			get {
    				return this._Pmat_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Pmat_Id, _Pmat_Id, value);
    				this._Pmat_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// -1
    		/// </summary>
    		public String OtherData {
    			get {
    				return this._OtherData;
    			}
    			set {
    				this.OnPropertyValueChange(_.OtherData, _OtherData, value);
    				this._OtherData = value;
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
    			return new WeiSha.Data.Table<Product>("Product");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Pd_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pd_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Pd_Id,
    					_.Pd_Name,
    					_.Pd_Tax,
    					_.Pd_IsUse,
    					_.Pd_IsNew,
    					_.Pd_IsRec,
    					_.Col_Id,
    					_.Col_Name,
    					_.Pd_Model,
    					_.Pd_Code,
    					_.Pd_Prise,
    					_.Pd_Weight,
    					_.Pd_ColorName,
    					_.Pd_Color,
    					_.Pd_KeyWords,
    					_.Pd_Descr,
    					_.Pd_Logo,
    					_.Pd_LogoSmall,
    					_.Pd_QrCode,
    					_.Pd_Number,
    					_.Pd_Stocks,
    					_.Pd_Unit,
    					_.Pd_SaleTime,
    					_.Pd_CrtTime,
    					_.Pd_IsNote,
    					_.Pd_Intro,
    					_.Pd_Details,
    					_.Pd_IsDel,
    					_.Pd_IsStatic,
    					_.Pd_PushTime,
    					_.Pd_Label,
    					_.Pd_Uid,
    					_.Acc_Id,
    					_.Acc_Name,
    					_.Pfact_Id,
    					_.Pori_Id,
    					_.Pmat_Id,
    					_.OtherData,
    					_.Org_ID,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Pd_Id,
    					this._Pd_Name,
    					this._Pd_Tax,
    					this._Pd_IsUse,
    					this._Pd_IsNew,
    					this._Pd_IsRec,
    					this._Col_Id,
    					this._Col_Name,
    					this._Pd_Model,
    					this._Pd_Code,
    					this._Pd_Prise,
    					this._Pd_Weight,
    					this._Pd_ColorName,
    					this._Pd_Color,
    					this._Pd_KeyWords,
    					this._Pd_Descr,
    					this._Pd_Logo,
    					this._Pd_LogoSmall,
    					this._Pd_QrCode,
    					this._Pd_Number,
    					this._Pd_Stocks,
    					this._Pd_Unit,
    					this._Pd_SaleTime,
    					this._Pd_CrtTime,
    					this._Pd_IsNote,
    					this._Pd_Intro,
    					this._Pd_Details,
    					this._Pd_IsDel,
    					this._Pd_IsStatic,
    					this._Pd_PushTime,
    					this._Pd_Label,
    					this._Pd_Uid,
    					this._Acc_Id,
    					this._Acc_Name,
    					this._Pfact_Id,
    					this._Pori_Id,
    					this._Pmat_Id,
    					this._OtherData,
    					this._Org_ID,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Pd_Id))) {
    				this._Pd_Id = reader.GetInt32(_.Pd_Id);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Name))) {
    				this._Pd_Name = reader.GetString(_.Pd_Name);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Tax))) {
    				this._Pd_Tax = reader.GetInt32(_.Pd_Tax);
    			}
    			if ((false == reader.IsDBNull(_.Pd_IsUse))) {
    				this._Pd_IsUse = reader.GetBoolean(_.Pd_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Pd_IsNew))) {
    				this._Pd_IsNew = reader.GetBoolean(_.Pd_IsNew);
    			}
    			if ((false == reader.IsDBNull(_.Pd_IsRec))) {
    				this._Pd_IsRec = reader.GetBoolean(_.Pd_IsRec);
    			}
    			if ((false == reader.IsDBNull(_.Col_Id))) {
    				this._Col_Id = reader.GetInt32(_.Col_Id);
    			}
    			if ((false == reader.IsDBNull(_.Col_Name))) {
    				this._Col_Name = reader.GetString(_.Col_Name);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Model))) {
    				this._Pd_Model = reader.GetString(_.Pd_Model);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Code))) {
    				this._Pd_Code = reader.GetString(_.Pd_Code);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Prise))) {
    				this._Pd_Prise = reader.GetFloat(_.Pd_Prise);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Weight))) {
    				this._Pd_Weight = reader.GetFloat(_.Pd_Weight);
    			}
    			if ((false == reader.IsDBNull(_.Pd_ColorName))) {
    				this._Pd_ColorName = reader.GetString(_.Pd_ColorName);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Color))) {
    				this._Pd_Color = reader.GetString(_.Pd_Color);
    			}
    			if ((false == reader.IsDBNull(_.Pd_KeyWords))) {
    				this._Pd_KeyWords = reader.GetString(_.Pd_KeyWords);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Descr))) {
    				this._Pd_Descr = reader.GetString(_.Pd_Descr);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Logo))) {
    				this._Pd_Logo = reader.GetString(_.Pd_Logo);
    			}
    			if ((false == reader.IsDBNull(_.Pd_LogoSmall))) {
    				this._Pd_LogoSmall = reader.GetString(_.Pd_LogoSmall);
    			}
    			if ((false == reader.IsDBNull(_.Pd_QrCode))) {
    				this._Pd_QrCode = reader.GetString(_.Pd_QrCode);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Number))) {
    				this._Pd_Number = reader.GetInt32(_.Pd_Number);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Stocks))) {
    				this._Pd_Stocks = reader.GetInt32(_.Pd_Stocks);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Unit))) {
    				this._Pd_Unit = reader.GetString(_.Pd_Unit);
    			}
    			if ((false == reader.IsDBNull(_.Pd_SaleTime))) {
    				this._Pd_SaleTime = reader.GetDateTime(_.Pd_SaleTime);
    			}
    			if ((false == reader.IsDBNull(_.Pd_CrtTime))) {
    				this._Pd_CrtTime = reader.GetDateTime(_.Pd_CrtTime);
    			}
    			if ((false == reader.IsDBNull(_.Pd_IsNote))) {
    				this._Pd_IsNote = reader.GetBoolean(_.Pd_IsNote);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Intro))) {
    				this._Pd_Intro = reader.GetString(_.Pd_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Details))) {
    				this._Pd_Details = reader.GetString(_.Pd_Details);
    			}
    			if ((false == reader.IsDBNull(_.Pd_IsDel))) {
    				this._Pd_IsDel = reader.GetBoolean(_.Pd_IsDel);
    			}
    			if ((false == reader.IsDBNull(_.Pd_IsStatic))) {
    				this._Pd_IsStatic = reader.GetBoolean(_.Pd_IsStatic);
    			}
    			if ((false == reader.IsDBNull(_.Pd_PushTime))) {
    				this._Pd_PushTime = reader.GetDateTime(_.Pd_PushTime);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Label))) {
    				this._Pd_Label = reader.GetString(_.Pd_Label);
    			}
    			if ((false == reader.IsDBNull(_.Pd_Uid))) {
    				this._Pd_Uid = reader.GetString(_.Pd_Uid);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Name))) {
    				this._Acc_Name = reader.GetString(_.Acc_Name);
    			}
    			if ((false == reader.IsDBNull(_.Pfact_Id))) {
    				this._Pfact_Id = reader.GetInt32(_.Pfact_Id);
    			}
    			if ((false == reader.IsDBNull(_.Pori_Id))) {
    				this._Pori_Id = reader.GetInt32(_.Pori_Id);
    			}
    			if ((false == reader.IsDBNull(_.Pmat_Id))) {
    				this._Pmat_Id = reader.GetInt32(_.Pmat_Id);
    			}
    			if ((false == reader.IsDBNull(_.OtherData))) {
    				this._OtherData = reader.GetString(_.OtherData);
    			}
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
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
    			if ((false == typeof(Product).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Product>();
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Id - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Id = new WeiSha.Data.Field<Product>("Pd_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Name = new WeiSha.Data.Field<Product>("Pd_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Tax - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Tax = new WeiSha.Data.Field<Product>("Pd_Tax");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pd_IsUse = new WeiSha.Data.Field<Product>("Pd_IsUse");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_IsNew - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pd_IsNew = new WeiSha.Data.Field<Product>("Pd_IsNew");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_IsRec - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pd_IsRec = new WeiSha.Data.Field<Product>("Pd_IsRec");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Col_Id = new WeiSha.Data.Field<Product>("Col_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Col_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Col_Name = new WeiSha.Data.Field<Product>("Col_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Model - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Model = new WeiSha.Data.Field<Product>("Pd_Model");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Code - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Code = new WeiSha.Data.Field<Product>("Pd_Code");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Prise - 数据类型：Single(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Prise = new WeiSha.Data.Field<Product>("Pd_Prise");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Weight - 数据类型：Single(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Weight = new WeiSha.Data.Field<Product>("Pd_Weight");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_ColorName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_ColorName = new WeiSha.Data.Field<Product>("Pd_ColorName");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Color - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Color = new WeiSha.Data.Field<Product>("Pd_Color");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_KeyWords - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_KeyWords = new WeiSha.Data.Field<Product>("Pd_KeyWords");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Descr - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Descr = new WeiSha.Data.Field<Product>("Pd_Descr");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Logo = new WeiSha.Data.Field<Product>("Pd_Logo");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_LogoSmall - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_LogoSmall = new WeiSha.Data.Field<Product>("Pd_LogoSmall");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_QrCode - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_QrCode = new WeiSha.Data.Field<Product>("Pd_QrCode");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Number - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Number = new WeiSha.Data.Field<Product>("Pd_Number");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Stocks - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Stocks = new WeiSha.Data.Field<Product>("Pd_Stocks");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Unit - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Unit = new WeiSha.Data.Field<Product>("Pd_Unit");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_SaleTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pd_SaleTime = new WeiSha.Data.Field<Product>("Pd_SaleTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_CrtTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pd_CrtTime = new WeiSha.Data.Field<Product>("Pd_CrtTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_IsNote - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pd_IsNote = new WeiSha.Data.Field<Product>("Pd_IsNote");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Intro = new WeiSha.Data.Field<Product>("Pd_Intro");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Details - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Details = new WeiSha.Data.Field<Product>("Pd_Details");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_IsDel - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pd_IsDel = new WeiSha.Data.Field<Product>("Pd_IsDel");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_IsStatic - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Pd_IsStatic = new WeiSha.Data.Field<Product>("Pd_IsStatic");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_PushTime - 数据类型：DateTime(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pd_PushTime = new WeiSha.Data.Field<Product>("Pd_PushTime");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Label - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Label = new WeiSha.Data.Field<Product>("Pd_Label");
    			
    			/// <summary>
    			/// -1 - 字段名：Pd_Uid - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Pd_Uid = new WeiSha.Data.Field<Product>("Pd_Uid");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Id = new WeiSha.Data.Field<Product>("Acc_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Acc_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Acc_Name = new WeiSha.Data.Field<Product>("Acc_Name");
    			
    			/// <summary>
    			/// -1 - 字段名：Pfact_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pfact_Id = new WeiSha.Data.Field<Product>("Pfact_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Pori_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pori_Id = new WeiSha.Data.Field<Product>("Pori_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：Pmat_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static WeiSha.Data.Field Pmat_Id = new WeiSha.Data.Field<Product>("Pmat_Id");
    			
    			/// <summary>
    			/// -1 - 字段名：OtherData - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field OtherData = new WeiSha.Data.Field<Product>("OtherData");
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Product>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Product>("Org_Name");
    		}
    	}
    }
    