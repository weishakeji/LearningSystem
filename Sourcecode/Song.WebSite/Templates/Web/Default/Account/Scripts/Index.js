document.oncontextmenu = function () {
    return false;
}
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            uid: $api.querystring('uid'),
            organ: {},
            config: {},      //当前机构配置项  
            account: {},        //当前登录的学员账号
            //图片文件
            upfile: null, //本地上传文件的对象   

            menus: [],      //菜单项            
            default_menu: {     //默认菜单项
                'MM_Type': 'item',
                'MM_Link': '/student/start'
            },
            select_menus: [],        //选中的菜单项
            tabName: '',         //右侧选项卡名称

            full: false,         //管理界面是否全屏

            loading_init: true,
            loading_photo: false,        //图片上传的状态
            loading_menu: false      //菜单加载状态
        },
        mounted: function () {
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                vapp.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果             
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;

                document.title += vapp.organ.Org_PlatformName;
            })).catch(function (err) {
                console.error(err);
            });
            this.getmenus();
        },
        created: function () {
            $dom.load.css([$dom.path() + 'styles/pagebox.css']);
        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
            //选项卡切换时，切换显示内容（iframe)
            'tabName': {
                handler: function (nv, ov) {
                    this.$nextTick(function () {
                        $dom("#content_iframe>iframe").hide();
                        var iframe = $dom('iframe#node_' + nv + '');
                        if (iframe.length) {
                            iframe.show();
                            iframe[0].onload = function (e) {
                                var doc = e.currentTarget.contentWindow.document;
                                doc.oncontextmenu = function () {
                                    return false;
                                };
                            }

                        }
                        //显示遮罩
                        $dom('.iframe_loading').show();
                        window.setTimeout(function () {
                            $dom('.iframe_loading').hide();
                        }, 300);
                    });
                }, immediate: true
            }
        },
        methods: {
            //更新图片
            updatePhoto: function () {
                var th = this;
                th.loading_photo = true;
                $api.post('Account/ModifyPhoto', { 'account': th.account, 'file': th.upfile }).then(function (req) {
                    th.loading_photo = false;
                    if (req.data.success) {
                        th.upfile = null;
                        var result = req.data.result;
                        th.$refs["pageheader"].account = result;
                        th.account = result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading_photo = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //加载左侧菜单树
            getmenus: function () {
                var th = this;
                th.loading_menu = true;
                $api.cache('ManageMenu/OrganMarkerMenus:60', { 'marker': 'student' })
                    .then(function (req) {
                        th.loading_menu = false;
                        if (req.data.success) {
                            var result = req.data.result;
                            if (result != null && result.length > 0
                                && (result[0].children && result[0].children.length > 0)) {
                                th.menus = result[0].children;
                            }
                            //通过地址栏的uid参数，显示当前选中的菜单项
                            var obj = th.uid != '' ? th.getmenu(th.uid, th.menus) : null;
                            //obj = obj == null ? th.default_menu : obj;
                            //默认打开第一个
                            if (obj == null) obj = th.menus[0];

                            var interval = window.setInterval(function () {
                                var name = 'menu_node';
                                if (th.$refs[name]) {
                                    if ($api.getType(th.$refs[name]) == 'Array')
                                        th.$refs[name][0].nodeEvent(obj);
                                    else th.$refs[name].nodeEvent(obj);
                                    window.clearInterval(interval);
                                }
                            }, 200);
                            /*
                            th.$nextTick(function () {
                                var name = 'menu_node';
                                if (th.$refs[name]) {
                                    if ($api.getType(th.$refs[name]) == 'Array')
                                        th.$refs[name][0].nodeEvent(obj);
                                    else th.$refs[name].nodeEvent(obj);
                                }
                            });*/
                        } else {
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        th.loading_menu = false;
                        console.error(err);
                    });
            },
            //通过uid获取当前菜单项
            getmenu: function (uid, menus) {
                var obj = null;
                for (let i = 0; i < menus.length; i++) {
                    if (menus[i].MM_UID == uid) {
                        obj = menus[i];
                        break;
                    }
                    if (obj == null)
                        obj = this.getmenu(uid, menus[i].children);
                }
                return obj;
            },
            //打开菜单节点
            openmenu: function (menu) {
                console.log(menu);
                //更改地址栏，以应对刷新浏览器保持当前菜单项
                var url = $api.setpara('uid', menu.MM_UID);
                history.pushState({}, "", url);
                //子菜单项
                var nodes = [];
                if (menu.MM_Type == 'item') nodes.push(menu);
                if (menu.MM_Type == 'node') {
                    nodes = item_childs(menu);
                }
                this.select_menus = nodes;
                if (this.select_menus.length > 0) this.tabName = this.select_menus[0].MM_UID;
                function item_childs(menu) {
                    var arr = [];
                    if (!!menu.children && menu.children.length > 0) {
                        for (let i = 0; i < menu.children.length; i++) {
                            if (menu.children[i].MM_Type == 'item')
                                arr.push(menu.children[i]);
                            arr.push.apply(item_childs(menu.children[i]));
                        }
                    }
                    return arr;
                }
            },
            //是否显示iframe(右侧选项卡下的iframe)
            isshow: function (tab) {
                if (tab == this.tabName) return true;
                //如果之前已经加载过的iframe，不重新加载
                var iframe = $dom('iframe#node_' + tab + '');
                if (iframe.length) return true;
            },
            //当前登录账号变更
            accountChange: function (acc) {
                console.log(acc.Ac_Name);
                this.$refs["pageheader"].account = acc;
            },
            //打开窗体
            open: function (url, title, name, width, height, icon) {
                var obj = {};
                var shutevent = null;      // 窗体关闭事件
                if ($api.getType(url) == "Object") {
                    obj = url;
                    if (arguments.length > 1) {
                        var func = arguments[1];
                        if ($api.getType(func) == 'Function')
                            shutevent = func;
                    }
                } else {
                    obj = {
                        'url': url, 'ico': icon,
                        'pid': name,
                        'title': title,
                        'width': width,
                        'height': height
                    }
                }
                obj['showmask'] = true; //始终显示遮罩
                obj['min'] = false;

                var box = $pagebox.create(obj);
                if(shutevent!=null)box.onshut(shutevent);
                box.open();
            },
            //关闭窗口，并执行方法
            shut: function (name, func) {
                var box = $pagebox.get(name);
                if (box == null) return;
                var iframe = $dom('iframe[name="' + box.pid + '"]');
                iframe = iframe.length > 0 ? iframe[0] : null;
                if (iframe == null) return;
                var win = iframe.contentWindow;
                //刷新父页面数据
                if (win && func != null) {
                    if (func.charAt(func.length - 1) == ')') { eval('win.' + func); }
                    else {
                        var f = eval('win.' + func);
                        if (f != null) f();
                    }
                }
                window.setTimeout(function () {
                    $pagebox.shut(name);
                }, 1000);
            }
        }
    });
    //菜单项组件
    Vue.component('menu_node', {
        //menu:菜单项
        //level:菜单深度
        props: ["menu", "level"],
        data: function () {
            return {
                //菜单项样式
                styleObject: {
                    'padding-left': this.level * 20 + 'px',
                    'font-weight': this.menu.MM_IsBold ? 'bold' : 'normal',
                    'font-style': this.menu.MM_IsItalic ? 'italic' : 'normal',
                    'color': this.menu.MM_Color != '' ? this.menu.MM_Color : ''
                }
            }
        },
        watch: {
            'menu': {
                handler: function (nv, ov) {

                }, immediate: true
            }
        },
        computed: {},
        mounted: function () {
        },
        methods: {
            //节点的点击事件
            nodeClick: function () {
                var menu = this.menu;
                this.nodeEvent(menu);
            },
            //节点的事件
            nodeEvent: function (menu) {
                var ischildren = menu.MM_Type != 'node' && (menu.children && menu.children.length > 0);
                if (ischildren) {
                    $dom("div[mmid='" + menu.MM_Id + "']>div").toggle();
                    var node = $dom("div[mmid='" + menu.MM_Id + "']>weisha_menu_node");
                    if (node.hasClass("fold")) {
                        node.removeClass("fold");
                    } else {
                        node.addClass("fold");
                    }
                } else {
                    //菜单节点事件
                    if (menu.MM_Type == 'item' || menu.MM_Type == 'node') {
                        $dom("weisha_menu_node").removeClass("current");
                        $dom("div[mmid='" + menu.MM_Id + "']>weisha_menu_node").addClass("current");
                        this.$emit('open', menu);
                    }
                    //javascript事件
                    if (menu.MM_Type == 'event') {
                        var event = eval(menu.MM_Link);
                        if (event != null) event();
                    }
                    //打开窗体
                    if (menu.MM_Type == 'open') {
                        var result = JSON.stringify(menu);
                        result = result.replace(/MM_IcoS/g, "ico");
                        result = result.replace(/MM_Link/g, "url");
                        result = result.replace(/MM_WinWidth/g, "width");
                        result = result.replace(/MM_WinHeight/g, "height");
                        result = result.replace(/MM_WinMin/g, "min");
                        result = result.replace(/MM_WinMax/g, "max");
                        result = result.replace(/MM_WinMove/g, "move");
                        result = result.replace(/MM_WinResize/g, "resize");
                        var obj = JSON.parse(result);
                        //当有窗体标识，则替代id
                        if (obj.MM_WinID && obj.MM_WinID != "") obj.id = obj.MM_WinID;
                        if (obj.ico == '') delete obj['ico'];
                        if (obj.width <= 0) delete obj['width'];
                        if (obj.height <= 0) delete obj['height'];
                        obj['showmask'] = true; //始终显示遮罩
                        $pagebox.create(obj).open();
                    }
                }
            }
        },
        // 同样也可以在 vm 实例中像 "this.message" 这样使用
        template: `<div :mmid="menu.MM_Id">
            <weisha_menu_node :type="menu.MM_Type" v-if="menu.MM_Type!='hr'"
            @click="nodeClick" :style="styleObject" 
            :class="{'children':menu.MM_Type!='node' && menu.children.length>0}">
                <icon v-if="menu.MM_IcoS==''">&#xa038</icon>
                <icon v-html="'&#x'+menu.MM_IcoS" v-else></icon>
                <a v-if="menu.MM_Type=='link'" :href="menu.MM_Link" target="_blank">{{menu.MM_Name}}</a>       
                <span v-else>{{menu.MM_Name}}</span>
            </weisha_menu_node>
            <div v-else :style="styleObject" class="hr"></div>
            <menu_node v-for="(node,index) in menu.children" :level="level+1" @open="$listeners['open']" :menu="node" v-if="menu.MM_Type!='node'"></menu_node>
        </div>`
    });
}, ['../scripts/pagebox.js',
    '/Utilities/Components/upload-img.js']);
