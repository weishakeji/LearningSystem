//搜索框
$dom.load.css([$dom.pagepath() + 'Components/Styles/KnlHeader.css']);
Vue.component('knl_header', {
    props: [],
    data: function () {
        return {
            couid: $api.querystring("couid"),
            sortid: $api.querystring("sortid", 0),  //分类id
            search: $api.querystring("s"),       //搜索字符  
            course: {},                  //课程信息
            sorts: [],                   //知识库分类
            show_sort: false,             //是否显示分类
            loading: false
        }
    },
    watch: {
        'search': {
            handler: function (newVal, oldVal) {
                if (newVal == '') {
                    var file = $dom.file().toLowerCase();
                    var url = $api.url.set(file, {
                        's': encodeURIComponent(this.search),
                        'couid': $api.querystring("couid"),
                        'sortid': $api.querystring("sortid", 0)
                    });
                    history.pushState({}, "", url);
                    window.location.href = url;
                }
            },
            deep: true
        }
    },
    computed: {
        defimg: function () {
            return '/Utilities/Images/cou_nophoto.jpg';           
        }
    },
    mounted: function () {
        var th = this;
        th.loading = true;
        $api.bat(
            $api.get('Course/ForID', { 'id': this.couid }),
            $api.get('Knowledge/SortTree', { 'couid': this.couid, 'search': '', 'isuse': true })
        ).then(axios.spread(function (course, sorts) {
            th.loading = false;
            //判断结果是否正常
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i].status != 200)
                    console.error(arguments[i]);
                var data = arguments[i].data;
                if (!data.success && data.exception != null) {
                    console.error(data.message);
                }
            }
            //获取结果
            th.course = course.data.result;
            th.sorts = sorts.data.result;
        })).catch(function (err) {
            console.error(err);
        });
    },
    methods: {
        onSearch: function () {
            if (this.search == '') return;
            var file = $dom.file().toLowerCase();
            file = "knowledges";
            var url = $api.url.set(file, {
                's': encodeURIComponent(this.search),
                'couid': $api.querystring("couid")
            });
            history.pushState({}, "", url);
            window.location.href = url;
        },
        //返回上一页
        goback: function () {
            var file = $dom.file().toLowerCase();
            if (file == 'knowledges') {
                this.gocourse();
            }
            else {
                window.history.go(-1);
            }
        },
        //返回课程
        gocourse: function () {
            var couid = $api.querystring("couid", 0);
            var url = $api.url.dot(couid, '/mobi/course/Detail');
            window.location.href = url;
        },
        //生成树形
        buildTree: function (array, level, order) {
            var html = '';
            for (let i = 0; i < array.length; i++) {
                const item = array[i];
                var index = order + (i + 1) + ".";
                html += "<a href='" + this.buildLink(item) + "' style='text-indent:" + (level * 20) + "px;'>" + index + item.Kns_Name + "</a>";
                if (item.children && item.children.length > 0) {
                    html += this.buildTree(item.children, level + 1, index);
                }
            }
            return html;
        },
        //生成知识分类的链接
        buildLink: function (item) {
            var url = $api.url.set("Knowledges", {
                'couid': this.couid,
                'sortid': item.Kns_ID,
                'sortuid': item.Kns_UID,
                's': this.search
            });
            return url;
        },
        //根据分类id,获取路径
        sortname: function (array) {
            if (array == null) array = this.sorts;
            var sort = '';
            if (array == null) return sort;
            for (let i = 0; i < array.length; i++) {
                const item = array[i];
                if (item.Kns_ID == this.sortid) {
                    return item.Kns_Name;
                } else {
                    if (item.children && item.children.length > 0) {
                        sort += this.sortname(item.children);
                    }
                }
            }
            return sort;
        },
        close: function () {
            var file = $dom.file().toLowerCase();
            var url = $api.url.set(file, {
                's': encodeURIComponent(this.search),
                'couid': $api.querystring("couid")
            });
            //history.pushState({}, "", url);
            window.location.href = url;
        }
    },
    template: `<div><div class="header">
    <icon @click="goback">&#xe748</icon>
    <icon @click="gocourse">&#xe813</icon>
    <van-tag v-if="sortid!=''" closeable size="medium" class="sortname" type="primary" @close="close">
        {{sortname()}}
    </van-tag>
     <van-search v-model.trim="search" placeholder="搜索知识点" background="transparent" @search="onSearch">
        <template #action>

        </template>
        <template #right-icon>
            <div @click="onSearch">搜索</div>
        </template>
        <template #left-icon>

        </template>
    </van-search>
    <icon @click="show_sort=true">&#xa00c</icon>
    </div>
    <van-popup v-model="show_sort" position="right" :style="{ height: '100%',width:'50%' }" id="menu">
        <van-loading size="24px" v-if="loading">加载中...</van-loading>
        <div class='cour-info'>
            <img :src='course.Cou_Logo' v-if='course.Cou_Logo && course.Cou_Logo.length>0'/>
            <img :src='defimg' class='no' v-else/>
            <icon course>{{course.Cou_Name}}</icon>
            <icon subject>{{course.Sbj_Name}}</icon>        
        </div>
        <van-divider>知识分类</van-divider>    
        <div v-if="sorts && sorts.length>0" v-html="buildTree(sorts,0,'')" class="sort_tree"></div>
        <div v-else class="noSort">没有分类信息</div>
    </van-popup>
    </div>`
});

