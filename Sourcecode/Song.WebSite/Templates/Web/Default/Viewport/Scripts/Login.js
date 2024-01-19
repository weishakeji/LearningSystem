$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            org: {}, //当前机构
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
                        zoom: 1.2, //当前视角的缩放比例
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
        },
        mounted: function () {
            var th = this;
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    th.org = req.data.result;
                    th.form.orgid = th.org.Org_ID;
                    th.createmap();
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => { });
        },
        created: function () {

        },
        computed: {
            //表格高度
            tableHeight: () => document.body.clientHeight - 70
        },
        watch: {
            datas: function (nv, ov) {
                console.log(nv);
            }
        },
        methods: {
            //选择时间区间
            selectDate: function (start, end) {
                this.form.start = start;
                this.form.end = end;
                this.onserch();
            },
            //创建地图
            createmap: function () {
                var th = this;
                th.myChart = echarts.init($dom("#chartsDOM")[0]);
                // 显示 loading 动画
                th.myChart.showLoading();
                //获取地图的图形数据
                th.getmapdata().then(function (mapdata) {
                    th.mapdata = mapdata;
                    th.myChart.hideLoading();
                    th.myChart.on('click', params => {
                        if (params.componentType != 'series' || params.seriesType != 'map') return;
                        if (params.data == null) return;
                        th.eventMapclick(params.data);
                    });
                    th.myChart.setOption(th.option);
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
                th.getSummary().then(function (data) {
                    th.datas = data.data;
                    th.total = data.total;
                    th.myChart.setOption({
                        series: [{ data: th.datas }],
                        visualMap: {
                            max: data.max
                        },
                        title: {
                            'subtext': '-- 登录数据分析 (共' + th.total + '人次) --'
                        }
                    });
                    //console.log(data);
                }).finally(() => th.loading = false);
            },
            //获取统计数据,并结合地图数据，生成所需的数据格式
            getSummary: function () {
                var th = this;
                return new Promise(function (resolve, reject) {
                    //当前机构下的登录数据统计
                    $api.get('Account/LoginLogsSummary', th.form)
                        .then(function (req) {
                            if (req.data.success) {
                                let summary = req.data.result;
                                //计算总数
                                let total = summary.reduce((total, obj) => total + obj.count, 0);
                                var maxvalue = 0;       //地图数据项目中的最大value，是百分比
                                //
                                let data = [];      //图表数据
                                let mapdata = th.mapdata == null ? [] : th.mapdata.features;
                                for (let i = 0; i < mapdata.length; i++) {
                                    const el = mapdata[i];
                                    const id = el.id;   //行政区划的代码
                                    const name = el.properties.name; //行政区划的简名
                                    //从统计数据中查询，地图中的行政区划是简称，而统计数据中的行政区划是全称
                                    let index = summary.findIndex(obj => obj.area.indexOf(name) > -1);
                                    if (index < 0) {
                                        data.push({
                                            'name': name, 'value': 0, 'fullname': name, 'id': id, 'count': 0
                                        });
                                    } else {
                                        let item = summary[index];
                                        let percent = Math.floor(item.count / total * 1000) / 10;
                                        percent = isNaN(percent) ? 0 : percent;
                                        maxvalue = percent > maxvalue ? percent : maxvalue;
                                        data.push({
                                            'name': name, 'value': percent,
                                            'fullname': item.area, 'id': id, 'count': item.count
                                        });
                                    }
                                    summary.splice(index, 1);
                                }
                                //其它区域
                                for (let i = 0; i < summary.length; i++) {
                                    const item = summary[i];
                                    data.push({
                                        'name': item.area, 'value': item.count,
                                        'fullname': '(其它)', 'id': -1, 'count': item.count
                                    });
                                }
                                //按登录次数倒序
                                data.sort((a, b) => b.count - a.count);
                                //分别是：地图图形数据（行政区划的线框坐标），每个省市的统计数据（登录数据的人次），最大平均值
                                resolve({ 'map': mapdata, 'data': data, 'max': maxvalue, 'total': total });
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err))
                        .finally(() => { });
                });

            },
            //地图的点击事件
            eventMapclick: function (data) {
                console.log('地图区域名称：', data.fullname);
                console.log('行政区划编码：', data.id);
                console.log('数值：', data.value);
                this.$refs['province'].show(data.id);
                return;
                var th = this;
                // 其他需要的操作
                th.getmapdata(data.id).then(function (mapdata) {
                    console.log(mapdata);
                    //;
                    echarts.registerMap('china_' + data.id, mapdata);

                    th.myChart.setOption({
                        series: [{ map: 'china_' + data.id, zoom: 0.9 }],

                    });
                });
            }
        }
    });
    //省级区划的地图
    Vue.component('province', {
        //number:要显示的值
        //icon:图标
        //unit:数值的单位
        props: ['code', 'area', 'option', 'orgid'],
        data: function () {
            return {
                display: false,     //是否显示
                datas: null,    //统计数据
                total: 0,        //登录人次的总数
                loading: false,
            }
        },
        watch: {

        },
        computed: {
        },
        mounted: function () {

        },
        methods: {
            show: function (code) {

                this.code = code;
                this.createmap();
            },
            //创建地图
            createmap: function () {
                var th = this;
                if (th.myChart == null)
                    th.myChart = echarts.init($dom("#province_map")[0]);
                // 显示 loading 动画
                th.myChart.showLoading();
                //获取地图的图形数据
                th.getmapdata(th.code).then(function (mapdata) {
                    th.mapdata = mapdata;
                    th.display = true;
                    th.myChart.hideLoading();
                    /*
                    th.myChart.on('click', params => {
                        if (params.componentType != 'series' || params.seriesType != 'map') return;
                        if (params.data == null) return;
                        //th.eventMapclick(params.data);
                    });     */
                    th.myChart.setOption(th.option);
                    th.myChart.setOption({
                        series: [{ name: '', map: th.code, zoom: 0.8, data: {} }],
                    });
                    //查询统计并加载数据，前面只是显示地图
                    //th.onserch();
                });
                /*
                window.addEventListener("resize", () => {
                    if (th.myChart != null)
                        th.myChart.resize();
                });*/
            },
            getmapdata: function (code) {
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
            }
        },

        template: `<div id="province_map" :style="'visibility:'+(display ? 'visible' : 'hidden')" @click="display=false">
              
          </div>`
    });

}, ['/Utilities/echarts/echarts.min.js',
    '/Utilities/baiduMap/map_show.js',
    '/Utilities/Components/date_range.js',]);
