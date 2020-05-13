var menvue = new Vue({
    el: '#menu',
    data: {
        apisearch: '',
        list: [] //接口列表
    },
    methods: {
        homeClick: function() {
            rvue.method = null;
        }
    },
    computed: {
        // 计算属性的 getter
        apilist: function() {
            var search = this.apisearch;
            if (search == '') return this.list;
            var arr = new Array();
            for (var i = 0; i < this.list.length; i++) {
                if (this.list[i].Name.indexOf(search) > -1 || this.list[i].Intro.indexOf(search) > -1) {
                    arr.push(this.list[i]);
                }
            }
            return arr;
        }
    },
    created: function() {
        var th = this;
        $api.get("helper/List").then(function(req) {
            if (req.data.success) {
                th.list = req.data.result;
            } else {
                alert(req.data.message);
            }
        });
        window.setTimeout(function() {
            document.getElementById("apisearch").focus();
        }, 1000);
    }
});
//menvue.$mount('#menu');

// 注册组件
Vue.component('methods', {
    // 声明 props，用于向组件传参
    props: ['name', 'intro', 'index'],
    data: function() {
        return {
            methods: [], //方法列表
            loading: true, //预载中
            open: false //是否打开方法列表
        }
    },
    methods: {
        //方法的点击事件
        methodClick: function(method) {
            rvue.method = method;
        }
    },
    created: function() {
        var name = this.name;
        var th = this;
        $api.get("helper/Methods", {
            classname: name
        }).then(function(req) {
            if (req.data.success) {
                th.methods = req.data.result;
                th.loading = false;
            }
        });
    },
    // 
    template: "<div>\
      <div class='classname' v-on:click='open=!open' :title='\"摘要：\"+intro'>\
            {{index}}. <b class='el-icon-loading' v-show='loading'></b>{{ name }} \
            <span>{{intro}}</span>\
            <i class='el-icon-arrow-right' v-show='!open'></i><i class='el-icon-arrow-down' v-show='open'></i>\
       </div>\
       <dl class='methods' v-show='open'>\
       <dt v-if='intro.length>0'>摘要：{{intro}}</dt>\
            <dd v-for='(item,i) in methods' v-on:click='methodClick(item)'>\
                {{index}}.{{i+1}}.{{item.Name}} \
                <div v-show='item.Intro.length>0'>--{{item.Intro}}</div>\
            </dd>\
       </dl>\
       </div>"
});
/*
 
 */
