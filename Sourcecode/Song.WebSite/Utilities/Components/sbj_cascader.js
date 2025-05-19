//专业的级联选择器
$dom.load.css(['/Utilities/Components/Styles/sbj_cascader.css']);
//事件：
//change:选项变量时,参数:当前专业id,当前专业id的路径数组
//load:加载数据完成，参数:专业数据
Vue.component('sbj_cascader', {
    //sbjid:当前专业id
    //disabled:是否禁用
    //showitem: 是否显示course课程数，ques试题数，test试卷数,child子级专业数
    props: ['sbjid', 'orgid', 'disabled', 'showitem'],
    data: function () {
        return {
            //专业树形下拉选择器的配置项
            defaultSubjectProps: {
                children: 'children',
                label: 'Sbj_Name',
                value: 'Sbj_ID',
                expandTrigger: 'hover',
                checkStrictly: true
            },
            subjects: [],
            sbjids: [],
            loading: false       //预载
        }
    },
    computed: {
        //专业数据是否为空
        isnull: function () {
            return $api.isnull(this.subjects) || this.subjects.length == 0;
        }
    },
    watch: {
        'orgid': {
            handler: function (nv, ov) {
                if (nv != null && nv != '' && (nv != ov))
                    this.getSubjects(nv);
            }, immediate: true
        },
        'sbjid': {
            handler: function (nv, ov) {
                //this.sbjids = this.clac_sbjids(nv);
            }, immediate: true
        },
    },
    created: function () {
        var th = this;
        window.clac_sbjids_setInterval = window.setInterval(function () {
            if (th.sbjid != null && th.sbjid != 0 && !th.isnull) {
                th.sbjids = th.clac_sbjids();
                th.$nextTick(function () {
                    th.evetChange(th.sbjids);
                });
                clearInterval(window.clac_sbjids_setInterval);
            }
        }, 100);
    },
    methods: {
        //设置当前专业
        setsbj: function (sbjid) {
            var th = this;
            th.sbjid = sbjid;
            if (!th.isnull) {
                th.sbjids = th.clac_sbjids(sbjid);
            } else {
                window.setTimeout(function () {
                    th.setsbj(sbjid);
                }, 200);
            }
        },
        //获取课程专业的数据
        getSubjects: function () {
            var th = this;
            th.loading = true;
            var form = { orgid: th.orgid, search: '', isuse: true };
            $api.get('Subject/Tree', form).then(function (req) {
                if (req.data.success) {
                    th.subjects = th.clacCount(req.data.result);
                    th.$emit('load', th.subjects);
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.$emit('load', null);
                console.error(err);
            }).finally(() => th.loading = false);
        },
        //计算课程数，ques数，test数
        clacCount: function (datas) {
            this.calcSerial(datas);
            datas.forEach(d => this.ergodic_clacCount(d, 'Sbj_CourseCount', 'CourseCount'));
            datas.forEach(d => this.ergodic_clacCount(d, 'Sbj_QuesCount', 'QuesCount'));
            datas.forEach(d => this.ergodic_clacCount(d, 'Sbj_TestCount', 'TestCount'));
            return datas;
        },
        //遍历计算各个专业的课程数，包括当前专业的子专业
        //field:要计算的字段
        //result:计算结果的字段名，主要为了保留field原始值，方便恢复
        ergodic_clacCount: function (sbj, field, result) {
            let count = sbj[field];
            if (sbj.children && sbj.children.length > 0) {
                let datas = sbj.children;
                for (let i = 0; i < datas.length; i++)
                    count += this.ergodic_clacCount(datas[i], field, result);
            }
            sbj[result] = count;
            return count;
        },
        //计算序号
        calcSerial: function (item, lvl) {
            var childarr = Array.isArray(item) ? item : (item.children ? item.children : null);
            if (childarr == null) return;
            for (let i = 0; i < childarr.length; i++) {
                childarr[i].serial = (lvl ? lvl : '') + (i + 1) + '.';
                childarr[i]['CourseCount'] = 0;
                childarr[i]['QuesCount'] = 0;
                childarr[i]['TestCount'] = 0;
                childarr[i]['calcChild'] = this.calcChild(childarr[i]);
                this.calcSerial(childarr[i], childarr[i].serial);
            }
        },
        calcChild: function (sbj) {
            if (!sbj.children) return 0;
            var count = sbj.children.length;
            for (var i = 0; i < sbj.children.length; i++) {
                count += this.calcChild(sbj.children[i]);
            }
            return count;
        },

        //计算当前选中项
        clac_sbjids: function () {
            var th = this;
            //将当前课程的专业，在控件中显示
            var arr = [];
            arr.push(th.sbjid);
            var sbj = th.traversalQuery(th.sbjid, th.subjects);
            if (sbj == null) {
                throw '专业“' + th.sbjid + '”不存在，或该专业被禁用';
            }
            arr = th.getParentPath(sbj, th.subjects, arr);
            return arr;;
        },
        //获取当前专业的上级路径
        getParentPath: function (entity, datas, arr) {
            if (entity == null) return null;
            var obj = this.traversalQuery(entity.Sbj_PID, datas);
            if (obj == null) return arr;
            arr.splice(0, 0, obj.Sbj_ID);
            arr = this.getParentPath(obj, datas, arr);
            return arr;
        },
        //从树中遍历对象
        traversalQuery: function (sbjid, datas) {
            var obj = null;
            for (let i = 0; i < datas.length; i++) {
                const d = datas[i];
                if (d.Sbj_ID == sbjid) {
                    obj = d;
                    break;
                }
                if (d.children && d.children.length > 0) {
                    obj = this.traversalQuery(sbjid, d.children);
                    if (obj != null) break;
                }
            }
            return obj;
        },
        //选择变化后的事件
        evetChange: function (val) {
            var currid = '';
            if (val.length > 0) currid = val[val.length - 1];
            this.$refs['subject_cascader'].dropDownVisible = false;
            this.$emit('change', currid, val);
        },
        //专业的路径，从子级上溯
        //sbjid:专业id
        //course:课程对象
        subjectPath: function (sbjid, course) {
            if (sbjid == null) return course ? course.Sbj_Name : '';
            if (!th.isnull) return course ? course.Sbj_Name : '';
            //获取专业的路径，从顶级到子级
            var arr = [];
            var sbj = null;
            do {
                sbj = getsbj(sbjid, this.subjects);
                if (sbj == null) break;
                sbjid = sbj.Sbj_PID;
                arr.push(sbj);
            } while (sbj && sbj.Sbj_PID > 0);
            //输出专业的路径
            var path = '';
            for (let i = arr.length - 1; i >= 0; i--) {
                path += arr[i].Sbj_Name;
                if (i != 0) path += '<b>／</b>'
            }
            return path != '' ? path : (course ? course.Sbj_Name : '');
            function getsbj(id, arr) {
                var obj = null;
                for (let i = 0; i < arr.length; i++) {
                    const el = arr[i];
                    if (arr[i].Sbj_ID == id) {
                        obj = arr[i];
                        break;
                    }
                    if (arr[i].children && arr[i].children.length > 0) {
                        obj = getsbj(id, arr[i].children);
                        if (obj != null) return obj;
                    }
                }
                return obj;
            }
        },
    },
    template: `<div>
        <loading v-if="loading" bubble>加载中...</loading>
        <el-cascader v-else class="sbj_cascader" ref="subject_cascader"  style="width: 100%;" clearable v-model="sbjids" placeholder="请选择课程专业" 
            :disabled="disabled" popper-class="sbj_cascader-panel"
            :options="subjects" separator="／" :props="defaultSubjectProps" filterable @change="evetChange">
            <template slot-scope="{ node, data }">             
                <span>{{data.serial}} {{ data.Sbj_Name }}</span>
                <icon course v-if="showitem=='course' && data.CourseCount>0" title="课程数">{{ data.CourseCount }}</icon> 
                <icon question v-if="showitem=='ques' && data.QuesCount>0" title="试题数">{{ data.QuesCount }}</icon>
                <icon test v-if="showitem=='test' && data.TestCount>0" title="试卷数">{{ data.TestCount }}</icon>
                <icon subject v-if="showitem=='child' && data.calcChild>0" title="子专业数">{{ data.calcChild }}</icon>
            </template>
        </el-cascader>      
    </div>`
});