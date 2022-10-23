//附件
$dom.load.css([$dom.pagepath() + 'Components/Styles/accessory.css']);
//附件列表
Vue.component('accessory', {
    props: ['outline', 'account', 'studied'],
    data: function () {
        return {
            files: [],		//附件文件列表		
            pdfview: false,     //显示Pdf展示面板
            pdfurl: '',         //pdf的页面地址
            pdftitle: '',        //pdf文件名，这里为作为标题

            loading: true //预载中
        }
    },
    watch: {
        'outline': {
            deep: true,
            immediate: true,
            handler: function (newV, oldV) {
                if (JSON.stringify(newV) == '{}' || newV == null) return;
                var th = this;
                $api.cache('Accessory/List', { 'uid': newV.Ol_UID, 'type': 'Course' }).then(function (req) {
                    if (req.data.success) {
                        th.files = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            }
        }
    },
    created: function () { },
    methods: {
        //是不是pdf格式
        ispdf: function (href) {
            var exist = "";
            if (href.indexOf("?") > -1) href = href.substring(0, href.indexOf("?"));
            if (href.indexOf(".") > -1) exist = href.substring(href.lastIndexOf(".") + 1).toLowerCase();
            return exist == "pdf";
        },
        //附件击事件
        openpdf: function (href, name) {
            var tit = $api.trim(name);
            var url = $api.pdfViewer(href);
            console.log(url);
            this.pdfview = true;
            this.pdfurl = url;
            //标题
            if (name.indexOf('.') > -1) {
                name = name.substring(0, name.lastIndexOf('.'));
            }
            this.pdftitle = name;
        }
    },
    template: `<div class='accessory'>
        <div class='noaccess' v-if='!studied'>（未登录或未选修课程）</div>            
        <dl class='access_list' v-else-if='files.length>0'>
            <dd v-for='(f,i) in files'>
                <a target='_blank' :href='f.As_FileName' v-if='ispdf(f.As_FileName)' 
                :download='f.As_Name' @click.prevent ='openpdf(f.As_FileName,f.As_Name)'>
                {{i+1}}、{{f.As_Name}}</a>
                <a target='_blank' :href='f.As_FileName' v-else
                :download='f.As_Name'>{{i+1}}、{{f.As_Name}}</a>
            </dd>
        </dl>
        <div class='noaccess' v-else>（没有附件）</div>       
        <van-popup v-model:show="pdfview" :style="{ height: '100%',width: '100%' }" :closeable="true">  
            <div class="pdftitle"><icon>&#xe848</icon>{{pdftitle}}</div>
            <iframe frameborder="0" border="0" marginwidth="0" marginheight="0" :src="pdfurl"></iframe>
        </van-popup>
    </div>`
});
