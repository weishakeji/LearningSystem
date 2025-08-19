$ready([
    'Components/interfaces.js'
], function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            method: null,    //当前接口方法
            //调用接口的相关参数
            testform: {
                httpmethod: 'GET',
                returntype: 'JSON',      //接口返回的方式
                apipath: ''              //接口路径
            },
            httpmethods: ["GET", "POST", "DELETE", "PUT", "PATCH", "OPTIONS", "CACHE"],
            //接口完整路径
            apiurl: '',
            foldintro: false,        //是否显示接口介绍
            showscript: false,       //是否显示脚本
            showresult: false,       //是否显示接口调用的结果

            loading: false,  //预载，测试中的预载
        },
        watch: {
            //当前接口方法改变时
            'method': function (val) {
                let method = val.ClassName + "/" + val.Name;
                this.apiurl = window.location.protocol + "//" + window.location.host + "/api/v" + $api.version + "/" + method.toLowerCase();
                this.testform.apipath = val.ClassName + "/" + val.Name;
                //隐藏脚本显示
                this.displayscript(false);
                //隐藏结果
                this.showresult = false;
                $jsontree.render(document.getElementById("apiresult-json"), '');
                $jsontree.setJson("apiresult-json", '');
                $jsontree.destroyAll();
                let xmlel = document.getElementById("apiresult-xml");
                if (xmlel != null) {
                    xmlel.innerHTML = '';
                    Prism.highlightAll();
                }
                //是否折叠接口介绍区域
                this.foldintro = false;
            }
        },
        computed: {
            //api的测试脚本
            'apiscript': function () {
                //API路径
                let method = this.method.ClassName + "/" + this.method.Name;
                //参数
                let params = {};
                this.method.Paras.forEach(item => params[item.Name] = item.Value ? item.Value : "");
                //Http方法
                let http = this.testform.httpmethod.toLowerCase();
                let jsstr = "$api." + http + "(\"" + method + "\"" + (JSON.stringify(params) == "{}" ? "" : ", " + JSON.stringify(params)) + ")";
                jsstr += "\r\.then(req=>{\r\
     if(req.data.success){\r\
         let result=req.data.result;\r\
         //...业务代码\r\
     }else{\r\
         console.error(req.data.exception);\r\
         throw req.config.way + ' ' + req.data.message;\r\
     }\r}).catch(err=>console.error(err))\r\
     .finally(()=>{});";
                return jsstr;
            },
            //测试脚本的参数
            'apiparams': function () {
                //参数
                let params = {};
                this.method.Paras.forEach(item => params[item.Name] = item.Value ? item.Value : "");
                return JSON.stringify(params) == "{}" ? "" : JSON.stringify(params);
            }
        },
        mounted: function () {

        },
        methods: {
            //显示文本
            showintro: txt => {
                txt = txt.replace(/\s+/g, " ");
                txt = txt.replace(/\r/g, " ");
                txt = txt.replace(/\n/g, " ");
                return txt.replace(/ /g, '');
            },
            //复制到粘贴板
            copytext: function (val, textbox) {
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
            //生成测试代码
            displayscript: function (show) {
                if (show != null && typeof show === 'boolean') this.showscript = show;
                let testel = document.getElementById("teststring");
                if (!testel) return;
                testel.innerHTML = this.apiscript;
                Prism.highlightAll();
            },
            //测试接口
            testapi: function () {
                var method = this.method.ClassName + "/" + this.method.Name;
                var para = {};
                this.method.Paras.forEach(item => para[item.Name] = item.Value ? item.Value : "");
                var http = this.testform.httpmethod.toLowerCase();
                var rettype = this.testform.returntype.toLowerCase();
                var th = this;
                th.showresult = true;
                //调用
                $api.query(method, para, http, () => th.loading = true, () => th.loading = false, rettype)
                    .then(req => {
                        if (req == null) throw '没有获取到返回值，可能是服务器端错误';
                        th.foldintro = true;
                        th.$nextTick(function () {
                            if (req.config && req.config.returntype == "xml") {
                                var ele = document.getElementById("apiresult-xml");
                                let xml = th.xmlformat(unescape(req.data));
                                document.getElementById("apiresult-xml").innerHTML = xml;
                                Prism.highlightAll();
                            }
                            if (!req.config || req.config.returntype == "json") {
                                let json = unescape(JSON.stringify(req.data));
                                $jsontree.render(document.getElementById("apiresult-json"), req.data);
                                $jsontree.setJson("apiresult-json", json);
                            }
                        });

                    }).catch(function (ex) {
                        alert(ex);
                    }).finally(() => th.loading = false);
            },
            //xml格式化
            xmlformat: function (text) {
                let formatted = '', indent = '';
                const tab = '  ';
                text.split(/>\s*</).forEach(node => {
                    if (node.match(/^\/\w/)) indent = indent.substring(tab.length)
                    formatted += indent + '<' + node + '>\n'
                    if (node.match(/^<?\w[^>]*[^\/]$/)) indent += tab
                });
                text = formatted.substring(1, formatted.length - 1);
                return text.replace(/</g, '&lt;').replace(/>/g, '&gt;');
            },
        },
    });
});