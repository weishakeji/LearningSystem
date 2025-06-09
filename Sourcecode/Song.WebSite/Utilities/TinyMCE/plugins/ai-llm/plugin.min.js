tinymce.PluginManager.add('ai-llm', function (editor, url) {
	var pluginName = 'AI助手';
	var baseURL = tinymce.baseURL;
	//console.error(pluginName);
	var editorid = editor.id;
	//console.error(editor);
	var iframe1 = baseURL + '/plugins/ai-llm/index.html?editorid=' + editorid;
	var openDialog = function () {
		return editor.windowManager.openUrl({
			title: 'AI创作助手',
			size: 'large',
			width: Math.floor(window.innerWidth * 0.8),
			height: Math.floor(window.innerHeight * 0.8),
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

	editor.ui.registry.getAll().icons.aillm || editor.ui.registry.addIcon('aillm', '<svg class="icon"  width="20" height="20" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg"><path fill="#d81e06" d="M986.24 243.648C915.072 91.072 752.512 0 551.488 0 184.96 0 118.72 356.352 108.16 440.896 84.352 461.056 36.224 507.328 7.68 571.648c-11.264 25.344-10.112 51.264 3.328 73.024 25.984 42.24 93.696 58.112 139.84 64.128v241.92c0 21.376 17.28 38.656 38.656 38.656h232.704a38.656 38.656 0 0 0 0-77.312H228.16v-238.528a38.656 38.656 0 0 0-36.8-38.592c-50.304-2.496-107.072-17.728-113.088-31.936 29.632-66.56 88.832-110.4 89.408-110.848a39.04 39.04 0 0 0 15.872-28.992c0.896-15.744 27.456-385.92 367.936-385.92 170.24 0 306.56 74.368 364.8 199.04 64.384 137.92 20.608 313.408-120.064 481.536a38.72 38.72 0 0 0-8.96 24.768v162.176a38.656 38.656 0 1 0 77.248 0v-148.224c153.472-188.928 197.888-389.824 121.792-552.96"  /><path fill="#d81e06" d="M703.424 268.608a38.656 38.656 0 0 0-38.656 38.656v341.824a38.656 38.656 0 0 0 77.312 0V307.264a38.656 38.656 0 0 0-38.656-38.656zM441.856 531.84l31.04-86.272 29.44 86.272h-60.48z m68.48-215.552a38.656 38.656 0 0 0-36.288-26.176h-0.256a38.656 38.656 0 0 0-36.352 25.6l-119.232 331.776a38.656 38.656 0 0 0 72.704 26.24l23.168-64.64h114.752l21.824 64a38.656 38.656 0 0 0 73.152-24.96L510.336 316.224z"  /></svg>');

	//console.error(editor.ui.registry.getAll().icons.aillm);
	editor.ui.registry.addButton('ai-llm', {
		icon: 'aillm',
		tooltip: pluginName,
		onAction: function () {
			openDialog();
		}
	});
	editor.ui.registry.addMenuItem('ai-llm', {
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
window.ai_llm_action = function (id, txt) {
	var editor = tinyMCE.editors[id];
	if (txt != '') {
		txt = '<span class="ai-llm">' + txt + '</span>';
		editor.insertContent(txt);		
		editor.windowManager.close();
	}
	console.log(txt);
}
