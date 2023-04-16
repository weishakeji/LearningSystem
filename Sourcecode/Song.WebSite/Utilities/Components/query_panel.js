$dom.load.css(['/Utilities/Components/Styles/query_panel.css']);
//查询面板
//事件search:触发查询
Vue.component('query_panel', {
    //model:表单绑定的数据对象
    //rules: 表单校验的方法
    //expand: 是否展开
    //more: 显示更多查询的按钮
    //mask: 展开查询面板时，是否显示背景遮罩
    //width:面板宽度
    //loading: 预载
    props: ['model', 'rules', 'expand', 'more', 'mask', 'width', 'loading'],
    data: function () {
        return {
        }
    },
    watch: {
        expanded: function (nv, ov) {
            console.log('expanded:' + nv);
        }
    },
    computed: {
        //当前状态是否是展开的,默认不展开
        expanded: function () {
            if ($api.getType(this.expand) != 'Boolean') return false;
            return $api.isnull(this.expand) ? false : this.expand;
        },
        //更多查询的按钮是否显示,默认不显示
        showmore: function () {
            if ($api.getType(this.more) != 'Boolean') return false;
            return $api.isnull(this.more) ? false : this.more;
        },
        //是否显示背景遮罩，默认为显示
        showmask: function () {
            if ($api.getType(this.mask) != 'Boolean') return true;
            return $api.isnull(this.mask) ? true : this.mask;
        },
        //宽度值
        'width_val': function () {
            if (!this.expanded) return 'auto';
            let def = 50;
            var val = $api.isnull(this.width) ? def : this.width;
            if ($api.getType(val) == 'Number')
                return val + (val > 100 ? 'px' : '%');
            if ($api.getType(val) == 'String') return val;
            return def + '%';
        }
    },
    created: function () {

    },
    methods: {
        onserch: function () {
            //this.expand = false;
            this.$emit('search', this.model);
        }
    },
    template: `<div :class="{'query_panel':true,'query_panel_expand':expanded}">
        <div class="query_panel_mask" v-if="showmask && expanded" @click="expand=false"></div>
       
        <el-form :inline="!expanded" :model="model" :style="{'width':width_val}" v-on:submit.native.prevent  label-width="80px">
            <slot></slot>
            <el-form-item v-show="!expanded">
                <el-button-group>
                    <el-button type="primary" v-on:click="onserch()" :loading="loading"
                        native-type="submit" plain  class="el-icon-search">
                        查询
                    </el-button>
                    <el-tooltip effect="light" content="更多查询条件" placement="bottom">
                        <el-button type="primary" v-if="showmore" plain @click="expand=true" >
                            <icon>&#xe838</icon>
                        </el-button>
                    </el-tooltip>                   
                </el-button-group>               
            </el-form-item>
            <slot name="full" v-if="expanded"></slot>
            <el-form-item label="" class="search_btns" v-show="expanded">
                <el-button type="primary"  v-on:click="onserch()" :loading="loading" class="el-icon-search">
                    查询
                </el-button>
                <el-button type="primary" @click="expand=false" native-type="submit" plain class="el-icon-view">
                    隐藏面板
                </el-button>
            </el-form-item>
        </el-form>
    </div>`
});
