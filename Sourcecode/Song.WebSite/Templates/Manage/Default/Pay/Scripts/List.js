$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            datas: [],
            loading: false,
            loadingid: false,
            selects: [], //数据表中选中的行   
            tmCount: 0,      //数据集的个数，用于获取机构数的返回结果是否完成
            tmProfit: 0      //用于获取分润方案的返回结果是否完成
        },
        created: function () {
            this.handleCurrentChange();
        },
        methods: {
            //删除
            deleteData: function (datas) {
                $api.delete('Pay/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vue.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        window.vue.handleCurrentChange();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //加载数据页
            handleCurrentChange: function () {
                var th = this;
                $api.get('Pay/List', { 'platform': '' }).then(function (req) {
                    if (req.data.success) {
                        vue.datas = req.data.result;
                        vue.rowdrop();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                var rowkey = this.$refs.datatable.rowKey;
                this.$refs.btngroup.modify(row[rowkey]);
            },
            //更改使用状态
            changeUse: function (row) {
                var th = this;
                this.loadingid = row.Pai_ID;
                $api.post('Pay/Modify', { 'entity': row }).then(function (req) {
                    if (req.data.success) {
                        vue.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    vue.$alert(err, '错误');
                });
            },
            //行的拖动
            rowdrop: function () {
                // 首先获取需要拖拽的dom节点            
                const el1 = document.querySelectorAll('table > tbody')[0];
                Sortable.create(el1, {
                    disabled: false, // 是否开启拖拽
                    ghostClass: 'sortable-ghost', //拖拽样式
                    handle: '.draghandle',     //拖拽的操作元素
                    animation: 150, // 拖拽延时，效果更好看
                    group: { // 是否开启跨表拖拽
                        pull: false,
                        put: false
                    },
                    onStart: function (evt) { },
                    onMove: function (evt, originalEvent) {
                        // Example: http://jsbin.com/tuyafe/1/edit?js,output
                        evt.dragged; // dragged HTMLElement
                        evt.draggedRect; // TextRectangle {left, top, right и bottom}
                        evt.related; // HTMLElement on which have guided
                        evt.relatedRect; // TextRectangle
                        originalEvent.clientY; // mouse position
                        // return false; — for cancel
                    },
                    onEnd: (e) => {
                        var table = this.$refs.datatable;
                        let indexkey = table.$attrs['index-key'];
                        let arr = this.datas; // 获取表数据
                        arr.splice(e.newIndex, 0, arr.splice(e.oldIndex, 1)[0]); // 数据处理
                        this.$nextTick(function () {
                            this.datas = arr;
                            for (var i = 0; i < this.datas.length; i++) {
                                this.datas[i][indexkey] = i * 1;
                            }
                            this.changeTax();
                        });
                    }
                });
            },
            //更新排序
            changeTax: function () {
                var arr = $api.clone(this.datas);
                $api.post('Pay/ModifyTaxis', { 'items': arr }).then(function (req) {
                    if (req.data.success) {
                        vue.$notify({
                            type: 'success',
                            message: '修改顺序成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //获取支付类型，例如支付宝、微信
            getpattern: function (pattern) {
                if (pattern.indexOf('支付宝') > -1) return 'zhifubao';
                if (pattern.indexOf('微信') > -1) return 'weixin';
            }
        }
    });
    //计算通过支付接口的资金总额
    Vue.component('moneysummary', {
        //entity:接口的对象实体   
        props: ['entity'],
        data: function () {
            return {
                summary: 0,     //支付接口的资金总额
                loading: false
            }
        },
        watch: {
            'entity': {
                handler: function (nv, ov) {
                    if (JSON.stringify(nv) != '{}' && nv != null)
                        this.getval();
                }, immediate: true, deep: true
            }
        },
        computed: {},
        mounted: function () { },
        methods: {
            getval: function () {
                var th = this;
                th.loading = true;
                $api.get('Pay/Summary', { 'id': th.entity.Pai_ID }).then(function (req) {
                    if (req.data.success) {
                        th.summary = req.data.result;
                        //th.money(th.summary+89565421);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                }).finally(function () {
                    th.loading = false;
                });
            },
            //输出金额的格式，即三位一个逗号
            money: function (num) {
                if ($api.getType(num) != 'Number') return num;
                return num.money();              
            }
        },
        template: `<span>
            <loading v-if="loading"></loading>
            <template v-else>
                <icon>&#xe746</icon>
                {{money(summary)}} 元
            </template>
        </span>`
    });
});