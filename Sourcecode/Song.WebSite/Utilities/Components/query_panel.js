$dom.load.css(['/Utilities/Components/Styles/query_panel.css']);
//查询面板
//事件search:触发查询
Vue.component('query_panel', {
    //model:表单绑定的数据对象
    //rules: 表单校验的方法
    //expand: 是否展开 
    //mask: 展开查询面板时，是否显示背景遮罩
    //width:面板宽度
    //loading: 预载
    props: ['model', 'rules', 'expand', 'mask', 'width', 'loading'],
    data: function () {
        return {
            visible: false
        }
    },
    watch: {
        expand: function (nv, ov) {
            this.visible = nv;          
        }
    },
    computed: {
        //当前状态是否是展开的,默认不展开
        'expanded': function () {
            if ($api.getType(this.visible) != 'Boolean') return false;
            return $api.isnull(this.visible) ? false : this.visible;
        },
        //显示“更多”按钮，如果更多查询的插槽中有内容，则显示
        'showmore': function () {
            var slots = this.$slots
            return slots['full'] && slots['full'].length > 0;
        },
        //是否显示背景遮罩，默认为显示
        'showmask': function () {
            if ($api.getType(this.mask) != 'Boolean') return true;
            return $api.isnull(this.mask) ? true : this.mask;
        },
        //是否显示查询按钮
        'showbutton': function () {
            var slots = this.$slots
            return slots['default'] && slots['default'].length > 0;
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
    mounted: function () {
    },
    methods: {
        //查询事件
        onserch: function () {
            this.$refs['form'].validate((valid) => {
                if (valid) {
                    this.visible = false;
                    this.$emit('search', this.model);
                } else {
                    console.log('error');
                    return false;
                }
            });
        }
    },
    template: `<div :class="{'query_panel':true,'query_panel_expand':expanded}">
        <div :class="{'query_panel_mask':showmask && expanded}"  @click="visible=false"></div>    
        <el-form ref="form" :rules="rules" :inline="!expanded" :model="model" :style="{'width':width_val}" v-on:submit.native.prevent  label-width="80px">
            <slot></slot>
            <el-form-item v-show="!expanded && showbutton">
                <el-button-group>
                    <el-button type="primary" v-on:click="onserch()" :loading="loading"
                        native-type="submit" plain  class="el-icon-search">
                        查询
                    </el-button>
                    <el-tooltip effect="light" content="更多查询条件" placement="bottom">
                        <el-button type="primary" v-if="showmore" plain @click="visible=true" >
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
                <el-button type="primary" @click="visible=false" native-type="submit" plain class="el-icon-view">
                    隐藏面板
                </el-button>
            </el-form-item>
        </el-form>
    </div>`
});
