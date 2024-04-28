$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项    
            site: $api.dot("web"),    //电脑端为web,手机端为mobi
            type: $api.querystring("type", "main"),   //主菜单为main,底部为foot

            drawer: false,       //是否显示详情
            curr: {},            //当前要显示的项           
            defaultProps: {
                children: 'children',
                label: 'label'
            },
            data_main: [],       //导航
            data_foot: [],        //底部导航
            rules: {
                Nav_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ]
            },
            loading: false,
            loading_sumbit: false,   //提交时的预载
            loading_up: false,       //上传图片的预载
            loading_init: true,
            loading_id: -1           //正在操作id
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, organ) {
                //获取结果          
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                //获取导航菜单
                th.getdata();
            })).catch(err => console.error(err))
                .finally(() => th.loading_init = false);;
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
            //选项卡（也是导航分类）切换时
            'type': function (nv, ov) {
                var href = window.location.href;
                var url = $api.url.set(href, { 'type': nv });
                window.history.pushState({}, '', url);
            }
        },
        methods: {
            //获取树形导航菜单
            getdata: function () {
                var th = this;
                var path = 'Navig/' + th.site + 'all';
                $api.bat(
                    $api.get(path, { 'orgid': th.organ.Org_ID, 'type': 'main' }),
                    $api.get(path, { 'orgid': th.organ.Org_ID, 'type': 'foot' })
                ).then(axios.spread(function (main, foot) {
                    //获取结果
                    th.data_main = main.data.result ? main.data.result : [];
                    th.data_foot = foot.data.result ? foot.data.result : [];
                })).catch(err => console.error(err))
                    .finally(() => { });
            },
            //保存主菜单
            btnSave: function (datas) {
                var th = this;
                this.$confirm('保存导航菜单更改?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    th.submitSave(datas);
                }).catch(() => { });
            },
            //提交保存事件
            submitSave: function (datas) {
                var th = this;
                th.loading_sumbit = true;
                var query = { 'site': this.site, 'type': this.type, 'orgid': this.organ.Org_ID, 'items': datas };
                $api.post('Navig/ModifyMenus', query).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            message: '操作成功',
                            type: 'success'
                        });
                        th.freshcache();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading_sumbit = false);
            },
            append: function (parent, datas) {
                var obj = this.clone();
                obj.Nav_PID = parent ? parent.Nav_UID : '';
                if (parent != null) {
                    if (!parent.children) {
                        this.$set(parent, 'children', []);
                    }
                    parent.children.push(obj);
                } else {
                    datas.push(obj);
                }
                console.log(obj);
            },
            //克隆一个新节点
            clone: function (data) {
                var temp = {
                    "Nav_ID": -1,
                    "Nav_PID": '',
                    "Nav_Name": "课程",
                    "Nav_EnName": "",
                    "Nav_Url": "",
                    "Nav_Target": "",
                    "Nav_Event": "",
                    "Nav_Image": "",
                    "Nav_Color": "",
                    "Nav_Font": "",
                    "Nav_Child": 0,
                    "Nav_Tax": 0,
                    "Nav_Intro": "",
                    "Nav_Title": "",
                    "Nav_Type": "main",
                    "Nav_Site": "web",
                    "Nav_IsShow": true,
                    "Nav_IsBold": false,
                    "Nav_Logo": "",
                    "Nav_Icon": "",
                    "id": 0,
                    "label": "",
                    "ico": ""
                }
                var obj = $api.clone(temp);
                obj.Nav_ID = obj.id = -parseInt(Math.random() * 9999, 10) + 1;
                obj.Nav_Name = "newnode" + obj.id;
                obj.children = [];
                obj.Nav_Type = this.type;
                obj.Nav_Site = this.site;
                obj.Nav_Url = '';
                if (data != null) {
                    obj.Nav_PID = data.Nav_ID;
                }
                return obj;
            },
            //移除，并删除
            remove(node, data) {
                const parent = node.parent;
                const children = parent.data.children || parent.data;
                const index = children.findIndex(d => d.id === data.id);
                children.splice(index, 1);
                //console.log(data);
                var th = this;
                $api.delete('Navig/Delete', { 'id': data.Nav_ID }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            message: '删除成功',
                            type: 'success'
                        });
                        //刷新本地缓存
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
            //更改导航菜单的显示状态
            changeState: function (item) {
                item.Nav_IsShow = !item.Nav_IsShow;
                if (item.Nav_ID <= 0) {
                    return this.$notify({
                        title: '警告',
                        message: '新增项更改状态后，请点击右下方的“保存导航菜单”的按钮',
                        type: 'warning'
                    });
                }
                var th = this;
                th.loading_id = item.Nav_ID;
                $api.post('Navig/ModifyState', { 'id': item.Nav_ID, 'show': item.Nav_IsShow }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.freshcache(item);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_id = -1);
            },
            //图片上传
            fileupload: function (file, data, id) {
                var th = this;
                th.loading_up = true;
                $api.post('Navig/ModifyLogo', { 'file': file, 'entity': data }).then(function (req) {
                    th.loading_up = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        for (var i = 0; i < th.data_main.length; i++) {
                            if (th.data_main[i].Nav_ID == result.Nav_ID) {
                                th.data_main[i].Nav_Logo = result.Nav_Logo;
                                Vue.set(th.data_main, i, th.data_main[i]);
                            }
                        }
                        for (var i = 0; i < th.data_foot.length; i++) {
                            if (th.data_foot[i].Nav_ID == result.Nav_ID) {
                                th.data_foot[i].Nav_Logo = result.Nav_Logo;
                                Vue.set(th.data_foot, i, th.data_foot[i]);
                            }
                        }
                        data.Nav_Logo = result.Nav_Logo;
                        //刷新本地缓存
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
            //清除logo
            clearlogo: function (item) {
                var th = this;
                th.loading_up = true;
                $api.delete('Navig/ClearLogo', { 'item': item }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        item.Nav_Logo = "";
                        //刷新本地缓存
                        th.freshcache(item);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading_up = false);
            },
            //刷新本地缓存
            freshcache: function (data) {
                var site, orgid;
                if (data == null) {
                    site = this.site;
                    orgid = this.organ.Org_ID;
                } else {
                    site = data.Nav_Site;
                    orgid = data.Org_ID;
                }
                $api.cache('Navig/' + site + ':clear', { 'orgid': orgid, 'type': 'main' });
            },
            //鼠标滑过Logo，显示大图
            hoverlogo: function (logo) {
                if (logo == '') return;
                var img = $dom("#logo_img_hover");
                if (img == null || img.length < 1) {
                    img = $dom("body").add("img");
                    img.attr("id", "logo_img_hover");
                }
                img.show();
                var mouse = $dom.mouse();
                img.css("top", mouse.y + 'px');
                img.css("left", mouse.x + 'px');
                img.attr("src", logo);

            },
            //鼠标滑离图片，隐藏预览
            mouseleave: function () {
                var img = $dom("#logo_img_hover");
                img.hide();
            }
        }
    });

});
