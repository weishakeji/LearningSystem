//个人中心的头部
$dom.load.css([$dom.pagepath() + 'Components/Styles/account_header.css']);
Vue.component('account_header', {
    props: ['account'],
    data: function () {
        return {
            loading: false
        }
    },
    watch: {
    },
    computed: {
        //是否登录
        islogin: t => !$api.isnull(t.account),
        //是不是首页
        isindex: t => $dom('meta[view]').attr("view").toLowerCase() == 'index'
    },
    mounted: function () { },
    methods: {
        goback: function () {
            let url = this.commonaddr('myself');
            this.$dialog.confirm({
                title: '返回个人中心',
                message: '是否继续？',
            }).then(function () {
                window.navigateTo(url);
            }).catch(function () { });
        }
    },
    template: `<div class="header_info">
       <template  v-if="!islogin" remark="未登录">
            <div class="acc_photo nophoto"></div>
            <div class="accInfo">
                <div class="acc-name"> <a :href="commonaddr('signin')">未登录 </a>
                </div>
                <span class="acc-money"> ... </span>
            </div>
       </template>
       <template  v-else remark="已经登录">
            <avatar :account="account" circle="true" size="58"></avatar>
            <div class="accInfo">           
                <icon :woman="account.Ac_Sex==2" :man="account.Ac_Sex==1">
                    <span v-html='account.Ac_Name' v-if="account.Ac_Name!=''"></span>
                    <span v-else class="noname">(没有名字)</span>
                </icon>
                <span class="acc-acname">账号：{{account.Ac_AccName}}</span>
            </div>
            <div class="navi">
                <icon v-if="isindex">&#xe777</icon>
                <icon v-else class="back" @click="goback">&#xe748</icon>
            </div>
       </template>
    </div>`
});