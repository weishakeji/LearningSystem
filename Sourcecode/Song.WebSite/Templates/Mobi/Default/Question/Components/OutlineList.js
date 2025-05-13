$dom.load.css([$dom.pagepath() + 'Components/Styles/outlinelist.css']);
//章节列表
Vue.component('outlinelist', {
  props: ["outlines", "course", "acid", "isbuy", "showalloutline"],
  data: function () {
    return {

    }
  },
  watch: {},
  computed: {},
  mounted: function () {

  },
  methods: {},
  template: `<div v-if="outlines && outlines.length>0" class="outlinelist">
      <outline_row v-for="(o,i) in outlines" :outline="o" :course="course" :acid="acid"
       :isbuy="isbuy" :showalloutline="showalloutline"></outlines>
  </div>`
});

//章节显示的行
Vue.component('outline_row', {
  props: ["outline", "course", "acid", "isbuy", "showalloutline"],
  data: function () {
    return {
      state: null,
      count: {},       //练习记录的统计数据
      preload: false,   //试题预加载是否完成
      loading: false
    }
  },
  watch: {
    "outline": {
      deep: false, immediate: true,
      handler: function (nv, ov) {
        //创建试题练习状态的记录的操作对象
        if (this.state == null) {
          this.state = $state.create(this.acid, this.course.Cou_ID, nv.Ol_ID, "exercise");
          this.getprogress();
          this.getquestions(nv);
        }
      }
    },
  },
  computed: {
    //是否初始化过，例如outline是否为空
    init: function () {
      return JSON.stringify(this.outline) != '{}' && this.outline != null;
    },
  },
  mounted: function () { },
  methods: {
    //获取练习进度
    getprogress: function () {
      var th = this;
      var olid = th.outline.Ol_ID;
      //获取练习记录
      this.state.restore().then(function (d) {
        th.outline.count = th.count = d.count;
        th.state = d;
      }).catch(function (d) {
        th.outline.count = th.count = d.count;
        th.state = d;
      }).finally(function () {
        var parent = window.vapp;
        th.state['olid'] = olid;
        parent.statepush(th.state);
      });
    },
    //跳转
    goExercises: function () {
      var outline = this.outline;
      var course = this.course;
      //是否可以练习
      if ((course.Cou_IsTry && outline.Ol_IsFree) || this.isbuy || course.Cou_IsFree || course.Cou_IsLimitFree) {
        if (outline.Ol_QuesCount < 1) {
          alert('当前章节没有试题');
          return;
        }
        var couid = $api.url.get(null, 'couid');
        var uri = $api.url.set('exercise', {
          'path': outline.Ol_XPath,
          'couid': couid,
          'olid': outline.Ol_ID,
        });
        window.navigateTo(uri);
      } else {
        var link = window.location.href;
        link = link.substring(link.indexOf(window.location.pathname));
        var url = $api.url.set('/mobi/course/buy', {
          'couid': outline.Cou_ID,
          'olid': outline.Ol_ID,
          'link': encodeURIComponent(link)
        });
        window.navigateTo(url);
      }
    },
    //获取试题，用于练习之前的预载
    getquestions: function (outline) {
      if (outline.Ol_QuesCount < 1) return;
      var th = this;
      var form = { 'couid': outline.Cou_ID, 'olid': outline.Ol_ID, 'type': -1, 'diff': -1, 'count': 0 };
      $api.cache('Question/Simplify:' + (60 * 24 * 30), form).then(function (req) {
        if (req.data.success) {
          var result = req.data.result;
          th.preload = true;
        } else {
          console.error(req.data.exception);
          throw req.data.message;
        }
      }).catch(err => console.error(err));
    }
  },
  //
  template: `<div class="outline_row">
  <van-cell @click="goExercises()" class="outline" v-show="showalloutline || outline.Ol_QuesCount>0">
    <div>
      <span v-html="outline.serial"></span>
      <van-circle :rate="count.rate" v-model="count.rate" size="25px" layer-color="#ebedf0" :stroke-width="60"
        :text="count.rate<100 ? Math.round(count.rate) : '✔'" v-if="count.rate>0"></van-circle>
      <span class="olname" v-html="outline.Ol_Name" :preload="preload"></span>
      <van-tag type="danger" v-if="!outline.Ol_IsFinish">未完结</van-tag>
    </div>
    <div class="tag" v-if="outline.Ol_QuesCount>0">     
        <van-tag v-if="isbuy || course.Cou_IsFree || course.Cou_IsLimitFree || outline.Ol_IsFree" 
        plain type="primary">{{count.answer}}/{{outline.Ol_QuesCount}}  </van-tag>     
        <van-tag v-else-if="course.Cou_IsTry && outline.Ol_IsFree" type="success">免费</van-tag>
        <van-tag v-else type="warning" plain>购买 </van-tag>
    </div>
  </van-cell>
  <template #left>
    <van-button square type="info" text="正确率">正确率：{{count.rate}}%</van-button>
  </template>
  <template #right>
    <van-button square type="primary" text="答对">答对：{{count.correct}}</van-button>
    <van-button square type="warning" text="答错">答错：{{count.wrong}}</van-button>
  </template>
  <outlinelist ref="outlines" :outlines="outline.children" :course="course" :acid="acid" :isbuy="isbuy"  :showalloutline="showalloutline"></outlinelist>
</div>`
});