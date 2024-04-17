//分类下的链接数
Vue.component('links_count', {
    //title:提示信息
    props: ['sort', 'title'],
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
                //获取结果
                th.total = total.data.result;
                th.count = count.data.result;
            })).catch(err => console.error(err))
                .finally(() => th.loading = false);
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