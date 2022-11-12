
var xhrOnProgress = function (fun) {
    xhrOnProgress.onprogress = fun;
    return function () {
        var xhr = $.ajaxSettings.xhr();
        if (typeof xhrOnProgress.onprogress !== 'function')
            return xhr;
        if (xhrOnProgress.onprogress && xhr.upload) {
            xhr.upload.onprogress = xhrOnProgress.onprogress;
        }
        return xhr;
    }
}

/*==============================================================*/
var tinymceConfig = {
    tinyID: "mytextarea",//作用域ID
    placeholder: '5555', //默认文字
    infoHtml: $(this.tinyID).html(),//初始化内容
    GbaseUrl: '',//全局baseUrl
    OMHtml: '<div style="height: 1500px;"><p><h2>操作手册：</h2></p></div><p>666</p>', //设置操作手册Html
    CPHtml: '',
}

tinymce.init({
    selector: '#' + tinymceConfig.tinyID,
    language: 'zh_CN',
    menubar: false,
    branding: false,
    min_height: 400,
    max_height: 700,
    plugins: `print preview clearhtml searchreplace autolink layout fullscreen image upfile link 
                 media code codesample table charmap hr pagebreak nonbreaking anchor advlist lists textpattern 
                 help emoticons autosave bdmap indent2em lineheight formatpainter axupimgs powerpaste letterspacing
                 imagetools quickbars attachment wordcount autoresize importword`,
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
    toolbar: [`|code formatselect fontselect fontsizeselect forecolor backcolor bold italic underline 
                 strikethrough link alignment indent2em outdent indent lineheight letterspacing bullist numlist
                  blockquote subscript superscript  layout removeformat table image media upfile attachment importword 
                  charmap  hr pagebreak  clearhtml    bdmap  formatpainter  cut copy undo redo restoredraft 
                   searchreplace fullscreen help`],
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
    images_upload_handler: function (blobInfo, succFun, failFun) {//自定义插入图片函数  blobInfo: 本地图片blob对象, succFun(url|string)： 成功回调（插入图片链接到文本中）, failFun(string)：失败回调
        var file = blobInfo.blob();
        var reader = new FileReader();
        reader.onload = function (e) {
            succFun(e.target.result)
        }
        reader.readAsDataURL(file)
    },
    file_picker_callback: function (succFun, value, meta) { //自定义文件上传函数 
        var filetype = '.pdf, .txt, .zip, .rar, .7z, .doc, .docx, .xls, .xlsx, .ppt, .pptx, .mp3, .mp4';
        var input = document.createElement('input');
        input.setAttribute('type', 'file');
        input.setAttribute('accept', filetype);
        input.click();
        input.onchange = function () {
            var file = this.files[0];
            var data = new FormData();
            data.append("file", file);
            $.ajax({
                data: data,
                type: 'GET',
                url: './api/file.json',
                header: { 'Content-Type': 'multipart/form-data' },
                cache: false,
                contentType: false,
                processData: false,
                dataType: 'json',
                xhr: xhrOnProgress(function (e) {
                    const percent = (e.loaded / e.total * 100 | 0) + '%';//计算百分比
                    // console.log(percent);
                    progressCallback(percent);

                }),
            }).then(function (data) {
                if (data.code == 200) {
                    succFun(data.data, { text: data.data });
                }
            }).fail(function (error) {
                failFun('上传失败:' + error.message)
            });
        }
    },
    file_callback: function (file, succFun) { //文件上传  file:文件对象 succFun(url|string,obj) 成功回调
        var data = new FormData();
        data.append("file", file);
        $.ajax({
            data: data,
            type: 'GET',
            url: './api/file.json',
            cache: false,
            contentType: false,
            processData: false,
            header: { 'Content-Type': 'multipart/form-data' },
            dataType: 'json',
            xhr: xhrOnProgress(function (e) {
                const percent = (e.loaded / e.total * 100 | 0) + '%';//计算百分比
                progressCallback(percent);
            }),
        }).then(function (data) {
            if (data.code == 200) {
                succFun(data.data, { text: file.name });
            }
        }).fail(function (error) {
            // failFun('上传失败:' + error.message)
        });
    },
    attachment_assets_path: './plugins/attachment/icons',
    attachment_upload_handler: function (file, succFun, failFun, progressCallback) {
        var data = new FormData();
        data.append("file", file);
        $.ajax({
            data: data,
            type: 'GET',
            url: './api/file.json',
            cache: false,
            contentType: false,
            processData: false,
            header: { 'Content-Type': 'multipart/form-data' },
            dataType: 'json',
            xhr: xhrOnProgress(function (e) {
                const percent = (e.loaded / e.total * 100 | 0) + '%';//计算百分比
                progressCallback(percent);
            }),
        }).then(function (data) {
            if (data.code == 200) {
                succFun(data.data);
            } else {
                failFun('上传失败:' + data.data);
            }
        }).fail(function (error) {
            failFun('上传失败:' + error.message)
        });
    },
    init_instance_callback: function (editor) {
        $('#tinymce-app').fadeIn(1000);
        //    editor.execCommand('selectAll');
        //    editor.selection.getRng().collapse(false);
        //    editor.focus();
    }
}).then(function (res) {
    tinymce.feedBackIframeUrl = './plugins/help/docBox.html'; //反馈链接
    tinymceConfig.setFCHtml = function (html) {//设置功能介绍Html
        tinymce.functionHtml = html;
    }
    tinymceConfig.setOMHtml = function (html) {//设置操作手册Html
        tinymce.OperationManualHtml = html;
    }
    tinymceConfig.setCPHtml = function (html) {//设置疑难问答Html
        tinymce.CommonProblemHtml = html;
    }
    tinymceConfig.getHtml = function getHtml() {
        let _html = tinyMCE.editors[tinymceConfig.tinyID].getContent();
        return '<style>.attachment>img{display:inline-block!important;max-width:30px!important;}.attachment>a{display:contents!important;}</style>' + _html;
    }

    tinymceConfig.setHtml = function setHtml(html) {
        return tinyMCE.editors[tinymceConfig.tinyID].setContent(html);
    }
    $.ajax({
        type: "GET",
        url: "./api/setFChtml.json",
        async: true,
        dataType: "json",
        success: function (data) {

            tinymceConfig.setFCHtml(data.data);//设置功能介绍
        },
        error: function () {
        }
    })
    $.ajax({
        type: "GET",
        url: "./api/setList.json",
        async: true,
        dataType: "json",
        success: function (data) {
            let temHtml = data.data
            tinymceConfig.setOMHtml(temHtml)
        }
    })
    $.ajax({
        type: "GET",
        url: "./api/setCP.json",
        async: true,
        dataType: "json",
        success: function (data) {
            tinymceConfig.setCPHtml(data.data);//设置疑难问答
            let _html = $("#" + tinymceConfig.tinyID).html();
            if (_html.indexOf('<div id="fvContentID">') != -1) {
                _html = _html.match(/<div id="fvContentID">([\s\S]*)<\/div>/)[1];
            }
            tinymceConfig.setHtml(_html)
        },
        error: function () {
        }
    })
    //
    //公告栏
    $.ajax({
        type: "GET",
        url: "./api/setGG.json",
        async: true,
        dataType: "json",
        success: function (data) {
            $("#tinymce-app").prepend('<div><p style="margin: 5px;">【公告栏】</p>' + data.data + '</div>')
        },
        error: function () {
        }
    })
});



