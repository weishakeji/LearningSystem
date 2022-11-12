tinymce.PluginManager.add('imageupload', function(editor, url) {
	var pluginName='图片上传';
	var baseURL=tinymce.baseURL;
	var iframe1 = baseURL+'/plugins/imageupload/index.html';
	var bdmap_width = function (editor) {
		return editor.getParam('bdmap_width', 560);
    };
    var bdmap_height = function (editor) {
		return editor.getParam('bdmap_height', 362);
    };
	window.tinymceLng='';
	window.tinymceLat='';
	var openDialog = function() {
		return editor.windowManager.openUrl({
			title: pluginName,
			size: 'large',
			//width: 800,
			//height: 500,
			url:iframe1,
			buttons: [
				{
					type: 'cancel',
					text: 'Close'
				},
				{
					type: 'custom',
					text: 'Save',
					name: 'save',
					primary: true
				},
			],
			onAction: function (api, details) {
				switch (details.name) {
					case 'save':
						html='<iframe src="'+baseURL+'/plugins/bdmap/bd.html?center='+tinymceLng+'%2C'+tinymceLat+'&zoom=19&width='+(bdmap_width(editor)-2)+'&height='+(bdmap_height(editor)-2)+'" frameborder="0" style="width:'+bdmap_width(editor)+'px;height:'+bdmap_height(editor)+'px;">';
						editor.insertContent(html);
						api.close();
						break;
					default:
						break;
				}

			}
		});
	};

	editor.ui.registry.getAll().icons.imageupload || editor.ui.registry.addIcon('imageupload','<svg t="1668240314007" class="icon" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="18562" width="22" height="22"><path d="M853.344 341.344C853.344 294.4 814.944 256 768 256s-85.344 38.4-85.344 85.344 38.4 85.344 85.344 85.344 85.344-38.4 85.344-85.344z" p-id="18889"></path><path d="M0 85.344v853.344h512v-85.344H85.344V742.4l256-256L512 657.056l59.744-59.744-230.4-230.4-256 256V170.656h853.344v298.656l85.344 85.344V85.312z" p-id="18890"></path><path d="M951.456 840.544L1011.2 780.8l-200.544-200.544-200.544 200.544 59.744 59.744L768 742.4v238.944h85.344V742.4z" p-id="18891"></path></svg>');

	editor.ui.registry.addButton('imageupload', {
    icon: 'imageupload',
    tooltip: pluginName,
		onAction: function() {
			openDialog();
		}
	});
	editor.ui.registry.addMenuItem('imageupload', {
		text: pluginName,
		onAction: function() {
			openDialog();
		}
	});
	return {
		getMetadata: function() {
			return  {
				name: pluginName,
				url: "http://tinymce.ax-z.cn/more-plugins/bdmap.php",
			};
		}
	};
});
