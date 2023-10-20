/*
	  风格管理
*/
(function (window) {
	let url = window.location.href;
	if (url.charAt(url.length - 1) == "/")
		url = url.substring(0, url.length - 1);
	if (url.indexOf("/") > -1)
		url = url.substring(url.lastIndexOf("/") + 1);
	window.module = url;
	//console.error(window.module);

	//皮肤管理
	var skins = function () {
		this.night = false;
		this.rootpath = $dom.path() + 'Panel/';	//UI库的根文件夹
		this.stylepath = this.rootpath + 'Styles/';	//默认css样式文件夹
		this.skinpath = this.rootpath + 'Skins/';
		//组件名称
		this.crts = ['treemenu', 'dropmenu', 'tabs', 'verticalbar', 'pagebox'];
		//风格样式库
		this.list = [];
		this._list = ['education', 'office', 'win10', 'win7'];
		this._night = '_Night'; //夜间模式
		this._cookies = {
			curr: 'WebdeskUI-' + window.module + '-skin',
			night: 'WebdeskUI-' + window.module + '-night'
		};
		//当前皮肤
		this.current = function () {
			let skin = $api.cookie(this._cookies.curr);
			return skin != null ? skin : this._list[0];
		};
		//设置当前皮肤
		this.setup = function (name) {
			$api.cookie(this._cookies.curr, name, { expires: 999 });
			if (this.isnight()) this.switch();
			this.trigger('change');
			this.trigger('setup', {
				skin: name
			});
		};
		//切换夜间模式或日间模式
		this.switch = function () {
			let night = !this.isnight();
			$api.cookie(this._cookies.night, String(night));
			this.trigger('change');
			this.trigger('switch', {
				night: night
			});
			return night;
		};
		//是不是夜晚模式
		this.isnight = function () {
			let night = $api.cookie(this._cookies.night);
			if (night == null || night == 'false') return false;
			return true;
		};
		//加载基础样式
		let crts = [];
		for (let i = 0; i < this.crts.length; i++)
			crts[i] = this.stylepath + this.crts[i] + '.css';
		window.$dom.load.css(crts, null, 'webdeskui');
		/*自定义事件
		switch：日间模式与夜间模式切换时触发
		setup:设置皮肤时触发
		change:不管哪种变化，都触发，在加载css样式前
		loadcss: css样式加载完成
		*/
		let customEvents = ['switch', 'setup', 'change', 'loadcss'];
		eval($ctrl.event_generate(customEvents));
	}
	var fn = skins.prototype;
	//加载csss
	fn.loadCss = function (callback) {
		//清除之前的
		$dom('link[tag=skin]').remove();
		//加载控件资源
		let crts = this.crts;
		let skin = this.isnight() ? this._night : this.current();
		for (let i = 0; i < crts.length; i++) {
			crts[i] = this.skinpath + skin + '/' + crts[i] + '.css';
		}
		var th = this;
		window.$dom.load.css(crts, function () {
			th.trigger('loadcss');
			if (callback != null) callback();
		}, 'skin');
	};
	fn.loadskin = function (skin) {
		var th = this;
		$dom.get(th.skinpath + skin + '/intro.json', function (d) {
			if (d == null || d == '') return;
			var obj = eval('(' + d + ')');
			obj.tag = skin;
			obj.path = th.skinpath + skin;
			window.$skins.list.push(obj);
		});
	}
	window.$skins = new skins();
	window.$skins.onchange(function (s, e) {
		s.loadCss();
	});
	//加载风格信息
	for (let i = 0; i < window.$skins._list.length; i++) {
		let skin = window.$skins._list[i];
		window.$skins.loadskin(skin);
	}
})(window);