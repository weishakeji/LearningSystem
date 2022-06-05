$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            course: {},
            form: {
                'couid': $api.querystring('id'),
                'acc': '', 'name': '', 'size': 10, 'index': 1
            },
            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数

            exportVisible: false,    //显示导出面板
            files: [],               //导出的文件列表
            fileloading: false,      //导出时的加载状态

            loading: true,
            loading_init: true
        },
        mounted: function () {
            var th = this;
            $api.get('Course/ForID', { 'id': this.form.couid }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.course = result;
                    document.title = '学习记录-《' + th.course.Cou_Name + '》';
                    th.getdata(1);
                    th.getFiles();
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });

        },
        created: function () {

        },
        computed: {
        },
        watch: {
        },
        methods: {
            //加载数据页
            getdata: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 41);
                th.loading = true;
                $api.get("Course/Students", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        for (var i = 0; i < d.data.result.length; i++) {
                            d.data.result[i].isAdminPosi = false;
                        }
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        //console.log(th.accounts);
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            },
            //显示电话
            showTel: function (row) {
                if (row.Ac_MobiTel1 == '' && row.Ac_MobiTel2 == '') {
                    return '';
                }
                if (row.Ac_MobiTel1 == '') row.Ac_MobiTel1 = row.Ac_MobiTel2;
                if (row.Ac_MobiTel1 == row.Ac_MobiTel2) {
                    return row.Ac_MobiTel1;
                }
                return row.Ac_MobiTel1 + (row.Ac_MobiTel2 != '' ? '/' + row.Ac_MobiTel2 : '');
            },
            //显示完成度
            showcomplete: function (num) {
                num = num > 100 ? 100 : num;
                num = Math.round(num * 10000) / 10000;
                return num;
            },
            showcomplPer: function (num) {
                return Math.round(this.showcomplete(num))
            },
            showstatus: function (num) {
                num = this.showcomplPer(num);
                if (num >= 90) return "";
                if (num >= 80) return "success";
                if (num >= 60) return "warning";
                return "exception";
            },
            //显示帮助
            btnhelp: function () {
                var msg = "课程的“学员数”与课程的“学习记录数”之间，可能会存在差异；<br/>";
                msg += "课程的学员数是购买该课程的学员总数；有可能存在学员购买课程后并没有学习，没有生成学习记录。";
                this.$alert(msg, '说明', {
                    dangerouslyUseHTMLString: true
                });
            },
            //在列中显示学员账号
            showacc: function (txt) {
                if (txt != '' && this.form.acc != '') {
                    var regExp = new RegExp(this.form.acc, 'g');
                    txt = txt.replace(regExp, `<red>${this.form.acc}</red>`);
                }
                return txt;
            },
            //在列中显示学员姓名
            showname: function (txt) {
                if (txt != '' && this.form.name != '') {
                    var regExp = new RegExp(this.form.name, 'g');
                    txt = txt.replace(regExp, `<red>${this.form.name}</red>`);
                }
                return txt;
            },
            //已经导出的文件列表
            getFiles: function () {
                var th = this;
                $api.get('Course/StudentsLogOutputFiles', { 'couid': this.form.couid }).then(function (req) {
                    th.fileloading = false;
                    if (req.data.success) {
                        th.files = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //生成导出文件
            toexcel: function () {
                var th = this;
                th.fileloading = true;
                $api.post('Course/StudentsLogOutputExcel', { 'couid': this.form.couid }).then(function (req) {
                    th.fileloading = false;
                    if (req.data.success) {
                        th.getFiles();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    th.fileloading = false;
                    console.error(err);
                });
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                this.fileloading = true;
                $api.get('Course/StudentsLogOutputDelete', { 'couid': this.form.couid, 'filename': file }).then(function (req) {
                    th.fileloading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getFiles();
                        th.$notify({
                            message: '文件删除成功！',
                            type: 'success',
                            position: 'bottom-right',
                            duration: 2000
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
        }
    });
    //考试成绩
    Vue.component('exam_result', {
        props: ["acid"],
        data: function () {
            return {
                result: {},  //成绩 {"score":成绩得分，没有成绩为-1,"state":nopass不及格，normal及格，fine优秀}
                loading: false
            }
        },
        watch: {
            'acid': {
                handler: function (nv, ov) {
                    this.getData(nv);
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () { },
        methods: {
            getData: function (acid) {
                var th = this;
                th.loading = true;
                $api.cache('Course/StudentForCourseExam', { 'couid': $api.querystring('id'), acid }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.result = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            },
            //显示所有价格信息
            showdetail: function () {
                var price = '';
                for (let i = 0; i < this.prices.length; i++) {
                    const item = this.prices[i];
                    price += item.CP_Span + item.CP_Unit + item.CP_Price + "元\n";
                }
                return price;
            }
        },
        template: `<div class="exam_result">
                <span v-if="result.score>-1" :class="result.state">{{result.score}}</span>      
                </div> `
    });
});
