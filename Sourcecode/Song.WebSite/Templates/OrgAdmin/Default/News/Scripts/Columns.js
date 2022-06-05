$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项   

            datas: [],
            curr: {},    //当前要编辑的节点          
            defaultProps: {
                children: 'children',
                label: 'label'
            },
            filterText: '',      //查询过虑树形的字符
            rules: {
                Col_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ]
            },
            loading: false,  //预载
            drawer: false,  //编辑的面板
            iconSelect: false    //图标选择
        },
        watch: {
            drawer: function (nl, ol) {
                var th = this;
                if (ol && !nl) {
                    this.$refs['form'].validate((valid) => {
                        if (!valid) {
                            this.drawer = true;
                        }
                    });
                }
            },
            filterText: function (val) {
                this.$refs.tree.filter(val);
            }
        },
        created: function () {
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
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
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
                vapp.getTreeData();
            })).catch(function (err) {
                console.error(err);
            });

        },
        methods: {
            getTreeData: function () {
                var th = this;
                this.loading = true;
                $api.get('News/ColumnsTree', { 'orgid': vapp.organ.Org_ID }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        window.vapp.datas = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                    window.vapp.loading = false;
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            },
            handleDragStart(node, ev) {
                //console.log('drag start', node);
            },
            handleDragEnter(draggingNode, dropNode, ev) {
                //console.log('tree drag enter: ', dropNode.label);
            },
            handleDragLeave(draggingNode, dropNode, ev) {
                //console.log('tree drag leave: ', dropNode.label);
            },
            handleDragOver(draggingNode, dropNode, ev) {
                //console.log('tree drag over: ', dropNode.label);
            },
            handleDragEnd(draggingNode, dropNode, dropType, ev) {
                //console.log('tree drag end: ', dropNode && dropNode.label, dropType);
                //console.log(this.data);
            },
            handleDrop(draggingNode, dropNode, dropType, ev) {
                console.log('tree drop: ', dropNode.label, dropType);
            },
            allowDrop(draggingNode, dropNode, type) {
                return true;
            },
            allowDrag(draggingNode) {
                return true;
            },
            append: function (d) {
                var obj = this.clone();
                if (d != null) {
                    if (!d.children) {
                        this.$set(d, 'children', []);
                    }
                    d.children.push(obj);
                } else {
                    this.datas.push(obj);
                }
            },
            //克隆一个新节点
            clone: function (data) {
                var temp = {
                    "Col_ID": -1,
                    "Col_PID": "",
                    "Col_Name": "newnode",
                    "Col_ByName": "",
                    "Col_Title": "",
                    "Col_Keywords": "",
                    "Col_Descr": "",
                    "Col_Intro": "",
                    "Col_Type": "News",
                    "Col_Tax": 0,
                    "Col_IsUse": true,
                    "Col_IsNote": true,
                    "Org_ID": this.organ.Org_ID,
                    "Org_Name": this.organ.Org_Name,
                    "Col_UID": String(new Date().getTime()),
                }
                var obj = $api.clone(temp);
                obj.Col_ID = obj.id = -new Date().getTime();             
                obj.children = [];
                if (data != null) {
                    obj.Col_PID = data.Col_UID;
                }
                return obj;
            },
            remove(node, data) {
                const parent = node.parent;
                const children = parent.data.children || parent.data;
                const index = children.findIndex(d => d.id === data.id);
                children.splice(index, 1);
            },
            //保存菜单项
            btnSave: function () {
                if (this.loading) return;
                this.$confirm('将保存新闻栏目的修改, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(function () {
                    window.vapp.updateSave();
                }).catch(function () {
                });
            },
            updateSave: function () {
                var th=this;
                th.loading = true;
                $api.post('News/ColumnsUpdate', { 'tree': th.datas,"orgid": th.organ.Org_ID}).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        window.vapp.$notify({
                            type: 'success',
                            message: '新闻栏目编辑成功！',
                            position: 'bottom-left',
                            center: true
                        });
                        $api.put('ManageMenu/SystemMenuShow');
                        $api.cache('ManageMenu/SystemMenuShow:update');
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                    window.vapp.loading = false;
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            },
            //过滤树形
            filterNode: function (value, data) {
                if (!value) return true;
                var txt = $api.trim(value.toLowerCase());
                if (txt == '') return true;
                return data.Col_Name.toLowerCase().indexOf(txt) !== -1;
            },
        }
    });

});