/**
 * layout 1.5v  2020-01-14
 * The tinymce-plugins is used to set up a layout
 * 
 * https://github.com/Five-great/tinymce-plugins
 * 
 * Copyright 2020, Five(Li Hailong) The Chengdu, China https://fivecc.cn/
 *
 * Licensed under MIT
 */
tinymce.PluginManager.add('layout', function(editor, url) {
    var pluginName='一键布局';
    var global$1 = tinymce.util.Tools.resolve('tinymce.util.Tools');
    var layout_opt = editor.getParam('layout_options', {clearStyle:[],filterTags:['table>*'],style:{'text-align':'justify','text-indent':'2em','line-height': 1.5},tagsStyle:{}});
    var layout_filterTags={};
    var layout_filterTagsRegex={};
    for( let key in layout_opt.filterTags){layout_opt.filterTags[key].indexOf('>*')!=-1?layout_filterTagsRegex[layout_opt.filterTags[key].replace('>*','').toUpperCase()]=true :layout_filterTags[layout_opt.filterTags[key].toUpperCase()]=true;}
    for( let key in layout_opt.tagsStyle){
        let ckeyList = key.split(',');
        for(let ckey in ckeyList)ckeyList[ckey].indexOf('>*')!=-1?layout_filterTagsRegex[ckeyList[ckey].replace('>*','').toUpperCase()]=key :layout_filterTags[ckeyList[ckey].toUpperCase()]=key;
    }
    var doAct = function () {
        var dom = editor.dom;
        editor.execCommand('selectAll');
        var blocks = editor.selection.getSelectedBlocks()
        function _indent2$getValue( key, str ) { 
            var m = str.match( new RegExp(key + ':?(.+?)"?[;}]') );
            return m ? m[ 1 ] : false;
        }
        function filterFun(el) {
            let parentSelector = 'BODY';
            let parents = el.tagName;
            if(layout_filterTags[parents] || layout_filterTagsRegex[parents]) {
                !layout_opt.tagsStyle[layout_filterTags[parents]]?'': setStyleFun(el,layout_opt.tagsStyle[layout_filterTags[parents]])
                return true;
            }
            let _p = el.parentNode;
            let _pName = _p.tagName
            while (_pName !== parentSelector) {
                let o = _p;
                parents = _pName + '>' + parents;
                if(layout_filterTags[parents] || layout_filterTagsRegex[_pName]) {
                    !layout_opt.tagsStyle[layout_filterTagsRegex[_pName]]?'': setStyleFun(el,layout_opt.tagsStyle[layout_filterTagsRegex[_pName]])
                    !layout_opt.tagsStyle[layout_filterTags[parents]]?'': setStyleFun(el,layout_opt.tagsStyle[layout_filterTags[parents]])
                    return true;
                }
                _p = o.parentNode;
                _pName = _p.tagName;
            }
            return false;
        }
        function clearStyleFun(_block){
            let style=dom.getAttrib(_block,'style');
            for(let key in layout_opt.clearStyle){
                let reg = new RegExp(layout_opt.clearStyle[key] + ':?(.+?)"?[;}]') 
                    style = style.replace(reg, '');
                }
                dom.setAttrib(_block,'style',style);
        }
        function removeStyleFun(_block){
            let style=dom.getAttrib(_block,'style');
            for(let key in layout_opt.style){
            let reg = new RegExp(key + ':?(.+?)"?[;}]') 
            style = style.replace(reg, '');
            }
            dom.setAttrib(_block,'style',style);
        }
        function setStyleFun(_block,_style){
            for(let key in _style){
               dom.setStyle(_block, key, _style[key]);
            }
            if(_style["text-indent"]){
            let kv = "",kl = "";
            if(_block&&_block.children['0']&&_block.children['0'].attributes&&_block.children['0'].attributes.style){
                kv = _indent2$getValue('font-size',_block.children['0'].attributes.style.textContent);
                kl = _indent2$getValue('letter-spacing',_block.children['0'].attributes.style.textContent);
                if(kv) {kv=(parseInt(kv)+parseInt((kl?kl:0)))*2+'px';}
                else kv=(parseInt((kl?kl:0))+16)*2+'px';
            }
            dom.setStyle(_block, 'text-indent', layout_opt.style['text-indent']&&layout_opt.style['text-indent']!='2em'?layout_opt.style['text-indent']: kv?kv:'2em');
           }
        }
        var layoutAct = '';
        global$1.each(blocks, function (block) {
                if(layoutAct==''){if(dom.hasClass(block,'layoutFV')){layoutAct =  'remove'; dom.removeClass(block,'layoutFV')}else{ layoutAct =  'add'; dom.addClass(block,'layoutFV')}}
                if( layoutAct =='add'){
                    !filterFun(block)?setStyleFun(block,layout_opt.style):'';
                    layout_opt.clearStyle?clearStyleFun(block):'';
                }else{
                    !filterFun(block)?removeStyleFun(block,layout_opt.style):'';
                }
        });
    };
    editor.ui.registry.getAll().icons.layout || editor.ui.registry.addIcon('layout','<svg t="1603868236215" class="icon" viewBox="0 0 1035 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="17720" width="20" height="20"><path d="M357.445818 285.358545L1005.730909 518.830545c21.76 8.192 32.628364 31.394909 24.471273 51.87491a40.634182 40.634182 0 0 1-21.748364 23.214545l-245.992727 110.592a80.034909 80.034909 0 0 0-42.135273 42.321455l-104.657454 249.856c-8.145455 20.48-32.616727 30.045091-53.003637 21.85309-10.868364-4.096-19.025455-13.661091-23.098182-24.576L305.792 337.221818a40.075636 40.075636 0 0 1 24.471273-51.874909c8.145455-4.096 17.664-4.096 27.182545 0z m8.145455-255.32509v99.67709c0 16.384-13.579636 30.033455-29.893818 30.033455-16.302545 0-29.905455-13.649455-29.905455-30.033455V30.021818C305.803636 13.649455 319.406545 0 335.709091 0c16.314182 0 29.905455 13.649455 29.905454 30.033455zM29.905455 303.104h99.211636c16.302545 0 29.905455 13.649455 29.905454 30.033455s-13.602909 30.045091-29.905454 30.04509H29.905455C13.591273 363.170909 0 349.521455 0 333.137455s13.591273-30.033455 29.905455-30.033455zM645.573818 66.897455l-144.058182 144.73309c-12.241455 12.288-29.905455 12.288-42.135272 0-12.229818-12.288-12.229818-30.045091 0-42.33309l144.058181-144.721455c12.229818-12.288 29.905455-12.288 42.135273 0 10.868364 10.926545 10.868364 30.033455 0 42.321455zM67.944727 20.48L212.014545 165.201455c12.241455 12.288 12.241455 30.045091 0 42.33309-12.218182 12.288-29.905455 12.288-42.123636 0L25.832727 62.801455c-12.241455-12.288-12.241455-30.033455 0-42.321455 10.868364-12.288 29.893818-12.288 42.123637 0z m149.515637 480.593455L73.402182 645.818182c-12.241455 12.288-29.905455 12.288-42.146909 0-12.218182-12.288-12.218182-30.045091 0-42.333091l144.058182-144.721455c12.241455-12.288 29.905455-12.288 42.146909 0 12.218182 12.288 12.218182 30.033455 0 42.321455z" style="width:20px; height:20px" p-id="17721"></path></svg>');
    editor.ui.registry.addToggleButton('layout', {
        icon: 'layout',
        tooltip: pluginName,
        onAction: function () {
            editor.undoManager.transact(function(){
                editor.focus();
                doAct();
             })
        }
    });
    editor.ui.registry.addMenuItem('layout', {
        text: pluginName,
        onAction: function() {
            editor.undoManager.transact(function(){
               editor.focus();
               doAct();
            })
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
