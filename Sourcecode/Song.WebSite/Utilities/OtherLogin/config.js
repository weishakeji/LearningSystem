// 第三方登录的配置项
$dom.load.css(['/Utilities/OtherLogin/Styles/config.css']);
Vue.component('config', {
    props: ['isuse'],
    data: function () {
        return {
            items: [
                { name: 'QQ登录', tag: 'qq', icon: 'e82a', size: 16, width: 700, height: 400, obj: {} },
                { name: '微信登录', tag: 'weixin', icon: 'e730', size: 18, width: 700, height: 500, obj: {} },
                { name: '金蝶.云之家', tag: 'yunzhijia', icon: 'e726', size: 18, width: 600, height: 500, obj: {} },
                { name: '郑州工商学院', tag: 'zzgongshang', icon: 'a006', size: 18, width: 600, height: 400, obj: {} }
            ],
            //配置项的数据记录，记录在数据库
            entities: [],
            //可用的项
            usable_items: []
        }
    },
    watch: {},
    computed: {
    },
    created: function () {
        this.get_all_items();
    },
    methods: {
        //图标地址
        logosrc: function (item) {
            return '/Utilities/OtherLogin/Images/' + item.tag + '.png';
        },
        //图标
        icon: function (item) {
            return '<icon style="font-size:"' + item.size + 'px;">&#x' + item.icon + '</icon>';
        },
        //获取所有的项
        get_all_items: function () {
            var th = this;
            th.usable_items = [];
            $api.get('OtherLogin/GetAll', { 'isuse': th.isuse }).then(function (req) {
                if (req.data.success) {
                    th.entities = req.data.result;
                    th.usable_items = th.get_usable_items();

                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                //alert(err);
                console.error(err);
            }).finally(() => {
                th.usable_items = th.get_usable_items();
            });
        },
        //获取可用的项
        get_usable_items: function () {
            //if (this.entities.length < 1) return [];
            var items = [];
            //将数据库中的记录，保存到配置项的obj属性
            for (let i = 0; i < this.entities.length; i++) {
                const el = this.entities[i];
                for (let j = 0; j < this.items.length; j++) {
                    if (this.items[j].tag == el.Tl_Tag) {
                        this.items[j].obj = el;
                        items.push(this.items[j]);
                    }
                }
            }
            //显示所有
            if (this.isuse == undefined || this.isuse == null) {
                for (let j = 0; j < this.items.length; j++) {
                    const el = this.items[j];
                    let index = this.entities.findIndex(t => t.Tl_Tag == el.tag);
                    if (index < 0) items.push(this.items[j]);
                }
            }           
            //如果简要名称没有，则显示配置项的名称
            for (let i = 0; i < items.length; i++) {
                if (items[i].obj.Tl_Name == '')
                    items[i].obj.Tl_Name = items[i].name;
            }
            this.$emit('load', items);
            return items;
        },
        //刷新
        fresh: function (tag) {
            this.get_all_items();          
        }
    },
    template: `<div v-if="usable_items.length>0">
        <slot v-for="(item,index) in usable_items" 
            name="item" :item="item" :index="index" :img="logosrc(item)" :icon="icon(item)">
        </slot>
    </div>`
});
