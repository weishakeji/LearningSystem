//课程附件
Vue.component('accessory', {
    props: ['uid', 'isbuy', 'account', 'outline'],
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
            var th = this;
            th.msg = '';
            $api.get("Outline/Accessory", { uid: val, 'acid': th.account.Ac_ID }).then(function (acc) {
                if (acc.data.success) {
                    th.datas = acc.data.result;
                } else {
                    th.msg = "附件信息加载异常！详情：\r" + acc.data.message;
                }
            }).catch(function (err) {
                th.msg = err;
            });
        },
        'outline': {
            handler: function (nv, ov) {
                var th = this;
                th.msg = '';
                $api.get("Outline/Accessory", { uid: nv.Ol_UID, 'acid': th.account.Ac_ID }).then(function (acc) {
                    if (acc.data.success) {
                        th.datas = acc.data.result;
                    } else {
                        th.msg = "附件信息加载异常！详情：\r" + acc.data.message;
                    }
                }).catch(function (err) {
                    th.msg = err;
                });
            }, deep: true
        },
        'isbuy': function (val, old) {

        }
    },
    methods: {
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
        }
    },
    template: `<div id="accessory">
        <div  v-if="!isbuy" style="color:red;">课程未购买，资料不提供下载或预览</div>
            <a  v-if="isbuy"  v-for="(item,index) in datas" target="_blank" :href="item.As_FileName"
                v-on:click="accessClick(item.As_FileName,item.As_Name,$event)"
                :download="item.As_Name">{{index+1}}、{{item.As_Name}}
                <span class="filesize">{{item.As_Size|size}}</span>
            </a>
            <div v-if="!isbuy"  v-for="(item,index) in datas" >
            {{index+1}}、{{item.As_Name}}
            <span class="filesize">{{item.As_Size|size}}</span>
        </div>
        <div class="noInfo">{{msg}}</div>
    </div>`
});