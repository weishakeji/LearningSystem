// 图标选择
$dom.load.css(['/Utilities/Components/Styles/icons.css']);
Vue.component('icons', {
    props: ['selected'],
    data: function () {
        return {
            data: [],
            show: false,         //是否显示
            search: '',      //用于检索的字符
            loading: false
        }
    },
    watch: {
        'selected': function (val, old) {
            this.show = false;
            this.$emit('change', val);
        }
    },
    computed: {
        //查询的结果
        'query_result': function () {
            var arr = [];
            var search = this.search;
            if (search == null || search == '') return [];
            for (let i = 0; i < this.data.length; i++) {
                const item = this.data[i];
                var key = '', val = '';
                for (var k in item) {
                    key = k;
                    val = item[k];
                }
                if (key.indexOf(search) > -1 || val.indexOf(search) > -1) {
                    arr.push(item);
                }
            }
            return arr;
        }
    },
    created: function () {
        var th = this;
        th.loading = true;
        //加载图标
        $api.cache('Platform/IconJson').then(function (req) {
            if (req.data.success) {
                th.data = req.data.result;
            } else {
                throw req.data.message;
            }
        }).catch(err => console.error(err))
            .finally(() => th.loading = false);
    },
    methods: {

    },
    template: ` <el-dialog :visible.sync="show" id="dialog_icons">
    <div slot="title">
        <span><icon>&#xa007</icon> 图标选择</span>
        <el-input placeholder="请输入内容" suffix-icon="el-icon-search" clearable v-model.trim="search"></el-input>    
        <span class="result_count" v-if="search!=''">查到 {{query_result.length}} 个</span>    
    </div>
    <div v-if="query_result.length>0"  class="query_result" >
        <template v-for="ico in query_result">
            <span v-for="(value, key)  in ico" v-html="'&#x'+key+';'" @click="selected=key;show=false;"
            :title="value"  :selected="key==selected"></span>
        </template>  
    </div>
    <div>
        <loading v-if="loading">loading...</loading>
        <template v-for="ico in data">
            <span v-for="(value, key)  in ico" v-html="'&#x'+key+';'" @click="selected=key;show=false;"
            :title="value"  :selected="key==selected"></span>
        </template>   
    </div>
    </el-dialog>`
});
