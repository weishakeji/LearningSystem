$ready([
    'Components/entities.js'
], function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            entity: {},      //当前实体
            datas: {},       //所有实体

            search: '',      //搜索
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
                    console.error(nv);
                }, deep: true
            },
            //表的结构介绍变更时
            'datas': {
                deep: true, immediate: false,
                handler: function (nval, oval) {
                    //if (JSON.stringify(oval) != "{}")
                        //this.updateentity();
                }
            }
        },
        methods: {
            //设置当前表结构的实体
            setentity: function (ent, entities) {
                this.entity = ent;
                this.datas = entities;
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
            },
            //退出编辑状态
            leave: function (attr) {
                this.state(attr, false);
            },
            //保存实体的标题说明与简介，不涉及字段
            updateentity: function () {
                var th = this;
                th.loading = true;
                let datas = {};
                for (let key in this.datas) {
                    datas[key] = {};
                    datas[key]['mark'] = this.datas[key]['mark'];
                    datas[key]['intro'] = this.datas[key]['intro'];
                }
                $api.post('Helper/EntitiesUpdate', { 'detail': datas }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            title: '保存成功',
                            message: '数据实体的描述信息保存成功！',
                            type: 'success'
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err);
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
        },
        filters: {

        },
        components: {

        }
    });
});