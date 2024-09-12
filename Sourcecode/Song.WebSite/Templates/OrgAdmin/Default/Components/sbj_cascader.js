//专业的级联选择器
$dom.load.css([$dom.path() + 'Components/Styles/sbj_cascader.css']);
Vue.component('sbj_cascader', {
    //sbjid:专业id
    props: ['sbjid', 'orgid', 'disabled'],
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
    watch: {
        'orgid': {
            handler: function (nv, ov) {
                if (nv != null && nv != '' && (nv != ov))
                    this.getSubjects(nv);
            }, immediate: true
        },
        'sbjid': {
            handler: function (nv, ov) {

            }, immediate: true
        },
    },
    created: function () {
        var th = this;
        window.clac_sbjids_setInterval = window.setInterval(function () {
            if (th.sbjid != null && th.subjects.length > 0) {
                th.sbjids = th.clac_sbjids(th.sbjid);
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
            th.sbjids = th.clac_sbjids(sbjid);
        },
        //获取课程专业的数据
        getSubjects: function () {
            var th = this;
            var form = { orgid: th.orgid, search: '', isuse: true };
            $api.get('Subject/Tree', form).then(function (req) {
                if (req.data.success) {
                    let datas = req.data.result;
                    for (let i = 0; i < datas.length; i++) 
                        th.ergodic_clacCourse(datas[i]);               
                    th.subjects = datas;   
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });
        },
        //遍历计算各个专业的课程数，包括当前专业的子专业
        ergodic_clacCourse: function (sbj) {
            let count = sbj.Sbj_CourseCount;
            if (sbj.children && sbj.children.length > 0) {
                let datas = sbj.children;
                for (let i = 0; i < datas.length; i++) 
                    count += this.ergodic_clacCourse(datas[i]);
            }
            sbj.Sbj_CourseCount = count;
            return count;
        },
        //计算当前选中项
        clac_sbjids: function (sbjid) {
            var th = this;
            //将当前课程的专业，在控件中显示
            var arr = [];
            arr.push(sbjid);
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
        subjectPath: function (sbjid, course) {
            if (sbjid == null) return course ? course.Sbj_Name : '';
            if (!this.subjects && this.subjects.length < 1) return course ? course.Sbj_Name : '';
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
    template: `<div class="sbj_cascader">
        <el-cascader ref="subject_cascader" style="width: 100%;" clearable v-model="sbjids" placeholder="请选择课程专业"
            :disabled="disabled" :options="subjects" separator="／" :props="defaultSubjectProps" filterable @change="evetChange">
            <template slot-scope="{ node, data }">
                <span>{{ data.Sbj_Name }}</span>
                <span class="sbj_course" v-if="data.Sbj_CourseCount>0">
                    <icon>&#xe813</icon>{{ data.Sbj_CourseCount }}
                </span>
            </template>
        </el-cascader>      
        </div>`
});