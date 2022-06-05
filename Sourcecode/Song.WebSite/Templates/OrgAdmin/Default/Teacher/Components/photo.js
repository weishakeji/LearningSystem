// 教师的照片

Vue.component('photo', {
    //teacher: 账号对象
    //circle: 是否为圆形(true)，默认是方形(false)
    //size: 宽高值
    props: ['teacher', 'circle', 'size'],
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
        teacher: function (nv, ov) {
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
            return JSON.stringify(this.teacher) != '{}' && this.teacher != null;
        }
    },
    created: function () {
        $dom.load.css([$dom.pagepath() + 'Components/Styles/Photo.css']);
    },
    methods: {

    },
    template: `<div :class="{'ws_avatar':true,'ws_circle':circle_val}" :style="{width:size+'px',height:size+'px'}">
        <div v-if="loading">loading...</div>
        <template v-if='init'>
            <div v-if="teacher.Th_Photo!=''" :style="'background:url('+teacher.Th_Photo+') no-repeat center'" class="ws_avatar_photo" ></div>
            <img v-else :src="teacher.Th_Sex==2 ? def.woman : def.man"/>    
        </template>         
    </div>`
});
