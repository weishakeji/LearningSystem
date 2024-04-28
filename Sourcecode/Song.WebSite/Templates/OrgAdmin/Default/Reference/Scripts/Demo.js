$ready(['/Utilities/driver/driver.js.iife.js'],
    function () {
        window.vapp = new Vue({
            el: '#vapp',
            data: {
                organ: {},
                config: {},      //当前机构配置项        
                datas: {},
                loading_init: true
            },
            mounted: function () {
                var th = this;
                $api.bat(
                    $api.get('Organization/Current')
                ).then(axios.spread(function (organ) {
                    //获取结果             
                    th.organ = organ.data.result;
                    //机构配置信息
                    th.config = $api.organ(th.organ).config;
                })).catch(err => console.error(err))
                    .finally(() => th.loading_init = false);
            },
            created: function () {
                const driver = window.driver.js.driver;
                /*
                                const driverObj = driver();
                
                                driverObj.highlight({
                                    element: "#some-element",
                                    popover: {
                                        title: "Title",
                                        description: "Description"
                                    }
                                });*/

                const driverObj = driver({
                    showProgress: true,
                    allowClose: true,
                    overlayClickNext: true,          // 点击遮罩进行下一步
                    showButtons: true,               // 在页脚不显示控制按钮
                    padding: 10,                      // padding
                    nextBtnText: '下一步',              // 下一步按钮的文本
                    prevBtnText: '上一步',          // 上一步按钮的文本
                    doneBtnText: '确定',              // 最后一共按钮的文本
                    closeBtnText: '我知道了',            // 关闭按钮的文本
                    showButtons: ['next', 'previous', 'close'],
                    steps: [
                        { element: '#some-element', popover: { title: '测试', description: '最后一共按钮的文本' } },
                        { element: '.top-nav', popover: { title: 'Title', description: 'Description' } },
                        { element: '.sidebar', popover: { title: 'Title', description: 'Description' } },
                        { element: '.footer', popover: { title: 'Title', description: 'Description' } },
                    ]
                });

                driverObj.drive();
            },
            computed: {

            },
            watch: {
            },
            methods: {
            }
        });

    });
