(function() {
    //所有风格
    var list = top.window.$skins.list;
    //按名称排序
    list.sort(function(a, b) {
        return a.tag > b.tag;
    });
    //当前风格
    var curr = top.window.$skins.current();
    //生成页面效果
    for (var i = 0; i < list.length; i++) {
        var skin = list[i];
        var box = $dom('.skins').add('skin');
        box.attr('tag', skin.tag);
        box.add('name').html(skin.name);
        box.add('img').attr('src', skin.path + '/logo.jpg');
        if (skin.tag.toLowerCase() == curr.toLowerCase()) {
            box.addClass('curr');
        }
        //点击切换风格
        box.click(function(event) {
            var n = event.target ? event.target : event.srcElement;
            while (n.tagName.toLowerCase() != 'skin') n = n.parentNode;
            var box = $dom(n);
            //当前点击的风格名称
            var tag = box.attr('tag').toLowerCase();
            //当前使用的风格
            var curr = top.window.$skins.current();
            if (curr == tag && !top.window.$skins.isnight()) return;
            //切换风格
            $dom('skin').removeClass('curr');
            box.addClass('curr');
            top.window.$skins.setup(tag);
        });
    }
    //console.log(list);
})();