$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),     //课程ID
            uid: $api.querystring('uid'),       //课程的uid         

            course: {},  //当前课程
            datas: [],              //章节


            expanded: [],        //树形默认展开的节点
            expanded_storage: 'outline_for_admin_tree' + $api.querystring('id'),  //用于记录展开节点的storage名称
            filterText: '',      //查询过虑树形的字符
            total: 0,     //章节总数

            live_show: false,            //直播的编辑显示

            loading: false,
            loading_init: false,
            loadingid: -1,
            loading_sumbit: false,   //提交时的预载          

        },
        mounted: function () {

        },
        created: function () {
            var th = this;
            th.loading_init = true;
            $api.get('Course/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    th.course = req.data.result;
                    document.title += " - " + th.course.Cou_Name;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            }).finally(() => th.loading_init = false);
            th.getTreeData(true);
        },
        computed: {
        },
        watch: {
            filterText: function (val) {
                this.$refs.tree.filter(val);
            }
        },
        methods: {
            //所取章节数据，为树形数据
            //showloading:是否出现预载效果
            getTreeData: function (showloading) {
                var th = this;
                if (showloading) th.loading = true;
                $api.put('Outline/Tree:update', { 'couid': th.id, 'isuse': null }).then(function (req) {
                    if (req.data.success) {
                        th.datas = req.data.result;
                        //console.log(th.datas);
                        //获取默认展开的节点
                        var arr = $api.storage(th.expanded_storage);
                        if ($api.getType(arr) == 'Array') {
                            th.expanded = arr;
                        }
                        th.total = 0;
                        th.calcSerial(null, '');
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
                $api.cache('Outline/TreeList:clear', { 'couid': th.id });
            },
            //拖动节点改变顺序
            handleDragEnd: function (draggingNode, dropNode, dropType, ev) {
                var th = this;
                th.loading_sumbit = true;
                var arr = th.tree2array(this.datas);
                $api.post('Outline/ModifyTaxis', { 'list': arr }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: '更改排序成功!',
                            center: true
                        });
                        th.updatedEvent(true);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading_sumbit = false);

            },
            //节点展开事件
            nodeexpand: function (data, node, tree) {
                this.expanded.push(data.Ol_ID);
                $api.storage(this.expanded_storage, this.expanded);
            },
            //节点折叠事件
            nodecollapse: function (data, node, tree) {
                var index = this.expanded.indexOf(data.Ol_ID);
                if (index > -1) {
                    this.expanded.splice(index, 1);
                    $api.storage(this.expanded_storage, this.expanded);
                }
            },
            //过滤树形
            filterNode: function (value, data) {
                if (!value) return true;
                var txt = $api.trim(value.toLowerCase());
                if (txt == '') return true;
                return data.Ol_Name.toLowerCase().indexOf(txt) !== -1;
            },
            //修改状态
            changeState: function (data, field) {
                data[field] = !data[field];
                var th = this;
                if (th.loadingid > 0) return;
                //let entity = $api.clone(data);
                this.loadingid = data.Ol_ID;
                $api.post('Outline/ModifyState',
                    { 'couid': th.id, 'id': data.Ol_ID, 'use': data.Ol_IsUse, 'finish': data.Ol_IsFinish, 'free': data.Ol_IsFree })
                    .then(function (req) {
                        if (req.data.success) {
                            th.$message({
                                type: 'success', center: true,
                                message: '修改状态成功!'
                            });
                            th.updatedEvent();
                        } else {
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err, '错误');
                    }).finally(() => th.loadingid = -1);
            },
            //将树形数据转到数据列表，用于递交到服务端更改专业的排序
            tree2array: function (datas) {
                var list = [];
                list = toarray(datas, 0, 1, list);
                return list;
                function toarray(arr, pid, level, list) {
                    for (let i = 0; i < arr.length; i++) {
                        const d = arr[i];
                        var obj = {
                            'Ol_ID': d.Ol_ID,
                            'Ol_PID': pid,
                            'Ol_Tax': i + 1,
                            'Ol_Level': level
                        }
                        list.push(obj);
                        if (d.children && d.children.length > 0) {
                            list = toarray(d.children, d.Ol_ID, ++level, list);
                        }
                    }
                    return list;
                }
            },
            remove: function (node, data) {
                if (!!data.children && data.children.length > 0) {
                    var msg = '当前章节“' + data.Ol_Name + '”下共有 <b>' + data.children.length + '</b> 个子章节，请先删除子章节。'
                    this.$alert(msg, '不可删除', {
                        confirmButtonText: '确定',
                        dangerouslyUseHTMLString: true,
                        callback: () => { }
                    });
                    return;
                }
                var th = this;
                th.loading_sumbit = true;
                $api.delete('Outline/Delete', { 'id': data.Ol_ID }).then(function (req) {
                    th.loading_sumbit = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: '删除成功!',
                            center: true
                        });
                        th.updatedEvent();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading_sumbit = false);
            },
            //计算序号，并将时间值从字符串转为时间对象
            calcSerial: function (outline, lvl) {
                var childarr = outline == null ? this.datas : (outline.children ? outline.children : null);
                if (childarr == null) return;
                for (let i = 0; i < childarr.length; i++) {
                    childarr[i].Ol_ModifyTime = new Date(childarr[i].Ol_ModifyTime);
                    childarr[i].Ol_LiveTime = new Date(childarr[i].Ol_LiveTime);
                    childarr[i].serial = lvl + (i + 1) + '.';
                    this.total++;
                    this.calcSerial(childarr[i], childarr[i].serial);
                }
            },
            //打开弹窗，编辑子级内容，例如直播、附件、视频等
            openbox: function (outline, url, title, width, height, icon) {
                let pagebox = window.top.$pagebox;
                if (pagebox == null) {
                    return this.$message.error({
                        message: '没有$pagebox对象，无法打开编辑窗！',
                        center: true
                    });
                }
                // 要打开的页面
                let root = String(window.document.location.href);
                let href = root.substring(0, root.lastIndexOf("/") + 1) + url;
                //标题
                let tit = title + (outline ? ' - ' + outline.Ol_Name : '');
                window.top.$pagebox.create({
                    'url': $api.url.set(href,
                        {
                            'couid': outline ? outline.Cou_ID : this.id,
                            'olid': outline ? outline.Ol_ID : '',
                            'uid': outline ? outline.Ol_UID : ''
                        }),
                    'pid': window.name, 'min': false, 'full': false, 'showmask': true,
                    'title': tit, 'ico': icon,
                    'width': width, 'height': height
                }).open();
            },
            //更新后触发的事件
            //freshall:是否刷新所有章节
            updatedEvent: function (freshall) {
                this.close_fresh('vapp.freshrow("' + this.id + '")');
                $api.cache('Outline/Tree:update', { 'couid': this.id, 'isuse': true });
                var th = this;
                th.getTreeData(false);

            },
            //关闭自身窗体，并刷新父窗体列表
            close_fresh: function (func) {
                //如果有选项卡组件，就处理选项卡页面中的事件
                if (window.top.$tabs) {
                    window.top.$pagebox.source.tab(window.name, func, false);
                } else {
                    //如果处在学员或教师管理界面
                    var winname = window.name;
                    if (winname.indexOf('_') > -1)
                        winname = winname.substring(0, winname.lastIndexOf('_'));
                    if (winname.indexOf('[') > -1)
                        winname = winname.substring(0, winname.lastIndexOf('['));
                    if (window.top.vapp && window.top.vapp.fresh)
                        window.top.vapp.fresh(winname, func);
                }
            },
        }
    });
}, []);
