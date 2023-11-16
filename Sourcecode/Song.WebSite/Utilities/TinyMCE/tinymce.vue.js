
//编辑器
//content:文本内容
//model:模式，full全部按钮,general常用按钮,simple简化版，更少按钮; mini极少的按钮；inline内联模式，没有按钮
//menubar:是否显示编辑上方的下拉菜单
//id:编辑器的html标签id
//placeholder: 空白显示内容
//upload:上传文件的路径key值，对应web.config中upload节点
//dataid:数据id，例如编辑试题此处为试题的ID，编辑通知时此处为通知的id
Vue.component('editor', {
    props: ['content', 'model', 'menubar', 'id', 'placeholsder', 'upload', 'dataid'],
    data: function () {
        return {
            //编辑器的文本
            text: '',
            isinit: false,       //是否初始化
            load: false       //预加载效果
        }
    },
    watch: {
        'ctrid': {
            handler: function (nv, ov) {
                if (nv == ov) return;
                this.$nextTick(function () {
                    if (!this.load && !this.isinit)
                        this.init();
                });

            }, immediate: true
        },
        'dataid': {
            handler: function (nv, ov) {
                if (tinyMCE.editors.length > 0 && tinyMCE.editors[this.ctrid])
                    tinyMCE.editors[this.ctrid].settings['dataid'] = nv;
            }, immediate: true
        },
        'content': {
            handler: function (nv, ov) {
                if (this.text == null || this.text == '') {
                    if (nv != '' && nv != null)
                        this.text = nv.replace(/[\r\n]/g, '');
                    this.setContent(this.text);
                }
            }, immediate: false
        }
    },
    computed: {
        //控件id
        'ctrid': function () {
            var id = this.id == null ? new Date().getTime() : this.id;
            return 'tinymce_editor_' + id;
        },
        //

    },
    created: function () {

        if (window.$dom) $dom.load.css(['/Utilities/TinyMCE/Styles/tinymce.vue.css']);
    },
    mounted: function () {

    },
    methods: {
        //工具栏按钮设置
        toolbar: function () {
            var arr = [];
            switch (this.model) {
                //full全部按钮
                case 'full':
                    arr = [`undo redo restoredraft cut copy formatpainter | formatselect fontselect fontsizeselect forecolor backcolor outdent indent |                  
                    bold italic underline strikethrough link anchor alignleft aligncenter alignright alignjustify indent2em 
                    lineheight letterspacing bullist numlist blockquote subscript superscript  layout removeformat | 
                    table image media  importword emoticons charmap kityformula-editor  hr pagebreak  clearhtml  insertdatetime  bdmap  
                    searchreplace fullscreen print preview code upfile image-weisha organinfo`]
                    break;
                //general常用按钮
                case 'general':
                    arr = [`undo redo cut copy formatpainter  | formatselect fontselect fontsizeselect forecolor backcolor table importword 
                    | formatting alignment indent_gr insert insertdatetime  lineheight letterspacing bullist numlist                       
                    layout removeformat  hr pagebreak  clearhtml  link fullscreen searchreplace preview`]
                    break;
                //simple简化版，相较于general更少按钮
                case 'simple':
                default:
                    arr = [`formatselect fontselect fontsizeselect forecolor 
                        backcolor lineheight letterspacing | formatting alignment indent_gr insert`]
                    break;
                //mini极少的按钮
                case 'mini':
                    arr = [` forecolor  backcolor  lineheight letterspacing bullist numlist 
                    | formatting  alignment indent_gr insert`]
                    break;
                //inline内联模式，没有按钮
                case 'inline':
                    arr = [];
                    break;
                //试题编辑
                case 'question':
                    arr = [` undo redo cut copy formatpainter | fontsizeselect forecolor backcolor bold italic underline strikethrough | superscript subscript 
                    indent2em outdent indent lineheight |  alignleft aligncenter alignright alignjustify
                    letterspacing bullist numlist  | hr table  insertdatetime removeformat  link importword 
                    image-weisha charmap  kityformula-editor  | fullscreen searchreplace preview`]
                    break;
                //试题的简单编辑
                case 'ques':
                    arr = [` forecolor  backcolor  bold italic underline strikethrough  superscript subscript 
                    | alignleft aligncenter alignright alignjustify 
                    |  image-weisha charmap  kityformula-editor
                    `]
                    break;
            }
            return arr;
        },
        //右键菜单的显示
        contextmenushow: function () {
            let def = 'copy paste | link image inserttable | cell row column deletetable | code';
            switch (this.model) {
                case 'mini':
                case 'inline':
                    return 'copy paste'
                    break;
                default:
                    return def;
            }
        },
        init: function () {
            var th = this;
            th.load = true;
            tinymce.init({
                selector: '#' + th.ctrid,
                language: 'zh_CN',
                upload_key: th.upload,
                menubar: th.menubar == null || th.menubar == false || th.menubar == 'false' ? false : "file edit view insert format table tools",
                branding: false,
                inline: th.model == 'inline', //开启内联模式
                height: '100%',
                max_height: 700,
                plugins: `kityformula-editor insertdatetime print preview clearhtml searchreplace autolink layout 
                fullscreen upfile link media code codesample table charmap hr pagebreak nonbreaking anchor 
                advlist lists textpattern help emoticons bdmap indent2em lineheight formatpainter axupimgs 
                powerpaste letterspacing quickbars attachment wordcount autoresize image importword image-weisha organinfo`,
                toolbar_groups: {
                    formatting: {
                        text: '格式',
                        tooltip: '粗体、斜体、下划线、上标、下标',
                        items: 'bold italic underline strikethrough | blockquote superscript subscript',
                    },
                    alignment: {
                        text: '对齐',
                        tooltip: '文字对齐',
                        items: 'alignleft aligncenter alignright alignjustify',
                    },
                    indent_gr: {
                        text: '缩进',
                        tooltip: '缩进、首行缩进',
                        items: 'indent2em outdent indent',
                    },
                    insert: {
                        text: '插入',
                        tooltip: '插入图片、特殊字符、公式',
                        items: 'image-weisha  emoticons charmap  kityformula-editor bdmap organinfo',
                    }
                },
                //upfile attachment //上传文件、附件
                toolbar: th.toolbar(),
                //选中时出现的快捷工具，与插件有依赖关系
                quickbars_selection_toolbar: 'bold italic underline forecolor | link blockquote',
                quickbars_insert_toolbar: 'kityformula-editor importword quicktable',
                contextmenu: th.contextmenushow(),
                table_style_by_css: true,
                OperationManualHtml: '',
                CommonProblemHtml: '',
                fixed_toolbar_container: '#tinymce-app .toolbar',
                custom_ui_selector: '#tinymce-app',
                placeholder: th.placeholder == null ? '' : th.placeholder,
                file_picker_types: 'file',
                powerpaste_word_import: "clean", // 是否保留word粘贴样式  clean | merge 
                powerpaste_html_import: 'clean', // propmt, merge, clean
                powerpaste_allow_local_images: true,//
                powerpaste_keep_unsupported_src: true,
                paste_data_images: true,
                convert_urls: false,     //当插入图片时，是否转换为相对路径
                toolbar_sticky: false,
                autosave_ask_before_unload: false,
                fontsize_formats: '12px 14px 16px 18px 24px 36px 48px 56px 72px',
                font_formats: '微软雅黑=Microsoft YaHei,Helvetica Neue,PingFang SC,sans-serif;苹果苹方=PingFang SC,Microsoft YaHei,sans-serif;宋体=simsun,serif;仿宋体=FangSong,serif;黑体=SimHei,sans-serif;Arial=arial,helvetica,sans-serif;Symbol=symbol;Tahoma=tahoma,arial,helvetica,sans-serif;Terminal=terminal,monaco;Times New Roman=times new roman,times;Verdana=verdana,geneva;Webdings=webdings;Wingdings=wingdings,zapf dingbats;',
                images_upload_base_path: '',
                setup: function (ed) {
                    //当录入变动时
                    ed.on('input change redo undo keydown', function (e) {
                        var content = tinyMCE.get(ed.id).getContent();
                        //触发vue组件事件
                        th.$emit('change', content);
                        //var escapedClassName = ed.id.replace(/(\[|\])/g, '\\$&');
                        //console.log(content);
                    });
                    //ed.oninput();
                },
                //自定义插入图片函数  blobInfo: 本地图片blob对象, succFun(url|string)： 成功回调（插入图片链接到文本中）, failFun(string)：失败回调
                images_upload_handler: function (blobInfo, succFun, failFun) {
                    var file = blobInfo.blob();
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        succFun(e.target.result)
                    }
                    reader.readAsDataURL(file)
                },
                init_instance_callback: function (editor) {
                    //console.log(editor);
                    th.load = false;
                    th.isinit = true;       //初始化功能
                    //将来自组件参数的content,传给内部参数text，以免外部数据变化，影响组件状态
                    th.text = th.content;
                    editor.setContent(th.text);

                    editor.settings['dataid'] = th.dataid;
                    var params = editor.settings;
                    for (var k in params) {

                        //console.log(k + ': ' + params[k]);
                    }
                    var d = editor.getParam('dataid', 123);
                    console.log('dataid: ' + d);
                    //var html = editor.getContent();   
                      
                    //初次加载时，计算内容中的字数           
                    var wordcount = tinymce.activeEditor.plugins.wordcount;
                    var wordcount_el = document.querySelectorAll('.tox-statusbar__wordcount');
                    if (wordcount_el.length > 0) {
                        wordcount_el[0].innerHTML = wordcount_el[0].innerHTML.replace('0', wordcount.body.getWordCount());
                    }
                  

                    //alert(html);
                    //tinyMCE.editors[tinymceConfig.tinyID+'2'].setContent(html2); 
                    //$('#tinymce-app').fadeIn(1000);
                    //    editor.execCommand('selectAll');
                    //    editor.selection.getRng().collapse(false);
                    //    editor.focus();
                }
            });
        },
        //获取内容
        getContent: function () {
            var html = tinyMCE.editors[this.ctrid].getContent();
            return html.replace(/<script[^>]+>/g, "");
        },
        //设置内容
        setContent: function (text) {

            tinyMCE.editors[this.ctrid].setContent(text);
        }
    },

    template: `<div class="editor" :ctrid="id" :model="model">
        <loading bubble v-if="load"></loading>
        <div v-show="!load">
            <div :id="ctrid"></div>
        </div>
    </div>`
});
