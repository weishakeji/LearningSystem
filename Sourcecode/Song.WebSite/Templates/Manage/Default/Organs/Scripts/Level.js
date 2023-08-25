$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: { 'search': '', 'use': '' },
            datas: [],
            selects: [], //数据表中选中的行   
            loading: false,
            loadingid: false,
        },
        created: function () {
            this.getlist();
        },
        methods: {
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                $api.delete('Organization/LevelDelete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success', center: true,
                            message: '成功删除' + result + '条数据'
                        });
                        th.getlist();
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => alert(err))
                    .finally(() => th.loading = false);
            },
            //加载数据页
            getlist: function () {
                var th = this;
                th.loading = true;
                $api.get('Organization/LevelAll', th.form).then(function (req) {
                    if (req.data.success) {
                        th.datas = req.data.result;
                        th.rowdrop();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => alert(err))
                    .finally(() => th.loading = false);
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                var rowkey = this.$refs.datatable.rowKey;
                this.$refs.btngroup.modify(row[rowkey]);
            },
            //更改使用状态
            changeUse: function (row) {
                var th = this;
                th.loadingid = row.Olv_ID;
                $api.post('Organization/LevelModify', { 'entity': row }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => alert(err))
                    .finally(() => th.loadingid = 0);
            },
            //更改默认等级
            changeDefault: function (row) {
                var th = this;
                th.loadingid = row.Olv_ID;
                $api.post('Organization/LevelSetDefault', { 'id': row.Olv_ID }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success', center: true,
                            message: '修改状态成功!'
                        });
                        th.getlist();
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => alert(err))
                    .finally(() => th.loadingid = 0);
            },
            //设置等级权限
            setPurview: function (row) {
                //let url = '/manage/organs/LevelPurview?id=' + row.Olv_ID;
                let url = $api.url.set('LevelPurview', { 'id':  row.Olv_ID });
                let title = "“" + row.Olv_Name + "”的权限设置";
                let boxid = window.name + '_' + row.Olv_ID + '[purview';
                this.$refs.btngroup.pagebox(url, title, boxid, 800, 600, { full: true });
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
                        evt.dragged; // dragged HTMLElement
                        evt.draggedRect; // TextRectangle {left, top, right и bottom}
                        evt.related; // HTMLElement on which have guided
                        evt.relatedRect; // TextRectangle
                        originalEvent.clientY; // mouse position
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
                var th = this;
                th.loading = true;
                $api.post('Organization/LevelModifyTaxis', { 'items': th.datas }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success', center: true,
                            message: '修改顺序成功!'
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => alert(err))
                    .finally(() => th.loading = false);
            }
        },
        components: {
            //机构数
            'orgcount': {
                props: ['lvid'],    //机构等级id
                data: function () {
                    return {
                        count: 0,
                        loading: false
                    }
                },
                watch: {
                    'lvid': {
                        handler: function (nv, ov) {
                            if ($api.isnull(nv)) return;
                            var th = this;
                            th.loading = true;
                            $api.get('Organization/LevelOrgcount', { 'id': nv }).then(function (req) {
                                if (req.data.success) {
                                    th.count = req.data.result;
                                }
                            }).catch(function (err) {
                                console.error(err);
                            }).finally(() => th.loading = false);
                        }, immediate: true,
                    }
                },
                template: `<span>
                        <loading v-if="loading"></loading>
                        <template v-else>{{count}}</template>
                </span>`
            },
            //分润方案
            'profit': {
                props: ['psid'],    //分润信息的id
                data: function () {
                    return {
                        name: '',       //分润方案的名称
                        loading: false
                    }
                },
                watch: {
                    'psid': {
                        handler: function (nv, ov) {
                            if ($api.isnull(nv)) return;
                            var th = this;
                            th.loading = true;
                            $api.get('ProfitSharing/ThemeForID', { 'id': nv }).then(function (req) {
                                if (req.data.success) {
                                    th.name = req.data.result.Ps_Name;
                                }
                            }).catch(function (err) {
                                console.error(err);
                            }).finally(() => th.loading = false);
                        }, immediate: true,
                    }
                },
                template: `<span>
                        <loading v-if="loading"></loading>
                        <template v-else>{{name}}</template>
                </span>`
            }
        }
    });

});