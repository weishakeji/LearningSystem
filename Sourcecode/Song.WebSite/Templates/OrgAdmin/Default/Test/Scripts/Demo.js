$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {           
            organ: {},
            config: {},      //当前机构配置项        
            datas: {},
           init: {
                language_url: "/lib/js/zh_CN.js",
                language: "zh_CN",
                height: 430,
                plugins:"link lists image code table colorpicker textcolor wordcount contextmenu",
                toolbar:"bold italic underline strikethrough | fontsizeselect | forecolor backcolor | alignleft aligncenter alignright alignjustify|bullist numlist |outdent indent blockquote | undo redo | link unlink image code | removeformat",
                branding: false,
                images_upload_handler:(blobInfo, success,failure)=> {
                  success('data:image/jpeg;base64,' + blobInfo.base64())
                }
             },
            loading_init: true
        },
        components: {
            "tinymce-editor": Editor
          },
        mounted: function () {
            $api.bat(              
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                vapp.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果             
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
           
        },
        watch: {
        },
        methods: {
        }
    });

});
