var vueapp = new Vue({
    el: '#entities',
    data: {
        entitysearch: '',   //用于左侧实体列表的搜索
        entities: {}, //实体列表，包括字段
        details: {}, //实体的详情,
        loading: false
    },
    computed: {
        // 用于左侧实体列表，带查询
        entitylist: function () {
            this.entitysearch = this.entitysearch.toLowerCase();
            if (this.entitysearch == '') return this.entities;
            var arr = {};
            for (var item in this.entities) {
                if (item.toLowerCase().indexOf(this.entitysearch) > -1) {
                    arr[item] = this.entities[item];
                    continue;
                }
                var obj = null;
                for (var t in this.details) {
                    if (t == item) {
                        obj = this.details[t];
                        break;
                    }
                }
                if (obj == null) continue;
                if (obj.mark.toLowerCase().indexOf(this.entitysearch) > -1) {
                    arr[item] = this.entities[item];
                    continue;
                }
            }
            return arr;
        },
        //左侧实体列表中的实体个数
        listcount: function () {
            var arr = this.entitylist;
            var count = 0;
            for (var item in arr) count++;
            return count;
        }
    },
    watch: {
        //实体详情变更时
        'details': {
            deep: true,
            handler: function (nval, oval) {
                if (JSON.stringify(oval) == "{}") return;
                var details = JSON.stringify(nval);
                var th = this;
                $api.post('Helper/EntityDetailsUpdate', { 'details': details }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            title: '保存成功',
                            message: '数据实体的描述信息保存成功！',
                            type: 'success'
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            }
        }
    },
    created: function () {
        var th = this;
        //实体信息的获取
        $api.get("helper/Entities").then(function (req) {
            if (req.data.success) {
                th.entities = req.data.result;

            } else {
                console.error(req.data.message);
            }
        });
        //实体说明信息的获取
        $api.get('Helper/EntityDetails').then(function (req) {
            if (req.data.success) {
                th.details = req.data.result;
            } else {
                throw req.data.message;
            }
        }).catch(function (err) {
            console.error(err);
        });
    },
    methods: {
        //获取实体的详情对象
        detail: function (name) {
            var obj = {};
            for (var t in this.details) {
                if (t == name) {
                    obj = this.details[t];
                    break;
                }
            }
            return obj;
        },
        //获取内容，name:实体或字段名称，cont:内容类型
        content: function (name, cont, html) {
            var item = this.detail(name);
            var text = item[cont] ? item[cont] : "";
            if (html == null || !html) return text;
            text = text.replace(/\n/g, "<br/>");
            return text;
        },
        //某个实体的详情
        entityDetails: function (name) {
            var detail = [];
            for (var t in this.details) {
                var pre = t.substring(0, name.length);
                if (pre == name) {
                    var item = new Object();
                    item[t] = this.details[t];
                    detail.push(item);
                }
            }
            return detail;
        },
        //实体的编辑状态，临时值
        entityStates: function (name) {
            //编辑状态的设置，全是临时变量
            var states = {}
            for (var t in this.details) {
                var pre = t.substring(0, name.length);
                if (pre == name) {
                    var item = new Object();
                    item["mark"] = false;
                    item["intro"] = false;
                    item["relation"] = false;
                    states[t] = item;
                }
            }
            return states;
        }
    }
});


// 实体详情
Vue.component('entity', {
    props: ['index', 'clname', 'properties', 'datas', 'details', 'states'],
    data: function () {
        // data 选项是一个函数，组件不相互影响
        return {
            search: '',
            editstates: {}   //编辑状态,临时数据           
        }
    },
    watch: {
        'search': function (val, old) {
            //console.log(val);
        },
        //当props中的states赋值时，传递给组件内部的editstates
        'states': {
            deep: true,
            immediate: true,
            handler: function (nval, oval) {
                this.editstates = nval;
            }
        }
    },
    methods: {
        //获取实体的详情对象
        detail: function (name) {
            var obj = {};
            for (var t in this.details) {
                var key = Object.keys(this.details[t])[0];
                if (key == name) {
                    obj = this.details[t];
                    break;
                }
            }
            return obj;
        },
        //获取内容，name:实体或字段名称，cont:内容类型
        content: function (name, cont, html) {
            var item = this.detail(name);
            if (!item[name]) return "";
            var text = item[name][cont] ? item[name][cont] : "";
            if (html == null || !html) return text;
            text = text.replace(/\n/g, "<br/>");
            return text;
        },
        //实体详情的标注,name:实体名称，cont:内容类型,state:是否编辑状态
        state: function (name, cont, state) {
            if (state != null) this.editstates[name][cont] = state;
            if (!!this.editstates[name] && this.editstates[name][cont])
                return this.editstates[name][cont];
            return null;
        },
        //进入编辑状态
        edit: function (name, cont) {
            this.state(name, cont, true);
            window.setTimeout(function () {
                document.getElementById(cont + '_' + name).focus();
            }, 200);
        },
        //退出编辑状态
        leave: function (name, cont) {
            var item = this.detail(name);
            var val = document.getElementById(cont + '_' + name).value;
            item[name][cont] = val;
            this.state(name, cont, false);
        },
        //复制到粘贴板
        copy: function (val) {
            var oInput = document.createElement('input');
            oInput.value = val;
            document.body.appendChild(oInput);
            oInput.select(); // 选择对象
            document.execCommand("Copy"); // 执行浏览器复制命令           
            oInput.style.display = 'none';
            this.$message({
                message: '复制 “' + val + '” 到粘贴板',
                type: 'success'
            });
        }
    },
    filters: {
        //实体详情的显示
        show: function (val, search) {
            if (!search || search == '') return val;
            var regExp = new RegExp(search, 'ig');
            return val.replace(regExp, `<b>${search}</b>`);
        }
    },
    template: '<div><a :name="clname" class="anchor">&nbsp;</a>\
    <div class="name">\
        {{index+1}}. <span @dblclick="copy(clname)">{{clname}}</span>\
        <span class="mark" v-show="!state(clname,\'mark\')" @click="edit(clname,\'mark\')">\
        <i class="el-icon-edit"></i><span>{{content(clname,\'mark\')}}</span></span>\
        <span v-show="state(clname,\'mark\')"><i class="el-icon-edit"></i>\
        <input type="text" :id="\'mark_\'+clname" :value="content(clname,\'mark\')" @keyup.enter="leave(clname,\'mark\')" @blur="leave(clname,\'mark\')" />\
        </span>\
        <div class="psearch"><input type="text" v-model="search" /><i class="el-icon-search"></i></div>\
    </div>\
    <div class="intro">\
        <span class="intro_text" v-show="!state(clname,\'intro\')" @click="edit(clname,\'intro\')">\
        <i class="el-icon-edit"></i>说明：<div v-html="content(clname,\'intro\',true)"></div></span>\
        <span v-show="state(clname,\'intro\')"><i class="el-icon-edit"></i> 说明：<br />\
        <textarea rows="3" style="width: 100%;" :id="\'intro_\'+clname" :value="content(clname,\'intro\')" @blur="leave(clname,\'intro\')"></textarea>\
        </span>\
    </div>\
    <table border="0">\
    <tr><th>序号</th><th>属性/字段</th><th>类型</th><th>可空</th><th>关联</th><th>备注</th><th>说明</th></tr>\
    <tr v-for="(v,k,i) in properties">\
        <td>{{i+1}}</td>\
        <td v-html="$options.filters.show(k,search)" @dblclick="copy(k)"></td>\
        <td>{{v.type}}</td>\
        <td>{{v.nullable ? v.nullable : \'\'}}</td>\
        <td @dblclick="edit(clname+\'.\'+k,\'relation\')" mark="关联">\
            <a :href="\'#\'+content(clname+\'.\'+k,\'relation\')"\
                :title="content(content(clname+\'.\'+k,\'relation\'),\'mark\')"\
                v-show="!state(clname+\'.\'+k,\'relation\')">{{content(clname+\'.\'+k,\'relation\')}}</a>\
            <select v-show="state(clname+\'.\'+k,\'relation\')" :id="\'relation_\'+clname+\'.\'+k"\
                :value="content(clname+\'.\'+k,\'relation\')" @blur="leave(clname+\'.\'+k,\'relation\')"\
                @change="leave(clname+\'.\'+k,\'relation\')">\
                <option value=""></option>\
                <option :value="key" v-for="(val,key,index) in datas">{{key}}</option>\
            </select>\
        </td >\
        <td @dblclick="edit(clname+\'.\'+k,\'mark\')" mark="备注" mark="备注">\
            <span v-show="!state(clname+\'.\'+k,\'mark\')" v-html="$options.filters.show(content(clname+\'.\'+k,\'mark\'),search)"></span>\
            <span v-show="state(clname+\'.\'+k,\'mark\')">\
            <input type="text" :id="\'mark_\'+clname+\'.\'+k" :value="content(clname+\'.\'+k,\'mark\')"\
               @keyup.enter="leave(clname+\'.\'+k,\'mark\')" @blur="leave(clname+\'.\'+k,\'mark\')" />\
            </span>\
        </td>\
        <td @dblclick="edit(clname+\'.\'+k,\'intro\')"  mark="说明">\
            <span v-show="!state(clname+\'.\'+k,\'intro\')" v-html="$options.filters.show(content(clname+\'.\'+k,\'intro\',true),search)"></span>\
            <span v-show="state(clname+\'.\'+k,\'intro\')">\
                <textarea rows="3" style="width: 100%;" :id="\'intro_\'+clname+\'.\'+k"\
                    :value="content(clname+\'.\'+k,\'intro\')" @blur="leave(clname+\'.\'+k,\'intro\')"></textarea>\
            </span>\
        </td>\
    </table >\
    </div>'
});
