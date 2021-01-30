/*!
 * 主 题：竖形工具条
 * 说 明：
 * 1、用于页面左侧或右侧的工具条;
 *
 * 作 者：微厦科技_宋雷鸣_10522779@qq.com
 * 开发时间: 2020年2月14日
 * 最后修订：2020年2月28日
 * github开源地址:https://github.com/weishakeji/WebdeskUI
 */
(function (win) {
	var verticalbar = function (param) {
		if (param == null || typeof (param) != 'object') param = {};
		this.attrs = {
			target: '', //所在Html区域			
			width: 100,
			height: 30,
			level: 1000, //初始深度
			id: '',
			bind: true //是否实时数据绑定
		};
		for (var t in param) this.attrs[t] = param[t];
		eval($ctrl.attr_generate(this.attrs));
		/* 自定义事件 */
		//data:数据项源变动时;click:点击菜单项
		eval($ctrl.event_generate(['data', 'click']));

		this.datas = new Array(); //数据源
		this._datas = ''; //数据源的序列化字符串
		this.dom = null; //控件的html对象
		//this.domtit = null; //控件标签栏部分的html对象
		this.dombody = null; //控件内容区
		//初始化并生成控件
		this._initialization();
		this.bind = this._bind;
		//
		$ctrls.add({
			id: this.id,
			obj: this,
			dom: this.dom,
			type: 'dropmenu'
		});
	};
	var fn = verticalbar.prototype;
	fn._initialization = function () {
		if (!this._id) this._id = 'vbar_' + new Date().getTime();
	};
	//添加数据源
	fn.add = function (item) {
		if (item instanceof Array) {
			for (var i = 0; i < item.length; i++)
				this.add(item[i]);
		} else {
			this.datas.push(item);
		}
	};
	//当属性更改时触发相应动作
	fn._watch = {
		'width': function (obj, val, old) {
			if (obj.dom) obj.dom.width(val);
		},
		'height': function (obj, val, old) {
			if (obj.dom) obj.dom.height(val);
		},
		//设定深度
		'level': function (obj, val, old) {
			if (obj.dom) obj.dom.level(val);
			obj.dombody.find('vbar-panel').each(function () {
				var id = $dom(this).attr('pid');
				var data = obj.getData(id);
				if (data == null) return;
				$dom(this).level(data.level + val);
			});
		},
		//是否启动实时数据绑定
		'bind': function (obj, val, old) {
			if (val) {
				obj._setinterval = window.setInterval(function () {
					var str = JSON.stringify(obj.datas);
					if (str != obj._datas) {
						obj._restructure();
						obj._datas = str;
						obj.trigger('data', {
							data: obj.datas
						});
					}
				}, 10);
			} else {
				window.clearInterval(obj._setinterval);
			}
		}
	};
	//重构
	fn._restructure = function () {
		var area = $dom(this.target);
		if (area.length < 1) {
			console.log('verticalbar所在区域不存在');
		} else {
			area.html(''); //清空原html节点
			$dom('verticalbar-body[ctrid=\'' + this.id + '\']').remove();
			//计算数据源的层深等信息
			for (var i = 0; i < this.datas.length; i++)
				this.datas[i] = this._calcLevel(this.datas[i], 1);
			//生成Html结构和事件
			for (var t in this._builder) this._builder[t](this);
			for (var t in this._baseEvents) this._baseEvents[t](this);
			this.width = this._width;
			this.height = this._height;
			this.level = this._level;
		}
	};
	//生成结构
	fn._builder = {
		shell: function (obj) {
			var area = $dom(obj.target);
			if (area.length < 1) {
				console.log('verticalbar所在区域不存在');
				return;
			}
			area.addClass('verticalbar').attr('ctrid', obj.id);
			obj.dom = area;
		},
		//主菜单栏
		title: function (obj) {
			//obj.domtit = obj.dom.add('vbar_roots');
			if (obj.datas == null || obj.datas.length < 1) return;
			for (var i = 0; i < obj.datas.length; i++) {
				var node = _rootnode(obj.datas[i], obj);
				if (node != null) obj.dom.append(node);
			}
			//生成根菜单项
			function _rootnode(item, obj) {
				if (item == null) return null;
				if (item.type == 'hr') return 'hr';
				//生成根节点
				var node = $dom(document.createElement('vbar-item'));
				node.attr('type', item.type ? item.type : 'node');
				node.attr('nid', item.id).css({
					'line-height': obj._width + 'px',
					'height': obj._width + 'px'
				});
				var span = null;
				if (item.type == 'link') {
					var link = node.add('a');
					link.attr('href', item.url).attr('target', item.target ? item.target : '_blank');
					link.html('&#x' + (item.ico ? item.ico : 'a022'));
				} else {
					node.html('&#x' + (item.ico ? item.ico : 'a022'));
				}
				
				return node;
			}
		},
		//子菜单内容区
		body: function (obj) {
			obj.dombody = $dom(document.body).add('verticalbar-body');
			obj.dombody.addClass('verticalbar').attr('ctrid', obj.id);
			for (var i = 0; i < obj.datas.length; i++) {
				if (obj.datas[i] == null) continue;
				var panel = $dom(document.createElement('vbar-panel'));
				panel.attr('pid', obj.datas[i].id).level(obj.datas[i].level);
				if (obj.datas[i].childs && obj.datas[i].childs.length > 0) {
					//计算高度
					var height = 0;
					for (var j = 0; j < obj.datas[i].childs.length; j++) {
						if (obj.datas[i].childs[j].type && obj.datas[i].childs[j].type == 'hr') {
							panel.append('hr');
							height += 1;
							continue;
						}
						height += 30;
						panel.append(_childnode(obj.datas[i].childs[j], obj)).addClass('child');
					}
					panel.height(height);
				} else {
					//如果没有子级菜单，则显示提示
					panel.html(obj.datas[i].title);
				}
				obj.dombody.append(panel);
			}

			function _childnode(item, obj) {
				if (item == null) return null;
				var node = $dom(document.createElement('vbar-node'));
				node.attr('nid', item.id);
				node.add('ico').html(item.ico ? '&#x' + item.ico : '');
				//节点类型
				node.attr('type', item.type ? item.type : 'node');
				var span = null;
				if (item.type == 'link') {
					var link = node.add('a');
					link.attr('href', item.url).attr('target', item.target ? item.target : '_blank');
					span = link.add('span');
				} else {
					span = node.add('span');
				}
				//字体样式
				if (item.font) {
					if (item.font.color) node.css('color', item.font.color);
					if (item.font.bold) span.css('font-weight', item.font.bold ? 'bold' : 'normal');
					if (item.font.italic) span.css('font-style', item.font.italic ? 'italic' : 'normal');
				}
				span.html(item.title);
				node.attr('title', item.intro && item.intro.length > 0 ? item.intro : item.title);
				if (item.childs && item.childs.length > 0) node.attr('child', true).add('child');
				return node;
			}
		}
	};
	//基础事件，初始化时即执行
	fn._baseEvents = {
		interval: function (obj) {
			obj.dombody.find('vbar-panel').bind('mouseover', function (e) {
				obj.leavetime = 3;
				obj.leave = false;
			});
			obj.leaveInterval = window.setInterval(function () {
				if (!obj.leave) return;
				if (--obj.leavetime <= 0) {
					obj.dombody.find('vbar-panel').hide();
					obj.dom.find('vbar-item').removeClass('hover');
				}
			}, 1000);
		},
		//根菜单滑过事件
		root_hover: function (obj) {
			obj.dom.find('vbar-item').bind('mouseover', function (event) {
				var n = event.target ? event.target : event.srcElement;
				while (n.tagName.toLowerCase() != 'vbar-item') n = n.parentNode;
				var node = $dom(n);
				var obj = verticalbar._getObj(n);
				var nid = node.attr('nid');
				//隐藏其它面板
				var brother = obj.getBrother(nid);
				for (var i = 0; i < brother.length; i++) {
					obj.dom.find('vbar-item[nid=\'' + brother[i].id + '\']').removeClass('hover');
					$dom('vbar-panel[pid=\'' + brother[i].id + '\']').hide();
					$dom('vbar-panel[pid=\'' + brother[i].id + '\'] vbar-item').removeClass('hover');
				}
				node.addClass('hover');

				//显示当前面板
				var offset = node.offset();
				var panel = $dom('vbar-panel[pid=\'' + nid + '\']');
				if (panel != null || panel.length > 0) {
					panel.show();
					var maxwd = window.innerWidth;
					var maxhg = window.innerHeight;
					var left = offset.left + panel.width() > maxwd ? offset.left - panel.width() - 8 : offset.left + obj.width + 8;
					var top = offset.top + panel.width() > maxhg ? offset.top - panel.height() : offset.top;
					//当前面板的位置
					panel.left(left).top(top);
					if (left - offset.left > 0) panel.attr('direction', 'left');
					//.attr('x', left - offset.left).attr('y', top - offset.top);
				}
				obj.leavetime = 3;
				obj.leave = false;

			});
			//当鼠标离开面板时，才允许计算消失时间
			obj.dombody.find('vbar-panel').merge(obj.dom.find('vbar-item'))
				.bind('mouseleave', function (e) {
					obj.leavetime = 3;
					obj.leave = true;
				});
		},
		//节点鼠标点击事件
		node_click: function (obj) {
			obj.dom.find('vbar-item:not([type=link])').merge(obj.dombody.find('vbar-node:not([type=link])'))
				.click(function (event) {
					var n = event.target ? event.target : event.srcElement;
					while ($dom(n).attr('nid') == null) n = n.parentNode;
					//节点id
					var nid = $dom(n).attr('nid');
					var obj = verticalbar._getObj(n);
					var data = obj.getData(nid);
					if (data.childs) return;
					//
					obj.trigger('click', {
						data: data
					});
					obj.leave = true;
					obj.dombody.find('vbar-panel').hide();
					obj.dom.find('vbar-item').removeClass('hover');
				});
		}
	};

	//计算层深
	fn._calcLevel = function (item, level) {
		if (item == null) return;
		//补全一些信息
		if (!item.id || item.id <= 0) item.id = Math.floor(Math.random() * 100000);
		if (!item.pid || item.pid < 0) item.pid = 0;
		if (!item.level || item.level <= 0) item.level = level;
		if (!item.path) item.path = item.title;
		//计算层深
		if (item.childs && item.childs.length > 0) {
			for (var i = 0; i < item.childs.length; i++) {
				item.childs[i].pid = item.id;
				item.childs[i].path = item.path + ',' + item.childs[i].title;
				item.childs[i] = this._calcLevel(item.childs[i], level + 1);
			}
		}
		return item;
	};
	//获取数据源的节点
	fn.getData = function (treeid) {
		if (this.datas.length < 1) return null;
		return getdata(treeid, this.datas);
		//
		function getdata(treeid, datas) {
			var d = null;
			for (var i = 0; i < datas.length; i++) {
				if (datas[i].id == treeid) return datas[i];
				if (datas[i].childs && datas[i].childs.length > 0)
					d = getdata(treeid, datas[i].childs);
				if (d != null) return d;
			}
			return d;
		}
	};
	//获取当前节点的兄弟节点（数据源）
	fn.getBrother = function (treeid) {
		var d = this.getData(treeid);
		if (d == null) return null;
		var brt = [];
		var datas = d.pid == 0 ? this.datas : this.getData(d.pid).childs;
		for (var i = 0; i < datas.length; i++) {
			if (datas[i].id != treeid) brt.push(datas[i]);
		}
		return brt;
	};
	//当前节点的所有子级（递归）
	fn.getChilds = function (treeid) {
		var childs = [];
		var d = this.getData(treeid);
		if (d == null) return childs;
		getdata(d.childs, childs);

		function getdata(datas, childs) {
			if (!datas) return;
			for (var i = 0; i < datas.length; i++) {
				childs.push(datas[i]);
				if (datas[i].childs && datas[i].childs.length > 0)
					getdata(datas[i].childs, childs);
			}
		}
		return childs;
	};
	/*
	静态方法
	*/
	verticalbar.create = function (param) {
		if (param == null) param = {};
		var tobj = new verticalbar(param);
		return tobj;
	};
	//用于事件中，取点击的pagebox的对象
	verticalbar._getObj = function (node) {		
		while (!node.classList.contains('verticalbar')) node = node.parentNode;
		var ctrl = $ctrls.get(node.getAttribute('ctrid'));
		return ctrl.obj;
	};
	win.$vbar = verticalbar;

})(window);