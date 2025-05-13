$dom.load.css([$dom.pagepath() + 'Components/Styles/outlinelist.css']);
//章节列表
Vue.component('outlinelist', {
  props: ["outlines", "course", "acid", "isbuy", "showalloutline"],
  data: function () {
    return {}
  },
  watch: {},
  computed: {},
  mounted: function () {

  },
  methods: {},
  template: `<div v-if="outlines && outlines.length>0" class="outlinelist">
      <outline_row v-for="(o,i) in outlines" :outline="o" :course="course"
       :acid="acid" :isbuy="isbuy" :showalloutline="showalloutline"></outlines>
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
          this.state = $state.create(this.acid, this.course.Cou_ID, nv.Ol_ID, "Exercise");
          //console.log(this.state);
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
    //获取试题，用于练习之前的预载
    getquestions: function (outline) {
      if (outline.Ol_QuesCount < 1) return;
      var th = this;
      let para = { 'couid': outline.Cou_ID, 'olid': outline.Ol_ID, 'type': -1, 'count': 0 };
      $api.cache('Question/Simplify:' + (60 * 24 * 30), para).then(function (req) {
        if (req.data.success) {
          var result = req.data.result;
          th.preload = true;
        } else {
          console.error(req.data.exception);
          throw req.data.message;
        }
      }).catch(function (err) {
      });
    },
    //生成跳转到学习页的网址
    gourl: function () {
      var outline = this.outline;
      if (outline.Ol_QuesCount < 1) return '#';
      if (this.isbuy || this.course.Cou_IsFree || this.course.Cou_IsLimitFree || outline.Ol_IsFree
        || (this.course.Cou_IsTry && outline.Ol_IsFree)) {
        var url = '/web/question/Exercise';
        return $api.url.set(url, {
          'olid': outline.Ol_ID,
          'couid': outline.Cou_ID,
          'back': true
        });
      } else {
        var link = window.location.href;
        link = link.substring(link.indexOf(window.location.pathname));
        return $api.url.set('/mobi/course/buy', {
          'couid': outline.Cou_ID,
          'olid': outline.Ol_ID,
          'link': encodeURIComponent(link)
        });
      }
    },
    //链接打开方式
    target: function () {
      var outline = this.outline;
      if (this.isbuy || this.course.Cou_IsFree || this.course.Cou_IsLimitFree || outline.Ol_IsFree
        || (this.course.Cou_IsTry && outline.Ol_IsFree)) return '_self';
      return '_blank'
    }
  },
  //
  template: `<div class="outline_row" v-show="showalloutline || outline.Ol_QuesCount>0">
    <div>
      <span v-html="outline.serial"></span>  
      <el-tag type="success" v-if="count.rate>0">{{count.rate}}%</el-tag>  
      <a class="olname" :preload="preload" v-html="outline.Ol_Name"></a>
      <el-tag type="danger" v-if="!outline.Ol_IsFinish">未完结</el-tag>
    </div>
    <a class="tag">
        <el-tag v-if="isbuy || course.Cou_IsFree || course.Cou_IsLimitFree || outline.Ol_IsFree" 
        plain type="primary">{{count.answer}}/{{outline.Ol_QuesCount}} </el-tag>     
        <el-tag v-else-if="course.Cou_IsTry && outline.Ol_IsFree" type="success">免费</el-tag>
        <el-tag v-else type="warning" plain>购买 </el-tag>            
    </a> 
    <outlinelist ref="outlines" :outlines="outline.children" :course="course" :acid="acid" :isbuy="isbuy"  :showalloutline="showalloutline"></outlinelist>
  </div>`
});