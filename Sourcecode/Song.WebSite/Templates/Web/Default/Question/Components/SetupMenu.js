//右上角的下拉菜单
$dom.load.css([$dom.pagepath() + 'Components/Styles/setupmenu.css']);
Vue.component('setupmenu', {
  props: ['account'],
  data: function () {
    return {
      //是否显示菜单
      show: false,
      fontsizekey: 'Exercise_Font_Size',       //用于存储字体大小的值
      //视图模式，night:夜晚模式，day:白天模式，cosy：护眼模式
      view_model: 'day',
      views: [{ name: '日常模式', icon: '&#xe729', val: 'day' },
      { name: '夜间模式', icon: '&#xa032', val: 'night' },
      { name: '护眼模式', icon: '&#xa03a', val: 'cosy' }],
      //清空各项内容的按钮
      clearmenus: [
        { name: '清空错题', icon: '&#xe800', val: 'error', event: this.clearErrors },
        { name: '清空笔记', icon: '&#xe800', val: 'notes', event: this.clearNotes },
        { name: '清空收藏', icon: '&#xe800', val: 'collects', event: this.clearCollects }]
    }
  },
  watch: {
    //视图模式变化时
    'view_model': {
      handler(nv, ov) {
        $dom("#vapp").attr('view', nv);
      },
      immediate: true
    },
  },
  computed: {
    //页面名称，不带后缀名
    'page': function () {
      return $dom.file().toLowerCase();
    }
  },
  mounted: function () {
      //刷新页面即重置字体大小
      this.setFont();
  },
  methods: {
    //更新试题
    updateQues: function () {
      var parent = this.$parent;
      if (parent != null) {
        this.$confirm('将试题保持与服务器端同步?', '更新试题', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          parent.setup_show = false;
          parent.starttime = new Date()
          parent.getQuesSimplify(true);
          parent.updateQues();
        }).catch(() => {

        });
      }
    },
    //清除学习记录
    clearlog: function () {
      //取章节id
      var olid = $api.querystring("olid", 0);
      var msg = '清除当前章节下的学习记录';
      if (olid == 0) msg = '清除当前练习的学习记录';
      var parent = this.$parent;
      if (parent != null) {
        this.$confirm(msg, '重新练习', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          parent.setup_show = false;
          parent.state.clear(true);
        }).catch(() => { });
      }
    },
    //设置字体大小，默认16px，num为增减数字，例如-1
    setFont: function (num) {
      let min = -4, max = 10;
      let init = Number($api.storage(this.fontsizekey));
      init = isNaN(init) ? 0 : init;
      let val = num == null ? init : (num == 0 ? num : init + num);
      if (val < min || val > max) return;
      this.$parent.fontsize = val;
      $api.storage(this.fontsizekey, val);
    },
    //清除所有错题记录
    clearErrors: function () {
      var couid = $api.querystring("couid", 0);
      var acid = this.account.Ac_ID;
      this.$confirm('清除当前课程的所有错题, 是否继续?', '清空错题', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        var th = this;
        $api.delete('Question/ErrorClear', { 'acid': acid, 'couid': couid }).then(function (req) {
          if (req.data.success) {
            let form = { 'acid': acid, 'couid': couid, 'type': '' };
            $api.cache('Question/ErrorQues:update', form).then(function (req) {
            }).finally(() => {
              th.$parent.state.clear(true);
            });
          } else {
            console.error(req.data.exception);
            throw req.data.message;
          }
        }).catch(function (err) {
          alert(err);
          console.error(err);
        });
      }).catch(() => { });
    },
    //清空笔记
    clearNotes: function () {
      var couid = $api.querystring("couid", 0);
      var acid = this.account.Ac_ID;
      var th=this;
      this.$confirm('清除当前课程所有试题笔记, 是否继续?', '清空笔记', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(function () {      
        $api.delete('Question/NotesClear', { 'acid': acid, 'couid': couid }).then(function (req) {
        }).catch((err) => console.error(err)).finally(() => {
          th.$parent.state.clear(true);
        });
      }).catch(function () { });
    },
    //清空收藏
    clearCollects: function () {
      var couid = $api.querystring("couid", 0);
      var acid = this.account.Ac_ID;
      var th = this;
      this.$confirm('清除当前课程的所有收藏, 是否继续?', '清空收藏', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(function () {
        $api.delete('Question/CollectClear', { 'acid': acid, 'couid': couid }).then(function (req) {
        }).catch((err) => console.error(err)).finally(() => {
          th.$parent.state.clear(true);
        });
      }).catch(function () { });
    },
  },
  template: `<el-drawer :visible.sync="show" :size="260" class="setup_show"  direction="rtl" remark="右上角菜单项" noview append-to-body>
          <span slot="title">
            <icon>&#xa038</icon>设置项
          </span>         
          <el-divider></el-divider>      
            <div v-for="item in views" class="view_model" :current="view_model==item.val"  @click="view_model=item.val" >
                <icon v-html="item.icon" noview></icon>{{item.name}}
            </div>            
            <el-divider></el-divider>          
          <el-row>          
              <span @click="updateQues"><icon>&#xe694</icon>更新试题</span>          
          </el-row>
          <el-row>           
              <span @click="clearlog"><icon>&#xe737</icon>重新练习</span>           
          </el-row>
          <el-row v-for="menu in clearmenus" v-if="menu.val==page">          
              <span @click="menu.event()"><icon v-html="menu.icon"></icon>{{menu.name}}</span>         
          </el-row>    
          <el-divider></el-divider>    
          <el-row class="font">          
              <icon>&#xe657</icon>字体
              <span class="font_btn">
                <span @click="setFont(-1)"><icon>&#xe600</icon>缩小</span>
                <span @click="setFont(1)"><icon>&#xe6ea</icon>放大</span>
              </span> 
          </el-row>    
          <el-row class="font">  
            <span class="fontsize">
              <span @click="setFont(0)"><icon>&#xe667</icon>默认大小</span>            
            </span>    
          </el-row>
      </el-drawer>`
});
