//右上角的下拉菜单
$dom.load.css([$dom.pagepath() + 'Components/Styles/setupmenu.css']);
Vue.component('setupmenu', {
  props: ['account'],
  data: function () {
    return {
      //是否显示菜单
      show: false,
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
          parent.getQuestion(true);
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
      var size = 16;
      if (num == null) num == 0;
      ergodic($dom("section.question"), num);
      function ergodic(dom, num) {
        var fontsize = parseInt(dom.css("font-size"));
        fontsize = isNaN(fontsize) ? size : fontsize + num;
        dom.css("font-size", fontsize + "px", true);
        var child = dom.childs();
        if (child.length < 1) return;
        child.each(function (node) {
          var n = $dom(this);
          if (n.attr('no-font-size') != null) return true;
          ergodic(n, num);
        });
      }
    },
    //清除所有错题记录
    clearErrors: function () {
      var couid = $api.querystring("couid", 0);
      var acid = this.account.Ac_ID;
      this.$dialog.confirm({
        title: '清空错题',
        message: '清除当前课程的所有错题',
      }).then(function () {
        $api.delete('Question/ErrorClear', { 'acid': acid, 'couid': couid }).then(function (req) {
          if (req.data.success) {
            window.location.reload();
          } else {
            console.error(req.data.exception);
            throw req.data.message;
          }
        }).catch(function (err) {
          alert(err);
          console.error(err);
        });

      }).catch(function () { });
    },
    //清空笔记
    clearNotes: function () {
      var couid = $api.querystring("couid", 0);
      var acid = this.account.Ac_ID;
      this.$dialog.confirm({
        title: '清空笔记',
        message: '清除当前课程的所有笔记',
      }).then(function () {
        $api.delete('Question/NotesClear', { 'acid': acid, 'couid': couid }).then(function (req) {
          if (req.data.success) {
            window.location.reload();
          } else {
            console.error(req.data.exception);
            throw req.data.message;
          }
        }).catch(function (err) {
          alert(err);
          console.error(err);
        });

      }).catch(function () { });
    },
    //清空收藏
    clearCollects: function () {
      var couid = $api.querystring("couid", 0);
      var acid = this.account.Ac_ID;
      this.$dialog.confirm({
        title: '清空收藏',
        message: '清除当前课程的所有收藏',
      }).then(function () {
        $api.delete('Question/CollectClear', { 'acid': acid, 'couid': couid }).then(function (req) {
          if (req.data.success) {
            window.location.reload();
          } else {
            console.error(req.data.exception);
            throw req.data.message;
          }
        }).catch(function (err) {
          alert(err);
          console.error(err);
        });

      }).catch(function () { });
    },
  },
  template: `<el-drawer :visible.sync="show" class="setup_show"  direction="rtl" remark="右上角菜单项" noview append-to-body>
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
      </el-drawer>`
});
