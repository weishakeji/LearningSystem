//API接口列表
$dom.load.css([$dom.path() + 'Components/Styles/interfaces.css']);
Vue.component('interfaces', {
    props: [],
    data: function () {
        return {
            activeName: '1',

            list: [], //接口列表
            current: null,   //当前选中的项

            search: '',  //搜索字符
            error: '',       //错误信息

            loading: false, //预载中           
        }
    },
    watch: {

    },
    computed: {
        //接口总数
        'total': function () {
            if (this.list.length < 1) return 0;
            let total = 0;
            for (let i = 0; i < this.list.length; i++) {
                if (this.list[i]['methods'])
                    total += this.list[i]['methods'].length;
            }
            return total;
        },
        //满足查询条件的接口数，不满足查询条件的不显示
        'showcount': function () {
            let total = 0;
            for (let i = 0; i < this.list.length; i++) {
                if (this.show(this.list[i], this.search))
                    total++;
            }
            return total;
        }
    },
    mounted: function () {
        this.getapilist();
    },
    methods: {
        //获取API的列表
        getapilist: function () {
            var th = this;
            th.loading = true;
            $api.get('Helper/APIList').then(req => {
                if (req.data.success) {
                    th.list = req.data.result;
                } else {
                    console.error(req.data.exception);
                    th.error = req.data.message;
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //是否存在search，大小写不敏感
        indexOf: function (text, search) {
            var regex = new RegExp('(' + search + ')', 'ig');
            return regex.test(text);
        },
        //接口项，在满足查询的情况下的才会显示
        show: function (item, search) {
            if (search == null || search.length < 1) return true;
            if (this.indexOf(item.Name, search) || this.indexOf(item.Intro, search)) return true;
            if (!item.methods) return true;
            for (var i = 0; i < item.methods.length; i++) {
                const method = item.methods[i];
                if (this.indexOf(method.Name, search) || this.indexOf(method.Intro, search)) return true;
            }
            return false
        },
        //接口的方法列表，加载完成后，触发selected事件
        methodsLoaded: function (method) {
            //是否全部完成
            let alldone = true;
            for (let i = 0; i < this.list.length; i++)
                if (!this.list[i].loaded) alldone = false;
            //假如全部完成
            if (!alldone) return;
            for (let i = 0; i < this.list.length; i++) {
                if (this.list[i].methods && this.list[i].methods.length > 0) {
                    this.selected(this.list[i].methods[i], this.list[i]);
                    //this.activeName = i;  //默认打开第一个折叠
                    return;
                }
            }
        },
        //选择变更后的事件
        selected: function (method, api) {
            this.current = api;
            //设置子组件的当前方法
            let methods = this.$refs['method'];
            let index = this.list.findIndex(item => item.Name == api.Name);
            methods[index].current = method;
            //触发事件
            this.$emit('selected', method, api);
        }
    },
    // 
    template: `<div class="interfaces"> 
        <div loading="p4" v-if="loading"></div>
        <div v-else-if="error!=''"><alert>{{error}}</alert></div>
        <template v-else>
            <div class="bar">
                <el-tag>总数 {{total}} / {{list.length}} </el-tag>
                <el-input placeholder="搜索" suffix-icon="el-icon-search" clearable v-model.trim="search"></el-input>
            </div>      
            <el-collapse v-model="activeName" accordion>
                <el-collapse-item  v-for="(item,idx) in list" :name="idx" v-show="show(item,search)">
                    <div slot="title" class="item_title" :current="current!=null && item.Name==current.Name">
                        <span v-html="(idx+1)+'.  '+ showsearch(item.Name,search)" class="name"></span>
                        <span class="intro">{{item.Intro}}</span> 
                    </div>
                    <div v-if='item.Intro.length>0' class="intro">摘要：
                        <span v-html="showsearch(item.Intro,search)"></span>
                    </div>
                    <methods :api="item" ref="method" :search="search" @load="methodsLoaded" @selected="selected"></methods>
                </el-collapse-item>
            </el-collapse>
            <div v-if="showcount<=0">没有满足条件的接口</div>
        </template>
    </div>`
});

//接口下的方法列表
Vue.component('methods', {
    props: ['api', 'search'],
    data: function () {
        return {
            methods: [], //方法列表
            current: null,   //当前选中的项
            loading: true, //预载中           
        }
    },
    computed: {},
    mounted: function () {
        this.getmethods();
    },
    methods: {
        //获取API的列表
        getmethods: function () {
            var th = this;
            th.loading = true;
            $api.get('Helper/APIMethods', { 'classname': th.api.Name }).then(req => {
                if (req.data.success) {
                    let methods = req.data.result;
                    for (let i = 0; i < methods.length; i++)
                        th.$set(methods[i], 'Paramstring', th.getParamstring(methods[i]));
                    //methods[i].Paramstring = th.getParamstring(methods[i]);
                    th.methods = methods;
                    th.$set(th.api, 'methods', methods);
                    th.$set(th.api, 'loaded', true);    //是否加载完成
                    th.$emit('load', methods, th.api);    //加载完成的事件
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //计算获取接口方法的参数信息，生成字符串
        getParamstring: function (method) {
            let fullname = method.FullName;
            if (fullname.indexOf('(') > -1) {
                let paras = fullname.substring(fullname.indexOf('(') + 1).split(',');
                let str = '';
                for (let t = 0; t < paras.length; t++) {
                    if (paras[t].indexOf('.') > -1) {
                        str += paras[t].substring(paras[t].lastIndexOf('.') + 1);
                        if (t < paras.length - 1) str += ', ';
                    }
                }
                return '(' + str.toLowerCase().replace(/\&/g, '');
            }
            return '';
        },
        //接口项，在满足查询的情况下的才会显示
        show: function (item, search) {
            if (search == null || search.length < 1) return true;
            let parent = this.$parent;
            while (parent.indexOf == null) parent = parent.$parent;
            return parent.indexOf(item.Name, search) || parent.indexOf(item.Intro, search);
        },
        //选择变更后的事件
        selected: function (item) {
            this.current = item;
            this.$emit('selected', item, this.api);
        },
        //是否是当前选中的接口
        iscurrent: function (item) {
            return this.current != null && JSON.stringify(item) === JSON.stringify(this.current);
        }
    },
    // 
    template: `<div>      
        <div v-for="(item,idx) in methods" class="method" :current="iscurrent(item)"
        v-show="show(item,search)" v-on:click="selected(item)">
            <div>
                <span v-html="(idx+1)+'.  '+ showsearch(item.Name,search)"></span>   
                <i>{{item.Paramstring}}</i>
            </div>
            <div v-if='item.Intro.length>0' class="intro">
                <span v-html="showsearch(item.Intro,search)"></span>
            </div>
        </div>
    </div>`
});