tinymce.init({
    selector: '#' + tinymceConfig.tinyID + '2',
    language: 'zh_CN',
    // menubar:false,
    branding: false,
    min_height: 400,
    max_height: 700,
    plugins: ' print preview clearhtml searchreplace insertdatetime autolink layout fullscreen image upfile link media autosave code codesample table charmap hr pagebreak nonbreaking anchor advlist lists textpattern help emoticons autosave bdmap indent2em lineheight formatpainter axupimgs powerpaste letterspacing imagetools quickbars attachment wordcount autoresize importword',
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
    toolbar: ['|code formatselect fontselect fontsizeselect  forecolor backcolor bold italic underline strikethrough link alignment undo redo  restoredraft| ', 'layout upfile importword letterspacing indent2em table image imagetools'],
    table_style_by_css: true,
    OperationManualHtml: '',
    CommonProblemHtml: '',
    fixed_toolbar_container: '#tinymce-app2 .toolbar',
    custom_ui_selector: '#tinymce-app2',
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
    importword_filter: function (result, insert, message) {
        console.log(result)
        insert(result)
    },
    table_icons: {
        // 'align-right-table': '<svg width="24" height="24"><path d="M5 5h14c.6 0 1 .4 1 1s-.4 1-1 1H5a1 1 0 1 1 0-2zm6 4h8c.6 0 1 .4 1 1s-.4 1-1 1h-8a1 1 0 0 1 0-2zm0 8h8c.6 0 1 .4 1 1s-.4 1-1 1h-8a1 1 0 0 1 0-2zm-6-4h14c.6 0 1 .4 1 1s-.4 1-1 1H5a1 1 0 0 1 0-2z" fill-rule="evenodd"></path></svg>',
        // 'align-left-table': '<svg width="24" height="24"><path d="M5 5h14c.6 0 1 .4 1 1s-.4 1-1 1H5a1 1 0 1 1 0-2zm0 4h8c.6 0 1 .4 1 1s-.4 1-1 1H5a1 1 0 1 1 0-2zm0 8h8c.6 0 1 .4 1 1s-.4 1-1 1H5a1 1 0 0 1 0-2zm0-4h14c.6 0 1 .4 1 1s-.4 1-1 1H5a1 1 0 0 1 0-2z" fill-rule="evenodd"></path></svg>',
        'align-center-table': '<svg width="24" height="24"><path d="M5 5h14c.6 0 1 .4 1 1s-.4 1-1 1H5a1 1 0 1 1 0-2zm3 4h8c.6 0 1 .4 1 1s-.4 1-1 1H8a1 1 0 1 1 0-2zm0 8h8c.6 0 1 .4 1 1s-.4 1-1 1H8a1 1 0 0 1 0-2zm-3-4h14c.6 0 1 .4 1 1s-.4 1-1 1H5a1 1 0 0 1 0-2z" fill-rule="evenodd"></path></svg>',
        'table-to-img': '<svg viewBox="0 0 1024 1024" xmlns="http://www.w3.org/2000/svg" width="24" height="24"><path fill-rule="evenodd" d="M794.4448 85.41696a20.79232 20.79232 0 0 0-29.35296-1.84832l-81.96608 72.50432-0.12288 0.14848c-1.44384 1.31584-2.70848 2.816-3.75808 4.46976-0.46592 0.7424-0.64 1.62816-0.98816 2.39104a20.93568 20.93568 0 0 0-2.06336 10.26048v0.04096c0.19968 2.40128 0.92672 4.77184 1.95072 6.9632 0.08192 0.1536 0.08192 0.42496 0.16384 0.60416l43.86304 87.68512a20.81792 20.81792 0 0 0 37.20192-18.60608l-28.00128-55.96672c54.85056 5.22752 96.62976 22.41536 124.53376 51.44064 38.272 39.7568 37.05344 89.59488 37.08928 90.00448-0.48128 11.20256 8.51456 21.09952 19.968 21.53984a20.7616 20.7616 0 0 0 21.55008-19.83488c0.07168-2.70848 2.26816-67.02592-48.05632-119.85408-32.70656-34.31424-79.31904-55.29088-138.624-63.02208l44.78464-39.6288 0.06656-0.0512a20.72576 20.72576 0 0 0 1.76128-29.24032zM497.59744 334.39232c31.54432-0.0256 57.1136-25.59488 57.1392-57.1392-0.0256-31.54944-25.59488-57.1136-57.1392-57.1392-31.54432 0.0256-57.1136 25.59488-57.1392 57.1392 0.0256 31.54944 25.58976 57.1136 57.1392 57.1392z m0-83.1232a25.984 25.984 0 1 1 0 51.92704 25.97888 25.97888 0 0 1-24.87296-27.04896 25.97376 25.97376 0 0 1 24.87296-24.87808z" p-id="34952" fill="#2c2c2c"></path><path d="M946.33472 519.02976c-8.44288-37.59104-44.98432-65.9456-88.6528-65.9456h-207.95904l-0.4608-231.49056c0-45.55264-40.62208-82.60096-90.54208-82.60096h-393.4208c-49.92512 0-90.5472 37.04832-90.5472 82.60096v307.8144c0 45.55264 40.61696 82.58048 90.5472 82.58048h208.39424v231.56224c0 5.64224 0.66048 11.2384 1.8432 16.5888 8.46336 37.63712 45.00992 65.99168 88.6784 65.99168h393.4464c49.92 0 90.5216-37.0688 90.5216-82.58048v-307.8144a73.64608 73.64608 0 0 0-1.84832-16.70656zM116.3264 221.568c0-22.61504 21.95968-41.05216 48.96768-41.05216h393.44128c27.008 0 48.96768 18.432 48.96768 41.05216v231.49056h-101.7856c-11.60704-20.22912-14.1568-24.23296-19.29728-31.9488-0.72704-1.18272-1.52576-2.98496-2.50368-5.05344-6.64576-13.952-19.0464-39.86944-45.37344-44.32896-18.0224-3.02592-36.75136 5.21216-55.79776 24.50944-28.98944 29.37856-35.04128 23.39328-41.3696 17.13664a42.46528 42.46528 0 0 0-3.13856-2.93376c-2.02752-2.43712-7.5776-12.53888-12.0576-20.66432-20.95616-38.13888-49.69984-90.3424-87.08608-91.49952l-0.77312-0.02048c-30.40256 0-78.30016 57.82016-122.19904 122.10688V221.568z m790.30272 412.47232v209.4592c0 22.5792-21.98528 41.03168-48.93184 41.03168H464.23552c-27.008 0-48.96768-18.45248-48.96768-41.03168v-307.80928c0-22.64064 21.9392-41.05216 48.96768-41.05216h393.4464c27.008 0 48.96768 18.41152 48.96768 41.05216v98.35008h-0.02048z" p-id="34953" fill="#2c2c2c"></path><path d="M658.64192 591.19104a20.78208 20.78208 0 0 1-20.80256 20.75136H471.60832a20.79232 20.79232 0 0 1 0-41.5488h166.23104a20.81792 20.81792 0 0 1 20.80256 20.79744z m-41.53344 173.11744a20.80256 20.80256 0 0 1-20.80256 20.80256h-124.672a20.77696 20.77696 0 0 1 0-41.55392h124.67712c11.4432 0.00512 20.79744 9.33376 20.79744 20.75136z m145.4336-86.53312a20.74112 20.74112 0 0 1-20.78208 20.70528H471.63392a20.70528 20.70528 0 0 1-20.77696-20.63872v-0.06656a20.7872 20.7872 0 0 1 20.77696-20.80256h270.12608a20.82304 20.82304 0 0 1 20.78208 20.80256z"></path></svg>'
    }
    , images_upload_handler: function (blobInfo, succFun, failFun) {//自定义插入图片函数  blobInfo: 本地图片blob对象, succFun(url|string)： 成功回调（插入图片链接到文本中）, failFun(string)：失败回调
        var file = blobInfo.blob();


        var reader = new FileReader();
        reader.onload = function (e) {
            // target.result 该属性表示目标对象的DataURL
            // console.log(e.target.result);
            succFun(e.target.result)
        }
        reader.readAsDataURL(file)
    },
    file_picker_callback: function (succFun, value, meta) { //自定义文件上传函数 
        var filetype = '.pdf, .txt, .zip, .rar, .7z, .doc, .docx, .xls, .xlsx, .ppt, .pptx, .mp3, .mp4';
        var input = document.createElement('input');
        input.setAttribute('type', 'file');
        input.setAttribute('accept', filetype);
        input.click();
        input.onchange = function () {
            var file = this.files[0];
            var data = new FormData();
            data.append("file", file);
            $.ajax({
                data: data,
                type: 'GET',
                url: './api/file.json',
                header: { 'Content-Type': 'multipart/form-data' },
                cache: false,
                contentType: false,
                processData: false,
                dataType: 'json',
                xhr: xhrOnProgress(function (e) {
                    const percent = (e.loaded / e.total * 100 | 0) + '%';//计算百分比
                    // console.log(percent);
                    progressCallback(percent);

                }),
            }).then(function (data) {
                if (data.code == 200) {
                    succFun(data.data, { text: data.data });
                }
            }).fail(function (error) {
                failFun('上传失败:' + error.message)
            });
        }
    },
    file_callback: function (file, succFun) { //文件上传  file:文件对象 succFun(url|string,obj) 成功回调
        var data = new FormData();
        data.append("file", file);
        $.ajax({
            data: data,
            type: 'GET',
            url: './api/file.json',
            cache: false,
            contentType: false,
            processData: false,
            header: { 'Content-Type': 'multipart/form-data' },
            dataType: 'json',
            xhr: xhrOnProgress(function (e) {
                const percent = (e.loaded / e.total * 100 | 0) + '%';//计算百分比
                progressCallback(percent);
            }),
        }).then(function (data) {
            if (data.code == 200) {
                succFun(data.data, { text: file.name });
            }
        }).fail(function (error) {
            // failFun('上传失败:' + error.message)
        });
    },
    init_instance_callback: function (editor) {


        var html2 = '<p>This is tinymce plugins 该项目主要为 tinymce 富文本编译器的扩展插件，或增强优化插件 目前整理完成插件列表如下：</p>' +
            '<p style="text-indent: 2em; text-align: justify; line-height: 1.5;"><input checked="checked" type="checkbox" />&nbsp;imagetools [增强优化]： 图片编辑工具插件， 对图片进行处理。优化跨域，功能更丰富；</p>' +
            '<p style="text-indent: 2em; text-align: justify; line-height: 1.5;"><input checked="checked" type="checkbox" />&nbsp;table [增强优化]：表格插件，处理表格。 增强优化表格控制，增加表格转图片功能，便捷布局按钮；</p>' +
            '<p style="text-indent: 2em; text-align: justify; line-height: 1.5;"><input checked="checked" type="checkbox" />&nbsp;indent2em[增强优化]：首行缩进插件。提供中文段落排版的首行缩进2个字符的功能。增强优化 加入字间距非默认情况，也能实现准确首行缩进2字符；</p>' +
            '<p style="text-indent: 2em; text-align: justify; line-height: 1.5;"><input checked="checked" type="checkbox" />&nbsp;letterspacing：设置间距插件。可以设置文档中的文字间距；</p>' +
            '<p style="text-indent: 2em; text-align: justify; line-height: 1.5;"><input checked="checked" type="checkbox" />&nbsp;layout： 一键布局插件。可以给文档段落进行一键快速排版布局；</p>' +
            '<p style="text-indent: 2em; text-align: justify; line-height: 1.5;"><input checked="checked" type="checkbox" />&nbsp;importword： 导入word插件。可以直接导入word ,并且保证word中图片不会丢失，自动转为base64;</p>' +
            '<p style="text-indent: 2em; text-align: justify; line-height: 1.5;"><input checked="checked" type="checkbox" />&nbsp;upfile： 文件上传。可以点击导入文件，可自定义编辑文件名;</p>' +
            '<p>表格样例</p>' +
            '<table style="border-collapse: collapse; width: 99.8937%;" border="1">' +
            '<tbody><tr><td style="width: 49.2138%; text-align: center;">&nbsp;表格可以转化为图片</td><td style="width: 49.267%; text-align: center;">表格可以转化为图片</td></tr><tr><td style="width: 49.2138%; text-align: center;">表格可以转化为图片</td><td style="width: 49.267%; text-align: center;">表格可以转化为图片</td></tr></tbody></table>' +
            '<p>图片样例</p>' +
            '<p><img style="display: block; margin-left: auto; margin-right: auto;"  src="https://s3.ax1x.com/2020/12/28/ro4Lng.png" alt="20201227164654484" /></p>' +
            '<a href="https://github.com/Five-great/tinymce-plugins" target="_bank">github项目地址</a>&nbsp;&nbsp;&nbsp;' +
            '<a href="https://blog.csdn.net/qq_41923622/article/details/111810804" target="_bank">CSDN博客</a>&nbsp;&nbsp;' +
            '<a href="https://blog.fivecc.cn" target="_bank">个人博客</a>' +
            '<p>联系邮箱：fivecc@qq.com</p>'
        tinyMCE.editors[tinymceConfig.tinyID + '2'].setContent(html2);
        $('#tinymce-app2').fadeIn(1000);
        //    editor.execCommand('selectAll');
        //    editor.selection.getRng().collapse(false);
        //    editor.focus();
    }

});