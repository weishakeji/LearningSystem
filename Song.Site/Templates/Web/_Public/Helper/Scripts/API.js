var menvue = new Vue({
    //el: '#menu',
    data: {
        message: '测试',
        arr: [1, 2]         //接口列表
    },
    created: function () {
        var th = this;
        $api.get("helper/List").then(function (req) {
            if (req.data.success) {
                th.arr = req.data.result;
                
            }
        });
    }
});
menvue.$mount('#menu');

// 注册组件
Vue.component('methods', {
    // 声明 props，用于向组件传参
    props: ['name', 'intro', 'index'],
    data: function () {
        return {
            methods: [],     //方法列表
            open: false      //是否打开方法列表
        }
    },
    methods: {
        //显示方法的参数
        showname: function (sign) {
            return '参数：' + sign.substring(sign.indexOf('('));
        },
        //方法的点击事件
        methodClick: function (classname, method) {
            var txt = "类名" + classname + "，方法：" + method.FullName;
            rvue.message=txt;
            //alert(txt);
        }
    },
    created: function () {
        var name = this.name;
        var th = this;
        $api.get("helper/Methods", { classname: name }).then(function (req) {
            if (req.data.success) {
                th.methods = req.data.result;
            }
        });
    },
    // 
    template: "<div>\
      <div class='classname' v-on:click='open=!open' :title='\"摘要：\"+intro'>\
            {{index}}. {{ name }} \
            <span>{{intro}}</span>\
            <i class='el-icon-arrow-right' v-show='!open'></i><i class='el-icon-arrow-down' v-show='open'></i>\
       </div>\
       <dl class='methods' v-show='open'>\
       <dt v-if='intro.length>0'>摘要：{{intro}}</dt>\
            <dd v-for='(item,i) in methods' v-on:click='methodClick(name,item)'>\
                {{index}}.{{i+1}}.{{item.Name}} \
                <div>{{showname(item.FullName)}}</div>\
            </dd>\
       </dl>\
       </div>"
})

var rvue = new Vue({
    //el: '#menu',
    data: {
        method:{},
        message:''
    },
    created: function () {
       
    }
});
rvue.$mount('context');