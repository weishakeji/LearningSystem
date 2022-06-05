/*!
 * 主 题：选项卡
 * 说 明：
 * 1、选项卡无限增加；
 * 2、鼠标滚轴切换选项卡
 * 3、鼠标双击关闭，关闭左侧、关闭右铡
 * 4、可全屏操作;
 *
 * 作 者：微厦科技_宋雷鸣_10522779@qq.com
 * 开发时间: 2020年1月1日
 * 最后修订：2020年2月4日
 * github开源地址:https://github.com/weishakeji/WebdeskUI
 */
(function (win) {
    var tabs = function (param) {
        if (param == null || typeof (param) != 'object') param = {};
        this.attrs = {
            target: '', //所在Html区域
            size: 0, //选项卡个数
            maximum: 100, //最多能有多少个选项卡，并没有用到这个参数
            width: '100%',
            height: '100%',
            default: null, //默认标签
            id: '',
            nowheel: false, //禁用鼠标滚轮切换
            morebox: false, //更多标签的面板是否显示
            cntmenu: false //右键菜单是否显示
        };
        for (var t in param) this.attrs[t] = param[t];
        eval($ctrl.attr_generate(this.attrs));
        /* 自定义事件 */
        //shut:关闭标签; add:添加标签；change:切换标签;load:内页加载完成; full:标签项全屏
        eval($ctrl.event_generate(['shut', 'add', 'change', 'load', 'full', 'help']));
        //以下不支持双向绑定
        this.childs = new Array(); //子级		
        this.dom = null; //控件的html对象
        this.domtit = null; //控件标签栏部分的html对象
        this.dombody = null; //控件内容区
        this.domenu = null; //控件右键菜单的html对象
        this.domore = null; //控件右侧更多标签的区域名		

        this._open();
        this.width = this._width;
        this.height = this._height;
        this.morebox = this._morebox;
        if (this.childs.length < 1 && this.default) {
            var th = this;
            window.setTimeout(function () {
                th.add(th.default);
            }, 1000);
        };

        //
        $ctrls.add({
            id: this.id,
            obj: this,
            dom: this.dom,
            type: 'tabs'
        });
    };
    var fn = tabs.prototype;
    fn._initialization = function () {
        if (!this._id) this._id = 'tabs_' + new Date().getTime();
    };
    //当属性更改时触发相应动作
    fn._watch = {
        'width': function (obj, val, old) {
            if (obj.dom) obj.dom.width(val);
            obj.trigger('resize', {
                width: val,
                height: obj._height
            });
        },
        'height': function (obj, val, old) {
            if (obj.dom) obj.dom.height(val);
            obj.trigger('resize', {
                width: obj._width,
                height: val
            });
        },
        //更多标签的面板是否显示
        'morebox': function (obj, val, old) {
            if (obj.domore) {
                if (val) obj.domore.show().width(200).css('opacity', 1);
                if (!val) obj.domore.width(0).css('opacity', 0);
                window.setTimeout(function () {
                    if (!val) obj.domore.hide();
                }, 300);
            }
        },
        //右键菜单的显示
        'cntmenu': function (obj, val, old) {
            if (obj.domenu) {
                if (val) obj.domenu.show();
                if (!val) obj.domenu.hide();
            }
        }
    };
    fn._open = function () {
        this._initialization();
        //创建控件html对象
        for (var t in this._builder) this._builder[t](this);
        for (var t in this._baseEvents) this._baseEvents[t](this);
    };
    fn._builder = {
        shell: function (obj) {
            var area = $dom(obj.target);
            if (area.length < 1) {
                console.log('tabs所在区域不存在');
                return;
            }
            area.addClass('tabsbox').attr('ctrid', obj.id);
            obj.dom = area;
        },
        title: function (obj) {
            var tagarea = obj.dom.add('tabs_tagarea');
            var tagsbox = tagarea.add('tabs_tagbox');
            obj.domtit = tagsbox;
            //右上角的更多按钮
            obj.dom.append('tabs_more');
        },
        body: function (obj) {
            obj.dombody = obj.dom.add('tabs_body');
        },
        //右侧，更多标签的区域
        morebox: function (obj) {
            obj.domore = obj.dom.add('tabs_morebox');
        },
        //右键菜单
        contextmenu: function (obj) {
            var menu = obj.dom.add('tabs_contextmenu');
            menu.add('menu_fresh').html('刷新');
            //menu.add('menu_freshtime').attr('num', 10).html('定时刷新(10秒)');
            menu.add('menu_print').html('打印');
            menu.add('hr');
            menu.add('menu_full').html('最大化');
            //menu.add('menu_restore').html('还原').addClass('disable');
            menu.add('menu_link').html('<a target=\'_blank\'>新窗体打开</a>');
            menu.add('hr');
            menu.add('menu_closeleft').html('关闭左侧');
            menu.add('menu_closeright').html('关闭右侧');
            menu.add('menu_closeall').html('关闭所有');
            menu.add('hr');
            menu.add('menu_close').html('关闭');
            obj.domenu = menu;
        }
    };
    //tabs的基础事件
    fn._baseEvents = {
        //设置自动执行的方法,用于一些菜单项的定时隐藏
        setinterval: function (obj) {

        },
        //右上角按钮事件
        morebtn: function (obj) {
            obj.dom.find('tabs_more').click(function (event) {
                var node = event.target ? event.target : event.srcElement;
                //获取组件id
                while (!node.classList.contains('tabsbox')) node = node.parentNode;
                var crt = $ctrls.get($dom(node).attr('ctrid'));
                crt.obj.morebox = !crt.obj.morebox;
            });
            //当鼠标滑动到面板上时
            obj.domore.bind('mouseover', function (event) {
                var node = event.target ? event.target : event.srcElement;
                //获取组件id
                while (!node.classList.contains('tabsbox')) node = node.parentNode;
                var crt = $ctrls.get($dom(node).attr('ctrid'));
                crt.obj.morebox = true;
            });
            obj.domore.bind('mouseleave', function (event) {
                var node = event.target ? event.target : event.srcElement;
                //获取组件id
                while (!node.classList.contains('tabsbox')) node = node.parentNode;
                var crt = $ctrls.get($dom(node).attr('ctrid'));
                crt.obj._morebox = false;
                window.setTimeout(function () {
                    if (!crt.obj._morebox)
                        crt.obj.morebox = false;
                }, 3000);
                //crt.obj.morebox = false;
            });
        },
        //右键菜单事件
        dropmenu: function (obj) {
            obj.domenu.bind('mouseover', function (event) {
                var node = event.target ? event.target : event.srcElement;
                tabs._getObj(node).cntmenu = true;
            });
            obj.domenu.bind('mouseleave', function (event) {
                var node = event.target ? event.target : event.srcElement;
                var obj = tabs._getObj(node);
                obj._cntmenu = false;
                window.setTimeout(function () {
                    if (!obj._cntmenu) obj.cntmenu = false;
                }, 500);
            });
            //菜单项的事件
            obj.dom.find('tabs_contextmenu>*').click(function (event) {
                //识别按钮，获取事件动作             
                var node = event.target ? event.target : event.srcElement;
                if (node.tagName.indexOf('_') < 0) return;
                var action = node.tagName.substring(node.tagName.indexOf('_') + 1).toLowerCase();
                //当前tabid和索引号
                var obj = tabs._getObj(node);
                var tabid = obj.domenu.attr('tabid');
                var index = Number(obj.domenu.attr('index'));
                //刷新
                if (action == 'fresh') {
                    var iframe = obj.dombody.find('tabpace[tabid=\'' + tabid + '\'] iframe');
                    iframe.attr('src', iframe.attr('src'));
                }
                //打印
                if (action == 'print') obj.print(tabid);
                //关闭
                if (action.indexOf('close') > -1) {
                    var tabids = new Array();
                    if (action == 'close') tabids.push(tabid);
                    if (action == 'closeall') {
                        for (var i = 0; i < obj.childs.length; i++) tabids.push(obj.childs[i].id);
                    }
                    if (action == 'closeright') {
                        for (var i = obj.childs.length - 1; i > index; i--) tabids.push(obj.childs[i].id);
                    }
                    if (action == 'closeleft') {
                        for (var i = 0; i < index; i++) tabids.push(obj.childs[i].id);
                    }
                    //批量关闭
                    for (var i = 0; i < tabids.length; i++) {
                        obj.remove(tabids[i], true, true);
                    }
                }
                //最大化
                if (action == 'full') {
                    obj.focus(String(tabid), true);
                    tabs.full(obj, tabid);
                }
                //还原
                if (action == 'restore') { }
                obj.cntmenu = false;
            });
        }

    };
    //增加选项卡
    fn.add = function (tab) {
        if (tab == null) return;
        if (tab instanceof Array) {
            for (var i = 0; i < tab.length; i++)
                this.add(tab[i]);
            return this;
        }
        //如果id已经存在，则不再添加，设置原有标签为焦点
        for (var i = 0; tab.id && i < this.childs.length; i++) {
            if (this.childs[i].id == tab.id) {
                this.focus(String(tab.id), true);
                return;
            }
        }
        //添加tab到控件	
        var size = this.childs.length;
        if (!tab.id) tab.id = 'tab_' + Math.floor(Math.random() * 100000) + '_' + (size + 1);
        if (!tab.index) tab.index = size + 1;
        if (!tab.ico) tab.ico = 'a01d';
        this.childs.push(tab);
        //添加标签
        var tabtag = this.domtit.add('tab_tag');
        tabtag.attr('title', tab.title).attr('tabid', tab.id);
        tabtag.add('ico').html('&#x' + tab.ico);
        tabtag.add('tagtxt').html(tab.title);
        tabtag.add('close');
        //添加更多标签区域
        var mtag = this.domore.add('tab_tag');
        mtag.add('ico').html('&#x' + tab.ico);
        mtag.attr('tabid', tab.id);
        mtag.add('tagtxt').html(tab.title).attr('title', tab.path.replace(/\,/g, ">"));
        mtag.add('close');
        //添加内容区
        var space = this.dombody.add('tabpace');
        space.attr('tabid', tab.id);
        var iframe = $dom(document.createElement('iframe'));
        iframe.attr({
            'name': tab.id,
            'id': tab.id,
            'frameborder': 0,
            'border': 0,
            'marginwidth': 0,
            'marginheight': 0,
            'src': tab.url ? tab.url : ''
        });
        iframe.bind('load', function (event) {
            var node = event.target ? event.target : event.srcElement;
            var obj = tabs._getObj(node);
            obj.trigger('load', {
                tabid: $dom(node).attr('id'), //标签id
                data: tab, //标签数据源
                iframe: iframe[0] //内页iframe对象
            });
            //禁用右键菜单
            var doc = node.contentDocument || node.contentWindow.document;
            doc.oncontextmenu = function () {
                return false
            }
        });
        //如果有帮助，但没有路径，那么路径等于标题
        if (!!tab.help && !tab.path) tab.path = tab.title;
        if (!!tab.path) {
            var path = space.add('tabpath');
            var paths = tab.path.split(',');
            for (var i = 0; i < paths.length; i++) {
                path.html(path.html() + paths[i]);
                if (i < paths.length - 1) path.html(path.html() + '<i>></i>');
            }
            //右侧按钮
            var btn = path.add('tabbar-btnbox');
            btn.add('print').html('&#xa046').attr('title', '打印');
            if (!!tab.help) btn.add('help').html('&#xa026').attr('title', '帮助');
            path.width('100%').height(30);
            iframe.height('calc(100% - 30px)');
        } else {
            iframe.height('100%');
        }
        space.append(iframe[0]);
        this.order();
        for (var t in this._tagBaseEvents) this._tagBaseEvents[t](this, tab.id);
        //新增标签的事件
        this.trigger('add', {
            tabid: tab.id,
            data: tab
        });
        this.focus(String(tab.id), true);
        return this;
    };
    //标签栏的可视区域,没有用到此代码
    fn._tagVisiblearea = function () {
        var offset = this.dom.offset();
        var tt = $dom('tabs_offset');
        tt.left(offset.left);
        tt.top(offset.top);
        tt.height(this.domtit.height());
        tt.width(this.dom.width() - 30);
        //this.dom.width() - 40
    };
    //标签tag的基础事件
    fn._tagBaseEvents = {
        //标签点击事件
        tagclick: function (obj, tabid) {
            obj.domtit.find('tab_tag[tabid=\'' + tabid + '\']')
                .merge(obj.domore.find('tab_tag[tabid=\'' + tabid + '\']'))
                .click(function (event) {
                    var node = event.target ? event.target : event.srcElement;
                    //是否移除
                    var isremove = node.tagName.toLowerCase() == 'close';
                    //获取标签id
                    while (node.tagName.toLowerCase() != 'tab_tag') node = node.parentNode;
                    var tabid = $dom(node).attr('tabid');
                    //获取组件id
                    while (!node.classList.contains('tabsbox')) node = node.parentNode;
                    var obj = tabs._getObj(node);
                    //是否移除标签
                    if (isremove) return obj.remove(tabid, true, true);
                    //切换焦点
                    obj.focus(String(tabid), true);
                });
            //双击标签关闭
            obj.domtit.find('tab_tag[tabid=\'' + tabid + '\']').dblclick(function (event) {
                var node = event.target ? event.target : event.srcElement;
                while (node.tagName.toLowerCase() != 'tab_tag') node = node.parentNode;
                var tabid = $dom(node).attr('tabid');
                var obj = tabs._getObj(node);
                obj.remove(tabid, true);
            });
        },
        //鼠标滚轴事件
        mousewheel: function (obj, tabid) {
            if (obj.nowheel) return;
            obj.domtit.find('tab_tag[tabid=\'' + tabid + '\']').bind('mousewheel', function (e) {
                e = e || window.event;
                var whell = e.wheelDelta ? e.wheelDelta : e.detail;
                var action = whell > 0 ? "up" : "down"; //上滚或下滚
                //获取组件
                var node = e.target ? e.target : e.srcElement;
                while (node.tagName.toLowerCase() != 'tabs_tagarea') node = node.parentNode;
                var ctrid = $dom(node).parent().attr('ctrid');
                var crt = $ctrls.get(ctrid);
                //当前活动标签
                var tag = crt.obj.domtit.find('.tagcurr');
                if (action == 'up') {
                    var next = tag.prev();
                    if (next.length < 1) next = crt.obj.domtit.childs().last();
                    crt.obj.focus(next, true);
                }
                if (action == 'down') {
                    var next = tag.next();
                    if (next.length < 1) next = crt.obj.domtit.childs().first();
                    crt.obj.focus(next, true);
                }
            });
        },
        //右键菜单
        contextmenu: function (obj, tabid) {
            obj.domtit.find('tab_tag[tabid=\'' + tabid + '\']')
                .merge(obj.dombody.find('tabpace[tabid=\'' + tabid + '\'] tabpath'))
                .bind('contextmenu', function (event) {
                    var node = event.target ? event.target : event.srcElement;
                    while ($dom(node).attr('tabid') == null) node = node.parentNode;
                    //当前tabs对象
                    var obj = tabs._getObj(node);
                    //当前标签id和索引号，用于关闭右侧或左侧时使用
                    var tabid = $dom(node).attr('tabid');
                    var data = obj.getData(tabid);    //当前节点的数据源
                    var index = obj.domtit.find('tab_tag[tabid=\'' + tabid + '\']').attr('index');
                    //菜单显示的位置
                    obj.cntmenu = true; //显示右键菜单
                    var maxwid = obj.dombody.width() + obj.dombody.offset().left; //右侧最大区域               
                    var off = obj.dom.offset();
                    var mouse = $dom.mouse(event);
                    var left = (mouse.x + obj.domenu.width()) > maxwid ? mouse.x - off.left - obj.domenu.width() + 10 : mouse.x - off.left - 10;
                    obj.domenu.left(left).top(mouse.y - off.top - 5);
                    obj.domenu.attr('tabid', tabid).attr('index', index);
                    obj.domenu.find('menu_link a').attr('href', data.url);
                    event.preventDefault();
                    return false;
                });
        },
        //帮助按钮点击事件
        help: function (obj, tabid) {
            obj.dombody.find('tabpace[tabid=\'' + tabid + '\'] help').click(function (event) {
                var node = event.target ? event.target : event.srcElement;
                while (node.tagName.toLowerCase() != 'tabpace') node = node.parentNode;
                var tabid = $dom(node).attr('tabid');
                var obj = tabs._getObj(node);
                var data = obj.getData(tabid); //当前数据项
                //触发帮助信息打开的事件
                obj.trigger('help', {
                    tabid: tabid,
                    data: data
                })
            });
        },
        //打印按钮的事件
        print: function (obj, tabid) {
            obj.dombody.find('tabpace[tabid=\'' + tabid + '\'] print').click(function (event) {
                var node = event.target ? event.target : event.srcElement;
                while (node.tagName.toLowerCase() != 'tabpace') node = node.parentNode;
                var tabid = $dom(node).attr('tabid');
                var obj = tabs._getObj(node);
                obj.print(tabid);
            });
        }
    };
    //排序
    fn.order = function () {
        var th = this;
        var tags = this.domtit.childs();
        th.domtit.childs().each(function (index) {
            //设置索引
            $dom(this).level(tags.length - index).attr('index', index);
            var tabid = $dom(this).attr('tabid');
            //索引号同步到tab对象上
            for (var i = 0; i < th.childs.length; i++) {
                if (th.childs[i].id == tabid) th.childs[i].index = index;
            }
        });
    };
    //获取数据源的某个标签对象
    fn.getData = function (id) {
        for (var i = 0; i < this.childs.length; i++) {
            if (this.childs[i].id == id)
                return this.childs[i];
        }
        return null;
    };
    //设置某一个标签为焦点
    //istrigger:是否触发事件
    fn.focus = function (tabid, istrigger) {
        if (tabid == null) return false;

        //如果tabid是数字，则按序号
        if (typeof tabid === 'number' && !isNaN(tabid)) {
            var tag = this.domtit.find('tab_tag').get(tabid);
            if (tag == null) return false;
            return this.focus(tag.attr('tabid'));
        }
        var tag = $dom.isdom(tabid) ? tabid : this.domtit.find('tab_tag[tabid=\'' + tabid + '\']');
        var data = this.getData(tag.attr('tabid'));
        //当前处于焦点的标签
        var tagcurr = this.domtit.find('.tagcurr');
        if (tagcurr.length > 0 && tag.attr('tabid') == tagcurr.attr('tabid')) {
            tag.level(this.domtit.childs().level() + 1);
            return false;
        }
        //
        //去除所有的焦点
        tagcurr.removeClass('tagcurr');
        this.dombody.childs().hide();
        this.order();
        //设置当前标签为焦点
        tag.addClass('tagcurr');
        tag.level(this.domtit.childs().level() + 1);
        this.dombody.find('tabpace[tabid=\'' + tag.attr('tabid') + '\']').show();
        //触发事件
        if (istrigger && data != null) this.trigger('change', {
            tabid: tag.attr('tabid'),
            data: data
        });
        //***********
        //计算标签区域的可视区域，左侧坐标与宽度
        var visiLeft = this.dom.offset().left;
        var visiWidth = this.domtit.parent().width() - 30;
        var area = this.domtit.parent();
        ///*
        //向左滚动
        var tagleft = (Number(tag.attr('index')) + 1) * 125;
        if (tagleft - visiLeft > visiWidth)
            area[0].scrollLeft = tagleft - visiLeft - visiWidth;
        //向右滚动
        tagleft = Number(tag.attr('index')) * tag.width();
        if (tagleft - visiLeft <= 0)
            area[0].scrollLeft = tagleft - visiLeft;
        return true;
    };
    //判断当前标签是否为焦点
    fn.isfocus = function (tabid) {
        var tagcurr = this.domtit.find('tab_tag.tagcurr[tabid=' + tabid + ']');
        return tagcurr.length > 0;
    };
    //打印选项卡的iframe中的内容页
    fn.print = function (tabid) {
        if (window.frames[tabid] == null) {
            var doc = $dom('iframe[name=\'' + tabid + '\']');
            if (doc.length > 0) doc[0].contentWindow.print();
        } else {
            window.frames[tabid].focus();
            window.frames[tabid].print();
        }
    };
    //移除某个选项卡
    //istrigger：是否触发事件
    //isdefault: 当移除所有选项卡，是否打开默认页
    fn.remove = function (tabid, istrigger, isdefault) {
        var data = this.getData(tabid);
        //触发关闭事件,如果返回false,则不再关闭
        if (istrigger) {
            var t = this.trigger('shut', {
                tabid: tabid,
                data: data
            });
            if (!t) return this;
        }
        var tittag = this.domtit.find('tab_tag[tabid=\'' + tabid + '\']');
        //设置关闭后的焦点选项卡
        var next = null;
        if (tittag.hasClass('tagcurr')) {
            next = tittag.next();
            if (next.length < 1) next = tittag.prev();
        } else {
            next = this.domtit.find('.tagcurr');
        }
        //移除html元素和数据
        tittag.remove();
        this.dombody.find('tabpace[tabid=\'' + tabid + '\']').remove();
        this.domore.find('tab_tag[tabid=\'' + tabid + '\']').remove();
        //从对象childs数组中移除
        for (var i = 0; i < this.childs.length; i++) {
            if (this.childs[i].id == tabid)
                this.childs.splice(i, 1);
        }
        //重建索引和焦点标签
        this.order();
        if (next != null) this.focus(next, true);

        //如果全都没有了，则显示默认标签
        if (this.childs.length < 1 && this.default && isdefault) {
            this.add(this.default);
        }
        return this;
    };
    //清空所有选项卡
    fn.clear = function () {
        var arr = Object.assign({}, this.childs);
        for (var i in arr) {
            this.remove(arr[i].id, false, false);
        }
    };
    /*** 
    以下是静态方法
    *****/
    tabs.create = function (param) {
        if (param == null) param = {};
        var tobj = new tabs(param);
        //pbox._initialization();
        return tobj;
    };
    //用于事件中，取点击的对象
    tabs._getObj = function (node) {
        //var node = event.target ? event.target : event.srcElement;
        while (node.classList.contains && !node.classList.contains('tabsbox'))
            node = node.parentNode;
        var ctrl = $ctrls.get(node.getAttribute('ctrid'));
        return ctrl.obj;
    };
    //刷新标签下的iframe
    tabs.fresh = function (tabname, func) {
        //tabs.js标签页的页面区域
        var iframe = $dom('iframe[name=' + tabname + ']');
        if (iframe.length > 0) {
            var win = iframe[0].contentWindow;
            //刷新父页面数据
            if (win && func != null) {
                if (func.charAt(func.length - 1) == ')') { eval('win.' + func); }
                else {
                    var f = eval('win.' + func);
                    if (f != null) f();
                }
            }
            if (win && func == null) {
                win.location.reload();
            }
        }
    }
    //一些基础事件
    tabs._baseEvents = function () {

    };
    //最大化内容区域
    tabs.full = function (obj, tabid) {
        var fbox = $dom('tabs_fullbox');
        if (fbox.length < 1) fbox = $dom(document.body).add('tabs_fullbox');
        //当前内容区，放到全屏fullbox中
        var tabpace = obj.dombody.find('tabpace[tabid=\'' + tabid + '\']');
        fbox.append(tabpace.find('iframe')).attr({
            crtid: obj.id,
            tabid: tabid
        });
        //fbox.find("iframe").width('100%').height('100%');
        tabpace.find('iframe').remove();
        //设置fullbox的初始位置
        var offset = tabpace.offset();
        fbox.left(offset.left).top(offset.top);
        fbox.width(tabpace.width()).height(tabpace.height()).show();
        fbox.css('transition', 'width 0.3s,height 0.3s,left 0.3s,top 0.3s,opacity 0.3s');
        window.setTimeout(function () {
            fbox.left(0).top(0);
            fbox.width('100%').height('100%');
        }, 300);
        //添加返回按钮
        var close = fbox.add('tabs_fullbox_back');
        close.click(function (e) {
            var fbox = $dom('tabs_fullbox');
            fbox.find('tabs_fullbox_back').hide();
            var crt = $ctrls.get(fbox.attr('crtid'));
            var tbody = crt.obj.dombody.find('tabpace[tabid=\'' + tabid + '\']');
            tbody.append(fbox.find('iframe'));
            //
            var tabpace = obj.dombody.find('tabpace[tabid=\'' + tabid + '\'] iframe');
            var offset = tabpace.offset();
            fbox.left(offset.left).top(offset.top);
            fbox.width(tabpace.width()).height(tabpace.height());
            window.setTimeout(function () {
                $dom('tabs_fullbox').remove();
            }, 300);
        });
        window.setTimeout(function () {
            $dom('tabs_fullbox_back').show();
        }, 500);
        //触发事件
        var data = obj.getData(tabid);
        obj.trigger('full', {
            tabid: tabid,
            data: data
        });
    }
    win.$tabs = tabs;
    win.$tabs._baseEvents();
})(window);