tinymce.PluginManager.add('image-weisha', function (editor, url) {
	var pluginName = '图片上传/选择';
	var baseURL = tinymce.baseURL;
	var openpage = baseURL + '/plugins/image-weisha/index.html';
	var editorid = editor.id;
	var uploadkey = function (editor) {
		return editor.getParam('upload_key', '');
	};
	var dataid = function (editor) {
		return editor.getParam('dataid', 0);
	};

	var openDialog = function (param) {
		dataid = dataid == null ? 0 : dataid;
		var url = openpage + '?uploadkey=' + uploadkey(editor) + '&dataid=' + dataid(editor) + '&editorid=' + editorid;
		console.log(url);
		return editor.windowManager.openUrl({
			title: pluginName,
			size: 'large',
			width: 720,
			height: 400,
			url: param ? url + '&param=' + param : url,
			buttons: [
			],
			onAction: function (api, details) {
				switch (details.name) {
					case 'save':
						html = '返回的Html';
						console.log(html);
						editor.insertContent(html);
						api.close();
						break;
					default:
						break;
				}

			}
		});
	};

	editor.ui.registry.getAll().icons.imageweisha || editor.ui.registry.addIcon('imageweisha', '<svg t="1668240314007" class="icon" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="18562" width="21" height="21"><path d="M853.344 341.344C853.344 294.4 814.944 256 768 256s-85.344 38.4-85.344 85.344 38.4 85.344 85.344 85.344 85.344-38.4 85.344-85.344z" p-id="18889"></path><path d="M0 85.344v853.344h512v-85.344H85.344V742.4l256-256L512 657.056l59.744-59.744-230.4-230.4-256 256V170.656h853.344v298.656l85.344 85.344V85.312z" p-id="18890"></path><path d="M951.456 840.544L1011.2 780.8l-200.544-200.544-200.544 200.544 59.744 59.744L768 742.4v238.944h85.344V742.4z" p-id="18891"></path></svg>');

	editor.ui.registry.addButton('image-weisha', {
		icon: 'imageweisha',
		tooltip: pluginName,
		onAction: function () {
			openDialog();
		}
	});
	editor.ui.registry.addMenuItem('image-weisha', {
		text: pluginName,
		onAction: function () {
			openDialog();
		}
	});
	return {
		getMetadata: function () {
			return {
				name: pluginName,
				url: "http://tinymce.ax-z.cn/more-plugins/bdmap.php",
			};
		}
	};
});
//接收来自内容页的调用
//id:编辑器的id
//img:图片对象，包括路径、宽高、等信息
window.image_weisha_action = function (id, img) {
	var editor = tinyMCE.editors[id];
	if (img != null) {
		var style = '';
		if (img.wd != null) style += 'width:' + img.wd + 'px;';
		if (img.hg != null) style += 'height:' + img.hg + 'px;';
		if (style != '') style = 'style="' + style + '" ';
		if (img.alt == null) img.alt = '';
		var txt = '<img src="' + img.full + '" ' + style + ' alt="' + img.alt + '"/>';
		editor.insertContent(txt);
		editor.windowManager.close();
	}
	console.log(img);
}
