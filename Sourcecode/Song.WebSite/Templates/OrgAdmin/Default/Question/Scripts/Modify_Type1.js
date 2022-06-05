
$ready(function () {
    Vue.use(VueHtml5Editor, {
        showModuleName: true,
        image: {
            sizeLimit: 512 * 1024,
            compress: true,
            quality: 80
        }
    });
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.dot(),
            organ: {},           //当前机构
            config: {},      //当前机构配置项    
            types: [],        //试题类型，来自web.config中配置项

            entity: {},      //当前试题            
            ansitems: [],       //试题的选项
            ans_min: 4,          //选项最少几个
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
            //选项的单选事件
            radio: function (item) {
                for (let i = 0; i < this.ansitems.length; i++) {
                    this.ansitems[i].Ans_IsCorrect = false;
                }
                item.Ans_IsCorrect = true;
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
            //验证方法
            verify: function (ques, alert) {
                var th = this;
                //至少两个选项
                if (this.ansitems.length < 2) return alert('选项数量不得少于两条', 0);
                //至少有一个正确答案
                var correct = 0
                for (let i = 0; i < this.ansitems.length; i++)
                    if (this.ansitems[i].Ans_IsCorrect) correct++;
                if (correct < 1) return alert('请择选项的正确答案', 0);
                //最多一个正确答案
                if (correct > 1) return alert('选项的正确答案最多设置一个', 0);
                //被选中的选项，内容不得为空
                var item = null;
                for (let i = 0; i < this.ansitems.length; i++)
                    if (this.ansitems[i].Ans_IsCorrect) item = this.ansitems[i];
                if (item != null && item.Ans_Context == '')
                    return alert('当前设置的正确答案，没有实际内容', 0);
                //将选项赋给试题
                this.entity.Qus_Items = this.ansitems;
                return true;
            }
        },
    });
}, ["/Utilities/editor/vue-html5-editor.js",
    'Components/ques_type.js',
    'Components/modify_main.js',
    'Components/knowledge.js',
    'Components/general.js',
    'Components/ques_error.js',
    'Components/ques_wrong.js',
    'Components/enter_button.js']);
