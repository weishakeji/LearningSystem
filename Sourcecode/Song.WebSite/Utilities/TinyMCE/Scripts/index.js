
window.vapp = new Vue({
    el: '#vapp',
    data: {
        txt: '测试一下下'
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
        getcontext: function () {
            var txt = this.$refs.editor.getContent();
            alert(txt);
        }
    }
});

