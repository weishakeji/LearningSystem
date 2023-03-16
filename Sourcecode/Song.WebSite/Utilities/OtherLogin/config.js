// 第三方登录的配置项
$dom.load.css(['/Utilities/OtherLogin/Styles/config.css']);
Vue.component('config', {
    props: ['isuse'],
    data: function () {
        return {
            items: [
                { name: 'QQ登录', tag: 'qq', icon: 'e82a', size: 16, width: 700, height: 400, obj: {} },
                { name: '微信登录', tag: 'weixin', icon: 'e730', size: 18, width: 700, height: 500, obj: {} },
                { name: '金碟.云之家', tag: 'yunzhijia', icon: 'e726', size: 18, width: 600, height: 500, obj: {} },
                { name: '郑州工商学院', tag: 'zzgongshang', icon: 'a006', size: 18, width: 600, height: 400, obj: {} }
            ],
            //配置项的数据记录，记录在数据库
            entities: [],
             //可用的项
            usable_items:[]
        }
    },
    watch: {},
    computed: {       
    },
    created: function () {
        var th = this;
        $api.get('OtherLogin/GetAll', { 'isuse': th.isuse }).then(function (req) {
            if (req.data.success) {
                th.entities = req.data.result;
                th.usable_items=th.get_usable_items();
            } else {
                console.error(req.data.exception);
                throw req.config.way + ' ' + req.data.message;
            }
        }).catch(function (err) {
            //alert(err);
            console.error(err);
        });
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
        //获取可用的项
        get_usable_items: function () {
            if (this.entities.length < 1) return [];
            var items = [];
            for (let i = 0; i < this.entities.length; i++) {
                const el = this.entities[i];
                for (let j = 0; j < this.items.length; j++) {
                    if (this.items[j].tag == el.Tl_Tag) {
                        this.items[j].obj = el;
                        items.push(this.items[j]);
                    }
                }
            }
            this.$emit('load',items);
            return items;
        },
        //刷新
        fresh: function (tag) {
            var th = this;
            $api.get('OtherLogin/GetObject', { 'tag': tag }).then(function (req) {
                if (req.data.success) {
                    var obj = req.data.result;                  
                    for (let i = 0; i < th.entities.length; i++) {
                        if (th.entities[i].Tl_Tag == obj.Tl_Tag) {                           
                            th.$set(th.entities, i, obj);
                        }
                    }                 
                    th.usable_items=th.get_usable_items();                   
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading_tag = '');
        }
    },
    template: `<div>
        <slot v-for="(item,index) in usable_items" 
            name="item" :item="item" :index="index" :img="logosrc(item)" :icon="icon(item)">
        </slot>
    </div>`
});
