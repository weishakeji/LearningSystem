$ready([], function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            total: 100,          //总题数
            index:0,          //当前试题索引
            loadstate: {
                init: false,        //初始化
                def: false,         //默认
                get: false,         //加载数据
                update: false,      //更新数据
                del: false          //删除数据
            }
        },
        mounted: function () {
            var th = this;
            window.setInterval(function () {
                let el = th.$el;
                console.log($dom(el).width());
            }, 1000);

        },
        created: function () {

        },
        computed: {
            loading: function () {
                if (!this.loadstate) return false;
                for (let key in this.loadstate) {
                    if (this.loadstate.hasOwnProperty(key)
                        && this.loadstate[key])
                        return true;
                }
                return false;
            },

        },
        watch: {

        },
        methods: {
            //试题滑动 
            swipe: function (e) {
                if (e) {
                    if (e.preventDefault) e.preventDefault();
                    //let node = $dom(e.target ? e.target : e.srcElement);
                    //if (node.length > 0 && (node.hasClass("van-overlay") || node.hasClass("van-popup"))) return;
                }
                //向左滑动
                if (e.direction == 2 && this.index < this.total) this.index++;
                //向右滑动
                if (e.direction == 4 && this.index > 0) this.index--;
                $dom('section').css('left', -this.index *100 + 'vw');
                //触发滑动事件,返回当前索引
                //this.$emit('swipe', this.index);
            },
        },
        filters: {

        },
        components: {

        }
    });
});