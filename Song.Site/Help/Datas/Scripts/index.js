/*!
 * 主 题：数据库表、字段的说明信息的查看维护
 * 说 明：
 * 1、查看或编辑数据实体的说明信息
 * 2、主要是为了提升开发效率，方便开发人员查阅;
 *
 * 作 者：微厦科技_宋雷鸣_10522779@qq.com
 * github开源地址:https://github.com/weishakeji/LearningSystem
 */
var vapp = new Vue({
    el: '#entities',
    data: {
        entitysearch: '',   //用于左侧实体列表的搜索
        entities: {}, //实体列表，       
        loading: false,
        helpshow: false,     //帮助信息的显示状态
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
        'entities': {
            deep: true,
            handler: function (nval, oval) {
                if (JSON.stringify(oval) != "{}")
                    this.update();
            }
        },
        'loading': function (vl, ol) {
            this.mask(vl);
        }
    },
    created: function () {
        var th = this;
        th.loading = true;
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
        //帮助信息隐藏
        window.setTimeout(function () {
            //th.helpshow = false;
        }, 3000);
    },
    methods: {
        update: function () {
            this.loading = true;
            $api.post('Helper/Entities', { 'detail': this.entities }).then(function (req) {
                if (req.data.success) {
                    vapp.loading = false;
                    vapp.$notify({
                        title: '保存成功',
                        message: '数据实体的描述信息保存成功！',
                        type: 'success'
                    });
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
                vapp.loading = false;
            });
        },
        //遮罩层，用于加载过程中锁屏
        mask: function (state) {
            var mask = document.getElementById("mask");
            mask.style.setProperty('display', state ? 'block' : 'none', 'important');
        }
    }
});

//组件
// 实体详情
Vue.component('entity', {
    //依次：索引，当前实体名称，当前实体对象，所有实体对象
    props: ['index', 'clname', 'entity', 'datas'],
    data: function () {
        return {
            search: '',
            properties: {},      //实体的属性
            details: {},         //实体的属性说明
            //编辑状态,临时数据
            states: {
                update: false,       //是否更新
                intro: false,        //实体的介绍，与属性不在一起存储
                mark: false
                //各个字段的状态
                //如：Ac_ID:{mark:false, intro:false, relation:false}
            },
            loading: false
        }
    },
    computed: {
    },
    watch: {
        'clname': function (val, old) {
            this.getDetails(val);
        },
        'loading': function (val, old) {
            vapp.mask(val);
        },
        //当props中的states赋值时，传递给组件内部的editstates
        'details': {
            deep: true,
            //immediate: true,
            handler: function (nval, oval) {
                if (this.states['update']) this.updateDetails();
            }
        }
    },
    mounted() {
        this.getDetails(this.clname);
    },
    methods: {
        //获取详细信息
        getDetails: function (clname) {
            this.loading = true;
            var th = this;
            $api.bat(
                $api.get('Helper/EntityField', { 'name': th.clname }), //获取字段（属性）
                $api.get('Helper/EntityDetails', { 'name': th.clname })  //字段说明
            ).then(axios.spread(function (field, detal) {
                if (field.data.success) th.properties = field.data.result;
                if (detal.data.success) th.details = detal.data.result;
                Vue.set(th.states, 'update', false);
                th.loading = false;
            })).catch(function (err) {
                alert(err);
                console.error(err);
                vapp.loading = false;
            });
        },
        //更新详细信息
        updateDetails: function () {
            this.loading = true;
            var th = this;
            $api.post('Helper/EntityDetails', { 'name': this.clname, 'detail': this.details }).then(function (req) {
                if (req.data.success) {
                    th.$notify({
                        title: '保存成功',
                        message: '数据实体的描述信息保存成功！',
                        type: 'success'
                    });
                    Vue.set(th.states, 'update', false);
                    th.loading = false;
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
                vapp.loading = false;
            });
        },
        //--------------------------------
        //获取内容，attr:实体或字段名称，cont:内容类型
        text: function (attr, cont, html) {
            var item = this.details[attr];
            if (item == null) return '';
            try {
                var text = !!item[cont] ? item[cont] : "";
                if (html == null || !html) return text;
                text = text.replace(/\n/g, "<br/>");
                return text;
            } catch (err) {
                console.error(err);
            }
        },
        //实体详情的标注,attr:实体属性,state:是否编辑状态
        state: function (attr, state) {
            if (this.states == null) this.states = {};
            if (state != null) Vue.set(this.states, attr, state);
            if (!!this.states[attr])
                return this.states[attr];
            return false;
        },
        //进入编辑状态
        edit: function (attr) {
            this.state(attr, true);
            window.setTimeout(function () {
                document.getElementById(attr).focus();
            }, 200);
        },
        //退出编辑状态
        leave: function (attr) {
            var val = document.getElementById(attr).value;
            if (attr.indexOf('.') > -1) {
                var prefix = attr.substring(0, attr.indexOf('.'));
                var suffix = attr.substring(attr.indexOf('.') + 1);
                this.details[prefix][suffix] = val;
                Vue.set(this.states, 'update', true);
            } else {
                this.entity[attr] = val;
            }
            this.state(attr, false);
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
        <span class="mark" v-show="!state(\'mark\')" @click="edit(\'mark\')">\
        <i class="el-icon-edit"></i><span>{{entity.mark}}</span></span>\
        <span v-show="state(\'mark\')"><i class="el-icon-edit"></i>\
        <input type="text" :value="entity.mark" id="mark" @keyup.enter="leave(\'mark\')" @blur="leave(\'mark\')" />\
        </span>\
        <div class="psearch"><input type="text" v-model="search" /><i class="el-icon-search"></i></div>\
    </div>\
    <div class="intro">\
        <span class="intro_text" v-show="!state(\'intro\')" @click="edit(\'intro\')">\
        <i class="el-icon-edit"></i>说明：<span v-html="entity.intro"></span></span>\
        <span v-show="state(\'intro\')"><i class="el-icon-edit"></i> 说明：<br />\
        <textarea rows="3" style="width: 100%;"  id="intro" :value="entity.intro" @blur="leave(\'intro\')"></textarea>\
        </span>\
    </div>\
    <table border="0">\
    <tr><th>序号</th><th>属性/字段</th><th>类型</th><th>可空</th><th>关联</th><th>备注</th><th>说明</th></tr>\
    <tr v-for="(v,k,i) in properties">\
        <td>{{i+1}}</td>\
        <td v-html="$options.filters.show(k,search)" @dblclick="copy(k)"></td>\
        <td>{{v.type}}</td>\
        <td>{{v.nullable ? v.nullable : \'\'}}</td>\
        <td @dblclick="edit(k+\'.relation\')" mark="关联">\
            <span v-if="!state(k+\'.relation\')">{{text(k,\'relation\')}}</span>\
            <select  v-if="state(k+\'.relation\')" :id="k+\'.relation\'"\
                :value="text(k,\'relation\')" @blur="leave(k+\'.relation\')"\
                @change="leave(k+\'.relation\')">\
                <option value=""></option>\
                <option :value="key" v-for="(val,key,index) in datas">{{key}}</option>\
            </select>\
        </td >\
        <td @dblclick="edit(k+\'.mark\')"  mark="备注">\
            <span v-if="!state(k+\'.mark\')" v-html="$options.filters.show(text(k,\'mark\',true),search)"></span>\
            <textarea rows="3" v-if="state(k+\'.mark\')" :id="k+\'.mark\'"\
            :value="text(k,\'mark\')" @blur="leave(k+\'.mark\')"></textarea>\
        </td>\
        <td @dblclick="edit(k+\'.intro\')"  mark="说明">\
            <span v-if="!state(k+\'.intro\')" v-html="$options.filters.show(text(k,\'intro\',true),search)"></span>\
            <textarea rows="3" v-if="state(k+\'.intro\')" :id="k+\'.intro\'"\
            :value="text(k,\'intro\')" @blur="leave(k+\'.intro\')"></textarea>\
        </td>\
        </table >\
    </div>'
});
