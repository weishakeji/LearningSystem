// 考试编辑中的学员组选择
Vue.component('group_select', {
    props: ['type', 'theme', 'organ'],
    data: function () {
        return {
            //按分组选择参考人员
            studentSorts: [],    //所有学员组
            selectedSorts: [],       //选中的学员组,记录的是id，不是对象

            examGroup: [],       //考试主题与学员组的关联对象

            completed: 2,        //是否加载完成，每加载一个条件完成，减一，等于0时为完成

            loading: false
        }
    },
    watch: {
        'organ': {
            handler: function (nv, ov) {
                this.getStudentSort();
            }, immediate: true
        },
        'theme': {
            handler: function (nv, ov) {
                this.getSelectedSort();
            }, immediate: true
        },
        //每加载一个条件完成，减一，等于0时为完成
        'completed': function (nv, ov) {
            this.selectedObj(this.selectedSorts);
        }

    },
    computed: {},
    mounted: function () {
        var css = $dom.pagepath() + 'Components/Styles/group_select.css';
        $dom.load.css([css]);
    },
    methods: {
        //获取所有学员组
        getStudentSort: function () {
            var th = this;
            $api.get('Account/SortAll', { 'orgid': th.organ.Org_ID, 'use': true }).then(function (req) {
                if (req.data.success) {
                    th.studentSorts = req.data.result;
                    th.completed--;
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        //获取当前选中的学员组
        getSelectedSort: function () {
            var th = this;
            if (th.theme.Exam_ID <= 0) return;
            $api.get('Exam/Groups', { 'uid': th.theme.Exam_UID }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    for (var i = 0; i < result.length; i++)
                        th.selectedSorts.push(result[i].Sts_ID);
                    th.completed--;
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err));
        },
        //选中的对象
        selectedObj: function (val) {
            var arr = [];
            if (val.length <= 0) {
                this.examGroup = [];
                return arr;
            }
            if (this.studentSorts.length <= 0) return arr;
            for (let i = 0; i < val.length; i++) {
                const id = val[i];
                var sort = this.studentSorts.find(function (item) {
                    return item.Sts_ID == id;
                });
                if (sort == null) continue;
                arr.push(sort);
            }
            //生成关联对象
            var groups = [];
            for (let i = 0; i < arr.length; i++) {
                const item = arr[i];
                groups.push({
                    Exam_UID: this.theme.Exam_UID,
                    Eg_Type: 2,
                    Org_ID: this.theme.Org_ID,
                    Sts_ID: item.Sts_ID
                });
            }
            this.examGroup = groups;
            console.log(arr);
            return arr;
        }

    },
    //
    template: `<div class="SortSelected" v-show="type==2">
        <el-transfer v-model="selectedSorts" :props="{key: 'Sts_ID',label: 'Sts_Name'}" filterable
        :titles="['学员组', '已选择的学员组']" :data="studentSorts" @change="selectedObj">
        </el-transfer>
    </div>`
});