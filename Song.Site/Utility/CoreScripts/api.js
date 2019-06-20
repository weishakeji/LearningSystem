/*!
* 主  题：API调用方法
* 说  明：用于调用Restful api接口的js方法。
* 功能描述：
* 1、封闭api调用方法，采用axios异步读取服务端；
* 2、
*
*
* 作  者：宋雷鸣 10522779@qq.com
* 开发时间: 2019年5月20日
*/
(function () {
    var config = {
        //api的版本号
        versions: ["", "1", "2"],
        versionDefault: "1",    //默认版本号
        //调用地址的根路径
        baseURL: '',
        pathUrl: "/api/v{0}/",  //url路径
    }
    //一些常用方法
    var methods = {
        //生成axios调用的路径,
        //vesion:api版本号,
        //way:api方法名，如/api/v1/[account/del]
        url: function (version, way) {
            if (way === undefined) throw 'api名称不得为空';
            var url = config.pathUrl.replace("{0}", version);
            //调用地址的根路径可以在此处理，（如果需要跨多台服务器请求的话）
            if (config.baseURL != '') url = config.baseURL + url;
            return url + way;
        },
        //获取url中的参数
        getpara: function (url, key) {
            if (arguments.length == 1) {
                key = arguments[0];
                url = String(window.document.location.href);
            }
            var value = "";
            if (url.indexOf("?") > -1) {
                var ques = url.substring(url.lastIndexOf("?") + 1);
                var tm = ques.split('&');
                for (var i = 0; i < tm.length; i++) {
                    var arr = tm[i].split('=');
                    if (arr.length < 2) continue;
                    if (key.toLowerCase() == arr[0].toLowerCase()) {
                        value = arr[1];
                        break;
                    }
                }
            }
            if (value.indexOf("#") > -1) value = value.substring(0, value.indexOf("#"));
            return value;
        }
    }
    //api操作的具体对象和方法
    var apiObj = function (version) {
        //加载效果，参数：前者为一般loading效果，后者一般为去除loading
        this.effect = function (loading, loaded) {
            this.loadeffect.before = loading;
            this.loadeffect.after = loaded;
            return this;
        }
        this.loadeffect = { before: null, after: null };
        //当前api要请求的服务端接口的版本号
        this.version = version == null ? config.versionDefault : version;
        var httpverb = ['get', 'post', 'delete', 'put', 'patch', 'options'];
        for (let i = 0; i < httpverb.length; i++) {
            var el = httpverb[i];
            eval("this." + el + " = function (way, parameters,loading,loaded) {return this.query(way, parameters, '" + el + "',loading,loaded);};");
        }
        var self = this;
        //创建请求对象，以及拦截器
        this.query = function (way, parameters, method, loading, loaded) {
            var url = methods.url(this.version, way);
            if (arguments.length < 2 || parameters == null) parameters = {};
            if (arguments.length < 3 || method == null || method == '') method = 'get';
            //创建axiso对象
            var instance = axios.create({
                timeout: 6000, url: url, method: method,
                headers: {
                    'X-Custom-Header': 'weishakeji',
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Headers': 'X-Requested-With',
                    'content-type': 'application/x-www-form-urlencoded',
                    'Access-Control-Allow-Methods': 'POST,GET,DELETE,PUT,PATCH,HEAD,OPTIONS'
                }
            });
            //添加请求拦截器（即请求之前）
            instance.interceptors.request.use((config) => {
                if (loading == null) loading = self.loadeffect.before;
                if (loading != null) loading(config);
                //在发送请求之前做某件事
                if (config.method != 'get') {
                    var formData = new FormData();
                    for (var d in config.data) {
                        formData.append(d, escape(config.data[d]));
                    }
                    config.data = formData;
                } else {
                    for (var d in config.params) {
                        config.params[d] = escape(config.params[d]);
                    }
                }
                return config;
            }, (error) => {
                console.log('错误的传参');
                if (loaded == null) loaded = self.loadeffect.after;
                if (loading != null) loaded(config, error);
                return Promise.reject(error);
            });
            //添加响应拦截器（即返回之后）
            instance.interceptors.response.use(function (response) {
                //如果返回的数据是字符串，这里转为json
                try {
                    if (typeof (response.data) == 'string') {
                        response.data = eval("(" + response.data + ")");
                    }
                } catch (err) {
                    var resultdata = { 'success': true, 'Data': response.data };
                    response.data = resultdata;
                }
                //执行加载完成后的方法
                if (loaded == null) loaded = self.loadeffect.after;
                if (loaded != null) loaded(response, null);
                return response;
            }, function (error) {
                if (loaded == null) loaded = self.loadeffect.after;
                if (loaded != null) loaded(response, error);
                // 对响应错误做点什么
                return Promise.reject(error);
            });
            //如果是get方式，参数名是params；非get参数名是data
            if (method == 'get') {
                return instance.request({ params: parameters });
            } else {
                return instance.request({ data: parameters });
            }
        }
        this.method = methods;
    };
    //创建$api调用对象
    for (var v in config.versions) {
        var str = config.versions[v] == "" ?
            "window.$api = new apiObj();" :
            "window.$api.v" + config.versions[v] + "= new apiObj('" + config.versions[v] + "')";
        eval(str);
    }
})();
//$api.get("/dd/xx");
//$api.v1.post();
//$api.v2.delete();