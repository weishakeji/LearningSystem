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

            rules: {
                MM_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                MM_Link: [
                    { required: false, message: '不得为空', trigger: 'blur' }
                ]
            },
            MM_PatId: '',    //临时数据，用于移动菜单时的临时记录
            loading: false,  //预载
            loading_init: false,     //初始加载
            error: '',       //错误信息
            drawer: false  //编辑的面板         
        },
        watch: {
            drawer: function (nl, ol) {
                var th = this;
                if (ol && !nl) {
                    this.$refs['form'].validate((valid) => {
                        if (!valid) this.drawer = true;
                    });
                }
            }
        },
        created: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('ManageMenu/ForUID', { 'uid': this.uid }),
                $api.get('ManageMenu/FuncMenu', { 'uid': this.uid }),
                $api.get("ManageMenu/Root")     //所有根节点
            ).then(([menu, menus, root]) => {
                th.rootMenu = menu.data.result; //当前菜单项                
                if (menus.data.result != null)
                    th.data = menus.data.result; //当前菜单树             
                th.rootdata = root.data.result;
            }).catch(err => {
                th.error = err;
                console.error(err);
            }).finally(() => th.loading_init = false);
        },
        methods: {
            append: function (d) {
                var obj = this.clone();
                if (d != null) {
                    if (!d.children) this.$set(d, 'children', []);
                    d.children.push(obj);
                } else {
                    this.data.push(obj);
                }
            },
            //克隆一个新节点
            clone: function (data) {
                let isshow = this.rootMenu.MM_IsShow;
                let temp = {
                    "MM_Id": -1, "MM_Name": "", "MM_Type": "", "MM_Root": 0, "MM_Link": "",
                    "MM_Marker": "", "MM_Tax": 0, "MM_PatId": 0, "MM_Color": "",
                    "MM_Font": "", "MM_IsBold": false, "MM_IsItalic": false, "MM_IcoCode": "",
                    "MM_IcoSize": "", "MM_IsUse": true, "MM_IsShow": isshow, "MM_Intro": "",
                    "MM_IsChilds": false, "MM_Func": "func", "MM_WinWidth": 0, "MM_WinHeight": 0,
                    "MM_IcoX": 0, "MM_IcoY": 0, "MM_IcoColor": "", "MM_UID": "", "MM_WinMin": false,
                    "MM_WinMax": false, "MM_WinMove": false, "MM_WinResize": false, "MM_WinID": "",
                    "id": 0, "label": "", "ico": "", "MM_Complete": 0
                }
                let obj = $api.clone(temp);
                obj.MM_Id = obj.id = -parseInt(Math.random() * 9999, 10) + 1;
                obj.MM_Name = "newnode" + obj.id;
                obj.children = [];
                obj.MM_Type = 'item';
                obj.MM_Link = '';
                obj.MM_Root = this.rootMenu.MM_Root;
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

                }).catch(() => { });
            },
            //字体颜色变化时
            colorChange: function (color, field) {
                this.curr[field] = color == null ? '' : color;
            },
            //设置菜单文本样式
            setTextstyle: function (data) {
                let css = 'background-image: linear-gradient(to right, rgba(255, 255, 255,0) '
                    + (data.MM_IsUse ? data.MM_Complete : 100) + '%,rgb(255, 0, 0) ' + (100 - data.MM_Complete) + '%);';
                if (!$api.isnull(data.MM_Color) && data.MM_Color != '') css += 'color:' + data.MM_Color + ';';
                if (data.MM_IsBold) css += 'font-weight: bold;';
                if (data.MM_IsItalic) css += 'font-style: italic;';
                return css;
            },
            //设置图标样式
            setIcostyle: function (data) {
                let css = '';
                if (data.MM_IcoSize && data.MM_IcoSize != 0)
                    css += 'transform:' + 'scale(' + (1 + data.MM_IcoSize / 100) + ');';
                if (!$api.isnull(data.MM_IcoColor) && data.MM_IcoColor != '') css += 'color:' + data.MM_IcoColor + ';'
                css += 'margin-top:' + ($api.isnull(data.MM_IcoY) || data.MM_IcoY == 0 ? 0 : data.MM_IcoY) + 'px;';
                css += 'margin-left:' + ($api.isnull(data.MM_IcoX) || data.MM_IcoX == 0 ? 0 : data.MM_IcoX) + 'px;';
                //console.log(css);
                return css;
            },
            //保存菜单项
            btnSave: function () {
                if (this.loading || this.loading_init) return;
                var th = this;
                if (th.data.length < 1) {
                    this.$alert('您还没有创建菜单项，请点击右上方“新建下级”创建菜单项！', '没有数据');
                    return;
                }
                th.$confirm('将保存菜单树的修改, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(function () {
                    th.updateSave();
                }).catch(() => { });
            },
            //保存修改
            updateSave: function () {
                var th = this;
                th.loading = true;
                th.setIsShow(null);
                console.error(th.data);
                //return;
                $api.post('ManageMenu/FuncMenuUpdate', { 'uid': th.uid, 'tree': th.data }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        $api.cache('ManageMenu/Menus:clear');
                        th.$notify({
                            type: 'success', center: true,
                            message: '菜单保存成功！',
                            position: 'bottom-left'
                        });
                        th.updatedEvent();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //设置所有菜单项的isShow，与根菜单项的isShow保持一致，即this.rootMenu.MM_IsShow;
            setIsShow: function (list) {
                let isshow = this.rootMenu.MM_IsShow;
                if (list == null) list = this.data;
                for (var i = 0; i < list.length; i++) {
                    list[i].MM_IsShow = isshow;
                    if (list[i].children != null && list[i].children.length > 0)
                        this.setIsShow(list[i].children);
                }
            },
            //更改完成状态
            changeComplete: function (val) {
                var th = this;
                th.loading = true;
                $api.post('ManageMenu/UpdateComplete', { 'uid': th.curr.MM_UID, 'complete': val }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '完成度修改完成！',
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
                }).finally(() => th.loading = false);
            },
            //更新后触发的事件
            updatedEvent: function () {
                $api.cache('ManageMenu/OrganMarkerMenus:update', { 'marker': this.rootMenu.MM_Marker });
            }
        }
    });

});