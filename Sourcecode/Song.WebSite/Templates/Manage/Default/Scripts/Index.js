/*!
 * 主 题：管理后台
 * 说 明：
 * 1、web端管理后台，集成树形菜单等控件；
 * 2、各控件数据源基本相同，可以相互转换；
 * 3、当tabs.js（选项卡）切换时，关联pagebox窗体同步切换；
 *
 * 作 者：微厦科技_宋雷鸣_10522779@qq.com
 * 开发时间: 2020年2月1日
 * 最后修订：2023年12月1日
 * github开源地址:https://github.com/weishakeji/WebdeskUI
 */

//起始页
var startpage = "/web/viewport/index";
window.onload = function () {
    //禁用iframe中的右键菜单
    $dom('iframe').each(function () {
        var doc = this.contentDocument.body;
        doc.setAttribute('oncontextmenu', "javascript:return false;");
    });
    console.log(window.location.hostname);
};
//加载组件所需的javascript文件
window.$ctrljs = function (f) {
    $dom.corejs(function () {
        var arr = ['pagebox', 'treemenu', 'dropmenu', 'tabs', 'verticalbar', 'timer', 'skins', 'login'];
        for (var t in arr) arr[t] = '/Utilities/Panel/Scripts/' + arr[t] + '.js';
        window.$dom.load.js(arr, f);
    }, '/Utilities/Panel/Scripts/ctrls.js');
};
$ctrljs(function () {
    window.login = $login.create({
        target: '#login-area',
        ico: 'e79b',
        //width: '320px',
        loading: true,
        title: '...',
        company: '',
        website: 'http://www.weishakeji.net',
        tel: ''
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
        $api.cache('Platform/PlatInfo').then(function (req) {
            if (req.data.success) {
                window.platinfo = req.data.result;
                if (s != null) s.title = window.platinfo.title;
                if (window.platinfo.title != '')
                    $dom("*[platinfo='title']").html(window.platinfo.title);
                if (window.platinfo.intro != '') {
                    $dom("*[platinfo='intro']").html(window.platinfo.intro).show();
                    $dom("*[platinfo='intro']").prev().hide();
                } else {
                    $dom("*[platinfo='intro']").hide();
                    $dom("*[platinfo='intro']").prev().show();
                }
                document.title = '平台管理 - ' + window.platinfo.title;
            } else {
                throw req.data.message;
            }
        }).catch(function (err) {
            console.error(err);
        });
        //版权信息
        $api.get('Copyright/Info').then(function (req) {
            if (req.data.success) {
                var copyright = req.data.result;
                window.login.company = copyright.abbr;
                window.login.website = copyright.website;
                window.login.tel = copyright.tel;
            } else {
                console.error(req.data.exception);
                throw req.data.message;
            }
        }).catch(function (err) {
            console.error(err);
        });
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
        $api.post('Admin/LoginSuper', { 'acc': s.user, 'pw': s.pw, 'vcode': s.vcode, 'vmd5': s.vcodemd5 }).then(function (req) {
            if (req.data.success) {
                //登录成功
                var result = req.data.result;
                $api.login.in('admin', result.Acc_Pw, result.Acc_Id);
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
                    case '94060':
                        s.tips(s.inputs.user, false, '数据库链接异常');
                        break;
                    default:
                        s.tips(s.inputs.user, false, '登录失败');
                        console.error(req.data.exception);
                        console.log(req.data.message);
                        break;
                }
                s.loading = false;
            }
        }).catch(function (err) {
            //alert(err);
            console.error(err);
        });
    });
    //判断是否登录
    $api.login.current('super', function (d) {
        ready(d);
    }, function (err) {
        window.login.loading = false;
    });
    //右上角菜单,用户信息
    window.usermenu = window.$dropmenu.create({
        target: '#user-area',
        width: 100,
        plwidth: 120,
        level: 40000
    }).onclick(nodeClick);
    $dom.get('/Utilities/datas/manage_userinfo.json', function (req) {
        //usermenu.add(eval('(' + req + ')'));
        usermenu.add(req);
    });
    //左上角下拉菜单
    var drop = window.$dropmenu.create({
        target: '#dropmenu-area',
        //width: 280,  
        plwidth: 180,
        level: 30000,
        id: 'main_menu'
    }).onclick(nodeClick);

    /*获取配置的菜单信息*/
    var islocal = $api.islocal();
    islocal = true;
    if (!islocal) {
        $dom.get('/Utilities/datas/systemmenu.json', function (d) {
            var result = nodeconvert(d);
            drop.add(result);
        });
    } else {
        //获取数据库的菜单信息
        $api.get('ManageMenu/SystemMenuShow:60').then(function (req) {
            if (req.data.success) {
                var result = nodeconvert(req.data.result);
                drop.add(result);
            } else {
                console.error(req.data.exception);
                throw req.config.way + ' ' + req.data.message;
            }
        }).catch(function (err) {
            console.error(err);
        });
    }
    /*
    drop.ondata(function (s, e) {
        window.setTimeout(function () {
            var left = $dom('#dropmenu-area').width() + 10;
            console.log('dropmenu:' + left);
            $dom('#headbar').css('opacity', 1).left(left);
            $dom('#headbar').width('calc(100% - ' + left + 'px - ' + (100) + 'px)');
        }, 1000);
    });*/
});

