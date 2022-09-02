/*!
 * 主  题：API调用方法
 * 说  明：用于调用Restful api接口的js方法。
 * 功能描述：
 * 1、封装api调用方法，采用axios异步读取服务端；支持get,post,delete,put,patch,options六种方式
 * 2、数据提交与返回均经过url编码与解码
 * 3、一些常用方法，如$api.trim，querystring,cookies等
 * 4、构建本地数据缓存（采用indexedDB）,调用方法$api.cache()，使用方式与get、post等无差别
 *
 *
 * 作  者：宋雷鸣 10522779@qq.com
 * 开发时间: 2019年5月20日
 */
(function () {
    var config = {
        //api的版本号
        versions: ["", "1", "2"],
        versionDefault: "1", //默认版本号
        //调用地址的根路径
        baseURL: '',
        pathUrl: "/api/v{0}/", //url路径
        apicache_location: false,     //本机是否缓存数据
        timeout: 60 * 60 * 24 * 1000        //请求过期时效       
    };
    //版权信息
    var copyright = {};
    //一些常用方法
    var methods = {
        //生成axios调用的路径,
        //vesion:api版本号,
        //way:api方法名，如/api/v1/[account/del]
        apiurl: function (version, way) {
            if (way === undefined) throw 'api名称不得为空';
            if (way.indexOf(':') > -1) way = way.substring(0, way.indexOf(':'));
            var url = config.pathUrl.replace("{0}", version);
            //调用地址的根路径可以在此处理，（如果需要跨多台服务器请求的话）
            if (config.baseURL != '') url = config.baseURL + url;
            return url + way;
        },
        //获取url中的参数
        //key:参数名
        //defvalue:默认值（如果key取不到值）
        querystring: function (key, defvalue) {
            defvalue = typeof (defvalue) == "undefined" ? "" : defvalue;
            var url = String(window.document.location.href);
            if (url.indexOf("?") < 0) return defvalue;
            //如果不带参数，则返回所有参数
            if (arguments.length == 0) return $api.url.get(url);
            //如果key不等空，则返回
            if (arguments.length == 1) return $api.url.get(url, key);
            if ($api.url.get(url, key) == '') return defvalue;
            return $api.url.get(url, key);
        },
        setpara: function (key, value) {
            var url = String(window.document.location.href);
            return $api.url.set(url, key, value);
        },
        //地址栏最后一个.后面的字符
        //如果只有一参数，val为默认值，即.后面没有值时，返回默认值
        //如果有两个参数，用于设置url的dot值
        dot: function (val, url) {
            if (arguments.length < 1) return $api.url.dot();
            if (arguments.length == 1) {
                var dot = $api.url.dot();
                return dot == '' ? val : dot;
            }
            if (arguments.length >= 2)
                return $api.url.dot(val, url);
        },
        //去除两端空格
        trim: function (str) {
            if (str == null) return '';
            return str.replace(/^\s*|\s*$/g, '').replace(/^\n+|\n+$/g, "");
        },
        //是否是本地路径
        islocal: function () {
            var host = window.location.hostname.toLowerCase();
            return host == 'localhost' || host == '127.0.0.1';
        },
        //将数据url解码
        unescape: function (data) {
            var typeName = methods.getType(data);
            if (typeName == 'String') return unescape(data);
            if (typeName == 'Object') return handleObject(data);
            if (typeName == 'Array') return handleArray(data);
            //反常时间的处理，如果时间处于一百年前，则返回空值
            function abnormalTime(date) {
                var now = new Date();
                var y = now.getFullYear();
                var m = now.getMonth();
                var d = now.getDate();
                now = new Date(y - 100, m < 10 ? '0' + m : m, d < 10 ? ('0' + d) : d);
                if (date > now) return date;
                return '';
            }
            //解码对象
            function handleObject(data) {
                for (var key in data) {
                    var typeName = methods.getType(data[key]);
                    if (typeName == 'String')
                        data[key] = methods.unescape(data[key]);
                    if (typeName == 'Date')
                        data[key] = abnormalTime(data[key]);
                    if (typeName == 'Array')
                        data[key] = handleArray(data[key]);
                    if (typeName == 'Object')
                        data[key] = handleObject(data[key]);
                }
                return data;
            }
            //解码数组
            function handleArray(data) {
                for (let i = 0; i < data.length; i++) {
                    const element = data[i];
                    data[i] = methods.unescape(element);
                }
                return data;
            }
            return data;
        },
        //判断数据类型
        getType: function (data) {
            var getType = Object.prototype.toString;
            var myType = getType.call(data); //调用call方法判断类型，结果返回形如[object Function]  
            var typeName = myType.slice(8, -1); //[object Function],即取除了“[object ”的字符串。 
            return typeName;
        },
        //storage存储
        storage: function (key, value) {
            var isAndroid = (/android/gi).test(navigator.appVersion);
            var uzStorage = function () {
                var ls = window.localStorage;
                if (isAndroid) ls = window.localStorage;
                return ls;
            };
            if (arguments.length === 0) {
                return uzStorage();
            }
            //var th=this;
            //console.error(th);
            //如果只有一个参数，为读取
            if (arguments.length === 1) {
                var ls = uzStorage();
                if (ls) {
                    var v = ls.getItem(key);
                    if (!v) return;
                    if (v.indexOf('obj-') === 0) {
                        v = v.slice(4);
                        return this.parseJson(v);
                    } else if (v.indexOf('str-') === 0) {
                        return v.slice(4);
                    }
                }
            }
            //如果两个参数，为写入，第一个为键，第二个为值
            if (arguments.length === 2) {
                if (value != null) {
                    var v = typeof value == 'object' ? 'obj-' + this.toJson(value) : 'str-' + value;
                    var ls = uzStorage();
                    if (ls) {
                        try {
                            ls.setItem(key, v);
                        } catch (oException) {
                            if (oException.name == 'QuotaExceededError') {
                                console.log('超出本地存储限额！');
                            }
                        }
                    }
                } else {
                    var ls = uzStorage();
                    if (ls && key) {
                        ls.removeItem(key);
                    }
                }
            }
        },
        cookie: function (name, value, options) {
            if (typeof value != 'undefined') { // name and value given, set cookie 
                options = options || {};
                if (value === null) { value = ''; options.expires = -1; }
                var expires = '';
                if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
                    var date;
                    if (typeof options.expires == 'number') {
                        date = new Date();
                        date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
                    } else {
                        date = options.expires;
                    }
                    expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE 
                }
                var path = options.path ? '; path=' + options.path : '; path=/';
                var domain = options.domain ? '; domain=' + options.domain : '';
                var secure = options.secure ? '; secure' : '';
                document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
            } else { // only name given, get cookie 
                var cookieValue = null;
                if (document.cookie && document.cookie != '') {
                    var cookies = document.cookie.split(';');
                    for (var i = 0; i < cookies.length; i++) {
                        var cookie = methods.trim(cookies[i]);
                        // Does this cookie string begin with the name we want? 
                        if (cookie.substring(0, name.length + 1) == (name + '=')) {
                            cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                            break;
                        }
                    }
                }
                return cookieValue;
            }
        },
        //记录或获取登录状态值
        loginstatus: function (key, code, id) {
            var storagename = 'weishakeji_loginstatus';
            var status = methods.storage(storagename);
            //读取
            if (arguments.length <= 1) {
                if (status == null || !(status instanceof Array)) return '';
                var str = '';
                if (arguments.length == 0) {

                    for (var i = 0; i < status.length; i++) {
                        if (!!status[i].val) str += status[i].val;
                        if (i < status.length - 1) str += ',';
                    }
                }
                if (arguments.length == 1) {
                    for (var t in status) {
                        if (!!status[t].key && status[t].key == key)
                            str = status[t].val;
                    }
                }
                return str;
            }
            //写入
            if (arguments.length >= 2) {
                if (status == null || !(status instanceof Array)) status = [];
                var time = (new Date()).format('yyyy-MM-dd HH:mm:ss');
                var isexist = false;
                for (var t in status) {
                    if (!!status[t].key && status[t].key == key) {
                        status[t].val = code;
                        status[t].time = time;
                        isexist = true;
                    }
                }
                if (!isexist) status.push({
                    key: key,
                    val: code,
                    time: time,
                    id: id == null ? 0 : id
                });
                methods.storage(storagename, status);
                return status;
            }
        },
        //判断变量是否是对象
        isobj: function (obj) {
            return Object.prototype.toString.call(obj) === '[object Object]';
        },
        //在线浏览pdf文件
        pdfViewer: function (file) {
            var viewer = "/Utilities/PdfViewer/viewer.html";
            if (file.indexOf("?") > -1) file = file.substring(0, file.indexOf("?"));
            viewer += "?file=" + encodeURIComponent(file);
            //window.location.href = viewer;
            return viewer;
        },
        //网页是否处于微信内置浏览器
        isWeixin: function () {
            var ua = window.navigator.userAgent.toLowerCase();
            return ua.match(/MicroMessenger/i) == 'micromessenger';
        },
        //网页是否处于微信小程序内置浏览器
        isWeixinApp: function () {
            var ua = window.navigator.userAgent.toLowerCase();
            return ua.match(/miniProgram/i) == 'miniprogram';
        },
        //是否是IE浏览器
        isIE: function () {
            return !!window.ActiveXObject || "ActiveXObject" in window;
        },
        //对象转Json
        toJson: function (value) {
            //将对象中为的DateTime对象转为数值
            function handle_time(obj) {
                var typeName = methods.getType(obj);
                if (typeName == 'Date') obj = String(obj.getTime()) + '.0000';
                if (obj instanceof Array || typeName === 'Object') {
                    for (var d in obj) {
                        obj[d] = handle_time(obj[d]);
                    }
                }
                return obj;
            }
            value = handle_time(value);
            var json = JSON.stringify(value);
            //再将对象中的数值转为时间DateTime            
            value = methods.restore_time(value);
            return json;
        },
        //将json还原为对象
        parseJson: function (str) {
            var result = JSON.parse(str);
            return methods.restore_time(result);
        },
        //将对象中为的时间数值还原为DateTime
        restore_time: function (obj) {
            var typeName = methods.getType(obj);
            if (obj instanceof Array || typeName === 'Object') {
                for (var d in obj) obj[d] = methods.restore_time(obj[d]);
            }
            if (typeName == 'String') {
                if (obj.indexOf('.') > -1) {
                    var prefix = obj.substring(0, obj.lastIndexOf('.'));
                    var suffix = obj.substring(obj.lastIndexOf('.') + 1);
                    if (suffix === '0000' && /^\d+$/.exec(prefix))
                        obj = new Date(Number(prefix));
                }
            }
            return obj;
        },
        //克隆对象
        clone: function (obj) {
            return methods.parseJson(methods.toJson(obj));
        }
    };
    //api操作的具体对象和方法
    var apiObj = function (version) {
        //加载效果，参数：前者为一般loading效果，后者一般为去除loading
        this.effect = function (loading, loaded) {
            this.loadeffect.before = loading;
            this.loadeffect.after = loaded;
            return this;
        }
        this.loadeffect = {
            before: null,
            after: null
        };
        //当前api要请求的服务端接口的版本号
        this.version = version == null ? config.versionDefault : version;
        var httpverb = ['get', 'post', 'delete', 'put', 'patch', 'options', 'cache'];
        for (var i = 0; i < httpverb.length; i++) {
            var el = httpverb[i];
            var tm = "this." + el + " = function (way, parameters,loading,loaded) {return this.query(way, parameters, '" + el + "',loading,loaded,'json');}";
            eval(tm);
        }
        var self = this;
        //创建请求对象，以及拦截器
        //way:接口方法
        //returntype:返回数据的类型，Json或xml
        this.query = function (way, parameters, method, loading, loaded, returntype) {
            if (method == 'cache') {
                return $api.api_cache.get(way, parameters).then(function (res) {
                    return new Promise(function (resolve, reject) {
                        resolve(res);
                    });
                }).catch(function (err) {
                    console.log(err);
                    // console.log(p);
                    return $api.query(way, parameters, 'get_cache', loading, loaded, returntype);
                });
            }
            //动作，即way冒号后面的关键字，一般用于cache方法的动作，例如clear清除缓存
            var action = way.indexOf(':') > -1 ? way.substring(way.indexOf(':') + 1) : 'null';
            var url = methods.apiurl(this.version, way);
            if (arguments.length < 2 || parameters == null) parameters = {};
            if (arguments.length < 3 || method == null || method == '') method = 'get';
            var true_method = method.indexOf('get') < 0 ? 'post' : 'get';
            var custom_method = method == "get_cache" ? "cache" : method;
            //开始时间
            var startTime = new Date();
            //登录状态
            var loginstatus = methods.loginstatus();
            //console.log(loginstatus);
            //创建axiso对象
            var instance = axios.create({
                method: true_method,
                custom_method: custom_method,
                way: way,
                url: url,
                headers: {
                    'X-Custom-Header': 'weishakeji',
                    'Content-Type': 'multipart/form-data',
                    'Access-Control-Allow-Headers': 'X-Requested-With',
                    'content-type': 'application/x-www-form-urlencoded',
                    'Access-Control-Allow-Methods': 'POST,GET,DELETE,PUT,PATCH,HEAD,OPTIONS'
                },
                auth: {
                    username: 'weishakeji ' + custom_method + ' ' + action + ' ' + returntype + ' ' + window.location,
                    password: loginstatus
                },
                timeout: config.timeout,
                returntype: returntype
            });
            //添加请求拦截器（即请求之前）
            instance.interceptors.request.use(function (config) {
                if (loading == null) loading = self.loadeffect.before;
                if (loading != null) loading(config);
                //在发送请求之前做某件事
                if (config.method == 'get') {
                    config.parameters = config.params;
                    //克隆参数对象，因为上传的参数要escape转码，需要保留原数据类型
                    var tmpObj = new Object();
                    for (var d in config.params) {
                        var typeName = methods.getType(config.params[d]);
                        if (typeName == 'Date') {
                            tmpObj[d] = config.params[d].getTime();
                            continue;
                        }
                        //json值，序列化为字符串
                        if (typeName === 'Object' || config.params[d] instanceof Array) {
                            try {
                                tmpObj[d] = escape(methods.toJson(config.params[d]));
                                continue;
                            } catch (err) {
                                var msg = err + ' 有可能的原因：不要采用get方式递交超长参数，建议采用Post方式';
                                console.error(msg);
                                return Promise.reject(msg);
                            }
                        }
                        tmpObj[d] = escape(config.params[d]);
                    }
                    config.params = tmpObj;
                } else {
                    config.parameters = config.data;
                    var formData = new FormData();
                    for (var d in config.data) {
                        var typeName = methods.getType(config.data[d]);
                        if (typeName == 'Date') {
                            formData.append(d, config.data[d].getTime());
                            continue;
                        }
                        //json值，序列化为字符串                       
                        if (typeName === 'Object' || config.data[d] instanceof Array) {
                            formData.append(d, escape(methods.toJson(config.data[d])));
                            continue;
                        }
                        if (typeName === 'File' || typeName === "FileList" || typeName === "Blob") {
                            formData.append(d, config.data[d]);
                            continue;
                        }
                        formData.append(d, escape(config.data[d]));
                    }
                    config.data = formData;
                }
                return config;
            }, function (error) {
                console.log('错误的传参');
                if (loaded == null) loaded = self.loadeffect.after;
                if (loading != null) loaded(config, error);
                //return Promise.reject(error);
            });
            //添加响应拦截器（即返回之后）
            instance.interceptors.response.use(function (response) {
                //保留未解析为json之前的数据
                //response.text = response.data;
                //如果返回的数据是字符串，这里转为json
                if (response.config.returntype == "json") {
                    if (typeof (response.data) == 'string') {
                        response.data = eval("(" + response.data + ")");
                    }
                    //处理数据，服务器端返回的数据是经过Url编码的，此处进行解码
                    response.data = methods.unescape(response.data);
                    if (response.data.result != null) {
                        if (response.data.datatype != 'String' && typeof (response.data.result) == 'string') {
                            try {
                                response.data.result = eval("(" + response.data.result + ")");
                            } catch (err) {
                                //alert(err);
                            }
                        }
                        if (response.data.datatype == "JArray" || response.data.datatype == "JObject") {
                            response.data.result = methods.unescape(response.data.result);
                        }
                    }
                }
                //如果要缓存接口数据，返回成功才会被缓存
                if (response.config.custom_method.indexOf('cache') > -1 && response.data.success) {
                    var params = response.config.parameters ? response.config.parameters : response.config.params;
                    $api.api_cache.put(response.config.way, params, response);
                }
                //计算执行耗时
                if (response.data) {
                    response.data['webspan'] = new Date() - startTime;
                }
                //执行加载完成后的方法
                if (loaded == null) loaded = self.loadeffect.after;
                if (loaded != null) loaded(response, null);
                return response;
            }, function (error) {
                if (loaded == null) loaded = self.loadeffect.after;
                if (loaded != null) loaded(null, error);
                //return Promise.reject(error);
            });
            //如果是get方式，参数名是params；非get参数名是data
            if (true_method == 'get') {
                return instance.request({ params: parameters });
            } else {
                return instance.request({ data: parameters });
            }
        }
        //一次获取多个数据
        this.bat = function (queryArr) {
            if (arguments.length == 0) return null;
            //if (arguments.length == 1) return queryArr;
            return axios.all(arguments);
        }
        //常用方法加到$api根，方便调用
        for (var m in methods) {
            eval("this." + m + "=" + methods[m] + ";");
        }
    };

    //分享页面的超链接处理
    apiObj.prototype.spread = function (f) {

    };
    //登录状态的管理
    apiObj.prototype.login = {
        //当前登录的管理员
        admin: function (f) {
            return this.get('Admin/General', f);
        },
        //当前登录的超级管理员
        super: function (f) {
            return this.get('Admin/Super', f);
        },
        //当前登录的学员
        account: function (f) {
            return this.get('Account/Current', f);
        },
        //当前登录的教师
        teacher: function (f) {
            return this.get('Teacher/Current', f);
        },
        admin_fresh: function (f) {
            return this.fresh('admin', f);
        },
        super_fresh: function (f) {
            return this.fresh('admin', f);
        },
        account_fresh: function (f) {
            return this.fresh('account', f);
        },
        teacher_fresh: function (f) {
            return this.fresh('account', f);
        },
        //获取登录对象
        get: function (apiurl, func) {
            if (func != null) func();
            var promise = new Promise(function (resolve, reject) {
                $api.post(apiurl).then(function (req) {
                    if (req.data.success) {
                        resolve(req.data.result);
                    } else {
                        reject(apiurl + ' : not logined !');
                    }
                }).catch(function (err) {
                    reject(apiurl + ' : ' + err);
                });
            });
            return promise;
        },
        //刷新登录状态
        //status:登录状态类型，只有admin、account两个选择
        //lose_func: 登录失效后调用的方法
        fresh: function (status, lose_func) {
            if (status == 'account') return;    //学员账号不刷新登录状态
            var interval_name = 'login_' + status + '_fresh_keyname';
            //console.log(interval_name);
            window[interval_name] = window.setInterval(function () {
                var loginfo = $api.loginstatus(status);
                if (loginfo == '') {
                    $api.loginstatus(status, '');
                    clearInterval(window[interval_name]);
                    if (lose_func != null) lose_func();
                    return;
                }
                //console.error('interval_name');
                $api.post(status + '/Fresh').then(function (req) {
                    if (req == null) throw '';
                    if (req.data.success) {
                        var result = req.data.result;
                        if (result == '') throw '校验码返回为空';
                        $api.loginstatus(status, result);
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                    //$api.loginstatus(status, '');
                    clearInterval(window[interval_name]);
                    if (lose_func != null) lose_func();
                });
            }, 1000 * 60 * 10);
        }

    };
    //一些网址的处理方法
    apiObj.prototype.url = {
        //地址的参数
        params: function (url) {
            if (url == null || url == '') url = String(window.document.location.href);
            //取所有参数
            var values = new Array();
            if (url.indexOf("?") > -1) {
                var query = url.substring(url.lastIndexOf("?") + 1);
                var paras = query.split('&');
                for (var q in paras) {
                    var arr = paras[q].split('=');
                    if (arr.length < 2) continue;
                    if (arr[1].indexOf("#") > -1) arr[1] = arr[1].substring(0, arr[1].indexOf("#"));
                    arr[1] = decodeURI(arr[1]).replace(/<[^>]+>/g, "");
                    values.push({
                        key: arr[0],
                        val: arr[1]
                    });
                }
            }
            return values;
        },
        //获取参数
        get: function (url, key) {
            if (key == undefined || key == null) return '';
            var values = this.params(url);
            for (var q in values) {
                if (values[q].key.toLowerCase() == key.toLowerCase())
                    return values[q].val;
            }
            return '';
        },
        set: function (url, key, value) {
            if (key.constructor === Object) {
                for (let k in key) {
                    url = this.set(url, k, key[k]);
                }
                return url;
            }
            var values = this.params(url);
            var isExist = false;
            for (var q in values) {
                if (values[q].key.toLowerCase() == key.toLowerCase()) {
                    values[q].val = value;
                    isExist = true;
                }
            }
            if (!isExist) values.push({ key: key, val: value });

            //拼接Url      
            if (url == null || url == '') url = String(window.document.location.href);
            if (url.indexOf("?") > -1) url = url.substring(0, url.lastIndexOf("?"));
            var parastr = "";
            for (var i = 0; i < values.length; i++) {
                if (values[i].val == null || values[i].val == '') continue;
                parastr += values[i].key + "=" + values[i].val;
                if (i < values.length - 1) parastr += "&";
            }
            return parastr.length > 0 ? url + "?" + parastr : url;
        },
        //地址栏最后一个.后面的字符
        //key:如果
        dot: function (key, url) {
            if (arguments.length <= 1) {
                var url = String(window.document.location.href);
                if (url.indexOf('/') > -1) url = url.substring(url.lastIndexOf('/') + 1);
                if (url.indexOf('?') > -1) url = url.substring(0, url.lastIndexOf('?'));
                if (url.indexOf('#') > -1) url = url.substring(0, url.lastIndexOf('#'));
                if (url.indexOf('.') > -1) return url.substring(url.lastIndexOf('.') + 1);
                var id = url.substring(url.lastIndexOf('.') + 1);
                return this.get(String(window.document.location.href), key);
            }
            if (arguments.length == 2) {
                var val = key;
                var prefix = '', suffix = '', parastr = '';
                if (url.indexOf('?') > -1) {
                    parastr = url.substring(url.lastIndexOf('?'));
                    url = url.substring(0, url.lastIndexOf('?'));
                }
                if (url.indexOf('.') > -1) {
                    suffix = url.substring(url.lastIndexOf('.') + 1);
                    prefix = url.substring(0, url.lastIndexOf('.'));
                } else {
                    prefix = url;
                }
                while (prefix.substring(prefix.length - 1) == "#")
                    prefix = prefix.substring(0, prefix.length - 1);
                return prefix.toLowerCase() + '.' + val + parastr;
            }
        },
        //验证是否是合法的网址
        check: function (url) {
            var reg=/(http|ftp|https|mms):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?/;           
            return reg.test(url);
        },
        //取地址的主机部分，例如 http://xxx.com/
        host: function (url) {
            if (url == null) url = window.location.href;
            if (url.length < 4) return '';
            if (url.substring(0, 4).toLowerCase() != 'http') return '';
            var arr = url.split('/');
            var host = '';
            if (arr.length >= 3) {
                for (let i = 0; i < 3; i++) host += arr[i] + '/';
            }
            return host.toLowerCase();
        }
    };
    //机构信息，主要为了解析config
    apiObj.prototype.organ = function (organ) {
        var obj = { 'obj': organ, 'id': organ.Org_ID, 'domain': organ.Org_TwoDomain, 'config': {} };
        if (!organ.Org_Config || organ.Org_Config == '') return obj;

        obj.config = this.xmlconfig.tojson(organ.Org_Config);
        for (var t in obj.config) {
            if (!isNaN(Number(obj.config[t]))) {
                obj.config[t] = Number(obj.config[t]);
                continue;
            }
            if (obj.config[t] == 'True') obj.config[t] = true;
            if (obj.config[t] == 'False') obj.config[t] = false;

        }
        //console.log(obj);
        return obj;
    };
    //处理xml的配置信息，老版本中服务端大量采用了xml作为配置项
    apiObj.prototype.xmlconfig = {
        tojson: function (xml) {
            var obj = {};
            if (xml == '') return obj;
            //创建文档对象
            var parser = new DOMParser();
            var xmlDoc = parser.parseFromString(xml, "text/xml");
            var nodes = xmlDoc.lastChild.children;
            for (var i = 0; i < nodes.length; i++) {
                var key = nodes[i].attributes['key'].value;
                var val = nodes[i].attributes['value'].value;
                //如果是逻辑值
                if (val == 'True') obj[key] = true;
                if (val == 'False') obj[key] = false;
                //如果是数值
                if (!isNaN(Number(val))) {
                    obj[key] = Number(val);
                } else {
                    obj[key] = val;
                }
            }
            return obj;
        },
        toxml: function (json) {
            var xml = '<?xml version="1.0" encoding="UTF-8"?>';
            xml += '<items>';
            if (json != null) {
                for (var key in json) {
                    xml += '<item key="' + key + '" value="' + json[key] + '">';
                    xml += '</item>';
                }
            }
            xml += '</items>';
            return xml;
        }
    };
    //md5编码
    apiObj.prototype.md5 = function (string) {
        function md5_RotateLeft(lValue, iShiftBits) {
            return (lValue << iShiftBits) | (lValue >>> (32 - iShiftBits));
        }
        function md5_AddUnsigned(lX, lY) {
            var lX4, lY4, lX8, lY8, lResult;
            lX8 = (lX & 0x80000000);
            lY8 = (lY & 0x80000000);
            lX4 = (lX & 0x40000000);
            lY4 = (lY & 0x40000000);
            lResult = (lX & 0x3FFFFFFF) + (lY & 0x3FFFFFFF);
            if (lX4 & lY4) {
                return (lResult ^ 0x80000000 ^ lX8 ^ lY8);
            }
            if (lX4 | lY4) {
                if (lResult & 0x40000000) {
                    return (lResult ^ 0xC0000000 ^ lX8 ^ lY8);
                } else {
                    return (lResult ^ 0x40000000 ^ lX8 ^ lY8);
                }
            } else {
                return (lResult ^ lX8 ^ lY8);
            }
        }
        function md5_F(x, y, z) {
            return (x & y) | ((~x) & z);
        }
        function md5_G(x, y, z) {
            return (x & z) | (y & (~z));
        }
        function md5_H(x, y, z) {
            return (x ^ y ^ z);
        }
        function md5_I(x, y, z) {
            return (y ^ (x | (~z)));
        }
        function md5_FF(a, b, c, d, x, s, ac) {
            a = md5_AddUnsigned(a, md5_AddUnsigned(md5_AddUnsigned(md5_F(b, c, d), x), ac));
            return md5_AddUnsigned(md5_RotateLeft(a, s), b);
        };
        function md5_GG(a, b, c, d, x, s, ac) {
            a = md5_AddUnsigned(a, md5_AddUnsigned(md5_AddUnsigned(md5_G(b, c, d), x), ac));
            return md5_AddUnsigned(md5_RotateLeft(a, s), b);
        };
        function md5_HH(a, b, c, d, x, s, ac) {
            a = md5_AddUnsigned(a, md5_AddUnsigned(md5_AddUnsigned(md5_H(b, c, d), x), ac));
            return md5_AddUnsigned(md5_RotateLeft(a, s), b);
        };
        function md5_II(a, b, c, d, x, s, ac) {
            a = md5_AddUnsigned(a, md5_AddUnsigned(md5_AddUnsigned(md5_I(b, c, d), x), ac));
            return md5_AddUnsigned(md5_RotateLeft(a, s), b);
        };
        function md5_ConvertToWordArray(string) {
            var lWordCount;
            var lMessageLength = string.length;
            var lNumberOfWords_temp1 = lMessageLength + 8;
            var lNumberOfWords_temp2 = (lNumberOfWords_temp1 - (lNumberOfWords_temp1 % 64)) / 64;
            var lNumberOfWords = (lNumberOfWords_temp2 + 1) * 16;
            var lWordArray = Array(lNumberOfWords - 1);
            var lBytePosition = 0;
            var lByteCount = 0;
            while (lByteCount < lMessageLength) {
                lWordCount = (lByteCount - (lByteCount % 4)) / 4;
                lBytePosition = (lByteCount % 4) * 8;
                lWordArray[lWordCount] = (lWordArray[lWordCount] | (string.charCodeAt(lByteCount) << lBytePosition));
                lByteCount++;
            }
            lWordCount = (lByteCount - (lByteCount % 4)) / 4;
            lBytePosition = (lByteCount % 4) * 8;
            lWordArray[lWordCount] = lWordArray[lWordCount] | (0x80 << lBytePosition);
            lWordArray[lNumberOfWords - 2] = lMessageLength << 3;
            lWordArray[lNumberOfWords - 1] = lMessageLength >>> 29;
            return lWordArray;
        };
        function md5_WordToHex(lValue) {
            var WordToHexValue = "", WordToHexValue_temp = "", lByte, lCount;
            for (lCount = 0; lCount <= 3; lCount++) {
                lByte = (lValue >>> (lCount * 8)) & 255;
                WordToHexValue_temp = "0" + lByte.toString(16);
                WordToHexValue = WordToHexValue + WordToHexValue_temp.substr(WordToHexValue_temp.length - 2, 2);
            }
            return WordToHexValue;
        };
        function md5_Utf8Encode(string) {
            string = string.replace(/\r\n/g, "\n");
            var utftext = "";
            for (var n = 0; n < string.length; n++) {
                var c = string.charCodeAt(n);
                if (c < 128) {
                    utftext += String.fromCharCode(c);
                } else if ((c > 127) && (c < 2048)) {
                    utftext += String.fromCharCode((c >> 6) | 192);
                    utftext += String.fromCharCode((c & 63) | 128);
                } else {
                    utftext += String.fromCharCode((c >> 12) | 224);
                    utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                    utftext += String.fromCharCode((c & 63) | 128);
                }
            }
            return utftext;
        };
        var x = Array();
        var k, AA, BB, CC, DD, a, b, c, d;
        var S11 = 7, S12 = 12, S13 = 17, S14 = 22;
        var S21 = 5, S22 = 9, S23 = 14, S24 = 20;
        var S31 = 4, S32 = 11, S33 = 16, S34 = 23;
        var S41 = 6, S42 = 10, S43 = 15, S44 = 21;
        string = md5_Utf8Encode(string);
        x = md5_ConvertToWordArray(string);
        a = 0x67452301; b = 0xEFCDAB89; c = 0x98BADCFE; d = 0x10325476;
        for (k = 0; k < x.length; k += 16) {
            AA = a; BB = b; CC = c; DD = d;
            a = md5_FF(a, b, c, d, x[k + 0], S11, 0xD76AA478);
            d = md5_FF(d, a, b, c, x[k + 1], S12, 0xE8C7B756);
            c = md5_FF(c, d, a, b, x[k + 2], S13, 0x242070DB);
            b = md5_FF(b, c, d, a, x[k + 3], S14, 0xC1BDCEEE);
            a = md5_FF(a, b, c, d, x[k + 4], S11, 0xF57C0FAF);
            d = md5_FF(d, a, b, c, x[k + 5], S12, 0x4787C62A);
            c = md5_FF(c, d, a, b, x[k + 6], S13, 0xA8304613);
            b = md5_FF(b, c, d, a, x[k + 7], S14, 0xFD469501);
            a = md5_FF(a, b, c, d, x[k + 8], S11, 0x698098D8);
            d = md5_FF(d, a, b, c, x[k + 9], S12, 0x8B44F7AF);
            c = md5_FF(c, d, a, b, x[k + 10], S13, 0xFFFF5BB1);
            b = md5_FF(b, c, d, a, x[k + 11], S14, 0x895CD7BE);
            a = md5_FF(a, b, c, d, x[k + 12], S11, 0x6B901122);
            d = md5_FF(d, a, b, c, x[k + 13], S12, 0xFD987193);
            c = md5_FF(c, d, a, b, x[k + 14], S13, 0xA679438E);
            b = md5_FF(b, c, d, a, x[k + 15], S14, 0x49B40821);
            a = md5_GG(a, b, c, d, x[k + 1], S21, 0xF61E2562);
            d = md5_GG(d, a, b, c, x[k + 6], S22, 0xC040B340);
            c = md5_GG(c, d, a, b, x[k + 11], S23, 0x265E5A51);
            b = md5_GG(b, c, d, a, x[k + 0], S24, 0xE9B6C7AA);
            a = md5_GG(a, b, c, d, x[k + 5], S21, 0xD62F105D);
            d = md5_GG(d, a, b, c, x[k + 10], S22, 0x2441453);
            c = md5_GG(c, d, a, b, x[k + 15], S23, 0xD8A1E681);
            b = md5_GG(b, c, d, a, x[k + 4], S24, 0xE7D3FBC8);
            a = md5_GG(a, b, c, d, x[k + 9], S21, 0x21E1CDE6);
            d = md5_GG(d, a, b, c, x[k + 14], S22, 0xC33707D6);
            c = md5_GG(c, d, a, b, x[k + 3], S23, 0xF4D50D87);
            b = md5_GG(b, c, d, a, x[k + 8], S24, 0x455A14ED);
            a = md5_GG(a, b, c, d, x[k + 13], S21, 0xA9E3E905);
            d = md5_GG(d, a, b, c, x[k + 2], S22, 0xFCEFA3F8);
            c = md5_GG(c, d, a, b, x[k + 7], S23, 0x676F02D9);
            b = md5_GG(b, c, d, a, x[k + 12], S24, 0x8D2A4C8A);
            a = md5_HH(a, b, c, d, x[k + 5], S31, 0xFFFA3942);
            d = md5_HH(d, a, b, c, x[k + 8], S32, 0x8771F681);
            c = md5_HH(c, d, a, b, x[k + 11], S33, 0x6D9D6122);
            b = md5_HH(b, c, d, a, x[k + 14], S34, 0xFDE5380C);
            a = md5_HH(a, b, c, d, x[k + 1], S31, 0xA4BEEA44);
            d = md5_HH(d, a, b, c, x[k + 4], S32, 0x4BDECFA9);
            c = md5_HH(c, d, a, b, x[k + 7], S33, 0xF6BB4B60);
            b = md5_HH(b, c, d, a, x[k + 10], S34, 0xBEBFBC70);
            a = md5_HH(a, b, c, d, x[k + 13], S31, 0x289B7EC6);
            d = md5_HH(d, a, b, c, x[k + 0], S32, 0xEAA127FA);
            c = md5_HH(c, d, a, b, x[k + 3], S33, 0xD4EF3085);
            b = md5_HH(b, c, d, a, x[k + 6], S34, 0x4881D05);
            a = md5_HH(a, b, c, d, x[k + 9], S31, 0xD9D4D039);
            d = md5_HH(d, a, b, c, x[k + 12], S32, 0xE6DB99E5);
            c = md5_HH(c, d, a, b, x[k + 15], S33, 0x1FA27CF8);
            b = md5_HH(b, c, d, a, x[k + 2], S34, 0xC4AC5665);
            a = md5_II(a, b, c, d, x[k + 0], S41, 0xF4292244);
            d = md5_II(d, a, b, c, x[k + 7], S42, 0x432AFF97);
            c = md5_II(c, d, a, b, x[k + 14], S43, 0xAB9423A7);
            b = md5_II(b, c, d, a, x[k + 5], S44, 0xFC93A039);
            a = md5_II(a, b, c, d, x[k + 12], S41, 0x655B59C3);
            d = md5_II(d, a, b, c, x[k + 3], S42, 0x8F0CCC92);
            c = md5_II(c, d, a, b, x[k + 10], S43, 0xFFEFF47D);
            b = md5_II(b, c, d, a, x[k + 1], S44, 0x85845DD1);
            a = md5_II(a, b, c, d, x[k + 8], S41, 0x6FA87E4F);
            d = md5_II(d, a, b, c, x[k + 15], S42, 0xFE2CE6E0);
            c = md5_II(c, d, a, b, x[k + 6], S43, 0xA3014314);
            b = md5_II(b, c, d, a, x[k + 13], S44, 0x4E0811A1);
            a = md5_II(a, b, c, d, x[k + 4], S41, 0xF7537E82);
            d = md5_II(d, a, b, c, x[k + 11], S42, 0xBD3AF235);
            c = md5_II(c, d, a, b, x[k + 2], S43, 0x2AD7D2BB);
            b = md5_II(b, c, d, a, x[k + 9], S44, 0xEB86D391);
            a = md5_AddUnsigned(a, AA);
            b = md5_AddUnsigned(b, BB);
            c = md5_AddUnsigned(c, CC);
            d = md5_AddUnsigned(d, DD);
        }
        return (md5_WordToHex(a) + md5_WordToHex(b) + md5_WordToHex(c) + md5_WordToHex(d)).toLowerCase();
    };
    //本地接口缓存,way:api请求路径,para：请求参数,value:api的返回值
    apiObj.prototype.api_cache = {
        //api本地缓存库(indexedDb)
        dbname: "apicache_weishakeji",
        indexedDB: window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB,
        //way:api请求路径,para：请求参数,value:api的返回值
        //return:无返回值
        createstore: function (store, version) {
            var th = this;
            new Promise(function (resolve, reject) {
                var request = version ? th.indexedDB.open(th.dbname, version) : th.indexedDB.open(th.dbname);
                request.onupgradeneeded = function (event) {
                    var db = event.target.result;
                    var objectStore;
                    if (db && !db.objectStoreNames.contains(store)) {
                        objectStore = db.createObjectStore(store, { keyPath: "keyid" });
                        resolve(db.objectStoreNames);
                    }
                    if (db) db.close();
                }
            }).then(function (stores) {
                console.group('当前库中的stores:');
                console.log(stores);
                console.groupEnd();

            }).catch(function (err) {
                console.group('创建 store 错误:');
                console.log(err);
                console.groupEnd();
            });;
        },
        //添加数据
        put: function (way, para, value) {
            var p = this.parse(way, para, value);
            if (!config.apicache_location) {
                if (window.location.hostname == 'localhost' && !p.compel) return;
            }
            var th = this;
            if (p.active == 'clear') return;
            new Promise(function (resolve, reject) {
                var request = th.indexedDB.open(th.dbname);
                if (request.readyState == "pending") {
                    var err = th.error(-4, 'pending', request, '数据库"' + th.dbname + '"进行中(pending)', th.dbname, 1);
                    //reject(err);
                }
                request.onerror = function (event) {
                    var err = th.error(-2, 'onerror', event.target, '数据库"' + th.dbname + '"打开失败', th.dbname, 1);
                    reject(err);
                };
                request.onblocked = function (event) {
                    var err = th.error(-3, 'onblocked', event.target, '存储空间"' + p.store + '"被占用', th.dbname, p.store);
                    reject(err);
                };
                request.onsuccess = function (event) {
                    var db = event.target.result;
                    if (!db.objectStoreNames.contains(p.store)) {
                        var err = th.error(0, 'onsuccess', event.target, '存储空间"' + p.store + '"不存在', db, p.store);
                        reject(err);
                    } else {
                        var store = db.transaction([p.store], 'readwrite').objectStore(p.store);
                        store.put(p);
                        resolve(db);
                    }
                };
            }).then(function (db) {
                if (db) db.close();

            }).catch(function (err) {
                console.group('apicace_get error:');
                if (!!err.message) console.log(err.message);
                if (!!err.target) console.log(err.target);
                console.groupEnd();
            });
        },
        //获取缓存数据
        get: function (way, para) {
            var p = this.parse(way, para);
            var th = this;
            return new Promise(function (resolve, reject) {
                var subject = ':' + p.store + '[' + JSON.stringify(p.para) + ']';
                if (!config.apicache_location) {
                    if (window.location.hostname == 'localhost' && !p.compel) return reject('本机不允许缓存');
                }
                var request = window.indexedDB.open(th.dbname);
                if (request.readyState == "pending") {
                    var err = th.error(-4, 'pending', request,
                        '数据库"' + th.dbname + '"进行中(pending),' + way + '没有获取到数据',
                        th.dbname, 1);
                    //reject(err);
                }
                request.onerror = function (event) {
                    var err = th.error(-2, 'onerror', event.target, '数据库"' + th.dbname + '"打开失败', th.dbname, p.store);
                    reject(err);
                };
                request.onblocked = function (event) {
                    var err = th.error(-3, 'onblocked', '数据库"' + p.store + '"被占用', th.dbname, p.store);
                    reject(err);
                };
                request.onsuccess = function (event) {
                    var db = event.target.result;
                    if (!db.objectStoreNames.contains(p.store)) {
                        var err = th.error(0, 'onsuccess', event.target, '存储空间"' + p.store + '"不存在', db, p.store);
                        var version = db.version;
                        db.close();
                        th.createstore(p.store, version + 1);
                        reject(err);
                    } else {
                        if (p.active == 'update') return reject('更新缓存' + subject);
                        if (p.active == 'clear') {
                            th.del(way, para).then(function (d) {
                                reject(d);
                            }).catch(function (err) {
                                reject(err);
                            });
                            return;
                        }
                        var transaction = db.transaction(p.store);
                        var store = transaction.objectStore(p.store);
                        var storeget = store.get(p.keyid);
                        var result;
                        storeget.onsuccess = function (e) {
                            result = e.target.result;
                        };
                        transaction.oncomplete = function (event) {
                            if (!result) {
                                var err = th.error(0, 'oncomplete',
                                    event.target, '数据项"' + p.store + ':' + JSON.stringify(p.para) + '"不存在', db,
                                    {
                                        store: p.store,
                                        keyid: p.valuekeyid
                                    });
                                reject(err);
                            } else {
                                if (result.expires.time >= new Date())
                                    resolve(result.value);
                                else
                                    reject('缓存过期' + subject);
                            }
                            db.close();
                        }
                    }
                };
                setTimeout(function () {
                    var err = th.error(-3, 'timeout', '数据库"' + p.store + '"执行超时', th.dbname, p.store);
                    reject(err);
                }, 500);
            });
        },
        del: function (way, para) {
            var p = this.parse(way, para);
            var th = this;
            return new Promise(function (resolve, reject) {
                var request = window.indexedDB.open(th.dbname);
                request.onsuccess = function (event) {
                    var db = event.target.result;
                    if (!db.objectStoreNames.contains(p.store)) {
                        var err = th.error(0, 'onsuccess', event.target, '存储空间"' + p.store + '"不存在', db, p.store);
                        reject(err);
                        if (db) db.close();
                        return;
                    }
                    var store = db.transaction(p.store, 'readwrite').objectStore(p.store);
                    var trans = store.delete(p.keyid);
                    trans.onsuccess = function (event) {
                        var db = event.target.result;
                        if (db) db.close();
                        resolve(p);
                    };
                    trans.onerror = function (event) {
                        var db = event.target.result;
                        var err = th.error(1, 'onerror', event.target, '数据项"' + p.store + ':' + key + '"删除失败', db, {
                            store: p.store,
                            keyid: keyid
                        });
                        reject(err);
                        if (db) db.close();
                    }
                };
            });
        },
        //解析来自api请求数据的信息
        parse: function (way, para, value) {
            way = way.toLowerCase();
            //接口缓存名称，缓存指令，是否强制缓存，缓存项的名称
            var name, active = '', compel = false, key;
            if (way.indexOf(":") > -1) {
                active = way.substring(way.indexOf(':') + 1);
                way = way.substring(0, way.indexOf(":"));
            }
            para = para == null || JSON.stringify(para) == '{}' ? '' : para;
            name = way.replace(/\//g, "_").toLowerCase();
            key = para == '' ? 'nothing' : JSON.stringify(para).toLowerCase();
            key = $api.md5(key.replace(/\{|\}|\"/g, "").replace(/:/g, "_"));
            if (active.length > 0 && active.substring(0, 1) == "+") {
                compel = true;
                active = active.substring(1);
            }
            //缓存时长
            var duration = 10;
            if (active != '' && !isNaN(Number(active))) {
                duration = Number(active);
                active = '';
            }
            var time = new Date();
            time.setMinutes(time.getMinutes() + duration);
            //缓存时长和过期时间
            var expires = {
                'duration': duration,
                'time': time,
                'timestring': time.format('yyyy-MM-dd HH:mm:ss')
            }
            //处理value
            if (value != null) {
                //删除多余值
                for (var v in value) {
                    if (v == 'data' || v == 'status') continue;
                    delete value[v];
                }
                //进一步删除多余值
                if (!!value['data']) {
                    var retain = 'result,success,size,index,total,totalpages'.split(',');
                    //console.log(retain);
                    for (var d in value['data']) {
                        if (retain.includes(d)) continue;
                        delete value['data'][d];
                    }
                }
            }
            return { way: way, para: para, store: name, active: active, compel: compel, expires: expires, keyid: key, value: value };
        },
        //获取某个存储空间下的所有记录
        getall: function (store) {
            var th = this;
            return new Promise(function (resolve, reject) {
                function _getall(storeName, callback) {
                    var request = window.indexedDB.open(th.dbname);
                    var db = null;
                    request.onsuccess = function (event) {
                        db = event.target.result;
                        if (!db.objectStoreNames.contains(storeName)) {
                            var err = th.error(0, 'onsuccess', event.target, '存储空间"' + storeName + '"不存在', db, storeName);
                            reject(err);
                            return;
                        }
                        var transaction = db.transaction(storeName);
                        var store = transaction.objectStore(storeName);
                        var storeget = store.openCursor();
                        let data = [];
                        storeget.onsuccess = function (e) {
                            let result = e.target.result;
                            if (result && result !== null) {
                                data.push(result.value);
                                result.continue();
                            } else {
                                if (callback) callback(data);
                                if (db) db.close();
                            }
                        };
                    };
                }
                _getall(store, function (data) {
                    resolve(data);
                })
            });
        },
        //所有存储空间
        stores: function () {
            var th = this;
            return new Promise(function (resolve, reject) {
                var request = window.indexedDB.open(th.dbname);
                request.onsuccess = function (event) {
                    var db = event.target.result;
                    resolve(db.objectStoreNames);
                    if (db) db.close();
                };
                request.onerror = function (event) {
                    var err = th.error(0, 'onerror', event.target, '数据库"' + th.dbname + '"打开失败', th.dbname, null);
                    reject(err);
                };
            });
        },
        //清理某个存储空间下的过期缓存
        clear: function (store) {
            var th = this;
            if (store) {
                th.getall(store).then(function (items) {
                    if (items.length < 1) return;
                    for (var i = 0; i < items.length; i++) {
                        const item = items[i];
                        if (item.expires.time < new Date()) {
                            th.del(item.way, item.para);
                        }
                    }
                });
            } else {
                th.stores().then(function (d) {
                    for (var i = 0; i < d.length; i++) {
                        th.clear(d[i]);
                    }
                });
            }
        },
        //清空所有缓存
        //store:存储空间名称；compel：true保留强制缓存，false或为空时连强制缓存也清除
        reset: function (store, compel) {
            var th = this;
            if (store) {
                //保留强制缓存
                if (compel) {
                    th.getall(store).then(function (items) {
                        for (var i = 0; i < items.length; i++) {
                            if (items[i].compel) continue;
                            th.del(items[i].way, items[i].para);
                        }
                    });
                } else {
                    var request = window.indexedDB.open(th.dbname);
                    request.onsuccess = function (event) {
                        var db = event.target.result;
                        var store = db.transaction(store, 'readwrite').objectStore(store);
                        store.clear();
                        db.close();
                    };
                }
            } else {
                th.stores().then(function (d) {
                    for (var i = 0; i < d.length; i++) {
                        th.reset(d[i], compel);
                    }
                });
            }
        },
        //错误信息的返回值      
        error: function (state, event, target, message, database, value) {
            return {
                'state': state,     //状态，0不存在，-1为其它
                'event': event,     //触发错误的事件名称，例如onsuccess
                'target': target,        //触发事件的对象
                'message': message,     //自定义错误信息
                'db': database,         //数据库对象
                'value': value          //传值
            }
        }
    };
    apiObj.prototype.loadxml = function (xmlString) {
        if (typeof (xmlString) != 'string') return xmlString;
        var xmlDoc = null;
        //判断浏览器的类型
        if (!window.DOMParser && window.ActiveXObject) {   //window.DOMParser 判断是否是非ie浏览器
            var xmlDomVersions = ['MSXML.2.DOMDocument.6.0', 'MSXML.2.DOMDocument.3.0', 'Microsoft.XMLDOM'];
            for (var i = 0; i < xmlDomVersions.length; i++) {
                try {
                    xmlDoc = new ActiveXObject(xmlDomVersions[i]);
                    xmlDoc.async = false;
                    xmlDoc.loadXML(xmlString); //loadXML方法载入xml字符串
                    break;
                } catch (e) { }
            }
        }
        //支持Mozilla浏览器
        else if (window.DOMParser && document.implementation && document.implementation.createDocument) {
            try {
                domParser = new DOMParser();
                xmlDoc = domParser.parseFromString(xmlString, 'text/xml');
            } catch (e) { }
        }
        else {
            return null;
        }
        return xmlDoc;
    };
    //创建$api调用对象
    for (var v in config.versions) {
        var str = config.versions[v] == "" ?
            "window.$api = new apiObj();" :
            "window.$api.v" + config.versions[v] + "= new apiObj('" + config.versions[v] + "')";
        eval(str);
    }
})();

//异步获取数据的示例
//$api.get("/dd/xx");
/* $api.v1.post("/dd/xx",{id:1}).then(function(req){
.....
});*/

/*一次获取多个数据
$api.bat(
    $api.get("Outline/TreeList", { couid: $api.querystring("couid") }),
    $api.get("Course/ForID", { id: $api.querystring("couid") })
).then(axios.spread(function (req, cur) {
    if (req.data.success) {
        var outlines = req.data.result;
    }
}));
*/
//日期格式化
Date.prototype.format = function (fmt) {

    var fmtfuc = function (fmt, date) {
        fmt = fmt.replace(/\Y/g, "y");
        //24小时制
        var h24 = date.toLocaleString();
        try {
            h24 = date.toLocaleString('chinese', {
                hour12: false
            });
        } catch (e) { }
        h24 = h24.substring(h24.indexOf(' ') + 1, h24.indexOf(':'));
        h24 = h24 == '24' ? '0' : h24;
        //12小时制
        var h12 = date.toLocaleString();
        try {
            h12 = date.toLocaleString('chinese', {
                hour12: true
            });
        } catch (e) { }
        h12 = h12.substring(h12.indexOf(' ') + 1, h12.indexOf(':'));
        //星期
        var week = ['天', '一', '二', '三', '四', '五', '六'];
        //
        var ret;
        var opt = {
            "yyyy": date.getFullYear().toString(), // 年
            "yy": date.getFullYear().toString().substring(2),
            "M+": (date.getMonth() + 1).toString(), // 月
            "d+": date.getDate().toString(), // 日
            "w+": week[date.getDay()], // 星期
            "H+": h24, //小时
            "h+": h12,
            "m+": date.getMinutes().toString(), // 分
            "s+": date.getSeconds().toString() // 秒			
        };
        for (var k in opt) {
            ret = new RegExp("(" + k + ")").exec(fmt);
            if (ret) {
                fmt = fmt.replace(ret[1], (ret[1].length == 1) ? (opt[k]) : (opt[k].padStart(ret[1].length, "0")))
            };
        };
        return fmt;
    }
    return fmtfuc(fmt, this);
};

//日期字符串解析为日期对象
Date.parse = function (str) {
    //如果是数值
    if (typeof (str) == 'number') {
        return new Date(str);
    }
    //如果本身就是日期对象
    if (str instanceof Date) {
        return str;
    }
    var date = '', time = '';
    str = str.replace(/\//g, "-");
    if (str.indexOf(' ') > -1) {
        date = str.substring(0, str.indexOf(' '));
        time = str.substring(str.lastIndexOf(' ') + 1);
    } else {
        if (str.indexOf('-') > -1) date = str;
        if (str.indexOf(':') > -1) {
            date = new Date().format('yyyy-MM-dd');
            time = str;
        }
    }
    var dateStrs = date.split('-');
    var year = parseInt(dateStrs[0], 10);
    var month = parseInt(dateStrs[1], 10) - 1;
    var day = parseInt(dateStrs[2], 10);
    var timeStrs = time.split(':');
    var hour = parseInt(timeStrs[0], 10);
    var minute = parseInt(timeStrs[1], 10);
    var second = parseInt(timeStrs[2], 10);
    second = isNaN(second) ? 0 : second;
    return new Date(year, month, day, hour, minute, second);
}
Date.prototype.addmonth = function (n) {
    var dt = this;
    var yy = dt.getYear();
    var mm = dt.getMonth();
    dt.setMonth(dt.getMonth() + n);
    if ((dt.getYear() * 12 + dt.getMonth()) > (yy * 12 + mm + n)) {
        dt = new Date(dt.getYear(), dt.getMonth(), 0);
    }
    var year = dt.getYear();
    var month = dt.getMonth() + 1;
    var days = dt.getDate();
    var dd = year + "-" + month + "-" + days;
    return dd;
};
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g, function (m, i) {
        return args[i];
    });
};
//将数值转金额的格式，即三位一个逗号
//len:小数字后的长度，默认是两位
Number.prototype.money = function (len) {
    var str = String(this);
    //小数部分
    var float_num = 0;
    if (len == null) len = 2; 
    if (str.indexOf('.') > -1) {
        var f = str.substring(str.indexOf('.') + 1);
        var j = 0, s = '';
        while (len-- > 0) s += f.substring(j++, j);
        float_num = isNaN(Number(s)) ? 0 : Number(s);       
    }
    str = str.indexOf('.') > -1 ? str.substring(0, str.indexOf('.')) : str;
    //整数部分，每三位加一个逗号
    var mstr = '', n = 0;
    for (let i = str.length - 1; i >= 0; i--) {
        mstr = str.substring(i, i + 1) + mstr;
        if (++n % 3 == 0 && n != str.length) mstr = ',' + mstr;
    }
    return float_num > 0 ? mstr + '.' + float_num : mstr;
};
//添加加载前后的事件
$api.effect(function () {

}, function (response, err) {
    if (response == null) {
        console.error(err);
        return;
    }
    if (response.data.state == '94060') {
        alert('数据库链接异常');
        return;
    }
    if (!response.config) return;
    //方法
    var method = response.config.method;
    //请求网址
    var url = response.config.url;
    url = url.substring(url.indexOf('/v1/') + 3);
    //请求参数
    var para = JSON.stringify(response.config.params);
    if (!para) para = JSON.stringify(response.config.parameters);
    para = para == undefined ? '' : para;
    //时长
    var exec = response.data.execspan;
    var span = response.data.webspan;
    console.log(method + '-' + url + ':' + para + ' 用时 ' + span + ' 毫秒，服务端 ' + exec + ' 毫秒');
    //console.log(response);
});

(function () {
    if (!window.localStorage) {
        console.log('浏览器不支持localStorage');
    }
    var size = 0;
    for (item in window.localStorage) {
        if (window.localStorage.hasOwnProperty(item)) {
            size += window.localStorage.getItem(item).length;
        }
    }
    console.log('当前localStorage容量为' + (size / 1024).toFixed(2) + 'KB');

})();

//版权信息的展现
//示例： <span copyright="powerby"></span> html属性增加copyright，其值为copyrigth.weisha中的节点；
//示例： <a copyright="url"></a>，超链接的href属性将显示copyright中的url
window.addEventListener("load", function () {
    $api.get('Copyright/Info').then(function (req) {
        if (req.data.success) {
            var copyright = req.data.result;
            var nodes = document.querySelectorAll("*[copyright]");
            for (var i = 0; i < nodes.length; i++) {
                var node = nodes[i];
                var name = node.tagName.toLowerCase();
                var val = node.getAttribute("copyright");
                for (var attr in copyright) {
                    if (attr == val) {
                        var txt = copyright[attr];
                        switch (name) {
                            case "a":
                                node.setAttribute("href", txt);
                                break;
                            case "img":
                                node.setAttribute("src", txt);
                                break;
                            default:
                                node.innerText = txt;
                        }
                    }
                }
            }
        }
    }).catch(function (err) {

    });
});