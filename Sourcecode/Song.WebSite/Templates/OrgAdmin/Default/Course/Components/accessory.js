//章节的附件
Vue.component('accessory', {
    props: [],
    data: function () {
        return {
            outline: {},
            showpanel: false,     //是否显示
            datas: [],       //附件列表

            ext_limit: "zip,rar,pdf,ppt,pptx,doc,docx,xls,xlsx",

            loading_upload: false,
            loading: false
        }
    },
    watch: {
        'outline': {
            handler: function (nv, ov) {
                this.getDatas(nv.Ol_UID,);
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        $dom.load.css([$dom.path() + 'Course/Components/Styles/accessory.css']);
    },
    methods: {
        //设置初始值
        setvalue: function (ol) {
            this.outline = ol;
            console.log(this.outline);
            this.showpanel = true;
        },
        //弹出面板的标题
        title_panel: function () {
            var name = this.outline ? this.outline.Ol_Name : '';
            return '附件 - ' + name;
        },
        //获取附件
        getDatas: function (uid) {
            var th = this;
            th.loading = true;
            $api.get("Accessory/List", { uid, 'type': 'Course' }).then(function (acc) {
                th.loading = false;
                if (acc.data.success) {
                    th.datas = acc.data.result;
                } else {
                    th.msg = "附件信息加载异常！详情：\r" + acc.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                th.msg = err;
            });
        },
        //附件的点击事件
        accessClick: function (file, tit, event) {
            var exist = file.substring(file.lastIndexOf(".") + 1).toLowerCase();
            if (exist == "pdf") {
                event.preventDefault();
                var obj = {
                    width: "60%", height: "60%",
                    full: true, max: true, min: false,
                    resize: true, move: true,
                    ico: "e848", showmask: true
                };
                obj.url = $api.pdfViewer(file);
                $pagebox.create(obj).open();
            }
            return false;
        },
        //删除文件
        deleteitem: function (data) {
            console.error(data);
            var th = this;
            var id = data.As_Id;
            $api.delete('Accessory/DeleteForID', { 'id': id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.getDatas(data.As_Uid);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        //附件文件上传
        uploadAccessory: function (file) {
            var th = this;
            th.loading_upload = true;
            var uid = this.outline.Ol_UID;
            $api.post('Accessory/Upload', { 'uid': uid, 'type': 'Course', 'file': file }).then(function (req) {
                th.loading_upload = false;
                if (req.data.success) {
                    var result = req.data.result;
                    th.getDatas(uid);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.loading_upload = false;
                console.error(err);
            });
        },
    },
    template: `<el-dialog :visible.sync="showpanel" append-to-body custom-class="accessory_dialog"
     :show-close="true" :close-on-click-modal="false">
        <template slot="title">
            <icon>&#xe853</icon> {{title_panel()}}
        </template>
        <loading v-if="loading_upload" ></loading>
        <upload-file v-else @change="uploadAccessory" :data="outline" size="102400" height="30"
            :ext="ext_limit">
            <el-tooltip :content="'允许的文件类型：'+ext_limit" placement="right"
                effect="light">
                <el-button type="primary" plain>
                    <icon>&#xe6ea</icon>点击上传文件
                </el-button>
            </el-tooltip>
        </upload-file>
        <dl v-if="datas.length>0" remark="附件列表">
            <dd v-for="(data,i) in datas">
                <a :href="data.As_FileName" v-if="data.As_FileName!=''" target="_blank" type="link"
                    :download="data.As_Name">{{(i+1)}} . {{data.As_Name}}</a>
                <span v-else title="文件不存在">{{(i+1)}} . {{data.As_Name}}</span>
                <span class="filesize">{{data.As_Size|size}}</span>
                <el-popconfirm title="确定删除吗？" class="btndel" @confirm="deleteitem(data)">
                    <el-link icon="el-icon-delete" type="danger" slot="reference">删除</el-link>
                </el-popconfirm>                
            </dd>
        </dl>
        <div v-else>没有附件</div>
        <div slot="footer" class="dialog-footer">
            <el-button @click="()=> { showpanel = false;}">取 消</el-button>
        </div>        
    </el-dialog> `
});