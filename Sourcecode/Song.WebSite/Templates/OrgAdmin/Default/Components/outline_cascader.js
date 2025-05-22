//章节的级联选择器
$dom.load.css([$dom.path() + 'Components/Styles/outline_cascader.css']);
Vue.component('outline_cascader', {
    //olid:当前章节id
    props: ['olid', 'couid', 'disabled'],
    data: function () {
        return {
            //专业树形下拉选择器的配置项
            defaultSubjectProps: {
                children: 'children',
                label: 'Ol_Name',
                value: 'Ol_ID',
                expandTrigger: 'hover',
                checkStrictly: true
            },
            treedatas: [],
            ids: [],
            loading: false       //预载
        }
    },
    watch: {
        'couid': {
            handler: function (nv, ov) {
                if (nv != null && (nv != ov))
                    this.getOutlines(nv);
            }, immediate: true
        },
        'olid': {
            handler: function (nv, ov) {

            }, immediate: true
        },
    },
    created: function () {
        var th = this;
        if (th.olid == '') return;
        window.clac_sbjids_setInterval = window.setInterval(function () {
            if (th.olid != null && th.treedatas.length > 0) {
                th.ids = th.clac_ids();
                th.$nextTick(function () {
                    th.evetChange(th.ids);
                });
                clearInterval(window.clac_sbjids_setInterval);
            }
        }, 100);
    },
    methods: {
        //获取课程章节的数据
        getOutlines: function () {
            var th = this;
            var form = { couid: th.couid, isuse: true };
            $api.cache('Outline/Tree', form).then(function (req) {
                if (req.data.success) {
                    th.treedatas = req.data.result;
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });
        },
        //计算当前选中项
        clac_ids: function () {
            var th = this;
            //将当前课程的专业，在控件中显示
            var arr = [];
            arr.push(th.olid);
            var olid = th.traversalQuery(th.olid, th.treedatas);
            if (olid == null) {
                throw '专业“' + th.olid + '”不存在，或该专业被禁用';
            }
            arr = th.getParentPath(sbj, th.treedatas, arr);
            return arr;;
        },
        //获取当前专业的上级路径
        getParentPath: function (entity, datas, arr) {
            if (entity == null) return null;
            var obj = this.traversalQuery(entity.Ol_PID, datas);
            if (obj == null) return arr;
            arr.splice(0, 0, obj.Ol_ID);
            arr = this.getParentPath(obj, datas, arr);
            return arr;
        },
        //从树中遍历对象
        traversalQuery: function (olid, datas) {
            var obj = null;
            for (let i = 0; i < datas.length; i++) {
                const d = datas[i];
                if (d.Ol_ID == olid) {
                    obj = d;
                    break;
                }
                if (d.children && d.children.length > 0) {
                    obj = this.traversalQuery(olid, d.children);
                    if (obj != null) break;
                }
            }
            return obj;
        },
        //选择变化后的事件
        evetChange: function (val) {
            var currid = -1;
            if (val.length > 0) currid = val[val.length - 1];
            this.$emit('change', currid, val);
        }
    },
    template: `<div class="sbj_cascader">
        <el-cascader style="width: 100%;" clearable v-model="ids" placeholder="请选择课程章节" :disabled="disabled"
            :options="treedatas" separator="／" :props="defaultSubjectProps" filterable @change="evetChange">
            <template slot-scope="{ node, data }">
                <span>{{ data.Ol_Name }}</span>              
            </template>
        </el-cascader>      
        </div>`
});