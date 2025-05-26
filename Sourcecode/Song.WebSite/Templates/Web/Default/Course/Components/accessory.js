//课程附件
Vue.component('accessory', {
    //studied:是否可以学习该章节
    //owned: 是否拥有该课程，例如学员组关联，购买
    props: ['uid', 'studied','owned', 'account', 'outline'],
    data: function () {
        return {
            datas: [],  //附件列表
            msg: ''      //提示信息
        }
    },
    mounted: function () {
        var css = $dom.path() + 'course/Components/Styles/accessory.css';
        $dom.load.css([css]);
    },
    watch: {
        'uid': function (val, old) {
            if (val == null || $api.trim(val) == '') return;
            this.getdatas(val);
        },
        'outline': {
            handler: function (nv, ov) {
                if (JSON.stringify(nv) == '{}' || nv == null) return;
                this.getdatas(nv.Ol_UID);
            }, deep: true
        },
        'owned': function (val, old) {

        }
    },
    methods: {
        //加载附件数据
        getdatas: function (uid) {
            var th = this;
            th.msg = '';
            $api.cache('Accessory/List', { 'uid': uid, 'type': 'Course' }).then(function (acc) {
                if (acc.data.success) {
                    th.datas = acc.data.result;                  
                } else {
                    th.msg = "附件信息加载异常！详情：\r" + acc.data.message;
                }
            }).catch(function (err) {
                th.msg = err;
            });
        },
        //附件的点击事件
        accessClick: function (file, tit, event) {
            var exist = file.substring(file.lastIndexOf(".") + 1).toLowerCase();
            if (exist == "pdf") {
                event.preventDefault();
                var obj = {
                    title:tit,
                    width: "60%", height: "60%",
                    full: true, max: true, min: false,
                    resize: true, move: true,
                    ico: "e848", showmask: true
                };
                obj.url = $api.pdfViewer(file);
                $pagebox.create(obj).open();
            }
            return false;
        }
    },
    template: `<div id="accessory">
        <div v-if="!owned" style="color:red;">课程未购买，资料不提供下载或预览</div>
        <div v-if="owned" v-for="(item,index) in datas" class="download">
            <span>{{index+1}} . </span>
            <a target="_blank" :href="item.As_FileName" :filetype="item.As_Extension"
                v-on:click="accessClick(item.As_FileName,item.As_Name,$event)"
                :download="item.As_Name">{{item.As_Name}}                
            </a>
            <span class="filesize">{{item.As_Size|size}}</span>
        </div>
        <div v-if="!owned"  v-for="(item,index) in datas" >
            {{index+1}}、{{item.As_Name}}
            <span class="filesize">{{item.As_Size|size}}</span>
        </div>
        <div class="noInfo">{{msg}}</div>
    </div>`
});