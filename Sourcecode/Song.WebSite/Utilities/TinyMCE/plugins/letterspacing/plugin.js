
 /**
 * letterspacing 1.6v 2021-5-14
 * The tinymce-plugins is used to set the word spacing
 * 
 * https://github.com/Five-great/tinymce-plugins
 * 
 * Copyright 2020, Five(Li Hailong) The Chengdu, China https://www.fivecc.cn/
 *
 * Licensed under MIT
 */
tinymce.PluginManager.add('letterspacing', function(editor, url) {
    var pluginName='设置间距';
    var global$1 = tinymce.util.Tools.resolve('tinymce.util.Tools');
    var letterspacing_val = editor.getParam('letterspacing', '0px 1px 2px 4px 6px 8px 10px 20px 40px');
    editor.on('init', function() {
        editor.formatter.register({
            letterspacing: {
                inline: 'span',
                styles: { 'letter-spacing': '%value' },
            }
        });
    });
    var doAct = function (value) {
        upIndent2em(value);
    };
    function getChildren(curEle,tagName){
        if( curEle.nodeName.toLowerCase() === tagName){ return curEle;}
        var nodeList = curEle.childNodes;
        var ary = [];
        if(/MSIE(6|7|8)/.test(navigator.userAgent)){
            for(var i=0;i<nodeList.length;i++){
                var curNode = nodeList[i];
                if(curNode.nodeType ===1){
                   ary[ary.length] = curNode;
                }
            }
        }else{
            ary = Array.prototype.slice.call(curEle.children);
        }
        
        // 获取指定子元素
        if(typeof tagName === "string"){
         
            for(var k=0;k<ary.length;k++){
              curTag = ary[k];
              if(curTag.nodeName.toLowerCase() !== tagName.toLowerCase()){
               ary.splice(k,1);
               k--;
              }
            }
        }

        return ary[0];
  }
    function _indent2$getValue( key, str ) { 
        var m = str.match( new RegExp(key + ':?(.+?)"?[;}]') );
        return m ? m[ 1 ] : false;
    }
    function upIndent2em(value){
        var dom = editor.dom;
        var blocks = editor.selection.getSelectedBlocks();
        global$1.each(blocks, function(block) {
            if(dom.getStyle(block,'text-indent')){
                let kv = "";
                let kl = "";
                 if(block&&block.children['0']&&block.children['0'].attributes&&block.children['0'].attributes.style){
                   kv = _indent2$getValue('font-size',block.children['0'].attributes.style.textContent);
                   kl = value;
                   if(kv) {kv=(parseInt(kv)+parseInt((kl?kl:0)))*2+'px';}
                   else kv=(parseInt((kl?kl:0))+16)*2+'px';
                 }
                dom.setStyle(block, 'text-indent', kv?kv:'2em');
            }
         
        });
        editor.undoManager.transact(function(){
            editor.focus();
            editor.formatter.apply('letterspacing', { value: value });
        })
    }
    editor.ui.registry.getAll().icons.letterspacing || editor.ui.registry.addIcon('letterspacing','<svg t="1610616201691" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="969" width="24" height="24"><path d="M682.666667 704l128 106.666667-128 106.666666v-85.333333H341.333333v85.333333L213.333333 810.666667l128-106.666667v85.333333h341.333334v-85.333333zM170.666667 170.666667v682.666666H85.333333V170.666667h85.333334z m768 0v682.666666h-85.333334V170.666667h85.333334z m-394.666667 0l202.666667 469.333333h-89.6l-38.4-93.866667h-213.333334L366.933333 640H277.333333l202.666667-469.333333h64zM512 255.146667L432.213333 469.333333h159.573334L512 255.146667z" p-id="970" fill="#222f3e"></path></svg>');
    editor.ui.registry.addMenuButton('letterspacing', {
        icon: 'letterspacing',
        tooltip: pluginName,
        fetch: function(callback) {
            var dom = editor.dom;
            var block = editor.selection.getStart();
            var lhv = 0;
                block = getChildren(block,'span')
                if(lhv==0){
                    lhv = dom.getStyle(block,'letter-spacing') ? dom.getStyle(block,'letter-spacing') : 0;
                }
            var items = letterspacing_val.split(' ').map(function(item){
                var text = item;
                var value = item;
                return {
                    type: 'togglemenuitem',
                    text: text,
                    active : lhv==value ? true :false,
                    onAction: function() {
                        doAct(value);
                    }
                };
            });
            callback(items);
        }
    });

    return {
        getMetadata: function () {
            return  {
                name: pluginName,
                url: "https://github.com/Five-great/tinymce-plugins",
            };
        }
    };
});
