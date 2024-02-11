window.menvue = new Vue({
    el: '#menu',
    data: {
        apisearch: '',
        error: '',       //错误信息
        list: [], //接口列表   
        //总数，接口数、接口方法数
        total: {
            apiobj: 0,
            method: 0
        },
        loading: false
    },
    methods: {
        homeClick: function () {
            rvue.method = null;
        }
    },
    computed: {
        // 计算属性的 getter
        apilist: function () {
            var search = this.apisearch.toLowerCase();
            if (search == '') return this.list;
            var arr = new Array();
            for (var i = 0; i < this.list.length; i++) {
                var item = this.list[i];
                if (item.Name.toLowerCase().indexOf(search) > -1 || item.Intro.indexOf(search) > -1) {
                    arr.push(this.list[i]);
                } else {
                    // if (!!this.list[i].methods) {
                    var methods = item.methods;
                    for (var j = 0; j < methods.length; j++) {
                        if (methods[j].Name.toLowerCase().indexOf(search) > -1 || methods[j].Intro.indexOf(search) > -1) {
                            arr.push(item);
                            break;
                        }
                    }
                    // }
                }
            }
            return arr;
        }
    },
    methods: {
        //判断是否满足查询条件
        searhState: function (item) {
            var search = this.apisearch.toLowerCase();
            if (search == '') return true;

            if (item.Name.toLowerCase().indexOf(search) > -1 || item.Intro.indexOf(search) > -1) {
                return true;
            } else {
                var methods = item.methods;
                for (var j = 0; j < methods.length; j++) {
                    if (methods[j].Name.toLowerCase().indexOf(search) > -1 || methods[j].Intro.indexOf(search) > -1) {
                        return true;
                    }
                }
            }
            return false;
        },
        //获取接口列表
        //usecache:是否启用缓存
        getapi: function (usecache) {
            var th = this;
            th.loading = true;
            var way = "helper/apiList";
            way += usecache != null && usecache ? ':' + (60 * 60 * 24) : ':update';
            $api.cache(way).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.list = req.data.result;
                    th.total.apiobj = th.list.length;
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                th.error = err;
                console.error(err);
                alert(err);
            });
        }
    },
    created: function () {
        this.getapi(true);
        window.setTimeout(function () {
            document.getElementById("apisearch").focus();
        }, 1000);
        /*
        var items = $api.api_cache.getall("helper/Methods:update");
        items.then(function (d) {
            console.log(d);
        });
        $api.api_cache.stores().then(function (d) {
            console.log(d);
            for (var i = 0; i < d.length; i++) {
                console.log(d[i]);
            }
        });
        $api.api_cache.reset()
        */
        //$api.api_cache.clear();
    }
});
//menvue.$mount('#menu');

// 注册组件
Vue.component('methods', {
    // 声明 props，用于向组件传参
    //apiobject:接口对象
    props: ['apiobject', 'search', 'index'],
    data: function () {
        return {
            methods: [], //方法列表
            loading: true, //预载中
            open: false //是否打开方法列表
        }
    },
    computed: {
        list: function () {
            var sear = this.search.toLowerCase();
            if (sear == '') return this.methods;
            var regExp = new RegExp(sear, 'ig');
            var arr = $api.clone(this.methods);
            for (var i = 0; i < arr.length; i++) {
                arr[i].Name = arr[i].Name.replace(regExp, `<red>${sear}</red>`);
                arr[i].Intro = arr[i].Intro.replace(regExp, `<red>${sear}</red>`);
            }
            return arr;
        }
    },
    methods: {
        //方法的点击事件
        methodClick: function (method) {
            rvue.method = method;
            rvue.helpshow = false;
        },
        //刷新事件
        btnfresh: function () {
            this.loading = true;
            window.menvue.total.method -= this.methods.length;
            this.methods = [];
            this.apiobject["methods"] = [];
            this.getmethods(false);
        },
        //获取接口方法的列表
        getmethods: function (usecache) {
            var th = this;
            var name = th.apiobject.Name;
            var way = "helper/apiMethods";
            way += usecache != null && usecache ? ':' + (60 * 60 * 24) : ':update';
            $api.cache(way, { classname: name }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.methods = req.data.result;
                    th.apiobject["methods"] = req.data.result;
                    window.menvue.total.method += th.methods.length;
                }
            }).catch(function (err) {
                th.loading = false;
            }).finally(() => th.loading = false);
        }
    },
    created: function () {
        this.getmethods(true);
    },
    // 
    template: `<div>
      <div class='classname' v-on:click='open=!open' :title='\"摘要：\"+apiobject.Intro'>
            {{index}}. <b class='el-icon-loading' v-show='loading'></b>{{ apiobject.Name }} 
            <span>{{apiobject.Intro}}</span>
            <i class='el-icon-arrow-right' :class='{show:open}'></i>
       </div>
       <dl class='methods' :class='{show:open}'>
       <dt v-if='apiobject.Intro.length>0'>摘要：{{apiobject.Intro}}
        <span class='fresh' @click='btnfresh' v-show='!loading' title='刷新'></span>
       </dt>
            <dd v-for='(item,i) in list' v-on:click='methodClick(item)'>
                <name>{{index}}.{{i+1}}.<span v-html='item.Name'></span></name>
                <intro v-show='item.Intro.length>0' v-html='item.Intro'></intro>
            </dd>
       </dl>
       </div>`
});
/*
 
 */
