 //试题类型
 Vue.component('ques_type', {
     //showname：是否显示题型名称
    props: ["type", "types","showname"],
    data: function () {
        return {
            //单选，多选，判断，简答，填空
            icons: ['e653', 'e78b', 'e634', 'e801', 'e823']
        }
    },
    mounted: function () {
        $dom.load.css([$dom.path() + 'Question/Components/Styles/ques_type.css']);
    },
    template: `<icon ques_type v-html="'&#x'+icons[Number(type)-1]" :title="types[Number(type)-1]+'题'" :showname="showname"></icon> `
});