/*!
 * 主 题：管理后台
 * 说 明：
 * 1、web端管理后台，集成树形菜单等控件；
 * 2、各控件数据源基本相同，可以相互转换；
 * 3、当tabs.js（选项卡）切换时，关联pagebox窗体同步切换；
 *
 * 作 者：微厦科技_宋雷鸣_10522779@qq.com
 * 开发时间: 2020年2月1日
 * 最后修订：2025年1月30日
 * github开源地址:https://github.com/weishakeji/WebdeskUI
 */
$ctrljs(function () {
    //禁用iframe中的右键菜单
    document.addEventListener('DOMContentLoaded', function () {
        $dom('iframe').each(function () {
            var doc = this.contentDocument ? this.contentDocument.body : null;
            if (doc == null) return;
            doc.setAttribute('oncontextmenu', "javascript:return false;");
        });
    });
    document.title = '管理中心';
    //
    $api.bat(
        $api.cache('Platform/PlatInfo:60'),
        $api.get('Organization/current')
    ).then(([platinfo, organ]) => {
        //皮肤设置
        window.$skins.setup('', function (skin) {
            if (skin == null) return;
            console.log(skin);
            //设置登录页的背景动画
            $dom("#backgroup_iframe").attr("src", skin.bgpage);
        });
        //获取结果    
        window.$loyout(platinfo.data.result, organ.data.result);
    }).catch(err => console.error(err))
        .finally(() => { });
});
//初始布局
window.$loyout = function (platinfo, org) {
    //管理界面的头部标题部分
    $dom("*[platinfo='title']").html(org.Org_PlatformName);
    $dom("*[platinfo='intro']").html(platinfo.intro);

    //创建登录框
    window.login = $login.create({
        target: '#login-area',
        ico: 'a001',
        icoimg: org.Org_Logo,
        loading: true,
        //width: '320px',
        title: platinfo.title,
        buttontxt: '登  录',
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
                let result = req.data.result;
                s.vcodebase64 = result.base64;
                s.vcodemd5 = result.value;
            } else {
                throw req.data.message;
            }
        }).catch(err => console.error(err));
    });
    window.login.onsubmit(function (s, e) {
        s.loading = true;
        $api.post('Admin/Login', { 'acc': s.user, 'pw': s.pw, 'vcode': s.vcode, 'vmd5': s.vcodemd5 }).then(function (req) {
            if (req.data.success) {
                //登录成功
                var result = req.data.result;
                $api.login.in('admin', result.Acc_Pw);
                $succeeded(req.data.result);
            } else {
                let data = req.data;
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
            }
        }).catch(err => console.error(err))
            .finally(() => s.loading = false);
    });
    //判断是否登录
    $api.login.current('admin', d => $succeeded(d), () => window.login.loading = false);
    //10分钟刷新一次登录状态
    window.setInterval(function () {
        $api.login.fresh('admin');
    }, 1000 * 60 * 10);
    //右上角菜单（即当前登录用户的信息）
    window.usermenu = window.$dropmenu.create({
        target: '#user-area',
        width: 110,
        plwidth: 120,
        level: 30000
    }).onclick($event.nodeClick);
    //用户信息的下拉菜单
    $dom.get($dom.path() + 'Panel/Datas/usermenu.json', req => usermenu.add(req));

};
//登录成功
window.$succeeded = function (result) {
    window.setTimeout(function () {
        window.login.loading = false;
        $dom('panel#admin').show().css('opacity', 0);
        window.$skins.onchange();
        $dom('panel#login').remove();
        //右侧菜单信息
        window.usermenu.datas[0].title = result.Acc_Name;
        if (result.Acc_Photo != '')
            window.usermenu.datas[0].img = result.Acc_Photo;
        //打开起始引导的窗体
        //window.openStartBox();
    }, 1000);
    //打开起始引导的窗体
    window.openStartBox = function () {
        //该值的设置，在SatarBox.js中设置
        var show = $api.storage('StartBox_not_show');
        show = show == null ? false : JSON.parse(show);
        if (show === true) return;
        $pagebox.create({
            width: '800', height: '80%', ico: 'e820', showmask: true, min: false, max: false,
            url: '/admin/StartBox', title: '欢迎使用'
        }).open();
    }
    //树形菜单
    window.tree = $treemenu.create({
        target: '#treemenu-area',
        width: 200,
        taghide: false, query: true, fold: false
    }).onresize(function (s, e) { //当宽高变更时
        $dom('#tabs-area').width('calc(100% - ' + (e.width + 35) + 'px )');
    }).onfold(function (s, e) { //当右侧树形折叠时
        let width = e.action == 'fold' ? 80 : s.width + 35;
        $dom('#tabs-area').width('calc(100% - ' + width + 'px )');
    }).onclick($event.nodeClick);
    //监听树形菜单的菜单查询面板状态，给背景加模糊效果
    window.tree.watch({
        'querypanel': function (obj, val, old) {
            $dom("#admin").css('filter', val ? 'blur(3px)' : 'none');
            $dom(".pagebox").css('filter', val ? 'blur(3px)' : 'none');
        }
    });
    //加载左侧菜单树
    $api.get('ManageMenu/OrganMarkerMenus', { 'marker': 'organAdmin' }).then(function (req) {
        if (req.data.success) {
            var result = nodeconvert(req.data.result);
            for (var i = 0; i < result.length; i++) {
                if (i == 0)
                    tree.add(result[i].childs);
                else {
                    tree.add(result[i]);

                }
            }
        } else throw req.data.message;
    }).catch(err => console.error(err));
/*
    //左上角下拉菜单（即系统菜单）
    window.drop = window.$dropmenu.create({
        target: '#dropmenu-area',
        width: 280,
        plwidth: 180,
        level: 30000,
        id: 'main_menu'
    }).onclick($event.nodeClick);
    $api.get('ManageMenu/SystemMenuShow:60').then(function (req) {
        if (req.data.success) {
            var result = nodeconvert(req.data.result);
            drop.add(result);
        } else {
            console.error(req.data.exception);
            throw req.config.way + ' ' + req.data.message;
        }
    }).catch(err => console.error(err));*/
    //竖形工具条
    var vbar = $vbar.create({
        target: '#vbar-area', id: 'rbar-156',
        width: 30, height: 'calc(100% - 35px)'
    }).onclick($event.nodeClick);
    $dom.get($dom.path() + 'Panel/Datas/vbar.json', req => vbar.add(req));
    //选项卡
    window.tabs = $tabs.create({
        target: '#tabs-area',
        help: true,    //不显示帮助按钮
        default: {
            title: '启始页',
            path: '机构管理,启始页',
            url: '/orgadmin/start',
            ico: 'a020'
        }
    });
    tabs.onshut($event.tabsShut).onchange($event.tabChange).onfull((s, e) => {
        //alert(s);
    });
    //选项卡的帮助
    tabs.onhelp(function (s, e) {
        let url = e.data.help && e.data.help != '' ? e.data.help : '/help/Documents/index.html?page=' + encodeURIComponent(e.data.url);
        //父id,此处必须设置，用于判断该弹窗属于哪个选项卡
        $pagebox.create({
            pid: e.data.id, id: $api.md5(e.data.url),
            width: '800', height: '80%', ico: 'a026',
            url: url, title: e.data.title + ' - 帮助说明'
        }).open();
    });
    //风格切换事件
    window.$skins.onchange(function (s, e) {
        $dom('#loading').show();
        $dom('body>*:not(panel#loading)').css('opacity', 0);
        //$dom('body>*:not(panel#loading)').hide();   
    });
    window.$skins.onloadcss(function (s, e) {
        window.setTimeout(function () {
            $dom('body>*:not(panel#loading)').css('opacity', 1);
        }, 1000);
    });
    /*
        节点处理方法，后端数据库中存储的菜单数据，转为前端的树形菜单数据
    */
    function nodeconvert(obj) {
        var result = JSON.stringify(obj);
        var matchs = {
            'menuid': 'MM_Id', 'title': 'MM_Name', 'tit': 'MM_AbbrName', 'childs': 'children',
            'intro': 'MM_Intro', 'type': 'MM_Type', 'url': 'MM_Link', 'help': 'MM_Help',
            'use': 'MM_IsUse', 'width': 'MM_WinWidth',   // 弹窗相关  
            'height': 'MM_WinHeight', 'winid': 'MM_WinID', 'min': 'MM_WinMin', 'max': 'MM_WinMax',
            'move': 'MM_WinMove', 'resize': 'MM_WinResize', 'complete': 'MM_Complete'
        };
        Object.entries(matchs).forEach(([newKey, oldKey]) => {
            result = result.replace(new RegExp(oldKey, 'g'), newKey);
        });
        return JSON.parse(result);
    }
};
/* webdeskUI的自定义事件 */
window.$event = {
    //节点点击事件，tree,drop,统一用这一个
    'nodeClick': function (sender, eventArgs) {
        var data = $api.clone(eventArgs.data);
        //如果有下级节点，则不响应事件
        if ((!!data.childs && data.childs.length > 0) && data.type != 'node') return;
        //处理相对路径
        if (data.type != 'event' && data.url != ''
            && !(data.url.startsWith('/') || data.url.startsWith('http'))) {
            let href = window.location.pathname;
            if (!href.endsWith('/')) href += '/';
            data.url = href + data.url;
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
                window.tabs.clear();
                if (data.childs.length > 0) {
                    for (var i = 0; i < data.childs.length; i++) {
                        window.tabs.add(data.childs[i]);
                    }
                    window.tabs.focus(data.childs[0].id);
                }
                break;
            default:
                if (!!data.url) {
                    //window.tabs.clear();
                    //if (data.help == null || data.help == '') data.help = data.url;
                    window.tabs.add(data);
                }
                break;
        }
    },
    //选项卡关闭事件
    'tabsShut': function (sender, eventArgs) {
        let data = eventArgs.data;
        //获取当前标签生成的窗体
        let boxs = $ctrls.all('pagebox');
        let arr = new Array();
        for (let i = 0; i < boxs.length; i++) {
            if (boxs[i].obj.pid == data.id) {
                arr.push(boxs[i].obj);
                let childs = boxs[i].obj.getChilds();
                for (let j = 0; j < childs.length; j++)
                    arr.push(childs[j]);
            }
        }
        //关闭当前标签生成的窗体
        if (arr.length > 0) {
            if (confirm('当前选项卡“' + data.title + '”有 ' + arr.length + '个 窗体未关闭，\n是否全部关闭？')) {
                for (let i = 0; i < arr.length; i++) arr[i].shut();
                return true;
            }
            return false;
        }
        return true;
    },
    //选项卡切换事件
    'tabChange': function (sender, eventArgs) {
        //所有窗体
        let boxs = $ctrls.all('pagebox');
        //当前选项卡所产生的窗体
        let gettabsbox = (boxs, tabid) => {
            let arr = [];
            boxs.filter(el => el.obj.pid == tabid).forEach(item => {
                arr.push(item.obj);
                arr = arr.concat(item.obj.getChilds());
            });
            return arr.sort((a, b) => a.level - b.level);
        }
        //非当前选项卡的窗体(不包括其它控件生成的窗体)
        let getotherbox = (boxs, tabid, sender) => {
            let arr = [];
            sender.childs.filter(el => el.id != tabid).forEach(item => {
                arr = arr.concat(gettabsbox(boxs, item.id));
            });
            return arr;
        }
        //获取当前标签生成的窗体，全部还原
        gettabsbox(boxs, eventArgs.data.id).forEach(item => item.toWindow().focus(false));
        //非当前标签的窗体，全部最小化
        getotherbox(boxs, eventArgs.data.id, sender).forEach(item => item.toMinimize(false));

        //设置左侧树形菜单的当前菜单项
        window.tree.currentnode(eventArgs.tabid);
    }
}