$ready([
    'Components/entities.js'
], function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            entity: null,      //当前实体
            datas: {},       //所有实体

            search: '',      //搜索
            fields: {},      //表的字段
            details: {},         //实体的属性说明
            //编辑状态,临时数据
            states: {
                update: false,       //是否更新
                intro: false,        //实体的介绍，与属性不在一起存储
                mark: false
                //各个字段的状态
                //如：Ac_ID:{mark:false, intro:false, relation:false}
            },
            loadstate: {
                init: true,        //初始化
                def: false,         //默认
                get: false,         //加载数据
                update: false,      //更新数据
                del: false          //删除数据
            }
        },
        mounted: function () {

        },
        created: function () {
            //alert(3)
        },
        computed: {
            loading: function () {
                if (!this.loadstate) return false;
                for (let key in this.loadstate) {
                    if (this.loadstate.hasOwnProperty(key)
                        && this.loadstate[key])
                        return true;
                }
                return false;
            }
        },
        watch: {
            'entity': {
                handler: function (nv, ov) {
                    //console.error(nv);
                }, deep: true
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
        methods: {
            //设置当前表结构的实体
            setentity: function (ent, entities) {
                this.entity = ent;
                this.datas = entities;
                this.getDetails();
            },
            //获取详细信息
            getDetails: function () {
                var th = this;
                th.loadstate.init = true;
                $api.bat(
                    $api.get('DataBase/Fields', { 'tablename': th.entity.name }), //获取字段（属性）
                    $api.get('DataBase/FieldsDescr', { 'tablename': th.entity.name })  //字段说明
                ).then(([field, detal]) => {
                    th.fields = field.data.result;
                    th.details = detal.data.result;
                    Vue.set(th.states, 'update', false);
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.init = false);
            },
            //实体详情的标注,attr:实体属性,state:是否编辑状态
            state: function (attr, state) {
                if (this.states == null) this.states = {};
                if (state != null) Vue.set(this.states, attr, state);
                if (!!this.states[attr]) return this.states[attr];
                return false;
            },
            //进入编辑状态
            edit: function (attr) {
                this.state(attr, true);
                this.$nextTick(function () {
                    $dom('.' + attr).find('input,textarea').focus();
                });
                window.setTimeout(function () {
                    let el = document.getElementById(attr);
                    if (el) el.focus();
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
            //保存实体的标题说明与简介，不涉及字段
            updateentity: function (attr) {
                var th = this;
                th.state(attr, false);
                th.loading = true;
                let datas = {};
                for (let key in this.datas) {
                    datas[key] = {};
                    datas[key]['mark'] = this.datas[key]['mark'];
                    datas[key]['intro'] = this.datas[key]['intro'];
                }
                $api.post('DataBase/TablesDescrUpdate', { 'detail': datas }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            title: '保存成功',
                            message: '实体描述信息保存成功！',
                            type: 'success'
                        });
                    } else {
                        throw + req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '保存更新失败', { type: 'error' });

                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //更新详细信息
            updateDetails: function () {
                var th = this;
                th.loading = true;
                $api.post('DataBase/FieldsDescrUpdate', { 'tablename': th.entity.name, 'detail': th.details }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            title: '保存成功',
                            message: '描述信息保存成功！',
                            type: 'success'
                        });
                        th.$set(th.states, 'update', false);
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '保存更新失败', { type: 'error' });
                    console.error(err);
                }).finally(() => th.loading = false);
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
            },
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
        },
        filters: {
            //字段类型的显示
            type: function (ty, length) {
                if (ty == 'nvarchar') {
                    var leng = Number(length);
                    if (leng < 0) return 'nvarchar(max)'
                    else return 'nvarchar(' + (leng / 2) + ')';
                }
                return ty;
            }
        },
        components: {

        }
    });
});