/*!
 * 主 题：下拉菜单
 * 说 明：
 * 1、支持无限级菜单分类;
 * 2、可自定义节点样式，例如：粗体、斜体、颜色;
 * 3、节点事件可定义
 *
 * 作 者：微厦科技_宋雷鸣_10522779@qq.com
 * 开发时间: 2020年2月14日
 * 最后修订：2020年2月28日
 * github开源地址:https://github.com/weishakeji/WebdeskUI
 */

(function (win) {
	var dropmenu = function (param) {
		if (param == null || typeof (param) != 'object') param = {};
		this.attrs = {
			target: '', //所在Html区域		
			deftype: 'node',		//	默认标识类型
			width: 0,
			height: 30,
			plwidth: 180, //子菜单面板的宽度
			level: 1000, //菜单的初始深度
			id: '',
			bind: true //是否实时数据绑定
		};
		for (let t in param) this.attrs[t] = param[t];
		eval($ctrl.attr_generate(this.attrs));
		/* 自定义事件 */
		//data:数据项源变动时;click:点击菜单项
		eval($ctrl.event_generate(['data', 'click']));

		this.datas = new Array(); //数据源
		this._datas = ''; //数据源的序列化字符串
		this.dom = null; //控件的html对象
		this.domtit = null; //控件标签栏部分的html对象
		this.dombody = null; //控件内容区
		//默认数据
		this.def_data = {
			title: '加载中...',
			tit: 'load',
			type: 'loading',
			ico: 'e621'
		};
		this.datas.push(this.def_data);
		//初始化并生成控件
		this._initialization();
		this.bind = this._bind;
		//this.target = this._target;
		//
		$ctrls.add({
			id: this.id,
			obj: this,
			dom: this.dom,
			type: 'dropmenu'
		});
	};
	var fn = dropmenu.prototype;
	fn._initialization = function () {
		if (!this._id) this._id = 'dropmenu_' + Math.random();
	};
	//添加数据源
	fn.add = function (item) {
		if (item instanceof Array) {
			for (let i = 0; i < item.length; i++)
				this.add(item[i]);
		} else {
			this.datas.push(item);
		}
	};
	//当属性更改时触发相应动作
	fn._watch = {
		'target': function (obj, val, old) {
			let area = $dom(obj.target);
			if (area.length < 1) {
				console.log('dropmenu的target不正确');
				return;
			}
			//if(obj.width<=0)
			//obj.width=area.width();
			//console.log(area.width());
		},
		'width': function (obj, val, old) {
			if (obj.dom) {
				obj.dom.width(val);
				//第一次下拉菜单的宽度，不受plwidth属性限制
				if (obj.datas && obj.datas.length > 0) {
					let wd = val / obj.datas.length;
					wd = wd > obj.plwidth ? wd : obj.plwidth;
					obj.dombody.find('drop-panel.level1').width(wd);
				}
			};
		},
		'height': function (obj, val, old) {
			if (obj.dom) obj.dom.height(val);
		},
		//子菜面板宽度
		'plwidth': function (obj, val, old) {
			if (obj.dombody) obj.dombody.find('drop-panel:not(.level1)').width(val);
		},
		//设定深度
		'level': function (obj, val, old) {
			if (obj.dom) obj.dom.level(val);
			obj.dombody.find('drop-panel').each(function () {
				let id = $dom(this).attr('pid');
				let data = obj.getData(id);
				if (data == null) return;
				$dom(this).level(data.level + val);
			});
		},
		//是否启动实时数据绑定
		'bind': function (obj, val, old) {
			if (val) {
				obj._setinterval = window.setInterval(function () {
					let str = JSON.stringify(obj.datas);
					if (str != obj._datas) {
						//去除loading信息
						for (let i = 0; i < obj.datas.length; i++) {
							if (obj.datas[i].type && obj.datas[i].type == 'loading')
								obj.datas.splice(i, 1);
						}
						//计算数据源的层深等信息
						obj.datas = obj._calcLevel($dom.clone(obj.datas), 1);
						obj._restructure();
						obj._datas = JSON.stringify(obj.datas);
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
		let area = $dom(this.target);
		if (area.length < 1) {
			console.log('dropmenu所在区域不存在');
		} else {
			area.html(''); //清空原html节点
			$dom('drop-body[ctrid=\'' + this.id + '\']').remove();
			//生成Html结构和事件
			for (let t in this._builder) this._builder[t](this);
			for (let t in this._baseEvents) this._baseEvents[t](this);
			this.target = this._target;
			if (this._width > 0)
				this.width = this._width;
			this.height = this._height;
			this.plwidth = this._plwidth;
			this.level = this._level;
		}
	};
	//生成结构
	fn._builder = {
		shell: function (obj) {
			let area = $dom(obj.target);
			if (area.length < 1) {
				console.log('dropmenu所在区域不存在');
				return;
			}
			area.addClass('dropmenu').attr('ctrid', obj.id);
			obj.dom = area;
		},
		//主菜单栏
		title: function (obj) {
			obj.domtit = obj.dom.add('drop_roots');
			if (obj.datas == null || obj.datas.length < 1) return;
			//如果数据源不是数组，转为数组
			if (!(obj.datas instanceof Array)) {
				let tm = obj.datas;
				obj.datas = new Array();
				obj.datas.push(tm);
			}
			for (let i = 0; i < obj.datas.length; i++) {
				let node = obj._createNode(obj.datas[i]);
				if (obj.datas[i].type == 'loading') node.addClass('loading');
				if (node != null) obj.domtit.append(node);
			}
		},
		//子菜单内容区
		body: function (obj) {
			obj.dombody = $dom(document.body).add('drop-body');
			obj.dombody.addClass('dropmenu').attr('ctrid', obj.id);
			for (let i = 0; i < obj.datas.length; i++) {
				if (obj.datas[i] == null) continue;

				if (obj.datas[i].type == 'loading') continue;
				if (obj.datas[i].childs && obj.datas[i].childs.length > 0)
					_childs(obj.datas[i], obj);
			}

			function _childs(item, obj) {
				let panel = $dom(document.createElement('drop-panel'));
				panel.attr('root', obj.target);
				panel.attr('pid', item.id).level(item.level + item.index + 1);
				if (item.level == 1) panel.addClass('level1');
				//计算高度
				let height = 0;
				for (let i = 0; i < item.childs.length; i++) {
					if (item.childs[i].type && item.childs[i].type == 'hr') {
						panel.append('hr');
						height += 1;
						continue;
					}
					height += obj.height;
					panel.append(obj._createNode(item.childs[i]));
					if (item.childs[i].childs && item.childs[i].childs.length > 0)
						_childs(item.childs[i], obj);
				}
				panel.height(height).hide();
				obj.dombody.append(panel);
			}
		}
	};
	//基础事件，初始化时即执行
	fn._baseEvents = {
		interval: function (obj) {
			obj.dombody.find('drop-panel').bind('mouseover', function (e) {
				obj.leavetime = 1;
				obj.leave = false;
			});
			obj.leaveInterval = window.setInterval(function () {
				if (!obj.leave) return;
				if (--obj.leavetime <= 0) {
					obj.dombody.find('drop-panel').hide();
					obj.domtit.find('drop-node').removeClass('hover');
				}
			}, 1000);
		},
		//根菜单滑过事件
		root_hover: function (obj) {
			obj.domtit.find('drop-node').bind('mouseover', function (event) {
				let n = event.target ? event.target : event.srcElement;
				while (n.tagName.toLowerCase() != 'drop-node') n = n.parentNode;
				let node = $dom(n);
				let obj = dropmenu._getObj(n);
				let nid = node.attr('nid');
				//隐藏其它面板
				let brother = obj.getBrother(nid);
				if (brother == null) return;
				for (let i = 0; brother != null && i < brother.length; i++) {
					obj.domtit.find('drop-node[nid=\'' + brother[i].id + '\']').removeClass('hover');
					$dom('drop-panel[pid=\'' + brother[i].id + '\']').hide();
					$dom('drop-panel[pid=\'' + brother[i].id + '\'] drop-node').removeClass('hover');
					let childs = obj.getChilds(brother[i].id);
					for (let j = 0; j < childs.length; j++) $dom('drop-panel[pid=\'' + childs[j].id + '\']').hide();
				}
				node.addClass('hover');
				//显示当前面板
				let offset = node.offset();
				let panel = $dom('drop-panel[pid=\'' + nid + '\']');
				if (panel != null || panel.length > 0) {
					panel.show();
					panel.width(node.width());		//第一级面板宽度，与根菜单宽度相同
					let maxwd = window.innerWidth;
					let maxhg = window.innerHeight;
					let left = offset.left + panel.width() > maxwd ? offset.left + node.width() - panel.width() : offset.left;
					let top = offset.top + obj.height + panel.height() > maxhg ? offset.top - panel.height() : offset.top + obj.height;
					//当前面板的位置
					panel.left(left).top(top).attr('x', left - offset.left).attr('y', top - offset.top);
				}
				obj.leavetime = 1;
				obj.leave = false;
			});
		},
		//子菜单滑过事件
		node_hover: function (obj) {
			obj.dombody.find('drop-panel drop-node').bind('mouseover', function (event) {
				let n = event.target ? event.target : event.srcElement;
				while (n.tagName.toLowerCase() != 'drop-node') n = n.parentNode;
				let node = $dom(n);
				let obj = dropmenu._getObj(n);
				let nid = node.attr('nid');
				//隐藏其它面板
				let brother = obj.getBrother(nid);
				for (let i = 0; i < brother.length; i++) {
					$dom('drop-panel[pid=\'' + brother[i].id + '\']').hide();
					$dom('drop-panel[pid=\'' + brother[i].id + '\']').find('drop-node').removeClass('hover');
					obj.dombody.find('drop-node[nid=\'' + brother[i].id + '\']').removeClass('hover');
					let childs = obj.getChilds(brother[i].id);
					for (let j = 0; j < childs.length; j++) $dom('drop-panel[pid=\'' + childs[j].id + '\']').hide();
				}
				node.addClass('hover');
				//显示当前面板
				let offset = node.offset();
				let panel = $dom('drop-panel[pid=\'' + nid + '\']');
				if (panel != null || panel.length > 0) {
					panel.show();
					let maxwd = window.innerWidth;
					let maxhg = window.innerHeight;
					let x = Number(node.parent().attr('x'));
					let y = Number(node.parent().attr('y'));
					let left = x < 0 || offset.left + node.width() + panel.width() > maxwd ? offset.left - panel.width() + 5 : offset.left + node.width() - 5;
					let top = y <= 0 || offset.top + obj.height + panel.width() > maxhg ? offset.top - panel.height() + node.height() * 3 / 4 : offset.top + node.height() * 1 / 4;
					//当前面板的位置
					panel.left(left).top(top).attr('x', left - offset.left).attr('y', top - offset.top);
				}
			});
			//当鼠标离开面板时，才允许计算消失时间
			obj.dombody.find('drop-panel').merge(obj.domtit.find('drop-node'))
				.bind('mouseleave', function (e) {
					obj.leavetime = 1;
					obj.leave = true;
				});
		},
		//节点鼠标点击事件
		node_click: function (obj) {
			obj.dombody.find('drop-node:not([type=link])')
				.merge(obj.dom.find('drop-node:not([type=link])'))
				.click(function (event) {
					let n = event.target ? event.target : event.srcElement;
					while (n.tagName.toLowerCase() != 'drop-node') n = n.parentNode;
					//节点id
					let nid = $dom(n).attr('nid');
					let obj = dropmenu._getObj(n);
					let data = obj.getData(nid);
					//
					obj.trigger('click', {
						data: data
					});
					obj.leave = true;
					obj.dombody.find('drop-panel').hide();
					obj.domtit.find('drop-node').removeClass('hover');
				});
		}
	};

	//创建节点
	fn._createNode = function (item) {
		if (item == null) return null;
		let node = $dom(document.createElement('drop-node'));
		node.attr('nid', item.id).css({
			'line-height': this._height + 'px',
			'height': this._height + 'px'
		});
		//节点类型
		item.type = item.type ? item.type : this.deftype;
		node.attr('type', item.type);
		let span = null;
		if (item.type == 'link') {
			let link = node.add('a');
			if (item.img) link.add('ico').add('img').attr('src', item.img);
			else link.add('ico').html(item.ico ? '&#x' + item.ico : '');
			link.attr('href', item.url).attr('target', item.target ? item.target : '_blank');
			span = link.add('span');
		} else {
			if (item.img) node.add('ico').add('img').attr('src', item.img);
			else node.add('ico').html(item.ico ? '&#x' + item.ico : '');
			span = node.add('span');
		}
		//字体样式
		if (item.font) {
			if (item.font.color && item.font.color != null) {
				node.find("span").css('color', item.font.color, true);
				node.find("ico").css('color', item.font.color, true);
			}
			if (item.font.bold) span.css('font-weight', item.font.bold ? 'bold' : 'normal');
			if (item.font.italic) span.css('font-style', item.font.italic ? 'italic' : 'normal');
		}
		span.html(item.title);
		node.attr('title', item.intro && item.intro.length > 0 ? item.intro : item.title);
		if (item.childs && item.childs.length > 0) node.attr('child', true).add('child');
		return node;
	};
	//计算层深
	fn._calcLevel = function (items, level) {
		for (let i = 0; i < items.length; i++) {
			let item = items[i];
			//补全一些信息
			if (!item.id || item.id < 0) item.id = 'nid_' + Math.floor(Math.random() * 100000);
			if (!item.pid || item.pid < 0) item.pid = 0;
			if (!item.level || item.level <= 0) item.level = level;
			if (!item.path) item.path = item.title;
			//if (!item.ico || item.ico == '') item.ico = 'a009';
			if (!item.tit || item.tit == '') item.tit = item.title;
			if (!item.index) item.index = i;
			if (item.childs && item.childs.length > 0) {
				for (let j = 0; j < item.childs.length; j++) {
					item.childs[j].pid = item.id;
					item.childs[j].path = item.path + ',' + item.childs[j].title;
					item.childs = this._calcLevel(item.childs, level + 1);
				}
			}
		}
		return items;
	};
	//获取数据源的节点
	fn.getData = function (treeid) {
		if (this.datas.length < 1) return null;
		return $dom.clone(getdata(treeid, this.datas));
		//
		function getdata(treeid, datas) {
			let d = null;
			for (let i = 0; i < datas.length; i++) {
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
		let d = this.getData(treeid);
		if (d == null) return null;
		let brt = [];
		let datas = d.pid == 0 ? this.datas : this.getData(d.pid).childs;
		for (let i = 0; i < datas.length; i++) {
			if (datas[i].id != treeid) brt.push(datas[i]);
		}
		return brt;
	};
	//当前节点的所有子级（递归）
	fn.getChilds = function (treeid) {
		let childs = [];
		let d = this.getData(treeid);
		if (d == null) return childs;
		getdata(d.childs, childs);

		function getdata(datas, childs) {
			if (!datas) return;
			for (let i = 0; i < datas.length; i++) {
				childs.push(datas[i]);
				if (datas[i].childs && datas[i].childs.length > 0)
					getdata(datas[i].childs, childs);
			}
		}
		return childs;
	};
	/*
	dropmenu的静态方法
	*/
	dropmenu.create = function (param) {
		if (param == null) param = {};
		let tobj = new dropmenu(param);
		return tobj;
	};
	//用于事件中，取点击的pagebox的对象
	dropmenu._getObj = function (node) {
		//let node = event.target ? event.target : event.srcElement;
		while (!node.classList.contains('dropmenu')) node = node.parentNode;
		let ctrl = $ctrls.get(node.getAttribute('ctrid'));
		return ctrl.obj;
	};
	win.$dropmenu = dropmenu;

})(window);