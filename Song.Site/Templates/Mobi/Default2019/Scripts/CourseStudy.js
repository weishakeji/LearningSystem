$(function () {
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
		box.CloseEvent = function () {
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
	box.CloseEvent = function () {
		if ($(".video-box").height() > 10) {
			$(".video-box").show();
		}
	}
	box.Open();
}

//
var vdata = new Vue({
	data: {
		account: {},		//当前登录学员
		course: {},		//当前课程
		outline: {},		//当前章节
		outlines: [], 	//当前课程所有章节
		isbuy: false,		//当前学员是否购买该课程
		messages: [], //咨询留言
		menuShow: false,		//章节菜单是否显示
		contextShow: '',		//内容显示的判断
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
	watch: {
		//课程状态
		state: function (val) {
			//视频播放
			if (vdata.state.existVideo || vdata.state.isLive)
				vdata.videoPlay(vdata.state);
		},
		//播放进度变化
		playtime: function (val) {
			vdata.video.studytime++;
			//当前视频播放进度百分比
			var per = Math.floor(vdata.video.studytime <= 0 ? 0 : vdata.video.studytime / vdata.video.total * 100);
			vdata.playpercent = per;
			//触发视频事件
			//vdata.videoEvent(vdata.playtime);
		},
		//播放进度百分比变化，
		playpercent: function (val, oldval) {
			//console.log('当前播放进度百分比：'+val);
			//学习记录提交
			if (val <= 100) vdata.videoLog(val);
		},
		//章节菜单变化
		menuShow: function (val, oval) {
			//隐藏视频
			var video = document.querySelector("video");
			if (val) {
				if (video) video.style.display = "none";
			} else {
				if (video) video.style.display = "block";
			}
		}
	},
	created: function () {
		var couid = $api.querystring("couid");
		$api.bat(
			$api.get('Account/Current'),
			$api.get("Course/ForID", { id: couid }),
			$api.cache("Outline/tree", { couid: couid }),
			$api.get('Course/Studied', { 'couid': couid })
		).then(axios.spread(function (account, cur, ol, isbuy) {
			//判断结果是否正常
			for (var i = 0; i < arguments.length; i++) {
				if (arguments[i].status != 200)
					console.error(arguments[i]);
				var data = arguments[i].data;
				if (!data.success && data.exception != '') {
					console.error(data.exception);
					throw data.message;
				}
			}
			vdata.account = account.data.result;
			vdata.course = cur.data.result;
			vdata.outlines = ol.data.result;
			//vdata.outlineClick(vdata.olid, null);		
			vdata.isbuy = isbuy.data.result;
			vdata.msgGet();

		})).catch(function (err) {
			alert(err);
		});
		//定时刷新（加载）咨询留言
		window.setInterval('vdata.msgGet()', 1000 * 10);
	},
	methods: {
		//页面刷新
		pagefresh: function () {
			//alert("页面刷新");
			window.location.reload();
		},
		//播放器是否准备好
		playready: function () {
			if (vdata.player != null) {
				return vdata.player._isReady && vdata.player.engine;
			}
			return false;
		},
		//视频播放跳转
		videoSeek: function (second) {
			if (vdata.playready()) vdata.player.seek(second);
		},
		//视频开始播放
		videoPlay: function (state) {
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
				vdata.player.on("error", function (e) {
					//alert("播放发生错误："+e);
				});
				vdata.player.on("play", function (e) {
					vdata.video.loading = false;
				});
				vdata.player.on("loading", function () {
					vdata.video.loading = true;
				});
			}
			if (vdata.player != null) {
				vdata.player.on("ready", vdata.videoready);
				vdata.player.on("timeupdate", function (currentTime, totalTime) {
					vdata.video.total = parseInt(totalTime);
					vdata.video.playTime = currentTime; //详细时间，精确到毫秒
					vdata.playtime = parseInt(currentTime);
					//学习完成度，最大为百分百
					vdata.video.percent = Math.floor(vdata.video.studytime <= 0 ? 0 : vdata.video.studytime / vdata.video.total * 1000) / 10;
					vdata.video.percent = vdata.video.percent > 100 ? 100 : vdata.video.percent;
				});
				vdata.player.on("seeked", function () {
					vdata.playtime = vdata.playready() ? vdata.player.currentTime : 0;
					window.setTimeout(function () {
						if (vdata.playready()) vdata.player.pause();
					}, 50);

				});
			}
		},
		//播放器加载后的事件
		videoready: function () {
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
			window.setInterval(function () {
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
		outlineClick: function (olid, event) {
			var url = $api.setpara("olid", olid);
			history.pushState({}, null, url);
			vdata.olid = olid;
			if (event != null) event.preventDefault();
			//获取当前章节状态，和专业信息
			$api.bat(
				$api.get("Outline/ForID", { id: olid }),
				$api.get("Outline/state", { olid: olid })
			).then(axios.spread(function (ol, state) {
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
					window.setTimeout(function () {
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
		videoLog: function (per) {
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
				}, function () {
					vdata.studylogUpdate = true;
				}, function () {
					vdata.studylogUpdate = false;
				}).then(function (req) {
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
					window.setTimeout(function () {
						vdata.studylogState = 0;
					}, 2000);
				}).catch(function (err) {
					vdata.studylogState = -1;
					alert(err);
					window.setTimeout(function () {
						vdata.studylogState = 0;
					}, 2000);
				});
			}
		},
		//发送消息
		msgSend: function () {
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
			}).then(function (req) {
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
		msgBlur: function (e) {
			document.getElementById("footer").style.display = "table";
			document.getElementById("messageinput").blur();
		},
		//留言输入框获取焦点
		msgFocus: function (e) {
			document.getElementById("footer").style.display = "none";
			document.getElementById("messageinput").focus();
		},
		//获取当前章节的留言信息
		msgGet: function () {
			if (!vdata.olid || vdata.olid < 1) return;
			$api.post("message/count", {
				olid: vdata.olid,
				order: 'asc',
				count: 100
			}).then(function (req) {
				var d = req.data;
				if (d.success) {
					vdata.messages = d.result;
					window.setTimeout(function () {
						var dl = document.getElementById("chatlistdl");
						if (dl == null) return;
						document.getElementById("chatlist").scrollTop = dl.offsetHeight;
					}, 1000);
				} else {
					throw "留言信息加载异常！详情：\r" + d.message;
				}
			}).catch(function (err) {
				alert(err);
			});
		}
	}
});
vdata.$mount('#offCanvasWrapper');
//窗体失去焦点的事件
window.onblur = function () {
	if (vdata.playready()) {
		if (!vdata.state.isLive)
			vdata.player.pause();
	}
}
window.onfocus = function () {
	if (vdata.playready()) {
		!vdata.state.isLive ? vdata.player.play() : vdata.player.pause();
	}
}
$(function () {
	if (typeof (mui) != 'undefined') {
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
// 章节列表组件
Vue.component('menutree', {
	//章节列表，课程对象，菜单是否显示
	props: ['outlines', 'course', 'isbuy', 'menushow'],
	data: function () {
		return {
			current: {}, //当前章节对象
			state: {},		//章节状态
			olid: 0,		//当前章节id
			loading: true, //预载中
			defimg: ''   //课程默认图片
		}
	},
	watch: {
		//当章节id变化时
		olid: {
			handler(nv, ov) {
				if (nv == 0) return;
				var th = this;
				$api.get('Outline/State', { 'olid': nv }).then(function (req) {
					if (req.data.success) {
						th.state = req.data.result;
					} else {
						console.error(req.data.exception);
						throw req.data.message;
					}
				}).catch(function (err) {
					alert(err);
					console.error(err);
				});
			},
			immediate: true
		}
	},
	created: function () {
		//默认图片
		var img = document.getElementById("default-img");
		this.defimg = img.getAttribute("src");
		//当前章节
		this.olid = $api.querystring("olid");
		if (this.olid == '') {
			for (var i = 0; i < this.outlines.length; i++) {
				if (this.outlines[i].Ol_IsVideo) {
					current = this.outlines[i];
					this.olid = current.Ol_ID;
					break;
				}
			}
		} else {
			for (var i = 0; i < this.outlines.length; i++) {
				if (this.outlines[i].Ol_ID == this.olid) {
					current = this.outlines[i];
					break;
				}
			}
		}
		vdata.outline = current;
	},
	methods: {
		//计算缩进
		padding: function (level) {
			return 'padding-left:' + (level * 10 + 15) + 'px';
		},
		//章节点击事件
		click: function (node) {
			vdata.outline = node;
			var url = $api.setpara('olid', node.Ol_ID);
			history.pushState({}, null, url);
			vdata.menuShow = false;
			console.log(node);
		}
	},
	template: "<div id='menu-layer' :class='{active:menushow}'  v-on:click.self='vdata.menuShow=false'>\
	<div id='menu'>\
		<div class='cour-info'>\
		<img :src='course.Cou_Logo' v-if='course.Cou_Logo.length>0'/>\
		<img :src='defimg' class='no' v-else/>\
		<div class='cour-info-right'>\
			<cour-name>{{course.Cou_Name}}</cour-name>\
			<sbj-name>{{course.Sbj_Name}}</sbj-name>\
			</div>\
		</div>\
		<ul class='mui-table-view mui-table-view-chevron mui-table-view-inverted' v-if='outlines.length>0'>\
        <li class='mui-table-view-cell outline' v-for='o in outlines' :isvideo='o.Ol_IsVideo' :islive='o.Ol_IsLive'\
          :olid='o.Ol_ID' :style='padding(o.Ol_Level)' v-on:click='click(o)'>\
          <template v-if='isbuy'>\
            <a class='ol_name'>{{o.Ol_XPath}}{{o.Ol_Name}}</a>\
			<span v-if='!o.Ol_IsFinish' class='mui-badge'>未完</span>\
          </template>\
          <template v-else>\
            <a v-if='o.Ol_IsFree' class='mui-navigate-right'>{{o.Ol_XPath}}{{o.Ol_Name}}\
              <span class='mui-badge try'>试学</span></a>\
            <a v-else class='mui-navigate-right'>{{o.Ol_XPath}}{{o.Ol_Name}}\
              <span class='mui-badge buy'>购买</span></a>\
          </template>\
        </li>\
      </ul>\
      <div class='mui-table-view-cell' v-else style='color: azure;'> 当前课程没有章节 </div>\
 	</div>\
	</div>"
});
//底部按钮组件
Vue.component('coursestudyfooter', {
	props: ['course'],
	data: function () {
		return {
			menus: [
				{ label: '章节列表', id: 'outline', icon: '&#xe79c', show: true },
				{ label: '视频', id: 'video', icon: '&#xe615', show: true },
				{ label: '交流', id: 'message', icon: '&#xe75a', show: false },
				{ label: '学习内容', id: 'content', icon: '&#xe65e', show: true },
				{ label: '附件', id: 'accessory', icon: '&#xe651', show: true },
				{ label: '返回课程', id: 'goback', icon: '&#xe60b', show: true }
			],
			loading: true, //预载中
			defimg: ''   //课程默认图片
		}
	},
	computed: {},
	created: function () {
		//默认图片
		var img = document.getElementById("default-img");
		this.defimg = img.getAttribute("src");
	},
	methods: {
		//事件
		click: function (item) {
			switch (item.id) {
				//显示章节树形菜单
				case 'outline':
					vdata.menuShow = true;
					break;
				case 'goback':
					document.location.href = 'CoursePage.ashx?couid=' + this.course.Cou_ID;
					break;
				default:
					vdata.contextShow = item.id;
					break;
			}
			console.log(this.course);
		}
	},
	template: "<nav class='mui-bar mui-bar-tab footer' id='footer'>\
	<a v-for='item in menus' :id='item.id' v-if='item.show' v-on:click='click(item)'>\
	<b v-html='item.icon'></b>\
	<span>{{item.label}}</span>\
	</a></nav>"
});
//附件列表
Vue.component('accessory', {
	props: ['outline', 'account'],
	data: function () {
		return {
			files: [],		//附件文件列表		
			loading: true //预载中
		}
	},
	watch: {
		'outline': {
			deep: true,
			immediate: true,
			handler: function (newV, oldV) {
				if (JSON.stringify(newV) == '{}' || newV == null) return;
				var th = this;
				$api.get('Outline/Accessory', { 'uid': newV.Ol_UID }).then(function (req) {
					if (req.data.success) {
						th.files = req.data.result;
					} else {
						console.error(req.data.exception);
						throw req.data.message;
					}
				}).catch(function (err) {
					alert(err);
					console.error(err);
				});
			}
		}
	},
	created: function () {

	},
	methods: {
		//是不是pdf格式
		ispdf: function (href) {
			var exist = "";
			if (href.indexOf("?") > -1) href = href.substring(0, href.indexOf("?"));
			if (href.indexOf(".") > -1) exist = href.substring(href.lastIndexOf(".") + 1).toLowerCase();
			return exist == "pdf";
		},
		//附件击事件
		openpdf: function (href) {
			var tit = $.trim($(this).text());
			var pdfview = $().PdfViewer(href);
			var box = new PageBox(tit, pdfview, 100, 100);
			if ($(".video-box").height() > 10) $(".video-box").hide();
			$('video').trigger('pause');
			box.CloseEvent = function () {
				if ($(".video-box").height() > 10) {
					$(".video-box").show();
				}
			}
			box.Open();
			return false;
		}
	},
	template: "<template>\
	<div class='mui-scroll'  v-if='account.Ac_ID'>\
		<dl id='access' v-if='files.length>0'>\
			<dd v-for='(f,i) in files'>\
				<a target='_blank' :href='f.As_FileName' v-if='ispdf(f.As_FileName)' \
				:download='f.As_Name' @click.prevent ='openpdf(f.As_FileName)'>\
				{{i+1}}、{{f.As_Name}}</a>\
				<a target='_blank' :href='f.As_FileName' v-else\
				:download='f.As_Name'>{{i+1}}、{{f.As_Name}}</a>\
			</dd>\
		</dl>\
		<div class='noaccess' v-else>（没有附件）</div>\
	</div>\
	<div class='noaccess' v-else>（未登录或未购买）</div>\
	</template>"
});
