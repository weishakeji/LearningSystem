$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项        
            stat: {},       //机构的各种统计数据
            loading_init: true,
            loading_stat: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(([platinfo, org]) => {
                //获取结果           
                th.platinfo = platinfo.data.result;
                th.org = org.data.result;
                //机构配置信息
                th.config = $api.organ(th.org).config;
                th.getStatistics(th.org);
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);

            window.setTimeout(function () {

            }, 1000);
            this.$nextTick(function () {

            });
        },
        created: function () {

        },
        computed: {
        },
        watch: {
            'loading_stat': function (nv, ov) {
                if (nv) return;
            }
        },
        methods: {
            //获取统计数据
            getStatistics: function (org) {
                var th = this;
                th.loading_stat = true;
                $api.cache('Organization/Statistics', { 'orgid': org.Org_ID }).then(function (req) {
                    if (req.data.success) {
                        th.stat = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_stat = false);
            }
        },
        filters: {
            //数字转三位带逗号
            'commas': function (number) {
                return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
        }
    });

    //地图展示
    Vue.component('map_display', {
        props: ['org'],
        data: function () {
            return {
                form: { 'orgid': '', 'start': '', 'end': '', 'province': '', 'city': '' },
                datas: null,    //统计数据
                total: 0,        //登录人次的总数
                loading: false,

                myChart: null,    //地图对象
                mapdata: null,   //地图基础数据（用于描绘图形）
                //地图配置项
                option: {
                    title: {
                        text: '学  员  地  理  位  置  分  布 ',
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
                        max: 10,
                        calculable: true,
                        inRange: {
                            color: ['#fff', 'yellow', 'orangered']
                        },
                        show: false     //地图左下角，显示百度分区间的柱状
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
                            zoom: 1.22, //当前视角的缩放比例
                            aspectScale: 0.75,
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
                                    areaColor: 'rgba(255,255,255,1)',
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
                            select: {//这个就是鼠标点击后，地图想要展示的配置
                                disabled: false,//可以被选中
                                itemStyle: {//相关配置项很多，可以参考echarts官网
                                    areaColor: "#77e8e4"//选中
                                }
                            },
                            data: [] //统计数据                         
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
                }
            }
        },
        watch: {
            'org': {
                handler: function (nv, ov) {
                    if (nv != null) {
                        this.form.orgid = nv.Org_ID;
                        this.$nextTick(function(){
                            this.createmap();
                        })
                       
                    }
                }, immediate: true, deep: true
            }
        },
        methods: {
            //创建地图
            createmap: function () {
                var th = this;
                var dom = $dom("[charts]");
                console.log(dom);
                th.myChart = echarts.init($dom("[charts")[0]);
                // 显示 loading 动画
                th.myChart.showLoading();
                //获取地图的图形数据
                th.getmapdata().then(function (mapdata) {
                    th.mapdata = mapdata;
                    th.myChart.hideLoading();
                    th.myChart.on('click', params => {
                        if (params.componentType != 'series' || params.seriesType != 'map') return;
                        if (params.data == null) return;
                        //
                        let data = params.data;
                        console.log('地图区域名称：', data.fullname);
                        console.log('行政区划编码：', data.id);
                        console.log('数值：', data.value);
                        //显示省级区域详图
                        //th.$refs['province'].show(data.id, data.fullname, data.name);
                    });
                    th.myChart.setOption(th.option, true);
                    //查询统计并加载数据，前面只是显示地图
                    th.onserch();
                });
                window.addEventListener("resize", () => {
                    if (th.myChart != null)
                        th.myChart.resize();
                });
            },
            //获取地图的图形数据，并注册到echarts
            //code:行政区划的编码
            //area:行政区划的名称           
            getmapdata: function (code, area) {
                var mapid = code == null ? 'china' : code;  //地图标识
                var url = '/Utilities/ChinaMap/{code}_full.json';
                return new Promise(function (resolve, reject) {
                    let mapdata = echarts.getMap(mapid);
                    if (mapdata) resolve(mapdata);
                    else $dom.get(url.replace('{code}', mapid), result => {
                        echarts.registerMap(mapid, result);
                        resolve(result);
                    });
                });
            },
             //查询统计数据
             onserch: function () {
                var th = this;
                th.loading = true;
                th.getSummary(th.mapdata).then(function (data) {
                    th.datas = data.data;
                    th.total = data.total;
                    th.myChart.setOption({
                        series: [{ data: th.datas }],
                        visualMap: { max: data.max > 0 ? data.max : 1 },
                        title: {
                            subtext: '-- 登录数据分析 (共' + th.total + '人次) --'
                        }
                    });
                    //th.$refs['province'].hide();
                }).finally(() => th.loading = false);
            },
              //获取统计数据,并结合地图数据，生成所需的数据格式
            //province:行政区划的名称
            getSummary: function (mapdata, province) {
                var th = this;
                var form = $api.clone(th.form);
                if (province != null) form.province = province;
                return new Promise(function (resolve, reject) {
                    //当前机构下的登录数据统计
                    $api.get('Account/LoginLogsSummary', form)
                        .then(function (req) {
                            if (req.data.success) {
                                let result = th.calcSummary(mapdata, req.data.result);
                                resolve(result);
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err))
                        .finally(() => { });
                });
            },
            //计算地图数据，生成所需的数据格式
            calcSummary: function (mapdata, logdata) {
                //计算总数
                let total = logdata.reduce((total, obj) => total + obj.count, 0);
                var maxvalue = 0;       //地图数据项目中的最大value，是百分比
                //
                let data = [];      //图表数据
                let features = mapdata == null ? [] : mapdata.features;
                for (let i = 0; i < features.length; i++) {
                    const el = features[i];
                    const id = el.id ? el.id : el.properties.adcode;   //行政区划的代码
                    const name = el.properties.name; //行政区划的简名
                    //从统计数据中查询，地图中的行政区划是简称，而统计数据中的行政区划是全称
                    let index = logdata.findIndex(obj => obj.area.indexOf(name) > -1);
                    if (index < 0) {
                        data.push({
                            'name': name, 'value': 0, 'fullname': name, 'id': id, 'count': 0
                        });
                    } else {
                        let item = logdata[index];
                        let percent = Math.floor(item.count / total * 1000) / 10;
                        percent = isNaN(percent) ? 0 : percent;
                        maxvalue = percent > maxvalue ? percent : maxvalue;
                        data.push({
                            'name': name, 'value': percent,
                            'fullname': item.area, 'id': id, 'count': item.count
                        });
                        logdata.splice(index, 1);
                    }
                }
                //其它区域
                for (let i = 0; i < logdata.length; i++) {
                    const item = logdata[i];
                    if (item.count < 1) continue;
                    let percent = Math.floor(item.count / total * 1000) / 10;
                    percent = isNaN(percent) ? 0 : percent;
                    data.push({
                        'name': item.area, 'value': percent,
                        'fullname': '(其它)', 'id': -1, 'count': item.count
                    });
                }
                //按登录次数倒序
                data.sort((a, b) => b.count - a.count);
                //分别是：地图图形数据（行政区划的线框坐标），每个省市的统计数据（登录数据的人次），最大平均值
                return { 'map': mapdata, 'data': data, 'max': maxvalue, 'total': total };
            },
        },
        template: `<div class="map_display" charts style="height:100%;width:100%;">

        </div>`
    });
}, ['Viewport/Components/piece.js',
    'Viewport/Components/dataitem.js',
    'Viewport/Components/studentage.js',
    'Viewport/Components/studentlogin.js',
    'Viewport/Components/courses_hot.js',

    '/Utilities/echarts/echarts.min.js',
    '/Utilities/baiduMap/map_show.js',
]);
