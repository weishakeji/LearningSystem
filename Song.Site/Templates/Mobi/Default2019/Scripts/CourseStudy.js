$(function() {
	try {
		mui('body').on('tap', '#access a', access_a_click);
		mui('body').on('tap', '#details a', details_a_click);
	} catch (err) {
		$("#access a").click(access_a_click);
		$("#details a").click(details_a_click);
	}
});
//附件下载,如果是pdf则预览
function access_a_click() {
	var href = $(this).attr("href");
	var exist = "";
	if (href.indexOf("?") > -1) href = href.substring(0, href.indexOf("?"));
	if (href.indexOf(".") > -1) exist = href.substring(href.lastIndexOf(".") + 1).toLowerCase();
	if (exist != "pdf") {
		document.location.href = this.href;
	} else {
		var tit = $.trim($(this).text());
		var pdfview = $().PdfViewer(href);
		var box = new PageBox(tit, pdfview, 100, 100);
		if ($(".video-box").height() > 10) $(".video-box").hide();
		$('video').trigger('pause');
		box.CloseEvent = function() {
			if ($(".video-box").height() > 10) {
				$(".video-box").show();
			}
		}
		box.Open();
	}
	return false;
}
//学习资料内容中的超链接打开
function details_a_click() {
	var href = $(this).attr("href");
	var tit = $.trim($(this).text());
	var box = new PageBox(tit, href, 100, 100);
	if ($(".video-box").height() > 10) $(".video-box").hide();
	$('video').trigger('pause');
	box.CloseEvent = function() {
		if ($(".video-box").height() > 10) {
			$(".video-box").show();
		}
	}
	box.Open();
}

