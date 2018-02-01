myFocus.extend({//*********************淘宝商城风格******************
	mF_taobaomall:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['txt']);F.wrap([F.$c('pic',box)],'cont')
		var cont=F.$c('cont',box),pics=F.$c('pic',box),txt=F.$li('txt',box);
		var n=txt.length,param={};//运行时相关参数
		pics.innerHTML+=pics.innerHTML;//无缝复制
		//CSS
		var pic=F.$li('pic',cont),dir=par.direction,dis=par.width;//先假设左右
		for(var i=0;i<pic.length;i++) pic[i].style.cssText='width:'+par.width+'px;height:'+par.height+'px;'//消除上下li间的多余间隙
		if(dir=='left'||dir=='right') {pics.style.cssText='width:'+2*dis*n+'px;';pics.className+=' '+dir;}//左右运动设定
		else {dis=par.height; pics.style.height=2*dis*n + 'px';}//上下运动设定
		if(dir=='bottom'||dir=='right') pics.style[dir]=0+'px';//向下或向右的特殊处理
		//CSS++
		var txtH=isNaN(par.txtHeight/1)?28:par.txtHeight;//设置默认的文字高度
		box.style.cssText='height:'+(par.height+txtH+1)+'px;';
		cont.style.cssText='width:'+par.width+'px;height:'+par.height+'px;';
		for(var i=0;i<n;i++) txt[i].style.width=(par.width-n-1)/n+1+'px';
		txt[n-1].style.border=0;
		F.$c('txt',box).style.width=2*par.width+'px';
		//PLAY
		eval(F.switchMF(function(){
			txt[index>=n?(index-n):index].className = '';
		},function(){
			param[dir]=-dis*next;
			F.slide(pics,param,par.duration,par.easing);
			txt[next>=n?(next-n):next].className = 'current';
		},'par.less','dir'));
		eval(F.bind('txt','par.trigger',par.delay));
	}
});
myFocus.set.params('mF_taobaomall',{//可选个性参数
	less:true,//是否无缝：true(是)| false(否)
	duration:600,//过渡时间(毫秒)，时间越大速度越小
	direction:'top',//运动方向，可选：'top'(向上) | 'bottom'(向下) | 'left'(向左) | 'right'(向右)
	easing:'easeOut'//运动形式，可选：'easeOut'(快出慢入) | 'easeIn'(慢出快入) | 'easeInOut'(慢出慢入) | 'swing'(摇摆运动) | 'linear'(匀速运动)
});