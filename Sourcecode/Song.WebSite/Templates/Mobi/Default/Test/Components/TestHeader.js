//搜索框
$dom.load.css([$dom.pagepath() + 'Components/Styles/TestHeader.css']);
Vue.component('test_header', {
    props: ['title', 'icon','course'],
    data: function () {
        return {
            couid: $api.querystring("couid"),
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
                        'sortid': $api.querystring("sortid", '')
                    });
                    history.pushState({}, "", url);
                    window.navigateTo(url);
                }
            },
            deep: true
        }
    },
    computed: {
        'tit': function () {
            if (this.title) return this.title;
            return '在线测试';
        },
        'ico': function () {
            if (this.icon) return this.icon;
            return 'e84b';
        }
    },
    mounted: function () {
    },
    methods: {
        onSearch: function () {
            if (this.search == '') return;
            var file = $dom.file().toLowerCase();
            file = "index";
            var url = $api.url.set(file, {
                's': encodeURIComponent(this.search),
                'couid': $api.querystring("couid")
            });
            //history.pushState({}, "", url);
            window.navigateTo(url);
        },
        //返回上一页
        goback: function () {
            var file = $dom.file().toLowerCase();
            if (file == 'index') {
                this.gocourse();
            }
            else {
                window.history.go(-1);
            }
        },
        //返回课程
        gocourse: function () {
            var couid = $api.querystring("couid", 0);
            if (couid == 0 && !$api.isnull(this.course)){
                couid=this.course.Cou_ID;
            }
            var url = $api.url.dot(couid, '/mobi/course/Detail');
            window.navigateTo(url);
        }
    },
    template: `<div class="header">
    <icon @click="goback">&#xe748</icon>
    <icon @click="gocourse">&#xe813</icon>
    <van-tag  size="medium" class="sortname" type="success">
        <icon v-html="'&#x'+ico"></icon>
       {{tit}}
    </van-tag>
     <van-search v-model.trim="search" placeholder="请输入检索字符" background="transparent" @search="onSearch">
        <template #action>

        </template>
        <template #right-icon>
            <div @click="onSearch">搜索</div>
        </template>
        <template #left-icon>

        </template>
    </van-search> 
    </div>`
});

