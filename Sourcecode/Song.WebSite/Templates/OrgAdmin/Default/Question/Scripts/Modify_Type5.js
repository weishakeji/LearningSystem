
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.dot(),
            organ: {},           //当前机构
            config: {},      //当前机构配置项    
            types: [],        //试题类型，来自web.config中配置项

            course: {},          //当前试题的课程
            entity: {},      //当前试题            
            ansitems: [],       //试题的选项
            ans_min: 3,          //选项最少几个
            ans_max_id: 0,       //答案项的最大id

            showitem: false,        //显示选项编辑的面板
            edititem: {},        //当前要编辑的选项

            loading: false
        },
        watch: {
            'entity': {
                handler: function (nv, ov) {
                    if (this.ansitems.length < 1 && $api.getType(this.entity.Qus_Items) == "Array") {
                        this.ansitems = this.entity.Qus_Items;
                        for (let i = 0; i < this.ansitems.length; i++) {
                            if (this.ansitems[i].Ans_ID > this.ans_max_id)
                                this.ans_max_id = this.ansitems[i].Ans_ID;
                        }
                        while (this.ansitems.length < this.ans_min) {
                            this.$set(this.ansitems, this.ansitems.length, this.newitem());
                        }
                        this.$nextTick(function () {
                            this.rowdrop();
                        });

                    }
                }, immediate: false, deep: true
            }
        },
        created: function () { },
        mounted: function () { },
        methods: {
            //创建新的选项
            newitem: function () {
                return {
                    'Ans_ID': ++this.ans_max_id,
                    'Qus_ID': 0,
                    'Qus_UID': '',
                    'Ans_Context': '',
                    'Ans_IsCorrect': false
                };
            },
            //选项的多选事件
            checkbox: function (item) {
                item.Ans_IsCorrect = !item.Ans_IsCorrect;
            },
            //编辑选项
            edit: function (item, index) {
                this.showitem = true;
                this.edititem = $api.clone(item);
                this.edititem.index = index;
            },
            //确认编辑
            editenter: function () {
                this.showitem = false;
                var item = this.ansitems[this.edititem.index];
                item.Ans_Context = this.edititem.Ans_Context;
            },
            //删除选项
            del: function (item, index) {
                console.log(index);
                this.ansitems.splice(index, 1);
            },
            add: function () {
                this.$set(this.ansitems, this.ansitems.length, this.newitem());
            },
            //清理Html标签
            clearhtml: function (str) {
                return str.replace(/(<([^>]+)>)/ig, "");
            },
            //行的拖动
            rowdrop: function () {
                // 首先获取需要拖拽的dom节点            
                const el1 = document.getElementById('ansitems');
                Sortable.create(el1, {
                    disabled: false, // 是否开启拖拽
                    ghostClass: 'sortable-ghost', //拖拽样式
                    handle: '.text',     //拖拽的操作元素
                    animation: 150, // 拖拽延时，效果更好看
                    group: { // 是否开启跨表拖拽
                        pull: false,
                        put: false
                    },
                    onEnd: (e) => {
                        let arr = $api.clone(this.ansitems);
                        arr.splice(e.newIndex, 0, arr.splice(e.oldIndex, 1)[0]); // 数据处理
                        this.ansitems = [];
                        this.$nextTick(function () {
                            for (let i = 0; i < arr.length; i++)
                                Vue.set(this.ansitems, i, arr[i]);
                            this.entity.Qus_Items = this.ansitems;
                        });
                    }
                });
            },
            //更新排序
            changeTax: function () {
                var arr = $api.clone(this.ansitems);
                console.log(arr);
            },
            //验证方法
            verify: function (ques, alert) {
                //将选项赋给试题
                this.entity.Qus_Items = this.ansitems;
                return true;
            }
        },
    });
}, ['Components/ques_type.js',
    'Components/modify_main.js',
    'Components/knowledge.js',
    'Components/general.js',
    'Components/ques_error.js',
    'Components/ques_wrong.js',
    'Components/enter_button.js']);
