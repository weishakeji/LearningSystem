$dom.load.css([$dom.pagepath() + 'Components/Styles/outlinelist.css']);
//章节列表
Vue.component('outlinelist', {
  props: ["outlines", "course", "acid"],
  data: function () {
    return {}
  },
  watch: {},
  computed: {},
  mounted: function () {

  },
  methods: {},
  template: `<div v-if="outlines && outlines.length>0" class="outlinelist">
      <outline_row v-for="(o,i) in outlines" :outline="o" :course="course" :acid="acid"></outlines>
  </div>`
});

//章节显示的行
Vue.component('outline_row', {
  props: ["outline", "course", "acid"],
  data: function () {
    return {
      state: null,
      count: {},       //练习记录的统计数据
      loading: false
    }
  },
  watch: {
    "outline": {
      deep: false, immediate: true,
      handler: function (nv, ov) {
        //创建试题练习状态的记录的操作对象
        if (this.state == null) {
          this.state = $state.create(this.acid, this.course.Cou_ID, nv.Ol_ID, "Exercises");
          //console.log(this.state);
          this.getprogress();
          this.getques_count();
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
    //获取章节的试题数量
    getques_count: function () {
      var th = this;
      var couid = this.course.Cou_ID;
      var olid = this.outline.Ol_ID;
      $api.get('Question/Count', { 'orgid': -1, 'sbjid': -1, 'couid': couid, 'olid': olid, 'type': '', 'use': true }).then(function (req) {
        if (req.data.success) {
          var result = req.data.result;
          th.outline.Ol_QuesCount = result;
        } else {
          console.error(req.data.exception);
          throw req.config.way + ' ' + req.data.message;
        }
      }).catch(function (err) {
        //alert(err);
        Vue.prototype.$alert(err);
        console.error(err);
      });
    },
    //跳转到学习页
    gourl: function () {
      var url = '/web/course/study.258?olid=3406';
      url = $api.url.dot(this.outline.Cou_ID, url);
      url = $api.url.set(url, { 'olid': this.outline.Ol_ID });
      return url;
    }
  },
  //
  template: `<div class="outline_row">
    <div>
      <span v-html="outline.serial"></span>  
      <el-tag type="success" v-if="count.rate>0">{{count.rate}}%</el-tag>  
      <a class="olname" v-html="outline.Ol_Name" :href="gourl()" target="_blank"></a>
      <el-tag type="danger" v-if="!outline.Ol_IsFinish">未完结</el-tag>
    </div>
    <div class="tag">
          <el-tag plain type="primary">{{count.answer}}/{{outline.Ol_QuesCount}}
          </el-tag>       
    </div> 
  <outlinelist ref="outlines" :outlines="outline.children" :course="course" :acid="acid"></outlinelist>
</div>`
});