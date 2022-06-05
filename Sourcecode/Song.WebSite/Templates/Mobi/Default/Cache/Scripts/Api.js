$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            active: 0,         //选项卡索引 
            active_name: 'all',    //选项卡名称
            stores: []

        },
        mounted: function () {
            this.tabChange(0, 'all');
            this.initload();
        },
        created: function () {
        },
        computed: {
            //共有多少个缓存数据记录
            itemtotal: function () {
                var total = 0;
                for (var i = 0; i < this.stores.length; i++) {
                    total += this.stores[i].items.length;
                }
                return total;
            },
            //所有缓存库，缓存记录为空的不显示
            allitems: function () {
                var items = [];
                for (var i = 0; i < this.stores.length; i++) {
                    var item = this.stores[i];
                    var arr = []; //临时记录
                    for (var j = 0; j < item.items.length; j++) {
                        var data = item.items[j];
                        arr.push(data);
                    }
                    if (arr.length > 0) {
                        item.items = arr;
                        items.push(item);
                    }
                }
                return items;
            },
            //过期的数据项
            expiresItems: function () {
                var items = [];
                for (var i = 0; i < this.stores.length; i++) {
                    var item = this.stores[i];
                    var arr = []; //临时记录
                    for (var j = 0; j < item.items.length; j++) {
                        var data = item.items[j];
                        if (data.expires.time < new Date()) {
                            arr.push(data);
                        }
                    }
                    if (arr.length > 0) {
                        item.items = arr;
                        items.push(item);
                    }
                }
                return items;
            },
            //强制缓存
            compelItems: function () {
                var items = [];
                for (var i = 0; i < this.stores.length; i++) {
                    var item = this.stores[i];
                    var arr = []; //临时记录
                    for (var j = 0; j < item.items.length; j++) {
                        var data = item.items[j];
                        if (data.compel) arr.push(data);
                    }
                    if (arr.length > 0) {
                        item.items = arr;
                        items.push(item);
                    }
                }
                return items;
            }
        },
        watch: {
        },
        methods: {
            //初始化载入
            initload: function () {
                this.stores = [];
                //取所有缓存的存储空间
                $api.api_cache.stores().then(function (data) {
                    //console.log(data);
                    for (var i = 0; i < data.length; i++) {
                        vapp.stores.push({
                            'store': data[i],
                            'items': []
                        });
                    }
                    for (var i = 0; i < vapp.stores.length; i++) {
                        getall(vapp.stores[i]);
                    }

                }).catch(function () {

                });
                //取缓存数据项
                function getall(item) {
                    $api.api_cache.getall(item.store).then(function (d) {
                        item.items = d;
                    });
                }
            },
            //选项卡切换,index没有用，title为选项卡标识，作为排序类型用
            tabChange: function (index, title) {
                var func = 'this.show_' + title;
                this.active_name = title;               
            },            
            //过期的记录数
            expirestotal: function () {
                var arr = this.expiresItems;
                var total = 0;
                for (var i = 0; i < arr.length; i++) {
                    total += arr[i].items.length;
                }
                return total;
            },
            //强制缓存记录数
            compeltotal: function () {
                var arr = this.compelItems;
                var total = 0;
                for (var i = 0; i < arr.length; i++) {
                    total += arr[i].items.length;
                }
                return total;
            },
            //清空，store：存储空间名称，expires：是否只清理过期的，默认是false，即清理所有
            clear: function (store, expires) {
                var msg = ''
                msg += store == null ? '是否清空所有的' : '是否清空缓存库 ' + store + ' 的';
                msg += expires ? '过期缓存数据？' : '缓存数据（强制缓存除外）？';
                this.$dialog.confirm({
                    title: '清理缓存',
                    message: msg,
                }).then(() => {
                    //清空所有,保留强制缓存
                    if (expires == false) {
                        $api.api_cache.reset(store, true);
                    }
                    //清空过期
                    if (expires == true) {
                        $api.api_cache.clear(store);
                    }
                    this.$notify({ type: 'success', message: '清理成功！' });
                    this.initload();
                    window.setTimeout(function () {
                        window.location.reload();
                    }, 1000);
                }).catch(function () {

                });
            }
        }
    });

}, ['../Components/page_header.js']);
