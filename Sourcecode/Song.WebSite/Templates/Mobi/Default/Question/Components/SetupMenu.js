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
        this.$dialog.confirm({
          title: '更新试题',
          message: '将试题保持与服务器端同步,\n更新过程中不影响做题',
          allowHtml: true
        }).then(function () {
          parent.setup_show = false;
          parent.starttime = new Date()
          parent.getQuesSimplify(true);
          parent.updateQues();
        }).catch(function () { });
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
        this.$dialog.confirm({
          title: '重新练习',
          message: msg,
        }).then(function () {
          parent.setup_show = false;
          parent.state.clear(true);
        }).catch(function () { });
      }
    },
    //记录设置字体的大小，num为增减数字，例如-1
    setFont: function (num) {
      if (num == null || num == '') num = 0;
      let min = -4, max = 10;
      let init = $api.storage(this.fontsizekey);
      init = init == null || init == '' ? 0 : Number(init);
      let val = num == 0 ? 0 : init + num;
      if (val < min || val > max) return;
      this.$parent.fontsize = val;
      $api.storage(this.fontsizekey, val);
    },
    //清除所有错题记录
    clearErrors: function () {
      var th = this;
      var couid = $api.querystring("couid", 0);
      var acid = th.account.Ac_ID;
      this.$dialog.confirm({
        title: '清空错题',
        message: '清除当前课程的所有错题',
      }).then(function () {
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
  template: `<van-popup v-model="show" class="setup_show" overlay-class="overlay" position="right" remark="右上角菜单项">
        <van-cell-group>
          <van-cell class="view_model">
            <!-- 使用 title 插槽来自定义标题 -->
            <template #title>
            <div v-for="item in views" :current="view_model==item.val"  @click="view_model=item.val" >
                <icon v-html="item.icon" noview></icon>{{item.name}}
            </div> 
            </template>
          </van-cell>

          <van-cell>
            <template #title>
              <icon>&#xe657</icon>字体
              <span class="fontsize">
                <span @click="setFont(-1)"><icon>&#xe600</icon>缩小</span>
                <span @click="setFont(1)"><icon>&#xe6ea</icon>放大</span>
              </span>
            </template>
          </van-cell>
          <van-cell>
            <template #title>
            <span class="fontsize">
              <span @click="setFont(0)"><icon>&#xe667</icon>默认大小</span>            
            </span>
            </template>
          </van-cell>
          <van-cell>
            <template #title>
              <span @click="updateQues"><icon>&#xe694</icon>更新试题</span>
            </template>
          </van-cell>
          <van-cell>
            <template #title>
              <span @click="clearlog"><icon>&#xe737</icon>重新练习</span>
            </template>
          </van-cell>
          <van-cell v-for="menu in clearmenus" v-if="menu.val==page">
            <template #title>
              <span @click="menu.event()"><icon v-html="menu.icon"></icon>{{menu.name}}</span>
            </template>
          </van-cell>         
        </van-cell-group>
      </van-popup>`
});
