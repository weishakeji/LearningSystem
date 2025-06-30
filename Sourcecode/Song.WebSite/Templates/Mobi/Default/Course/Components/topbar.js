$dom.load.css([$dom.path() + 'Course/Components/Styles/topbar.css']);
Vue.component('topbar', {
    //title:标题
    //icon:图标
    //svg:svg图标，优先使用svg图标
    //size:图标大小
    //setup:是否显示设置按钮
    props: ['title', 'icon', 'svg', 'size', 'setup'],
    data: function () {
        return {
            couid: $api.querystring("couid"),        //课程id  
            loading: false
        }
    },
    watch: {

    },
    computed: {},
    mounted: function () {

    },
    methods: {
    },
    template: `<van-row class="topBox">
        <div class="topbtns">
            <a @click="window.history.back();"><icon large>&#xe748</icon></a>
            <a :href="'../course/Detail.'+couid"><icon course></icon></a>
            <a href="/mobi/"><icon>&#xa020</icon></a>
        </div>
        <span>
            <template v-if="title">
                <icon v-if="svg" :svg="svg" :[size]="true"></icon>
                <icon v-else-if="icon" v-html="'&#x'+icon" :[size]="true"></icon>
                {{title}} 
            </template>           
        </span>
        <icon v-if="setup" class="setup" large  @click="$emit('click')">&#xa00c</icon>
    </van-row>`
});