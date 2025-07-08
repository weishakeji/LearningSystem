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
 * 最后修订：2023年10月8日
 * Git开源地址:https://gitee.com/weishakeji/WebdeskUI
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
            help: true,     //是否显示帮助按钮
            print: true,     //是否显示打印按钮
            nowheel: false, //禁用鼠标滚轮切换
            morebox: false, //更多标签的面板是否显示
            cntmenu: false //右键菜单是否显示
        };
        for (let t in param) this.attrs[t] = param[t];
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
        //更多面板是否显示，当鼠标滑过时如果为false，3秒后隐藏面板
        this.leavetime = 3;
        this.leaveshow = false;
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
                if (val) obj.domore.show().width(200).css({ 'opacity': 1, 'right': '0px' });
                if (!val) obj.domore.width(0).css({ 'opacity': 0, 'right': '-5px' });
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
        for (let t in this._builder) this._builder[t](this);
        for (let t in this._baseEvents) this._baseEvents[t](this);
    };
    fn._builder = {
        shell: function (obj) {
            let area = $dom(obj.target);
            if (area.length < 1) {
                console.log('tabs所在区域不存在');
                return;
            }
            area.addClass('tabsbox').attr('ctrid', obj.id);
            obj.dom = area;
        },
        title: function (obj) {
            let tagarea = obj.dom.add('tabs_tagarea');
            let tagsbox = tagarea.add('tabs_tagbox');
            obj.domtit = tagsbox;
            //右上角的更多按钮
            tagarea.append('tabs_more');
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
            let menu = obj.dom.add('tabs_contextmenu');
            menu.add('menu_fresh').html('刷新');
            //menu.add('menu_freshtime').attr('num', 10).html('定时刷新(10秒)');
            if (obj.print) menu.add('menu_print').html('打印');
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
            obj.domenu.hide();
        }
    };
    //tabs的基础事件
    fn._baseEvents = {
        //右上角按钮事件
        morebtn: function (obj) {
            obj.dom.find('tabs_more').click(function (event) {
                let node = event.target ? event.target : event.srcElement;
                //获取组件id
                while (!node.classList.contains('tabsbox')) node = node.parentNode;
                let crt = $ctrls.get($dom(node).attr('ctrid'));
                crt.obj.morebox = true;
                crt.obj.leavetime = 3;
                crt.obj.leaveshow = true;
            });
            //当鼠标滑动到面板上时
            obj.domore.bind('mouseover,mousemove,mousedown', function (event) {
                let node = event.target ? event.target : event.srcElement;
                //获取组件id
                while (!node.classList.contains('tabsbox')) node = node.parentNode;
                let crt = $ctrls.get($dom(node).attr('ctrid'));
                crt.obj.morebox = true;
                crt.obj.leavetime = 3;
                crt.obj.leaveshow = true;
            });
            obj.domore.bind('mouseleave', function (event) {
                let node = event.target ? event.target : event.srcElement;
                //获取组件id
                while (!node.classList.contains('tabsbox')) node = node.parentNode;
                var crt = $ctrls.get($dom(node).attr('ctrid'));
                crt.obj._morebox = false;
                crt.obj.leavetime = 3;
                crt.obj.leaveshow = false;

            });
            obj.leaveInterval = window.setInterval(function () {
                if (!obj.leaveshow && --obj.leavetime <= 0)
                    obj.morebox = false;
            }, 200);
        },
        //右键菜单事件
        dropmenu: function (obj) {
            obj.domenu.bind('mouseover', function (event) {
                let node = event.target ? event.target : event.srcElement;
                tabs._getObj(node).cntmenu = true;
            });
            obj.domenu.bind('mouseleave', function (event) {
                let node = event.target ? event.target : event.srcElement;
                var obj = tabs._getObj(node);
                obj._cntmenu = false;
                window.setTimeout(function () {
                    if (!obj._cntmenu) obj.cntmenu = false;
                }, 500);
            });
            //菜单项的事件
            obj.dom.find('tabs_contextmenu>*').click(function (event) {
                //识别按钮，获取事件动作             
                let node = event.target ? event.target : event.srcElement;
                if (node.tagName.indexOf('_') < 0) return;
                let action = node.tagName.substring(node.tagName.indexOf('_') + 1).toLowerCase();
                //当前tabid和索引号
                let obj = tabs._getObj(node);
                let tabid = obj.domenu.attr('tabid');
                let index = Number(obj.domenu.attr('index'));
                //刷新
                if (action == 'fresh') {
                    let iframe = obj.dombody.find('tabpace[tabid=\'' + tabid + '\'] iframe');
                    iframe.attr('src', iframe.attr('src'));
                }
                //打印
                if (action == 'print') obj.printtab(tabid);
                //关闭
                if (action.indexOf('close') > -1) {
                    let tabids = new Array();
                    if (action == 'close') tabids.push(tabid);
                    if (action == 'closeall') {
                        for (let i = 0; i < obj.childs.length; i++) tabids.push(obj.childs[i].id);
                    }
                    if (action == 'closeright') {
                        for (let i = obj.childs.length - 1; i > index; i--) tabids.push(obj.childs[i].id);
                    }
                    if (action == 'closeleft') {
                        for (let i = 0; i < index; i++) tabids.push(obj.childs[i].id);
                    }
                    //批量关闭
                    for (let i = 0; i < tabids.length; i++) {
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
            for (let i = 0; i < tab.length; i++)
                this.add(tab[i]);
            return this;
        }
        //如果id已经存在，则不再添加，设置原有标签为焦点
        for (let i = 0; tab.id && i < this.childs.length; i++) {
            if (this.childs[i].id == tab.id) {
                this.focus(String(tab.id), true);
                return;
            }
        }
        //添加tab到控件	
        let size = this.childs.length;
        if (!tab.id) tab.id = 'tab_' + Math.floor(Math.random() * 100000) + '_' + (size + 1);
        if (!tab.index) tab.index = size + 1;
        if (!tab.ico) tab.ico = 'a01d';
        this.childs.push(tab);
        //添加标签
        let tabtag = this.domtit.add('tab_tag');
        tabtag.attr('title', tab.title).attr('tabid', tab.id);
        //图标样式
        let ico = tabtag.add('ico').html('&#x' + tab.ico);
        if (tab.icon) {
            if (tab.icon.color) ico.css('color', tab.icon.color);
            else if (tab.font && tab.font.color) ico.css('color', tab.font.color);
            if (tab.icon.x != 0) {
                let x = parseInt(ico.css('left'));
                ico.css('left', (x + tab.icon.x) + 'px');
            }
            if (tab.icon.y != 0) {
                let y = parseInt(ico.css('bottom'));
                ico.css('bottom', (y - tab.icon.y) + 'px');
            }
            if (tab.icon.size != 0) ico.css('transform', 'scale(' + (1 + tab.icon.size / 100) + ')');

        }
        //标签文本
        let txt = tabtag.add('tagtxt').html(tab.title);
        if (tab.font) {
            if (tab.font.color) txt.css('color', tab.font.color, true);
            if (tab.font.bold) txt.css('font-weight', tab.font.bold ? 'bold' : 'normal', true);
            if (tab.font.italic) txt.css('font-style', tab.font.italic ? 'italic' : 'normal', true);
        }
        tabtag.add('close');
        //添加更多标签区域
        let mtag = this.domore.add('tab_tag');
        ico = mtag.add('ico').html('&#x' + tab.ico);
        if (tab.icon) {
            if (tab.icon.color) ico.css('color', tab.icon.color);
            else if (tab.font && tab.font.color) ico.css('color', tab.font.color);
            if (tab.icon.x != 0) ico.css('padding-left', tab.icon.x + 'px');
            if (tab.icon.y != 0) ico.css('padding-top', tab.icon.y + 'px');
            if (tab.icon.size != 0) ico.css('transform', 'scale(' + (1 + tab.icon.size / 100) + ')');
        }
        mtag.attr('tabid', tab.id);
        txt = mtag.add('tagtxt').html(tab.title).attr('title', tab.path.replace(/\,/g, ">"));
        if (tab.font) {
            if (tab.font.color) txt.css('color', tab.font.color, true);
            if (tab.font.bold) txt.css('font-weight', tab.font.bold ? 'bold' : 'normal', true);
            if (tab.font.italic) txt.css('font-style', tab.font.italic ? 'italic' : 'normal', true);
        }
        mtag.add('close');
        //添加内容区
        let space = this.dombody.add('tabpace');
        space.attr('tabid', tab.id);
        let iframe = $dom(document.createElement('iframe'));
        iframe.attr({
            'name': tab.id,
            'id': tab.id,
            'frameborder': 0,
            'border': 0,
            'marginwidth': 0,
            'marginheight': 0
        });
        //设置iframe的src，即页面地址
        let src = tab.url ? tab.url : '';
        if (src != '') {
            if (!(/^(https?:\/\/|\/)/.test(src) || /^(http?:\/\/|\/)/.test(src) || /^\//.test(src))) {
                const baseUrl = window.location.href.split('/').slice(0, 3).join('/') + 
                window.location.pathname.split('/').slice(0, -1).join('/');
                src = baseUrl + '/' + src;
            }
            iframe.attr('src', src);
        }
        //加载完成
        iframe.bind('load', function (event) {
            let node = event.target ? event.target : event.srcElement;
            let obj = tabs._getObj(node);
            obj.trigger('load', {
                tabid: $dom(node).attr('id'), //标签id
                data: tab, //标签数据源
                iframe: iframe[0] //内页iframe对象
            });
            //禁用右键菜单
            let doc = node.contentDocument || node.contentWindow.document;
            doc.oncontextmenu = function () {
                return false
            }
        });
        //如果有帮助，但没有路径，那么路径等于标题
        if (!!tab.help && !tab.path) tab.path = tab.title;
        if (!!tab.path) {
            let path = space.add('tabpath');
            let paths = tab.path.split(',');
            for (let i = 0; i < paths.length; i++) {
                path.html(path.html() + paths[i]);
                if (i < paths.length - 1) path.html(path.html() + '<i>></i>');
            }
            //右侧按钮
            let btn = path.add('tabbar-btnbox');
            btn.add('div').attr('title', 'help').html('&#xa026');
            if (!this.help) btn.find("div[title='help']").hide();
            //打印按钮
            btn.add('div').attr('title', 'print').html('&#xa046');
            if (!this.print) btn.find("div[title='print']").hide();
            path.width('100%').height(35);
            iframe.height('calc(100% - 35px)');
        } else {
            iframe.height('100%');
        }
        space.append(iframe[0]);
        this.order();
        for (let t in this._tagBaseEvents) this._tagBaseEvents[t](this, tab.id);
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
        let offset = this.dom.offset();
        let tt = $dom('tabs_offset');
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
                    let node = event.target ? event.target : event.srcElement;
                    //是否移除
                    let isremove = node.tagName.toLowerCase() == 'close';
                    //获取标签id
                    while (node.tagName.toLowerCase() != 'tab_tag') node = node.parentNode;
                    let tabid = $dom(node).attr('tabid');
                    //获取组件id
                    while (!node.classList.contains('tabsbox')) node = node.parentNode;
                    let obj = tabs._getObj(node);
                    //是否移除标签
                    if (isremove) return obj.remove(tabid, true, true);
                    //切换焦点
                    obj.focus(String(tabid), true);
                });
            //双击标签关闭
            obj.domtit.find('tab_tag[tabid=\'' + tabid + '\']').dblclick(function (event) {
                let node = event.target ? event.target : event.srcElement;
                while (node.tagName.toLowerCase() != 'tab_tag') node = node.parentNode;
                let tabid = $dom(node).attr('tabid');
                let obj = tabs._getObj(node);
                obj.remove(tabid, true);
            });
        },
        //鼠标滚轴事件
        mousewheel: function (obj, tabid) {
            if (obj.nowheel) return;
            obj.domtit.find('tab_tag[tabid=\'' + tabid + '\']').bind('mousewheel', function (e) {
                e = e || window.event;
                let whell = e.wheelDelta ? e.wheelDelta : e.detail;
                let action = whell > 0 ? "up" : "down"; //上滚或下滚
                //获取组件
                let node = e.target ? e.target : e.srcElement;
                while (node.tagName.toLowerCase() != 'tabs_tagarea') node = node.parentNode;
                let ctrid = $dom(node).parent().attr('ctrid');
                let crt = $ctrls.get(ctrid);
                //当前活动标签
                let tag = crt.obj.domtit.find('.tagcurr');
                if (action == 'up') {
                    let next = tag.prev();
                    if (next.length < 1) next = crt.obj.domtit.childs().last();
                    crt.obj.focus(next, true);
                }
                if (action == 'down') {
                    let next = tag.next();
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
                    let node = event.target ? event.target : event.srcElement;
                    while ($dom(node).attr('tabid') == null) node = node.parentNode;
                    //当前tabs对象
                    let obj = tabs._getObj(node);
                    //当前标签id和索引号，用于关闭右侧或左侧时使用
                    let tabid = $dom(node).attr('tabid');
                    let data = obj.getData(tabid);    //当前节点的数据源
                    let index = obj.domtit.find('tab_tag[tabid=\'' + tabid + '\']').attr('index');
                    //菜单显示的位置
                    obj.cntmenu = true; //显示右键菜单
                    let maxwid = obj.dombody.width() + obj.dombody.offset().left; //右侧最大区域               
                    let off = obj.dom.offset();
                    let mouse = $dom.mouse(event);
                    let left = (mouse.x + obj.domenu.width()) > maxwid ? mouse.x - off.left - obj.domenu.width() + 10 : mouse.x - off.left - 10;
                    obj.domenu.left(left).top(mouse.y - off.top - 5);
                    obj.domenu.attr('tabid', tabid).attr('index', index);
                    obj.domenu.find('menu_link a').attr('href', data.url);
                    event.preventDefault();
                    return false;
                });
        },
        //帮助按钮点击事件
        help: function (obj, tabid) {
            obj.dombody.find('tabpace[tabid=\'' + tabid + '\']  div[title=help]').click(function (event) {
                let node = event.target ? event.target : event.srcElement;
                while (node.tagName.toLowerCase() != 'tabpace') node = node.parentNode;
                let tabid = $dom(node).attr('tabid');
                let obj = tabs._getObj(node);
                let data = obj.getData(tabid); //当前数据项
                //触发帮助信息打开的事件
                obj.trigger('help', {
                    tabid: tabid,
                    data: data
                })
            });
        },
        //打印按钮的事件
        print: function (obj, tabid) {
            obj.dombody.find('tabpace[tabid=\'' + tabid + '\'] div[title=print]').click(function (event) {
                let node = event.target ? event.target : event.srcElement;
                while (node.tagName.toLowerCase() != 'tabpace') node = node.parentNode;
                let tabid = $dom(node).attr('tabid');
                let obj = tabs._getObj(node);
                obj.printtab(tabid);
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
            let tabid = $dom(this).attr('tabid');
            //索引号同步到tab对象上
            for (let i = 0; i < th.childs.length; i++) {
                if (th.childs[i].id == tabid) th.childs[i].index = index;
            }
        });
    };
    //获取数据源的某个标签对象
    fn.getData = function (id) {
        for (let i = 0; i < this.childs.length; i++) {
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
            let tag = this.domtit.find('tab_tag').get(tabid);
            if (tag == null) return false;
            return this.focus(tag.attr('tabid'));
        }
        let tag = $dom.isdom(tabid) ? tabid : this.domtit.find('tab_tag[tabid=\'' + tabid + '\']');
        let data = this.getData(tag.attr('tabid'));
        //当前处于焦点的标签
        let tagcurr = this.domtit.find('.tagcurr');
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
        let visiLeft = this.dom.offset().left;
        let visiWidth = this.domtit.parent().width() - 30;
        let area = this.domtit.parent();
        ///*
        //向左滚动
        let tagleft = (Number(tag.attr('index')) + 1) * 125;
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
        let tagcurr = this.domtit.find('tab_tag.tagcurr[tabid=' + tabid + ']');
        return tagcurr.length > 0;
    };
    //打印选项卡的iframe中的内容页
    fn.printtab = function (tabid) {
        if (window.frames[tabid] == null) {
            let doc = $dom('iframe[name=\'' + tabid + '\']');
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
        let data = this.getData(tabid);
        //触发关闭事件,如果返回false,则不再关闭
        if (istrigger) {
            let t = this.trigger('shut', {
                tabid: tabid,
                data: data
            });
            if (!t) return this;
        }
        let tittag = this.domtit.find('tab_tag[tabid=\'' + tabid + '\']');
        //设置关闭后的焦点选项卡
        let next = null;
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
        for (let i = 0; i < this.childs.length; i++) {
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
        let arr = Object.assign({}, this.childs);
        for (let i in arr) {
            this.remove(arr[i].id, false, false);
        }
    };
    /*** 
    以下是静态方法
    */
    tabs.create = function (param) {
        if (param == null) param = {};
        let tobj = new tabs(param);
        //pbox._initialization();
        return tobj;
    };
    //用于事件中，取点击的对象
    tabs._getObj = function (node) {
        //let node = event.target ? event.target : event.srcElement;
        while (node.classList.contains && !node.classList.contains('tabsbox'))
            node = node.parentNode;
        let ctrl = $ctrls.get(node.getAttribute('ctrid'));
        return ctrl.obj;
    };
    //刷新标签下的iframe
    tabs.fresh = function (tabname, func) {
        //tabs.js标签页的页面区域
        let iframe = $dom('iframe[name=' + tabname + ']');
        if (iframe.length > 0) {
            let win = iframe[0].contentWindow;
            //刷新父页面数据
            if (win && func != null) {
                if (func.charAt(func.length - 1) == ')') { eval('win.' + func); }
                else {
                    let f = eval('win.' + func);
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
        let tabpace = obj.dombody.find('tabpace[tabid=\'' + tabid + '\']');
        var iframe = tabpace.find('iframe');
        //获取坐标与宽高
        var offset = iframe.offset();
        //设置位置与放大致全屏       
        obj.dom.css('position', 'static');
        iframe.css({ 'position': 'absolute', 'background-color': '#fff', 'z-index': 999998 })
            .left(offset.left).top(offset.top).width(offset.width).height(offset.height)
            .attr({ 'left': offset.left, 'top': offset.top, 'wd': offset.width, 'hg': offset.height });
        //添加返回按钮，默认是隐藏的
        let close = obj.dom.add('tabs_fullbox_back');
        //全屏
        window.setTimeout(function () {
            iframe.css('transition', 'width 0.3s,height 0.3s,left 0.3s,top 0.3s,opacity 0.3s');
            iframe.left(0).top(0);
            iframe.width('100%').height('100%');
            //触发全屏事件
            obj.trigger('full', {
                tabid: tabid,
                data: obj.getData(tabid)
            });
            close.show();
        }, 300);
        //返回按钮的点击事件
        close.click(function (e) {
            let tabpace = obj.dombody.find('tabpace[tabid=\'' + tabid + '\']');
            var iframe = tabpace.find('iframe');
            iframe.left(Number(iframe.attr('left'))).top(Number(iframe.attr('top')))
                .width(Number(iframe.attr('wd'))).height(Number(iframe.attr('hg')));
            close.remove();
            window.setTimeout(function () {
                //去除之前添加的属性
                iframe.css({ 'transition': 'none', 'position': 'static', 'background-color': 'transparent' })
                    .width('100%').height('calc(100% - 35px)');
                obj.dom.css('position', 'relative');
                //触发全屏还事件
                obj.trigger('restore', {
                    tabid: tabid,
                    data: obj.getData(tabid)
                });
            }, 300);
        });
    }
    win.$tabs = tabs;
    win.$tabs._baseEvents();
})(window);