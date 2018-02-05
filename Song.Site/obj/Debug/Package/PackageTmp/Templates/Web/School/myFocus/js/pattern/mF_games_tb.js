myFocus.extend({//*********************games******************
	mF_games_tb:function(par,F){
		var box=F.$(par.id);
		F.addList(box,['thumb','txt','prev','next']);F.wrap([F.$c('thumb',box)],'win');
		var pics=F.$c('pic',box),win=F.$c('win',box),thus=F.$c('thumb',box),thu=F.$li('thumb',box),txt=F.$li('txt',box),pre=F.$c('prev',box),nex=F.$c('next',box),n=txt.length;
		pre.innerHTML='<a href="javascript:;">&#8249;</a>';nex.innerHTML='<a href="javascript:;">&#8250;</a>';//前后箭头
		//CSS
		var pic=F.$li('pic',box),winW=par.width-20,sw=Math.floor(winW/4),sh=par.height/par.width*(sw-14)+28;//参考*按比例求高
		box.style.height=par.height+sh+'px';
		win.style.cssText='width:'+winW+'px;height:'+sh+'px;';
		thus.style.width=sw*n+'px';
		for(var i=0;i<n;i++){
			thu[i].style.cssText='width:'+(sw-14)+'px;height:'+(sh-28)+'px';//减去总padding
			F.$$('span',thu[i])[0].style.cssText='width:'+(sw-14)+'px;height:'+(sh-28)+'px';//参考*
			txt[i].style.bottom=sh+'px';
		}
		pre.style.cssText=nex.style.cssText='height:'+(sh-28+6)+'px;line-height:'+(sh-28+6)+'px;';
		//PLAY
		eval(F.switchMF(function(){
			pic[index].style.display='none';
			txt[index].style.display='none';
			thu[index].className = '';
		},function(){
			F.fadeIn(pic[next]);
			txt[next].style.display='block';
			thu[next].className = 'current';
			eval(F.scroll('thus','"left"','sw',4));
		}));
		eval(F.bind('thu','"click"'));//让其只支持click点击
		eval(F.turn('pre','nex'));
	}
});