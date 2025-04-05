
$ready(function () {

    window.vue = new Vue({
        el: '#app',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            defaultProps: {
                children: 'children',
                label: 'label'
            },
            datas: [] //数据源          
        },
        created: function () {
            var th = this;
            th.loading = true;
            //获取所有供选择的菜单项
            $api.get('ManageMenu/OrganPurviewSelect').then(function (req) {
                if (req.data.success) {
                    th.datas = req.data.result;
                    console.error(th.datas);
                    //获取已经选择的菜单项
                    $api.get('ManageMenu/OrganPurviewUID', { 'lvid': th.id }).then(function (req) {
                        if (req.data.success) {
                            var arr = req.data.result;
                            for (var i = 0; i < arr.length; i++)
                                arr[i] = 'node_' + arr[i];
                            window.setTimeout(function () {
                                var trees = window.vue.$refs.tree;
                                for (var i = 0; i < trees.length; i++)
                                    trees[i].setCheckedKeys(arr, true);
                            }, 100);
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(err => alert(err))
                        .finally(() => th.loading = false);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => alert(err))
                .finally(() => { });
        },
        methods: {
            btnEnter: function (isclose) {
                var th = this;
                if (th.loading) return;
                th.loading = true;
                //选中的菜单项
                var arr = new Array();
                var trees = th.$refs.tree;
                for (var i = 0; i < trees.length; i++) {
                    var nodes = trees[i].getCheckedNodes(true, false);
                    for (var j = 0; j < nodes.length; j++)
                        arr.push(nodes[j].MM_UID);
                }
                $api.post('ManageMenu/OrganPurviewSelected', { 'lvid': th.id, 'mms': arr })
                .then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success', center: true,
                            message: '操作成功!'
                        });
                        th.operateSuccess(isclose);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => alert(err))
                    .finally(() => th.loading = false);
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
            //全选或清空
            selected: function (root, index) {
                var arr = new Array();
                if (root.MM_IsBold) {
                    for (var i = 0; i < root.children.length; i++)
                        arr.push(root.children[i].id);
                }
                this.$refs.tree[index].setCheckedKeys(arr);
            },
            //操作成功
            operateSuccess: function (isclose) {
                //更新后触发的事件
                for (let i = 0; i < this.datas.length; i++) {
                    $api.cache('ManageMenu/OrganMarkerMenus:update', { 'marker': this.datas[i].MM_Marker });
                }
                if (window.top && window.top.$pagebox)
                    window.top.$pagebox.source.tab(window.name, 'vapp.getlist', isclose);
            }
        },
    });

});
