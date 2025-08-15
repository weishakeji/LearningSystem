//数据库实体列表
$dom.load.css([$dom.path() + 'Components/Styles/entities.css']);
Vue.component('entities', {
    props: [],
    data: function () {
        return {
            //当前选中的项的索引
            index: parseInt($api.querystring('index', 0)),
            activeName: '',


            entities: {},   //所有数据实体的对象         

            search: '',  //搜索字符
            error: '',       //错误信息

            loading: false, //预载中           
        }
    },
    watch: {

    },
    computed: {
        //总数
        'total': function () {
            let total = 0;
            for (var t in this.entities) total++;
            return total;
        },
        //满足查询条件的接口数，不满足查询条件的不显示
        'showcount': function () {
            let total = 0;
            for (var t in this.entities) {
                if (this.show(t, this.entities[t], this.search))
                    total++;
            }
            return total;
        }
    },
    mounted: function () {
        this.getlist();
    },
    methods: {
        //获取API的列表
        getlist: function () {
            var th = this;
            th.loading = true;
            $api.get('helper/Entities').then(req => {
                if (req.data.success) {
                    th.entities = req.data.result;
                    th.handleChange(th.index);
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
        show: function (key, value, search) {
            if (search == null || search.length < 1) return true;
            if (this.indexOf(key, search) || this.indexOf(value.intro, search) || this.indexOf(value.mark, search)) return true;
            return false
        },
        //当前选中
        handleChange: function (idx) {
            if (idx == null || idx.length < 1) return;
            this.activeName = idx;
            let index = parseInt(idx);
            if (index == this.index) return;
            //当前选中的表结构
            let obj = {};
            let count = 0;
            for (const key in this.entities) {
                if (count++ === index) {
                    obj = this.entities[key];
                    obj['name'] = key;
                }
            }
            this.index = index;
            this.$set(obj, 'index', index);
            let url = $api.setpara('index', index);
            history.pushState({}, "", url);
            //触发事件
            this.$emit('selected', obj, index);
        },
    },
    // 
    template: `<div class="entities"> 
        <div loading="p4" v-if="loading"></div>
        <div v-else-if="error!=''"><alert>{{error}}</alert></div>
        <template v-else>
            <div class="bar">
                <el-tag>总计 {{total}} 
                <template v-if="search.length>0"> / 查询到 {{showcount}} 个</template></el-tag>
                <el-input placeholder="搜索" suffix-icon="el-icon-search" clearable v-model.trim="search"></el-input>
            </div>      
            <warning v-if="showcount<=0">没有满足条件的信息</warning>
            <el-collapse accordion v-model="activeName"  @change="handleChange">
                <el-collapse-item  v-for="(value,key,idx) in entities" :name="idx" v-show="show(key,value,search)" :class="{'nointro':value.intro.length<1}">
                    <div slot="title" class="item_title" :current="index==idx">
                        <span v-html="(idx+1)+'.  '+ showsearch(key,search)" class="name"></span>
                        <span class="intro">{{value.mark}}</span> 
                    </div>
                    <div v-if='value.intro.length>0' class="intro"><span>摘要：</span>
                        <span v-html="showsearch(value.intro,search)"></span>
                    </div>                         
                </el-collapse-item>
            </el-collapse>
           
        </template>
    </div>`
});

