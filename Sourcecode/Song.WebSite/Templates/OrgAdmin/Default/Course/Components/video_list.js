//课程视频的列表，供编辑时选择
Vue.component('video_list', {
    props: [],
    data: function () {
        return {
            showpanel: false,     //是否显示
            datas: [],       //数据列表
            total: 1, //总记录数
            totalpages: 1, //总页数
            //查询
            query:
            {
                'pathkey': 'CourseVideo',
                'search': '', 'ext': 'mp4',
                'size': 20, 'index': 1
            },

            drawer: false,
            currfile: {},        //当前文档对象

            loading: false
        }
    },
    watch: {
        'showpanel': {
            handler: function (nv, ov) {
                if (nv == true && this.datas.length < 1) {
                    var th = this;
                    this.$nextTick(function () {
                        th.handleCurrentChange(1);
                    });
                }
                if (!nv) this.hidedrawer();
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        $dom.load.css([$dom.path() + 'Course/Components/Styles/video_list.css']);
    },
    methods: {
        //显示视频文件列表的面板
        show: function () {
            this.showpanel = true;
            //this.handleCurrentChange(1);
        },
        hide: function () {
            this.showpanel = false;
        },
        //获取文件
        handleCurrentChange: function (index) {
            if (index != null) this.query.index = index;
            var th = this;
            //每页多少条，通过界面高度自动计算
            var area = $dom(".video_list_dialog .el-dialog__body");
            var maxheight = area.height();
            th.query.size = Math.floor(maxheight / 31);
            th.loading = true;
            $api.put("Accessory/PagerForFiles", th.query).then(function (d) {
                th.loading = false;
                if (d.data.success) {
                    th.datas = d.data.result;
                    th.totalpages = Number(d.data.totalpages);
                    th.total = d.data.total;

                } else {
                    console.error(d.data.exception);
                    throw d.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                console.error(err);
            });
        },
        //显示视频详情
        showdrawer: function (file) {
            this.drawer = true;
            this.currfile = file;
            var th = this;
            window.setTimeout(function () {
                th.createplayer(th.currfile.fullname);
            }, 1000);
            return false;
        },
        //隐藏视频详情
        hidedrawer: function () {
            this.drawer = false;
            var th = this;
            if (window.drawer_player != null) window.drawer_player.destroy();
            window.setTimeout(function () {
                if (!th.drawer)
                    if (window.drawer_player != null) window.drawer_player.destroy();
            }, 500);
        },
        //创建播放器
        createplayer: function (urlVideo) {
            if (window.drawer_player != null) window.drawer_player.destroy();
            window.drawer_player = new QPlayer({
                url: urlVideo,
                container: document.getElementById("file_drawer_player"),
                autoplay: true,
            });
            return window.drawer_player;
        },
        //选择视频文件
        selected: function (file) {
            this.$confirm('是否选择文件 “' + file.name + '” ?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                this.$emit('selected', file);
            }).catch(() => { });
        }
    },
    template: `<el-dialog :visible.sync="showpanel" append-to-body custom-class="video_list_dialog"
     :show-close="true" :close-on-click-modal="false">
        <div slot="title" class="video_list_title">
            <div><icon>&#xe761</icon> 请选择视频文件</div>
            <el-input v-model="query.search" placeholder="查询" clearable 
             @keyup.enter.native="handleCurrentChange(1)" @clear="handleCurrentChange(1)"
             :disabled="drawer || loading">  
             <i slot="prefix" class="el-input__icon el-icon-search" @click="handleCurrentChange(1)"></i>     
            </el-input>
        </div>
        <loading v-if="loading">正在加载....</loading>
        <template v-else>
            <dl v-if="datas.length>0">
                <dd v-for="(file,i) in datas">
                    <div class="order">{{query.size*(query.index-1)+i+1}}</div>
                    <div class="filename">
                         <a @click="showdrawer(file)"> {{file.name}}</a>                   
                    </div>
                    <div class="size">{{file.size|size}}</div>
                    <div class="btn">        
                        <el-link type="primary" @click="selected(file)">选择</el-link>
                    </div>
                </dd>
            </dl>
            <template v-if="datas.length<1">没有满足条件的数据</template>
        </template>
        <div id="file_drawer" :class="{'file_drawer':true,'show':drawer}"  remark="视频文件的详情" @click.self="hidedrawer()">
            <div>
                <div class="file_drawer_title">{{currfile.name}}</div>
                <div class="file_drawer_body" id="file_drawer_player"></div>
                <div class="file_drawer_footer">
                    <el-button type="primary" @click="selected(currfile)">
                        <icon>&#xa048</icon>选择该视频文件
                    </el-button>
                </div>
            </div>
        </div>
        <div slot="footer">
            <el-pagination v-on:current-change="handleCurrentChange" :disabled="drawer || loading" :current-page="query.index" :page-sizes="[1]"
                :page-size="query.size" :pager-count="10" layout="total, prev, pager, next, jumper" :total="total">
            </el-pagination>
        </div>
    </el-dialog> `
});