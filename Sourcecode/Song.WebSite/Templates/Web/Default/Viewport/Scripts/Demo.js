$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            org: {}, //当前机构
            datas: null  //统计数据
        },
        mounted: function () {
            var myChart = echarts.init($dom("#chartsDOM")[0]);
            console.log(myChart);
            // 显示 loading 动画
            myChart.showLoading();
            //获取地图的图形数据
            this.getmapdata().then(function (data) {
                //console.error(data);
                let mapdata = data.map; //地图的图形数据
                console.log(data);
                myChart.hideLoading();
                // 注册地图(数据放在axios返回对象的data中哦)
                echarts.registerMap('china', mapdata);
                var option = {
                    title: {
                        text: '学 员 地 理 位 置 分 布 ',
                        subtext: '-- 登录数据分析 --',
                        left: 'center',
                        top: 30,
                        textStyle: {
                            color: 'rgb(15 98 183)', // 标题文本颜色
                            fontSize: 25, // 标题字体大小
                            fontWeight: 'bold', // 标题字体加粗
                            letterSpacing: 2, // 字符间距
                            textBorderColor: 'rgba(255, 255, 255, 0.8)', // 文字描边颜色
                            textBorderWidth: 6, // 文字描边宽度
                            textShadowColor: 'rgba(0, 0, 0, 0.5)', // 文字阴影颜色
                            textShadowBlur: 10, // 文字阴影模糊程度
                            textShadowOffsetX: 0, // 文字阴影水平偏移
                            textShadowOffsetY: 0 // 文字阴影垂直偏移
                        },
                        subtextStyle: {
                            fontSize: 16
                        }
                    },
                    visualMap: {
                        min: 0,
                        max: data.max * 1.2,
                        calculable: true,
                        inRange: {
                            color: ['#fff', 'lightskyblue', 'yellow', 'orangered']
                        }
                    },
                    //backgroundColor: '#f4f4f4', // 设置图表的背景色
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
                            data: data.data //统计数据                         
                        }
                    ],
                    // 提示框，鼠标移入
                    tooltip: {
                        show: true, //鼠标移入是否触发数据
                        trigger: "item", //出发方式
                        formatter: function (param) {
                            let data = param.data;
                            let count = data && data.count ? data.count : 0;
                            if (count > 0) {
                                return `<b>${data.fullname}</b><br/>
                            登录人次: ${count} 占比:${data.value}%`;
                            } else {
                                if (!data || !data.fullname) return param.name;
                                return `<b>${data.fullname}</b><br/>
                                登录人次: ${count}`;
                            }
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
                            if (data == null) return;
                            console.log('点击的地图区域名称：', data.fullname);
                            console.log('行政区划编码：', data.id);
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
        },
        watch: {

        },
        methods: {
            //获取地图数据
            getmapdata: function () {
                var th = this;
                var url = '/Utilities/ChinaMap/china_full.json';
                return new Promise(function (resolve, reject) {
                    $dom.get(url, function (mapdata) {
                        th.getSummary().then(function (summary) {
                            //计算总数
                            let total = summary.reduce((total, obj) => total + obj.count, 0);
                            var maxvalue = 0;       //地图数据项目中的最大value，是百分比
                            //
                            let data = [];      //图表数据
                            let array = mapdata.features;
                            for (let i = 0; i < mapdata.features.length; i++) {
                                const el = mapdata.features[i];
                                const id = el.id;   //行政区划的代码
                                const name = el.properties.name; //行政区划的简名
                                //从统计数据中查询，地图中的行政区划是简称，而统计数据中的行政区划是全称
                                let item = summary.find(obj => obj.area.indexOf(name) > -1);
                                if (item == null) {
                                    data.push({
                                        'name': name, 'value': 0, 'fullname': name, 'id': id, 'count': 0
                                    });
                                } else {
                                    let percent = Math.floor(item.count / total * 1000) / 10;
                                    percent = isNaN(percent) ? 0 : percent;
                                    maxvalue = percent > maxvalue ? percent : maxvalue;
                                    data.push({
                                        'name': name, 'value': percent,
                                        'fullname': item.area, 'id': id, 'count': item.count
                                    });
                                }
                            }
                            //分别是：地图图形数据（行政区划的线框坐标），每个省市的统计数据（登录数据的人次），最大平均值
                            resolve({ 'map': mapdata, 'data': data, 'max': maxvalue, 'total': total });
                        });
                    });
                });
            },
            //获取统计数据
            getSummary: function () {
                var th = this;
                return new Promise(function (resolve, reject) {
                    //获取当前机构
                    $api.get('Organization/Current').then(function (req) {
                        if (req.data.success) {
                            let org = req.data.result;
                            // th.org = org;
                            //当前机构下的登录数据统计
                            $api.get('Account/LoginLogsSummary', { 'orgid': org.Org_ID, 'province': '', 'city': '' })
                                .then(function (req) {
                                    if (req.data.success) {
                                        //th.datas = req.data.result;
                                        resolve(req.data.result);
                                    } else {
                                        console.error(req.data.exception);
                                        throw req.config.way + ' ' + req.data.message;
                                    }
                                }).catch(err => console.error(err))
                                .finally(() => { });
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => { });
                });

            }
        }
    });

}, ['/Utilities/echarts/echarts.min.js',
    '/Utilities/baiduMap/map_show.js']);
