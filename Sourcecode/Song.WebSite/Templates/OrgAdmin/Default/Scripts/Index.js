/*!
 * 主 题：管理后台
 * 说 明：
 * 1、web端管理后台，集成树形菜单等控件；
 * 2、各控件数据源基本相同，可以相互转换；
 * 3、当tabs.js（选项卡）切换时，关联pagebox窗体同步切换；
 *
 * 作 者：微厦科技_宋雷鸣_10522779@qq.com
 * 开发时间: 2020年2月1日
 * 最后修订：2020年2月28日
 * github开源地址:https://github.com/weishakeji/WebdeskUI
 */
window.onload = function () {
    //禁用iframe中的右键菜单
    $dom('iframe').each(function () {
        var doc = this.contentDocument.body;
        doc.setAttribute('oncontextmenu', "javascript:return false;");
    });
};
$ctrljs(function () {
    window.login = $login.create({
        target: '#login-area',
        ico: 'a003',
        //icoimg:'/favicon.ico',
        loading: true,
        //width: '320px',
        title: '...',
        buttontxt: '登录机构管理',
        //company: '微厦科技',
        //website: 'http://www.weishakeji.net',
        //tel: '400 6015615'
    });
    //自定义验证
    window.login.verify([{
        'ctrl': 'user',
        'regex': /^[a-zA-Z0-9_-]{4,16}$/,
        'tips': '长度不得小于4位大于16位'
    }, {
        ctrl: 'vcode',
        regex: /^\d{4}$/,
        tips: '请输入4位数字'
    }]);
    window.login.onlayout(function (s, e) {
        //console.log('布局完成' + e.data);        
    });
    window.login.ondragfinish(function (s, e) {
        $api.post('Helper/CheckCodeImg', { 'leng': s.vcodelen, 'acc': s.user }).then(function (req) {
            if (req.data.success) {
                var result = req.data.result;
                s.vcodebase64 = result.base64;
                s.vcodemd5 = result.value;
            } else {
                throw req.data.message;
            }
        }).catch(function (err) {
            throw err;
        });
    });
    window.login.onsubmit(function (s, e) {
        s.loading = true;
        $api.post('Admin/Login', { 'acc': s.user, 'pw': s.pw, 'vcode': s.vcode, 'vmd5': s.vcodemd5 }).then(function (req) {
            if (req.data.success) {
                //登录成功
                var result = req.data.result;
                $api.loginstatus('admin', result.Acc_Pw);
                $api.login.admin_fresh();
                ready(req.data.result);
            } else {
                var data = req.data;
                switch (String(data.state)) {
                    //验证码错误
                    case '1101':
                        s.tips(s.inputs.vcode, false, data.message);
                        break;
                    case '1102':
                        s.tips(s.inputs.pw, false, '账号或密码错误');
                        break;
                    case '1103':
                        s.tips(s.inputs.user, false, '账号被禁用');
                        break;
                    default:
                        s.tips(s.inputs.user, false, req.data.message);
                        break;
                }
                s.loading = false;
            }
        }).catch(function (err) {
            alert(err);
            console.error(err);
        });
    });
    //判断是否登录
    $api.login.admin().then(function (d) {
        ready(d);
        $api.login.admin_fresh();
    }).catch(() => {
        window.login.loading = false;
    });

    //右上角菜单,用户信息
    window.usermenu = window.$dropmenu.create({
        target: '#user-area',
        width: 100,
        plwidth: 120,
        level: 30000
    }).onclick(nodeClick);
    //用户信息的下拉菜单
    $dom.get($dom.path() + '../_public/datas/usermenu.json', function (req) {
        usermenu.add(req);
    });
    window.createVapp();
});

