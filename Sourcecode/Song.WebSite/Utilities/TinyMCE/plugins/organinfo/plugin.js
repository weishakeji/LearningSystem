tinymce.PluginManager.add('organinfo', function (editor, url) {
	var pluginName = '机构信息';
	var baseURL = tinymce.baseURL;
	var editorid = editor.id;
	//console.error(editor);
	var iframe1 = baseURL + '/plugins/organinfo/index.html?editorid=' + editorid;
	var openDialog = function () {
		return editor.windowManager.openUrl({
			title: '机构信息',
			size: 'large',
			width: 500,
			height: 300,
			url: iframe1,
			buttons: [

			],
			onAction: function (api, details) {
				switch (details.name) {
					case 'save':
						html = '1';
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

	editor.ui.registry.getAll().icons.organinfo || editor.ui.registry.addIcon('organinfo', '<svg t="1668240314007" class="icon" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="18562" width="20" height="20"><path d="M888.5 404.4h-753c-64.4 0-74.1-43.1-74.1-68.8v-65.3c0-32.8 20.8-62.2 53-74.9l364.1-143c21.3-8.4 45.7-8.4 66.9 0l364.1 143c32.2 12.6 53 42 53 74.9v65.3c0.1 25.7-9.6 68.8-74 68.8z m-745.1-81.9h737.3v-50L515.5 128.7c-2.1-0.8-4.9-0.8-7 0l-364.1 143c-0.7 0.4-0.9 0.3-1.1 0.1v50.7zM493.5 90.6h0.2-0.2zM901.1 977.8H122.9c-33.9 0-61.4-27.6-61.4-61.4v-108c0-33.9 27.6-61.4 61.4-61.4h778.2c33.9 0 61.4 27.6 61.4 61.4v108c0.1 33.9-27.5 61.4-61.4 61.4z m-757.7-81.9h737.3v-67H143.4v67z" p-id="56053"></path><path d="M184.3 363.4h81.9v430.1h-81.9zM757.8 363.4h81.9v430.1h-81.9zM566.6 363.4h81.9v430.1h-81.9zM375.5 363.4h81.9v430.1h-81.9z" p-id="56054"></path></svg>');

	editor.ui.registry.addButton('organinfo', {
		icon: 'organinfo',
		tooltip: pluginName,
		onAction: function () {
			openDialog();
		}
	});
	editor.ui.registry.addMenuItem('organinfo', {
		text: pluginName,
		onAction: function () {
			openDialog();
		}
	});
	return {
		getMetadata: function () {
			return {
				name: pluginName,
				url: "https://gitee.com/weishakeji/LearningSystem",
			};
		}
	};
});
//接收来自内容页的调用
window.organinfo_action = function (id, txt) {
	var editor = tinyMCE.editors[id];
	if (txt != '') {
		txt = '<span class="organinfo">' + txt + '</span>';
		editor.insertContent(txt);		
		editor.windowManager.close();
	}
	console.log(txt);
}