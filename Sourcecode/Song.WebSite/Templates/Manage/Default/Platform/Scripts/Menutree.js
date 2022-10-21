$ready(function () {
    window.vapp = new Vue({
        el: '#app',
        data: {
            uid: $api.querystring('uid'),
            rootMenu: {},    //根菜单
            rootdata: [],    //所有的根菜单
            data: [],       //菜单树数据
            curr: {},    //当前要编辑的节点
            curr_node: {},
            defaultProps: {
                children: 'children',
                label: 'label'
            },
            rules: {
                MM_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ]
            },
            MM_PatId: '',    //临时数据，用于移动菜单时的临时记录
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
            }
        },
        created: function () {
            var th = this;
            $api.bat(
                $api.get('ManageMenu/ForUID', { 'uid': this.uid }),
                $api.get('ManageMenu/FuncMenu', { 'uid': this.uid }),
                $api.get("ManageMenu/Root")     //所有根节点
            ).then(axios.spread(function (menu, menus, root) {
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw data.message;
                    }
                }
                th.rootMenu = menu.data.result; //当前菜单项
                if (menus.data.result != null)
                    th.data = menus.data.result; //当前菜单树             
                th.rootdata = root.data.result;
            })).catch(function (err) {
                console.error(err);
            });
        },
        methods: {
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
                    "MM_Id": -1, "MM_Name": "", "MM_Type": "", "MM_Root": 0, "MM_Link": "",
                    "MM_Marker": "", "MM_Tax": 0, "MM_PatId": 0, "MM_Color": "",
                    "MM_Font": "", "MM_IsBold": false, "MM_IsItalic": false, "MM_IcoS": "",
                    "MM_IcoB": "", "MM_IsUse": true, "MM_IsShow": false, "MM_Intro": "",
                    "MM_IsChilds": false, "MM_Func": "func", "MM_WinWidth": 0, "MM_WinHeight": 0,
                    "MM_IcoX": 0, "MM_IcoY": 0, "MM_UID": "", "MM_WinMin": false,
                    "MM_WinMax": false, "MM_WinMove": false, "MM_WinResize": false, "MM_WinID": "",
                    "id": 0, "label": "", "ico": "", "MM_Complete": 0
                }
                var obj = $api.clone(temp);
                obj.MM_Id = obj.id = -parseInt(Math.random() * 9999, 10) + 1;
                obj.MM_Name = "newnode" + obj.id;
                obj.children = [];
                obj.MM_Type = 'item';
                obj.MM_Link = '';
                obj.MM_Root = this.rootMenu.MM_UID;
                obj.MM_UID = String(new Date().getTime());
                if (data != null) {
                    obj.MM_PatId = data.MM_UID;
                }
                return obj;
            },
            remove(node, data) {
                const parent = node.parent;
                const children = parent.data.children || parent.data;
                const index = children.findIndex(d => d.id === data.id);
                children.splice(index, 1);
            },
            //移到其它根菜单
            move2Root: function (uid) {
                var th = this;
                const root = this.rootdata.find(d => d.MM_UID === uid);
                var msg = "是否将“" + this.curr.MM_Name + "”移动到“" + root.MM_Name + "”?";
                this.$confirm(msg, '确认', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(function () {
                    $api.post('ManageMenu/FuncMoveRoot', { 'cuid': th.curr.MM_UID, 'puid': uid }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.remove(th.curr_node, th.curr);
                            th.curr.MM_Name = '_';
                            th.drawer = false;
                            th.MM_PatId = '';
                            th.$notify({
                                type: 'success',
                                message: '菜单移动成功！',
                                position: 'bottom-left',
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
                    });

                }).catch(function () {
                });
               
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
                this.$confirm('将保存菜单树的修改, 是否继续?', '提示', {
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
                $api.post('ManageMenu/FuncMenuUpdate', { 'uid': this.uid, 'tree': this.data }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        $api.cache('ManageMenu/Menus:clear');
                        th.$notify({
                            type: 'success',
                            message: '菜单保存成功！',
                            position: 'bottom-left',
                            center: true
                        });
                        th.updatedEvent();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                  
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            },
            //更改完成状态
            changeComplete: function (val) {
                var th = this;
                $api.post('ManageMenu/UpdateComplete', { 'uid': this.curr.MM_UID, 'complete': val }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '完成度修改完成！',
                            position: 'bottom-right',
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
                });
            },
            //更新后触发的事件
            updatedEvent: function () {
                $api.cache('ManageMenu/OrganMarkerMenus:update', { 'marker': this.rootMenu.MM_Marker });
            }
        }
    });

});