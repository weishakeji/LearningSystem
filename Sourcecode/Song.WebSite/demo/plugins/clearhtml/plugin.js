tinymce.PluginManager.add('clearhtml', function(editor, url) {
    var pluginName='转文本格式';
    var global$1 = tinymce.util.Tools.resolve('tinymce.util.Tools');
   
    var indent2em_val = editor.getParam('indent2em_val', '2em');
    var doAct = function () {
        var dom = editor.dom;
        var blocks = editor.selection.getSelectedBlocks();
        var act = '';
        var getText = editor.getContent({ format: 'text' });
        // console.log(blocks)
        // console.log(getText)

        editor.setContent(getText)
        // global$1.each(blocks, function (block) {
        //     if(act==''){
        //         act = dom.getStyle(block,'text-indent')==indent2em_val ? 'remove' : 'add';
        //     }
        //     if( act=='add' ){
        //         dom.setStyle(block, 'text-indent', indent2em_val);
        //     }else{
        //         var style=dom.getAttrib(block,'style');
        //         var reg = new RegExp('text-indent:[\\s]*' + indent2em_val + ';', 'ig');
        //         style = style.replace(reg, '');
        //         dom.setAttrib(block,'style',style);
        //     }

        // });
    };

    editor.ui.registry.getAll().icons.clearhtml || editor.ui.registry.addIcon('clearhtml','<svg t="1603779600284" class="icon" viewBox="0 0 1024 1024"  version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="7514" width="20" height="20"><path d="M332.799002 686.081014m-332.799002 0a332.799002 332.799002 0 1 0 665.598003 0 332.799002 332.799002 0 1 0-665.598003 0Z" fill="#F4EFC9" p-id="7515"></path><path d="M883.19735 1024h-639.99808A141.055577 141.055577 0 0 1 102.399693 883.200422v-742.397772A141.055577 141.055577 0 0 1 243.19927 0.003072h516.350451a89.087733 89.087733 0 0 1 63.231811 25.599923l189.695431 189.695431A38.399885 38.399885 0 0 1 1023.996928 243.202342v639.99808a141.055577 141.055577 0 0 1-140.799578 140.799578zM243.19927 76.802842A63.999808 63.999808 0 0 0 179.199462 140.80265v742.397772A63.999808 63.999808 0 0 0 243.19927 947.20023h639.99808a63.999808 63.999808 0 0 0 63.999808-63.999808V259.074295l-179.199462-179.199463a12.799962 12.799962 0 0 0-8.447975-3.07199z" fill="#434260" p-id="7516"></path><path d="M366.846899 428.801786h-66.303801v-29.695911h168.447495v29.695911h-66.5598v196.095411h-35.583894zM541.182376 508.417547l-60.671818-109.311672h39.679881l27.391918 52.735841c5.631983 10.495969 10.495969 20.479939 17.151949 34.047898h1.535995c5.887982-13.567959 10.239969-23.551929 15.359954-34.047898l25.599923-52.735841h37.375888l-60.671818 111.103666 65.023805 114.943656h-38.655884l-29.695911-56.063832-18.687944-36.86389c-6.399981 13.823959-12.031964 25.599923-17.407948 36.86389l-28.927913 56.063832h-39.167882zM723.709829 428.801786h-66.303801v-29.695911h168.447494v29.695911h-66.5598v196.095411h-35.583893z"  p-id="7517"></path></svg>');

    editor.ui.registry.addToggleButton('clearhtml', {
        icon: 'clearhtml',
        tooltip: pluginName,
        onAction: function () {
            doAct();
        }
    });

    editor.ui.registry.addMenuItem('clearhtml', {
        text: pluginName,
        onAction: function() {
            doAct();
        }
    });

    // editor.addCommand('indent2em', doAct  );

    return {
        getMetadata: function () {
            return  {
                name: pluginName,
                url: "http://tinymce.ax-z.cn/more-plugins/indent2em.php",
            };
        }
    };
});
