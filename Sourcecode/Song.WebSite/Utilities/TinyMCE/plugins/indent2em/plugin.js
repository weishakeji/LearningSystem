/**
 * indent2em (Enhancement 1.5v) 2021-01-13
 * The tinymce-plugins is used to set the first line indent (Enhancement)
 * 
 * https://github.com/Five-great/tinymce-plugins
 * 
 * Copyright 2020, Five(Li Hailong) The Chengdu, China https://www.fivecc.cn/
 *
 * Licensed under MIT
 */
tinymce.PluginManager.add('indent2em', function(editor, url) {
    var pluginName='首行缩进';
    var global$1 = tinymce.util.Tools.resolve('tinymce.util.Tools');
    var indent2em_val = editor.getParam('indent2em_val', '2em');
    editor.on('init', function() {
        editor.formatter.register({
            indent2em: {
                selector: 'p,h1,h2,h3,h4,h5,h6,td,th,div,ul,ol,li,table',
                styles: { 'text-indent': '%value' },
            }
        });
    });
    function _indent2$getValue( key, str ) { 
        var m = str.match( new RegExp(key + ':?(.+?)"?[;}]') );
        return m ? m[ 1 ] : false;
    }
    var doAct = function () {
        editor.undoManager.transact(function(){
            editor.focus();
            var dom = editor.dom;
            var blocks = editor.selection.getSelectedBlocks();
            var act = '';
            global$1.each(blocks, function (block) {
                let kv = "";
                let kl = "";
                if(block&&block.children['0']&&block.children['0'].attributes&&block.children['0'].attributes.style){
                    kv = _indent2$getValue('font-size',block.children['0'].attributes.style.textContent);
                    kl = _indent2$getValue('letter-spacing',block.children['0'].attributes.style.textContent);
                    if(kv) {kv=(parseInt(kv)+parseInt((kl?kl:0)))*2+'px';}
                    else kv=(parseInt((kl?kl:0))+16)*2+'px';
                }
                if(act==''){
                    act = dom.getStyle(block,'text-indent') == (indent2em_val!='2em'?indent2em_val:kv?kv:'2em') ? 'remove' : 'add';
                }
                if( act=='add'){
                    dom.setStyle(block, 'text-indent',indent2em_val!='2em'?indent2em_val:kv?kv:'2em');
                }else{
                    var style=dom.getAttrib(block,'style');
                    var reg = new RegExp('text-indent?(.+?)"?[;}]', 'ig');
                    style = style.replace(reg, '');
                    dom.setAttrib(block,'style',style);
                }
            });
        });
    };
    editor.ui.registry.getAll().icons.indent2em || editor.ui.registry.addIcon('indent2em','<svg viewBox="0 0 1024 1024" xmlns="http://www.w3.org/2000/svg" width="24" height="24"><path d="M170.666667 563.2v-102.4H887.466667v102.4zM170.666667 836.266667v-102.4H887.466667v102.4zM512 290.133333v-102.4H887.466667v102.4zM238.933333 341.333333V136.533333l204.8 102.4z" fill="#2c2c2c" p-id="5210"></path></svg>');

    var stateSelectorAdapter = function (editor, selector) {
      return function (buttonApi) {
        return editor.selection.selectorChangedWithUnbind(selector.join(','), buttonApi.setActive).unbind;
      };
    };
    editor.ui.registry.addToggleButton('indent2em', {
        icon: 'indent2em',
        tooltip: pluginName,
        onAction: function () {
            doAct();
        },
        onSetup: stateSelectorAdapter(editor, [
          '*[style*="text-indent"]',
          '*[data-mce-style*="text-indent"]',
        ])
    });
    editor.ui.registry.addMenuItem('indent2em', {
        text: pluginName,
        onAction: function() {
            doAct();
        }
    });
    editor.addCommand('indent2em', doAct  );
    return {
        getMetadata: function () {
            return  {
                name: pluginName,
                url: "https://github.com/Five-great/tinymce-plugins",
            };
        }
    };
});
