$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            site: $api.dot("web"),    //电脑端为web,手机端为mobi
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项        
            datas: [],          //数据源

            edit_id: -1,     //当前编辑的id
            loading: false,
            loading_init: true,
            loading_id: -1       //当前更改行的id
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, organ) {
                vapp.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.getShowpic();
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //图片列表
            fileList: function () {
                var result = '';
                if (typeof (datas) != 'string')
                    result = JSON.stringify(this.datas);
                result = result.replace(/Shp_Intro/g, "name");
                result = result.replace(/Shp_File/g, "url");
                return JSON.parse(result);
            }
        },
        watch: {
        },
        methods: {
            //获取图片信息
            getShowpic: function (orgid) {
                var th = this;
                orgid = orgid ? orgid : th.organ.Org_ID;
                this.loading = true;
                $api.get('Showpic/All', { 'orgid': orgid, 'site': th.site }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.datas = req.data.result;
                        th.rowdrop();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },
            //行的拖动
            rowdrop: function () {
                // 首先获取需要拖拽的dom节点            
                const el1 = document.getElementById('imglist');
                Sortable.create(el1, {
                    disabled: false, // 是否开启拖拽
                    ghostClass: 'sortable-ghost', //拖拽样式
                    handle: '.imgitem',     //拖拽的操作元素
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
                        let old = $api.clone(this.datas); // 获取表数据
                        let arr = $api.clone(this.datas);
                        this.datas = [];
                        arr.splice(e.newIndex, 0, arr.splice(e.oldIndex, 1)[0]); // 数据处理
                        this.$nextTick(function () {
                            for (var i = 0; i < arr.length; i++) {
                                arr[i].Shp_Tax = i + 1;
                            }
                            var len = arr.length;
                            for (var i = 0; i < len; i++) {
                                for (var j = 0; j < len - 1 - i; j++) {
                                    if (arr[j].Shp_Tax > arr[j + 1].Shp_Tax) {  // 相邻元素两两对比
                                        var temp = arr[j + 1];  // 元素交换
                                        arr[j + 1] = arr[j];
                                        arr[j] = temp;
                                    }
                                }
                            }
                            this.datas = arr;
                            if ($api.toJson(old) != $api.toJson(arr)) {
                                this.changeTax();
                            }
                        });
                    }
                });
            },
            //更新排序
            changeTax: function () {
                if (this.datas.length <= 1) return;
                var th = this;
                var arr = $api.clone(this.datas);
                $api.post('Showpic/ModifyTaxis', { 'items': arr }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        console.log("操作成功");
                        th.$message({
                            message: '更改顺序成功',
                            type: 'success'
                        });
                        th.freshcache();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
                //console.log(arr);
            },
            //删除轮换图片项
            deleteImg: function (item) {
                var th = this;
                th.loading_id = item.Shp_ID;
                $api.delete('Showpic/Delete', { 'id': item.Shp_ID }).then(function (req) {
                    th.loading_id = -1;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.freshcache();
                        th.getShowpic();
                        th.$message({
                            message: '删除成功',
                            type: 'success'
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //更改显示状态
            changeShow: function (item, show) {
                item.Shp_IsShow = show;
                this.loading_id = item.Shp_ID;
                var th = this;
                $api.post('Showpic/Modify', { 'entity': item }).then(function (req) {
                    th.loading_id = -1;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            message: show ? '修改状态为启用' : '修改状态为禁用',
                            type: 'success'
                        });
                        th.freshcache(item);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //更新
            update: function (item) {
                this.loading_id = item.Shp_ID;
                var th = this;
                $api.post('Showpic/Modify', { 'entity': item }).then(function (req) {
                    th.loading_id = -1;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.edit_id = -1;
                        th.freshcache(item);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //上传新的轮换图片
            fileupload: function (file) {
                var th = this;
                $api.post('Showpic/AddPicture', { 'file': file, 'orgid': th.organ.Org_ID, 'site': th.site }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.freshcache();
                        th.getShowpic();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //更改轮换图片
            filechange: function (file, data, id) {
                var th = this;
                th.loading_id = id;
                $api.post('Showpic/ModifyPicture', { 'file': file, 'entity': data }).then(function (req) {
                    th.loading_id = -1;
                    if (req.data.success) {
                        var result = req.data.result;
                        for (var i = 0; i < th.datas.length; i++) {
                            if (th.datas[i].Shp_ID == result.Shp_ID) {
                                th.datas[i].Shp_File = result.Shp_File;
                                Vue.set(th.datas, i, th.datas[i]);
                            }
                        }
                        th.freshcache(data);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //字体颜色变化时
            colorChange: function (color) {
                var item = null;
                var index = 0;
                for (var i = 0; i < this.datas.length; i++) {
                    if (this.datas[i].Shp_ID == this.edit_id) {
                        item = this.datas[i];
                        index = i;
                        break;
                    }
                }
                if (item == null) return;
                item.Shp_BgColor = color == null ? '' : color;
                Vue.set(this.datas, index, item);
            },
            //刷新本地缓存
            freshcache: function (data) {
                var site, orgid;
                if (data == null) {
                    site = this.site;
                    orgid = this.organ.Org_ID;
                } else {
                    site = data.Shp_Site;
                    orgid = data.Org_ID;
                }
                $api.cache('Showpic/' + site + ':clear', { 'orgid': orgid });
            }
        }
    });

});