function ready(result) {
    window.setTimeout(function () {
        window.login.loading = false;
        //$dom('panel#login').remove();
        $dom('panel#admin').show().css('opacity', 0);
        //window.$skins.onchange();
        window.setTimeout(function () {
            $dom('panel#login').remove();
        }, 2000);
        window.$skins.setup('Office');
        //右侧菜单信息
        window.usermenu.datas[0].title = result.Acc_Name;
        if (result.Acc_Photo != '')
            window.usermenu.datas[0].img = result.Acc_Photo;
    }, 1000);
    //树形菜单
    window.tree = $treemenu.create({
        target: '#treemenu-area',
        width: 200,
        taghide: false, query: true, fold: false
    }).onresize(function (s, e) { //当宽高变更时
        $dom('#tabs-area').width('calc(100% - ' + (e.width + 5) + 'px )');
    }).onfold(function (s, e) { //当右侧树形折叠时
        var width = e.action == 'fold' ? 45 : s.width + 5;
        $dom('#tabs-area').width('calc(100% - ' + width + 'px )');
    }).onclick(nodeClick);
    //监听树形菜单的菜单查询面板状态，给背景加模糊效果
    window.tree.watch({
        'querypanel': function (obj, val, old) {
            $dom("#admin").css('filter', val ? 'blur(3px)' : 'none');
            $dom(".pagebox").css('filter', val ? 'blur(3px)' : 'none');
        }
    });

    //加载左侧菜单树
    $api.cache('ManageMenu/OrganMarkerMenus:60', { 'marker': 'organAdmin' }).then(function (req) {
        if (req.data.success) {
            var result = nodeconvert(req.data.result);//return;
            console.log(result);
            if (result[0].childs.length > 0)
                tree.add(result[0].childs);
            /*
       //启起页 
       window.setTimeout(function () {
           var data = window.tree.getData('node_a76471634c09b23347199ee23682d1ed');
           window.tree.trigger('click', {
               treeid: data.id,
               data: data
           });
       }, 1000);
*/
        } else {
            throw req.data.message;
        }
    }).catch(function (err) {
        console.error(err);
    });
    //选项卡
    var tabs = $tabs.create({
        target: '#tabs-area',
        width: 1,
        default: {
            title: '启始页',
            path: '机构管理,启始页',
            url: '/orgadmin/start',
            ico: 'a020'
        }
    });
    tabs.onshut(tabsShut).onchange(tabsChange).onfull(function (s, e) {
        //alert(s);
    });
    tabs.onhelp(function (s, e) {
        let url = e.data.help && e.data.help != '' ? e.data.help : '/help' + e.data.url + '.html';
        $pagebox.create({
            pid: e.data.id, //父id,此处必须设置，用于判断该弹窗属于哪个选项卡
            width: '80%',
            height: '80%',
            url: url,
            title: e.data.title + '- 帮助'
        }).open();
    });
    window.tabsContent = tabs;

    //风格切换事件
    window.$skins.onchange(function (s, e) {
        $dom('body>*:not(#loading,[crtid])').css('opacity', 0);
        $dom('#loading').show();
    });
    window.$skins.onloadcss(function (s, e) {
        window.setTimeout(function () {
            $dom('body>*:not(#loading,[crtid])').css('opacity', 1);
            $dom('#loading').hide();
        }, 500);

    });
};
/*
    节点处理方法
*/
function nodeconvert(obj) {
    var result = JSON.stringify(obj);
    result = result.replace(/MM_Id/g, "menuid");
    result = result.replace(/MM_Name/g, "title");
    result = result.replace(/MM_AbbrName/g, "tit");
    result = result.replace(/children/g, "childs");
    result = result.replace(/MM_Intro/g, "intro");
    result = result.replace(/MM_Type/g, "type");
    result = result.replace(/MM_Link/g, "url");
    result = result.replace(/MM_Help/g, "help");
    result = result.replace(/MM_IsUse/g, "use");
    result = result.replace(/MM_WinWidth/g, "width");   //弹窗相关
    result = result.replace(/MM_WinHeight/g, "height");
    result = result.replace(/MM_WinID/g, "winid");
    result = result.replace(/MM_WinMin/g, "min");
    result = result.replace(/MM_WinMax/g, "max");
    result = result.replace(/MM_WinMove/g, "move");
    result = result.replace(/MM_WinResize/g, "resize");
    result = result.replace(/MM_Complete/g, "complete");
    return JSON.parse(result);
}
/*
    事件
*/
//节点点击事件，tree,drop,统一用这一个
function nodeClick(sender, eventArgs) {
    //console.log(eventArgs);
    var data = eventArgs.data;
    //如果有下级节点，则不响应事件
    if ((!!data.childs && data.childs.length > 0) && data.type != 'node') {
        return;
    }
    //节点类型
    //open：弹窗，item菜单项（在tabs中打开)，event脚本事件,
    //link外链接（直接响应）,node节点下的子项将一次性打开（此处不触发）
    //console.log(eventArgs.data.title);
    switch (data.type) {
        case 'open':
            $pagebox.create(data).open();
            break;
        case 'event':
            if (!data.url) return;
            try {
                eval(data.url);
            } catch (err) {
                alert('脚本执行错误，请仔细检查：\n' + data.url);
                console.error(err);
            }
            break;
        case 'node':
            console.log(sender);
            window.tabsContent.clear();
            if (data.childs.length > 0) {
                for (var i = 0; i < data.childs.length; i++) {
                    window.tabsContent.add(data.childs[i]);
                }
                window.tabsContent.focus(data.childs[0].id);
            }
            break;
        default:
            if (!!data.url) {
                //window.tabsContent.clear();
                //if (data.help == null || data.help == '') data.help = data.url;
                window.tabsContent.add(data);
            }
            break;
    }
}
//选项卡关闭事件
function tabsShut(sender, eventArgs) {
    var data = eventArgs.data;
    //获取当前标签生成的窗体
    var boxs = $ctrls.all('pagebox');
    var arr = new Array();
    for (var i = 0; i < boxs.length; i++) {
        if (boxs[i].obj.pid == data.id) {
            arr.push(boxs[i].obj);
            var childs = boxs[i].obj.getChilds();
            for (var j = 0; j < childs.length; j++) {
                arr.push(childs[j]);
            }
        }
    }
    //关闭当前标签生成的窗体
    if (arr.length > 0) {
        if (confirm('当前选项卡“' + data.title + '”有 ' + arr.length + '个 窗体未关闭，\n是否全部关闭？')) {
            for (var i = 0; i < arr.length; i++) arr[i].shut();
            return true;
        }
        return false;
    }
    return true;
}
//选项卡切换事件
function tabsChange(sender, eventArgs) {
    //获取当前标签生成的窗体，全部还原
    var selfbox = getSelfbox(eventArgs.data.id);
    for (var i = 0; i < selfbox.length; i++) {
        selfbox[i].toWindow().focus(false);
    }
    //非当前标签的窗体，全部最小化
    var elsebox = getElsebox(sender, eventArgs.data.id);
    for (var i = 0; i < elsebox.length; i++)  elsebox[i].toMinimize(false);

    //当前标签生成的窗体
    function getSelfbox(tabid) {
        var boxs = $ctrls.all('pagebox');
        //获取当前标签生成的窗体，全部还原
        var arr = new Array();
        for (var i = 0; i < boxs.length; i++) {
            if (boxs[i].obj.pid == tabid) {
                arr.push(boxs[i].obj);
                var childs = boxs[i].obj.getChilds();
                for (var j = 0; j < childs.length; j++)
                    arr.push(childs[j]);
            }
        }
        //按层深排序，以保证在还原时保持窗体原有层叠效果
        for (var i = 0; i < arr.length - 1; i++) {
            for (var j = 0; j < arr.length - 1 - i; j++) {
                if (arr[j].level > arr[j + 1].level) {
                    var temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
        return arr;
    }
    //非当前标签的窗体(不包括其它控件生成的窗体)
    function getElsebox(sender, tabid) {
        var boxs = $ctrls.all('pagebox');
        var tabs = sender.childs;
        var arr = [];
        for (var i = 0; i < tabs.length; i++) {
            if (tabs[i].id == tabid) continue;
            for (var j = 0; j < boxs.length; j++) {
                if (boxs[j].obj.pid == tabs[i].id) {
                    arr.push(boxs[j].obj);
                    var childs = boxs[j].obj.getChilds();
                    for (var n = 0; n < childs.length; n++)
                        arr.push(childs[n]);
                }
            }
        }
        return arr;
    }
    //当前选项卡id
    let tabid = eventArgs.tabid;
    window.tree.currentnode(tabid);
}

//执行vue对象
window.createVapp = function () {
    window.vapp = new Vue({
        el: '#header-box',
        data: {
            organ: {},
            platinfo: {},
            menus: [],
            loading_init: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, organ) {
                th.loading_init = false;
                //获取结果              
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                /* 此处用来显示机构的logo，暂时没有用*/
                window.setTimeout(function () {
                    if (th.organ.Org_Logo != '') {
                        window.login.icoimg = th.organ.Org_Logo;
                        window.login.title = '';
                    } else {
                        window.login.title = th.organ.Org_PlatformName;
                    }
                }, 300);
                //window.login.title = th.organ.Org_PlatformName;
                document.title = '机构管理 - ' + th.organ.Org_PlatformName;
                //
                th.getnavi();
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {

        },
        watch: {
            'menus': {
                handler: function (nv, ov) {
                    this.$nextTick(function () {
                        //$dom(".menubar").text('ddd');
                        if (!window.$dropmenu) return;
                        window.menubar = window.$dropmenu.create({
                            target: '#menubar',
                            deftype: 'link',
                            plwidth: 180,
                            height: 45,
                            level: 20000
                        }).onclick(this.menuClick);
                        window.menubar.add(this.nodeconvert(nv));
                    });
                }, immediate: true
            }
        },
        methods: {
            //获取导航菜单
            getnavi: function () {
                if (!this.organ.Org_ID) return;
                var th = this;
                $api.get('Navig/web', { 'orgid': this.organ.Org_ID, 'type': 'main' }).then(function (req) {
                    if (req.data.success) {
                        th.menus = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },
            //导航菜单的点击事件
            menuClick: function (sender, eventArgs) {

                var data = eventArgs.data;
                if (!data || data.url == '') return;
                window.location.href = data.url;
                console.log(data);
            },
            //节点转换
            nodeconvert: function (obj) {
                var result = '';
                if (typeof (obj) != 'string')
                    result = JSON.stringify(obj);
                //result = result.replace(/MM_WinID/g, "id");
                result = result.replace(/Nav_Name/g, "title");
                result = result.replace(/Nav_Name/g, "tit");
                result = result.replace(/Nav_Icon/g, "ico");
                result = result.replace(/children/g, "childs");
                result = result.replace(/Nav_Url/g, "url");
                return JSON.parse(result);
            }
        }
    });

};
