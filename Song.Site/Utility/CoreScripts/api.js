/*!
 * 主  题：API调用方法
 * 说  明：用于调用Restful api接口的js方法。
 * 功能描述：
 * 1、封装api调用方法，采用axios异步读取服务端；支持get,post,delete,put,patch,options六种方式
 * 2、数据提交与返回均经过url编码与解码
 * 3、一些常用方法，如$api.trim，querystring,cookies等
 *
 *
 * 作  者：宋雷鸣 10522779@qq.com
 * 开发时间: 2019年5月20日
 */
(function() {
    var config = {
        //api的版本号
        versions: ["", "1", "2"],
        versionDefault: "1", //默认版本号
        //调用地址的根路径
        baseURL: '',
        pathUrl: "/api/v{0}/", //url路径
    };
    //版权信息
    var copyright = {};
    //一些常用方法
    var methods = {
        //生成axios调用的路径,
        //vesion:api版本号,
        //way:api方法名，如/api/v1/[account/del]
        url: function(version, way) {
            if (way === undefined) throw 'api名称不得为空';
            var url = config.pathUrl.replace("{0}", version);
            //调用地址的根路径可以在此处理，（如果需要跨多台服务器请求的话）
            if (config.baseURL != '') url = config.baseURL + url;
            return url + way;
        },
        //获取url中的参数
        querystring: function(url, key) {
            if (arguments.length == 1) key = arguments[0];
            if (arguments.length <= 1) url = String(window.document.location.href);
            if (url.indexOf("?") < 0) return "";
            //取所有参数
            var values = new Array();
            var query = url.substring(url.lastIndexOf("?") + 1).split('&');
            for (var q in query) {
                var arr = query[q].split('=');
                if (arr.length < 2) continue;
                if (arr[1].indexOf("#") > -1) arr[1] = arr[1].substring(0, arr[1].indexOf("#"));
                arr[1] = decodeURI(arr[1]).replace(/<[^>]+>/g, "");
                values.push({
                    key: arr[0],
                    val: arr[1]
                });
            }
            //返回
            if (arguments.length == 0) return values;
            if (arguments.length == 1) {
                for (var q in values) {
                    if (values[q].key.toLowerCase() == key.toLowerCase())
                        return values[q].val;
                }
            }
            return "";
        },
        setpara: function(key, value) {
            //获取所有参数
            var values = methods.querystring();
            var isExist = false;
            for (var q in values) {
                if (values[q].key.toLowerCase() == key.toLowerCase()) {
                    values[q].val = value;
                    isExist = true;
                }
            }
            if (!isExist) values.push({
                key: key,
                val: value
            });
            //拼接Url      
            var url = String(window.document.location.href);
            if (url.indexOf("?") > -1) url = url.substring(0, url.lastIndexOf("?"));
            var parastr = "";
            for (var i = 0; i < values.length; i++) {
                parastr += values[i].key + "=" + values[i].val;
                if (i < values.length - 1) parastr += "&";
            }
            return url + "?" + parastr;
        },
        //去除两端空格
        trim: function(str) {
            return str.replace(/^\s*|\s*$/g, '').replace(/^\n+|\n+$/g, "");
        },
        //将数据url解码
        unescape: function(data) {
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
                        data[key] = unescape(data[key]);
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
                for (var d in data) {
                    data[d] = handleObject(data[d]);
                }
                return data;
            }
            return data;
        },
        //判断数据类型
        getType: function(data) {
            var getType = Object.prototype.toString;
            var myType = getType.call(data); //调用call方法判断类型，结果返回形如[object Function]  
            var typeName = myType.slice(8, -1); //[object Function],即取除了“[object ”的字符串。 
            return typeName;
        },
        //storage存储
        storage: function(key, value) {
            var isAndroid = (/android/gi).test(navigator.appVersion);
            var uzStorage = function() {
                var ls = window.localStorage;
                if (isAndroid) ls = window.localStorage;
                return ls;
            };
            //如果只有一个参数，为读取
            if (arguments.length === 1) {
                var ls = uzStorage();
                if (ls) {
                    var v = ls.getItem(key);
                    if (!v) return;
                    if (v.indexOf('obj-') === 0) {
                        v = v.slice(4);
                        return JSON.parse(v);
                    } else if (v.indexOf('str-') === 0) {
                        return v.slice(4);
                    }
                }
            }
            //如果两个参数，为写入，第一个为键，第二个为值
            if (arguments.length === 2) {
                if (value != null) {
                    var v = typeof value == 'object' ? 'obj-' + JSON.stringify(value) : 'str-' + value;
                    var ls = uzStorage();
                    if (ls) ls.setItem(key, v);
                } else {
                    var ls = uzStorage();
                    if (ls && key) {
                        ls.removeItem(key);
                    }
                }
            }
        },
        cookie: function(name, value, options) {
            if (typeof value != 'undefined') { // name and value given, set cookie 
                options = options || {};
                if (value === null) {
                    value = '';
                    options.expires = -1;
                }
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
                        var cookie = jQuery.trim(cookies[i]);
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
        //在线浏览pdf文件
        pdfViewer: function(file) {
            var viewer = "/Utility/PdfViewer/viewer.html";
            if (file.indexOf("?") > -1) file = file.substring(0, file.indexOf("?"));
            viewer += "?file=" + encodeURIComponent(file);
            //window.location.href = viewer;
            return viewer;
        }
    };
    //api操作的具体对象和方法
    var apiObj = function(version) {
        //加载效果，参数：前者为一般loading效果，后者一般为去除loading
        this.effect = function(loading, loaded) {
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
        var httpverb = ['get', 'post', 'delete', 'put', 'patch', 'options'];
        for (var i = 0; i < httpverb.length; i++) {
            var el = httpverb[i];
            var tm = "this." + el + " = function (way, parameters,loading,loaded) {return this.query(way, parameters, '" + el + "',loading,loaded,'json');}";
            eval(tm);
        }
        var self = this;
        //创建请求对象，以及拦截器
        //way:接口方法
        //returntype:返回数据的类型，Json或xml
        this.query = function(way, parameters, method, loading, loaded, returntype) {
            var url = methods.url(this.version, way);
            if (arguments.length < 2 || parameters == null) parameters = {};
            if (arguments.length < 3 || method == null || method == '') method = 'get';
            //开始时间
            var startTime = new Date();
            //console.log(startTime);
            //创建axiso对象
            var instance = axios.create({
                method: method != 'get' ? 'post' : 'get',
                url: url,
                headers: {
                    'X-Custom-Header': 'weishakeji',
                    'Access-Control-Allow-Headers': 'X-Requested-With',
                    'content-type': 'application/x-www-form-urlencoded',
                    'Access-Control-Allow-Methods': 'POST,GET,DELETE,PUT,PATCH,HEAD,OPTIONS'
                },
                auth: {
                    username: 'weishakeji ' + method + ' ' + returntype + ' ' + window.location,
                    password: 'token'
                },
                timeout: 60 * 1000,
                returntype: returntype
            });
            //添加请求拦截器（即请求之前）
            instance.interceptors.request.use(function(config) {
                if (loading == null) loading = self.loadeffect.before;
                if (loading != null) loading(config);
                //在发送请求之前做某件事
                if (config.method != 'get') {
                    var formData = new FormData();
                    for (var d in config.data) {
                        var typeName = methods.getType(config.data[d]);
                        if (typeName == 'Date') {
                            formData.append(d, config.data[d].getTime());
                        } else {
                            formData.append(d, escape(config.data[d]));
                        }
                    }
                    config.data = formData;
                } else {
                    //克隆参数对象，因为上传的参数要escape转码，需要保留原数据类型
                    var tmpObj = new Object();
                    for (var d in config.params) {
                        var typeName = methods.getType(config.params[d]);
                        if (typeName == 'Date') {
                            tmpObj[d] = config.params[d].getTime();
                        } else {
                            tmpObj[d] = escape(config.params[d]);
                        }
                    }
                    config.params = tmpObj;
                }
                return config;
            }, function(error) {
                console.log('错误的传参');
                if (loaded == null) loaded = self.loadeffect.after;
                if (loading != null) loaded(config, error);
                //return Promise.reject(error);
            });
            //添加响应拦截器（即返回之后）
            instance.interceptors.response.use(function(response) {
                response.text = response.data;
                //如果返回的数据是字符串，这里转为json
                if (response.config.returntype == "json") {
                    if (typeof(response.data) == 'string') {
                        response.data = eval("(" + response.data + ")");
                    }
                    //处理数据，服务器端返回的数据是经过Url编码的，此处进行解码
                    if (response.data.result != null) {
                        if (typeof(response.data.result) == 'string') {
                            try {
                                response.data.result = eval("(" + response.data.result + ")");
                            } catch (err) {
                                //alert(err);
                            }
                        }
                        response.data.result = methods.unescape(response.data.result);
                    }
                }
                //计算执行耗时
                if (response.data) {
                    response.data['webspan'] = new Date() - startTime;
                }
                //执行加载完成后的方法
                if (loaded == null) loaded = self.loadeffect.after;
                if (loaded != null) loaded(response, null);
                return response;
            }, function(error) {
                if (loaded == null) loaded = self.loadeffect.after;
                if (loaded != null) loaded(null, error);
                //return Promise.reject(error);
            });
            //如果是get方式，参数名是params；非get参数名是data
            if (method == 'get') {
                return instance.request({
                    params: parameters
                });
            } else {
                return instance.request({
                    data: parameters
                });
            }
        }
        //一次获取多个数据
        this.all = function(queryArr) {
            if (arguments.length == 0) return null;
            if (arguments.length == 1) return queryArr;
            return axios.all(arguments);
        }
        //常用方法加到$api根，方便调用
        for (var m in methods) {
            eval("this." + m + "=" + methods[m] + ";");
        }
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
$api.all(
    $api.get("Outline/tree", { couid: $api.querystring("couid") }),
    $api.get("Course/ForID", { id: $api.querystring("couid") })
).then(axios.spread(function (req, cur) {
    if (req.data.success) {
        var outlines = req.data.result;
    }
}));
*/
//日期格式化
Date.prototype.format = function(fmt) {
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours()
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));

    return fmt;
}
//添加加载前后的事件
$api.effect(function() {

}, function(response, err) {
    //请求网址
    var url = response ? response.config.url : err.config.url;
    if(response==null){
        alert('"'+url+'",请求失败。message:'+err.message);
        return;
    }
    url=url.substring(url.indexOf('/v1/')+3);
    //请求参数
    var para = JSON.stringify(response.config.params);
    para = para == undefined ? '' : para;
    //时长
    var exec = response.data.execspan;
    var span = response.data.webspan;
    console.log(url + '' + para + ' 用时 ' + span + ' 毫秒，服务端 ' + exec + ' 毫秒');
    //console.log(response);
});