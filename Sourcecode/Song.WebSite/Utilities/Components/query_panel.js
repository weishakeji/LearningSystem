
//查询面板
//事件search:触发查询,如果引用组件时不添加这个事件，查询按钮也不会显示出来
Vue.component('query_panel', {
    //model:表单绑定的数据对象
    //rules: 表单校验的方法 
    //mask: 展开查询面板时，是否显示背景遮罩
    //width:面板宽度
    //loading: 预载
    props: ['model', 'rules', 'mask', 'width', 'loading', 'disabled'],
    data: function () {
        return {
            //是否显示面板
            visible: false,
            model_init: null
        }
    },
    watch: {
        //当第一次加载时，记录表单数据
        'model': {
            handler: function (nv, ov) {
                if (nv != null && this.model_init == null)
                    this.model_init = $api.clone(nv);
            }, immediate: true
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
            return this.$slots['more'] && this.$slots['more'].length > 0;
        },
        //是否显示背景遮罩，默认为显示
        'showmask': function () {
            if ($api.getType(this.mask) != 'Boolean') return true;
            return $api.isnull(this.mask) ? true : this.mask;
        },
        //是否显示查询按钮
        'showbutton': function () {
            if (this.$listeners && this.$listeners['search'] == undefined) return false;
            //if (this.showmore) return true;
            return this.$slots['default'] && this.$slots['default'].length > 0;
        },
        //宽度值，表单区域的宽度值
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
                    var allow = this.$listeners && this.$listeners['search'] != undefined;
                    if (!allow) return alert('未设置查询事件');
                    this.visible = false;
                    this.$emit('search', this.model);
                } else {
                    console.log('error');
                    return false;
                }
            });
        },
        //重置表单
        reset_model: function () {
            var model = this.model_init;
            for (var m in model)
                this.$set(this.model, m, model[m]);
        }
    },
    template: `<div :class="{'query_panel':true,'query_panel_expand':expanded}">
        <div :class="{'query_panel_mask':showmask && expanded}"  @click="visible=false"></div>    
        <el-form ref="form" :rules="rules" :disabled="disabled" :inline="!expanded" :model="model"
            :style="{'width':width_val}" v-on:submit.native.prevent  label-width="80px">
            <slot></slot>
            <el-form-item v-show="!expanded && (showbutton || showmore)">
                <el-button-group>
                    <el-button type="primary" v-if="showbutton" v-on:click="onserch()" :loading="loading"
                        native-type="submit" plain  class="el-icon-search">
                        查询
                    </el-button>
                    <el-tooltip effect="light" content="更多查询条件" placement="bottom">
                        <el-button type="primary" v-if="showbutton && showmore" plain @click="visible=true" >
                            <icon>&#xe838</icon>                          
                        </el-button>
                        <el-button type="primary" v-else-if="showmore" @click="visible=true" plain class="el-icon-search">
                            查询
                        </el-button>
                    </el-tooltip>                   
                </el-button-group>               
            </el-form-item>
            <slot name="more" v-if="expanded"></slot>
            <el-form-item label="" class="search_btns" v-show="expanded">
                <el-button type="primary"  v-on:click="onserch()" plain :loading="loading" class="el-icon-search">
                    查 询
                </el-button>
                <el-button type="info" @click="reset_model()" v-if="model_init" plain class="el-icon-refresh-left">
                    重 置
                </el-button>
            </el-form-item>
        </el-form>
    </div>`
});
$dom.load.css(['/Utilities/Components/Styles/query_panel.css']);