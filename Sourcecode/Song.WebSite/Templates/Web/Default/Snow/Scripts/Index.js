$ready(function () {
    /*
    console.log(window.location);
    return;
    var count = 2024;
    for (let i = 1; i <= count; i++) {
        document.write('<span>'+i + ' . ');

        var s = String.fromCharCode(i);
        if (s == '') s = '不可显示的字符';
        document.write(s + ' </span><br/> ');
    }*/
    tinymce.init({
        selector: '#mytextarea',
       
        language:'zh-Hans',
        plugins: 'kityformula-editor print preview searchreplace autolink directionality visualblocks visualchars fullscreen image link media template code codesample table charmap hr pagebreak nonbreaking anchor insertdatetime advlist lists wordcount imagetools textpattern help emoticons autosave bdmap indent2em autoresize formatpainter axupimgs',
        toolbar: 'code formatselect fontsizeselect undo redo restoredraft | cut copy paste pastetext | forecolor backcolor bold italic underline strikethrough link anchor kityformula-editor | alignleft aligncenter alignright alignjustify outdent indent | \
        styleselect | bullist numlist | blockquote subscript superscript removeformat | \
        table image media charmap emoticons hr pagebreak insertdatetime print preview | fullscreen | bdmap indent2em lineheight formatpainter axupimgs',
        height: 650, //编辑器高度
        min_height: 400,
        /*content_css: [ //可设置编辑区内容展示的css，谨慎使用
            '/static/reset.css',
            '/static/ax.css',
            '/static/css.css',
        ],*/
        //toolbar: false,
        menubar:"file edit view insert format table tools help",
        fontsize_formats: '12px 14px 16px 18px 24px 36px 48px 56px 72px',
        font_formats: '微软雅黑=Microsoft YaHei,Helvetica Neue,PingFang SC,sans-serif;苹果苹方=PingFang SC,Microsoft YaHei,sans-serif;宋体=simsun,serif;仿宋体=FangSong,serif;黑体=SimHei,sans-serif;Arial=arial,helvetica,sans-serif;Arial Black=arial black,avant garde;Book Antiqua=book antiqua,palatino;',
        link_list: [
            { title: '预置链接1', value: 'http://www.tinymce.com' },
            { title: '预置链接2', value: 'http://tinymce.ax-z.cn' }
        ],
        image_list: [
            { title: '预置图片1', value: 'https://www.tiny.cloud/images/glyph-tinymce@2x.png' },
            { title: '预置图片2', value: 'https://www.baidu.com/img/bd_logo1.png' }
        ],
        image_class_list: [
        { title: 'None', value: '' },
        { title: 'Some class', value: 'class-name' }
        ],
        importcss_append: true,
        //自定义文件选择器的回调内容
        file_picker_callback: function (callback, value, meta) {
            if (meta.filetype === 'file') {
              callback('https://www.baidu.com/img/bd_logo1.png', { text: 'My text' });
            }
            if (meta.filetype === 'image') {
              callback('https://www.baidu.com/img/bd_logo1.png', { alt: 'My alt text' });
            }
            if (meta.filetype === 'media') {
              callback('movie.mp4', { source2: 'alt.ogg', poster: 'https://www.baidu.com/img/bd_logo1.png' });
            }
        },
        toolbar_sticky: true,
        autosave_ask_before_unload: false,
      });
});