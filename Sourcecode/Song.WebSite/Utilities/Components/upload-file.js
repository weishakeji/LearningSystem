// 图片上传组件
//事件change:当选择本地文件后触发，参数file（含属性base64,当前上传图片文件的base64编码）
Vue.component('upload-file', {
    //text: 上传按钮的文字
    //id: 控件id
    //data: 要操作的数据项
    //ext: 限制的扩展名
    //size:限制的文件大小，单位Kb
    props: ['text', 'id', 'data', 'ext', 'size', 'height'],
    data: function () {
        return {
            ctrid: '',       //控件id
            //是否存在错误
            error: {
                state: false,    //是否错误
                text: ''         //提示信息
            },
            loading: false
        }
    },
    watch: {
    },
    computed: {
        'height_px': function () {
            if (this.height == null || this.height == '') return false;
            return this.height + 'px';
        }
    },
    created: function () {
        $dom.load.css(['/Utilities/Components/Styles/upload-file.css']);
        var th = this;
        th.loading = true;
        //控件id
        var tid = this.id == null || this.id == '' ? Math.round(Math.random() * 10000) : this.id;
        this.ctrid = 'upload_file_' + tid;
    },
    methods: {
        //选择文件
        select_file: function () {
            var input = document.querySelector("#" + this.ctrid);
            input.click();
        },
        //选文件结束后
        select_change: function () {
            var input = document.querySelector("#" + this.ctrid);
            if (input.files == null || input.files.length < 1) return;
            var file = input.files[0];
            this.error.state = false;
            //验证，文件类型
            if (this.ext != null && this.ext != '') {
                var extname = file.name.indexOf('.') > -1 ? file.name.substring(file.name.lastIndexOf('.') + 1) : '';
                extname = extname.toLowerCase();
                //console.log(extname);
                var exist = false;
                var extarr = this.ext.split(',');
                for (var i = 0; i < extarr.length; i++) {
                    if (extarr[i] == '') continue;
                    if (extarr[i].toLowerCase() == extname) {
                        exist = true;
                        break;
                    }
                }
                if (!exist) {
                    var txt = "上传文件类型限仅：{0}，当前文件被禁止上传";
                    var msg = this.format(txt, this.ext.replace(/,/ig, '、'));
                    this.error.state = true;
                    this.error.text = msg;
                    return;
                }
            }
            //验证,限制文件大小
            var limitsize = this.size ? Number(this.size) : 0;
            limitsize = isNaN(limitsize) ? 0 : limitsize;
            if (limitsize > 0) {
                if (this.size * 1024 < file.size) {
                    var txt = "文件：{0}，大于限定的{1}（实际大小{2})，被禁止上传";
                    var limit = this.clacSize(this.size);
                    var actual = this.clacSize(file.size / 1024);
                    var msg = this.format(txt, file.name, limit, actual);
                    this.error.state = true;
                    this.error.text = msg;
                    return;
                }
            }
            var th = this;
            /*
            if (!this.error.state) {
                var reads = new FileReader();
                reads.readAsDataURL(file);
                reads.onload = function (e) {
                    file.base64 = this.result;
                    th.$emit('change', file, th.data, th.id);
                };
            }*/
            this.$emit('change', file, this.data, this.id);
        },
        //计算文件大小的实际单位
        clacSize: function (size) {
            if (size < 1024) return Math.floor(size * 100) / 100 + "Kb";
            return Math.floor(size / 1024 * 100) / 100 + "Mb";
        },
        //格式化文本
        format: function (txt) {
            var arg = arguments;
            if (arguments.length == 0) return '';
            if (arguments.length == 1) return txt;
            for (var i = 1; i < arguments.length; i++) {
                var reg = new RegExp("\\\{" + (i - 1) + "\\\}");
                txt = txt.replace(reg, arguments[i]);
            }
            return txt;

        }
    },
    template: `<div class="upload_file_area" :ctrid="ctrid" :style="{'height':height_px}">   
    <input type="file" name="file" :id="ctrid" v-show="false" @change="select_change" ></input>
    <div @click="select_file">
        <slot>
            <span v-html="text" v-if="text!=''"></span>
            <span v-else>选择需要上传的文件</span>
        </slot>
    </div>
    <div v-if="error.state" class="error">* {{error.text}}</div>
    </div>`
});
