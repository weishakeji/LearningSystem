var vapp = new Vue({
    el: '#entities',
    data: {
        entitysearch: '',   //用于左侧实体列表的搜索
        entities: {}, //实体列表，       
        loading: false,
        //用于向子组件传参
        current: '',     //当前操作的实体名称
        index: 0,
        entity: {}      //当前要操作的实体对象      
    },
    computed: {
        // 用于左侧实体列表，带查询
        entitylist: function () {
            this.entitysearch = this.entitysearch.toLowerCase();
            if (this.entitysearch == '') return this.entities;
            var arr = {};
            for (var item in this.entities) {
                if (item.toLowerCase().indexOf(this.entitysearch) > -1
                    || this.entities[item]['mark'].indexOf(this.entitysearch) > -1) {
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
        'current': function (nl, ol) {
            console.log(nl);
        }
    },
    created: function () {
        var th = this;
        //实体信息的获取
        $api.get("helper/Entities").then(function (req) {
            if (req.data.success) {
                th.entities = req.data.result;
                for (var t in th.entities) {
                    th.current = t;
                    th.entity = th.entities[t];
                    break;
                }
                th.loading = false;
            } else {
                console.error(req.data.message);
            }
        });
    },
    methods: {
        update: function () {
            $api.post('Helper/Entities', { 'detail': this.entities }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    vapp.$notify({
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
});

//组件
// 实体详情
Vue.component('entity', {
    //依次：索引，当前实体名称，当前实体对象，所有实体对象
    props: ['index', 'clname', 'entity', 'datas', 'states'],
    data: function () {
        // data 选项是一个函数，组件不相互影响
        return {
            search: '',
            properties: {},      //实体的属性
            details: {},         //实体的属性说明
            editstates: {},   //编辑状态,临时数据     
            //编辑状态    
            editor: {
                mark: false,
                intro: false
            }
        }
    },
    computed: {
    },
    watch: {
        'clname': function (val, old) {
            this.getField(val);
            this.getDetails(val);
        },
        'search': function (val, old) {
            //console.log(val);
        },
        'editor': {
            deep: true,
            handler: function (nval, oval) {
                if (!nval.mark && !nval.intro)
                    vapp.update();
            }
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
    mounted() {
        this.getField(this.clname);
        this.getDetails(this.clname);
    },
    methods: {
        //获取字段信息
        getField: function (clname) {
            var th = this;
            $api.get('Helper/EntityField', { 'name': clname }).then(function (req) {
                if (req.data.success) {
                    th.properties = req.data.result;
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        //获取详细信息
        getDetails: function (clname) {
            var th = this;
            $api.get('Helper/EntityDetails', { 'name': clname, 'detail': '' }).then(function (req) {
                if (req.data.success) {
                    th.details = req.data.result;
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        //--------------------------------
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
        //实体的详情，cont为内容类型，mark或intro
        entityDetail: function (cont) {

        },
        //实体编辑状态，cont同上
        entityState: function (cont) {

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
        <span class="mark" v-show="!editor.mark" @click="editor.mark=true">\
        <i class="el-icon-edit"></i><span>{{entity.mark}}</span></span>\
        <span v-show="editor.mark"><i class="el-icon-edit"></i>\
        <input type="text" v-model="entity.mark" @keyup.enter="editor.mark=false" @blur="editor.mark=false" />\
        </span>\
        <div class="psearch"><input type="text" v-model="search" /><i class="el-icon-search"></i></div>\
    </div>\
    <div class="intro">\
        <span class="intro_text" v-show="!editor.intro" @click="editor.intro=true">\
        <i class="el-icon-edit"></i>说明：<div v-html="entity.intro"></div></span>\
        <span v-show="editor.intro"><i class="el-icon-edit"></i> 说明：<br />\
        <textarea rows="3" style="width: 100%;" v-model="entity.intro" @blur="editor.intro=false"></textarea>\
        </span>\
    </div>\
    <table border="0">\
    <tr><th>序号</th><th>属性/字段</th><th>类型</th><th>可空</th><th>关联</th><th>备注</th><th>说明</th></tr>\
    <tr v-for="(v,k,i) in properties">\
        <td>{{i+1}}</td>\
        <td v-html="$options.filters.show(k,search)" @dblclick="copy(k)"></td>\
        <td>{{v.type}}</td>\
        <td>{{v.nullable ? v.nullable : \'\'}}</td>\
        <td>{{v.relation}}</td>\
        <td>{{v.mark}}</td>\
        <td>{{v.intro}}</td>\
        </table >\
    </div>'
});
