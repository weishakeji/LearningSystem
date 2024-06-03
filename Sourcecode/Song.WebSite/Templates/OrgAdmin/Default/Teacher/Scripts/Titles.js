$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                orgid: '',
                name: '',
                use: ''
            },
            organ: {},       //当前机构
            config: {},

            loading: false,
            loadingid: 0,        //当前操作中的对象id
            datas: [], //数据源 

            selects: [] //数据表中选中的行
        },
        watch: {},
        computed: {},
        created: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/Current')
            ).then(([organ]) => {
                //获取结果             
                th.organ = organ.data.result;
                th.form.orgid = th.organ.Org_ID;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.handleCurrentChange();
            }).catch(err => console.error(err))
                .finally(() => { });
        },
        methods: {
            //加载数据页
            handleCurrentChange: function () {
                var th = this;
                th.loading = true;
                $api.get("Teacher/Titles", th.form).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                    } else {
                        throw d.data.message;
                    }
                    th.rowdrop();
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //刷新行数据，
            freshrow: function (id) {
                if (id == null || id == '' || id == 0) return this.handleCurrentChange();
                if (this.datas.length < 1) return;
                //要刷新的行数据
                let entity = this.datas.find(item => item.Ths_ID == id);
                if (entity == null) return;
                //获取最新数据，刷新
                var th = this;
                th.loadingid = id;
                $api.get('Teacher/TitleForID', { 'id': id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        let index = th.datas.findIndex(item => item.Ths_ID == id);
                        if (index >= 0) th.$set(th.datas, index, result);
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = 0);
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                $api.delete('Teacher/TitleDelete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //更改使用状态
            changeState: function (row) {
                var th = this;
                th.loadingid = row.Sts_ID;
                $api.post('Teacher/TitleModify', { 'entity': row }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = 0);
            },
            //行的拖动
            rowdrop: function () {
                // 首先获取需要拖拽的dom节点            
                const el1 = document.querySelectorAll('table > tbody')[0];
                if (el1 == null) return;
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
                        if ($dom('table tr.expanded').length > 0) {
                            return false;
                        };
                        evt.dragged; // dragged HTMLElement
                        evt.draggedRect; // TextRectangle {left, top, right и bottom}
                        evt.related; // HTMLElement on which have guided
                        evt.relatedRect; // TextRectangle
                        originalEvent.clientY; // mouse position

                    },
                    onEnd: (e) => {
                        let arr = this.datas; // 获取表数据
                        arr.splice(e.newIndex, 0, arr.splice(e.oldIndex, 1)[0]); // 数据处理
                        this.$nextTick(function () {
                            this.datas = arr;
                            for (var i = 0; i < this.datas.length; i++) {
                                this.datas[i].Ths_Tax = i + 1;
                            }
                            this.changeTax();
                        });
                    }
                });
            },
            //更新排序
            changeTax: function () {
                var arr = $api.clone(this.datas);
                for (var i = 0; i < arr.length; i++)
                    delete arr[i]['childs'];
                var th = this;
                th.loading = true;
                $api.post('Teacher/TitleUpdateTaxis', { 'items': arr }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '修改顺序成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //设置默认组
            setDefault: function (id) {
                var th = this;
                this.loadingid = id;
                $api.get('Teacher/TitleSetDefault', { 'orgid': th.organ.Org_ID, 'id': id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '设置默认成功!',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = 0);
            }
        }
    });
    //职称下的教师数
    Vue.component('teacher_count', {
        //sortid:职称id
        props: ['sortid',],
        data: function () {
            return {
                count: 0,
                loading: true       //预载
            }
        },
        watch: {
            'sortid': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv) || nv == ov) return;
                    var th = this;
                    th.loading = true;
                    $api.get('Teacher/TitleOfNumber', { 'id': nv }).then(function (req) {
                        if (req.data.success) {
                            th.count = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => th.loading = false);
                }, immediate: true
            }
        },
        created: function () {

        },
        methods: {
        },
        template: `<span class="teacher_count">
            <span class="el-icon-loading" v-if="loading"></span>
            <template v-else>{{count}}</template>
        </span>`
    });
});