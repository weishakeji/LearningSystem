$ready(['Components/topbar.js'],
    function () {
        window.vapp = new Vue({
            el: '#vapp',
            data: {
                couid: $api.querystring("couid"),        //课程id
                course: {},         //当前课程对象
                guides: [],          //课程通知
                current: {},     //当前要显示通知的对象
                finished: false,
                query: { "couid": "", "uid": "", "show": "", "use": "", "search": "", "size": 10, "index": 0 },
                total: 0,
                loading: false,
                //显示通知分类的面板
                showCols: false,
                show: false, //显示当前通知
                columns: [],    //通知分类
            },
            mounted: function () {
                var th = this;
                $api.get("Course/ForID", { "id": th.couid })
                    .then(req => {
                        if (req.data.success) {
                            th.course = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                    .finally(() => { });
                th.getcolumns();
            },
            created: function () {

            },
            computed: {

            },
            watch: {

            },
            methods: {
                //获取分类
                getcolumns: function () {
                    var th = this;
                    $api.get("Guide/ColumnsTree", { "couid": th.couid, "search": "", "isuse": "" })
                        .then(req => {
                            if (req.data.success) {
                                th.columns = req.data.result;
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err))
                        .finally(() => { });
                },
                //加载通知列表
                onload: function () {
                    var th = this;
                    th.query.index++;
                    th.query.couid = th.couid;
                    //th.query.uid = uid ? uid : '';
                    let query = this.query;
                    $api.get("Guide/Pager", query).then(req => {
                        if (req.data.success) {
                            th.total = req.data.total;
                            var result = req.data.result;
                            for (let i = 0; i < result.length; i++) {
                                th.guides.push(result[i]);
                            }
                            var totalpages = req.data.totalpages;
                            // 数据全部加载完成
                            if (th.guides.length >= th.total || result.length == 0) {
                                th.finished = true;
                            }
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => {
                        th.error = err;
                        console.error(err);
                        th.finished = true;
                    }).finally(() => th.loading = false);
                },
                //分类的选择事件
                selectCol: function (column) {
                    this.query.index = 0;
                    this.guides = [];
                    this.query.uid = column ? column.Gc_UID : "";
                    this.onload();
                    //console.error(column.Gc_Title);
                }
            },
            filters: {

            },
            components: {
                //通知的数据行
                'datarow': {
                    'props': ['data', 'index'],
                    'data': function () {
                        return {

                        }
                    },
                    'watch': {},
                    'methods': {
                        //行的点击事件
                        clk: function (item) {
                            this.$emit('click', item);
                        }
                    },
                    'template': `<row @click="clk(data)">               
                        <span>{{index}}. {{data.Gu_Title}}</span>                       
                        <van-tag color="#eee" text-color="#666">{{data.Gu_PushTime|date("yyyy-MM-dd")}}</van-tag>
                    </row>`
                },
                //分类
                'columns': {
                    name: 'columns',
                    props: ['datas', 'level', 'current'],
                    data: function () {
                        return {

                        }
                    },
                    watch: {},
                    methods: {
                        clk: function (item) {
                            this.$emit('click', item);
                        }
                    },
                    template: `<div class="columns">
                    <template v-for="(d,i) in datas" :key="d.Gc_ID">
                        <row class="col-item" :key="d.Gc_ID" :style="'padding-left: calc('+level+' * 20px);'" @click="clk(d)" :current="d.Gc_UID==current">
                            <span class="col-name">{{i+1}}. {{d.Gc_Title}}</span>                           
                        </row>
                        <columns :ref="'col_'+d.Gc_ID" :current="current" @click="m=>clk(m)" v-if="d.children && d.children.length>0" :datas="d.children" :level="level+1"></columns>

                    </template>
                </div>`
                },
            }
        });
    });
