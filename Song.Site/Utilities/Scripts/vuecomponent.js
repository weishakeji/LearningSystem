// 页面顶部的按钮条
//参数：
//show:显示哪些按钮，中文加逗号
//selects: 选中的数据，用于编辑或删除时
//idkey:数据对象中的ID的键名，用于取selects中的id值
//path:要打开的窗体的页面路路，width和height即窗口宽高
Vue.component('btngroup', {
    props: ['show', 'selects', 'idkey', 'path', 'width', 'height'],
    data: function () {
        // data 选项是一个函数，组件不相互影响
        return {
            buttons: [{
                text: '新增',
                tips: '新增',
                id: 'add',
                type: 'success',
                class: 'el-icon-plus'
            }, {
                text: '修改',
                tips: '修改数据，请选择数据行',
                id: 'modify',
                type: 'primary',
                class: 'el-icon-edit'
            }, {
                text: '删除',
                tips: '批量删除',
                id: 'delete',
                type: 'danger',
                class: 'el-icon-delete'
            }, {
                text: '导入',
                tips: '批量导入数据',
                id: 'input',
                type: 'info',
                class: 'el-icon-folder-add'
            }, {
                text: '导出',
                tips: '批量导出数据',
                id: 'output',
                type: 'info',
                class: 'el-icon-sell'
            }]
        }
    },
    watch: {
        'selects': function (val, old) {
            //console.log(val);
        }
    },
    methods: {
        //显示哪些按钮 btnname为按钮名称
        visible: function (btnname) {
            if (this.show == null) return true;
            if (btnname == this.show) return true;
            var arr = this.show.split(',');
            for (var t in arr) {
                if (btnname == arr[t]) return true;
            }
            return false;
        },
        eventClick: function (btnid) {
            //当前点击的按钮
            var curr = this.getCurrbtn(btnid);
            if (btnid == 'add' || btnid == 'modify') {
                if (!top.$pagebox) {
                    this.$message({
                        message: '未找到pagebox.js对象',
                        type: 'error'
                    });
                    return;
                }
            }
            if (btnid == 'add') this.add();     //添加            
            if (btnid == 'modify') this.modify(this.getid());       //修改            
            if (btnid == 'delete') return this.delete(this.getids(), curr);    //删除
            this.$emit(btnid, {}, curr);
        },
        //添加按钮事件
        add: function () {
            if (!(top.$pagebox && this.path)) return;
            this.pagebox(this.path, '新增', window.name + '[add]', this.width, this.height);
        },
        //修改事件
        modify: function (id) {
            if (id == '') {
                this.$message({
                    message: '请选中要编辑的数据行',
                    type: 'error'
                });
                return;
            }
            if (!this.path) {
                this.$message({
                    message: '未设置编辑页的路径',
                    type: 'error'
                });
                return;
            }
            if (!(top.$pagebox && this.path)) return;
            var url = this.setParameter(this.path, id);
            this.pagebox(url, '修改', window.name + '_' + id + '[modify]', this.width, this.height);
        },
        //删除事件
        delete: function (ids, btn) {
            var arr = String(ids).split(',');
            if (ids == '' || arr.length < 1) {
                this.$message({
                    message: '请选中要操作的数据行',
                    type: 'error'
                });
                return false;
            }
            if (btn == null) {
                this.$emit('delete', ids, btn);
                return true;
            }
            this.$confirm('是否确认删除这 ' + arr.length + ' 项数据? ', '谨慎操作', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                this.$emit('delete', ids, btn);
            }).catch(() => { });
        },
        //获取当前点击的按钮
        getCurrbtn: function (btnid) {
            var curr = {};
            for (var t in this.buttons) {
                if (this.buttons[t].id == btnid) {
                    curr = this.buttons[t];
                    break;
                }
            }
            return curr;
        },
        //获取id
        getid: function () {
            if (!this.idkey || !this.selects || this.selects.length < 1) return '';
            var id = !!this.selects[0][this.idkey] ? this.selects[0][this.idkey] : '';
            return id;
        },
        //获取树菜单节点数据
        getnode: function () {
            var tree = top.tree;
            if (tree) return tree.getData(window.name);
            return null;
        },
        //获取多个id，用逗号分隔
        getids: function () {
            if (!this.idkey || !this.selects || this.selects.length < 1) return '';
            var str = '';
            for (var i = 0; i < this.selects.length; i++) {
                var id = !!this.selects[i][this.idkey] ? this.selects[i][this.idkey] : '';
                if (id == '') continue;
                str += i < this.selects.length - 1 ? id + ',' : id;
            }
            return str;
        },
        //获取完整的页面路径
        getfullpath: function (path) {
            if (path.substring(0, 1) == "/") return path;
            var root = String(window.document.location.href);
            var file = root.substring(0, root.lastIndexOf("/") + 1);
            return file + path;
        },
        //设置参数
        setParameter: function (url, id) {
            if (!id) return url;
            url += url.indexOf("?") < 0 ? '?' : '&';
            return url + 'id=' + id;
        },
        //打开窗体
        //url:弹窗内页的链接
        //title:标题
        //boxid:弹窗的标识，不是单纯的id
        //param:弹窗参数，
        pagebox: function (url, title, boxid, width, height, param) {
            url = this.getfullpath(url);
            var node = this.getnode();
            var tit = node ? node.title : $dom('title').text();
            if ($dom('title').text() != '' && (!title || title != '')) tit += ' - ';
            var attrs = {
                width: width ? width : 400,
                height: height ? height : 300,
                url: url,
                ico: node ? node.MM_IcoS : 'e77c',
                pid: window.name,
                id: boxid,
                title: tit + title
            };
            if (param != null) {
                for (var t in param) attrs[t] = param[t];
            }
            console.log(attrs);
            var pbox = top.$pagebox.create(attrs);
            pbox.open();
        }
    },
    template: '<div class="btngroup"><el-button-group>\
    <template v-for="(btn,index) in buttons">\
    <el-tooltip class="item" :content="btn.tips" placement="bottom" effect="light">\
    <el-button :key="index" :type="btn.type"  size="small" plain v-on:click="eventClick(btn.id)"\
    :class="btn.class" v-if="visible(btn.text)"> &nbsp; {{btn.text}}</el-button>\
    </el-tooltip>\
    </template>\
    </el-button-group></div>'
});
