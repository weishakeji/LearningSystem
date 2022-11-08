var tinymceConfig = {
    tinyID: "mytextarea",//作用域ID
    placeholder: '', //默认文字
    infoHtml: "",//初始化内容
    GbaseUrl: '',//全局baseUrl
    OMHtml: '<div style="height: 1500px;"><p><h2>操作手册：</h2></p></div><p>666</p>', //设置操作手册Html
    CPHtml: '',
}

tinymce.init({
    selector: '#' + tinymceConfig.tinyID,
    language: 'zh_CN',
    menubar: false,
    branding: false,
    menubar: "file edit view insert format table tools",
    min_height: 400,
    max_height: 700,
    plugins: 'kityformula-editor insertdatetime print preview clearhtml searchreplace autolink layout fullscreen image upfile link media code codesample table charmap hr pagebreak nonbreaking anchor advlist lists textpattern help emoticons autosave bdmap indent2em lineheight formatpainter axupimgs powerpaste letterspacing imagetools quickbars attachment wordcount autoresize importword',
    toolbar_groups: {
        formatting: {
            text: '文字格式',
            tooltip: 'Formatting',
            items: 'bold italic underline | superscript subscript',
        },
        alignment: {
            icon: 'align-left',
            tooltip: 'alignment',
            items: 'alignleft aligncenter alignright alignjustify',
        }
    },
    //upfile attachment //上传文件、附件
    toolbar: ['|code kityformula-editor formatselect fontselect fontsizeselect forecolor backcolor bold italic underline strikethrough link alignment indent2em outdent indent lineheight letterspacing bullist numlist blockquote subscript superscript  layout removeformat table image media  importword charmap  hr pagebreak  clearhtml  insertdatetime  bdmap  formatpainter  cut copy undo redo restoredraft  searchreplace fullscreen print preview '],
    table_style_by_css: true,
    OperationManualHtml: '',
    CommonProblemHtml: '',
    fixed_toolbar_container: '#tinymce-app .toolbar',
    custom_ui_selector: '#tinymce-app',
    placeholder: '' + tinymceConfig.placeholder,
    file_picker_types: 'file',
    powerpaste_word_import: "clean", // 是否保留word粘贴样式  clean | merge 
    powerpaste_html_import: 'clean', // propmt, merge, clean
    powerpaste_allow_local_images: true,//
    powerpaste_keep_unsupported_src: true,
    paste_data_images: true,
    toolbar_sticky: false,
    autosave_ask_before_unload: false,
    fontsize_formats: '12px 14px 16px 18px 24px 36px 48px 56px 72px',
    font_formats: '微软雅黑=Microsoft YaHei,Helvetica Neue,PingFang SC,sans-serif;苹果苹方=PingFang SC,Microsoft YaHei,sans-serif;宋体=simsun,serif;仿宋体=FangSong,serif;黑体=SimHei,sans-serif;Arial=arial,helvetica,sans-serif;Symbol=symbol;Tahoma=tahoma,arial,helvetica,sans-serif;Terminal=terminal,monaco;Times New Roman=times new roman,times;Verdana=verdana,geneva;Webdings=webdings;Wingdings=wingdings,zapf dingbats;',
    images_upload_base_path: '',
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
        console.log(editor);
        editor.focus();
        editor.setContent('3444'); 
        var html=editor.getContent();
        //alert(html);
        //tinyMCE.editors[tinymceConfig.tinyID+'2'].setContent(html2); 
        //$('#tinymce-app').fadeIn(1000);
        //    editor.execCommand('selectAll');
        //    editor.selection.getRng().collapse(false);
        //    editor.focus();
    }
});