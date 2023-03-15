
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            loading: false
        },
        created: function () {

        },
        methods: {
            btnEnter: function (formName) {
                this.operateSuccess();
            },
            //操作成功
            operateSuccess: function () {
                if (window.top.$pagebox)
                    window.top.$pagebox.source.tab(window.name, 'vapp.reload', false);
            }
        }
    });

});


