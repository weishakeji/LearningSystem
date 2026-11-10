$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            datatypes: [],       //数据类型
            tables: [],          //表

            //查询条件
            form: {
                dbtype: '',
                table: '',
                field: ''
            },
            results: [],     //查询结果
            tabletotal: 0,     //表总数
            fieldtotal: 0,     //字段总数

            loadstate: {
                init: false,        //初始化
                query: false,         //查询              
            }
        },
        mounted: function () {
            this.getdatatypes();
            this.onSubmit()
        },
        created: function () {

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
            },
            //判断查询条件是否为空
            isEmpty: function () {
                var form = this.form;
                return Object.values(form).every(value => {
                    if (value === null || value === undefined) return true;
                    if (typeof value === 'string') return value.trim() === '';
                    if (Array.isArray(value)) return value.length === 0;
                    if (typeof value === 'object') return Object.keys(value).length === 0;
                    return false;
                });
            }
        },
        watch: {
            form: {
                handler: function () {
                    this.onSubmit();
                }, deep: true
            }
        },
        methods: {
            //获取数据库类型
            getdatatypes: function () {
                var th = this;
                th.loadstate.init = true;
                $api.bat(
                    $api.cache("DataBase/FieldDataTypes"),
                    $api.cache("DataBase/Tables")
                ).then(([types, tables]) => {
                    th.datatypes = types.data.result;
                    th.tables = tables.data.result;
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.init = false);
            },
            //查询
            onSubmit: function () {
                var th = this;
                th.tabletotal = 0;
                th.fieldtotal = 0;
                //if (th.isEmpty) return;
                th.loadstate.query = true;
                $api.get("DataBase/FieldQuery", th.form)
                    .then(req => {
                        if (req.data.success) {
                            th.results = req.data.result;
                            //计算表与字段的数总
                            let tabletotal = 0;
                            let fieldtotal = 0;
                            for (let key in th.results) {
                                if (th.results.hasOwnProperty(key)) {
                                    tabletotal++;
                                    fieldtotal += th.results[key].length;
                                }
                            }
                            th.tabletotal = tabletotal;
                            th.fieldtotal = fieldtotal;

                            //console.error(th.results);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                    .finally(() => th.loadstate.query = false);
            }
        },
        filters: {

        },
        components: {

        }
    });
});