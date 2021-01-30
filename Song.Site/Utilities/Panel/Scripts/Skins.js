/*
	  风格管理
*/
(function (window) {
	//皮肤管理
	var skins = function () {
		this.night = false;
		this.list = [];
		this._list = ['education', 'win10', 'win7'];
		this._night = '_Night'; //夜间模式
		this._cookies = {
			curr: 'WebdeskUI-admin-skin',
			night: 'WebdeskUI-admin-night'
		};
		//当前皮肤
		this.current = function () {
			var skin = $api.cookie(this._cookies.curr);
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
			var night = !this.isnight();
			$api.cookie(this._cookies.night, String(night));
			this.trigger('change');
			this.trigger('switch', {
				night: night
			});
			return night;
		};
		//是不是夜晚模式
		this.isnight = function () {
			var night = $api.cookie(this._cookies.night);
			if (night == null || night == 'false') return false;
			return true;
		};
		/*自定义事件
		switch：日间模式与夜间模式切换时触发
		setup:设置皮肤时触发
		change:不管哪种变化，都触发，在加载css样式前
		loadcss: css样式加载完成
		*/
		var customEvents = ['switch', 'setup', 'change', 'loadcss'];
		eval($ctrl.event_generate(customEvents));
	}
	var fn = skins.prototype;
	//加载csss
	fn.loadCss = function (callback) {
		//清除之前的
		$dom('link[tag=skin]').remove();
		//加载控件资源
		var resources = ['admin', 'treemenu', 'dropmenu', 'tabs', 'verticalbar', 'pagebox'];
		var skin = this.isnight() ? this._night : this.current();
		for (var i = 0; i < resources.length; i++) {
			resources[i] =  '/Utilities/panel/skins/' + skin + '/' + resources[i] + '.css';
		}
		var th = this;
		window.$dom.load.css(resources, function () {
			th.trigger('loadcss');
			if (callback != null) callback();
		}, 'skin');
	};
	fn.loadskin = function (skin) {
		$dom.get('/Utilities/panel/skins/' + skin + '/intro.json', function (d) {
			if (d == null || d == '') return;
			var obj = eval('(' + d + ')');
			obj.tag = skin;
			obj.path =  '/Utilities/panel/skins/' + skin;
			window.$skins.list.push(obj);
		});
	}
	window.$skins = new skins();
	window.$skins.onchange(function (s, e) {
		s.loadCss();
	});
	//加载风格信息
	for (var i = 0; i < window.$skins._list.length; i++) {
		var skin = window.$skins._list[i];
		window.$skins.loadskin(skin);
	}
})(window);