//右侧的内容区
var rvue = new Vue({
    el: '#context',
    data: {
        method: null, //接口方法对象
        loading: false, //调用接口时的状态
        showcode: false, //显示代码
        helpshow: true
    },
    watch: {
        method: function (n, o) {
            var ele = document.getElementById("testresult");
            if (ele != null) ele.innerText = "";
            var inputs = Array.from(document.querySelectorAll("#context table input"));
            inputs.forEach(function (item) {
                item.value = "";
            });
            window.setTimeout(function () {
                rvue.teststring();
            }, 200);
        }
    },
    computed: {
        parameter: function () {
            let fullname = this.method.FullName;
            let name = this.method.Name;
            if (fullname.indexOf('(') > -1) {
                let paras = fullname.substring(fullname.indexOf('(') + 1).split(',');
                let str = '';
                for (let t = 0; t < paras.length; t++) {
                    if (paras[t].indexOf('.') > -1) {
                        str += paras[t].substring(paras[t].lastIndexOf('.') + 1);
                        if (t < paras.length - 1) str += ',';
                    }
                }
                return '(' + str.toLowerCase();
            }
            return '';
        }
    },
    methods: {
        testapi: function () {
            var method = this.method.ClassName + "/" + this.method.Name;
            method = this.clearhtml(method);
            var params = this.getInputPara();
            var http = document.getElementById("httppre").value;
            var rettype = document.getElementById("returntype").value;
            //调用
            $api.query(method, eval("(" + params + ")"), http, function () {
                rvue.loading = true;
            }, function () {
                window.setTimeout(function () {
                    rvue.loading = false;
                }, 500);
            }, rettype).then(function (req) {
                var ele = document.getElementById("testresult");
                if (req == null) throw {
                    message: '没有获取到返回值，可能是服务器端错误'
                };
                console.log(req);
                var result = JSON.stringify(req.data.result);
                if (req.config && req.config.returntype == "xml")
                    ele.innerText = $api.trim(rvue.xmlformat(unescape(req.data)));
                if (!req.config || req.config.returntype == "json")
                    ele.innerText = $api.trim(rvue.jsonformat(unescape(JSON.stringify(req.data)), true));

            }).catch(function (ex) {
                rvue.loading = false;
                var ele = document.getElementById("testresult");
                ele.innerText = ex.message;
            });
        },
        //生成测试代码
        teststring: function () {
            let method = this.method.ClassName + "/" + this.method.Name;
            method = this.clearhtml(method);
            let params = this.getInputPara();
            let http = document.getElementById("httppre").value;
            //语句
            let urlstr = window.location.protocol + "//" + window.location.host + "/api/v" + $api.version + "/" + method;
            document.getElementById("apiurl").innerText = urlstr.toLowerCase();
            let jsstr = "$api." + http + "('" + method + "'" + (params == "{}" ? "" : "," + params) + ")";
            jsstr += ".then(function(req){\r\
            if(req.data.success){\r\
                var result=req.data.result;\r\
                //...\r\
            }else{\r\
                console.error(req.data.exception);\r\
                throw req.config.way + ' ' + req.data.message;\r\
            }\r}).catch(err=>console.error(err))\r\
                .finally(()=>{});";
            //jsstr = rvue.jsonformat(jsstr, false);
            document.getElementById("teststring").innerText = jsstr;
            return jsstr;
        },
        //复制测试代码
        btnCopyEvent: function () {
            this.copy(this.teststring(), 'textarea');
        },
        //复制api路径
        copyApipath: function (clname, method) {
            let path = this.clearhtml(clname + '/' + method);
            this.copy(path);
        },
        //复制到粘贴板
        copy: function (val, textbox) {
            if (textbox == null) textbox = 'input';
            let oInput = document.createElement(textbox);
            oInput.value = val;
            document.body.appendChild(oInput);
            oInput.select(); // 选择对象
            document.execCommand("Copy"); // 执行浏览器复制命令           
            oInput.style.display = 'none';
            this.$message({
                message: '复制 “' + val + '” 到粘贴板',
                type: 'success'
            });
        },
        //获取录入的参数
        getInputPara: function () {
            let arr = new Array();
            let inputs = Array.from(document.querySelectorAll("#context table input"));
            inputs.forEach(function (item) {
                let name = item.getAttribute("name");
                let val = item.value;
                arr.push("'" + name + "':'" + val + "'");
            });
            let txt = "{";
            for (let i = 0; i < arr.length; i++) {
                txt += arr[i];
                if (i < arr.length - 1) txt += ",";
            }
            txt += "}";
            return txt;
        },
        //json字符格式化
        //isbrace:花括号是否换行，true换行
        jsonformat: function (json, isbrace) {
            var formatted = '', //转换后的json字符串
                padIdx = 0, //换行后是否增减PADDING的标识
                PADDING = '    '; //4个空格符
            /**
             * 将对象转化为string
             */
            if (typeof json !== 'string') {
                json = JSON.stringify(json);
            }
            /** 
             *利用正则类似将{'name':'ccy','age':18,'info':['address':'wuhan','interest':'playCards']}
             *---> \r\n{\r\n'name':'ccy',\r\n'age':18,\r\n
             *'info':\r\n[\r\n'address':'wuhan',\r\n'interest':'playCards'\r\n]\r\n}\r\n
             */
            json = json.replace(/(\,)/g, '$1\r\n')
                //.replace(/([\[\]])/g, '\r\n$1\r\n')               
                .replace(/(\r\n\r\n)/g, '\r\n')
                .replace(/\r\n\,/g, ',');
            if (isbrace) json = json.replace(/([\{\}])/g, '\r\n$1\r\n');
            /** 
             * 根据split生成数据进行遍历，一行行判断是否增减PADDING
             */
            (json.split('\r\n')).forEach(function (node, index) {
                var indent = 0,
                    padding = '';
                if (node.match(/\{$/) || node.match(/\[$/)) indent = 1;
                else if (node.match(/\}/) || node.match(/\]/)) padIdx = padIdx !== 0 ? --padIdx : padIdx;
                else indent = 0;
                for (var i = 0; i < padIdx; i++) padding += PADDING;
                formatted += padding + node + '\r\n';
                padIdx += indent;
                //console.log('index:' + index + ',indent:' + indent + ',padIdx:' + padIdx + ',node-->' + node);
            });
            //formatted=formatted.replace(/\r\n/g, '<br/>');
            return formatted;
        },
        //xml格式化
        xmlformat: function (text) {
            //计算头函数 用来缩进
            function setPrefix(prefixIndex) {
                var result = '';
                var span = '    '; //缩进长度
                var output = [];
                for (var i = 0; i < prefixIndex; ++i) {
                    output.push(span);
                }
                result = output.join('');
                return result;
            }
            //使用replace去空格
            text = '\n' + text.replace(/(<\w+)(\s.*?>)/g, function ($0, name, props) {
                return name + ' ' + props.replace(/\s+(\w+=)/g, " $1");
            }).replace(/>\s*?</g, ">\n<");
            //处理注释
            text = text.replace(/\n/g, '\r').replace(/<!--(.+?)-->/g, function ($0, text) {
                var ret = '<!--' + escape(text) + '-->';
                return ret;
            }).replace(/\r/g, '\n');
            //调整格式  以压栈方式递归调整缩进
            var rgx = /\n(<(([^\?]).+?)(?:\s|\s*?>|\s*?(\/)>)(?:.*?(?:(?:(\/)>)|(?:<(\/)\2>)))?)/mg;
            var nodeStack = [];
            var output = text.replace(rgx, function ($0, all, name, isBegin, isCloseFull1, isCloseFull2, isFull1, isFull2) {
                var isClosed = (isCloseFull1 == '/') || (isCloseFull2 == '/') || (isFull1 == '/') || (isFull2 == '/');
                var prefix = '';
                if (isBegin == '!') { //!开头
                    prefix = setPrefix(nodeStack.length);
                } else {
                    if (isBegin != '/') { ///开头
                        prefix = setPrefix(nodeStack.length);
                        if (!isClosed) { //非关闭标签
                            nodeStack.push(name);
                        }
                    } else {
                        nodeStack.pop(); //弹栈
                        prefix = setPrefix(nodeStack.length);
                    }
                }
                var ret = '\n' + prefix + all;
                return ret;
            });
            var prefixSpace = -1;
            var outputText = output.substring(1);
            //还原注释内容
            outputText = outputText.replace(/\n/g, '\r').replace(/(\s*)<!--(.+?)-->/g, function ($0, prefix, text) {
                if (prefix.charAt(0) == '\r')
                    prefix = prefix.substring(1);
                text = unescape(text).replace(/\r/g, '\n');
                var ret = '\n' + prefix + '<!--' + text.replace(/^\s*/mg, prefix) + '-->';
                return ret;
            });
            outputText = outputText.replace(/\s+$/g, '').replace(/\r/g, '\r\n');
            return outputText;
        },
        //清理Html标签
        clearhtml: str => str.replace(/(<([^>]+)>)/ig, ""),
        //显示文本
        showintro: txt => {
            txt=txt.replace(/\s+/g, " ");
            txt=txt.replace(/\r/g, " ");
            txt=txt.replace(/\n/g, " ");
            return txt.replace(/ /g, '<br/>');
        }

    },
    created: function () {

    }
});