//右侧的内容区
var rvue = new Vue({
    data: {
        method: null, //接口方法对象
        loading: false, //调用接口时的状态
        showcode: false //显示代码
    },
    watch: {
        method: function(n, o) {
            var ele = document.getElementById("testresult");
            if (ele != null) ele.innerText = "";
            var inputs = Array.from(document.querySelectorAll("#context table input"));
            inputs.forEach(function(item) {
                item.value = "";
            });
            window.setTimeout(function() {
                rvue.teststring();
            }, 200);
        }
    },
    computed: {
        parameter: function() {
            var fullname = this.method.FullName;
            var name = this.method.Name;
            if (fullname.indexOf('(') > -1) {
                var paras = fullname.substring(fullname.indexOf('(') + 1).split(',');
                var str = '';
                for (var t = 0; t < paras.length; t++) {
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
        testapi: function() {
            var mathd = this.method.ClassName + "/" + this.method.Name;
            var params = this.getInputPara();
            var http = document.getElementById("httppre").value;
            var rettype = document.getElementById("returntype").value;
            //调用
            $api.query(mathd, eval("(" + params + ")"), http, function() {
                rvue.loading = true;
            }, function() {
                window.setTimeout(function() {
                    rvue.loading = false;
                }, 500);
            }, rettype).then(function(req) {
                var ele = document.getElementById("testresult");
                if (req == null) throw {
                    message: '没有获取到返回值，可能是服务器端错误'
                };
                if (req.config.returntype == "json")
                    ele.innerText = $api.trim(rvue.jsonformat(unescape(JSON.stringify(req.data)), true));
                if (req.config.returntype == "xml")
                    ele.innerText = $api.trim(rvue.xmlformat(unescape(req.text)));

            }).catch(function(ex) {
                //alert(ex.message);
                var ele = document.getElementById("testresult");
                ele.innerText = ex.message;
            });
        },
        //生成测试代码
        teststring: function() {
            var mathd = this.method.ClassName + "/" + this.method.Name;
            var params = this.getInputPara();
            var http = document.getElementById("httppre").value;
            //语句
            var urlstr = window.location.protocol + "//" + window.location.host + "/api/v1/" + mathd;
            document.getElementById("apiurl").innerText = urlstr;
            var jsstr = "$api." + http + "('" + mathd + "'" + (params == "{}" ? "" : "," + params) + ")";
            jsstr += ".then(function(req){\r\
            if(req.data.success){\r\
                var result=req.data.result;\r\
                //...\r\
            }else{\r\
                throw req.data.message;\r\
            }\r}).catch(function (err) {\r\
            alert(err);\r});";
            //jsstr = rvue.jsonformat(jsstr, false);
            document.getElementById("teststring").innerText = jsstr;
            document.getElementById("input_teststring").innerText = jsstr;
        },
        //复制测试代码
        btnCopyEvent: function() {
            var jsstr = document.getElementById("teststring").innerText;
            var input = document.getElementById("input_teststring");
            input.value = jsstr; // 修改文本框的内容
            input.select(); // 选中文本
            document.execCommand('copy');
        },
        //获取录入的参数
        getInputPara: function() {
            var arr = new Array();
            var inputs = Array.from(document.querySelectorAll("#context table input"));
            inputs.forEach(function(item) {
                var name = item.getAttribute("name");
                var val = item.value;
                arr.push("'" + name + "':'" + val + "'");
            });
            var txt = "{";
            for (var i = 0; i < arr.length; i++) {
                txt += arr[i];
                if (i < arr.length - 1) txt += ",";
            }
            txt += "}";
            return txt;
        },
        //json字符格式化
        //isbrace:花括号是否换行，true换行
        jsonformat: function(json, isbrace) {
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
            (json.split('\r\n')).forEach(function(node, index) {
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
            return formatted;
        },
        //xml格式化
        xmlformat: function(text) {
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
            text = '\n' + text.replace(/(<\w+)(\s.*?>)/g, function($0, name, props) {
                return name + ' ' + props.replace(/\s+(\w+=)/g, " $1");
            }).replace(/>\s*?</g, ">\n<");
            //处理注释
            text = text.replace(/\n/g, '\r').replace(/<!--(.+?)-->/g, function($0, text) {
                var ret = '<!--' + escape(text) + '-->';
                return ret;
            }).replace(/\r/g, '\n');
            //调整格式  以压栈方式递归调整缩进
            var rgx = /\n(<(([^\?]).+?)(?:\s|\s*?>|\s*?(\/)>)(?:.*?(?:(?:(\/)>)|(?:<(\/)\2>)))?)/mg;
            var nodeStack = [];
            var output = text.replace(rgx, function($0, all, name, isBegin, isCloseFull1, isCloseFull2, isFull1, isFull2) {
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
            outputText = outputText.replace(/\n/g, '\r').replace(/(\s*)<!--(.+?)-->/g, function($0, prefix, text) {
                if (prefix.charAt(0) == '\r')
                    prefix = prefix.substring(1);
                text = unescape(text).replace(/\r/g, '\n');
                var ret = '\n' + prefix + '<!--' + text.replace(/^\s*/mg, prefix) + '-->';
                return ret;
            });
            outputText = outputText.replace(/\s+$/g, '').replace(/\r/g, '\r\n');
            return outputText;
        }
    },
    created: function() {

    }
});
rvue.$mount('context');