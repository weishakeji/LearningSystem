// 学员的头像

Vue.component('avatar', {
    //account: 学员账号对象
    //teacher: 教师对象，
    //path:头像图片在服务端的路径
    //circle: 是否为圆形(true)，默认是方形(false)
    //size: 宽高值
    props: ['account', 'teacher', 'path', 'circle', 'size'],
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
            let type = $api.getType(this.circle);
            if (type == 'Boolean') return this.circle;
            if (type == 'String')
                return this.circle == 'true' ? true : false;
            return this.circle;
        },
        //学员对象是否存在
        'exist': t => !$api.isnull(t.account) ||  !$api.isnull(t.teacher) ,
        //头像图片的地址
        'photo': function () {
            if (this.exist) {
                if (!$api.isnull(this.account)) return this.account.Ac_Photo;
                if (!$api.isnull(this.teacher)) return this.teacher.Th_Photo;
                return '';
            }
        },
        //性别
        'gender': function () {
            if (this.exist) {
                if (!$api.isnull(this.account)) return this.account.Ac_Sex;
                if (!$api.isnull(this.teacher)) return this.teacher.Th_Sex;
                return 1;
            }
        }

    },
    created: function () {
        $dom.load.css(['/Utilities/Components/Styles/avatar.css']);
    },
    methods: {
        //头像的url路径
        photourl: function (photo) {
            if (photo == null || photo == '') return '';
            if (this.path != null && this.path != '') return this.path + photo;
            return photo;
        }
    },
    template: `<div :class="{'ws_avatar':true,'ws_circle':circle_val}" :style="{width:size+'px',height:size+'px'}">
        <div v-if="loading">loading...</div>
        <template v-if='exist'>
            <div v-if="photo!=''" :style="'background:url('+photourl(photo)+') no-repeat center'" class="ws_avatar_photo" ></div>
            <img v-else :src="gender==2 ? def.woman : def.man"/>    
        </template>         
    </div>`
});
