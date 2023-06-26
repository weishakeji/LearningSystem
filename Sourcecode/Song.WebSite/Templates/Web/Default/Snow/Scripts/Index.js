$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项        
            datas: {},
            loading_init: true
        },
        mounted: function () {
            let arr = this.buildrandom(10, 1168);
            console.log(arr);
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //生成随机数，平均分布，且不重复
            buildrandom: function (count, length) {
                let part = count * 2 + 1;       //分成几段
                let len = Math.floor(length / part);   //每段多长
                let arr = [];
                for (let i = 0; i < count; i++) {
                    let random = Math.floor(Math.random() * len);                
                    arr.push(len * (i * 2 + 1) + random);
                }
                return arr;
            }
        }
    });

});
