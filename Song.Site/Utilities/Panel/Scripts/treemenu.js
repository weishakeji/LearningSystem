/*!
 * 主 题：树形菜单
 * 说 明：
 * 1、支持无限级菜单分类;
 * 2、可自定义节点样式，例如：粗体、斜体、颜色;
 * 3、节点事件可定义
 *
 * 作 者：微厦科技_宋雷鸣_10522779@qq.com
 * 开发时间: 2020年1月1日
 * 最后修订：2020年2月4日
 * github开源地址:https://github.com/weishakeji/WebdeskUI
 */
(function (win) {
	var treemenu = function (param) {
		if (param == null || typeof (param) != 'object') param = {};
		this.attrs = {
			target: '', //所在Html区域			
			width: '100%',
			height: '100%',
			id: '',
			complete: false,	//是否显示完成度
			bind: true, //是否实时数据绑定
			fold: false //是否折叠
		};
		for (var t in param) this.attrs[t] = param[t];
		eval($ctrl.attr_generate(this.attrs));
		/* 自定义事件 */
		//fold,折叠或展开;data，数据源变化时; change，切换根菜单,click点击菜单项
		eval($ctrl.event_generate(['fold', 'data', 'change', 'resize', 'click']));

		this.datas = new Array(); //子级	
		this._datas = ''; //数据源的序列化字符串	
		this.dom = null; //控件的html对象
		this.domtit = null; //控件标签栏部分的html对象
		this.dombody = null; //控件内容区
		//默认数据
		this.def_data = {
			title: '数据加载...',
			tit: 'load',
			type: 'loading',
			ico: 'e621'
		};
		this.datas.push(this.def_data);
		//初始化并生成控件
		this._initialization();
		this.bind = this._bind;
		//
		$ctrls.add({
			id: this.id,
			obj: this,
			dom: this.dom,
			type: 'treemenu'
		});
	};
	var fn = treemenu.prototype;
	fn._initialization = function () {
		if (!this._id) this._id = 'treemenu_' + new Date().getTime();
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
			if (obj.dom) {
				obj.dom.width(val);
				obj.dombody.width(val - 40);
				obj.trigger('resize', {
					width: val,
					height: obj._height,
					action: 'width'
				});
			}
		},
		'height': function (obj, val, old) {
			if (obj.dom) {
				obj.dom.height(val);
				obj.domtit.height(val);
				//obj.dombody.height(obj.dom.height());
				obj.trigger('resize', {
					width: obj._width,
					height: val,
					action: 'height'
				});
			}
		},
		'complete': function (obj, val, old) {
			if (obj.dom) {
				if (val) {
					obj.dom.find('complete').show();
				} else {
					obj.dom.find('complete').hide();
				}
			}
		},
		//折叠与展开
		'fold': function (obj, val, old) {
			obj.domtit.find('tree-foldbtn').attr('class', obj.fold ? 'fold' : '');
			if (val) {
				//折叠
				obj.dom.width(40);
				//var offset = obj.dom.offset();
				obj.dombody.css('position', 'absolute');
				obj.dombody.left(obj.domtit.width());
				obj.dombody.height(obj.dom.height()).width(0);
			} else {
				obj.dom.width(obj.width);
				obj.dombody.width(obj.width - 40);
				window.setTimeout(function () {
					obj.dombody.css('position', 'relative');
					obj.dombody.left(0);
				}, 300);
			}
			//折叠事件
			obj.trigger('fold', {
				action: val ? 'fold' : 'open'
			});
		},
		//是否启动实时数据绑定
		'bind': function (obj, val, old) {
			if (val) {
				obj._setinterval = window.setInterval(function () {
					var str = JSON.stringify(obj.datas);
					if (str != obj._datas) {
						//计算数据源的层深等信息
						for (var i = 0; i < obj.datas.length; i++) {
							if (obj.datas[i].type && obj.datas[i].type == 'loading')
								obj.datas.splice(i, 1);
						}
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
		var area = $dom(this.target);
		if (area.length < 1) {
			console.log('treemenu所在区域不存在');
		} else {
			area.html(''); //清空原html节点
			//生成Html结构和事件
			for (var t in this._builder) this._builder[t](this);
			for (var t in this._baseEvents) this._baseEvents[t](this);
			this.width = this._width;
			this.height = this._height;
			this.complete = this._complete;
		}
	};

	//生成结构
	fn._builder = {
		shell: function (obj) {
			var area = $dom(obj.target);
			if (area.length < 1) {
				console.log('treemenu所在区域不存在');
				return;
			}
			area.addClass('treemenu').attr('ctrid', obj.id);
			obj.dom = area;
		},
		//左侧标题区
		title: function (obj) {
			obj.domtit = obj.dom.add('tree_tags');
			obj.domtit.add('tree-foldbtn');
			//左侧选项卡
			for (var i = 0; i < obj.datas.length; i++) {
				var item = obj.datas[i];
				var tabtag = obj.domtit.add('tree_tag');
				tabtag.attr('title', item.title).attr('treeid', item.id);
				tabtag.add('ico').html('&#x' + item.ico);
				tabtag.add('itemtxt').html(item.tit);
				if (item.type == 'loading') tabtag.addClass('loading');
			}
			//左侧空白区的高度
			var tags = obj.domtit.find('tree_tag');
			var hg = tags.height();
			obj.domtit.add('tree-tagspace').height('calc(100% - ' + (obj.datas.length * 60) + 'px)');
		},
		//右侧内容区
		body: function (obj) {
			obj.dombody = obj.dom.add('tree_body');
			for (var i = 0; i < obj.datas.length; i++) {
				var item = obj.datas[i];
				//右侧树形菜单区
				var area = obj.dombody.add('tree_area');
				area.attr('treeid', item.id).hide();
				//右侧菜单的大标题
				area.add('tree_tit').html(item.title);
				if (item.type == 'loading') {
					area.add('loading').html('&#x' + item.ico);
					continue;
				}
				if (item.childs) {
					for (var j = 0; j < item.childs.length; j++) {
						_addchild(area, item.childs[j], obj);
					}
				}
			}
			//添加树形的子级节点
			function _addchild(area, item, obj) {
				var box = area.add('tree_box');
				box.attr('treeid', item.id);
				obj._createNode(item, box);
				if (item.childs && item.childs.length > 0) {
					for (var i = 0; i < item.childs.length; i++) {
						_addchild(box, item.childs[i], obj);
					}
				}
			}
		}
	};
	//基础事件，初始化时即执行
	fn._baseEvents = {
		//树形菜单的收缩与展开
		fold: function (obj) {
			//左下角折叠按钮的事件
			obj.domtit.find('tree-foldbtn').click(function (event) {
				var node = event.target ? event.target : event.srcElement;
				while (!$dom(node).hasClass('treemenu')) node = node.parentNode;
				var crt = $ctrls.get($dom(node).attr('ctrid'));
				crt.obj.fold = !crt.obj.fold;
			});
			//当折叠时，鼠标滑过左侧标签后显示主体菜单，过几秒后自动消失
			obj.leavetime = 3;
			obj.dombody.bind('mouseover', function (event) {
				var node = event.target ? event.target : event.srcElement;
				while (!$dom(node).hasClass('treemenu')) node = node.parentNode;
				var crt = $ctrls.get($dom(node).attr('ctrid'));
				crt.obj.leavetime = 3;
			});
			obj.leaveInterval = window.setInterval(function () {
				if (obj.fold && --obj.leavetime <= 0) obj.dombody.width(0);
			}, 1000);
		},
		//左侧标签点击事件
		rootclick: function (obj) {
			obj.domtit.find('tree_tag').click(function (event) {
				var node = event.target ? event.target : event.srcElement;
				//获取标签id
				while (node.tagName.toLowerCase() != 'tree_tag') node = node.parentNode;
				var tag = $dom(node);
				//获取组件id
				while (!node.classList.contains('treemenu')) node = node.parentNode;
				var ctrid = $dom(node).attr('ctrid');
				//获取组件对象
				var crt = $ctrls.get(ctrid);
				//切换选项卡
				crt.obj.switch(obj, tag);
			});
			obj.domtit.find('tree_tag').bind('mouseover', function (event) {
				var node = event.target ? event.target : event.srcElement;
				//获取标签id
				while (node.tagName.toLowerCase() != 'tree_tag') node = node.parentNode;
				var tag = $dom(node);
				while (!$dom(node).hasClass('treemenu')) node = node.parentNode;
				var crt = $ctrls.get($dom(node).attr('ctrid'));
				if (!crt.obj.fold) return;
				crt.obj.dombody.show().css('z-index', 100).width(crt.obj.width - 40);
				crt.obj.leavetime = 3;
				crt.obj.switch(obj, tag);
			});
			obj.switch(obj, obj.domtit.find('tree_tag').first())
		}
	};
	//创建树形节点
	fn._createNode = function (item, box) {
		var node = box.add('tree-node');
		node.css('padding-left', ((item.level - 1) * 15) + 'px');
		if (item.intro) node.attr('title', item.intro);
		//节点类型
		node.attr('type', item.type ? item.type : 'node');
		node.add('ico').html('&#x' + (item.ico ? item.ico : 'a022'));
		var span = null;
		if (item.type == 'link') {
			var link = node.add('a');
			link.attr('href', item.url).attr('target', item.target ? item.target : '_blank');
			span = link.add('span');
		} else {
			span = node.add('span');
		}
		//完成度	
		if (item.complete < 100 && this.complete) {
			var surplus = 100 - item.complete;
			var color = "#00ca08";
			if (surplus >= 90) color = "#ff0000";
			else if (surplus >= 70) color = "#ff8a49";
			else if (surplus >= 50) color = "#e6a23c";
			else if (surplus >= 30) color = "#127ba0";
			node.add('complete').html(surplus).css('background-color', color);
			surplus = surplus < 20 ? 20 : surplus;
			node.css('border-image', 'linear-gradient( to right,' + color + ' ' + surplus + '%, rgba(255, 255, 255,0) ' + (surplus + 5) + '%, rgba(255, 255, 255,0))');
			node.css('border-bottom', 'solid 2px');
		}
		//字体样式
		if (item.font) {
			var fonts = span.merge(node.find('ico'));
			if (item.font.color) fonts.css('color', item.font.color, true);
			if (item.font.bold) fonts.css('font-weight', item.font.bold ? 'bold' : 'normal', true);
			if (item.font.italic) fonts.css('font-style', item.font.italic ? 'italic' : 'normal', true);
		}
		span.html(item.title);
		span.width('calc(100% - ' + ((item.level - 1) * 15 + 40) + 'px)');

		//如果有下级节点
		if (item.childs && item.childs.length > 0) {
			node.addClass('folder').click(function (event) {
				var n = event.target ? event.target : event.srcElement;
				while (n.tagName.toLowerCase() != 'tree-node') n = n.parentNode;
				var tnode = $dom(n);
				if (tnode.hasClass('folder')) {
					tnode.attr('class', 'folderclose');
					tnode.siblings('tree_box').hide();
				} else {
					tnode.attr('class', 'folder');
					tnode.siblings('tree_box').show();
				}

			});
		} else {
			if (item.type != 'link') {
				//节点点击事件
				node.click(function (event) {
					var n = event.target ? event.target : event.srcElement;
					while (n.tagName.toLowerCase() != 'tree_box') n = n.parentNode;
					//节点id
					var treeid = $dom(n).attr('treeid');
					//对象
					var tree = n;
					while (!$dom(tree).hasClass('treemenu')) tree = tree.parentNode;
					var crt = $ctrls.get($dom(tree).attr('ctrid'));
					var datanode = crt.obj.getData(treeid); //数据源节点
					crt.obj.trigger('click', {
						treeid: treeid,
						data: datanode
					});
				});
			}
		}
		return node;
	};
	//计算层深，并补全一些信息
	fn._calcLevel = function (items, level) {
		for (var i = 0; i < items.length; i++) {
			var item = items[i];
			//补全一些信息
			if (!item.id || item.id < 0) item.id = 'node_' + (i + Math.floor(Math.random() * 100000));
			if (!item.pid || item.pid < 0) item.pid = 0;
			if (!item.level || item.level <= 0) item.level = level;
			if (!item.path) item.path = item.title;
			if (!item.ico || item.ico == '') item.ico = 'a009';
			if (!item.tit || item.tit == '') item.tit = item.title;
			if (!item.index) item.index = i;
			if (item.childs && item.childs.length > 0) {
				for (var j = 0; j < item.childs.length; j++) {
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
	//切换选项卡
	fn.switch = function (obj, tag) {
		if (tag == null) return;
		this.domtit.find('tree_tag').removeClass('curr');
		tag.addClass('curr');
		this.dombody.childs().hide();
		this.dombody.find('tree_area[treeid=\'' + tag.attr('treeid') + '\']').show();
		var datanode = obj.getData(tag.attr('treeid')); //数据源节点
		obj.trigger('change', {
			data: obj.getData(tag.attr('treeid'))
		});
	}
	/*
	treemenu的静态方法
	*/
	treemenu.create = function (param) {
		if (param == null) param = {};
		var tobj = new treemenu(param);
		return tobj;
	};
	treemenu._initEvent = function () {
		window.addEventListener("resize", function () {
			var treebody = $dom('.treemenu tree_body');
			treebody.height(treebody.parent().height());
		}, false);
	}
	win.$treemenu = treemenu;
	win.$treemenu._initEvent();
})(window);