tinymce.PluginManager.add('bdmap', function(editor, url) {
	var pluginName='插入百度地图';
	var baseURL=tinymce.baseURL;
	var iframe1 = baseURL+'/plugins/bdmap/map.html';
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

	editor.ui.registry.getAll().icons.bdmap || editor.ui.registry.addIcon('bdmap','<svg viewBox="0 30 1024 1024" xmlns="http://www.w3.org/2000/svg" width="20" height="20"><path d="M537.972364 826.228364L512 855.179636l-25.972364-28.951272a2274.606545 2274.606545 0 0 1-44.520727-52.084364 2336.116364 2336.116364 0 0 1-97.489454-125.742545 1516.218182 1516.218182 0 0 1-68.072728-102.516364C225.908364 462.731636 197.818182 392.913455 197.818182 337.454545 197.818182 163.933091 338.478545 23.272727 512 23.272727s314.181818 140.660364 314.181818 314.181818c0 55.458909-28.090182 125.277091-78.126545 208.430546a1516.218182 1516.218182 0 0 1-68.072728 102.516364 2336.116364 2336.116364 0 0 1-97.466181 125.742545 2274.606545 2274.606545 0 0 1-44.544 52.084364z m-9.216-96.628364a2267.461818 2267.461818 0 0 0 94.533818-121.949091 1447.726545 1447.726545 0 0 0 64.930909-97.745454c43.985455-73.076364 68.142545-133.166545 68.142545-172.45091C756.363636 202.496 646.958545 93.090909 512 93.090909S267.636364 202.496 267.636364 337.454545c0 39.284364 24.180364 99.374545 68.142545 172.45091a1447.726545 1447.726545 0 0 0 64.930909 97.745454A2267.461818 2267.461818 0 0 0 512 749.591273c5.352727-6.283636 10.938182-12.986182 16.756364-20.014546zM888.226909 605.090909H861.090909a34.909091 34.909091 0 0 1 0-69.818182h58.181818c17.687273 0 32.581818 13.265455 34.676364 30.836364l46.545454 395.636364A34.909091 34.909091 0 0 1 965.818182 1000.727273h-907.636364a34.909091 34.909091 0 0 1-34.676363-38.981818l46.545454-395.636364A34.909091 34.909091 0 0 1 104.727273 535.272727H162.909091a34.909091 34.909091 0 0 1 0 69.818182H135.773091L97.442909 930.909091h829.114182l-38.330182-325.818182zM512 453.818182a128 128 0 1 1 0-256 128 128 0 0 1 0 256z m0-69.818182a58.181818 58.181818 0 1 0 0-116.363636 58.181818 58.181818 0 0 0 0 116.363636z" p-id="70380"></path></svg>');

	editor.ui.registry.addButton('bdmap', {
		icon: 'bdmap',
        tooltip: pluginName,
		onAction: function() {
			openDialog();
		}
	});
	editor.ui.registry.addMenuItem('bdmap', {
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