//
var vdata = new Vue({
	data: {
		outline: {}, //当前课程章节
		messages: [], //咨询留言
		state: {}, //课程状态  
		couid: $api.querystring("couid"),
		olid: $api.querystring("olid"),
		//当前章节的视频信息
		video: {
			url: '', //视频路径
			total: 0, //总时长      
			playTime: 0, //当前播放时间，单位：毫秒     
			playhistime: 0, //历史播放时间
			studytime: 0, //累计学习时间
			percent: 0, //完成度（百分比）
			loading: false //是否处于加载状态
		},
		playtime: 0, //当前播放时间，单位：秒
		playpercent: 0, //当前播放完成度百分比
		studylogUpdate: false, //学习记录是否在递交中
		studylogState: 0, //学习记录提交状态，1为成功，-1为失败
		switchSubtitle: $api.cookie("switchSubtitle_" + $api.querystring("couid")) == "true" ? true : false, //弹幕开关
		//控件
		player: null //播放器
	},
	watch: { //课程状态
		state: function(val) {
			//视频播放
			if (vdata.state.existVideo || vdata.state.isLive)
				vdata.videoPlay(vdata.state);
		},
		//播放进度变化
		playtime: function(val) {
			vdata.video.studytime++;
			//当前视频播放进度百分比
			var per = Math.floor(vdata.video.studytime <= 0 ? 0 : vdata.video.studytime / vdata.video.total * 100);
			vdata.playpercent = per;
			//触发视频事件
			//vdata.videoEvent(vdata.playtime);
		},
		//播放进度百分比变化，
		playpercent: function(val, oldval) {
			//console.log('当前播放进度百分比：'+val);
			//学习记录提交
			if (val <= 100) vdata.videoLog(val);
		}
	},
	methods: {
		//页面刷新
		pagefresh: function() {
			//alert("页面刷新");
			window.location.reload();
		},
		//播放器是否准备好
		playready: function() {
			if (vdata.player != null) {
				return vdata.player._isReady && vdata.player.engine;
			}
			return false;
		},
		//视频播放跳转
		videoSeek: function(second) {
			if (vdata.playready()) vdata.player.seek(second);
		},
		//视频开始播放
		videoPlay: function(state) {
			if (vdata.player != null) {
				vdata.player.destroy();
				vdata.player = null;
			}
			if (!vdata.state.isLive) { //点播
				vdata.player = new QPlayer({
					url: state.urlVideo,
					container: document.getElementById("videoplayer"),
					autoplay: true,
					loop: "loop",
					loggerLevel: 0
				});
			} else { //直播
				var u = navigator.userAgent,
					app = navigator.appVersion;
				var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Linux') > -1; //g
				var isIOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
				vdata.player = new QPlayer({
					url: state.urlVideo,
					container: document.getElementById("livebox"),
					isLive: !isIOS,
					autoplay: true
				});
				vdata.player.on("error", function(e) {
					//alert("播放发生错误："+e);
				});
				vdata.player.on("play", function(e) {
					vdata.video.loading = false;
				});
				vdata.player.on("loading", function() {
					vdata.video.loading = true;
				});
			}
			if (vdata.player != null) {
				vdata.player.on("ready", vdata.videoready);
				vdata.player.on("timeupdate", function(currentTime, totalTime) {
					vdata.video.total = parseInt(totalTime);
					vdata.video.playTime = currentTime; //详细时间，精确到毫秒
					vdata.playtime = parseInt(currentTime);
					//学习完成度，最大为百分百
					vdata.video.percent = Math.floor(vdata.video.studytime <= 0 ? 0 : vdata.video.studytime / vdata.video.total * 1000) / 10;
					vdata.video.percent = vdata.video.percent > 100 ? 100 : vdata.video.percent;
				});
				vdata.player.on("seeked", function() {
					vdata.playtime = vdata.playready() ? vdata.player.currentTime : 0;
					window.setTimeout(function() {
						if (vdata.playready()) vdata.player.pause();
					}, 50);

				});
			}
		},
		//播放器加载后的事件
		videoready: function() {
			//隐藏全屏按钮
			var fullbtn = document.getElementsByClassName("qplayer-fullscreen");
			for (var i = 0; i < fullbtn.length; i++) {
				//fullbtn[i].style.display = "none";
			}
			//隐藏设置按钮(播放倍速也禁用了)
			var setbtn = document.getElementsByClassName("qplayer-settings-btn");
			for (var i = 0; i < setbtn.length; i++) {
				//setbtn[i].style.display = "none";
			}
			window.setInterval(function() {
				var video = document.querySelector("video");
				if (!$().isWeixin()) {
					video.setAttribute("x5-playsinline", "true");
					video.setAttribute("playsinline", "true");
					video.setAttribute("webkit-playsinline", "true");
					video.removeAttribute("controls");
					//video.setAttribute("x5-video-player-fullscreen", "true");
					video.setAttribute("x5-video-orientation", "portraint");
					video.setAttribute("controlsList", "nodownload");
				} else {
					video.setAttribute("x-webkit-airplay", true);
					video.setAttribute("x5-video-player-type", "h5");
				}
			}, 3000);
			//给video对象增加属性
			var video = document.querySelector("video");

		},
		//章节列表的点击事件
		outlineClick: function(olid, event) {
			var url = $api.setpara("olid", olid);
			history.pushState({}, null, url);
			vdata.olid = olid;
			if (event != null) event.preventDefault();
			//获取当前章节状态，和专业信息
			$api.all(
				$api.get("Outline/ForID", {
					id: olid
				}),
				$api.get("Outline/state", {
					olid: olid
				})
			).then(axios.spread(function(ol, state) {
				if (ol.data.success && state.data.success) {
					vdata.outline = ol.data.result;
					vdata.state = state.data.result;
					if (!vdata.state.isLive && vdata.state.PlayTime > 0) {
						/*if (window.confirm("是否从上次进度播放？")) {
							vdata.videoSeek(vdata.state.PlayTime / 1000);
							window.setTimeout(function() {
								if (vdata.playready()) vdata.player.play();
							}, 500);
						}*/
					}
					//视频播放记录
					var result = state.data.result;
					vdata.video.studytime = isNaN(result.StudyTime) ? 0 : result.StudyTime;
					vdata.video.playhistime = isNaN(result.PlayTime) ? 0 : result.PlayTime / 1000;
					window.setTimeout(function() {
						vdata.outlineLoaded = true;
					}, 100);
				} else {
					if (!ol.data.success) alert("章节信息加载异常！详情：\r" + ol.data.message);
					if (!state.data.success) alert("章节状态加载异常！详情：\r" + state.data.message);
				}
			}));
			//获取留言列表
			//vdata.msgGet();
		},
		//学习记录记录
		videoLog: function(per) {
			if (vdata.studylogUpdate) return;
			var interval = 1; //间隔百分比多少递交一次记录
			if (vdata.video.total <= 5 * 60) interval = 10; //5分钟内的视频
			else if (vdata.video.total <= 10 * 60) interval = 5; //10分钟的视频，5%递交一次      
			else if (vdata.video.total <= 30 * 60) interval = 2; //30分钟的视频，2%递交一次 
			if (per > 0 && per < (100 + interval) && per % interval == 0) {
				$api.post("Course/StudyLog", {
					couid: vdata.outline.Cou_ID,
					olid: vdata.outline.Ol_ID,
					playTime: vdata.playtime,
					studyTime: vdata.video.studytime,
					totalTime: vdata.video.total
				}, function() {
					vdata.studylogUpdate = true;
				}, function() {
					vdata.studylogUpdate = false;
				}).then(function(req) {
					if (!req.data.success) {
						if (vdata.playready()) {
							vdata.player.pause();
							vdata.player.destroy();
							vdata.player = null;
						}
						alert(req.data.message);
						return;
					}
					vdata.studylogState = 1;
					window.setTimeout(function() {
						vdata.studylogState = 0;
					}, 2000);
				}).catch(function(err) {
					vdata.studylogState = -1;
					alert(err);
					window.setTimeout(function() {
						vdata.studylogState = 0;
					}, 2000);
				});
			}
		},
		//发送消息
		msgSend: function() {
			vdata.msgBlur();
			var msg = document.getElementById("messageinput").value;
			if ($api.trim(msg) == '') return;
			var span = Date.now() - Number($api.cookie("msgtime"));
			if (span / 1000 < 10) {
				vdata.$notify({
					message: '不要频繁发消息！',
					position: 'bottom-right'
				});
				return;
			}
			$api.cookie("msgtime", Date.now());
			$api.post("message/add", {
				acc: '',
				msg: msg,
				playtime: vdata.playtime,
				couid: vdata.couid,
				olid: vdata.olid
			}).then(function(req) {
				var d = req.data;
				if (d.success) {
					var input = document.getElementById("messageinput");
					input.value = '';
					vdata.msgGet();
				} else {
					alert("信息添加发生异常！详情：\r" + d.message);
				}
			});
		},
		//留言输入框失去焦点
		msgBlur: function(e) {
			document.getElementById("footer").style.display = "table";
			document.getElementById("messageinput").blur();
		},
		//留言输入框获取焦点
		msgFocus: function(e) {
			document.getElementById("footer").style.display = "none";
			document.getElementById("messageinput").focus();
		},
		//获取当前章节的留言信息
		msgGet: function() {
			if (!vdata.olid || vdata.olid < 1) return;
			$api.post("message/All", {
				olid: vdata.olid,
				order: 'desc'
			}).then(function(req) {
				var d = req.data;
				if (d.success) {
					vdata.messages = d.result;
					window.setTimeout(function() {
						var dl = document.getElementById("chatlistdl");
						document.getElementById("chatlist").scrollTop = dl.offsetHeight;
					}, 1000);
				} else {
					throw "留言信息加载异常！详情：\r" + d.message;
				}
			}).catch(function(err) {
				//alert(err);
			});			
		}
	},
	created: function() {
		var couid = $api.querystring("couid");
		$api.all(
			$api.get("Outline/tree", {
				couid: couid
			}),
			$api.get("Course/ForID", {
				id: couid
			})).then(axios.spread(function(ol, cur) {
			if (ol.data.success && cur.data.success) {
				vdata.outlines = ol.data.result;
				if (vdata.olid == '' || vdata.olid == null) vdata.olid = ol.data.result[0].Ol_ID;
				vdata.outlineClick(vdata.olid, null);
				vdata.course = cur.data.result;
				vdata.msgGet();
			} else {
				if (!ol.data.success) throw "章节列表加载异常！详情：\r" + ol.data.message;
				if (!cur.data.success) throw "课程信息加载异常！详情：\r" + cur.data.message;
			}
		})).catch(function(err) {
			alert(err);
		});
        //定时刷新（加载）咨询留言
        window.setInterval('vdata.msgGet()', 1000 * 20);
	}
});
vdata.$mount('#context-box');
//窗体失去焦点的事件
window.onblur = function() {
	if (vdata.playready()) {
		if (!vdata.state.isLive)
			vdata.player.pause();
	}
}
window.onfocus = function() {
	if (vdata.playready()) {
		!vdata.state.isLive ? vdata.player.play() : vdata.player.pause();
	}
}
//全局过滤器，日期格式化
Vue.filter('date', function(value, fmt) {
	if ($api.getType(value) != 'Date') return value;
	var o = {
		"M+": value.getMonth() + 1,
		"d+": value.getDate(),
		"h+": value.getHours(),
		"m+": value.getMinutes(),
		"s+": value.getSeconds()
	};
	if (/(y+)/.test(fmt))
		fmt = fmt.replace(RegExp.$1, (value.getFullYear() + "").substr(4 - RegExp.$1.length));
	for (var k in o)
		if (new RegExp("(" + k + ")").test(fmt))
			fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
	return fmt;
});

$(function() {
	if (typeof(mui) != 'undefined') {
		//点击留言按钮，进入留言输入状态
		mui('body').on('tap', '#msginputBtn', vdata.msgFocus);
		mui('body').on('tap', '#chatArea, #videoplayer', vdata.msgBlur);
	}
});

/*
//当点击视频播放区域时，显示控制条
mui('body').on('tap', '.videobox', function () {
    //alert("点击视频");
    vdata.msgBlur();
    var nodes = document.getElementsByClassName("qplayer-controls");
    if (nodes.length < 1) return;
    nodes[0].style.bottom = "40px";
    window.setTimeout(function () {
        var nodes = document.getElementsByClassName("qplayer-controls");
        if (nodes.length < 1) return;
        nodes[0].style.bottom = "0px";
    }, 5000);
});
*/