$ready(function () {
    window.vapp = new Vue({
        el: '#app',
        data: {
            data: [],
            curr: {},    //当前要编辑的节点          
            defaultProps: {
                children: 'children',
                label: 'label'
            },
            rules: {
                MM_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ]
            },
            loading: false,  //预载
            drawer: false  //编辑的面板

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
            }
        },
        created: function () {
            this.getTreeData();
        },
        methods: {
            getTreeData: function () {
                this.loading = true;
                $api.get('ManageMenu/SystemMenu').then(function (req) {
                    if (req.data.success) {
                        window.vapp.data = req.data.result;
                        //...
                    } else {
                        throw req.data.message;
                    }
                    window.vapp.loading = false;
                }).catch(function (err) {
                    alert(err);
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
            }
            ,
            append: function (d) {
                var obj = this.clone();
                if (d != null) {
                    if (!d.children) {
                        this.$set(d, 'children', []);
                    }
                    d.children.push(obj);
                } else {
                    this.data.push(obj);
                }
            },
            //克隆一个新节点
            clone: function (data) {
                var temp = {
                    "MM_Id": -1, "MM_Name": "",
                    "MM_Type": "", "MM_Root": 103,
                    "MM_Link": "", "MM_Marker": "",
                    "MM_Tax": 0, "MM_PatId": 0,
                    "MM_Color": "", "MM_Font": "",
                    "MM_IsBold": false, "MM_IsItalic": false,
                    "MM_IcoS": "", "MM_IcoB": "",
                    "MM_IsUse": true, "MM_IsShow": true,
                    "MM_Intro": "", "MM_IsChilds": false,
                    "MM_Func": "sys", "MM_WinWidth": 0,
                    "MM_WinHeight": 0, "MM_IcoX": 0,
                    "MM_IcoY": 0, "MM_UID": "",
                    "MM_WinMin": false, "MM_WinMax": false,
                    "MM_WinMove": false, "MM_WinResize": false,
                    "MM_WinID": "", "id": 0,
                    "label": "", "ico": ""
                }
                var obj = $api.clone(temp);
                obj.MM_Id = obj.id = -parseInt(Math.random() * 9999, 10) + 1;
                obj.MM_Name = "newnode" + obj.id;
                obj.children = [];
                obj.MM_Type = 'item';
                obj.MM_Link = '';
                if (data != null) {
                    obj.MM_PatId = data.MM_Id;
                }
                return obj;
            },
            remove(node, data) {
                const parent = node.parent;
                const children = parent.data.children || parent.data;
                const index = children.findIndex(d => d.id === data.id);
                children.splice(index, 1);
            },
            //字体颜色变化时
            colorChange: function (color) {
                this.curr.MM_Color = color == null ? '' : color;
            },
            //当图标选择变更时
            iconChange: function (icon) {
                this.curr.MM_IcoS = icon;
                console.log('选中图标:' + icon);
            },
            //保存菜单项
            btnSave: function () {
                if (this.loading) return;
                this.$confirm('将保存系统菜单的修改, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(function () {
                    window.vapp.updateSave();
                }).catch(function () {
                });
            },
            updateSave: function () {
                this.loading = true;
                $api.post('ManageMenu/SystemMenuUpdate', { 'tree': this.data }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        window.vapp.$notify({
                            type: 'success',
                            message: '系统菜单保存成功！',
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
                    alert(err);
                    console.error(err);
                });
            }
        }
    });

});