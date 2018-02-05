/*
* myFocus JavaScript Library v1.2.4
* Open source under the BSD & GPL License.
* 
* @Author  koen_lee@qq.com
* @Blog    http://hi.baidu.com/koen_li/
* 
* @Date    2011/07/20
*/
(function(){
	var mF={
		defConfig:{//全局默认设置
			pattern:'mF_fscreen_tb',//风格样式
			trigger:'click',//触发切换模式['click'(鼠标点击)|'mouseover'(鼠标悬停)]
			txtHeight:'default',//文字层高度设置['default'(默认高度)|0(隐藏)|num(数字,单位像素)]
			wrap:false,//是否保留边框(有的话)[true|false]
			auto:true,//是否自动播放[true|false]
			time:4,//每次停留时间[num(数字,单位秒)]
			index:0,//开始显示的图片序号(从0算起)[num(数字)]
			waiting:20,//Loading画面的最长等待时间[true(一直等待)|false(不等待)|num(数字,单位秒)]
			delay:100,//触发切换模式中'mouseover'模式下的切换延迟[num(数字,单位毫秒)]
			css:true,//是否需要程序定义CSS[true|false]
			path:'pattern/',//pattern文件的路径,相对html文件的路径,设置为false或0即取消程序引入
			autoZoom:false//是否允许图片自动缩放居中[true|false]
		},
		pattern:{},
		extend:function(){
			var a=arguments,l=a.length,i=1,parent=a[0];
			if(l===1){i=0,parent=this.pattern;}
			for(i;i<l;i++){for(var p in a[i]) if(!(p in parent)) parent[p]=a[i][p];}
		}
	};
	var url=$("#myFocusjs").attr("src");
	url=url.substring(0,url.lastIndexOf("/")+1); 
	mF.defConfig.path=url+mF.defConfig.path;
	var DOM={
		$:function(id){return typeof id==='string'?document.getElementById(id):id;},
		$$:function(tag,obj){return (this.$(obj)||document).getElementsByTagName(tag);},
		$$_:function(tag,obj){
			var arr=[],a=this.$$(tag,obj);
			for(var i=0;i<a.length;i++){
				if(a[i].parentNode===obj) arr.push(a[i]);
				i+=this.$$(tag,a[i]).length;
			} return arr;
		},
		$c:function(cla,obj){
			var tags=this.$$('*',obj),cla=cla.replace(/\-/g,'\\-'),reg=new RegExp('(^|\\s)'+cla+'(\\s|$)'),arr=[];
			for(var i=0,l=tags.length;i<l;i++){if(reg.test(tags[i].className)){arr.push(tags[i]);break;}}
			return arr[0];
		},
		$li:function(cla,obj){return this.$$_('li',this.$c(cla,obj));},
		wrap:function(arr,cla){//在arr(数组)外面添加wrap,cla为wrap的class
			var div=document.createElement('div');div.className=cla;arr[0].parentNode.insertBefore(div,arr[0]);
			for(var i=0;i<arr.length;i++) div.appendChild(arr[i]);
		},
		wrapIn:function(obj,cla){obj.innerHTML='<ul class='+cla+'>'+obj.innerHTML+'</ul>';},//在obj里面添加wrap,cla为wrap的class
		addList:function(obj,cla){
			var s=[],ul=this.$$('ul',obj)[0],li=this.$$_('li',ul),img,n=li.length,num=cla.length;
			for(var j=0;j<num;j++){
				s.push('<ul class='+cla[j]+'>');
				for(var i=0;i<n;i++){img=this.$$('img',li[i])[0];s.push('<li>'+(cla[j]=='num'?('<a>'+(i+1)+'</a>'):(cla[j]=='txt'&&img?li[i].innerHTML.replace(/\<img(.|\n|\r)*?\>/i,img.alt)+'<p>'+img.getAttribute("text")+'</p>':(cla[j]=='thumb'&&img?'<img src='+(img.getAttribute("thumb")||img.src)+' />':'')))+'<span></span></li>')};
				s.push('</ul>');
			}; obj.innerHTML+=s.join('');
		}
	},
	CSS={
		style:function(o,attr){var v=(this.isIE?o.currentStyle:getComputedStyle(o,''))[attr],pv=parseFloat(v);return isNaN(pv)?v:pv;},
		setOpa:function(o,val){o.style.filter = "alpha(opacity=" + val + ")",o.style.opacity = val/100;},
		removeClass:function(o,name){var cla=o.className,reg="/\\s*"+name+"\\b/g";o.className=cla?cla.replace(eval(reg),''):''}
	},
	Anim={
		animate:function(obj,attr,val,dur,type,fn){
			var opa=attr==='opacity',F=this,opacity=F.setOpa,am=typeof val==='string',st=(new Date).getTime();
			if(opa&&F.style(obj,'display')==='none') obj.style.display='block',opacity(obj,0);
			var os=F.style(obj,attr),b=isNaN(os)?1:os,c=am?val/1:val-b,d=dur||800,e=F.easing[type||'easeOut'],m=c>0?'ceil':'floor';
			if(obj[attr+'Timer']) clearInterval(obj[attr+'Timer']);
			obj[attr+'Timer']=setInterval(function(){
				var t=(new Date).getTime()-st;
				if(t<d){opa?opacity(obj,Math[m](e(t,b*100,c*100,d))):obj.style[attr]=Math[m](e(t,b,c,d))+'px';}
				else{
					clearInterval(obj[attr+'Timer']),opa?opacity(obj,(c+b)*100):obj.style[attr]=c+b+'px',
					opa&&val===0&&(obj.style.display='none'),fn&&fn.call(obj);
				}
			},13);return F;
		},
		fadeIn:function(obj,duration,fn){this.animate(obj,'opacity',1,duration==undefined?400:duration,'linear',fn);return this;},
		fadeOut:function(obj,duration,fn){this.animate(obj,'opacity',0,duration==undefined?400:duration,'linear',fn);return this;},
		slide:function(obj,params,duration,easing,fn){for(var p in params) this.animate(obj,p,params[p],duration,easing,fn);return this;},
		stop:function(obj){for(var p in obj) if(p.indexOf('Timer')!==-1) clearInterval(obj[p]);return this;},//停止所有运动
		easing:{
			linear:function(t,b,c,d){return c*t/d + b;},
			swing:function(t,b,c,d) {return -c/2 * (Math.cos(Math.PI*t/d) - 1) + b;},
			easeIn:function(t,b,c,d){return c*(t/=d)*t*t*t + b;},
			easeOut:function(t,b,c,d){return -c*((t=t/d-1)*t*t*t - 1) + b;},
			easeInOut:function(t,b,c,d){return ((t/=d/2) < 1)?(c/2*t*t*t*t + b):(-c/2*((t-=2)*t*t*t - 2) + b);}
		}
	},
	Init={
		set:function(p,DOMReady,callback){
			if(typeof DOMReady!=='boolean') callback=DOMReady,DOMReady=false;//可以省略DOMReady参数，默认false
			var F=this,cont=0;
			p.pattern=p.pattern||F.defConfig.pattern,p.path=p.path==undefined?F.defConfig.path:p.path,p.S=p.pattern+'-'+p.id;
			function show(){
				if(cont==2){//仅当JS和图片都加载完毕(cont=2)才开始轮播
					if(p.autoZoom) F.fixIMG(p.id,p.width,p.height);
					F.pattern[p.pattern].call(F,p,F);
					callback&&callback();
				}
			};
			function ready(){//当DOM就绪时
				var box=F.$(p.id);
				box.style.height=314+'px';//测试DOM加载&&赋值防变形
				F.loadPattern(p.pattern,p.path,function(){
					F.extend(p,F.pattern[p.pattern].cfg,F.defConfig);//收集完整参数
					p.width=p.width||F.style(box,'width'),p.height=p.height||F.style(box,'height');
					F.initCSS(p),box.className+=' '+p.pattern+' '+p.S,box.style.height='';
					cont+=1,show();
				});
				F.onloadIMG(box,p.waiting==undefined?F.defConfig.waiting:p.waiting,function(){cont+=1,show();});
			};
			if(DOMReady){ready();return;}//立即执行
			if(window.attachEvent){(function(){try{ready()}catch(e){setTimeout(arguments.callee,0)}})();}
	　　		else{F.addEvent(document,'DOMContentLoaded',ready);}
		},
		initCSS:function(p){
			var css=[],w=p.width,h=p.height,oStyle=document.createElement('style');oStyle.type='text/css';
			if(p.wrap) this.wrap([this.$(p.id)],p.pattern+'_wrap');
			if(p.css) css.push('.'+p.S+' *{margin:0;padding:0;border:0;list-style:none;}.'+p.S+'{position:relative;width:'+w+'px;height:'+h+'px;overflow:hidden;font:12px/1.5 Verdana;text-align:left;background:#fff;visibility:visible!important;}.'+p.S+' .loading{position:absolute;z-index:9999;width:100%;height:100%;color:#666;text-align:center;padding-top:'+0.3*h+'px;background:#fff url(myFocus/loading.gif) center '+0.4*h+'px no-repeat;}.'+p.S+' .pic{position:relative;width:'+w+'px;height:'+h+'px;overflow:hidden;}.'+p.S+' .txt li,.'+p.S+' .txt li span,.'+p.S+' .txt-bg{width:'+w+'px;height:'+p.txtHeight+'px!important;line-height:'+p.txtHeight+'px!important;overflow:hidden;}.'+p.S+' .txt li p a{display:inline;}');
			if(p.css&&p.autoZoom) css.push('.'+p.S+' .pic li{text-align:center;width:'+w+'px;height:'+h+'px;}');//缩放图片居中
			if(oStyle.styleSheet){oStyle.styleSheet.cssText=css.join('');} else {oStyle.innerHTML=css.join('');}
			var oHead = this.$$('head',document)[0];oHead.insertBefore(oStyle,oHead.firstChild);
		}
	},
	Method={
		isIE:!(+[1,]),
		switchMF:function(fn1,fn2,isless,dir,wrap){
			return "var _F=this,_ld=_F.$c('loading',box),less="+isless+",_tn,first=true,_dir="+dir+"||'left',_dis=_dir=='left'||_dir=='right'?par.width:par.height,_wp=less&&("+wrap+"||pics),index=par.index,_t=par.time*1000;if(less){_wp.style[_dir]=-_dis*n+'px';index+=n;}if(_ld)box.removeChild(_ld);var run=function(idx){("+fn1+")();var prev=index;if(less&&index==2*n-1&&_tn!=1){_wp.style[_dir]=-(n-1)*_dis+'px';index=n-1}if(less&&index==0&&_tn!=2){_wp.style[_dir]=-n*_dis+'px';index=n}if(!less&&index==n-1&&idx==undefined)index=-1;if(less&&idx!==undefined&&index>n-1&&!_tn&&!first) idx+=n;var next=idx!==undefined?idx:index+1;if("+fn2+")("+fn2+")();index=next;_tn=first=null;};run(index);if(_t&&par.auto)var auto=setInterval(function(){run()},_t);_F.addEvent(box,'mouseover',function(){if(auto)clearInterval(auto)});_F.addEvent(box,'mouseout',function(){if(auto)auto=setInterval(function(){run()},_t)});for(var i=0,_lk=_F.$$('a',box),_ln=_lk.length;i<_ln;i++) _lk[i].onfocus=function(){this.blur();}"
		},
		bind:function(arrStr,type,delay){
			return "for (var j=0;j<n;j++){"+arrStr+"[j].index=j;if("+type+"=='click'){"+arrStr+"[j].onmouseover=function(){if(this.index!=index)this.className+=' hover'};"+arrStr+"[j].onmouseout=function(){_F.removeClass(this,'hover')};"+arrStr+"[j].onclick=function(){if(this.index!=index) {run(this.index);return false}};}else if("+type+"=='mouseover'){"+arrStr+"[j].onmouseover=function(){var self=this;if("+delay+"==0){if(self.index!=index){run(self.index);return false}}else "+arrStr+".d=setTimeout(function(){if(self.index!=index) {run(self.index);return false}},"+delay+")};"+arrStr+"[j].onmouseout=function(){clearTimeout("+arrStr+".d)};}else{alert('Error Setting : \"'+"+type+"+'\"');break;}}"
		},
		toggle:function(obj,cla1,cla2){
			return "var _stop=false;"+obj+".onclick=function(){this.className=this.className=='"+cla1+"'?'"+cla2+"':'"+cla1+"';if(!_stop){clearInterval(auto);auto=null;_stop=true;}else{auto=true;_stop=false;}}"
		},
		scroll:function(obj,dir,dis,sn,dur){
			return "var scPar={},scDis="+dis+",scN=Math.floor("+sn+"/2),scDir=parseInt("+obj+".style["+dir+"])||0,scIdx=next>=n?next-n:next,scDur="+dur+"||500,scMax=scDis*(n-"+sn+"),scD=scDis*scIdx+scDir;if(scD>scDis*scN&&scIdx!==n-1) scPar["+dir+"]='-'+scDis;if(scD<scDis&&scIdx!==0) scPar["+dir+"]='+'+scDis;if(scIdx===n-1) scPar["+dir+"]=-scMax;if(scIdx===0) scPar["+dir+"]=0;_F.slide("+obj+",scPar,scDur);"
		},
		turn:function(prev,next){return prev+".onclick=function(){_tn=1;run(index>0?index-1:n-1);};"+next+".onclick=function(){_tn=2;var tIdx=index>=2*n-1?n-1:index;run(index==n-1&&!less?0:tIdx+1);}"},
		alterSRC:function(o,name,del){var img=this.$$('img',o)[0];img.src=del?img.src.replace(eval("/"+name+"\\.(?=[^\\.]+$)/g"),'.'):img.src.replace(/\.(?=[^\.]+$)/g,name+'.')},
		onloadIMG:function(box,wait,callback){
			var img=this.$$('img',box),len=img.length,count=0,ok=false;
			for(var i=0;i<len;i++){
				img[i].onload=function(){
					count+=1;
					if(count==len&&!ok){ok=true,callback();}
				};
				if(this.isIE) img[i].src=img[i].src;//修复IE BUG
			};
			if(wait===true) return;
			var t=wait===false?0:wait*1000;
			setTimeout(function(){
				if(!ok){ok=true,callback();}
			},t);
		},
		fixIMG:function(box,boxWidth,boxHeight){
			var imgs=this.$$('img',box),len=imgs.length,IMG = new Image();
			for(var i=0;i<len;i++){
				IMG.src = imgs[i].src;
				if(IMG.width / IMG.height >= boxWidth / boxHeight){
					imgs[i].style.width=boxWidth+'px';
					imgs[i].style.marginTop=(boxHeight-boxWidth/IMG.width*IMG.height)/2+'px';//垂直居中
				}else{
					imgs[i].style.height=boxHeight+'px';
				}
			};
		},
		loadPattern:function(name,path,callback){
			if(!path){callback();return;}//兼容手写路径
			var js= document.createElement("script"),css=document.createElement("link"),src=path+name+'.js',href=path+name+'.css'; 
    		js.type = "text/javascript",js.src=src;
			css.rel = "stylesheet",css.href=href;
			this.$$('head')[0].appendChild(css);
			this.$$('head')[0].appendChild(js);
			if(this.isIE) {
				js.onreadystatechange=function(){if(js.readyState=="loaded" || js.readyState=="complete") callback();}
			}
			else{js.onload=function(){callback();}}
			js.onerror=function(){alert('Not Found (404): '+src)}//chrome
		},
		addEvent:function(obj,type,fn){var b=this.isIE,e=b?'attachEvent':'addEventListener',t=(b?'on':'')+type;obj[e](t,fn,false);}
	};
	mF.extend(mF,DOM,CSS,Anim,Init,Method);
	mF.set.params=function(name,p){mF.pattern[name].cfg=p};
	//防变量冲突处理
	myFocus__AGENT__=mF;//引用
	if(typeof myFocus==='undefined') myFocus=myFocus__AGENT__;
	//支持JQ
	if(typeof jQuery!=='undefined'){
		jQuery.fn.extend({
			myFocus:function(p,fn){
				if(!p) p={};
				p.id=this[0].id;
				if(!p.id) p.id=this[0].id='mF__NAME__';
				myFocus__AGENT__.set(p,true,fn);
			}
		});
	}
})();