/*!
 * 主 题：一个简化版的Jquery
 * 说 明：
 * 1、类似JQuery，主要为了方便操作DOM;
 * 2、满足大多数DOM操作，不兼容IE678;
 * 3、另外写了动态加载css和js的方法，在最后面
 *
 * 作 者：微厦科技_宋雷鸣_10522779@qq.com
 * 开发时间: 2020年1月1日
 * 最后修订：2023年12月1日
 * github开源地址:https://gitee.com/weishakeji/LearningSystem
 */
(function () {
    //html节点查询，类似jquery
    var webdom = function (query, context) {
        return new webdom.init(query, context);
    };
    webdom.version = function () {
        var template = webdom('meta[build-number]');
        return template.attr("build-number");
    };
    webdom.init = function (query, context) {
        var nodes = [];
        if (typeof (query) == 'string') nodes = (context || document).querySelectorAll(query);
        if (query instanceof Node) nodes[0] = query;
        if (query instanceof NodeList || query instanceof Array) nodes = query;
        //查询结果附加到对象自身
        this.length = nodes.length;
        for (var i = 0; i < this.length; i++) this[i] = nodes[i];
        this.typeof = 'webui.element';
        return this;
    };
    var fn = webdom.init.prototype;
    //遍历节点元素，并执行fun函数
    //ret:默认返回自身对象，如果ret有值，则返回fun函的执行结果
    fn.each = function (fun, ret) {
        var results = [],
            res;
        for (var i = 0; i < this.length; i++) {
            res = fun.call(this[i], i);
            //实现continue和break方法
            if (typeof (res) == 'boolean') {
                results.push(res);
                if (res) continue;
                if (!res) break;
            }
            if (res instanceof NodeList || res instanceof Array) {
                for (var j = 0; j < res.length; j++) {
                    if (res[j] instanceof Node) {
                        if (res[j].nodeType == 1) results.push(res[j])
                    } else {
                        results.push(res[j]);
                    }
                }
            } else {
                switch (typeof (res)) {
                    case 'string':
                        results.push(res.replace(/^\s*|\s*$/g, ""));
                        break;
                    case 'boolean':
                    case 'number':
                        results.push(res);
                        break;
                    default:
                        if (res instanceof Node) {
                            if (res.nodeType && res.nodeType == 1)
                                results.push(res);
                        } else {
                            results.push(res);
                        }
                        break;
                }
            }
        }
        if (ret) {
            if (results instanceof NodeList || results instanceof Array)
                return results.length == 1 ? results[0] : results;
        }
        return this;
    };
    fn.find = function (query) {
        var nodes = [];
        var res = this.each(function () {
            return this.querySelectorAll(query);
        }, 1);
        if (res instanceof Array) {
            for (var i = 0; i < res.length; i++)
                nodes.push(res[i]);
        } else {
            nodes = res;
        }
        return new webdom(nodes);
    };
    //获取第n个元素,如果为负，则倒序取，例如-1为最后一个
    fn.get = function (index) {
        if (arguments.length < 1 || index == 0 || typeof index !== 'number') return this;
        //if (this.length < Math.abs(index)) throw 'webdom.get error : index greater than length';
        if (this.length < Math.abs(index)) return null;
        return index > 0 ? new webdom(this[index - 1]) : new webdom(this[this.length - Math.abs(index)]);
    };
    fn.first = function () {
        return this.length > 0 ? this.get(1) : null;
    };
    fn.last = function () {
        return this.length > 0 ? this.get(-1) : null;
    };
    fn.parent = function () {
        var nodes = this.each(function () {
            var p = this.parentNode;
            while (p.nodeType != 1) p = p.previousSibling;
            return p;
        }, 1);
        return new webdom(nodes);
    };
    fn.next = function () {
        var nodes = this.each(function () {
            var cur = this.nextSibling;
            if (cur == null) return cur;
            while (cur.nodeType != 1) cur = cur.nextSibling;
            return cur;
        }, 1);
        return new webdom(nodes);
    };
    fn.prev = function () {
        var nodes = this.each(function () {
            var p = this.previousSibling;
            if (p == null) return p;
            while (p.nodeType != 1) p = p.previousSibling;
            return p;
        }, 1);
        return new webdom(nodes);
    };
    fn.siblings = function (query) {
        return this.parent().childs(query);
    };
    fn.childs = function (query) {
        var nodes = this.each(function () {
            if (query == null) return this.childNodes;
            var chs = this.childNodes
            var tm = new Array();
            for (var i = 0; i < chs.length; i++) {
                if (chs[i].tagName.toLowerCase() == query.toLowerCase())
                    tm.push(chs[i]);
            }
            return tm;
        }, 1);
        return new webdom(nodes);
    }
    fn.hide = function () {
        return this.each(function () {
            this.style.display = "none";
        });
    };
    fn.show = function (display) {
        if (display == null) display = 'block';
        return this.each(function () {
            this.style.display = display;
            /*
            if (this.style.display == 'none')
                this.style.removeProperty('display');
                */
        });
    };
    fn.toggle = function () {
        this.each(function () {
            var styles = document.defaultView.getComputedStyle(this, null);
            var display = styles.getPropertyValue('display');
            this.style.setProperty('display', display == 'none' ? 'block' : 'none', 'important');
            //this.css('display',display=='' ? 'none' : '','important');
        });
    };
    fn.text = function (str) {
        if (str != undefined) {
            return this.each(function () {
                this.innerText = str;
            });
        } else {
            return this.each(function () {
                var text = this.innerText ? this.innerText : this.textContent;
                if (text == null || text == "") return "";
                return text.replace(/(^\s*)|(\s*$)/g, "");
            }, 1);
        }
    };
    fn.html = function (str) {
        if (str != undefined) {
            return this.each(function () {
                this.innerHTML = str;
            });
        } else {
            return this.each(function () {
                return this.innerHTML;
            }, 1);
        }
    };
    fn.outHtml = function (str) {
        if (str != undefined) {
            return this.each(function () {
                this.outerHTML = str;
            });
        } else {
            return this.each(function () {
                return this.outerHTML;
            }, 1);
        }
    };
    fn.val = function (str) {
        if (str != undefined) {
            return this.each(function () {
                this.value = str;
            });
        } else {
            return this.each(function () {
                if (this.getAttribute('type') == 'checkbox') return this.checked;
                if (this.getAttribute('type') == 'radio') return this.checked;
                return this.value;
            }, 1);
        }
    };
    fn.focus = function () {
        this.each(function () {
            return this.focus();
        });
        return this;
    };
    //设置或获取属性
    //arguments:
    fn.attr = function () {
        var len = arguments.length;
        if (len < 1) return this;
        //如果只有一个参数
        if (len == 1) {
            var key = arguments[0];
            if (typeof (key) == 'string') {
                return this.each(function (index) {
                    return this.getAttribute(key);
                }, 1);
            }
            //批量设置属性
            if (typeof (key) == 'object') {
                for (var k in key) this.attr(k, key[k]);
                return this;
            }
        }
        //两个参数，则一个为key，一个为value
        if (len >= 2) {
            var key = arguments[0];
            var val = arguments[1];
            return this.each(function (index) {
                this.setAttribute(key, val);
            });
        }
    }
    //移除属性
    fn.removeAttr = function (key) {
        return this.each(function () {
            this.removeAttribute(key);
        });
    };
    //设置css样式
    fn.css = function (key, value, important) {
        if (typeof (key) == 'object') {
            for (var k in key) this.css(k, key[k]);
            return this;
        }
        if (value != undefined) {
            return this.each(function () {
                this.style.setProperty(key, value, important ? 'important' : '');
            });
        } else {
            return this.each(function () {
                var styles = document.defaultView.getComputedStyle(this, null);
                return styles.getPropertyValue(key);
            }, 1);
        }
    };
    fn.hasClass = function (str) {
        return this.each(function () {
            return this.classList.contains(str);
        }, 1);
    };
    fn.addClass = function (str) {
        return this.each(function () {
            return this.classList.add(str);
        });
    };
    fn.removeClass = function (str) {
        return this.each(function () {
            return this.classList.remove(str);
        });
    };
    fn.remove = function () {
        return this.each(function () {
            if (this.remove) this.remove();
            if (this.removeNode) this.removeNode(true);
        });
    };
    //设置或读取层深，即z-index的值
    fn.level = function (num) {
        if (arguments.length < 1) {
            var res = this.each(function () {
                return this.style.getPropertyValue("z-Index");
            }, 1);
            if (typeof (res) == 'string') return res == '' ? 0 : parseInt(res);
            var l = 0;
            for (var i = 0; i < res.length; i++) {
                var n = parseInt(res[i]);
                if (n > l) l = n;
            }
            return l;
        } else {
            this.css("z-Index", num);
        }
        return this;
    };
    fn.width = function (num) {
        if (arguments.length < 1) {
            var ele = this[0] ? this[0] : null;
            if (ele == null) return 0;
            var styles = document.defaultView.getComputedStyle(ele, null);
            var width = ele.offsetWidth;
            var attr = ['border-left-width', 'border-right-width', 'padding-left', 'padding-right'];
            for (let i = 0; i < attr.length; i++)
                width -= parseFloat(styles.getPropertyValue(attr[i]));
            return width;
        } else {
            if (typeof arguments[0] == 'number')
                return this.css('width', arguments[0] + 'px');
            if (typeof arguments[0] == 'string')
                return this.css('width', arguments[0]);
        }
    };
    fn.height = function (num) {
        if (arguments.length < 1) {
            var ele = this[0] ? this[0] : null;
            if (ele == null) return 0;
            var styles = document.defaultView.getComputedStyle(ele, null);
            var height = ele.offsetHeight;
            var attr = ['border-top-width', 'border-bottom-width', 'padding-top', 'padding-bottom'];
            for (let i = 0; i < attr.length; i++)
                height -= parseFloat(styles.getPropertyValue(attr[i]));
            return height;
        } else {
            if (typeof arguments[0] == 'number')
                return this.css('height', arguments[0] + 'px');
            if (typeof arguments[0] == 'string')
                return this.css('height', arguments[0]);
        }
    };
    fn.left = function (num) {
        if (arguments.length < 1) {
            var offset = this.offset();
            return offset.length ? offset[0].left : offset.left;
        } else {
            if (typeof arguments[0] == 'number')
                return this.css('left', arguments[0] + 'px');
        }
    };
    fn.top = function (num) {
        if (arguments.length < 1) {
            var offset = this.offset();
            return offset.length ? offset[0].top : offset.top;
        } else {
            if (typeof arguments[0] == 'number')
                return this.css('top', arguments[0] + 'px');
        }
    };
    //获取元素的坐标
    fn.offset = function () {
        var offset = {
            top: 0,
            left: 0
        };
        if (this.length < 1) return offset;
        var node = this[0];
        // 当前为IE11以下, 直接返回{top: 0, left: 0}
        if (!node.getClientRects().length) return offset;
        // 当前DOM节点的 display === 'node' 时, 直接返回{top: 0, left: 0}
        if (window.getComputedStyle(node)['display'] === 'none') return offset;
        // Element.getBoundingClientRect()方法返回元素的大小及其相对于视口的位置。
        // 返回值包含了一组用于描述边框的只读属性——left、top、right和bottom，单位为像素。除了 width 和 height 外的属性都是相对于视窗的左上角位置而言的。
        // 返回如{top: 8, right: 1432, bottom: 548, left: 8, width: 1424…}
        offset = node.getBoundingClientRect();
        var docElement = node.ownerDocument.documentElement;
        return {
            top: offset.top + window.pageYOffset - docElement.clientTop,
            left: offset.left + window.pageXOffset - docElement.clientLeft,
            width: this.width(),
            height: this.height()
        };
    };
    //追加一个子节点，返回自身对象
    fn.append = function (ele) {
        if (typeof (ele) == 'string') {
            return this.append(document.createElement(ele));
        }
        if (webdom.isdom(ele)) {
            return this.each(function () {
                for (var i = 0; i < ele.length; i++)
                    this.appendChild(webdom.clone(ele[i]));
            });
        }
        if (ele instanceof Node) {
            return this.each(function () {
                this.appendChild(ele);
            });
        }
    };
    //添加了一个子节点，返回子节点对象
    fn.add = function (ele) {
        if (typeof (ele) == 'string') {
            if (this.length == 1) return this.add(document.createElement(ele));
            if (this.length > 1) {
                var res = this.each(function () {
                    return this.appendChild(document.createElement(ele));
                }, 1);
                return webdom(res)
            }
        }
        if (webdom.isdom(ele)) {
            var nodes = this.each(function () {
                var chils = [];
                for (var i = 0; i < ele.length; i++)
                    chils.push(this.appendChild(webdom.clone(ele[i])));
                return chils;
            }, 1);
            return webdom(nodes);
        }
        if (ele instanceof Node) {
            return webdom(this.each(function () {
                this.appendChild(ele);
                return ele;
            }, 1));
        }
    };
    //合并两个对象，返回新对象
    fn.merge = function (obj) {
        var arr = new Array();
        this.each(function () {
            arr.push(this);
        });
        if (obj instanceof Node) {
            arr.push(obj);
        } else {
            if (webdom.isdom(obj)) {
                obj.each(function (index) {
                    arr.push(this);
                });
            }
        }
        return new webdom(arr);
    }
    //绑定事件
    fn.bind = function (events, func, useCapture) {
        var arr = events.split(',');
        for (var i = 0; i < arr.length; i++) {
            var event = arr[i];
            this.each(function () {
                this.addEventListener(event, func, useCapture);
                if (event == 'click') {
                    var iframe = this.querySelector('iframe');
                    if (iframe) {
                        webdom.IframeOnClick.track(iframe, function (sender, boxid) {
                            sender.click();
                        });
                    }
                }
            });
        }
        return this;
    };
    //触发事件
    fn.trigger = function (events) {
        var arr = events.split(',');
        for (var i = 0; i < arr.length; i++) {
            var event = arr[i];
            this.each(function () {
                var eObj = document.createEvent('HTMLEvents');
                eObj.initEvent(event, true, false);
                this.dispatchEvent(eObj);
            });
        }
        return this;
    };
    //若含有参数就注册事件，无参数就触发事件
    fn.click = function (f) {
        if (typeof (f) == "function") {
            this.bind('click', f, true);
        } else {
            this.trigger('click');
        }
    };
    fn.dblclick = function (f) {
        if (typeof (f) == "function") {
            this.bind('dblclick', f, true);
        } else {
            this.trigger('dblclick');
        }
        return this;
    };
    fn.mousedown = function (f) {
        if (typeof (f) == "function") {
            this.bind('mousedown', f, true);
        } else {
            this.trigger('mousedown');
        }
        return this;
    };
    /*
    静态方法
    */
    //是否是webdom对象
    webdom.isdom = function (obj) {
        return typeof (obj) == 'object' && obj.typeof == 'webui.element';
    };
    //去除两端空格
    webdom.trim = function (str) {
        return str.replace(/^\s*|\s*$/g, '').replace(/^\n+|\n+$/g, "");
    };
    //当前页面的文件名，不包括路径和后缀名
    webdom.file = function () {
        var href = window.location.href;
        if (href.substring(href.length - 1) == '/') href = href.substring(0, href.length - 1);
        if (href.indexOf('?') > 0) href = href.substring(0, href.lastIndexOf('?'));
        if (href.indexOf('#') > 0) href = href.substring(0, href.lastIndexOf('#'));
        if (href.indexOf('/') > 0) href = href.substring(href.lastIndexOf('/') + 1);
        if (href.indexOf('.') > 0) href = href.substring(0, href.lastIndexOf('.'));
        return href;
    };
    //克隆对象
    webdom.clone = function (obj) {
        if (typeof obj == "object") {
            if (obj == null) return null;
            if (webdom.isdom(obj)) {
                var t = [];
                for (var i = 0; i < obj.length; i++)
                    t.push(obj[i]);
                return webdom(t);
            } else {
                if (obj instanceof Array) {
                    var t = [];
                    for (var i = 0, len = obj.length; i < len; i++)
                        t.push(webdom.clone(obj[i]));
                    return t;
                } else {
                    if (obj instanceof Node) {
                        return obj.cloneNode(true);
                    } else {
                        var t = {};
                        for (var j in obj)
                            t[j] = webdom.clone(obj[j]);
                        return t;
                    }
                }
            }
        }
        return obj;
    };
    webdom.ajax = function (options) {
        function empty() { }

        function obj2Url(obj) {
            var arr = [];
            for (var i in obj) {
                arr.push(encodeURI(i) + '=' + encodeURI(obj[i]));
            }
            return arr.join('&').replace(/%20/g, '+');
        }
        var opt = {
            url: '', //请求地址
            sync: true, //true，异步 | false　同步，会锁死浏览器，并且open方法会报浏览器警告
            method: 'GET', //提交方法
            data: null, //提交数据
            dataType: 'json', //返回数据类型
            username: null, //账号
            password: null, //密码
            success: empty, //成功返回回调
            error: empty, //错误信息回调
            timeout: 10000 //请求超时ms
        };
        for (var t in options) opt[t] = options[t];
        //Object.assign(opt, options); //直接合并对象,opt已有属性将会被options替换
        var abortTimeout = null;
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4) {
                xhr.onreadystatechange = empty;
                clearTimeout(abortTimeout);
                if ((xhr.status >= 200 && xhr.status < 300) || xhr.status == 304) {
                    var result = xhr.responseText;
                    try {
                        if (opt.dataType == 'json') {
                            result = result.replace(' ', '') == '' ? null : JSON.parse(result);
                        }
                    } catch (e) {
                        opt.error(e, xhr);
                        xhr.abort();
                    }
                    opt.success(result, xhr);
                } else if (0 == xhr.status) {
                    opt.error("跨域请求失败", xhr);
                } else {
                    opt.error(xhr.statusText, xhr);
                }
            }
        };
        var data = opt.data ? obj2Url(opt.data) : opt.data;
        opt.method = opt.method.toUpperCase();
        if (opt.method == 'GET') {
            opt.url += (opt.url.indexOf('?') == -1 ? '?' : '&') + data;
        }
        xhr.open(opt.method, opt.url, opt.sync, opt.username, opt.password);
        if (opt.method == 'POST') {
            xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
        }
        if (opt.timeout > 0) {
            abortTimeout = setTimeout(function () {
                xhr.onreadystatechange = empty;
                xhr.abort();
                opt.error('网络请求超时', xhr);
            }, opt.timeout);
        }
        xhr.send(data);
    };
    webdom.get = function (url, func) {
        var opt = {
            url: url,
            success: func
        };
        this.ajax(opt);
    };
    //鼠标的坐标值
    webdom.mouse = function (e) {
        var x = 0,
            y = 0;
        var e = e || window.event;
        if (e.pageX || e.pageY) {
            x = e.pageX;
            y = e.pageY;
        } else if (e.clientX || e.clientY) {
            x = e.clientX;
            y = e.clientY;
        } else if (e.touches[0]) {
            x = e.touches[0].clientX;
            y = e.touches[0].clientY;
        }
        return {
            'x': x,
            'y': y
        };
    };
    //是否是手机端
    webdom.ismobi = function () {
        var regex_match = /(nokia|iphone|android|motorola|^mot-|softbank|foma|docomo|kddi|up.browser|up.link|htc|dopod|blazer|netfront|helio|hosin|huawei|novarra|CoolPad|webos|techfaith|palmsource|blackberry|alcatel|amoi|ktouch|nexian|samsung|^sam-|s[cg]h|^lge|ericsson|philips|sagem|wellcom|bunjalloo|maui|symbian|smartphone|midp|wap|phone|windows ce|iemobile|^spice|^bird|^zte-|longcos|pantech|gionee|^sie-|portalmmm|jigs browser|hiptop|^benq|haier|^lct|operas*mobi|opera*mini|320x320|240x320|176x220)/i;
        var u = navigator.userAgent;
        if (null == u) return true;
        return regex_match.exec(u) != null;
    };
    //是否是平板
    webdom.ispad = function () {
        var regex_match = /(ipad|Android.*Tablet)/i;
        var u = navigator.userAgent;
        if (null == u) return true;
        return regex_match.exec(u) != null;
    };
    //网页是否处于微信内置浏览器
    webdom.isWeixin = function () {
        var ua = window.navigator.userAgent.toLowerCase();
        return ua.match(/MicroMessenger/i) == 'micromessenger';
    };
    //网页是否处于微信小程序内置浏览器
    webdom.isWeixinApp = function () {
        var ua = window.navigator.userAgent.toLowerCase();
        return ua.match(/miniProgram/i) == 'miniprogram';
    };
    //当click事件时，如果有iframe时，添加iframe的点击事件
    webdom.IframeOnClick = {
        resolution: 10,
        iframes: [],
        interval: null,
        Iframe: function () {
            this.element = arguments[0];
            this.cb = arguments[1];
            this.hasTracked = false;
        },
        track: function (element, cb) {
            this.iframes.push(new this.Iframe(element, cb));
            if (!this.interval) {
                var _this = this;
                this.interval = setInterval(function () {
                    _this.checkClick();
                }, this.resolution);
            }
        },
        checkClick: function () {
            if (document.activeElement) {
                var activeElement = document.activeElement;
                for (var i in this.iframes) {
                    var iframe = this.iframes[i];
                    if (!(iframe && iframe.element)) continue;
                    var name = iframe.element.getAttribute('name');
                    var pagebox = document.querySelector('.pagebox[boxid=\'' + name + '\']');
                    if (activeElement === this.iframes[i].element) { // user is in this Iframe  
                        if (this.iframes[i].hasTracked == false) {
                            this.iframes[i].cb.apply(window, [pagebox, name]);
                            this.iframes[i].hasTracked = true;
                        }
                    } else {
                        this.iframes[i].hasTracked = false;
                    }
                }
            }
        }
    };
    webdom.ready = (function () { //这个函数返回whenReady()函数
        var funcs = []; //当获得事件时，要运行的函数
        var ready = false; //当触发事件处理程序时,切换为true

        //当文档就绪时,调用事件处理程序
        function handler(e) {
            if (ready) return; //确保事件处理程序只完整运行一次
            //如果发生onreadystatechange事件，但其状态不是complete的话,那么文档尚未准备好
            if (e.type === 'onreadystatechange' && document.readyState !== 'complete') {
                return;
            }
            //运行所有注册函数
            //注意每次都要计算funcs.length
            //以防这些函数的调用可能会导致注册更多的函数
            for (var i = 0; i < funcs.length; i++) {
                funcs[i].call(document);
            }
            //事件处理函数完整执行,切换ready状态, 并移除所有函数
            ready = true;
            funcs = null;
        }
        //为接收到的任何事件注册处理程序
        if (document.addEventListener) {
            document.addEventListener('DOMContentLoaded', handler, false);
            document.addEventListener('readystatechange', handler, false); //IE9+
            window.addEventListener('load', handler, false);
        } else if (document.attachEvent) {
            document.attachEvent('onreadystatechange', handler);
            window.attachEvent('onload', handler);
        }
        //返回whenReady()函数
        return function whenReady(fn) {
            if (ready) {
                fn.call(document);
            } else {
                funcs.push(fn);
            }
        }
    })();
    //加载css或js文件，并加调
    webdom.load = {
        css: function (src, callback, tagName) {
            webdom.load.arraySync(function (one, i, c) {
                //判断css文件是否存在，如果存在则不加载，主要用于组件的css加载
                var exist = false;
                webdom("link[type='text/css']").each(function () {
                    let href = webdom(this).attr("href").toLowerCase();
                    if (href.indexOf('?') > -1) href = href.substring(0, href.lastIndexOf('?'));
                    if (one.toLowerCase() == href) {
                        exist = true;
                        return false;
                    }
                });
                if (exist) return;

                let cur_script = document.createElement("link");
                cur_script.type = 'text/css';
                cur_script.rel = "stylesheet";
                one += (one.indexOf('?') > -1 ? '&' : '?') + 'ver=' + webdom.version();
                if (!webdom.iscache()) one += (one.indexOf('?') > -1 ? '&' : '?') + 'gettimer=' + new Date().getTime();
                cur_script.href = one;
                if (tagName != null)
                    cur_script.setAttribute('tag', tagName);
                cur_script.addEventListener('load', function () {
                    c(0, { i: i, v: {} });
                }, false);
                document.head.appendChild(cur_script)
            }, src, function (err, r) {
                //全部加载完成后执行的回调函数
                if (err) {
                    alert(err.message);
                } else {
                    if (callback != null) callback();
                }
            });
        },
        js: function (src, callback, tagName) {
            webdom.load.arraySync(function (one, i, c) {
                //判断js文件是否存在，如果存在则不加载
                let exist = false;
                let arr = document.querySelectorAll("script");
                for (let i = 0; i < arr.length; i++) {
                    let src = arr[i].getAttribute('src');
                    if (src == null) continue;
                    if (src.indexOf('?') > -1) src = src.substring(0, src.lastIndexOf('?'));
                    if (one.toLowerCase() == src.toLowerCase()) {
                        exist = true;
                        break;
                    }
                }
                if (exist) return;

                var cur_script = document.createElement("script");
                cur_script.type = 'text/javascript';
                one += (one.indexOf('?') > -1 ? '&' : '?') + 'ver=' + webdom.version();
                if (!webdom.iscache()) one += (one.indexOf('?') > -1 ? '&' : '?') + 'gettimer=' + new Date().getTime();
                cur_script.src = one;
                if (tagName != null)
                    cur_script.setAttribute('tag', tagName);
                cur_script.addEventListener('load', function () {
                    c(0, { i: i, v: {} });
                }, false);
                document.head.appendChild(cur_script)
            }, src, function (err, r) {
                //全部加载完成后执行的回调函数
                if (err) {
                    alert(err.message);
                } else {
                    if (callback != null) callback();
                }
            });
        },
        //处理异步，不用promise的方案
        //arr:要加载对象的url，数组类型
        //callback:全部加载完成后执行的回调函数
        arraySync: function (bsFunc, arr, callback) {
            if (arr == null || !(arr instanceof Array)) arr = [arr];    //
            if (arr.length == 0 || (arr.length == 1 && arr[0] == null)) return callback(0, []);
            var sendErr = false;
            var finishNum = arr.length;
            var result = [];
            var args = [0, 0];
            for (let i = 2; i < arguments.length - 1; ++i)
                args.push(arguments[i]);
            args.push(function (err, r) {
                if (err) {
                    if (!sendErr) {
                        sendErr = true;
                        callback(err);
                    }
                    return;
                }
                --finishNum;
                result[r.i] = r.v;
                if (finishNum == 0) callback(0, result);
            });
            for (let i = 0; i < arr.length; ++i) {
                [args[0], args[1]] = [arr[i], i];
                bsFunc.apply(null, args);
            }
        }
    };
    //项目路径，此为模版库的路径
    webdom.path = function () {
        let template = webdom('meta[template]');
        return template.attr("path");
    };
    //模版文件的路径
    webdom.pagepath = function () {
        let view = webdom('meta[view]');
        let page = view.attr("page");
        return page.substring(0, page.lastIndexOf("/") + 1);
    };
    //模板是否启用缓存
    webdom.iscache = function () {
        let template = webdom('meta[template]');
        let iscache = template.attr("cache");
        return iscache == 'true';
    };
    //页面路由，和地址栏Url有一定区别
    webdom.route = function () {
        let template = webdom('meta[view]');
        return template.attr("route");
    };
    //页面路由的路径（不包括当前页面）
    webdom.routepath = function () {
        let route = this.route();
        if (route.indexOf('/') > -1) route = route.substring(0, route.lastIndexOf('/') + 1);
        return route;
    };
    //设备名称
    webdom.device = function () {
        let template = webdom('meta[device]');
        return template.attr("device");
    };
    //加载核心库的javascript文件
    //func:加载完成后的方法
    webdom.corejs = function (func, jsfile) {
        //要加载的js 
        let arr = ['vue.min', 'polyfill.min', 'axios_min',
            'api',      //axios封装
            'utils',    //工具类
            'position'  //获取GPS定位数据
        ];
        for (let t in arr) arr[t] = '/Utilities/Scripts/' + arr[t] + '.js';
        //附加js文件
        if (jsfile != null) {
            if (jsfile instanceof Array)
                arr = arr.concat(jsfile);
            else arr.push(jsfile);
        }
        webdom.load.js(arr, func);
    };
    //加载组件，例如vue组件
    //func:加载完成后的方法
    webdom.componentjs = function (jsfile, func) {
        if (jsfile != null) {
            //如果引用的js不是绝对路径，则默认取当前默认库的根路径
            for (var i = 0; i < jsfile.length; i++) {
                if (jsfile[i].substring(0, 1) == "/") continue;
                if (jsfile[i].length >= 7 && jsfile[i].substring(0, 7).toLowerCase() == "http://") continue;
                if (jsfile[i].length >= 8 && jsfile[i].substring(0, 8).toLowerCase() == "https://") continue;
                jsfile[i] = $dom.pagepath() + jsfile[i];
            }
            window.$dom.load.js(jsfile, func);
        } else if (func != null) func();
    };
    //加载自身相关的js或css  
    webdom.selfresource = function () {
        if (webdom('head[resource]').length > 0) {
            var view = webdom('meta[view]');
            if (view != null && view.length > 0) {
                var file = view.attr("view");
                if (file.indexOf('/') > -1) file = file.substring(file.lastIndexOf('/'));
                webdom.load.css([webdom.pagepath() + 'styles/' + file + '.css']);
                webdom.load.js([webdom.pagepath() + 'Scripts/' + file + '.js']);
            } else {
                let file = window.location.pathname;
                if (file.indexOf('/') > -1) file = file.substring(file.lastIndexOf('/') + 1);
                if (file.indexOf('.') > -1) file = file.substring(0, file.lastIndexOf('.'));
                webdom.load.css(['styles/' + file + '.css']);
                webdom.load.js(['Scripts/' + file + '.js']);
            }
        }
    };
    //创建全局对象，方便调用
    window.$dom = webdom;
})();