function ready(result) {
    window.setTimeout(function () {
        window.login.loading = false;
        $dom('panel#login').hide();
        $dom('panel#admin').show().css('opacity', 0);
        window.$skins.onchange();
        //右侧菜单信息
        window.usermenu.datas[0].title = result.Acc_Name;
        if (result.Acc_Photo != '')
            window.usermenu.datas[0].img = result.Acc_Photo;
    }, 1000);
    //树形菜单
    window.tree = $treemenu.create({
        target: '#treemenu-area',
        width: 200
    }).onresize(function (s, e) { //当宽高变更时
        $dom('#tabs-area').width('calc(100% - ' + (e.width + vbar.width + 10) + 'px )');
    }).onfold(function (s, e) { //当右侧树形折叠时
        var width = e.action == 'fold' ? vbar.width + 50 : s.width + vbar.width + 10;
        $dom('#tabs-area').width('calc(100% - ' + width + 'px )');
    }).onclick(nodeClick);
    //加载左侧菜单树
    var islocal = $api.islocal();
    islocal = true;
    if (!islocal) {
        $dom.get('/Utilities/datas/SuperAdmin.json', function (d) {
            var result = nodeconvert(d);
            window.tree.add(result);
            window.tree.complete = true;
        });
    } else {
        $api.get('ManageMenu/Menus:60').then(function (req) {
            if (req.data.success) {
                var result = nodeconvert(req.data.result);
                window.tree.add(result);
                window.tree.complete = true;
            } else {
                console.error(req.data.exception);
                throw req.data.message;
            }
        }).catch(function (err) {
            console.error(err);
        });
    }
    //竖形工具条
    var vbar = $vbar.create({
        target: '#vbar-area',
        id: 'rbar-156',
        width: 30,
        height: 'calc(100% - 35px)'
    }).onclick(nodeClick);
    $dom.get('/Utilities/datas/vbar.json', function (req) {
        vbar.add(req);
    });
    //选项卡
    var tabs = $tabs.create({
        target: '#tabs-area',
        width: 1,
        default: {
            title: '启始页',
            path: '学习平台,启始页',
            //url: '/manage/Startpage',
            url: startpage,
            ico: 'a020'
        }
    });
    tabs.onshut(tabsShut).onchange(tabsChange);
    tabs.onhelp(function (s, e) {
        $pagebox.create({
            pid: e.data.id, //父id,此处必须设置，用于判断该弹窗属于哪个选项卡
            width: 600,
            height: 400,
            url: e.data.help,
            //ico: e.data.ico,
            ico: "a026",
            title: e.data.title + '- 帮助'
        }).open();
    });
    window.tabsContent = tabs;

    //风格切换事件
    window.$skins.onchange(function (s, e) {
        $dom('body>*:not(#loading)').css('opacity', 0);
        $dom('#loading').show();
    });
    window.$skins.onloadcss(function (s, e) {
        window.setTimeout(function () {
            var left = $dom('#dropmenu-area').width() + 10;
            $dom('#headbar').css('opacity', 1).left(left);
            $dom('#headbar').width('calc(100% - ' + left + 'px - ' + (100) + 'px)');
            $dom('body>*:not(#loading)').css('opacity', 1);
            $dom('#loading').hide();
        }, 500);

    });
};
/*
    节点处理方法
*/
function nodeconvert(obj) {
    var result = '';
    if (typeof (obj) != 'string')
        result = JSON.stringify(obj);
    //result = result.replace(/MM_WinID/g, "id");
    result = result.replace(/MM_Name/g, "title");
    result = result.replace(/MM_AbbrName/g, "tit");
    result = result.replace(/children/g, "childs");
    result = result.replace(/MM_Intro/g, "intro");
    result = result.replace(/MM_Type/g, "type");
    result = result.replace(/MM_Link/g, "url");
    result = result.replace(/MM_Help/g, "help");
    result = result.replace(/MM_WinWidth/g, "width");
    result = result.replace(/MM_WinHeight/g, "height");
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
//节点点击事件，tree,drop,vbar统一用这一个
function nodeClick(sender, eventArgs) {
    var data = eventArgs.data;
    if (!!data.childs && data.childs.length > 0) return; //如果有下级节点，则不响应事件
    //节点类型
    //open：弹窗，item菜单项（在tabs中打开)，event脚本事件,
    //link外链接（直接响应）,node节点下的子项将一次性打开（此处不触发）
    //console.log(eventArgs.data.title);
    switch (data.type) {
        case 'open':
            //当有窗体标识，则替代id
            if (data.MM_WinID && data.MM_WinID != "") data.id = data.MM_WinID;
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
        default:
            if (!!data.url) {
                //if (data.help == null || data.help == '') data.help = data.url
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

}