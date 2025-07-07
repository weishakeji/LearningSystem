/*
	  风格管理
*/
(function (w) {
	//皮肤管理
	var skins = function () {
		this.list = [];
		this.loadingcompleted = false;	//是否初始加载完成
		//需要更换风格的组件
		this.crts = ['Admin', 'Login', 'treemenu', 'dropmenu', 'tabs', 'verticalbar', 'pagebox'];
		//加载基础样式
		let crts = [];
		for (let i = 0; i < this.crts.length; i++)
			crts[i] = this.stylepath + this.crts[i] + '.css';
		w.$dom.load.css(crts, null, 'webdeskui');
		/*自定义事件
			switch：日间模式与夜间模式切换时触发
			setup:设置皮肤时触发
			change:不管哪种变化，都触发，在加载css样式前
			loadcss: css样式加载完成
			*/
		let customEvents = ['switch', 'setup', 'change', 'loadcss'];
		eval($ctrl.event_generate(customEvents));

	};
	var fn = skins.prototype;
	//属性
	fn.night = false;
	fn.rootpath = $dom.path() + 'Panel/';	//UI库的根文件夹
	fn.stylepath = fn.rootpath + 'Styles/';	//默认css样式文件夹
	fn.skinpath = fn.rootpath + 'Skins/';
	//风格样式库
	fn._list = ['Admin2025', 'Education', 'win10', 'win7'];
	fn._night = '_Night'; //夜间模式
	//模块名称
	fn._module = function () {
		let url = w.location.pathname;
		if (url.charAt(url.length - 1) == "/") url = url.substring(0, url.length - 1);
		if (url.indexOf("/") > -1) url = url.substring(url.lastIndexOf("/") + 1);
		return url;
	};
	fn._cookies = {
		curr: 'WebdeskUI-' + fn._module() + '-skin',
		night: 'WebdeskUI-' + fn._module() + '-night'
	};
	//当前皮肤
	fn.current = function () {
		let skin = $api.cookie(fn._cookies.curr);
		return skin != null ? skin : fn._list[0];
	};
	//设置当前皮肤
	//func:回调函数，name为当前设置的风格名称，skin为皮肤对象
	fn.setup = function (name, func) {
		if (!this.loadingcompleted) {
			var th = this;
			window.setTimeout(function () {
				th.setup(name, func);
			}, 100);
		}
		//如果未设置风格名称，则取本地cookie中记录的
		if (name == null || name == '') name = this.current();
		//如果cookie中记录的皮肤不在风格列表中，则取第一个
		name = this._list.find(x => x.toLowerCase() == name.toLowerCase());
		if (name == null) name = this._list.length > 0 ? this._list[0] : '';
		if (name == null || name == '') return;
		name = name.toLowerCase();
		//重新写入本地cookie中
		$api.cookie(this._cookies.curr, name, { expires: 999 });
		if (this.isnight()) this.switch();	//切换夜间模式
		this.trigger('change');
		this.trigger('setup', { skin: name });
		//回调函数,两个参数,第一个为当前设置的皮肤对象，第二个为当前设置的皮肤名称
		if (func != null) {
			let skin = this.list.find(x => x.tag.toLowerCase() == name.toLowerCase());
			func(skin, name);
		}
	};
	//切换夜间模式或日间模式
	fn.switch = function () {
		let night = !fn.isnight();
		$api.cookie(fn._cookies.night, String(night));
		this.trigger('change');
		this.trigger('switch', { night: night });
		return night;
	};
	//是不是夜晚模式
	fn.isnight = function () {
		let night = $api.cookie(this._cookies.night);
		if (night == null || night == 'false') return false;
		return true;
	};
	//加载指定风格的css样式
	fn.loadCss = function (callback) {
		//清除之前的
		$dom('link[tag=webdeskui_skin]').remove();
		//加载控件资源
		let crts = [];
		let skin = this.isnight() ? this._night : this.current();
		for (let i = 0; i < this.crts.length; i++)
			crts[i] = this.skinpath + skin + '/' + this.crts[i] + '.css';
		var th = this;
		w.$dom.load.css(crts, function () {
			th.trigger('loadcss');
			if (callback != null) callback();
		}, 'webdeskui_skin');
	};
	//加载风格信息
	fn.loadskin = function (skin) {
		var th = this;
		$dom.get(th.skinpath + skin + '/intro.json', function (d) {
			if (d == null || d == '') return;
			var obj = typeof variable === 'string' ? eval('(' + d + ')') : d;
			obj.tag = skin;
			obj.path = th.skinpath + skin;
			let index = th._list.indexOf(skin);
			if (index >= 0) th.list.splice(index, 0, obj);
			//判断是否加载完成
			if (th.list.length == th._list.length) th.loadingcompleted = true;
		});
	};
	//初始化
	w.$skins = new skins();
	w.$skins.onchange((s, e) => s.loadCss());
	(function () {
		//加载风格信息
		for (let i = 0; i < w.$skins._list.length; i++) {
			let skin = w.$skins._list[i];
			w.$skins.loadskin(skin);
		}
	})();
})(window);