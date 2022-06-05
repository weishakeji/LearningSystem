// 学员的头像

Vue.component('avatar', {
    //account: 学员账号对象
    //circle: 是否为圆形(true)，默认是方形(false)
    //size: 宽高值
    props: ['account', 'circle', 'size'],
    data: function () {
        return {
            //默认图片
            def: {
                'man': '/Utilities/Images/head1.jpg',
                'woman': '/Utilities/Images/head2.jpg'
            },
            loading: false           
        }
    },
    watch: {
        account: function (nv, ov) {
            //if (nv) this.init = true;
        }
    },
    computed: {
        //是否为圆形
        'circle_val': function () {
            var type = $api.getType(this.circle);
            //console.log(type);
            if (type == 'Boolean') return this.circle;
            if (type == 'String') {
                return this.circle == 'true' ? true : false;
            }
            return this.circle;
        },
        'init': function () {
            return JSON.stringify(this.account) != '{}' && this.account != null;
        }
    },
    created: function () {
        $dom.load.css(['/Utilities/Components/Styles/avatar.css']);
    },
    methods: {

    },
    template: `<div :class="{'ws_avatar':true,'ws_circle':circle_val}" :style="{width:size+'px',height:size+'px'}">
        <div v-if="loading">loading...</div>
        <template v-if='init'>
            <div v-if="account.Ac_Photo!=''" :style="'background:url('+account.Ac_Photo+') no-repeat center'" class="ws_avatar_photo" ></div>
            <img v-else :src="account.Ac_Sex==2 ? def.woman : def.man"/>    
        </template>         
    </div>`
});
