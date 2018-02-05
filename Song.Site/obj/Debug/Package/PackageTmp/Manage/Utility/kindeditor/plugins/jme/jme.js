KindEditor.lang({
	jme : '插入公式'
});
KindEditor.plugin('jme', function(e){
	var editor = this, name = 'jme';
    editor.clickToolbar(name, function() {
    	var dialog = editor.createDialog({
    		name : 'jme',
	        width : 400,
	        height : 400,
	        title : editor.lang(name),
	        body : '<div style="width:400px;height:400px;">' +
	        	'<iframe id="math_frame" name="math_frame" style="width:400px;height:400px;" frameborder="no" src="' 
	        	+ KindEditor.basePath + 'plugins/jme/dialogs/mathdialog.html"></iframe></div>',
	        	
	        closeBtn : {
                name : '关闭',
                click : function(e) {
                        dialog.remove();
                }
	        },
	        yesBtn : {
                name : editor.lang('yes'),
                click : function(e) {
	        		var thedoc = document.frames ? document.frames['math_frame'].document : getIFrameDOM("math_frame");
		        	var mathHTML = '<span class="mathquill-rendered-math" style="font-size:' 
		        		+ '20px' + ';" >' + $("#jme-math",thedoc).html() + '</span><span>&nbsp;</span>';
		        	
		        	editor.insertHtml(mathHTML).hideDialog().focus();
		        	//dialog.hideDialog();
		        	return;		        	
                }
	        }
    	});
    });
});

function getIFrameDOM(fid){
	var fm = getIFrame(fid);
	return fm.document||fm.contentDocument;
}
function getIFrame(fid){
	return document.getElementById(fid)||document.frames[fid];
}
