$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {

        },
        mounted: function () {
            var myChart = echarts.init($dom("#chartsDOM")[0]);
            console.log(myChart);
            // 显示 loading 动画
            myChart.showLoading();
            this.getmapdata().then(function (data) {
                console.error(data);
                myChart.hideLoading();
                // 注册地图(数据放在axios返回对象的data中哦)
                echarts.registerMap('china', data);
                var option = {
                    title: {
                        text: '学 员 地 理 位 置 分 布 ',
                        subtext: '登录数据分析',
                        left: 'center',
                        top: 30,
                        textStyle: {
                            color: '#333', // 标题文本颜色
                            fontSize: 25, // 标题字体大小
                            fontWeight: 'bold', // 标题字体加粗
                            letterSpacing: 2, // 字符间距
                            textBorderColor: 'rgba(255, 255, 255, 0.5)', // 文字描边颜色
                            textBorderWidth: 2, // 文字描边宽度
                            textShadowColor: 'rgba(0, 0, 0, 0.5)', // 文字阴影颜色
                            textShadowBlur: 10, // 文字阴影模糊程度
                            textShadowOffsetX: 0, // 文字阴影水平偏移
                            textShadowOffsetY: 0 // 文字阴影垂直偏移
                        },
                        subtextStyle: {
                            fontSize: 18
                        }
                    },
                    visualMap: {
                        min: 0,
                        max: 100,
                        calculable: true,
                        inRange: {
                            color: ['lightskyblue', 'yellow', 'orangered']
                        }
                    },
                    series: [
                        {
                            name: '中国地图',
                            type: 'map',
                            map: 'china',// 这个是上面注册时的名字哦，registerMap（'这个名字保持一致'）
                            label: {
                                show: true,
                                position: 'center'
                            },
                            zoom: 1.32, //当前视角的缩放比例
                            aspectScale: 0.83,
                            layoutCenter: ["50%", "50%"], //地图位置
                            layoutSize: '100%',
                            itemStyle: {
                                normal: {//未选中状态
                                    borderWidth: 1,//边框大小
                                    borderColor: '#aaa',    //边框颜色
                                    areaColor: 'rgba(255,255,255,0.3)',//背景颜色
                                    label: {
                                        show: true//显示名称                                      
                                    }
                                },
                                emphasis: {// 也是选中样式
                                    borderWidth: 2,
                                    borderColor: '#999',
                                    areaColor: 'rgba(255,255,255,0.6)',
                                    label: {
                                        show: true,
                                        textStyle: {
                                            fontSize: 16,
                                            color: '#333'
                                        }
                                    },
                                    shadowColor: 'rgba(0, 0, 0, 0.3)', // 阴影颜色
                                    shadowBlur: 10, // 阴影模糊大小
                                    shadowOffsetX: 2, // 阴影水平偏移
                                    shadowOffsetY: 2 // 阴影垂直偏移
                                }
                            },
                            data: [
                                { name: '北京', value: 90, count: 30 },
                                { name: '天津市', value: 70 },
                                { name: '河北省', value: 50 },
                                { name: '辽宁省', value: 50, count: 40 },
                                { name: '吉林省', value: 50, count: 60 }
                                // ... 更多省市数据
                            ]
                        }
                    ],
                    // 提示框，鼠标移入
                    tooltip: {
                        show: true, //鼠标移入是否触发数据
                        trigger: "item", //出发方式
                        formatter: function (param) {
                            let data = param.data;
                            let count = data && data.count ? data.count : 0;
                            return `${param.name}<br/>
                            个数: ${count}`;;
                        },
                    },

                };
                myChart.on('click', function (params, ev, n) {
                    //console.log("点击了图表");
                    //console.log(params);
                    if (params.componentType === 'series') {
                        // 点击在系列上
                        if (params.seriesType === 'map') {
                            // 点击在地图系列上
                            let data = params.data;
                            console.log('点击的地图区域名称：', data.name);
                            console.log('对应的数值：', data.value);
                            // 其他需要的操作
                        }
                    }
                });
                myChart.on('selectchanged', function (params) {
                    //console.log(params);
                });
                myChart.setOption(option);
            });
            window.addEventListener("resize", () => {
                // 自动渲染echarts
                myChart.resize();
            });
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: t => !$api.isnull(t.account)
        },
        watch: {
            'brw_posi': function (nv, ov) {
                //if (nv.longitude > 0 && nv.latitude > 0)
                this.getposi();
            }
        },
        methods: {
            //获取地图数据
            getmapdata: function () {
                var url = '/Utilities/ChinaMap/100000_full.json';
                return new Promise(function (resolve, reject) {
                    $dom.get(url, function (req) {
                        console.log(req);
                        resolve(req);
                    });
                });
            }
        }
    });

}, ['/Utilities/echarts/echarts.min.js',
    '/Utilities/baiduMap/map_show.js']);
