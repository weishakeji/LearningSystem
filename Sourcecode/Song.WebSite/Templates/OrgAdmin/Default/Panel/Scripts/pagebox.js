/*!
 * 主 题：Pagebox.js 页面窗体
 * 说 明：
 * 1、可拖放，可缩放，模拟windows桌面窗体
 * 2、可上溯父级，遍历下级，父子窗体可互动
 * 3、自定义事件，多播，可追加、可去除
 * 4、属性支持双向绑定，可监听
 *
 * 作 者：微厦科技_宋雷鸣_10522779@qq.com
 * 开发时间: 2020年1月1日
 * 最后修订：2020年2月4日
 * github开源地址:https://github.com/weishakeji/WebdeskUI
 */
(function (win) {
    //窗体最小化时所处位置区域
    window.$pageboxcollect = '#pageboxcollect';
    //param: 初始化时的参数
    var box = function (param) {
        if (param == null || typeof (param) != 'object') param = {};
        //默认参数，
        this.attrs = {
            width: 200,
            height: 150,
            top: null,
            left: null,
            bottom: null,
            right: null,
            level: null, //窗体的层深
            initLvl: 10000, //初始层深
            title: '默认标题',
            ico: 'a021', //图标
            url: '',
            id: 0,
            pid: '', //父级窗体名称
            resize: true, //是否允许缩放大小
            move: true, //是否允许移动
            min: true, //是否允许最小化按钮
            max: true, //是否允许最大化按钮    
            print: true, //是否允许打印        
            close: true, //是否允许关闭按钮
            fresh: true, //是否允许刷新
            full: false, //打开后是否全屏，默认是false
            mini: false, //是否处于最小化状态
            showmask: false,     //是否显示背景遮罩
            dropmenu: false //下拉菜单是否显示
        };
        for (var t in param) this.attrs[t] = param[t];
        eval($ctrl.attr_generate(this.attrs));
        /* 自定义事件 */
        //shown打开，shut关闭，load加载，fail加载失败，
        //click点击，drag拖动,focus得到焦点，blur失去焦点
        //min最小化，full全屏，restore还原，resize缩放
        var customEvents = ['shown', 'shut', 'load', 'fail',
            'click', 'drag', 'focus', 'blur',
            'mini', 'full', 'restore', 'resize'
        ];
        eval($ctrl.event_generate(customEvents));
        //以下不支持双向绑定
        this.parent = null; //父窗体对象
        this.childs = new Array(); //子级窗体
        this.dom = null; //html对象  
        this._watchlist = new Array(); //自定义监听  
        this._isinit = false; //是否初始化   
        //增加窗体的总数
        box.total(1);
    };
    var fn = box.prototype;
    //初始化相关参数
    fn._initialization = function () {
        if (!this.id) this.id = 'pagebox_' + new Date().getTime();
        //是否有父级窗体
        var parent = $ctrls.get(this.pid);
        if (parent != null) {
            this.parent = parent.obj;
            parent.obj.childs.push(this);
        }
        this.width = this._method.calcSize(this._width, 'width');
        this.height = this._method.calcSize(this._height, 'height');
        //最前面的窗体，用于设置当前窗体的位置，以免覆盖之前的
        let topbox = box.gettop();
        if (topbox != null && topbox.full) topbox = null;
        //如果位置没有设置
        if (!this.top && this.bottom) this.top = box.availHeight() - this.height - this.bottom;
        if (!this.top && !this.bottom) {
            if (topbox == null || this.showmask)
                this.top = (box.availHeight() - document.body.scrollTop - this.height) / 2;
            else
                this.top = topbox.dom.offset().top + 30;
        }
        if (!this.left && this.right) this.left = box.availWidth() - this.width - this.right;
        if (!this.left && !this.right) {
            if (topbox == null || this.showmask)
                this.left = (box.availWidth() - document.body.scrollLeft - this.width) / 2;
            else
                this.left = topbox.dom.offset().left + 30;
        }
        //
        $ctrls.add({
            id: this.id,
            obj: this,
            type: 'pagebox'
        });
        this._isinit = true;
        return this;
    };
    //当属性更改时触发相应动作
    fn._watch = {
        //参数：
        //box:pagebox对象，val：传入的值，old:原值
        'title': function (box, val, old) {
            if (box.dom) {
                box.dom.find('pagebox_title pb-text').html(val);
                box.domin.find('pb-text').html(val);
            }
        },
        'url': function (box, val, old) {
            if (box.dom) box.dom.find('iframe').attr('src', val);
        },
        'width': function (box, val, old) {
            var newval = box._method.calcSize(val, 'width');
            if (box.dom) box.dom.width(newval);
        },
        'height': function (box, val, old) {
            var newval = box._method.calcSize(val, 'height');
            if (box.dom) box.dom.height(newval);
        },
        'left': function (box, val, old) {
            var newval = box._method.calcSize(val, 'width');
            if (box.dom) box.dom.left(newval);
        },
        'top': function (box, val, old) {
            var newval = box._method.calcSize(val, 'height');
            if (box.dom) box.dom.top(newval);
        },
        'right': function (box, val, old) {
            var newval = box._method.calcSize(val, 'height');
            box.left = box.availWidth() - box._width - newval;
        },
        'bottom': function (box, val, old) {
            var newval = box._method.calcSize(val, 'height');
            box.top = box.availHeight() - box._height - newval;
        },
        'level': function (box, val, old) {
            if (box.dom) box.dom.level(val);
        },
        'full': function (box, val, old) {
            if (val == old) return;
            if (val) box.toFull(true);
            if (!val) box.toWindow(true);
        },
        'mini': function (box, val, old) {
            if (val == old) return;
            if (val) box.toMinimize(true);
            if (!val) box.toWindow(true);
        },
        'min': function (box, val, old) {
            box._builder.buttonbox(box);
            var menubtn = box.dom.find('dropmenu menu_min');
            menubtn.attr('class', val ? 'enable' : 'disable');
        },
        'max': function (box, val, old) {
            box._builder.buttonbox(box);
            var menubtn = box.dom.find('dropmenu menu_max');
            menubtn.attr('class', val ? 'enable' : 'disable');
        },
        'print': function (box, val, old) {
            var menubtn = box.dom.find('dropmenu menu_print');
            menubtn.attr('class', val ? 'enable' : 'disable');
        },
        'close': function (box, val, old) {
            box._builder.buttonbox(box);
            //左上角菜单的关闭按钮
            var menubtn = box.dom.find('dropmenu menu_close');
            menubtn.attr('class', val ? 'enable' : 'disable');
            //最小化区域中的关闭按钮         
            var minbtn = $dom('pagebox-min[boxid=\'' + box.id + '\'] btn_close');
            minbtn.attr('class', val ? 'enable' : 'disable');
        },
        'resize': function (box, val, old) {
            box.dom.find('margin *').each(function () {
                $dom(this).css({
                    'cursor': val ? this.tagName + '-resize' : 'default'
                });
            });
        },
        'fresh': function (box, val, old) {
            var menubtn = box.dom.find('dropmenu menu_fresh');
            menubtn.attr('class', val ? 'enable' : 'disable');
        },
        //下拉菜单是否显示
        'dropmenu': function (obj, val, old) {
            if (obj.dom) {
                if (val) obj.domdrop.show();
                if (!val) obj.domdrop.hide();
            }
        }
    };
    //一些常用方法
    fn._method = {
        //计算尺寸
        //size:width，height
        calcSize: function (val, size) {
            var newval = Number(val);
            if (isNaN(newval)) {
                //如果是百分比
                if (String(val).charAt(String(val).length - 1) == '%') {
                    if (size == 'width') newval = Math.floor(window.innerWidth * parseInt(val) / 100);
                    if (size == 'height') newval = Math.floor(window.innerHeight * parseInt(val) / 100);
                }
                //如果是像素                
                if (String(val).substring(String(val).length - 2) == 'px') {
                    newval = parseInt(val);
                }
            }
            return newval;
        }
    };
    //打开pagebox窗体，并触发shown事件 
    fn.open = function () {
        if (!this._isinit) this._initialization();
        //如果窗体已经存在
        var ctrl = $ctrls.get(this.id);
        if (ctrl != null && ctrl.dom != null) return ctrl.obj.focus();
        //创建窗体
        for (var t in this._builder) this._builder[t](this);
        //添加事件（基础事件，例如移动、拖放等，并不包括自定义事件）
        var boxele = document.querySelector('.pagebox[boxid=\'' + this.id + '\']');
        for (var t in this._baseEvents) this._baseEvents[t](boxele);
        //构建最小化的区域
        var area = $dom('pagebox-minarea');
        if (area.length < 1) {
            area = $dom('body').add('pagebox-minarea');
            area.css('opacity', 0).hide();
        }

        for (var t in this._builder_min) this._builder_min[t](this, area);
        //更新dom
        //this.dom = $dom(boxele);
        $ctrls.update({
            id: this.id,
            dom: $dom(boxele)
        });
        //设置层深
        var maxlevel = $dom('.pagebox').level();
        this.level = maxlevel < 1 ? this.initLvl : maxlevel + 2;
        //设置一些初始值
        this.min = this._min;
        this.max = this._max;
        this.print = this._print;
        this.close = this._close;
        this.resize = this._resize;
        this.width = this._width;
        this.height = this._height;
        this.left = this._left;
        this.top = this._top;
        this.trigger('shown');
        //生成遮罩
        if (this.showmask) this.showBgMark();
        return this.focus();
    };
    //构建pagebox窗体
    fn._builder = {
        //生成外壳
        shell: function (obj) {
            var div = $dom(document.body).add('div');
            div.attr({
                'boxid': obj.id,
                'class': 'pagebox',
                'pid': obj.pid
            });
            obj.dom = div;
        },
        //边缘部分，主要是用于控制缩放
        margin: function (obj) {
            var margin = obj.dom.add('margin');
            var arr = ['nw', 'w', 'sw', 'n', 's', 'ne', 'e', 'se'];
            for (var i = 0; i < arr.length; i++)
                margin.append(arr[i]);
        },
        //标题栏，包括图标、标题文字、关闭按钮，有拖放功能
        title: function (obj) {
            //图标和标题文字
            var title = obj.dom.add('pagebox_title');
            title.add('pb-ico').html('&#x' + obj.ico);
            if (obj.url != '') {
                title.find('pb-ico').hide();
                title.add('pb-ico').addClass('pb-loading').html('&#xe621');
            }
            title.add('pb-text').html(obj.title);
            //移动窗体的响应条
            obj.dom.append('pagebox_dragbar');
        },
        //右上角的最小化，最大化，关闭按钮
        buttonbox: function (obj) {
            var btnbox = obj.dom.find('btnbox');
            if (btnbox.length < 1) btnbox = obj.dom.add('btnbox');
            btnbox.childs().remove();
            //标题文本的宽度值（是负值，100%要减去的值）
            var pbtext_width = 25;
            //如果最小化或最大化，有任意一个允许，则两个都显示
            if (obj._min || obj._max) {
                btnbox.append('btn_min').append('btn_max');
                if (!obj._min) btnbox.find('btn_min').addClass('btndisable');
                if (!obj._max) btnbox.find('btn_max').addClass('btndisable');
                obj._baseEvents.min_max(obj.dom[0]);
                pbtext_width += $dom('btn_min').width();
                pbtext_width += $dom('btn_max').width();
            }
            if (obj._close) {
                btnbox.append('btn_close');
                obj._baseEvents.close(obj.dom[0]);
                pbtext_width += $dom('btn_close').width();
            }
            //设置标题的宽度
            obj.dom.find('pb-text').width('calc(100% - ' + pbtext_width + 'px)');
        },
        //主体内容区
        body: function (obj) {
            var iframe = $dom(document.createElement('iframe'));
            iframe.attr({
                'name': obj.id,
                'id': obj.id,
                'frameborder': 0,
                'border': 0,
                'marginwidth': 0,
                'marginheight': 0,
                'src': obj.url
            });
            obj.dom.append(iframe[0]);
        },
        //左上角图标的下拉菜单
        dropmenu: function (obj) {
            var menu = obj.dom.add('dropmenu');
            menu.addClass('ui_menu');
            menu.add('menu_fresh').html('刷新');
            menu.add('menu_print').html('打印');
            menu.append('hr');
            menu.add('menu_min').html('最小化');
            menu.add('menu_max').html('最大化');
            menu.add('menu_win').html('还原');
            menu.add('hr');
            menu.add('menu_close').html('关闭');
            obj.domdrop = menu;
        },
        //内部遮罩
        mask: function (obj) {
            obj.dom.append('pagebox_mask');
        }
    };
    //添加pagebox自身事件，例如拖放、缩放、关闭等
    fn._baseEvents = {
        click: function (elem) {
            //窗体点击事件，主要是为了设置焦点
            $dom(elem).click(function (e) {
                var obj = box._getObj(e);
                obj.focus().trigger('click');
                obj.dropmenu = false;
            });
        },
        load: function (elem) {
            var boxid = elem.getAttribute('boxid');
            var ctrl = $ctrls.get(boxid);

            var src = $dom(elem).find('iframe').attr('src');
            //如果iframe路径为空，则直接算加载完成
            if (src == '') return ctrl.obj.trigger('load', { url: '', target: null });
            //iframe路径如果不是同域的，直接设置为加载完成，并触发加载完成事件（其实可能并没有加载完成）
            if ((src.length >= 'http://'.length && src.substring(0, 7).toLowerCase() == 'http://')
                || (src.length >= 'https://'.length && src.substring(0, 8).toLowerCase() == 'https://')
                || (src.length >= '//'.length && src.substring(0, 2).toLowerCase() == '//')
                && !(() => {
                    const origin = window.location.origin.toLowerCase();
                    if (src.length < origin) return false;
                    return src.toLowerCase().substring(0, origin.length) == origin;
                })()
            ) {
                $dom(elem).find('pb-ico').last().hide();
                $dom(elem).find('pb-ico').first().show();
                return ctrl.obj.trigger('load', { url: '', target: null });
            }
            //同域路径才会启用预载状态判断
            $dom(elem).find('iframe').bind('load', function (e) {
                var obj = box._getObj(e);
                var eventArgs = {
                    url: obj.url,
                    target: obj.document()
                };
                if (obj.events('fail').length > 0) {
                    try {
                        var ifDoc = obj.dom.find('iframe')[0].contentWindow.document;
                        var ifTitle = ifDoc.title;
                        if (ifTitle.indexOf("404") >= 0 || ifTitle.indexOf("错误") >= 0) {
                            //加载失败的事件
                            obj.trigger('fail', eventArgs);
                        }
                    } catch (e) {
                        var msg = '当iframe的src与当前页面不同源时，无法触发onfail事件';
                        console.log('pagebox onfail event error : ' + msg + '，' + e.message);
                    }
                }
                //加载完成的事件，不管是否失败
                obj.trigger('load', eventArgs);
                //操作图标
                obj.dom.find('pb-ico').last().hide();
                obj.dom.find('pb-ico').first().show();
            });

        },
        //拖动事件的起始，当鼠标点下时
        drag: function (elem) {
            var boxdom = $dom(elem);
            var dragbar = boxdom.find('pagebox_dragbar');
            dragbar = dragbar.merge(boxdom.find('margin>*'));
            dragbar.mousedown(function (e) {
                //鼠标点中的对象
                var node = e.target ? e.target : e.srcElement;
                var tagname = node.tagName.toLowerCase(); //点中的节点名称                   
                //获取窗体对象
                while (!node.getAttribute('boxid')) node = node.parentNode;
                var ctrl = $ctrls.get(node.getAttribute('boxid'));
                //记录当鼠标点下时的数据
                ctrl.mousedown = {
                    target: tagname, //鼠标点下时的Html元素
                    mouse: $dom.mouse(e), //鼠标坐标：x，y值
                    offset: ctrl.dom.offset(), //窗体位置：left,top
                    width: ctrl.dom.width(), //窗体宽高
                    height: ctrl.dom.height()
                }
                ctrl.dom.addClass('pagebox_drag');
                //设置当前窗体为焦点窗
                box.focus(ctrl.id);
            });
        },
        //关闭，最大化，最小化
        button: function (elem) {
            var boxdom = $dom(elem);
            //双击左侧图标关闭
            boxdom.find('pagebox_title pb-ico').dblclick(function (e) {
                var obj = box._getObj(e);
                if (obj.close) obj.shut();
            });
            //双击标题栏，最大化或还原
            boxdom.find('pagebox_dragbar').dblclick(function (e) {
                var obj = box._getObj(e);
                if (obj.max) obj.full = !obj.full;
            });
        },
        min_max: function (elem) {
            //最大化或还原
            $dom(elem).find('btnbox btn_max').click(function (e) {
                var obj = box._getObj(e);
                obj.full = !obj.full;
            });
            $dom(elem).find('btnbox btn_min').click(function (e) {
                var obj = box._getObj(e);
                if (obj.min) obj.mini = true;
            });
        },
        close: function (elem) {
            //关闭窗体，点击右上角关闭按钮
            $dom(elem).find('btnbox btn_close').click(function (e) {
                box._getObj(e).shut();
            });
        },
        //左上角下拉菜单
        dropmenu: function (elem) {
            var boxdom = $dom(elem);
            //点击左上角图标，显示下拉菜单
            boxdom.find('pagebox_title pb-ico').click(function (e) {
                var obj = box._getObj(e);
                obj.dropmenu = true;
                obj.domdrop.top(29).left(6);
            });
            //标题栏右键，显示下拉菜单
            boxdom.find('pagebox_dragbar').bind('contextmenu', function (e) {
                var obj = box._getObj(e);
                obj.dropmenu = true;
                var mouse = $dom.mouse(e);
                var offset = obj.dom.offset();
                obj.domdrop.top(mouse.y - offset.top - 5).left(mouse.x - offset.left - 5);
            });
            boxdom.find('dropmenu').bind('mouseover', function (e) {
                box._getObj(e).dropmenu = true;
            });
            boxdom.find('dropmenu').bind('mouseleave', function (e) {
                var obj = box._getObj(e);
                obj._dropmenu = false;
                window.setTimeout(function () {
                    if (!obj._dropmenu) obj.dropmenu = false;
                }, 500);
            });
            //下拉菜单中各项事件
            boxdom.find('dropmenu>*').click(function (e) {
                //识别按钮，获取事件动作             
                var node = e.target ? e.target : e.srcElement;
                if (node.tagName.indexOf('_') < 0) return;
                var action = node.tagName.substring(node.tagName.indexOf('_') + 1).toLowerCase();
                //当前pagebox.js对象
                var obj = box._getObj(e);
                obj.dropmenu = false;
                //最大化
                if (action == 'max' && !obj.full) obj.full = true;
                //最小化
                if (action == 'min' && obj.min) obj.mini = true;
                //还原，从最大化还原
                if (action == 'win') obj.full = false;
                //刷新
                if (action == 'fresh') obj.url = obj._url;
                //打印
                if (action == 'print') obj.doPrint();
                //关闭
                if (action == 'close' && obj.close) obj.shut();
            });
        }
    };
    //构建最小化的状态，每一个窗体都会有，不管是否最小化
    fn._builder_min = {
        //target:pagebox对象
        //area:最小化的管理区域
        shell: function (target, area) {
            var min = area.add('pagebox-min');
            min.attr({
                'boxid': target.id
            });
            box.pageboxcollect_boxsize();
            target.domin = min;
        },
        title: function (target, area) {
            var min = area.find('pagebox-min[boxid=\'' + target.id + '\']');
            min.attr('title', target.title);
            //图标和标题文字           
            min.add('pb-ico').html('&#x' + target.ico);
            min.add('pb-text').html(target.title);
            min.find('pb-ico,pb-text').click(function (e) {
                var obj = box._getObj(e);
                //如果窗体不处于焦点，则设置为焦点；如果已经是焦点，则最小化
                if (!obj.mini) {
                    if (!obj.dom.hasClass('pagebox_focus')) obj.focus();
                    else
                        obj.mini = true;
                } else {
                    obj.mini = false;
                    box.focus(obj.id);
                }
            });
            //关闭窗体
            min.add('btn_close').click(function (e) {
                box._getObj(e).shut();
            });
        }
    }
    //窗体中的iframe文档对象
    fn.document = function () {
        if (this.dom) {
            var iframe = this.dom.find('iframe');
            return iframe[0].contentWindow;
        }
        return null;
    };
    //获取所有子级窗体
    fn.getChilds = function () {
        var arr = gchild(this);
        //按层深level排序
        for (var j = 0; j < arr.length - 1; j++) {
            for (var i = 0; i < arr.length - 1; i++) {
                if (arr[i].level > arr[i + 1].level) {
                    var temp = arr[i];
                    arr[i] = arr[i + 1];
                    arr[i + 1] = temp;
                }
            }
        }

        function gchild(box) {
            var arr = new Array();
            for (var i = 0; i < box.childs.length; i++) {
                var c = box.childs[i];
                arr.push(c);
                if (c.childs.length > 0) {
                    var tm = gchild(c);
                    for (var j = 0; j < tm.length; j++) arr.push(tm[j]);
                }
            }
            return arr;
        }
        return arr;
    };
    //设置当前窗体为焦点
    fn.focus = function () {
        return box.focus(this.id);
    };
    //关闭窗体
    fn.shut = function () {
        box.shut(this.id);
        return this;
    };
    //打印窗体内容
    fn.doPrint = function () {
        if (window.frames[this.id] == null) {
            this.document().print();
        } else {
            window.frames[this.id].focus();
            window.frames[this.id].print();
        }
    };
    //smooth:是否平滑过渡
    fn.toFull = function (smooth) {
        return box.toFull(this.id, smooth);
    };
    fn.toWindow = function (smooth) {
        return box.toWindow(this.id, smooth);
    };
    fn.toMinimize = function (smooth) {
        return box.toMinimize(this.id, smooth);
    };
    //显示背景的遮罩
    fn.showBgMark = function () {
        box.mask.show(this);
    };
    //隐藏背景的遮罩
    fn.hideBgMask = function () {
        if (!this.showmask)
            box.mask.hide();
    };
    /*** 
    以下是静态方法或属性
    *****/
    //当前所有窗体的总数
    box.total = function (num) {
        let total = !window.pagebox_total ? 0 : window.pagebox_total;
        if (num == null || Number.isNaN(num)) return total;
        total += num;
        window.pagebox_total = total;
        //设置窗体集中管理处的窗体数量
        let collect = $dom(window.$pageboxcollect + '>div');
        if (total > 0)
            collect.attr('total', total);
        else
            collect.removeAttr('total');
        return total;
    };
    //创建一个窗体对象
    box.create = function (param) {
        if (param == null) param = {};
        //如果窗体已经存在
        if (param.id) {
            var ctrl = $ctrls.get(param.id);
            if (ctrl != null && ctrl.dom != null)
                return ctrl.obj.focus().toWindow();
        }
        if (typeof (param.pid) == 'undefined') param.pid = window.name;
        var pbox = new box(param);
        pbox._initialization();
        //加载后，禁用右键
        pbox.onload(function (s, e) {
            var doc = s.document();
            doc.document.oncontextmenu = function () {
                return false
            };
        });
        pbox.onshown(function (s, e) {
            if (s.full) s.toFull(true);
        });
        return pbox;
    };
    //创建窗体对象并打开
    box.open = function (param) {
        var pbox = box.create(param);
        return pbox.open();
    };
    //获取上级窗体对象
    box.parent = function (boxid) {
        var ctrl = $ctrls.get(boxid);
        return ctrl.obj.parent;
    };
    //所有窗体
    box.all = function () {
        var boxs = $ctrls.all('pagebox');
        var arr = new Array();
        for (var i = 0; i < boxs.length; i++) {
            arr.push(boxs[i].obj);
        }
        return arr;
    };
    //获取最顶级窗体
    box.gettop = function () {
        let boxs = this.all();
        let box = null;
        for (var i = 0; i < boxs.length; i++) {
            if (boxs[i].mini) continue;     //最小化的窗体不计算在内
            if (boxs[i].level == null) continue;
            if (box == null || boxs[i].level > box.level)
                box = boxs[i];
        }
        return box;
    };
    //获取已经存在窗体对象
    box.get = function (id) {
        var ctrl = $ctrls.get(id);
        if (ctrl) return ctrl.obj;
        return null;
    };
    //用于事件中，取点击的pagebox的对象
    box._getObj = function (e) {
        var node = e.target ? e.target : e.srcElement;
        while (!node.getAttribute('boxid')) node = node.parentNode;
        var ctrl = $ctrls.get(node.getAttribute('boxid'));
        return ctrl.obj;
    };
    //设置某个窗体为焦点
    box.focus = function (boxid) {
        var ctrl = $ctrls.get(boxid);
        if (ctrl == null) return;
        if (!ctrl.dom.hasClass('pagebox_focus')) {
            //之前的焦点窗体，触发失去焦点事件
            var focusbox = $dom('.pagebox_focus');
            if (focusbox.length > 0 && focusbox.attr('boxid') != boxid) {
                var ctr = $ctrls.get(focusbox.attr('boxid'));
                ctr.obj.trigger('blur');
            }
            var boxs = $dom('.pagebox');
            boxs.removeClass('pagebox_focus');
            ctrl.dom.addClass('pagebox_focus');
            var level = boxs.level();
            ctrl.obj.level = level < 1 ? obj.initLvl : level + 2;
            //如果是最大化，则子窗体要浮于上面
            if (ctrl.obj.full) {
                var childs = ctrl.obj.getChilds();
                for (var i = 0; i < childs.length; i++) {
                    childs[i].level = childs[i].level - ctrl.obj.initLvl + ctrl.obj.level;
                }
            }
            //激活当前窗体的焦点事件
            ctrl.obj.trigger('focus');
        }
        return ctrl.obj;
    };
    //关闭窗体
    box.shut = function (boxid) {
        var ctrl = $ctrls.get(boxid);
        if (!ctrl) return;
        //触发关闭事件,如果返回false,则不再关闭
        var t = ctrl.obj.trigger('shut');
        if (!t) return;
        //执行关闭窗体的一系列代码
        ctrl.dom.css('transition', 'opacity 0.3s');
        ctrl.dom.css('opacity', 0);
        ctrl.obj.domin.css('opacity', 0);
        box.mask.hide();
        setTimeout(function () {
            ctrl.remove();
            ctrl.obj.domin.remove();
            //如果存在父级窗体
            if (ctrl.obj.parent && $dom('.pagebox[boxid=\'' + ctrl.obj.parent.id + '\']').length > 0) {
                //父级的子级，即兄弟
                var siblings = ctrl.obj.parent.childs;
                for (let i = 0; i < siblings.length; i++) {
                    if (siblings[i].id == ctrl.obj.id) {
                        ctrl.obj.parent.childs.splice(i, 1);
                    }
                }
                ctrl.obj.parent.focus();
            } else {
                let last = $dom('.pagebox').last();
                if (last != null) box.focus(last.attr('boxid'));
            }
            //子级
            var childs = ctrl.obj.getChilds();
            for (let i = 0; i < childs.length; i++) {
                box.shut(childs[i].id);
            }
            box.total(-1);
            box.pageboxcollect_boxsize();
        }, 300);
    };
    //最大化
    box.toFull = function (boxid, smooth) {
        var ctrl = $ctrls.get(boxid);
        if (!ctrl.obj.max) return ctrl.obj;
        if (!ctrl.obj.trigger('full')) return ctrl.obj;
        //记录放大前的数据，用于还原
        ctrl.win_offset = ctrl.dom.offset();
        ctrl.win_size = {
            width: ctrl.obj.width,
            height: ctrl.obj.height
        };
        ctrl.win_state = {
            move: ctrl.obj.move,
            resize: ctrl.obj.resize
        };
        ctrl.obj.move = ctrl.obj.resize = false;
        //开始全屏放大      
        if (smooth) ctrl.dom.css('transition', 'width 0.3s,height 0.3s,left 0.3s,top 0.3s');
        ctrl.dom.addClass('pagebox_full');
        ctrl.obj.width = window.innerWidth - 3;
        ctrl.obj.height = window.innerHeight - 2;
        ctrl.obj.left = 1;
        ctrl.obj.top = 0;
        ctrl.obj.resize = false;
        ctrl.obj._full = true;
        //如果是最大化，则子窗体要浮于上面      
        var childs = ctrl.obj.getChilds();
        for (var i = 0; i < childs.length; i++) {
            childs[i].level = childs[i].level - ctrl.obj.initLvl + ctrl.obj.level;
        }
        return ctrl.obj;
    };
    //最小化
    box.toMinimize = function (boxid, smooth) {
        var ctrl = $ctrls.get(boxid);
        if (!ctrl.obj.min) return ctrl.obj;
        ctrl.obj._mini = true;
        if (!ctrl.obj.trigger('mini')) return ctrl.obj;
        //记录之前的数据，用于还原
        if (!ctrl.obj.full && ctrl.win_size == null) {
            ctrl.win_offset = ctrl.obj.dom.offset();
            ctrl.win_size = {
                width: ctrl.obj.width,
                height: ctrl.obj.height
            };
            ctrl.win_state = {
                move: ctrl.obj.move,
                resize: ctrl.obj.resize
            };
        }
        if (ctrl.obj.full) {
            ctrl.obj._full = false;
            ctrl.dom.removeClass('pagebox_full');
        }
        var obj = ctrl.obj;
        if (smooth) obj.dom.css('transition', 'width 0.3s,height 0.3s,left 0.3s,top 0.3s');
        obj.dom.addClass('pagebox_min');
        //最小化后的所在区域
        var collect = $dom('.pagebox-collect');
        var offset = collect.offset();
        obj.left = offset.left + collect.width() / 3;
        obj.top = offset.top + collect.height() / 3;
        obj.width = 0;
        obj.height = 0;
        window.setTimeout(function () {
            obj.dom.hide();
            //obj.dom.css('opacity', 0);
            collect.addClass('pagebox-collect-action');
            window.setTimeout(function () {
                collect.removeClass('pagebox-collect-action');
            }, 150);

        }, 300);
        return ctrl.obj;
    };
    //恢复窗体状态
    box.toWindow = function (boxid, smooth) {
        var ctrl = $ctrls.get(boxid);
        if (ctrl == null) return;
        if (!(ctrl.dom.hasClass('pagebox_full') || ctrl.dom.hasClass('pagebox_min'))) return ctrl.obj;
        if (!smooth) ctrl.dom.css('transition', '');
        //从最大化还原
        if (ctrl.dom.hasClass('pagebox_full')) {
            ctrl.dom.removeClass('pagebox_full');
            ctrl.obj.trigger('restore', {
                'action': 'from-full'
            });
            ctrl.obj.level = $dom('.pagebox').level() + 2;
            ctrl.obj.resize = true;
            ctrl.obj._full = false;
        } else {
            //从最小化还原
            ctrl.dom.removeClass('pagebox_min');
            ctrl.dom.show();
            ctrl.obj.trigger('restore', {
                'action': 'from-min'
            });
            ctrl.obj._mini = false;
        }
        window.setTimeout(function () {
            //ctrl.dom.css('opacity', 1);
            ctrl.obj.left = ctrl.win_offset.left;
            ctrl.obj.top = ctrl.win_offset.top;
            ctrl.obj.width = ctrl.win_size.width;
            ctrl.obj.height = ctrl.win_size.height;
            ctrl.obj.move = ctrl.win_state.move;
            ctrl.obj.resize = ctrl.win_state.resize;
            window.setTimeout(function () {
                ctrl.dom.css('transition', '');

            }, 300);
        }, 10);
        return ctrl.obj;
    };
    //拖动窗体所需的事件
    box.dragRealize = function () {
        document.addEventListener('mousemove', function (e) {
            var node = e.target ? e.target : e.srcElement;
            var boxdom = $dom('div.pagebox_drag');
            if (boxdom.length < 1) return;
            var ctrl = $ctrls.get(boxdom.attr('boxid'));
            var box = ctrl.obj;
            //当鼠标点下时的历史信息，例如位置、宽高    
            var ago = ctrl.mousedown;
            //获取移动距离
            var mouse = $dom.mouse(e);
            mouse.x = mouse.x < 0 ? 0 : (mouse.x > window.innerWidth ? window.innerWidth : mouse.x);
            mouse.y = mouse.y < 0 ? 0 : (mouse.y > window.innerHeight ? window.innerHeight : mouse.y);
            //事件参数
            var eargs = {
                'mouse': mouse,
                'move': {
                    x: mouse.x - ago.mouse.x,
                    y: mouse.y - ago.mouse.y
                },
                target: node
            };
            //移动窗体   
            if (ago.target == 'pagebox_dragbar') {
                if (box.move) {
                    ctrl.obj.showBgMark();
                    box.left = ago.offset.left + eargs.move.x;
                    box.top = ago.offset.top + eargs.move.y;
                    ctrl.win_offset = ctrl.obj.dom.offset();
                    //触发拖动事件
                    eargs.offset = ctrl.dom.offset();
                    box.trigger('drag', eargs);
                }
            } else {
                //缩放窗体
                if (box.resize) {
                    ctrl.obj.showBgMark();
                    var minWidth = 200,
                        minHeight = 150;
                    if (ctrl.dom.attr('resize') != 'false') {
                        if (ago.target.indexOf('e') > -1) box.width = ago.width + eargs.move.x < minWidth ? minWidth : ago.width + eargs.move.x;
                        if (ago.target.indexOf('s') > -1) box.height = ago.height + eargs.move.y < minHeight ? minHeight : ago.height + eargs.move.y;
                        if (ago.target.indexOf('w') > -1) {
                            box.width = ago.width - eargs.move.x < minWidth ? minWidth : ago.width - eargs.move.x;
                            if (box.width > minWidth) box.left = ago.offset.left + eargs.move.x;
                        }
                        if (ago.target.indexOf('n') > -1) {
                            box.height = ago.height - eargs.move.y < minHeight ? minHeight : ago.height - eargs.move.y;
                            if (box.height > minHeight) box.top = ago.offset.top + eargs.move.y;
                        }
                        //触发resize事件                     
                        eargs.offset = ctrl.dom.offset();
                        eargs.width = box.width;
                        eargs.height = box.height;
                        eargs.action = eargs.target.tagName;
                        ctrl.obj.trigger('resize', eargs);
                    }
                }
            }
            //
        });
        document.addEventListener('mouseup', function (e) {
            //var mouse = $dom.mouse(e);
            $ctrls.removeAttr('mousedown');
            var page = $dom('.pagebox_focus');
            page.removeClass('pagebox_drag');
            var obj = box.get(page.attr("boxid"));
            if (obj != null)
                obj.hideBgMask();
        });
        window.addEventListener('blur', function (e) {
            //document.onmouseup();
        });
        window.addEventListener('resize', function (e) {
            $dom('div.pagebox_full')
                .width(window.innerWidth - 3).height(innerHeight - 2)
                .left(1).top(0);
            box.mask.resize();
        });
        document.addEventListener('mousedown', function (e) {
            //如果点在最小化管理区，则不隐藏最小管理面板
            var node = e.target ? e.target : e.srcElement;
            var tagname = '';
            do {
                tagname = node.tagName ? node.tagName.toLowerCase() : '';
                node = node.parentNode;
            } while (!(tagname == 'pagebox-minarea' || tagname == 'html'));
            if (tagname != 'pagebox-minarea') {
                $dom('pagebox-minarea').css('opacity', 0);
                window.setTimeout(function () {
                    var area = $dom('pagebox-minarea');
                    if (parseInt(area.css('opacity')) == 0) {
                        $dom('pagebox-minarea').hide();
                        $dom('.pagebox-collect').attr('state', 'close');
                    }
                }, 300);
            }
        });
    };
    /* 最小化的所在区域的管理 */
    //最小化管理区的盒子图标
    box.pageboxcollect = function () {
        window.addEventListener('load', function (e) {
            let collect = $dom(window.$pageboxcollect);
            collect.add('div').attr('title', '窗体管理');
            let area = $dom('pagebox-minarea');
            if (area.length < 1) area = $dom('body').add('pagebox-minarea');
            //窗体管理的点击事件
            collect.addClass('pagebox-collect').click(function () {
                let state = collect.attr('state');
                collect.attr('state', state == 'open' ? 'close' : 'open');
                if (collect.attr('state') == 'open')
                    box.pageboxcollect_boxcreate();
            });
            //窗体集中管理中的右键菜单
            collect.merge($dom('pagebox-minarea')).bind('contextmenu', function (e) {
                let menu = box.pageboxcollect_contextmenu($dom.mouse(e));
            });
        });

    };
    //窗体集中管理区的右键菜单
    box.pageboxcollect_contextmenu = function (mouse) {
        var menu = $dom('pageboxcollect_contextmenu');
        if (menu.length < 1) {
            menu = $dom('body').add('pageboxcollect_contextmenu');
            menu.addClass('ui_menu');
            menu.css('position', 'absolute');
            menu.add('menu_all_towindow').html('全部还原');
            menu.add('menu_all_min').html('全部最小化');
            menu.add('hr');
            menu.add('menu_all_close').html('全部关闭');
            //菜单事件
            menu.childs().click(function (e) {
                //识别按钮，获取事件动作             
                var node = e.target ? e.target : e.srcElement;
                if (node.tagName.indexOf('_') < 0) return;
                var action = node.tagName.substring(node.tagName.lastIndexOf('_') + 1).toLowerCase();
                $dom('pageboxcollect_contextmenu').hide();
                var boxs = box.all();
                for (var i = 0; i < boxs.length; i++) {
                    //还原
                    if (action == 'towindow') {
                        if (boxs[i].mini) boxs[i].mini = false;
                        if (boxs[i].full) boxs[i].full = false;
                    }
                    //最小化
                    if (action == 'min') boxs[i].mini = true;
                    //关闭
                    if (action == 'close') boxs[i].shut();
                }
            });

            menu.bind('mouseleave', function (e) {
                $dom('pageboxcollect_contextmenu').hide();
            });
        }
        menu.show();
        var maxwd = window.innerWidth;
        var maxhg = window.innerHeight;
        var left = mouse.x + menu.width() > maxwd ? mouse.x - menu.width() + 5 : mouse.x - 5;
        var top = mouse.y + menu.height() > maxhg ? mouse.y - menu.height() + 5 : mouse.y - 5;
        menu.left(left).top(top);
        return menu;
    };
    //生成窗体最小化的管理区
    box.pageboxcollect_boxcreate = function () {
        var area = $dom('pagebox-minarea');
        if (area.length < 1) area = $dom('body').add('pagebox-minarea');
        area.show().css('opacity', 1);
        //设置大小      
        box.pageboxcollect_boxsize();
        //计算最小化管理区的按钮所在方位
        var collect = $dom('.pagebox-collect');
        var offset = collect.offset();
        var region = '';
        region += box.availWidth() / 2 < offset.left + collect.width() / 2 ? 'right' : 'left';
        region += box.availHeight() / 2 < offset.top + collect.height() / 2 ? 'bottom' : 'top';
        //设置"最小化的管理区"的位置
        area.css('right', region.indexOf('right') > -1 ? (box.availWidth() - offset.left + 0) + 'px' : 'auto');
        area.css('left', region.indexOf('left') > -1 ? offset.left + collect.width() + 'px' : 'auto');
        area.css('top', region.indexOf('top') > -1 ? offset.top + 'px' : 'auto');
        area.css('bottom', region.indexOf('bottom') > -1 ? (box.availHeight() - offset.top - collect.height() * 3 / 4) + 'px' : 'auto');
    };
    //自动设置最小化管理的宽高
    box.pageboxcollect_boxsize = function () {
        var area = $dom('pagebox-minarea');
        var size = area.find('pagebox-min').length;
        if (size < 1) {
            $dom('pagebox-minarea').hide();
            $dom('.pagebox-collect').attr('state', 'close');
        }
        if (size <= 4) area.width(320 + 8).height(80 + 8);
        if (size > 4 && size <= 8) area.width(320 + 8).height(160 + 8);
        if (size > 8 && size <= 12) area.width(320 + 8).height(240 + 8);
        if (size > 12 && size <= 18) area.width(480 + 8).height(240 + 8);
        if (size > 18) area.width(480 + 8 + 20).height(240 + 8).css('overflow', 'auto');
        else
            area.css('overflow', 'hidden');
    };
    //窗体的背景遮罩层，当拖动时出现，用于当窗体处于Iframe时，鼠标一旦离开窗体会失去效果
    box.mask = {
        show: function (obj) {
            var mask = $dom('pagebox_bg_mask');
            if (mask.length < 1) mask = $dom(document.body).add('pagebox_bg_mask');
            mask.width(box.availWidth()).height(box.availHeight());
            mask.level(obj.level - 1);
            mask.show();
            $dom('body').addClass('pagebox_overflow');
        },
        hide: function () {
            var mask = $dom('pagebox_bg_mask');
            if (mask.length < 1) return;
            mask.hide();
            $dom('body').removeClass('pagebox_overflow');
        },
        resize: function () {
            var mask = $dom('pagebox_bg_mask');
            if (mask.length < 1) return;
            mask.width(box.availWidth()).height(box.availHeight());
        }
    };
    //屏幕有效宽度
    box.availWidth = function () {
        return document.documentElement.clientWidth;
        return window.screen.availWidth;
    };
    //屏幕有效高度
    box.availHeight = function () {
        return document.documentElement.clientHeight;
        return window.screen.availHeight;
    };
    //执行来源对象中的方法
    box.source = {
        //父级为选项卡时
        //name:为当前窗体的window.name
        //func:要执行的方法，必须是window下的
        //close:是否关闭当前窗体
        tab: function (name, func, close) {
            name = $dom.trim(name);
            //当前pagebox窗体对象
            var currbox = box.get(name);
            if (currbox == null) return;
            //tabs.js标签页的页面区域
            var iframe = $dom('iframe[name=\'' + currbox.pid + '\']');
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
            }
            if (close) {
                window.setTimeout(function () {
                    $pagebox.shut(name);
                }, 1000);
            }

        },
        //父级为pagebox
        box: function (name, func, close) {
            name = $dom.trim(name);
            console.log(name);
            var pbox = box.parent(name);
            if (pbox == null) return;
            var win = pbox.document();
            //tabs.js标签页的页面区域
            if (win && func != null) {
                if (func.charAt(func.length - 1) == ')') { eval('win.' + func); }
                else {
                    var f = eval('win.' + func);
                    if (f != null) f();
                }
            }
            if (close) {
                window.setTimeout(function () {
                    $pagebox.shut(name);
                }, 1000);
            }
        },
        //查找自身
        self: function (name, func, close) {
            name = $dom.trim(name);
            //当前pagebox窗体对象
            var currbox = box.get(name);
            if (close) currbox.shut();
            return currbox;
        }
    };
    win.$pagebox = box;
    win.$pagebox.dragRealize();
    win.$pagebox.pageboxcollect();
})(window);