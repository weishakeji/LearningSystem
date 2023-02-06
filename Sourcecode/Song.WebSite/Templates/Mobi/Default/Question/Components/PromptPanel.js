//试题练习的入场提示面板
$dom.load.css([$dom.pagepath() + 'Components/Styles/PromptPanel.css']);
Vue.component('prompt_panel', {
    props: [],
    data: function () {
        return {
            showpanel: false,
            fadeout: false      //是否渐隐
        }
    },
    watch: {
        //当渐隐完成后，关闭面板
        'fadeout': function (nv, ov) {
            if (nv) {
                var th = this;
                window.setTimeout(function () {
                    th.showpanel = false;
                }, 1000);
            }
        }
    },
    mounted: function () {

    },
    methods: {
        show: function () {
            //当前课程下，只有第一次打开时，才显示手式提示面板
            var name = 'ques_prompt_' + $api.querystring('couid');
            var isfirst = $api.storage(name);
            if (isfirst == undefined) {
                this.showpanel = true;
                $api.storage(name, true);
            }

        }
    },
    template: `<div class="ws_prompt_panel" v-if="showpanel" :fadeout="fadeout" @click="fadeout=true">        
        <div>
            <icon>&#xe781</icon> 左滑，上一道题
        </div>
        <div>
            <icon>&#xe783</icon> 右滑，下一道题
        </div>

        <div>
            <icon>&#xe77a</icon> 扩张，字体放大
        </div>
        <div>
            <icon>&#xe778</icon> 收缩，字体变小
        </div>
    </div> `
});