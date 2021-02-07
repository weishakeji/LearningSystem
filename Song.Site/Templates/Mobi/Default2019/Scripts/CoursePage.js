$(function () {
    default_event();
});
//菜单项的事件
function default_event() {
    //主菜单，即课程学习菜单的事件
    mui('body').on('tap', 'div.mm-item', function () {
        var type = $(this).attr("type");
        //if($.trim(type)=="open")return;
        var canStudy = $(this).parent().attr("canStudy");
        var couid = $(this).parent().attr("couid");
        if (canStudy != "true") {
            var txt = "由此进入<a href='CourseBuy.ashx?couid=" + couid + "' type='link' target='_top'>购买当前课程</a>";
            txt += "或<a href='coureses.ashx' type='link' target='_top'>课程中心</a>";
            txt = "当前课程没有购买！<br/>" + txt;
            new MsgBox("提示", txt, 90, 200, "alert").Open();
        } else {
            document.location.href = $(this).attr("href");
        }
        return false;
    });
}

window.vapp = new Vue({
    el: '#vapp',
    data: {
        couid: 0,        //课程id
        organ: {},       //当前机构
        config: {},      //当前机构配置项     
        account: null,     //当前登录学员 
        course: {},         //当前课程对象
        sum: 0,              //购买课程的人数
        teacher: null,     //课程教师
        outlines: [],     //课程章节
        guides: [],          //课程通知
        prices: [],          //课程价格
        isbuy: false,        //是否购买课程
        record: null,          //课程购买记录
        canStudy: false,     //是否能够学习
        defimg: '',   //课程默认图片
        loading: false,       //加载状态
        showState: 1         //内容显示的切换状态
    },
    watch: {
        'curr_sbjid': function (nv, ov) {
            console.log(nv);
        }
    },
    created: function () {
        this.loading = true;
        //默认图片
        var img = document.getElementById("default-img");
        this.defimg = img.getAttribute("src");
        //当前机构，当前登录学员
        $api.bat(
            $api.get('Organ/Current'),
            $api.get('Account/Current')
        ).then(axios.spread(function (organ, account) {
            //判断结果是否正常
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i].status != 200)
                    console.error(arguments[i]);
                var data = arguments[i].data;
                if (!data.success && data.exception != '') {
                    console.error(data.exception);
                    throw data.message;
                }
            }
            //获取结果
            vapp.organ = organ.data.result;
            vapp.account = account.data.result;
            console.log(vapp.account);
        })).catch(function (err) {
            console.error(err);
        });
        //课程信息
        this.couid = $api.querystring("couid");
        $api.cache('Course/ForID', { 'id': this.couid }).then(function (req) {
            if (req.data.success) {
                vapp.course = req.data.result;
                vapp.course.Cou_Target = vapp.clearTag(vapp.course.Cou_Target);
                vapp.course.Cou_Intro = $api.trim(vapp.course.Cou_Intro);
                document.title = vapp.course.Cou_Name;
                //课程章节，价格，购买人数,通知，教师，是否购买,课程访问数
                var couid = vapp.course.Cou_ID;
                $api.bat(
                    $api.cache('Outline/Tree', { 'couid': couid }),
                    $api.cache('Course/Prices', { 'uid': vapp.course.Cou_UID }),
                    $api.get('Course/StudentSum', { 'couid': couid }),
                    $api.cache('Course/Guides', { 'couid': couid, 'count': 20 }),
                    $api.cache('Teacher/ForID', { 'id': vapp.course.Th_ID }),
                    $api.get('Course/Studied', { 'couid': couid }),
                    $api.get('Course/PurchaseRecord', { 'couid': couid }),
                    $api.get('Course/StudyAllow', { 'couid': couid })
                ).then(axios.spread(function (outlines, prices, sum, guides, teacher, isbuy, record, canStudy) {
                    //判断结果是否正常
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i].status != 200)
                            console.error(arguments[i]);
                        var data = arguments[i].data;
                        if (!data.success && data.exception != '') {
                            console.error(data.exception);
                            throw data.message;
                        }
                    }
                    vapp.loading = false;
                    //获取结果
                    vapp.outlines = outlines.data.result;
                    vapp.prices = prices.data.result;
                    vapp.sum = sum.data.result;
                    vapp.guides = guides.data.result;
                    vapp.teacher = teacher.data.result;
                    vapp.isbuy = isbuy.data.result;
                    vapp.record = record.data.result;
                    vapp.canStudy = canStudy.data.result;
                })).catch(function (err) {
                    console.error(err);
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
    methods: {
        //清理Html标签
        clearTag: function (html) {
            var txt = html.replace(/<\/?.+?>/g, "");
            txt = $api.trim(txt);
            return txt;
        }
    }
});


