$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            pid: $api.querystring('pi'),    //接口id
            serial: $api.querystring('serial'), //流水号

            account: {},      //当前账户
            orgin: {},           //当前机构
            interface: {},       //支付接口
            moneyAccount: {},       //账单

            notify_url: '',      //成功后的回调地址
            pay_url: '',         //支付地址

            loading: true,
            loading_qr: false //二维码加载
        },
        mounted: function () {
            
        },
        created: function () {

        },
        computed: {
          
        },
        watch: {

        },
        methods: {
            
        }
    });

});
