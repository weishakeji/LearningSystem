$(function () {
    courseCharts();
	CoursePie();
	courseAcctouts();
	courseQues();
});
window.colorList = [
                          '#FE8463','#B5C334','#FCCE10','#E87C25','#27727B',
                           '#C1232B','#9BCA63','#FAD860','#F3A43B','#60C0DD',
                           '#D7504B','#C6E579','#F4E001','#F0805A','#26C0C0'
                        ];
function courseCharts() {
    // 基于准备好的dom，初始化echarts图表
    var myChart = echarts.init(document.getElementById('courCharts'));
    option = {
        title: {
            x: 'center',
            text: '各机构课程数汇总图-1',
            subtext: '共计课程：'+getDataSum("course","count")
        },
        tooltip: {
            trigger: 'item'
        },
        calculable: false,
        grid: {
            borderWidth: 0,
            y: 80,
            y2: 60
        },
        xAxis: [
        {
            type: 'category',
            show: false,
            data: getData("course", "pname", "text")
        }
    ],
        yAxis: [
        {
            type: 'value',
            show: false
        }
    ],
        series: [{
            name: '',
            type: 'bar',
            itemStyle: {
                normal: {
                    color: function (params) {
                        // build a color map as your need.
                        var colorList =window.colorList;
                        return colorList[params.dataIndex]
                    },
                    label: {
                        show: true,
                        position: 'top',
                        formatter: '{b}\n{c}'
                    }
                }
            },
            data: getData("course", "count", "number"),
            markPoint: {
                data: [
                    { type: 'max', name: '最大值' },
                    { type: 'min', name: '最小值' }
                ]
            },
            markLine: {
                data: [
                    { type: 'average', name: '平均值' }
                ]
            }
        }
    ]
    };
    // 为echarts对象加载数据 
    myChart.setOption(option);
}
//课程数汇总的饼图
function CoursePie(){
	 // 基于准备好的dom，初始化echarts图表
    var myChart = echarts.init(document.getElementById('courChartsPie'));
	option = {
    title : {
        text: '各机构课程数汇总图-2',
        subtext: '共计课程：'+getDataSum("course","count"),
        x:'center'
    },
    tooltip : {
        trigger: 'item',
        formatter: "{a} <br/>{b} : {c} %"
    },    
    calculable : true,
    series : [
        {
            name:'百分比',
			 itemStyle: {
                normal: {
                    color: function (params) {
                        // build a color map as your need.
                        var colorList =window.colorList;
                        return colorList[params.dataIndex]
                    },
                    label: {
                        show: true,
                        position: 'top',
                        formatter: '{b}\n{c}%'
                    }
                }
            },
            type:'pie',
            radius : '55%',
            center: ['50%', '60%'],
            data:getDataValue("course")
        }
    ]
};
                    
	 myChart.setOption(option);
}
function courseAcctouts() {
	 // 基于准备好的dom，初始化echarts图表
    var myChart = echarts.init(document.getElementById('accoutsCharts'));
	option = {
    title : {
        text: '各机构注册的用户量',
        subtext: '合计注册用户：'+getDataSum("accouts","count"),
		 x:'center'
    },
    tooltip : {
        trigger: 'axis'
    },
    calculable : false,
    xAxis : [
        {
            type : 'category',
            data : getData("accouts", "pname", "text")
        }
    ],
    yAxis : [
        {
            type : 'value'
        }
    ],
    series : [
        {
            name:'注册量',
            type:'bar',
            data: getData("accouts", "count", "number"),
			itemStyle: {
                normal: {
                    color: function (params) {
                        // build a color map as your need.
                        var colorList =window.colorList;
                        return colorList[params.dataIndex]
                    },
                    label: {
                        show: true,
                        position: 'top',
                        formatter: '{b}\n{c}'
                    }
                }
            },
            markPoint : {
                data : [
                    {type : 'max', name: '最大值'},
                    {type : 'min', name: '最小值'}
                ]
            },
            markLine : {
                data : [
                    {type : 'average', name: '平均值'}
                ]
            }
        }
    ]
};
                    
	 myChart.setOption(option);
}
function courseQues() {
	 // 基于准备好的dom，初始化echarts图表
    var myChart = echarts.init(document.getElementById('quesCharts'));
	option = {
    title : {
        text: '各机构的试题数量',
        subtext: '合计试题数量：'+getDataSum("question","count"),
		 x:'center'
    },
    tooltip : {
        trigger: 'axis'
    },
    calculable : false,
    xAxis : [
        {
            type : 'category',
            data : getData("question", "pname", "text")
        }
    ],
    yAxis : [
        {
            type : 'value'
        }
    ],
    series : [
        {
            name:'试题',
            type:'bar',
            data: getData("question", "count", "number"),
			itemStyle: {
                normal: {
                    color: function (params) {
                        // build a color map as your need.
                        var colorList =window.colorList;
                        return colorList[params.dataIndex]
                    },
                    label: {
                        show: true,
                        position: 'top',
                        formatter: '{b}\n{c}'
                    }
                }
            },
            markPoint : {
                data : [
                    {type : 'max', name: '最大值'},
                    {type : 'min', name: '最小值'}
                ]
            },
            markLine : {
                data : [
                    {type : 'average', name: '平均值'}
                ]
            }
        }
    ]
};
                    
	 myChart.setOption(option);
}


//获取数据
//boxname:数据区的id
//column:数据列的名称
//type:数据类型
function getData(boxname, column, type) {
    var arrayObj = new Array();
    var box = $("#dataArea").find("#" + boxname);
    box.find("row").each(function (index, element) {
        var val = $(this).find(column).text();
        if (type == 'number') arrayObj.push(Number(val));
        if (type == 'text') arrayObj.push(val);
    });
    return arrayObj;
}
//获取饼所有数据
function getDataValue(boxname) {
    var val = getData(boxname,"count","number");
	var name = getData(boxname,"pname","text");
	var sum=getDataSum(boxname,"count");
    var arrayObj = new Array();
	for(var i=0;i<val.length;i++){
		arrayObj.push({value:Math.floor(val[i]/sum*1000)/10,name:name[i]})
	}
    return arrayObj;
}
//获取总数
function getDataSum(boxname, column) {
    var sum=0;
    var box = $("#dataArea").find("#" + boxname);
    box.find("row").each(function (index, element) {
        var val = $(this).find(column).text();
        sum+=Number(val);
    });
    return sum;
}