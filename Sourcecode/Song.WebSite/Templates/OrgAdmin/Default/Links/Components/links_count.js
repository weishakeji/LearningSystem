//分类下的链接数
Vue.component('links_count', {
    //title:提示信息
    props: ['sort','title'],
    data: function () {
        return {
            total: 0,       //总数量
            count: 0,         //启用的链接数
            loading: false

        }
    },
    watch: {
        'sort': {
            handler: function (nv, ov) {
                if (JSON.stringify(nv) != '{}' && nv != null)
                    this.getcount(nv);
            }, immediate: true
        }
    },
    methods: {
        getcount: function (sort) {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('Link/SortOfCount', { 'sortid': sort.Ls_Id, 'use': true }),
                $api.get('Link/SortOfCount', { 'sortid': sort.Ls_Id, 'use': null })
            ).then(axios.spread(function (count, total) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw arguments[i].config.way + ' ' + data.message;
                    }
                }
                //获取结果
                th.total = total.data.result;
                th.count = count.data.result;
            })).catch(function (err) {
                console.error(err);
            }).finally(function () {
                th.loading = false;
            });
        }
    },
    template: `<span>
        <loading v-if="loading" bubble>..</loading>
        <template v-else>
            <el-tooltip :content="'当前分类下启用的链接数：'+count+' 个，总计：'+total+' 个'" placement="bottom" effect="light">
                <el-tag type="warning">                        
                    {{title}}{{count}} / {{total}}
                </el-tag>
            </el-tooltip>       
        </template>             
    </span>`
});